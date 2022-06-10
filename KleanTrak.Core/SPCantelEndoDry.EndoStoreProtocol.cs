using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace KleanTrak.Core
{
	/*
	 -<Events Header="Evento durante l'operazione:">

-<Event Caption="Evento:" Time="Data / Orario" AdditionalContent="Informazioni aggiuntive" Description="Descrizione">

<Time>2021-05-07T16:54:08</Time>

<Description>Pressione aria OK</Description>

<AdditionalContent>---</AdditionalContent>

</Event>

</Events>
	 */
	public partial class SPCantelEndoDry
	{
		public class Event
		{
			[XmlElement(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, DataType = "dateTime")]
			public DateTime Time { get; set; }
			public string Description { get; set; }
			public string AdditionalContent { get; set; }
		}
		public class HIS
		{
			public IN IN { get; set; } = null;
			public OUT OUT { get; set; } = null;
		}
		public class IN
		{
			public EndoStore EndoStore { get; set; }
			public Owner Owner { get; set; }
			public Instrument Instrument { get; set; }
			public PutIn PutIn { get; set; }
			public Processdata Processdata { get; set; }
			public List<Event> Events { get; set; }
		}
		public class OUT
		{
			public EndoStore EndoStore { get; set; }
			public Owner Owner { get; set; }
			public Instrument Instrument { get; set; }
			public PutIn PutIn { get; set; }
			public TakeOut TakeOut { get; set; }
			public Processdata Processdata { get; set; }
			public List<Event> Events { get; set; }
		}
		public class EndoStore
		{
			/// <summary>
			/// Numero seriale
			/// </summary>
			public string SerialNo { get; set; } = "";
			/// <summary>
			/// Numero gruppo
			/// </summary>
			public int GroupID { get; set; } = 0;
			/// <summary>
			/// Lato di carico
			/// </summary>
			public string Side { get; set; } = "";
			/// <summary>
			/// Posizione di stoccaggio numero
			/// </summary>
			public int StorageUnitNo { get; set; } = 0;
		}
		/// <summary>
		/// Proprietario
		/// </summary>
		public class Owner
		{
			/// <summary>
			/// Nome
			/// </summary>
			public string Name { get; set; } = "";
			/// <summary>
			/// Dipartimento
			/// </summary>
			public string Department { get; set; } = "";
			/// <summary>
			/// Via
			/// </summary>
			public string Street { get; set; } = "";
			/// <summary>
			/// Numero civico
			/// </summary>
			public string Number { get; set; } = "";
			/// <summary>
			/// Cap
			/// </summary>
			public string ZIP { get; set; } = "";
			/// <summary>
			/// Città
			/// </summary>
			public string City { get; set; } = "";
		}
		/// <summary>
		/// Endoscopio
		/// </summary>
		public class Instrument
		{
			/// <summary>
			/// Codice a barre
			/// </summary>
			public string Barcode { get; set; } = "";
			/// <summary>
			/// Nome
			/// </summary>
			public string Name { get; set; } = "";
			/// <summary>
			/// Tempo di stoccaggio massimo in ore
			/// </summary>
			public int MaxStorageTime { get; set; } = 0;
		}
		/// <summary>
		/// Carico
		/// </summary>
		public class PutIn
		{
			/// <summary>
			/// Data / Orario
			/// </summary>
			[XmlElement(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, DataType = "dateTime")]
			public DateTime Time { get; set; }
			/// <summary>
			/// Nome utente
			/// </summary>
			public string UserName { get; set; } = "";
			/// <summary>
			/// Codice a barre utente
			/// </summary>
			public string UserBarcode { get; set; } = "";
		}
		/// <summary>
		/// Scarico
		/// </summary>
		public class TakeOut
		{
			/// <summary>
			/// Data / Orario
			/// </summary>
			public DateTime Time { get; set; }
			/// <summary>
			/// Nome utente
			/// </summary>
			public string UserName { get; set; } = "";
			/// <summary>
			/// Codice a barre utente
			/// </summary>
			public string UserBarcode { get; set; } = "";
		}
		/// <summary>
		/// Dati di processo
		/// </summary>
		public class Processdata
		{
			/// <summary>
			/// Informazioni sul paziente - patient barcode 
			/// </summary>
			public string Patientinfo { get; set; } = "";
			/// <summary>
			/// Check di sicurezza
			/// 0..6
			/// 0: Add on not activated or undefined
			/// 1: no connection to washer disinfection
			/// 2: approval from washer disinfector
			/// 3: FFU
			/// 4: FFU
			/// 5: FFU
			/// 6: no approval from washer disinfector
			/// </summary>
			public int SecurityCheck { get; set; } = 0;
			/// <summary>
			/// Reprocessing cycel number from washing disinfector
			/// </summary>
			public string WdRunCycle { get; set; } = "";
			/// <summary>
			/// Cycle number of endostore
			/// </summary>
			public string ActualRunCycle { get; set; } = "";
			/// <summary>
			/// Process result 0...3
			/// 0: drying process finished
			/// 1: unloading during drying process
			/// 2: FAILURE, maximum storage time exceeded 
			///    or pressure malfunction during storage
			/// 3: FFU
			/// </summary>
			public int ProcessResult { get; set; } = 0;
			/// <summary>
			/// Possible cycle number of the previous endostore
			/// </summary>
			public string LastRunCycle { get; set; } = "";
			/// <summary>
			/// Total storge time xd hh:mm:ss 
			/// xd: number of days
			/// hh: hours
			/// mm: minutes
			/// ss: seconds
			/// </summary>
			public string Runtime { get; set; } = "";
		}
	}
}
