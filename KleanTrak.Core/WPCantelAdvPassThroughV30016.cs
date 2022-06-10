using KleanTrak.Model;
using Commons;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace KleanTrak.Core
{
    // ATTENZIONE I NOMI DEI FILE SONO PROGRESSIVI E RICOMINCIANO QUANDO LA DIRECTORY
    // CONDIVISIA VIENE SVUOTATA, SE SI SOSPENDE LA SCRITTURA E IL SERVIZIO SVUOTA LA
    // DIRECTORY (TUTTI FILE CON PIU' DI UN GIORNO) AL SUCCESSIVO AVVIO DELLE LAVATRICI
    // LA NUMERAZIONE DEI FILES RIPRENDE DA ZERO, CREANDO PERTANTO NOMI FILE DUPLICATI
    // NELLA TABELLA STERILIZZATRICIPARSING. 
    //
    // IMPORTANTE: LO SCONTRINO DEVE ESSERE DEPOSITATO SOLO A FINE CICLO!!!!
    public class WPCantelAdvPassThroughV30016 : WPBase
    {
        private const string CYCLE_RESULT_BUSY = "0";
        private const string CYCLE_RESULT_OK = "100";
        private const string CYCLE_RESULT_OK_FAILURES = "101";
        private const string CYCLE_RESULT_OK_SAFETYDISABLED = "102";
        private const string CYCLE_RESULT_FAILED_OPERATORCAUSE = "120";
        private const string CYCLE_RESULT_FAILED_FAILUREOCCURRED = "121";
        private const string CYCLE_RESULT_FAILED_INTERRUPTEDCYCLE = "130";
        private const string CYCLE_RESULT_FAILED_LIOCAUSE = "140";
        private const string CYCLE_RESULT_FAILED_SAFETYCONTROLLERCAUSE = "150";
        private char[] LINE_FIELD_SEPARATOR = new char[] { '|' }; // line field separator char
        private const string HEADER_INS = "INS"; // instrument disinfection
        private const string HEADER_FAI = "FAI"; // failures
        private const string HEADER_SOA = "SOA"; // soap tank change
        private const string HEADER_ALC = "ALC"; // alchohol tank change
        private const string HEADER_DIS = "DIS"; // reusable disinfection tank change
        private const string HEADER_CMA = "CMA"; // component A tank change
        private const string HEADER_CMB = "CMB"; // component B tank change
                                                 // FFU
        private const string HEADER_MEA = "MEA"; // mesasurements
        private const string HEADER_MAC = "MAC"; // machine disinfection information
                                                 // il conteggio dei fields di ciascuna linea contiene l'header
        private const int LINE_INS_FIELDS_NUM = 22;
        private const int LINE_FAI_FIELDS_NUM = 7;
        private const int LINE_MANTAINENCE_FIELDS_NUM = 8;
        private const int LINE_MEA_FIELDS_NUM = 7;
        private const int LINE_MAC_FIELDS_NUM = 14;

        public WPCantelAdvPassThroughV30016()
        {
            FileSystemWasher = true;
            IsPreWasher = false;
        }

        public override List<WasherCycle> GetCycles(Washer washer, DateTime lastDate)
        {
            var retcycles = new List<WasherCycle>();
            try
            {
                if (!CheckWasherSerialNumber(washer))
                    return retcycles;
                if (!Directory.Exists(FolderOrFileName))
                    throw new ApplicationException($"Directory {FolderOrFileName} doesn't exists");
                var dirinfo = new DirectoryInfo(FolderOrFileName);
                var fileToDelete = new List<string>();
                foreach (var file in dirinfo.GetFiles("*.adm", SearchOption.TopDirectoryOnly))
                {
                    var cycle = ParseFile(file);
                    // si aggiunge un ciclo fake per inizio sterilizzazione
                    WasherCycle startCycle = GetStartFakeCycle(cycle);

                    // nel caso in cui il ciclo sia null (scontrini di manutenzione macchina) oppure nel caso
                    // in cui lo stato sia diverso da inizio sterilizzazione, posso eliminare il file, infatti
                    // queste lavaendoscopi aggiornano sempre lo stesso file per passare le informazioni del ciclo
                    // prima di eliminare devo avere un ciclo completo.
                    if (cycle == null || cycle.Completed || cycle.Failed)
                        MoveToOldDir(file.FullName);
                    if (cycle != null)
                    {
                        retcycles.Add(startCycle);
                        retcycles.Add(cycle);
                    }
                    // StateTransaction memorizes filename and data as already processed
                }
            }
            catch (Exception e)
            {
                Logger.Error(e);
            }
            return retcycles;
        }

        private WasherCycle ParseFile(FileInfo file)
        {
            string[] filelines = null;
            try
            {
                var newcycle = CreateCycle(file, out filelines);
                if (newcycle == null)
                    throw new ApplicationException($"new cycle creation failed content: {filelines}");
                for (int i = 0; i < filelines.Count(); i++)
                {
                    var lineResult = ParseLine(filelines[i], newcycle);
                    switch (lineResult)
                    {
                        case ParseLineResult.Error:
                            Logger.Error($"error parsing line {i} of file {file.FullName} ");
                            continue;
                        case ParseLineResult.SkipLine:
                            continue;
                        // se trovo la linea di ins ritorno il ciclo
                        case ParseLineResult.Ok:
                        default:
                            return newcycle;
                    }
                }
                return null;
            }
            catch (Exception e)
            {
                Logger.Error($"filename {file.FullName}" +
                    $"{Environment.NewLine}" +
                    $"filecontent {filelines.ToCsvString<string>()}, ", e);
                return null;
            }
        }

        private WasherCycle CreateCycle(FileInfo file, out string[] filelines)
        {
            filelines = null;
            try
            {
                filelines = File.ReadAllLines(file.FullName);
                var newcycle = new WasherCycle();
                newcycle.FileDatetime = file.CreationTime;
                newcycle.Filename = file.FullName;
                foreach (var line in filelines)
                    newcycle.FileContent += line + Environment.NewLine;
                return newcycle;
            }
            catch (Exception e)
            {
                Logger.Error($"filename {file.FullName}", e);
                return null;
            }
        }

        private enum ParseLineResult
        {
            Ok,
            SkipLine,
            Error
        }
        private ParseLineResult ParseLine(string line, WasherCycle newcycle)
        {
            try
            {
                var header = GetLineHeader(line);
                switch (header)
                {
                    case HEADER_ALC:
                        return ParseLineResult.SkipLine; // ParseAlcoholLine(line, newcycle);
                    case HEADER_CMA:
                        return ParseLineResult.SkipLine; // ParseComponentALine(line, newcycle);
                    case HEADER_CMB:
                        return ParseLineResult.SkipLine; // ParseComponentBLine(line, newcycle);
                    case HEADER_DIS:
                        return ParseLineResult.SkipLine; // ParseDisinfectantChangeLine(line, newcycle);
                    case HEADER_FAI:
                        return ParseLineResult.SkipLine; // (ParseFailureLine(line, newcycle)) ? ParseLineResult.Ok : ParseLineResult.Erorr;
                    case HEADER_INS:
                        return (ParseInstrumentDisinfectionLine(line, newcycle)) ? ParseLineResult.Ok : ParseLineResult.Error;
                    case HEADER_SOA:
                        return ParseLineResult.SkipLine; // ParseSoaLine(line, newcycle);
                    case HEADER_MAC:
                    case HEADER_MEA:
                        return ParseLineResult.SkipLine; // ;
                    default:
                        throw new ApplicationException($"header {header} not recognized");
                }
            }
            catch (Exception e)
            {
                Logger.Error(line, e);
                return ParseLineResult.Error;
            }
        }

        private bool ParseAlcoholLine(string line, WasherCycle newcycle) => ParseManteinanceLine(line, "Alcohol", newcycle);

        private bool ParseSoaLine(string line, WasherCycle newcycle) => ParseManteinanceLine(line, "Soap", newcycle);

        private bool ParseDisinfectantChangeLine(string line, WasherCycle newcycle) => ParseManteinanceLine(line, "Disinfectant", newcycle);

        private bool ParseComponentALine(string line, WasherCycle newcycle) => ParseManteinanceLine(line, "Component A", newcycle);

        private bool ParseComponentBLine(string line, WasherCycle newcycle) => ParseManteinanceLine(line, "Component B", newcycle);

        private DateTime ParseDateTime(string date, string time)
        {
            var ds = (date.IndexOf("/") >= 0) ? "/" : "-";
            return DateTime.ParseExact($"{date.Trim()} {time.Trim()}",
                                $"dd{ds}MM{ds}yyyy HH:mm:ss",
                                CultureInfo.InvariantCulture);
        }

        private bool ParseManteinanceLine(string line, string manteinancetype, WasherCycle newcycle)
        {
            try
            {
                var fields = line.Split(LINE_FIELD_SEPARATOR, StringSplitOptions.None);
                if (fields.Length != LINE_MANTAINENCE_FIELDS_NUM)
                    throw new ApplicationException($"wrong fai line length. Actual length {fields.Length} expected {LINE_MANTAINENCE_FIELDS_NUM}");
                var datetime = ParseDateTime(fields[4], fields[5]);
                string side = GetMachineSide(fields[2]);
                newcycle.AdditionalInfoList.Add(new WasherCycleInfo
                {
                    Date = datetime,
                    isAlarm = false,
                    Description = $"{manteinancetype} change on machine serial number",
                    Value = GetFieldString(fields[1])
                });
                newcycle.AdditionalInfoList.Add(new WasherCycleInfo
                {
                    Date = datetime,
                    isAlarm = false,
                    Description = $"{manteinancetype} change on side",
                    Value = side
                });
                newcycle.AdditionalInfoList.Add(new WasherCycleInfo
                {
                    Date = datetime,
                    isAlarm = false,
                    Description = $"{manteinancetype} change on tank code",
                    Value = GetFieldString(fields[3])
                });
                newcycle.AdditionalInfoList.Add(new WasherCycleInfo
                {
                    Date = datetime,
                    isAlarm = false,
                    Description = $"{manteinancetype} changed by operator id",
                    Value = GetFieldString(fields[6])
                });
                newcycle.AdditionalInfoList.Add(new WasherCycleInfo
                {
                    Date = datetime,
                    isAlarm = false,
                    Description = $"{manteinancetype} changed by operator name",
                    Value = GetFieldString(fields[7])
                });
                return true;
            }
            catch (Exception e)
            {
                Logger.Error($"manteinance type {manteinancetype} " +
                    $"{Environment.NewLine}" +
                    $"line {line}", e);
                return false;
            }
        }

        private string GetMachineSide(string field)
        {
            try
            {
                switch (field.Trim())
                {
                    case "0":
                        return "both sides";
                    case "1":
                        return "both sides";
                    case "2":
                        return "both sides";
                    default:
                        throw new ApplicationException("unrecognized side");
                }
            }
            catch (Exception e)
            {
                Logger.Error(field, e);
                return "Side Error";
            }
        }

        private bool ParseFailureLine(string line, WasherCycle newcycle)
        {
            try
            {
                var fields = line.Split(LINE_FIELD_SEPARATOR, StringSplitOptions.None);
                if (fields.Length != LINE_FAI_FIELDS_NUM)
                    throw new ApplicationException($"wrong fai line length. Actual length {fields.Length} expected {LINE_FAI_FIELDS_NUM}");
                var datetime = ParseDateTime(fields[2], fields[3]);
                newcycle.AdditionalInfoList.Add(new WasherCycleInfo
                {
                    Date = datetime,
                    isAlarm = true,
                    Description = "Failure id",
                    Value = GetFieldString(fields[1])
                });
                newcycle.AdditionalInfoList.Add(new WasherCycleInfo
                {
                    Date = datetime,
                    isAlarm = true,
                    Description = "Failure Description",
                    Value = GetFieldString(fields[4])
                });
                newcycle.AdditionalInfoList.Add(new WasherCycleInfo
                {
                    Date = datetime,
                    isAlarm = true,
                    Description = "Phase reference",
                    Value = GetFieldString(fields[5])
                });
                newcycle.AdditionalInfoList.Add(new WasherCycleInfo
                {
                    Date = datetime,
                    isAlarm = true,
                    Description = "Step reference",
                    Value = GetFieldString(fields[5])
                });
                return true;
            }
            catch (Exception e)
            {
                Logger.Error(line, e);
                return false;
            }
        }

        private bool ParseInstrumentDisinfectionLine(string line, WasherCycle newcycle)
        {
            try
            {
                // importante che ci siano anche eventuali stringhe vuote
                var fields = line.Split(LINE_FIELD_SEPARATOR, StringSplitOptions.None);
                if (fields.Length != LINE_INS_FIELDS_NUM)
                    throw new ApplicationException($"wrong ins line length. Actual length {fields.Length} expected {LINE_INS_FIELDS_NUM}");
                string externalid = GetExternalId(fields);
                // check date separator string
                var completedatetime = ParseDateTime(fields[7], fields[8]);
                newcycle.DeviceExternalID = externalid;
                var deviceid = TranscodeDevice(externalid);
                if (deviceid == 0)
                    throw new ApplicationException($"device not found for external id {fields[1]}");
                newcycle.DeviceID = deviceid;
                newcycle.Failed = (fields[2] == "0");
                newcycle.Completed = true; // ATTENZIONE !! BISOGNA CHE LO SCONTRINO VENGA DEPOSITATO SOLO ALLA FINE.
                newcycle.DesiredDestinationState = StateTransactions.GetStateId(FixedStates.End_wash);
                // SetCycleResultInfo(fields, completedatetime, newcycle); // si trascura il campo 3 perchè in contrasto con fields[2]
                var washerid = 0;
                var washerextid = fields[4];
                washerid = TranscodeWasher(washerextid);
                if (washerid == 0)
                    throw new ApplicationException($"washer matricola not found for external id {washerextid}");
                newcycle.WasherExternalID = washerextid;
                newcycle.WasherID = washerid;
                GetSideIdentification(fields, completedatetime, newcycle);
                newcycle.CycleCount = fields[6];
                newcycle.EndTimestamp = completedatetime;
                newcycle.StartTimestamp = GetStartDateTime(completedatetime, fields[9]);
                SetTotalDuration(fields, completedatetime, newcycle);
                SetProgramParameter(fields, completedatetime, newcycle);
                SetOperators(fields, completedatetime, newcycle);
                SetOtherInfos(fields, completedatetime, newcycle);
                return true;
            }
            catch (Exception e)
            {
                Logger.Error(line, e);
                return false;
            }
        }

        protected virtual string GetExternalId(string[] fields)
        {
            if (fields.Length < 11)
                throw new ApplicationException("externalid not found");
            var externalid = fields[10];
            Regex regex = new Regex(@"\^[\w\d]+\^");
            var matches = regex.Matches(externalid);
            if (matches.Count == 0)
                throw new ApplicationException("externalid not found");
            return matches[0]
                .Value
                .Replace("^", "");
        }

        private DateTime GetStartDateTime(DateTime start, string duration)
        {
            try
            {
                var durationSpan = TimeSpan.Parse(duration);
                return start.Add(-durationSpan);
            }
            catch (Exception e)
            {
                Logger.Error($"start {start} duration {duration}", e);
                return DateTime.MinValue;
            }
        }

        private void SetOperators(string[] fields,
            DateTime datetime,
            WasherCycle newcylcle)
        {
            try
            {
                string oploadingid = fields[11];
                string oploadingname = fields[12];
                string opunloadingid = fields[13];
                string opunloadingname = fields[14];
                newcylcle.OperatorStartExternalID = oploadingid;
                newcylcle.OperatorEndExternalID = opunloadingid;
                var opstartid = TranscodeOperator(oploadingid);
                if (opstartid == 0)
                {
                    Logger.Error($"unknown external loading operator id {oploadingid}");
                    opstartid = TranscodeOperator("UNKNOWN");
                }
                var opendid = TranscodeOperator(opunloadingid);
                if (opendid == 0)
                {
                    Logger.Error($"unknown external unloading operator id {opunloadingid}");
                    opendid = TranscodeOperator("UNKNOWN");
                }
                newcylcle.OperatorStartID = opstartid;
                newcylcle.OperatorEndID = opendid;
                newcylcle.AdditionalInfoList.Add(new WasherCycleInfo
                {
                    Date = datetime,
                    isAlarm = false,
                    Description = $"loading operator name {oploadingname}",
                    Value = oploadingname
                });
                newcylcle.AdditionalInfoList.Add(new WasherCycleInfo
                {
                    Date = datetime,
                    isAlarm = false,
                    Description = $"unloading operator name {opunloadingname}",
                    Value = oploadingname
                });
            }
            catch (Exception e)
            {
                Logger.Error(e);
                // nessun rilancio, sono solo info ausiliarie
            }
        }

        private void SetProgramParameter(string[] fields, DateTime datetime, WasherCycle newcycle)
        {
            try
            {

                newcycle.AdditionalInfoList.Add(new WasherCycleInfo
                {
                    Date = datetime,
                    isAlarm = false,
                    Description = $"program parameter set: {fields[10].Trim()}",
                    Value = fields[10]
                });
            }
            catch (Exception e)
            {
                Logger.Error(e);
                // nessun rilancio, sono solo info ausiliarie
            }
        }

        private void SetTotalDuration(string[] fields, DateTime datetime, WasherCycle newcycle)
        {
            try
            {
                newcycle.AdditionalInfoList.Add(new WasherCycleInfo
                {
                    Date = datetime,
                    isAlarm = false,
                    Description = $"cycle total duration: {fields[9].Trim()}",
                    Value = fields[9]
                });
            }
            catch (Exception e)
            {
                Logger.Error(e);
                // nessun rilancio, sono solo info ausiliarie
            }
        }

        private void GetSideIdentification(string[] fields, DateTime datetime, WasherCycle newcycle)
        {
            try
            {
                newcycle.AdditionalInfoList.Add(new WasherCycleInfo
                {
                    Date = datetime,
                    isAlarm = false,
                    Description = "Machine Side",
                    Value = $"machine side: {((fields[5].Trim() == "1") ? "left side" : "right side")}"
                });
            }
            catch (Exception e)
            {
                Logger.Error(e);
                // nessun rilancio, sono solo info ausiliarie
            }
        }

        private void SetCycleResultInfo(string[] fields, DateTime datetime, WasherCycle newcycle)
        {
            try
            {
                var infos = new WasherCycleInfo
                {
                    Date = datetime,
                    Value = fields[3]
                };
                switch (fields[3].Trim())
                {
                    case CYCLE_RESULT_BUSY:
                        infos.Description = "Washer buisy";
                        newcycle.Completed = false;
                        newcycle.Failed = false;
                        newcycle.DesiredDestinationState = StateTransactions.GetStateId(FixedStates.Start_wash);
                        break;
                    case CYCLE_RESULT_OK:
                        infos.Description = "Ok";
                        newcycle.Completed = true;
                        newcycle.Failed = false;
                        newcycle.DesiredDestinationState = StateTransactions.GetStateId(FixedStates.End_wash);
                        break;
                    case CYCLE_RESULT_OK_FAILURES:
                        infos.Description = "Cycle ok, but failures occurred";
                        newcycle.Completed = true;
                        newcycle.Failed = false;
                        newcycle.DesiredDestinationState = StateTransactions.GetStateId(FixedStates.End_wash);
                        break;
                    case CYCLE_RESULT_OK_SAFETYDISABLED:
                        infos.Description = "Cycle ok, but safety controller disabled";
                        newcycle.Completed = true;
                        newcycle.Failed = false;
                        newcycle.DesiredDestinationState = StateTransactions.GetStateId(FixedStates.End_wash);
                        break;
                    case CYCLE_RESULT_FAILED_OPERATORCAUSE:
                        infos.Description = "Cycle failed, operatore cause";
                        infos.isAlarm = true;
                        newcycle.Completed = true;
                        newcycle.Failed = true;
                        newcycle.DesiredDestinationState = StateTransactions.GetStateId(FixedStates.Start_cycle);
                        break;
                    case CYCLE_RESULT_FAILED_FAILUREOCCURRED:
                        infos.Description = "Cycle failed, failures occured";
                        infos.isAlarm = true;
                        newcycle.Completed = true;
                        newcycle.Failed = true;
                        newcycle.DesiredDestinationState = StateTransactions.GetStateId(FixedStates.Start_cycle);
                        break;
                    case CYCLE_RESULT_FAILED_INTERRUPTEDCYCLE:
                        infos.Description = "Cycle failed, interrupted cycle";
                        infos.isAlarm = true;
                        newcycle.Completed = true;
                        newcycle.Failed = true;
                        newcycle.DesiredDestinationState = StateTransactions.GetStateId(FixedStates.Start_cycle);
                        break;
                    case CYCLE_RESULT_FAILED_LIOCAUSE:
                        infos.Description = "Cycle failed, Lio cause";
                        infos.isAlarm = true;
                        newcycle.Completed = true;
                        newcycle.Failed = true;
                        newcycle.DesiredDestinationState = StateTransactions.GetStateId(FixedStates.Start_cycle);
                        break;
                    case CYCLE_RESULT_FAILED_SAFETYCONTROLLERCAUSE:
                        infos.Description = "Cycle failed, safety controller cause";
                        infos.isAlarm = true;
                        newcycle.Completed = true;
                        newcycle.Failed = true;
                        newcycle.DesiredDestinationState = StateTransactions.GetStateId(FixedStates.Start_cycle);
                        break;
                    default:
                        Logger.Error(new ApplicationException("ERROR: unrecognized cycleresult"));
                        infos.Description = "ERROR: unrecognized cycleresult";
                        infos.isAlarm = true;
                        newcycle.Completed = true;
                        newcycle.Failed = true;
                        newcycle.DesiredDestinationState = StateTransactions.GetStateId(FixedStates.Start_cycle);
                        break;
                }
                newcycle.AdditionalInfoList.Add(infos);
            }
            catch (Exception e)
            {
                Logger.Error(e);
                // nessun rilancio, sono solo info ausiliarie
            }
        }

        private string GetLineHeader(string line)
        {
            try
            {
                var chunks = line.Split(LINE_FIELD_SEPARATOR, StringSplitOptions.RemoveEmptyEntries);
                if (chunks.Length > 0)
                    return chunks[0];
                throw new ApplicationException($"line header not found for line {line}");
            }
            catch (Exception e)
            {
                Logger.Error(line, e);
                return "ERROR";
            }
        }

        private void SetOtherInfos(string[] fields, DateTime datetime, WasherCycle newcycle)
        {
            try
            {
                newcycle.AdditionalInfoList.Add(new WasherCycleInfo
                {
                    Date = datetime,
                    isAlarm = false,
                    Description = "Patient",
                    Value = GetFieldString(fields[15])
                });
                newcycle.AdditionalInfoList.Add(new WasherCycleInfo
                {
                    Date = datetime,
                    isAlarm = false,
                    Description = "Physician id",
                    Value = GetFieldString(fields[16])
                });
                newcycle.AdditionalInfoList.Add(new WasherCycleInfo
                {
                    Date = datetime,
                    isAlarm = false,
                    Description = "Physician name",
                    Value = GetFieldString(fields[17])
                });
                newcycle.AdditionalInfoList.Add(new WasherCycleInfo
                {
                    Date = datetime,
                    isAlarm = false,
                    Description = "Assistant id",
                    Value = GetFieldString(fields[18])
                });
                newcycle.AdditionalInfoList.Add(new WasherCycleInfo
                {
                    Date = datetime,
                    isAlarm = false,
                    Description = "Assistant name",
                    Value = GetFieldString(fields[19])
                });
                newcycle.AdditionalInfoList.Add(new WasherCycleInfo
                {
                    Date = datetime,
                    isAlarm = false,
                    Description = "Instrument location",
                    Value = GetFieldString(fields[20])
                });
                newcycle.AdditionalInfoList.Add(new WasherCycleInfo
                {
                    Date = datetime,
                    isAlarm = false,
                    Description = "Machine location",
                    Value = GetFieldString(fields[21])
                });
            }
            catch (Exception e)
            {
                Logger.Error(e);
            }
        }

        private string GetFieldString(string value, string empty = "n.p.") => ((value.Trim().Length > 0) ? value.Trim() : empty);
    }
}
