using System;
using System.Data;
using System.Data.Odbc;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;

namespace KleanTrak
{
    public class Globals
    {
        public static string Query_ricerca = "";
        public static int checkutente = -1;
        public static int Insert_check_utente = -1;
        public static string Query_log_Transazioni = "";
        public static string log_OrderBy = "";
        public static int IDSEDE = 1;
        public static string DESCR_SEDE = "";

        public static bool ReadOnly = false;
        public static bool bBadgeActive = false;
        public static string strBadgeAddressIP = "";
        public static uint iBadgeAddressPort = 0;
        public static uint iBadgeTimeout = 0;
        public static uint TempoLavaggio;

        public static string strDatabase = "";

        public static uint m_iUserID = 0;
        public static string m_strUser = "";
        public static bool su_user_mode = false;
        public static ulong m_iUserPermission = 0;

        public static string ServerHttpEndpoint = "";

        public static string[] strTable = new string[] {
            "Eliminare la causale selezionata?",                                                                    //   0
			"Impossibile eliminare la causale perchè già in uso!",                                                  //   1
			"Eliminare la tipologia di dispositivo selezionato?",                                                   //   2
			"Impossibile eliminare la causale perchè già in uso!",                                                  //   3
			"Password errata",                                                                                      //   4
			"Password errata",                                                                                      //   5
			"Problema durante la connessione al database:\n",                                                       //   6
			"\n\nImpostare la connessione e riavviare l'applicazione.",                                             //   7
			"Problema durante la connessione al database del dizionario:\n",                                        //   8
			"Chiave HASP non presente!",                                                                            //   9
			"La chiave presente non è una MemoHASP!",                                                               //  10
			"La chiave presente è una MemoHASP ma non è programmata correttamente!",                                //  11
			"Soltanto un utente amministratore può aprire la gestione utenti",                                      //  12
			"Soltanto un utente amministratore può aprire la gestione utenti",                                      //  13
			"Connessione avvenuta con successo!",                                                                   //  14
			"RFID gia utilizzato! Impossibile aggiungere il dispositivo!",                                          //  15
			"\n\nFormato data non corretto",                                                                        //  16
			"Formato data non valido!",                                                                             //  17
			"Nessun dispositivo selezionato!",                                                                      //  18
			"Ripristinare il dispositivo dismesso ",                                                                //  19
			"Nessun dispositivo selezionato!",                                                                      //  20
			"Ricerca abbandonata: nessun risultato",                                                                //  21
			"Eliminare il dispositivo selezionato?",                                                                //  22
			"Impossibile eliminare il dispositivo selezionato perchè già in uso!",                                  //  23
			"Chiudere le precedenti finestre prima di continuare!",                                                 //  24
			"Chiudere le precedenti finestre prima di continuare!",                                                 //  25
			"Nessun dispositivo dismesso",                                                                          //  26
			"Inserire la matricola per ogni dispositivo!",			                                                //  27
			"Eliminare il dispositivo selezionato?",                                                                //  28
			"Impossibile eliminare il dispositivo selezionato perchè già in uso!",                                  //  29
			"L'utente risulta disattivato!\n\nImpossibile continuare.",                                             //  30
			"Utente disattivato!\n\nRipetere l'operazione",                                                         //  31
			"Operatore non identificato!\n\nRipetere l'operazione.",                                                //  32
			"Sterilizzatrice non identificata!\n\nRipetere l'operazione.",                                          //  33
			"IL DISPOSITIVO NON E' ANCORA STATO UTILIZZATO!!!\n\nIMPOSSIBILE PROCEDERE!",                           //  34
			"LA STERILIZZAZIONE DEL DISPOSITIVO NON E' ANCORA TERMINATA!!!\n\nIMPOSSIBILE PROCEDERE!",              //  35
			"Dispositivo non identificato!\n\nRipetere l'operazione.",                                              //  36
			"Eliminare il fornitore selezionato?",                                                                  //  37
			"Impossibile eliminare la causale perchè già in uso!",                                                  //  38
			"Eliminare il fornitore selezionato?",                                                                  //  39
			"Impossibile eliminare la causale perchè già in uso!",                                                  //  40
			"Data inizio utilizzo non valida!",                                                                     //  41
			"Data inizio utilizzo non valida!",                                                                     //  42
			"Data fine utilizzo non valida!",                                                                       //  43
			"Data fine utilizzo non valida!",                                                                       //  44
			"Le data di inzio non deve essere superiore alla data di fine!",                                        //  45
			"Inserire almeno un parametro per la ricerca!",                                                         //  46
			"Impossibile leggere il timer da database!",                                                            //  47
			"Valore di timer non valido!",                                                                          //  48
			"Codice gia utilizzato! Impossibile aggiungere la sterilizzatrice!",                                    //  49
			"Selezionare una sterilizzatrice!",                                                                     //  50
			"Eliminare la sterilizzatrice selezionata?",                                                            //  51
			"Impossibile eliminare la sterilizzatrice perchè già in uso!",                                          //  52
			"Ripristinare la sterilizzatrice dismessa?",                                                            //  53
			"Nessun dispositivo selezionato!",                                                                      //  54
			"Nessun dispositivo selezionato!",                                                                      //  55
			"Nessuna sterilizzatrice dismessa",                                                                     //  56
			"Eliminare la sterilizzatrice selezionata?",                                                            //  57
			"Impossibile eliminare la sterilizzatrice perchè già in uso!",                                          //  58
			"Utente disattivato!\n\nRipetere l'operazione",                                                         //  59
			"Utente disattivato!\n\nRipetere l'operazione",                                                         //  60
			"Operatore non identificato!\n\nRipetere l'operazione.",                                                //  61
			"IL DISPOSITIVO NON E' ANCORA STATO UTILIZZATO!!!\n\nIMPOSSIBILE PROCEDERE!",                           //  62
			"LA STERILIZZAZIONE DEL DISPOSITIVO NON E' ANCORA INIZIATA!!!\n\nIMPOSSIBILE PROCEDERE!",               //  63
			"Dispositivo non identificato!\n\nRipetere l'operazione.",                                              //  64
			"Eliminare il tipo di dispositivo selezionato?",                                                        //  65
			"Eliminare la tipologia di dispositivo selezionato?",                                                   //  66
			"Impossibile eliminare il tipo dispositivo perchè già in uso!",                                         //  67
			"Selezionare un tipo dispositivo!",                                                                     //  68
			"Eliminare la tipologia di dispositivo selezionato?",                                                   //  69
			"Impossibile eliminare il tipo dispositivo perchè già in uso!",                                         //  70
			"Utente disattivato!\n\nRipetere l'operazione",                                                         //  71
			"Utente disattivato!\n\nRipetere l'operazione",                                                         //  72
			"Operatore non identificato!\n\nRipetere l'operazione",						                            //  73
			"IL DISPOSITIVO E' ANCORA SPORCO!\n\nIMPOSSIBILE PROCEDERE CON L'ESAME!",                               //  74
			"LA STERILIZZAZIONE DEL DISPOSITIVO NON E' ANCORA CONCLUSA!\n\nIMPOSSIBILE PROCEDERE CON L'ESAME!",     //  75
			"Dispositivo non identificato!\n\nRipetere l'operazione.",                                              //  76
			"Salvataggio larghezza colonne riuscito!",                                                              //  77
			"Codice gia utilizzato! Impossibile aggiungere l'operatore.",                                           //  78
			"Eliminare l'operatore selezionato?",                                                                   //  79
			"Impossibile eliminare l'operatore selezionato perchè già in uso!",                                     //  80
			"Impossibile eliminare l'operatore selezionato perchè già in uso!",                                     //  81
			"Riattivare l'operatore selezionato?",                                                                  //  82
			"Disattivare l'operatore selezionato?",                                                                 //  83
			"Chiudere le precedenti finestre prima di continuare!",                                                 //  84
			"Chiudere le precedenti finestre prima di continuare!",                                                 //  85
			"Chiudere le precedenti finestre prima di continuare!",                                                 //  86
			"Chiudere le precedenti finestre prima di continuare!",                                                 //  87
			"Selezionare un operatore!",                                                                            //  88
			"Eliminare l'operatore selezionato?",                                                                   //  89
			"Impossibile eliminare l'operatore perchè ha già effettuato esami!",                                    //  90
			"Impossibile eliminare l'operatore perchè ha già effettuato esami!",                                    //  91
			"Nessuna chiave di ricerca inserita. Procedere?",                                                       //  92
			"Definire Cognome e Nome dell'operatore!",                                                              //  93
			"Soltanto un utente amministratore può aprire il Log delle operazioni",                                 //  94
			"Selezionare un operatore dalla lista per associare il TAG!",                                           //  95
			"Associare Il TAG all'operatopre selezionato?",                                                         //  96
			"Sovrascrivere il codice TAG associato all'operatore selezionato?",                                     //  97
			"Attenzione, la durata del lavaggio è inferiore a quanto impostato. Si desidera procedere?",            //  98
			"Impossibile leggere il tempo di lavaggio da database!",                                                //  99
			"Inserire solo valori numerici interi!",                                                                // 100
			"Pin inserito non valido. Cambiare il nuovo Pin inserito",                                              // 101
			"Il dispositivo ha superato il tempo massimo di stoccaggio e deve essere sterilizzato",                 // 102
			"  -- L' operatore ",                                                                                   // 103
			" ha tentato di utilizzare il dispositivo ",                                                            // 104
			" che risulta in stato ",                                                                               // 105
			"sporco.",                                                                                              // 106
			"in lavaggio.",                                                                                         // 107
			"scaduto.",                                                                                             // 108
			"Impossibile terminare la sterilizzazione",                                                             // 109
			"Sterilizzazione completata del dispositivo ",                                                          // 110
			" da parte dell' operatore ",                                                                           // 111
			"Tentativo di utilizzo scorretto del dispositivo ",                                                     // 112
			"Sterilizzazione del dispositivo ",                                                                     // 113
            "Impossibile procedere alla sterilizzazione",                                                           // 114
            "Conferma tra ",                                                                                        // 115
			" secondi",																			                    // 116
            "CLEAN TRACK - Dispositivi in uso",                                                                     // 117
            "Dispositivi in uso",                                                                                   // 118 
            "Dispositivi dismessi",                                                                                 // 119
            "Data ultimo utilizzo",                                                                                 // 120
            "CLEAN TRACK - Dispositivi dismessi",                                                                   // 121
            "Operatore",                                                                                            // 122
			"Problema di localizzazione!\n\n",																		// 123
			"Errore durante il caricamento di AmLogin.dll e AmLocalize.dll!\n\n",									// 124
			"Errore in AmLogin.dll\n\n",																			// 125
			"Impostare indirizzo server in tabella configurazioni!",												// 126
			"Operatore non abilitato.",																				// 127
            "Selezionare un dispositivo",                                                                           // 128
            "Impossibile eliminare operatore sconosciuto",                                                          // 129
            "Selezionare armadio o lavatrice",                                                                      // 130
            "Data Scontrino",                                                                                       // 131
            "File Scontrino",                                                                                       // 132
            "La data selezionata non è congruente",                                                                 // 133
            "Selezionare una data dal calendario",                                                                  // 134
            "Ok",                                                                                                   // 135
            "Cancel",                                                                                               // 136
            "La versione del database non corrisponde a quella dell'applicativo",                                   // 137
            "Si",																									// 138
            "No",																									// 139
            "Selezione Mancante",																					// 140
            "Selezionare almeno un utente",																			// 141
			"Risultati trovati",																					// 142
			"Dismissione",																							// 143
			"Sicuri di voler dismettere il dispositivo?",															// 144
			"Aggiunge un nuovo dispositivo",																		// 145
			"Elimina il dispositivo selezionato",																	// 146
			"Associa un tag rfid al dispositivo selezionato",														// 147
			"Dismette il dispositivo selezionato",																	// 148
			"Ripristina il dispositivo selezionato",																// 149
			"Visualizza i dispositivi attivi",																		// 150
			"Visualizza i dispositivi dismessi",																	// 151
			"Esegue una ricerca sui dispositivi",																	// 152
			"Visualizza il registro del dispositivo selezionato",													// 153
			"Stampa la visualizzazione corrente",																	// 154
			"Chiude la finestra",																					// 155
			"Nessun ciclo disponibile",																				// 156
			"report ciclo sterilizzazione",																			// 157
			"strumento",																							// 158
			"categoria",																							// 159
			"matricola",																							// 160
			"dati ciclo corrente",																					// 161
			"esame",																								// 162
			"sala esame",																							// 163
			"dati ciclo precedente",																				// 164
			"dati estesi",																							// 165
			"ciclo",																								// 166
			"Desideri davvero eliminare la transazione selezionata?",												// 167
			"ATTENZIONE",																							// 168
			"Eliminazione fallita",																					// 169
			"Riavvia CleanTrack e CleanTrackServer per rendere effettive le modifiche!",							// 170
			"Inserimento fallito",																					// 171
			"Elimina la transazione selezionata",																	// 172
			"Aggiunge una nuova transazione di stato",																// 173
			"Aggiunge una nuova causale di dismissione",															// 174
			"Elimina la causale di dismissione selezionata",														// 175
			"Aggiunge un nuovo tipo dispositivo",																	// 176
			"Elimina il tipo dispositivo selezionato",																// 177
			"Operatori attivi",																						// 178
			"Operatori disabilitati",																				// 179
			"Aggiungi operatore",																					// 180
			"Elimina operatore",																					// 181
			"Disabilita operatore",																					// 182
			"Abilita operatore",																					// 183
			"Associa tag",																							// 184
			"Chiude la finestra",																					// 185
			"Disattivato",																							// 186
			"Tag rfid già assegnato all'operatore",																	// 187
			"Dismesso",																								// 188
			"Selezionare un operatore dalla lista",																	// 189
			"Aggiunge un nuovo macchinario",																		// 190
			"Elimina il macchinario selezionato",																	// 191
			"Dismette macchinario selezionato",																		// 192
			"Riattiva macchinario selezionato",																		// 193
			"Mostra macchinari attivi",																				// 194
			"Mostra macchinari dismessi",																			// 195
			"Si desidera veramente dismettere il macchinario selezionato?",											// 196
			"Spiacenti si è verificato un errore",																	// 197
			"Si desidera riattivare il macchinario selezionato?",													// 198
			"Aggiunge lettore",																						// 199
			"Elimina lettore",																						// 200
			"Invia configurazione al lettore",																		// 201
			"Chiude la finestra",																					// 202
			"Si desidera veramente eliminare il lettore selezionato?",												// 203
			"Aggiunge tipo scadenza",																				// 204
			"Elimina tipo scadenza",																				// 205
			"Chiude la finestra",																					// 206
			"Matricola già presente in archivio",																	// 207
			"Seriale già presente in archivio",																		// 208
			"Tag già presente in archivio",																			// 209
			"Barcode già presente in archivio",																		// 210
			"Aggiungi nuovo stato",																					// 211
			"Elimina stato",																						// 212
			"Modifica stato",																						// 213
			"Configurare almeno un tipo di dispositivo",															// 214
			"Il codice non corrisponde ad un dispositivo",															// 215
			"Il codice non corrisponde ad un operatore",															// 216
			"Riavviare Client e SERVER per applicare le modifiche!",												// 217
            "Id ciclo sterilizzatrice",                                                                             // 218
            "Esito ciclo",                                                                                                // 219
            "Fallito"                                                                                               // 220
		};
        public static void CaricaStringTable()
        {
            OdbcConnection connTemp = new OdbcConnection(KleanTrak.Globals.strDictionary);
            connTemp.Open();
            OdbcCommand commTemp = new OdbcCommand("", connTemp);
            OdbcDataReader readerTemp;
            for (int i = 0; i < strTable.Length; i++)
            {
                if ((strTable[i] != null) && (strTable[i].Length > 0))
                {
                    commTemp.CommandText = "SELECT * FROM Dictionary WHERE IDMessaggio='strTable." + i.ToString("D2") + "' AND Forms='StringTable'";
                    readerTemp = commTemp.ExecuteReader();

                    if (readerTemp.Read())
                    {
                        string strTemp;
                        if (readerTemp.IsDBNull(nColonnaLingua[nLinguaInUso]))
                            strTemp = readerTemp.GetString(nColonnaLingua[0]);
                        else
                            strTemp = readerTemp.GetString(nColonnaLingua[nLinguaInUso]);
                        if (strTemp.Length > 0)
                            strTable[i] = strTemp;
                        if ((readerTemp != null) && (readerTemp.IsClosed == false))
                            readerTemp.Close();
                    }
                    else
                    {
                        if ((readerTemp != null) && (readerTemp.IsClosed == false))
                            readerTemp.Close();

                        try
                        {
                            commTemp.CommandText = "INSERT INTO Dictionary(IDMessaggio, Forms, New, ITA) VALUES('strTable." + i.ToString("D2") + "', 'StringTable', 1, '" + strTable[i].Replace("'", "''") + "')";
                            commTemp.ExecuteNonQuery();
                        }
                        catch (Exception exc)
                        {
                            MessageBox.Show(exc.Message);
                        }

                    }

                    if ((readerTemp != null) && (readerTemp.IsClosed == false))
                        readerTemp.Close();

                }
            }
            if (connTemp.State == ConnectionState.Open)
                connTemp.Close();

            connTemp.Dispose();

        }

