using System;
using System.Collections.Generic;
using System.IO;
using Commons;
using System.Linq;
using System.Text;
using KleanTrak.Model;
using System.Security.AccessControl;

namespace KleanTrak.Core
{
    public abstract class WPBase
    {
        public bool TestMode { get; set; } = false;
        protected static readonly log4net.ILog Logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        protected string OldDir { get { return (FolderOrFileName == null || FolderOrFileName.Length == 0) ? "" : FolderOrFileName.ComposePathToDir("OLD"); } }
        protected string BadDir { get { return (FolderOrFileName == null || FolderOrFileName.Length == 0) ? "" : FolderOrFileName.ComposePathToDir("BAD"); } }
        protected string UID { set; get; }
        public string FolderOrFileName { set; get; }
        protected string User { set; get; }
        protected string Password { set; get; }

        abstract public List<WasherCycle> GetCycles(Washer washer, DateTime lastDate);
        public bool FileSystemWasher { get; set; } = true;
        public bool IsPreWasher { get; set; } = false;
        public virtual bool Start() => true;
        public virtual bool Stop() => true;

        public static WPBase Get(Washer washer, string forced_filename = "")
        {
            WPBase ret = null;
            switch (washer.Type)
            {
                case WasherStorageTypes.Washer_IMS7:
                    ret = new WPIMS7();
                    break;
                case WasherStorageTypes.Washer_ISAWD:
                    ret = new WPISAWD();
                    break;
                case WasherStorageTypes.Washer_Cantel_Medivators_XXX:
                    ret = new WPCantelMedivatorsXXX();
                    break;
                case WasherStorageTypes.Washer_Steelco:
                    ret = new WPSteelco(false);
                    break;
                case WasherStorageTypes.Washer_Mirth:
                    ret = new WPMirth();
                    break;
                case WasherStorageTypes.Washer_Cantel_RapidAir:
                    ret = new WPCantel();
                    break;
                case WasherStorageTypes.Storage_Cantel_EDC:
                    ret = new SPCantelEdc();
                    break;
                case WasherStorageTypes.Washer_Steelco_ManualEnd:
                    ret = new WPSteelco(true);
                    break;
                case WasherStorageTypes.Washer_Cantel_Medivators_Serial:
                    ret = new WPCantelMedivatorsSerial { FileSystemWasher = false };
                    break;
                case WasherStorageTypes.Washer_ICT_DbConnect:
                    ret = new WPICT { FileSystemWasher = false };
                    break;
                case WasherStorageTypes.PreWasher_Cantel_MDG:
                    ret = new PWPCantelMDG(washer.SerialNumber)
                    {
                        FileSystemWasher = false,
                        IsPreWasher = true
                    };
                    break;
                case WasherStorageTypes.Washer_Cantel_AdvantagePassThrough_PV_3_0_0_16:
                    ret = new WPCantelAdvPassThroughV30016();
                    break;
                case WasherStorageTypes.Storage_Cantel_EndoDry:
                    ret = new SPCantelEndoDry();
                    break;
                case WasherStorageTypes.Washer_Cantel_AdvantagePassThrough_PV_3_0_0_16_Old:
                    ret = new WPCantelAdvPassThroughV30016Old();
                    break;
                case WasherStorageTypes.PreWasher_Steelco_EPW100:
                    ret = new PWPSteelcoEPW100();
                    break;
                default:
                    new ArgumentException("Storage/Washer " + washer.Type.ToString() + " not definied!");
                    break;
            }
            if (ret != null)
            {
                ret.UID = washer.ID + " - " + washer.Code;
                ret.FolderOrFileName = (forced_filename.Length == 0) ? washer.FolderOrFileName : forced_filename;
                ret.User = washer.User;
                ret.Password = washer.Password;
            }
            if (ret.FileSystemWasher && ret.OldDir.Length > 0 && ret.BadDir.Length > 0)
            {
                if (!Directory.Exists(ret.OldDir))
                    Directory.CreateDirectory(ret.OldDir);
                if (!Directory.Exists(ret.BadDir))
                    Directory.CreateDirectory(ret.BadDir);
            }
            return ret;
        }

