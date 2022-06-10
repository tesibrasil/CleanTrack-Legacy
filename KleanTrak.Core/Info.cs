using KleanTrak.Model;

namespace KleanTrak.Core
{
	public class Info
	{
        private static readonly log4net.ILog Logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static CmdGetInfoFromBarcodeResponse GetInfoFromBarcode(CmdGetInfoFromBarcode req)
		{
			Logger.Info("input: " + req.Barcode);

			CmdGetInfoFromBarcodeResponse ret = new CmdGetInfoFromBarcodeResponse();
			ret.BarcodeType = BarcodeTypes.Unknown;
			ret.ErrorMessage = Dictionary.Instance["barcodeNotFound"];
			ret.Successed = false;

			string description = "";
			if (Operations.TryToFindOperationBarcode(req.Barcode, ref description))
			{
				ret.BarcodeType = BarcodeTypes.Operation;
				ret.Description = description;
				ret.ErrorMessage = null;
				ret.Successed = true;

                Logger.Info("output: " + ret.BarcodeType.ToString() + " " + ret.Description);
				return ret;
			}

			if (Devices.TryToFindDeviceBarcode(req.Barcode, ref description))
			{
				ret.BarcodeType = BarcodeTypes.Device;
				ret.Description = description;
				ret.ErrorMessage = null;
				ret.Successed = true;

                Logger.Info("output: " + ret.BarcodeType.ToString() + " " + ret.Description);
				return ret;
			}

			if (Operators.TryToFindUserBarcode(req.Barcode, ref description))
			{
				ret.BarcodeType = BarcodeTypes.Operator;
				ret.Description = description;
				ret.ErrorMessage = null;
				ret.Successed = true;

                Logger.Info("output: " + ret.BarcodeType.ToString() + " " + ret.Description);
				return ret;
			}
            Logger.Error("output: " + ret.BarcodeType.ToString() + " " + ret.Description);
			return ret;
		}
	}
}