        public enum LINGUA
        {
            ITA,
            ENG,
            ESP,
            POR,

            TOTALELINGUE
        }

        public static void Log(Exception e, params string[] log_lines) =>
            LibLog.Logger.Get().Write(e, Globals.GetLocalIPAddress(), log_lines);
        public static void WarnAndLog(Exception e, params string[] p)
        {
            MessageBox.Show(e.ToString(), "Clean Track", MessageBoxButtons.OK, MessageBoxIcon.Error);
            Log(e, p);
        }

        public static int[] nColonnaLingua = new int[(int)LINGUA.TOTALELINGUE];
        public static string strDictionary = "";
        public static int nLinguaInUso = 0;
        public static int MaxStockVal = 0;
        public static int ConfirmTimer = 3;
        public static bool EnableConfirmTimer = true;

        public static void LocalizzaDialog(System.Windows.Forms.Form form)
        {
            OdbcConnection connTemp = new OdbcConnection(KleanTrak.Globals.strDictionary);
            connTemp.Open();

            Globals.AggiornaTitle(form, connTemp);

            for (int i = 0; i < form.Controls.Count; i++)
            {
                if (form.Controls[i].GetType().FullName == "System.Windows.Forms.Panel")
                {
                    LocalizzaDialog(form.Controls[i].Controls, form);
                }
                else
                {

                    if (form.Controls[i].GetType().FullName == "System.Windows.Forms.Label")
                    {
                        Globals.AggiornaLabel((System.Windows.Forms.Label)form.Controls[i], form, connTemp);
                    }
                    else
                    {
                        if (form.Controls[i].GetType().FullName == "System.Windows.Forms.Button")
                        {
                            Globals.AggiornaButton((System.Windows.Forms.Button)form.Controls[i], form, connTemp);
                        }
                        else
                        {
                            if (form.Controls[i].GetType().FullName == "ListViewEx.ListViewEx")
                            {
                                Globals.AggiornaListViewEx((ListViewEx.ListViewEx)form.Controls[i], form, connTemp);
                            }
                            else
                            {
                                if (form.Controls[i].GetType().FullName == "System.Windows.Forms.GroupBox")
                                {
                                    Globals.AggiornaGroupBox((System.Windows.Forms.GroupBox)form.Controls[i], form, connTemp);
                                }
                            }
                        }
                    }
                }
            }

            Globals.AggiornaMenu(form, connTemp);

            if (connTemp.State == ConnectionState.Open)
                connTemp.Close();

            connTemp.Dispose();
        }

