using KleanTrak.Model;
using System.Collections.Generic;

namespace KleanTrak.Core
{
	public class RfidReaderManager
    {
        private static readonly log4net.ILog Logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static RfidReaderManager _instance = new RfidReaderManager();
        public static RfidReaderManager Instance { get { return _instance; } }

        private List<ReaderRfid> _listReaders = new List<ReaderRfid>();

        public void Start(bool debug = false)
        {
            Logger.Info("1");

			List<Reader> list = Readers.Get();
			foreach (Reader r in list)
			{
				ReaderRfid rfidTemp = new ReaderRfid(r.DefaultStateID, r.Timeout, r.IP, r.Port, r.Description);
				_listReaders.Add(rfidTemp);
			}

            Logger.Info("2");
        }

        public void Stop()
        {
            Logger.Info("1");

            foreach (ReaderRfid rfidTemp in _listReaders)
			{
				rfidTemp.Dispose();
			}

            Logger.Info("2");
        }
    }
}
