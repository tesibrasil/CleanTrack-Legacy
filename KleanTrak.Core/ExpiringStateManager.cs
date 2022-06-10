using System;
using System.Threading;

namespace KleanTrak.Core
{
	public class ExpiringStateManager
    {
        private static readonly log4net.ILog Logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static ExpiringStateManager _instance = new ExpiringStateManager();
        public static ExpiringStateManager Instance { get { return _instance; } }

        ManualResetEvent _terminateThreadEvent = new ManualResetEvent(false);
        Thread _thread;

        public void Start(bool debug = false)
        {
            Logger.Info("1");

            if (_thread == null)
            {
                _thread = new Thread(WorkerThread);
                _thread.Start();
            }

            Logger.Info("2");
        }

        public void Stop()
        {
            Logger.Info("1");

            _terminateThreadEvent.Set();
            if (_thread != null)
                _thread.Join();
            _thread = null;

            Logger.Info("2");
        }

        void WorkerThread()
        {
            while (!_terminateThreadEvent.WaitOne(60000))
            {
                Logger.Info("Check expiring devices");
                DbConnection conn = new DbConnection();
                try
                {
                    DbRecordset reader = conn.ExecuteReader("SELECT id, stato, idstatodestinazione, idoperatorestato from vistadispositiviscaduti");
                    foreach (var record in reader)
                    {
                        int devID = record.GetInt("id").HasValue ? record.GetInt("id").Value : 0;
                        int stateOld = record.GetInt("stato").HasValue ? record.GetInt("stato").Value : 0;
                        int stateNew = record.GetInt("idstatodestinazione").HasValue ? record.GetInt("idstatodestinazione").Value : 0;
                        int userID = record.GetInt("idoperatorestato").HasValue ? record.GetInt("idoperatorestato").Value : 0;
						if (devID > 0)
						{
							var device = Devices.FromID(devID);
							Logger.Info($"Found expiring device id: {device.ID} description: {device.Description}");
                            if (!StateTransactions.DeviceExpired(devID, userID, stateNew, DateTime.Now, device.Id_sede))
                                Logger.Error($"device expired return false " +
                                    $"devID: {devID}, userID: {userID}, stateNew: {stateNew}, " +
                                    $"datetime: {DateTime.Now}, device.Id_sede: {device.Id_sede}");
						}
					}
                }
                catch (Exception ex)
                {
                    Logger.Error(ex);
                }
            }
        }
    }
}
