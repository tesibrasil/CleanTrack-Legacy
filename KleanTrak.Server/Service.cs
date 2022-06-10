using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Channels;
using KleanTrak.Core;
using KleanTrak.Model;

namespace KleanTrak.Server
{
    public class Service : IService
    {
        // static object locker = new object();
        private static readonly log4net.ILog Logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public Stream AcceptMessage(Stream stream)
        {
			try
			{
				var reader = new StreamReader(stream, Encoding.UTF8);
				string message = reader.ReadToEnd();

				Logger.Info("ip: " + GetIp() + " input: " + message);

				Request req = (Request)Request.ReadObjectFromXml(message);
				Response ret = AcceptMessageString(req);
				string retMsg = ret.SaveObjectToXml();

				MemoryStream streamResponse = new MemoryStream();
				StreamWriter writer = new StreamWriter(streamResponse);
				writer.Write(retMsg);
				writer.Flush();
				streamResponse.Position = 0;

				Logger.Info("output: " + retMsg);
				return streamResponse;
			}
			catch (Exception e)
			{
				Logger.Error(stream, e);
				var response = new Response() { Successed = false, ErrorMessage = e.ToString() };
				MemoryStream streamResponse = new MemoryStream();
				StreamWriter writer = new StreamWriter(streamResponse);
				writer.Write(response.SaveObjectToXml());
				writer.Flush();
				streamResponse.Position = 0;
				return streamResponse;
			}
		}

        public Stream AcceptJsonMessage(Stream stream)
        {
            var reader = new StreamReader(stream, Encoding.UTF8);
            string message = reader.ReadToEnd();

            Logger.Info("ip: " + GetIp() + " input: "+ message);

            Request req = (Request)Request.ReadObjectFromJson(message);
            Response ret = AcceptMessageString(req);
            string retMsg = ret.SaveObjectToJson();

            MemoryStream streamResponse = new MemoryStream();
            StreamWriter writer = new StreamWriter(streamResponse);
            writer.Write(retMsg);
            writer.Flush();
            streamResponse.Position = 0;

            Logger.Info("output: "+ retMsg);
            return streamResponse;
        }