        private static void LocalizzaDialog(Control.ControlCollection controls, Form parentForm)
        {
            OdbcConnection connTemp = new OdbcConnection(KleanTrak.Globals.strDictionary);
            connTemp.Open();


            for (int i = 0; i < controls.Count; i++)
            {
                if (controls[i].GetType().FullName == "System.Windows.Forms.Panel")
                {
                    LocalizzaDialog(controls[i].Controls, parentForm);
                }
                else
                {

                    if (controls[i].GetType().FullName == "System.Windows.Forms.Label")
                    {
                        Globals.AggiornaLabel((System.Windows.Forms.Label)controls[i], parentForm, connTemp);
                    }
                    else
                    {
                        if (controls[i].GetType().FullName == "System.Windows.Forms.Button")
                        {
                            Globals.AggiornaButton((System.Windows.Forms.Button)controls[i], parentForm, connTemp);
                        }
                        else
                        {
                            if (controls[i].GetType().FullName == "ListViewEx.ListViewEx")
                            {
                                Globals.AggiornaListViewEx((ListViewEx.ListViewEx)controls[i], parentForm, connTemp);
                            }
                            else
                            {
                                if (controls[i].GetType().FullName == "System.Windows.Forms.GroupBox")
                                {
                                    Globals.AggiornaGroupBox((System.Windows.Forms.GroupBox)controls[i], parentForm, connTemp);
                                }
                            }
                        }
                    }
                }
            }

            //Globals.AggiornaMenu(form, connTemp);

            if (connTemp.State == ConnectionState.Open)
                connTemp.Close();

            connTemp.Dispose();
        }

