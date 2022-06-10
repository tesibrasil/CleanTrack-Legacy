using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KleanTrak.Core
{
	public class DeviceStateChangeData
	{
		public string DeviceCode = "";                  // Dispositivi.Matricola - Cicli.IDDispositivo
		public string DeviceSerial = "";                // Dispositivi.Seriale - Cicli.IDDispositivo
		public string DeviceDescription = "";           // Dispositivi.Descrizione - Cicli.IDDispositivo
		public string DeviceType = "";                  // TipiDispositivi.Descrizione - TipiDispositivi.ID - Cicli.IDDispositivo
		public string DeviceBrand = "";                 // Fornitori.Descrizione - Fornitori.ID - Cicli.IDDispositivo

		public string WasherCode = "";                  // Armadi_Lavatrici.Matricola - Cicli.IDSterilizzatrice
		public string WasherSerial = "";                // Armadi_Lavatrici.Seriale - Cicli.IDSterilizzatrice
		public string WasherDescription = "";           // Armadi_Lavatrici.Descrizione - Cicli.IDSterilizzatrice
		public string WasherType = "";                  // enum KleanTrak.Model.WasherStorageTypes - Armadi_Lavatrici.Tipo - Cicli.IDSterilizzatrice

		public string CycleNumber = "";                 // ? (scontrino "NUMERO CICLO")
		public string CycleType = "";                   // ? (scontrino "TIPO CICLO")
		public string CycleStartDateTime = "";          // ? (scontrino "INIZIO CICLO")
		public string CycleEndDateTime = "";            // ? (scontrino dataora ultima riga "CICLO REGOLARE")

		public string OperatorCode = "";                // Operatori.Matricola - CicliStatoLog.IDOperatore
		public string OperatorSurname = "";             // Operatori.Cognome - CicliStatoLog.IDOperatore
		public string OperatorName = "";                // Operatori.Nome - CicliStatoLog.IDOperatore

		public string StateOld = "";                    // Stato.Descrizione - CicliStatoLog.IDStatoOld
		public string StateNew = "";                    // Stato.Descrizione - CicliStatoLog.IDStatoNew
		public string StateChangedDateTime = "";        // CicliStatoLog.DataOra

	}
}