        protected static WasherCycle GetStartFakeCycle(WasherCycle cycle)
        {
            var startCycle = cycle.Clone();
            startCycle.DesiredDestinationState = StateTransactions.GetStateId(FixedStates.Start_wash);
            startCycle.Failed = false;
            startCycle.Completed = false;
            startCycle.EndTimestamp = DateTime.MinValue;
            startCycle.OperatorEndExternalID = "";
            startCycle.OperatorEndID = Operators.GetUnknownOperator().ID;
            return startCycle;
        }

        protected bool CheckWasherSerialNumber(Washer washer)
        {
            try
            {
                if (washer.SerialNumber == null || washer.SerialNumber.Length == 0)
                {
                    Logger.Warn($"washer.ID {washer.ID} " +
                        $"washer.Code {washer.Code} " +
                        $"washer.Description {washer.Description} " +
                        $"--> Serial is empty!!");
                    return false;
                }
                return true;
            }
            catch (Exception e)
            {
                Logger.Error(washer, e);
                return false;
            }
        }

        public bool MoveToOldDir(string sourcefile)
        {
            try
            {
                if (FolderOrFileName.Length == 0 || !FileSystemWasher)
                    return false;
                var destfilepath = OldDir.ComposePathToFile(Path.GetFileName(sourcefile));
                if (File.Exists(destfilepath))
                    File.Delete(destfilepath);
                Logger.Info($"moving {sourcefile}{Environment.NewLine}to {destfilepath}");
                File.Move(sourcefile, destfilepath);
                return true;
            }
            catch (Exception e)
            {
                Logger.Error($"sourcefile: {sourcefile}", e);
                return false;
            }
        }

        public bool MoveToBadDir(string sourcefile)
        {
            try
            {
                if (FolderOrFileName.Length == 0 || !FileSystemWasher)
                    return false;
                var destfilepath = BadDir.ComposePathToFile(Path.GetFileName(sourcefile));
                if (File.Exists(destfilepath))
                    File.Delete(destfilepath);
                File.Move(sourcefile, destfilepath);
                return true;
            }
            catch (Exception e)
            {
                Logger.Error($"sourcefile: {sourcefile}", e);
                return false;
            }
        }

        protected int TranscodeWasher(string serial)
        {
            if (TestMode)
                return 999;
            Washer washer = Washers.FromMatr(serial);
            if (washer != null)
                return washer.ID;
            return 0;
        }

        protected int TranscodeDevice(string tag_or_serial_or_barcode)
        {
            Logger.Info($"transcode device: {tag_or_serial_or_barcode}");
            if (TestMode)
                return 999;
            Device device = Devices.FromMat(tag_or_serial_or_barcode);
            if (device != null)
                return device.ID;

            device = Devices.FromBarcode(tag_or_serial_or_barcode);
            if (device != null)
                return device.ID;

            device = Devices.FromSerial(tag_or_serial_or_barcode);
            if (device != null)
                return device.ID;
            return 0;
        }

        protected int TranscodeOperator(string serial_or_barcode)
        {
            if (TestMode)
                return 999;
            serial_or_barcode.Trim();
            Operator opS = Operators.FromSerial(serial_or_barcode);
            if (opS != null)
                return opS.ID;

            Operator opB = Operators.FromBarcode(serial_or_barcode);
            if (opB != null)
                return opB.ID;
            Operator unknown = Operators.GetUnknownOperator();
            if (unknown != null)
                return unknown.ID;
            return 0;
        }

        protected bool IsFileAlreadyProcessed(string filename)
        {
            if (TestMode)
                return false;
            bool bRet = false;
            string query = "SELECT COUNT(*) AS CONT FROM SterilizzatriciParsing WHERE Filename = '" + filename + "'";
            try
            {
                DbConnection conn = new DbConnection();
                DbRecordset dataset = conn.ExecuteReader(query);
                if (dataset.Count == 1)
                    bRet = dataset[0].GetInt("CONT") > 0;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }

            return bRet;
        }
    }
}