        private static void AggiornaButton(System.Windows.Forms.Button button, System.Windows.Forms.Form form, OdbcConnection connTemp)
        {
            if (button.Text.Length > 0)
            {
                OdbcCommand commTemp = new OdbcCommand("SELECT * FROM Dictionary WHERE IDMessaggio='" + button.Name.Replace("'", "''") + "' AND Forms='" + form.Name.Replace("'", "''") + "'", connTemp);
                OdbcDataReader readerTemp = commTemp.ExecuteReader();

                if (readerTemp.Read())
                {
                    string strTemp = "";
                    if (readerTemp.IsDBNull(nColonnaLingua[nLinguaInUso]))
                        strTemp = readerTemp.GetString(nColonnaLingua[0]);
                    else
                        strTemp = readerTemp.GetString(nColonnaLingua[nLinguaInUso]);
                    if (strTemp.Length > 0)
                        button.Text = strTemp;
                    if ((readerTemp != null) && (readerTemp.IsClosed == false))
                        readerTemp.Close();
                }
                else
                {
                    if ((readerTemp != null) && (readerTemp.IsClosed == false))
                        readerTemp.Close();

                    try
                    {
                        string text = button.Text.Replace("'", "''");
                        commTemp.CommandText = "INSERT INTO Dictionary(IDMessaggio, Forms, New, ITA) VALUES('" + button.Name.Replace("'", "''") + "', '" + form.Name.Replace("'", "''") + "', 1, '" + text + "')";
                        commTemp.ExecuteNonQuery();
                    }
                    catch (Exception exc)
                    {
                        MessageBox.Show(exc.Message);
                    }
                }
            }
        }