        private Response AcceptMessageString(Request req)
        {
            if (req == null)
            {
                Logger.Error("Error deserializing message!");
                return new Response
                { 
                    Successed = false, 
                    ErrorMessage = "Error deserializing message!" 
                };
            }
            Response ret = null;
            try
            {
                switch (req.GetType().ToString())
                {
                    case "KleanTrak.Model.CmdGetInfoFromBarcode":
                        ret = Info.GetInfoFromBarcode((CmdGetInfoFromBarcode)req);
                        break;

                    case "KleanTrak.Model.CmdSetDeviceStatus":
                        ret = Devices.SetDeviceStatus((CmdSetDeviceStatus)req);
                        break;

                    case "KleanTrak.Model.CmdGetDeviceStatus":
                        ret = Devices.GetDeviceStatus((CmdGetDeviceStatus)req);
                        break;
                    case "KleanTrak.Model.CmdRemoveExamCycle":
                        var remove_request = (CmdRemoveExamCycle)req;
                        var remove_response = CycleManager.RemoveExamCycle(remove_request.ExamId, 
                            remove_request.ExamSiteId, 
                            remove_request.ExamUoId,
                            remove_request.DeviceBarCode, 
                            remove_request.OperatorBarCode, 
                            out string error_message, 
                            out string reset_state_name);
                        ret = new CmdRemoveExamCycleResponse 
                        {
                            Successed = remove_response == CycleManager.RemoveCycleResponse.Ok,
                            NotLastCycle = remove_response == CycleManager.RemoveCycleResponse.NotLastCycle,
                            ResetStateName = reset_state_name,
                            ErrorMessage = error_message
                        };
                        break;
                    case "KleanTrak.Model.CmdGetDevicesList":
                        var request = req as CmdGetDevicesList;
                        ret = new CmdGetDevicesListResponse 
                        { 
                            Successed = true,
                            Devices = Devices.GetDevicesList(request.SiteId, 
                                request.UoId, 
                                request.DeviceName,
                                request.IncludeDismissed)
                        };
                        break;
                    case "KleanTrak.Model.CmdGetDeviceExams":
                        var devreq = req as CmdGetDeviceExams; 
                        ret = new CmdGetDeviceExamsResponse
                        {
                            Successed = true,
                            DeviceExams = Devices.GetDeviceExams(devreq.DeviceId, devreq.UoId)
                        };
                        break;
                    case "KleanTrak.Model.CmdSendPiConfiguration":
                        ret = SendPiConfiguration();
                        break;
                    case "KleanTrak.Model.CmdSetWasherConfiguration":
                        ret = WasherManager.SetWasherConfiguration();
                        break;

                    case "KleanTrak.Model.CmdPing":
                        ret = new Response() { Successed = true };
                        break;

                    case "KleanTrak.Model.CmdGetStartCycleBarcode":
                        ret = Operations.GetStartCycleBarcode();
                        break;

                    case "KleanTrak.Model.CmdGetExamCycles":
                        ret = CycleManager.GetExamCycles((CmdGetExamCycles)req);
                        break;

                    case "KleanTrak.Model.CmdGetCurrentExamCycleId":
                        var curr_cycle_req = (CmdGetCurrentExamCycleId)req;
                        int last_cycle_id = CycleManager.CmdGetCurrentExamCycle(curr_cycle_req.ExamId,
                            curr_cycle_req.SiteId,
                            curr_cycle_req.UoId);
                        ret = new CmdGetCurrentExamCycleIdResponse
                        {
                            CycleId = last_cycle_id,
                            Successed = true
                        };
                        break;

                    case "KleanTrak.Model.CmdAddWasherCycle":
                        WasherManager.Instance.AddCycles(((CmdAddWasherCycle)req).WasherCycleList);
                        ret = new Response() { Successed = true };
                        break;


                    default:
                        Logger.Info("Message type " + req.GetType().ToString() + " not recognized!");
                        ret = new Response() { Successed = false, ErrorMessage = "Message type " + req.GetType().ToString() + " not recognized!" };
                        break;
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                ret = new Response() { Successed = false, ErrorMessage = "Exception: " + ex.Message + " " + ex.StackTrace };
            }

            return ret;
        }

        public Response SendPiConfiguration()
        {
            Response ret = new Response() { Successed = true };
            DbConnection db = new DbConnection();
            DbRecordset dataset = null;

            try
            {
                dataset = db.ExecuteReader("SELECT LETTORI.*, STATO.BARCODE AS STATOBARCODE, STATO.DESCRIZIONEAZIONE FROM LETTORI LEFT OUTER JOIN STATO ON STATO.ID = LETTORI.IDSTATODEFAULT WHERE LETTORI.ELIMINATO = 0 AND TIPO = " + ((int)DeviceReadersTypes.Pi));
            }
            catch (Exception ex)
            {
                return new Response() { Successed = false, ErrorMessage = ex.Message };
            }

            foreach (var record in dataset)
            {
                PiConfiguration conf = new PiConfiguration()
                {
                    ServerHttpEndpoint = KleanTrakService.HttpEndpoint + "AcceptMessage",
                    DefaultOperationBarcode = record.GetString("STATOBARCODE"),
                    DefaultOperationDescription = record.GetString("DESCRIZIONEAZIONE"),
                    LabelOperationText = record.GetString("ETICHETTAOPERAZIONE"),
                    LabelDeviceText = record.GetString("ETICHETTADISPOSITIVO"),
                    LabelUserText = record.GetString("ETICHETTAOPERATORE"),
                    TimeoutCompletingSteps = record.GetInt("TIMEOUTCOMPLETAMENTOSTEP"),
                    DelayBeforeApplyChanges = record.GetInt("ATTESAPRIMAAPPLICA"),
                    WorklistSelection = record.GetBoolean("SELEZIONEWORKLIST"),
                    DelayBeforeClosingPopup = record.GetInt("ATTESAPRIMACHIUSURAPOPUP")
                };

                string url = "http://" + record.GetString("IP") + ":" + record.GetString("PORTA") + "/AcceptMessage";

                Logger.Info( "----------------------------------------------------------------------------------------------");
                Logger.Info("Sending configuration to " + record.GetString("DESCRIZIONE") + " " + url + "...");
                Response r = Send(url, new CmdSetPiConfiguration() { Configuration = conf });
                Logger.Info(url + (r.Successed ? " success!" : " failed!"));
                if (!r.Successed)
                {
                    Logger.Error(url + " " + r.ErrorMessage);

                    ret.Successed = false;
                    if (ret.ErrorMessage == null)
                        ret.ErrorMessage = "";
                    ret.ErrorMessage += record.GetString("DESCRIZIONE") + " " + record.GetString("IP") + ":" + record.GetString("PORTA") + "\r\n" + r.ErrorMessage;
                }

                Logger.Info("----------------------------------------------------------------------------------------------");
                Logger.Info("Sending dictionary to " + record.GetString("DESCRIZIONE") + " " + url + "...");
                r = Send(url, new CmdSetDictionary() { Dictionary = Dictionary.Instance.DictionaryBase });
                Logger.Info(url + (r.Successed ? " success!" : " failed!"));
                if (!r.Successed)
                {
                    Logger.Error("SendPiConfiguration " + url + " " + r.ErrorMessage);

                    ret.Successed = false;
                    if (ret.ErrorMessage == null)
                        ret.ErrorMessage = "";
                    ret.ErrorMessage += record.GetString("DESCRIZIONE") + " " + record.GetString("IP") + ":" + record.GetString("PORTA") + "\r\n" + r.ErrorMessage;
                }
            }

            return ret;
        }

        private Response Send(string url, Request req)
        {
            Response ret = null;

            try
            {
                string strReq = req.SaveObjectToXml();

                HttpClientHandler httpClientHandler = new HttpClientHandler()
                {
                    Proxy = new WebProxy()
                };

                HttpClient httpClient = new HttpClient(httpClientHandler);
                HttpResponseMessage response = httpClient.PostAsync(url, new StringContent(strReq)).Result;

                response.EnsureSuccessStatusCode();

                string httpResponseBody = response.Content.ReadAsStringAsync().Result;
                ret = (Response)Response.ReadObjectFromXml(httpResponseBody);
            }
            catch (Exception ex)
            {
                ret = new Response();
                ret.Successed = false;
                ret.ErrorMessage = ex.Message + " " + ex.StackTrace.ToString();

                if (ex.InnerException != null)
                    ret.ErrorMessage += "\r\n\r\n" + ex.InnerException.Message;
            }

            return ret;
        }

        private string GetIp()
        {
            OperationContext context = OperationContext.Current;
            MessageProperties properties = context.IncomingMessageProperties;
            RemoteEndpointMessageProperty endpoint = properties[RemoteEndpointMessageProperty.Name] as RemoteEndpointMessageProperty;
            string address = string.Empty;
            
            if (properties.Keys.Contains(HttpRequestMessageProperty.Name))
            {
                HttpRequestMessageProperty endpointLoadBalancer = properties[HttpRequestMessageProperty.Name] as HttpRequestMessageProperty;
                if (endpointLoadBalancer != null && endpointLoadBalancer.Headers["X-Forwarded-For"] != null)
                    address = endpointLoadBalancer.Headers["X-Forwarded-For"];
            }
            if (string.IsNullOrEmpty(address))
            {
                address = endpoint.Address;
            }

            return address;
        }
    }
}
