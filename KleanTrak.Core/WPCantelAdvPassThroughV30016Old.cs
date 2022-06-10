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
    public class WPCantelAdvPassThroughV30016Old : WPCantelAdvPassThroughV30016
    {
        public WPCantelAdvPassThroughV30016Old() : base()
        {
        }

        protected override string GetExternalId(string[] fields)
        {
            if (fields.Length >= 2)
                return fields[1];
            else
                throw new ApplicationException("externalid not found");
        }
    }
}
