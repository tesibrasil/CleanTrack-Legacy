using System;
using System.Collections.Generic;
using System.Linq;

namespace KleanTrak.Model
{
    public class DictionaryBase : ObjectSerializeHelper
    {
        public SerializableDictionary<string, string> InternalList { set; get; }

        public enum Languages
        {
            ITA,
            ENG,
            ESP,
            POR
        };

        public static Languages Language { set; get; }

        public DictionaryBase()
        {
            InternalList = new SerializableDictionary<string, string>();
            InitializeStrings();
        }

        public string this[string index]
        {
            get
            {
                string ret = "";
                try
                {
                    ret = InternalList[index + "_" + Language.ToString()];
                }
                catch (Exception)
                {
                    try
                    {
                        ret = InternalList[index + "_ITA"];
                    }
                    catch (Exception)
                    {
                        return index + "_" + Language.ToString();
                    }
                }

                return ret;
            }
        }

        private void InitializeStrings()
        {
            InternalList.Add("barcodeNotFound_ITA", "Barcode non riconosciuto!");
            InternalList.Add("operationNotFound_ITA", "Operazione non trovata!");
            InternalList.Add("deviceNotFound_ITA", "Dispositivo non trovato!");
            InternalList.Add("operatorNotFound_ITA", "Operazione non trovata!");
            InternalList.Add("stateNotValid2_ITA", "Stato attuale dispositivo non valido!\r\nBarcode: {0:s}\r\nSonda: {1:s}\r\nStato attuale: {2:s}\r\nStato atteso: {3:s}");
            InternalList.Add("errorGettingServiceName_ITA", "Errore nel reperimento del nome del servizio");
            InternalList.Add("serviceNotInstalled_ITA", "Servizio non installato!");
            InternalList.Add("serviceAlreadyInstalled_ITA", "Il servizio risulta già installato!");
            InternalList.Add("setDbConnectionString_ITA", "Impostare la stringa di connessione al db!");
            InternalList.Add("httpClosed_ITA", "Endpoint {0:s} close");
            InternalList.Add("cycle_already_present_ITA", "La sonda è già associata a questo esame.");

            // Pi
            InternalList.Add("piWorklistTitle_ITA", "Worklist interventi");
            InternalList.Add("piSelectWorklistItem_ITA", "Seleziona intervento");
            InternalList.Add("piDeviceConfigurationNeeded_ITA", "E' necessario configurare il device!");
            InternalList.Add("piNotSpecified_ITA", "Non specificato");
            InternalList.Add("piCantConnectToServer_ITA", "Impossibile contattare il server");
            InternalList.Add("piDeviceAddress_ITA", "Indirizzo dispositivo");
            InternalList.Add("piServerAddress_ITA", "Indirizzo server");
            InternalList.Add("piError_ITA", "ERRORE!");
            InternalList.Add("piOperationAlreadySelected_ITA", "Operazione già selezionata!");
            InternalList.Add("piDeviceAlreadySelected_ITA", "Sonda già selezionata!");
            InternalList.Add("piOperatorAlreadySelected_ITA", "Utente già selezionato!");
            InternalList.Add("piAccessoryAlreadySelected_ITA", "Accesorio già selezionato!");
            InternalList.Add("piWaitingDevice_ITA", "In attesa barcode sonda");
            InternalList.Add("piConfirm_ITA", "Conferma");
            InternalList.Add("piOperation_ITA", "Operazione");
            InternalList.Add("piWaiting_ITA", "Dispositivo");
            InternalList.Add("piDevice_ITA", "Dispositivo");
            InternalList.Add("piActualState_ITA", "Stato attuale");
            InternalList.Add("piOperator_ITA", "Operatore");
            InternalList.Add("piUpdating_ITA", "Aggiornamento in corso...");
            InternalList.Add("piUpdated_ITA", "Aggiornamento concluso");
            InternalList.Add("piNextStatusError_ITA", "Operazione fuori flusso");            
            InternalList.Add("piBarcodeNotFound_ITA", "Barcode non trovato");

            AddOtherLanguageDefault();
        }

        private void AddOtherLanguageDefault()
        {
            Dictionary<string, string> newList = new Dictionary<string, string>();
            foreach (var item in InternalList)
            {
                newList.Add(item.Key, item.Value);
                newList.Add(item.Key.Substring(0, item.Key.Length - 3) + "ENG", item.Value);
                newList.Add(item.Key.Substring(0, item.Key.Length - 3) + "ESP", item.Value);
                newList.Add(item.Key.Substring(0, item.Key.Length - 3) + "POR", item.Value);
            }

            InternalList.Clear();
            foreach (var item in newList)
                InternalList.Add(item.Key, item.Value);
        }

        public void UpdateWithDefaultValues()
        {
            DictionaryBase defaultDictionary = new DictionaryBase();
            foreach (var defItem in defaultDictionary.InternalList)
            {
                var query = from item in InternalList where item.Key == defItem.Key select item;
                if (query.Count() == 0)
                {
                    string value = "";
                    if (InternalList.TryGetValue(defItem.Key, out value))
                        InternalList.Remove(defItem.Key);

                    InternalList.Add(defItem.Key, defItem.Value);
                }
            }
        }
    }
}
