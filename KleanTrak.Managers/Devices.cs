using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KleanTrak.Models;
using LibLog;

namespace KleanTrak.Managers
{
    public class Devices
    {
        public static Device FromBarcode(string barcode)
        {
            DbConnection db = new DbConnection();
            DbRecordset dataset = db.ExecuteReader("SELECT DISPOSITIVI.*, STATO.DESCRIZIONE AS DESCRIZIONESTATO FROM DISPOSITIVI LEFT OUTER JOIN STATO ON DISPOSITIVI.STATO = STATO.ID WHERE DISPOSITIVI.ELIMINATO = 0 AND DISMESSO = 0 AND TAG = '" + barcode + "'");

            if (dataset.Count() >= 1)
            {
                return new Device()
                {
                    ID = dataset.First().GetInt("ID").Value,
                    Code = dataset.First().GetString("MATRICOLA"),
                    Description = dataset.First().GetString("DESCRIZIONE"),
                    Tag = dataset.First().GetString("TAG"),
                    StateID = dataset.First().GetInt("STATO").Value,
                    StateDescription = dataset.First().GetString("DESCRIZIONESTATO")
                };
            }

            return null;
        }

        public static Device FromSerial(string serial)
        {
            DbConnection db = new DbConnection();
            DbRecordset dataset = db.ExecuteReader("SELECT DISPOSITIVI.*, STATO.DESCRIZIONE AS DESCRIZIONESTATO FROM DISPOSITIVI LEFT OUTER JOIN STATO ON DISPOSITIVI.STATO = STATO.ID WHERE DISPOSITIVI.ELIMINATO = 0 AND DISMESSO = 0 AND MATRICOLA = '" + serial + "'");

            if (dataset.Count() >= 1)
            {
                return new Device()
                {
                    ID = dataset.First().GetInt("ID").Value,
                    Code = dataset.First().GetString("MATRICOLA"),
                    Description = dataset.First().GetString("DESCRIZIONE"),
                    Tag = dataset.First().GetString("TAG"),
                    StateID = dataset.First().GetInt("STATO").Value,
                    StateDescription = dataset.First().GetString("DESCRIZIONESTATO")
                };
            }

            return null;
        }

		public static CmdGetDeviceStatusResponse GetDeviceStatus(CmdGetDeviceStatus req)
		{
			Logger.Get().Write("", "Service.GetDeviceStatus input: ", req.DeviceBarcode + " " + req.DeviceSerial, null, Logger.LogLevel.Info);

			Device dev = null;

			try
			{
				dev = req.DeviceBarcode != null ? Devices.FromBarcode(req.DeviceBarcode) : Devices.FromSerial(req.DeviceSerial);
			}
			catch (Exception ex)
			{
				Logger.Get().Write("", "Service.GetDeviceStatus exception: ", ex.Message + " " + ex.StackTrace, null, Logger.LogLevel.Error);
				return new CmdGetDeviceStatusResponse() { Successed = false, ErrorMessage = ex.Message };
			}

			if (dev != null)
			{
				Logger.Get().Write("", "Service.GetDeviceStatus output: ", dev.Description + " " + dev.StateDescription, null, Logger.LogLevel.Info);
				return new CmdGetDeviceStatusResponse() { Successed = true, Description = dev.Description, Status = dev.StateDescription };
			}

			Logger.Get().Write("", "Service.GetDeviceStatus output: ", Dictionary.Instance["deviceNotFound"], null, Logger.LogLevel.Error);
			return new CmdGetDeviceStatusResponse() { Successed = false, ErrorMessage = Dictionary.Instance["deviceNotFound"] };
		}

		public static Response SetDeviceStatus(CmdSetDeviceStatus req)
		{
			Logger.Get().Write("", "Service.SetDeviceStatus input: ", req.DeviceBarcode + " " + req.DeviceSerial, null, Logger.LogLevel.Info);

			try
			{
				Operation op = req.OperationBarcode != null ? Operations.FromBarcode(req.OperationBarcode) : Operations.FromID(req.OperationID);
				if (op == null)
				{
					Logger.Get().Write("", "Service.SetDeviceStatus", Dictionary.Instance["operationNotFound"], null, Logger.LogLevel.Error);
					return new Response() { Successed = false, ErrorMessage = Dictionary.Instance["operationNotFound"] };
				}

				Device dev = req.DeviceBarcode != null ? Devices.FromBarcode(req.DeviceBarcode) : Devices.FromSerial(req.DeviceSerial);
				if (dev == null)
				{
					Logger.Get().Write("", "Service.SetDeviceStatus", Dictionary.Instance["deviceNotFound"], null, Logger.LogLevel.Error);
					return new Response() { Successed = false, ErrorMessage = Dictionary.Instance["deviceNotFound"] };
				}

				Operator user = req.UserBarcode != null ? Operators.FromBarcode(req.UserBarcode) : Operators.FromSerial(req.UserSerial);
				if (user == null)
				{
					Logger.Get().Write("", "Service.SetDeviceStatus", Dictionary.Instance["operatorNotFound"], null, Logger.LogLevel.Error);
					return new Response() { Successed = false, ErrorMessage = Dictionary.Instance["operatorNotFound"] };
				}

				if (!StateTransactions.IsValid(dev.StateID, op.ID))
				{
					string stateAccepted = (from s in StateTransactions.Get() where s.IDStateOld == dev.StateID select s.StateNew).Aggregate((i, j) => i + ";" + j);

					Logger.Get().Write("", "Service.SetDeviceStatus", string.Format(Dictionary.Instance["stateNotValid2"], dev.Tag, dev.Description, dev.StateDescription, stateAccepted), null, Logger.LogLevel.Error);
					return new Response() { Successed = false, ErrorMessage = string.Format(Dictionary.Instance["stateNotValid2"], dev.Tag, dev.Description, dev.StateDescription, stateAccepted) };
				}

				StateTransactions.Add(dev.ID, user.ID, dev.StateID, op.ID);

				/*
				lock (locker)
				{
					//amrfidmgrex.DBUtilities.insertnewCycleEx(dev.Code, user.Code, -1, op.ID, dev.StateID, -1, DbConnection.ConnectionString);
				}
				*/
			}
			catch (Exception ex)
			{
				Logger.Get().Write("", "Service.SetDeviceStatus exception: ", ex.Message + " " + ex.StackTrace, null, Logger.LogLevel.Error);
				return new Response() { Successed = false, ErrorMessage = ex.Message };
			}

			Logger.Get().Write("", "Service.SetDeviceStatus output: ", "true", null, Logger.LogLevel.Info);
			return new Response() { Successed = true };
		}

		public static bool TryToFindDeviceBarcode(string barcode, ref string description)
		{
			Logger.Get().Write("", "Service.TryToFindDeviceBarcode input: ", barcode, null, Logger.LogLevel.Info);

			try
			{
				Device device = FromBarcode(barcode);
				if (device != null)
				{
					description = device.Description;
					Logger.Get().Write("", "Service.TryToFindDeviceBarcode return: ", description, null, Logger.LogLevel.Info);
					return true;
				}
			}
			catch (Exception ex)
			{
				Logger.Get().Write("", "Service.TryToFindDeviceBarcode exception ", ex.Message + " " + ex.StackTrace, null, Logger.LogLevel.Error);
			}

			Logger.Get().Write("", "Service.TryToFindDeviceBarcode return: ", "false", null, Logger.LogLevel.Info);
			return false;
		}
	}
}
