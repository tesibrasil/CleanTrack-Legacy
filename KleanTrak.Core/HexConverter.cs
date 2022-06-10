using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KleanTrak.Core
{
	/// <summary>
	/// Serve per convertire dei byte in stringhe che ne 
	/// rappresentano la visualizzazione esadecimale.
	/// Le funzioni statiche sono thread safe.
	/// </summary>
	public class HexConverter
	{
		#region PROPERTIES AND VARIABLES

		#endregion

		#region PRIVATE METHODS

		/// <summary>
		/// Riceve un byte ma che deve avere solo i 4 bit meno 
		/// significativi settati, il carattere hex viene calcolato
		/// su questi 4 bit.
		/// </summary>
		/// <param name="b"></param>
		/// <returns></returns>
		private string GetHexChar(byte b)
		{
			string strreturn = "";
			try
			{
				switch ((int)(b & 0x0F) % 16)
				{
					case 0:
					case 1:
					case 2:
					case 3:
					case 4:
					case 5:
					case 6:
					case 7:
					case 8:
					case 9:
						strreturn = ((int)(b & 0x0F) % 16).ToString();
						break;
					case 10:
						strreturn = "A";
						break;
					case 11:
						strreturn = "B";
						break;
					case 12:
						strreturn = "C";
						break;
					case 13:
						strreturn = "D";
						break;
					case 14:
						strreturn = "E";
						break;
					case 15:
						strreturn = "F";
						break;
				}

				return strreturn;
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		/// <summary>
		/// Converte la rappresentazione del caratter esadecimale 
		/// passata in argomento nel byte corrispondente
		/// </summary>
		/// <param name="c_4bit">
		/// Il char che rappresenta un carattere hex
		/// </param>
		/// <exception cref="ApplicationException">
		/// Nel caso non si riesca a convertire il char.
		/// </exception>
		private byte GetByte(char c_4bit)
		{
			switch (c_4bit)
			{
				case '0':
				case '1':
				case '2':
				case '3':
				case '4':
				case '5':
				case '6':
				case '7':
				case '8':
				case '9':
					return byte.Parse(c_4bit.ToString());
				case 'A':
					return 0x0A;
				case 'B':
					return 0x0B;
				case 'C':
					return 0x0C;
				case 'D':
					return 0x0D;
				case 'E':
					return 0x0E;
				case 'F':
					return 0x0F;
				default:
					throw new ApplicationException("HexConverter.HexConverter.GetByte: char not converted!");
			}

		}

		#endregion //PRIVATE METHODS

		#region PUBLIC METHODS
		/// <summary>
		/// Ritorna una stringa con la rappresentazione
		/// esadecimale del byte passato in argomento
		/// </summary>
		/// <exception cref="Exception">
		/// Rilancia le eccezioni con i parametri di chiamata alla funzione
		/// </exception>
		public string GetHexString(byte b)
		{
			try
			{
				string retstr = "";
				//prima i 4 bit più signficativi
				retstr = GetHexChar((byte)((b >> 4) & 0x0F));
				//poi aggiunge i 4 bit meno significativi
				retstr += GetHexChar((byte)(b & 0x0F));
				return retstr;
			}
			catch (Exception e)
			{
				throw new ApplicationException("HexConverter.GetHexString exception - byte: " + b, e);
			}
		}

		/// <summary>
		/// Ritorna i byte corrispondenti alla rappresentazione
		/// esadecimale passata come argomento
		/// </summary>
		/// <param name="hex"></param>
		/// <exception cref="ApplicationException">
		/// Rilancia le eccezioni con i parametri di chiamata alla funzione
		/// </exception>
		public List<byte> GetBytes(string hex)
		{
			try
			{
				//sistema la stringa di input
				hex = hex.ToUpper();
				//elimina tutti gli spazi
				string[] strsplit = hex.Split(' ');
				hex = "";

				foreach (string str in strsplit)
					hex += str;

				char[] hexchars = hex.ToCharArray();
				List<byte> retlist = new List<byte>(hex.Length / 2);

				//verifica che hexchars sia multiplo di 2
				if ((hexchars.Count() % 2) != 0)
					throw new ApplicationException("hex bytes not even!!!");

				for (int i = 0; i < hexchars.Count() - 1; i = i + 2)
				{
					retlist.Add((byte)(((GetByte(hexchars[i])) << 4) | GetByte(hexchars[i + 1])));
				}
				return retlist;
			}
			catch (Exception e)
			{
				throw new ApplicationException("HexConverter.GetBytes exception - hex: " + hex, e);
			}
		}

		/// <summary>
		/// Ritorna la rappresentazione stringa dei caratteri
		/// esadecimali che rappresentano i byte passati in 
		/// argomento
		/// </summary>
		/// <exception cref="ApplicationException">
		/// Rilancia le eccezioni e vi aggiunge i parametri di chiamata alla
		/// funzione.
		/// </exception>
		public string GetHexString(List<byte> bytelist)
		{
			try
			{
				if (bytelist == null || bytelist.Count == 0)
					return "";

				string retstring = "";
				foreach (byte b in bytelist)
					retstring += GetHexString(b) + " ";
				return retstring.Substring(0, retstring.Length - 1);
			}
			catch (Exception e)
			{
				throw new ApplicationException("HexConverter.GetHexString exception - bytelist.count: " + bytelist.Count, e);
			}
		}

		public string GetHexString(List<UInt16> shortlist)
		{
			try
			{
				if (shortlist.Count == 0)
					return "";

				string retstring = "";
				foreach (ushort s in shortlist)
					retstring += GetHexString(s) + " ";
				return retstring.Substring(0, retstring.Length - 1);
			}
			catch (Exception e)
			{
				throw new ApplicationException("HexConverter.GetHexString exception - shortlist.count: " + shortlist.Count, e);
			}
		}

		public string GetHexString(List<UInt32> intlist)
		{
			try
			{
				if (intlist.Count == 0)
					return "";

				string retstring = "";
				foreach (uint i in intlist)
					retstring += GetHexString(i) + " ";
				return retstring.Substring(0, retstring.Length - 1);
			}
			catch (Exception e)
			{
				throw new ApplicationException("HexConverter.GetHexString exception - intlist.count: " + intlist.Count, e);
			}
		}

		public string GetHexString(List<UInt64> longlist)
		{
			try
			{
				if (longlist.Count == 0)
					return "";

				string retstring = "";
				foreach (ulong l in longlist)
					retstring += GetHexString(l) + " ";
				return retstring.Substring(0, retstring.Length - 1);
			}
			catch (Exception e)
			{
				throw new ApplicationException("HexConverter.GetHexString exception - longlist.count: " + longlist.Count, e);
			}
		}

		public string GetHexString(UInt64 number)
		{
			return GetHexString(System.Convert.ToUInt32(0x00000000FFFFFFFF & number)) + " " +
					GetHexString(System.Convert.ToUInt32(0x00000000FFFFFFFF & number >> 32));
		}

		public string GetHexString(UInt32 number)
		{
			return GetHexString(System.Convert.ToUInt16(0x0000FFFF & number)) + " " +
					GetHexString(System.Convert.ToUInt16(0x0000FFFF & number >> 16));
		}

		public string GetHexString(UInt16 number)
		{
			return GetHexString(new List<byte>
				{
					System.Convert.ToByte(0x00FF & number),
					System.Convert.ToByte(0x00FF & number >> 8)
				});
		}

		#endregion //PUBLIC METHODS

	}
}
