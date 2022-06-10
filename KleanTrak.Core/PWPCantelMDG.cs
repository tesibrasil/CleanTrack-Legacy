using KleanTrak.Model;
using log4net.Repository.Hierarchy;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KleanTrak.Core
{
    public class PWPCantelMDG : WPBase
    {
        private const int SEND_CMD_LEN = 32;
        private const int RESP_CMD_LEN = 256;
        private const int HEAD_SEGMENT_LEN = 2;
        private const int MSG_LEN_SEGMENT_LEN = 4;
        private const int ID_MAC_SEGMENT_LEN = 4;
        private const int EXP_SEGMENT_LEN = 4;
        private const int REQ_MSG_SEGMENT_LEN = 14;
        private const int RESP_MSG_SEGMENT_LEN = 238;
        private const int CHK_SEGMENT_LEN = 4;
        public string Ipaddress { get; private set; }
        public int Portnumber { get; set; } = -1;
        private string idmac = "";
        private int washerid = -1;
        private DateTime startcycledatetime = DateTime.MinValue;
        private const string COM_ERROR_RESPONSE = "ERROR";
        private const string DEL_MEM_OK = "OK";
        private const string DEL_MEM_ERROR = "ERR";
        private const string MODE_NORMAL_WASH = "0";
        private const string MODE_SANIFICATION = "1";
        private const string MODE_PROTEIN_TEST = "2";
        private const string TASK_READY_ITA = "PRONTO";
        private const string TASK_READY_ENG = "STAND-BY";
        private const string TASK_KEEP_TEST_ITA = "TEST TENUTA";
        private const string TASK_KEEP_TEST_ENG = "KEEP TEST";
        private const string TASK_CHEMICAL_ITA = "CHIMICO";
        private const string TASK_CHEMICAL_ENG = "CHEMICAL";
        private const string TASK_WASHING_ITA = "LAVAGGIO";
        private const string TASK_WASHING_ENG = "WASHING";
        private const string TASK_CLEANING_ITA = "PULIZIA"; // lavaggio della macchina
        private const string TASK_CLEANING_ENG = "CLEANING"; // lavaggio della macchina
        private const string TASK_VALID_CYCLE_ITA = "VALID.CIC.";
        private const string TASK_VALID_CYCLE_ENG = "VALID.CYC.";
        private const string TASK_SINK_LOAD_ITA = "CARICO";
        private const string TASK_SINK_LOAD_ENG = "SINK LOAD";
        private const string TASK_SINK_DRAIN_ITA = "SCARICO";
        private const string TASK_SINK_DRAIN_ENG = "SINK DRAIN";
        private const string TASK_PURGING_ITA = "SVUOTAM.";
        private const string TASK_PURGING_ENG = "PURGING";
        private char[] colonsplitchar = new char[] { ':' };
        private char[] datesplitchar = new char[] { '/' };
        private char[] headcharsbuff = new char[] { '#', '^' };
        private char[] expbuff = new char[EXP_SEGMENT_LEN] { '0', '0', '0', '0' };
        private char[] reqmsglenbuff = new char[MSG_LEN_SEGMENT_LEN] { '0', '0', '3', '0' };
        private char[] respmsglenbuff = new char[MSG_LEN_SEGMENT_LEN] { '0', '2', '5', '4' };
        private char[] idmacbuff = null; // nel costruttore
        private ConcurrentQueue<WasherCycle> memorycycles = new ConcurrentQueue<WasherCycle>();
        private bool _poll = false;
        public bool IsRunning { get { return _poll; } }
        public delegate void TraceCommunicationCb(string data);
        public event TraceCommunicationCb CommandSent;
        public event TraceCommunicationCb CommandReceived;
        public event TraceCommunicationCb ErrorsThrown;
        private int _maxErrorSeconds = 300;
        public PWPCantelMDG(string serialnumber)
        {
            SetNewIdMac(serialnumber);
            try
            {
                DbConnection conn = new DbConnection();
                DbRecordset dataset = conn.ExecuteReader("SELECT VALORE FROM CONFIGURAZIONE WHERE CHIAVE = 'Max_Time_Error_Seconds'");
                if (dataset.Count == 0)
                {
                    conn.ExecuteNonQuery("INSERT INTO CONFIGURAZIONE (CHIAVE, VALORE) VALUES ('Max_Time_Error_Seconds','" + _maxErrorSeconds.ToString() + "')");
                }
                else
                {
                    int? val = dataset[0].GetInt("VALORE");
                    if (val != null)
                        _maxErrorSeconds = val.Value;
                }
            }
            catch (Exception exc)
            {
                Log($"Error reading maximum datetime error from DB, using default value of {_maxErrorSeconds} seconds", exc);
            }
        }
        public void SetNewIdMac(string newid)
        {
            try
            {
                // controllo che in serialnumber ci sia il progressivo
                idmac = int.Parse(newid).ToString().PadLeft(4, '0');
                idmacbuff = idmac.ToCharArray();
            }
            catch (Exception e)
            {
                Log($"serial number doesn't correspond to a ER214 MAC_ID newid: {newid}", e);
                throw;
            }
        }
        public void SetFolderOrFilename(string foldername) => this.FolderOrFileName = foldername;
        public override bool Start()
        {
            try
            {
                if (!SetConnectionsParameters())
                    return false;
                _poll = true;
                Task.Run(() => Polling());
                return true;
            }
            catch (Exception e)
            {
                Log(e);
                return false;
            }
        }
        public bool SetConnectionsParameters()
        {
            try
            {
                if (FolderOrFileName.Length == 0)
                    throw new ApplicationException("missing ip:port pair in folderorfilename (PERCORSO FIELD)");
                var ipandport = FolderOrFileName.Split(colonsplitchar, StringSplitOptions.RemoveEmptyEntries);
                Ipaddress = ipandport[0];
                Portnumber = int.Parse(ipandport[1]);
                washerid = TranscodeWasher(idmac);
                // GetWasherInfo();
                return true;
            }
            catch (Exception e)
            {
                Logger.Error(e);
                return false;
            }
        }
        public override bool Stop()
        {
            try
            {
                _poll = false;
                return true;
            }
            catch (Exception e)
            {
                Log(e);
                return false;
            }
        }
        public override List<WasherCycle> GetCycles(Washer washer, DateTime lastDate)
        {
            var cycles = new List<WasherCycle>();
            try
            {
                while (memorycycles.TryDequeue(out WasherCycle newcycle))
                    cycles.Add(newcycle);
                return cycles;
            }
            catch (Exception ex)
            {
                Log($"washer: {washer}{Environment.NewLine} " +
                    $"Exception: {ex}");
                return cycles;
            }
        }
        private void GetWasherInfo()
        {
            try
            {
                var response = SendCommand("INFO");
                if (IsError(response, out string errmsg))
                    throw new ApplicationException($"INFO command returns errors {errmsg}");
                idmac = GetParameterValue("IDMAC", response);
                washerid = TranscodeWasher(idmac);
                if (washerid == 0)
                    throw new ApplicationException($"washerid not found for washerserialext: {idmac}");
            }
            catch (Exception e)
            {
                Log(e);
                throw;
            }
        }
        private bool IsError(string response, out string msg)
        {
            try
            {
                msg = GetParameterValue("ERR", response);
                // caleidoscopio riotrna un errore formattato così
                // ERR=DESCRIZIONE ERRORE;CHK=FFFF;
                return msg.Length > 0 || response == COM_ERROR_RESPONSE;
            }
            catch (Exception e)
            {
                Log(e);
                throw;
            }
        }
        private void Polling()
        {
            try
            {
                var getstatusresp = "";
                var startcycleid = -1;
                var cyclestarted = false;
                while (_poll)
                {
                    try
                    {
                        // inserita all'inizio così anche quando 
                        // viene eseguito un continue si attende
                        Thread.Sleep(5000);
                        getstatusresp = SendCommand("GETSTATUS");
                        if (IsError(getstatusresp, out string errmsg))
                        {
                            Log($"GETSTATUS communication error {errmsg}");
                            continue;
                        }
                        CheckDateTime(getstatusresp);
                        WasherCycle cycle = null;
                        if (IsCycleStarted(getstatusresp) && !cyclestarted)
                        {
                            cyclestarted = true;
                            // grazie caleidoscopio!!! trick non comprensibile alla partenza del nuovo ciclo
                            // l'id è in realtà quello del ciclo successivo.
                            startcycleid = int.Parse(GetParameterValue("ID", getstatusresp)) - 1;
                            cycle = CycleStarted(getstatusresp, startcycleid);
                        }
                        else if (IsWasherInStandBy(getstatusresp) && !IsMemoryEmpty(getstatusresp))
                        {
                            cyclestarted = false;
                            cycle = CloseCycle(getstatusresp, startcycleid);
                        }
                        if (cycle != null)
                            memorycycles.Enqueue(cycle);
                    }
                    catch (Exception e)
                    {
                        Log("Polling cycle error", e);
                    }
                }
            }
            catch (Exception e)
            {
                Log("Polling error exiting...", e);
            }
        }

        private void CheckDateTime(string getStatusResponse)
        {
            try
            {
                string date = GetParameterValue("DATE", getStatusResponse);
                string time = GetParameterValue("TIME", getStatusResponse);
                DateTime dt = DateTime.Parse(date + " " + time);
                TimeSpan delta = dt - DateTime.Now;
                if (Math.Abs(delta.TotalSeconds) > _maxErrorSeconds)
                {
                    string resp;
                    resp = SendCommand(FillDateTimeCommand("DT dd/MM/yyyy"));
                    if (!resp.Contains("OK") && resp.Contains("ERR"))
                        throw new ApplicationException("Cannot set date to device");
                    Thread.Sleep(200);
                    resp = SendCommand(FillDateTimeCommand("TM HH:mm:ss"));
                    if (!resp.Contains("OK") && resp.Contains("ERR"))
                        throw new ApplicationException("Cannot set time to device");
                    Thread.Sleep(200);
                }
            }
            catch (Exception exc)
            {
                Log("CheckDateTime Error", exc);
            }
        }

        private void OnCommandSent(byte[] cmd)
        {
            CommandSent?.Invoke(Encoding.ASCII.GetString(cmd));
        }
        private void OnCommandReceived(byte[] cmd)
        {
            CommandReceived?.Invoke(Encoding.ASCII.GetString(cmd));
        }
        private void OnError(string message)
        {
            ErrorsThrown?.Invoke(message);
        }
        private void Log(object message, Exception e = null)
        {
            if (e != null)
            {
                Logger.Error(message, e);
                OnError($"{message} Exception: {e}");
            }
            else
            {
                Logger.Error(message);
                OnError($"{message}");
            }

        }
        public string SendCommand(string command)
        {
            var cycle = new WasherCycle();
            TcpClient client = null;
            NetworkStream stream = null;
            try
            {
                client = new TcpClient(Ipaddress, Portnumber);
                stream = client.GetStream();
                var cmd = GetRequestCommand(command);
                OnCommandSent(cmd);
                stream.ReadTimeout = 20000;
                stream.Write(cmd, 0, cmd.Length);
                var data = new byte[RESP_CMD_LEN];
                int bytesread = stream.Read(data, 0, RESP_CMD_LEN);
                OnCommandReceived(data);
                if (bytesread == 0)
                    return "";
                // DELMEM non ritorna bytes.
                if (bytesread != RESP_CMD_LEN)
                    throw new ApplicationException($"error reading response bytes from stream COMMAND {command}");
                if (!CheckHeadChars(data))
                    throw new ApplicationException("missing response head chars");
                if (!ValidateChecksum(data))
                    throw new ApplicationException("checksum failed on response");
                if (!ExtractInnerResponse(data, out byte[] innerresp))
                    throw new ApplicationException("old response message extraction failed");
                return Encoding.ASCII.GetString(innerresp);
            }
            catch (Exception ex)
            {
                Log($"Exception: {ex}");
                return COM_ERROR_RESPONSE;
            }
            finally
            {
                if (stream != null)
                    stream.Close();
                if (client != null)
                    client.Close();
            }
        }

        public static string FillDateTimeCommand(string cmd)
        {
            if (cmd.StartsWith("TM "))
                return "TM " + DateTime.Now.ToString("HH:mm:ss");
            else if (cmd.StartsWith("DT "))
                return "DT " + DateTime.Now.ToString("dd/MM/yyyy");
            else
                throw new ApplicationException("Invalid Date/Time command");
        }

        public bool ExtractInnerResponse(byte[] data, out byte[] oldresponse)
        {
            oldresponse = new byte[RESP_MSG_SEGMENT_LEN];
            try
            {
                var startindex = HEAD_SEGMENT_LEN + MSG_LEN_SEGMENT_LEN + ID_MAC_SEGMENT_LEN + EXP_SEGMENT_LEN;
                for (int i = startindex; i < startindex + RESP_MSG_SEGMENT_LEN; i++)
                    oldresponse[i - startindex] = data[i];
                return true;
            }
            catch (Exception e)
            {
                Log(data, e);
                return false;
            }
        }
        private bool CheckHeadChars(byte[] data)
        {
            try
            {
                return data[0] == headcharsbuff[0] &&
                    data[1] == headcharsbuff[1];
            }
            catch (Exception e)
            {
                Log(e);
                return false;
            }
        }
        private bool ValidateChecksum(byte[] data)
        {
            try
            {
                var checksum = GetChecksum(data);
                int startindex = RESP_CMD_LEN - CHK_SEGMENT_LEN;
                return checksum[0] == data[startindex] &&
                    checksum[1] == data[startindex + 1] &&
                    checksum[2] == data[startindex + 2] &&
                    checksum[3] == data[startindex + 3];
            }
            catch (Exception e)
            {
                Log(e);
                return false;
            }
        }
        public byte[] GetRequestCommand(string command)
        {
            try
            {
                char[] retval = new char[SEND_CMD_LEN];
                int copyindex = 0;
                headcharsbuff.CopyTo(retval, copyindex);
                copyindex += HEAD_SEGMENT_LEN;
                reqmsglenbuff.CopyTo(retval, copyindex);
                copyindex += MSG_LEN_SEGMENT_LEN;
                idmacbuff.CopyTo(retval, copyindex);
                copyindex += ID_MAC_SEGMENT_LEN;
                expbuff.CopyTo(retval, copyindex);
                copyindex += EXP_SEGMENT_LEN;
                string msg = command.PadRight(REQ_MSG_SEGMENT_LEN, ' ');
                msg.ToCharArray().CopyTo(retval, copyindex);
                copyindex += REQ_MSG_SEGMENT_LEN;
                GetChecksum(retval).CopyTo(retval, copyindex);
                return Encoding.ASCII.GetBytes(retval);
            }
            catch (Exception e)
            {
                Log($"command: {command}", e);
                throw;
            }
        }
        public byte[] GetChecksum(byte[] cmdbuff)
        {
            try
            {
                return Encoding.ASCII.GetBytes(GetChecksum(Encoding.ASCII.GetChars(cmdbuff)));
            }
            catch (Exception e)
            {
                Log(cmdbuff, e);
                throw;
            }
        }
        public char[] GetChecksum(char[] cmdbuff)
        {
            try
            {
                ushort sum = 0;
                for (int i = 0; i < cmdbuff.Length - CHK_SEGMENT_LEN; i++)
                    sum += (ushort)cmdbuff[i];
                var retval = BitConverter.ToString(BitConverter.GetBytes(sum))
                    .Replace("-", "")
                    .PadLeft(4, '0')
                    .ToCharArray();
                if (BitConverter.IsLittleEndian)
                {
                    var temp = new char[CHK_SEGMENT_LEN] { retval[2], retval[3], retval[0], retval[1] };
                    retval = temp;
                }
                return retval;
            }
            catch (Exception e)
            {
                Log(cmdbuff, e);
                throw;
            }
        }
        private bool IsMemoryEmpty(string response)
        {
            try
            {
                return int.Parse(GetParameterValue("MEM", response)) == 0;
            }
            catch (Exception e)
            {
                Log(response, e);
                return true;
            }
        }
        private bool IsWasherInStandBy(string getstatusresp)
        {
            try
            {
                var task = GetParameterValue("TASK", getstatusresp).ToUpper();
                return task == TASK_READY_ENG ||
                    task == TASK_READY_ITA;
            }
            catch (Exception e)
            {
                Log(getstatusresp, e);
                return false;
            }
        }
        /// <summary>
        /// In tutti i cicli di lavaggio, test etc. si comincia 
        /// sempre con un test di tenuta, il task test di tenuta
        /// viene dunque identificato come inizio di un ciclo.
        /// </summary>
        /// <param name="getstatusresp"></param>
        /// <returns></returns>
        private bool IsCycleStarted(string getstatusresp)
        {
            try
            {
                var task = GetParameterValue("TASK", getstatusresp).ToUpper();
                return task == TASK_KEEP_TEST_ENG ||
                    task == TASK_KEEP_TEST_ITA;
            }
            catch (Exception e)
            {
                Log($"getstatusresp: {getstatusresp}", e);
                return false;
            }
        }
        private WasherCycle CycleStarted(string getstatusresp, int startcycleid)
        {
            try
            {
                var retval = new WasherCycle { IsPreWash = true };
                retval.CycleCount = startcycleid.ToString();
                retval.Completed = false;
                retval.StartTimestamp = GetDateTime(getstatusresp);
                startcycledatetime = retval.StartTimestamp;
                retval.DesiredDestinationState = StateTransactions.GetStateId(FixedStates.Start_pre_wash);
                retval.Failed = false;
                retval.OperatorStartExternalID = GetParameterValue("OP", getstatusresp);
                retval.OperatorStartID = TranscodeOperator(retval.OperatorStartExternalID);
                if (retval.OperatorStartID <= 0)
                    retval.OperatorStartID = TranscodeOperator("UNKNOWN");
                // operatore di start ed end sono uguali
                retval.WasherID = TranscodeWasher(idmac);
                retval.WasherExternalID = idmac;
                retval.DeviceExternalID = GetParameterValue("STR", getstatusresp);
                retval.DeviceID = TranscodeDevice(retval.DeviceExternalID);
                if (retval.DeviceID <= 0)
                    retval.DeviceID = TranscodeDevice("UNKNOWN");
                retval.Filename = "socket connection";
                retval.FileDatetime = retval.StartTimestamp;
                retval.FileContent = getstatusresp.Replace("\n", "");
                return retval;
            }
            catch (Exception e)
            {
                Log($"getstatusresp: {getstatusresp}", e);
                return null;
            }
        }
        private WasherCycle CloseCycle(string getstatusresp, int startcycleid)
        {
            try
            {
                var retval = new WasherCycle { IsPreWash = true };
                var mem = GetParameterValue("MEM", getstatusresp);
                var memnumber = int.Parse(mem);
                var response = "";
                var currentid = -2;
                do
                {
                    // cancella tutte le memorie non di questo ciclo
                    response = SendCommand("GETMEM");
                    SendCommand("DELMEM");
                    if (response.Length == 0)
                        continue;
                    if (IsError(response, out string message))
                        throw new ApplicationException($"Error in GETMEM, errormessage {message}");
                    currentid = int.Parse(GetParameterValue("ID", response));
                    memnumber--;
                } while (currentid != startcycleid && memnumber > 0);
                // se non abbiamo trovato il currentid
                // è un problema
                if (currentid != startcycleid)
                    throw new ApplicationException($"startcycleid {startcycleid} not found, dirty memory trouble!");
                if (IsError(response, out string errmsg))
                    throw new ApplicationException($"memory error getting memory {errmsg}");
                // a questo punto la chiusura del ciclo procede
                retval.Completed = true;
                retval.CycleCount = startcycleid.ToString();
                retval.StartTimestamp = startcycledatetime;
                retval.EndTimestamp = DateTime.Now;// GetDateTime(response); --> non dalla risposta, dato non presente, sempre inizio
                retval.DesiredDestinationState = StateTransactions.GetStateId(FixedStates.End_pre_wash);
                var cycle_err = GetParameterValue("ERR", response);
                retval.Failed = cycle_err.Length > 0;
                Log($"PREWASHER cycle_err {cycle_err}");
                retval.OperatorStartExternalID = GetParameterValue("OP", response);
                retval.OperatorStartID = TranscodeOperator(retval.OperatorStartExternalID);
                if (retval.OperatorStartID <= 0)
                    retval.OperatorStartID = TranscodeOperator("UNKNOWN");
                // operatore di start ed end sono uguali nel caso in cui
                // non sia implementato la versione 06 del protocollo
                var operatorend = GetParameterValue("OPF", response);
                if (operatorend.Length > 0)
                {
                    retval.OperatorEndExternalID = operatorend;
                    retval.OperatorEndID = TranscodeOperator(retval.OperatorEndExternalID);
                    if (retval.OperatorEndID <= 0)
                        retval.OperatorEndID = TranscodeOperator("UNKNOWN");
                }
                // caso vecchio protocollo, i due opend = opstart
                else
                {
                    retval.OperatorEndExternalID = retval.OperatorStartExternalID;
                    retval.OperatorEndID = retval.OperatorStartID;
                }
                retval.WasherID = TranscodeWasher(idmac);
                retval.WasherExternalID = idmac;
                retval.DeviceExternalID = GetParameterValue("STR", response);
                retval.DeviceID = TranscodeDevice(retval.DeviceExternalID);
                retval.Filename = "socket connection";
                retval.FileDatetime = retval.StartTimestamp;
                retval.FileContent = response.Replace("\n", "");
                if (retval.DeviceID <= 0)
                    retval.DeviceID = TranscodeDevice("UNKNOWN");
                return retval;
            }
            catch (Exception e)
            {
                Log($"getstatusersp: {getstatusresp}", e);
                return null;
            }
        }
        private DateTime GetDateTime(string response)
        {
            try
            {
                string date = GetParameterValue("DATE", response);
                string time = GetParameterValue("TIME", response);
                var dateslices = date.Split(datesplitchar, StringSplitOptions.RemoveEmptyEntries);
                var timeslices = time.Split(colonsplitchar, StringSplitOptions.RemoveEmptyEntries);
                return new DateTime(int.Parse(dateslices[2]),
                    int.Parse(dateslices[1]),
                    int.Parse(dateslices[0]),
                    int.Parse(timeslices[0]),
                    int.Parse(timeslices[1]),
                    0);
            }
            catch (Exception e)
            {
                Log($"response: {response}", e);
                return DateTime.Now;
            }
        }
        /// <summary>
        /// Estrae il valore del parametro dalla risposta della scheda
        /// la risposta è nel formato parametro=valore;
        /// </summary>
        /// <param name="parametro">il parametro da cercare</param>
        /// <param name="response">la risposta della scheda</param>
        /// <returns></returns>
        public static string GetParameterValue(string parametro, string response)
        {
            try
            {
                var startindex = response.ToUpper().IndexOf(parametro.ToUpper() + "=");
                if (startindex < 0)
                    return "";
                var substring = response.Substring(startindex);
                var endindex = substring.IndexOf(";");
                startindex = substring.IndexOf("=");
                return substring.Substring(startindex + 1, endindex - startindex - 1);
            }
            catch (Exception e)
            {
                Logger.Error($"parametro: {parametro}, response = {response}", e);
                throw;
            }
        }
    }
}