        private static void AggiornaGroupBox(System.Windows.Forms.GroupBox groupbox, System.Windows.Forms.Form form, OdbcConnection connTemp)
        {
            if (groupbox.Text.Length > 0)
            {
                OdbcCommand commTemp = new OdbcCommand("SELECT * FROM Dictionary WHERE IDMessaggio='" + groupbox.Name.Replace("'", "''") + "' AND Forms='" + form.Name.Replace("'", "''") + "'", connTemp);
                OdbcDataReader readerTemp = commTemp.ExecuteReader();

                if (readerTemp.Read())
                {
                    string strTemp = "";
                    if (readerTemp.IsDBNull(nColonnaLingua[nLinguaInUso]))
                        strTemp = readerTemp.GetString(nColonnaLingua[0]);
                    else
                        strTemp = readerTemp.GetString(nColonnaLingua[nLinguaInUso]);
                    if (strTemp.Length > 0)
                        groupbox.Text = strTemp;
                    if ((readerTemp != null) && (readerTemp.IsClosed == false))
                        readerTemp.Close();
                }
                else
                {
                    if ((readerTemp != null) && (readerTemp.IsClosed == false))
                        readerTemp.Close();

                    try
                    {
                        commTemp.CommandText = "INSERT INTO Dictionary(IDMessaggio, Forms, New, ITA) VALUES('" + groupbox.Name.Replace("'", "''") + "', '" + form.Name.Replace("'", "''") + "', 1, '" + groupbox.Text.Replace("'", "''") + "')";
                        commTemp.ExecuteNonQuery();
                    }
                    catch (Exception exc)
                    {
                        MessageBox.Show(exc.Message);
                    }
                }
            }

            for (int i = 0; i < groupbox.Controls.Count; i++)
            {
                if (groupbox.Controls[i].GetType().FullName == "System.Windows.Forms.Label")
                {
                    Globals.AggiornaLabel((System.Windows.Forms.Label)groupbox.Controls[i], form, connTemp);
                }
                else
                {
                    if (groupbox.Controls[i].GetType().FullName == "System.Windows.Forms.Button")
                    {
                        Globals.AggiornaButton((System.Windows.Forms.Button)groupbox.Controls[i], form, connTemp);
                    }
                    else
                    {
                        if (groupbox.Controls[i].GetType().FullName == "ListViewEx.ListViewEx")
                        {
                            Globals.AggiornaListViewEx((ListViewEx.ListViewEx)groupbox.Controls[i], form, connTemp);
                        }
                        else
                        {
                            if (groupbox.Controls[i].GetType().FullName == "System.Windows.Forms.GroupBox")
                            {
                                Globals.AggiornaGroupBox((System.Windows.Forms.GroupBox)groupbox.Controls[i], form, connTemp);
                            }
                        }
                    }
                }
            }
        }

        private static void AggiornaLabel(System.Windows.Forms.Label label, System.Windows.Forms.Form form, OdbcConnection connTemp)
        {
            if (label.Text.Length > 0)
            {
                OdbcCommand commTemp = new OdbcCommand("SELECT * FROM Dictionary WHERE IDMessaggio='" + label.Name.Replace("'", "''") + "' AND Forms='" + form.Name.Replace("'", "''") + "'", connTemp);
                OdbcDataReader readerTemp = commTemp.ExecuteReader();

                if (readerTemp.Read())
                {
                    string strTemp = "";
                    if (readerTemp.IsDBNull(nColonnaLingua[nLinguaInUso]))
                        strTemp = readerTemp.GetString(nColonnaLingua[0]);
                    else
                        strTemp = readerTemp.GetString(nColonnaLingua[nLinguaInUso]);
                    if (strTemp.Length > 0)
                        label.Text = strTemp;
                    if ((readerTemp != null) && (readerTemp.IsClosed == false))
                        readerTemp.Close();
                }
                else
                {
                    if ((readerTemp != null) && (readerTemp.IsClosed == false))
                        readerTemp.Close();

                    try
                    {
                        commTemp.CommandText = "INSERT INTO Dictionary(IDMessaggio, Forms, New, ITA) VALUES('" + label.Name.Replace("'", "''") + "', '" + form.Name.Replace("'", "''") + "', 1, '" + label.Text.Replace("'", "''") + "')";
                        commTemp.ExecuteNonQuery();
                    }
                    catch (Exception exc)
                    {
                        MessageBox.Show(exc.Message);
                    }

                }
            }
        }

        private static void AggiornaListViewEx(ListViewEx.ListViewEx list, System.Windows.Forms.Form form, OdbcConnection connTemp)
        {
            for (int i = 0; i < list.Columns.Count; i++)
            {
                if (list.Columns[i].Name != "CUSTOM1" && list.Columns[i].Name != "CUSTOM2")
                {
                    OdbcCommand commTemp = new OdbcCommand("SELECT * FROM Dictionary WHERE IDMessaggio='" + list.Name.Replace("'", "''") + ".Column" + i.ToString("D2") + "' AND Forms='" + form.Name.Replace("'", "''") + "'", connTemp);
                    OdbcDataReader readerTemp = commTemp.ExecuteReader();

                    if (readerTemp.Read())
                    {
                        string strTemp = "";
                        if (readerTemp.IsDBNull(nColonnaLingua[nLinguaInUso]))
                            strTemp = readerTemp.GetString(nColonnaLingua[0]);
                        else
                            strTemp = readerTemp.GetString(nColonnaLingua[nLinguaInUso]);
                        if (strTemp.Length > 0)
                            list.Columns[i].Text = strTemp;
                        if ((readerTemp != null) && (readerTemp.IsClosed == false))
                            readerTemp.Close();
                    }
                    else
                    {
                        if ((readerTemp != null) && (readerTemp.IsClosed == false))
                            readerTemp.Close();

                        if (list.Columns[i].Text != "")
                        {
                            try
                            {
                                commTemp.CommandText = "INSERT INTO Dictionary(IDMessaggio, Forms, New, ITA) VALUES('" + list.Name.Replace("'", "''") + ".Column" + i.ToString("D2") + "', '" + form.Name.Replace("'", "''") + "', 1, '" + list.Columns[i].Text.Replace("'", "''") + "')";
                                commTemp.ExecuteNonQuery();
                            }
                            catch (Exception exc)
                            {
                                MessageBox.Show(exc.Message);
                            }

                        }
                    }
                }
            }
        }

        private static void AggiornaMenu(System.Windows.Forms.Form form, OdbcConnection connTemp)
        {
            if (form.Menu != null)
            {
                for (int i = 0; i < form.Menu.MenuItems.Count; i++)
                {
                    OdbcCommand commTemp1 = new OdbcCommand("SELECT * FROM Dictionary WHERE IDMessaggio='MainMenu." + i.ToString("D2") + "' AND Forms='" + form.Name.Replace("'", "''") + "'", connTemp);
                    OdbcDataReader readerTemp1 = commTemp1.ExecuteReader();

                    if (readerTemp1.Read())
                    {
                        string strTemp = "";
                        if (readerTemp1.IsDBNull(nColonnaLingua[nLinguaInUso]))
                            strTemp = readerTemp1.GetString(nColonnaLingua[0]);
                        else
                            strTemp = readerTemp1.GetString(nColonnaLingua[nLinguaInUso]);
                        if (strTemp.Length > 0)
                            form.Menu.MenuItems[i].Text = strTemp;
                        if ((readerTemp1 != null) && (readerTemp1.IsClosed == false))
                            readerTemp1.Close();
                    }
                    else
                    {
                        if ((readerTemp1 != null) && (readerTemp1.IsClosed == false))
                            readerTemp1.Close();

                        try
                        {
                            commTemp1.CommandText = "INSERT INTO Dictionary(IDMessaggio, Forms, New, ITA) VALUES('MainMenu." + i.ToString("D2") + "', '" + form.Name.Replace("'", "''") + "', 1, '" + form.Menu.MenuItems[i].Text.Replace("'", "''") + "')";
                            commTemp1.ExecuteNonQuery();
                        }
                        catch (Exception exc)
                        {
                            MessageBox.Show(exc.Message);
                        }

                    }

                    for (int j = 0; j < form.Menu.MenuItems[i].MenuItems.Count; j++)
                    {
                        OdbcCommand commTemp2 = new OdbcCommand("SELECT * FROM Dictionary WHERE IDMessaggio='MainMenu." + i.ToString("D2") + "." + j.ToString("D2") + "' AND Forms='" + form.Name.Replace("'", "''") + "'", connTemp);
                        OdbcDataReader readerTemp2 = commTemp2.ExecuteReader();

                        if (readerTemp2.Read())
                        {
                            string strTemp = "";
                            if (readerTemp2.IsDBNull(nColonnaLingua[nLinguaInUso]))
                                strTemp = readerTemp2.GetString(nColonnaLingua[0]);
                            else
                                strTemp = readerTemp2.GetString(nColonnaLingua[nLinguaInUso]);
                            if (strTemp.Length > 0)
                                form.Menu.MenuItems[i].MenuItems[j].Text = strTemp;
                            if ((readerTemp2 != null) && (readerTemp2.IsClosed == false))
                                readerTemp2.Close();
                        }
                        else
                        {
                            if ((readerTemp2 != null) && (readerTemp2.IsClosed == false))
                                readerTemp2.Close();


                            try
                            {
                                commTemp2.CommandText = "INSERT INTO Dictionary(IDMessaggio, Forms, New, ITA) VALUES('MainMenu." + i.ToString("D2") + "." + j.ToString("D2") + "', '" + form.Name.Replace("'", "''") + "', 1, '" + form.Menu.MenuItems[i].MenuItems[j].Text.Replace("'", "''") + "')";
                                commTemp2.ExecuteNonQuery();
                            }
                            catch (Exception exc)
                            {
                                MessageBox.Show(exc.Message);
                            }

                        }
                    }
                }
            }
        }
        private static void AggiornaTitle(System.Windows.Forms.Form form, OdbcConnection connTemp)
        {
            if (form.Text.Length > 0)
            {
                OdbcCommand commTemp = new OdbcCommand("SELECT * FROM Dictionary WHERE IDMessaggio='Title' AND Forms='" + form.Name.Replace("'", "''") + "'", connTemp);
                OdbcDataReader readerTemp = commTemp.ExecuteReader();

                if (readerTemp.Read())
                {
                    string strTemp = "";
                    if (readerTemp.IsDBNull(nColonnaLingua[nLinguaInUso]))
                        strTemp = readerTemp.GetString(nColonnaLingua[0]);
                    else
                        strTemp = readerTemp.GetString(nColonnaLingua[nLinguaInUso]);
                    if (strTemp.Length > 0)
                        form.Text = strTemp;
                    if ((readerTemp != null) && (readerTemp.IsClosed == false))
                        readerTemp.Close();
                }
                else
                {
                    if ((readerTemp != null) && (readerTemp.IsClosed == false))
                        readerTemp.Close();
                    try
                    {
                        commTemp.CommandText = "INSERT INTO Dictionary(IDMessaggio, Forms, New, ITA) VALUES('Title', '" + form.Name.Replace("'", "''") + "', 1, '" + form.Text.Replace("'", "''") + "')";
                        commTemp.ExecuteNonQuery();
                    }
                    catch (Exception exc)
                    {
                        MessageBox.Show(exc.Message);
                    }
                }
                //si aggiunge la descrizione della sede in coda al titolo della finestra
                form.Text += $" - {Globals.DESCR_SEDE}";
            }
        }

        public static string ConvertDate(System.DateTime dtConvert)
        {
            return dtConvert.Year.ToString("D4") + dtConvert.Month.ToString("D2") + dtConvert.Day.ToString("D2");
        }
        public static string ConvertDate(string strConvert)
        {
            if (strConvert.Length != 8)
                return "";

            return strConvert.Substring(6, 2) + "/" + strConvert.Substring(4, 2) + "/" + strConvert.Substring(0, 4);
        }

        public static string ConvertDateTime(System.DateTime dtConvert)
        {
            return dtConvert.Year.ToString("D4") + dtConvert.Month.ToString("D2") + dtConvert.Day.ToString("D2") + dtConvert.Hour.ToString("D2") + dtConvert.Minute.ToString("D2") + dtConvert.Second.ToString("D2");
        }
        public static string ConvertDateTime(string strConvert)
        {
            if (strConvert.Length != 14)
                return "";

            return strConvert.Substring(6, 2) + "/" + strConvert.Substring(4, 2) + "/" + strConvert.Substring(0, 4) + " " + strConvert.Substring(8, 2) + ":" + strConvert.Substring(10, 2);
        }

        public static void InsertLogState(int iddisp, int old, int nw)
        {
            try
            {
                OdbcConnection connTemp = DBUtil.GetODBCConnection();
                OdbcCommand commTemp_LOG = new OdbcCommand("INSERT INTO LOG (UTENTE, TABELLA, OPERAZIONE, DATA, RECORD,NOMECAMPO,VALOREORIGINALE,VALOREMODIFICATO ) VALUES ('" + KleanTrak.Globals.m_strUser + "', 'DISPOSITIVI', 'Modifica', '" + KleanTrak.Globals.ConvertDateTime(DateTime.Now) + "', " + iddisp + ",'STATO','" + old + "','" + nw + "')", connTemp);

                commTemp_LOG.ExecuteNonQuery();
                connTemp.Close();
            }
            catch (Exception)
            {
            }
        }

        public static int Refresh { get; set; }

        public static string GetLocalIPAddress()
        {
            if (System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())
            {
                IPHostEntry host;
                string localIP = "";
                host = Dns.GetHostEntry(Dns.GetHostName());
                foreach (IPAddress ip in host.AddressList)
                {
                    if (ip.AddressFamily == AddressFamily.InterNetwork)
                    {
                        localIP = ip.ToString();
                        break;
                    }
                }
                return localIP;
            }
            else
            {
                return "";
            }
        }
        public static void ResizeList(Form form, ListView lv, bool first_col_empty = true)
        {
            try
            {
                lv.Visible = false;
                int col_num = (first_col_empty) ? lv.Columns.Count - 1 : lv.Columns.Count;
                int col_width = Convert.ToInt32(Math.Floor((decimal)(form.Width / col_num)));
                col_width -= 3; //per evitare scroll orizzontale
                for (int i = 0; i < lv.Columns.Count; i++)
                    lv.Columns[i].Width = (i == 0 && first_col_empty) ? 0 : col_width;//prima colonna dato vuoto per inserire item
            }
            catch (Exception ex)
            {
                Log(ex);
            }
            finally
            {
                lv.Visible = true;
            }
        }
        public static void ResizeGrid(Form form, DataGridView dgv)
        {
            try
            {
                dgv.Visible = false;
                int col_width = System.Convert.ToInt32(Math.Floor((decimal)(form.Width / (dgv.Columns.Count))));
                col_width -= 3; //per evitare scroll orizzontale
                foreach (DataGridViewColumn col in dgv.Columns)
                    col.Width = col_width;
            }
            catch (Exception ex)
            {
                Globals.WarnAndLog(ex);
            }
            finally
            {
                dgv.Visible = true;
            }
        }

    }
}
