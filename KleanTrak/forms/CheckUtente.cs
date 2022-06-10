using OdbcExtensions;
using System;
using System.Data.Odbc;
using System.Windows.Forms;


namespace KleanTrak
{
    /// <summary>
    /// Descrizione di riepilogo per checkutente.
    /// </summary>
    public class checkutente : System.Windows.Forms.Form
    {
        protected static readonly log4net.ILog Logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// Variabile di progettazione necessaria.
        /// </summary>
        private System.ComponentModel.Container components = null;

        public checkutente()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Pulire le risorse in uso.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #region Codice generato da Progettazione Windows Form
        /// <summary>
        /// Metodo necessario per il supporto della finestra di progettazione. Non modificare
        /// il contenuto del metodo con l'editor di codice.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(checkutente));
            this.listView = new ListViewEx.ListViewEx();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.buttonConfirm = new System.Windows.Forms.Button();
            this.buttonExit = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // listView
            // 
            this.listView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.listView.FullRowSelect = true;
            this.listView.GridLines = true;
            this.listView.Location = new System.Drawing.Point(12, 43);
            this.listView.Name = "listView";
            this.listView.Size = new System.Drawing.Size(759, 322);
            this.listView.TabIndex = 4;
            this.listView.UseCompatibleStateImageBehavior = false;
            this.listView.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Utente";
            this.columnHeader1.Width = 416;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Tipo Operatore";
            this.columnHeader2.Width = 258;
            // 
            // buttonConfirm
            // 
            this.buttonConfirm.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonConfirm.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.buttonConfirm.Location = new System.Drawing.Point(573, 381);
            this.buttonConfirm.Name = "buttonConfirm";
            this.buttonConfirm.Size = new System.Drawing.Size(96, 32);
            this.buttonConfirm.TabIndex = 5;
            this.buttonConfirm.Text = "Conferma";
            this.buttonConfirm.Click += new System.EventHandler(this.buttonConfirm_Click);
            // 
            // buttonExit
            // 
            this.buttonExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonExit.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.buttonExit.Location = new System.Drawing.Point(675, 381);
            this.buttonExit.Name = "buttonExit";
            this.buttonExit.Size = new System.Drawing.Size(96, 32);
            this.buttonExit.TabIndex = 6;
            this.buttonExit.Text = "Esci";
            this.buttonExit.Click += new System.EventHandler(this.buttonExit_Click);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.Location = new System.Drawing.Point(12, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(759, 40);
            this.label2.TabIndex = 7;
            this.label2.Text = "Selezionare l\'utente da aggiungere nella lista operatori sterilizzazione";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // checkutente
            // 
            this.ClientSize = new System.Drawing.Size(783, 425);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.buttonExit);
            this.Controls.Add(this.buttonConfirm);
            this.Controls.Add(this.listView);
            this.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "checkutente";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "CLEAN TRACK  - Aggiungi nuovo Operatore Sterilizzazione";
            this.Load += new System.EventHandler(this.checkutente_Load);
            this.ResumeLayout(false);

        }
        #endregion
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.Button buttonConfirm;
        private System.Windows.Forms.Button buttonExit;
        private ListViewEx.ListViewEx listView;
        private System.Windows.Forms.Label label2;

        public string password_utente = "";
        private void check_password()
        {
            OdbcConnection connTemp = DBUtil.GetODBCConnection();

            OdbcCommand commTemp = new OdbcCommand("SELECT password FROM Utenti WHERE ID='" + KleanTrak.Globals.m_iUserID.ToString() + "'", connTemp);
            OdbcDataReader readerTemp = commTemp.ExecuteReader();
            if (readerTemp.Read() && readerTemp.HasRows)
            {
                if (!readerTemp.IsDBNull(readerTemp.GetOrdinal("password")))
                    password_utente = readerTemp.GetString(readerTemp.GetOrdinal("password"));
            }

            if ((readerTemp != null) && (readerTemp.IsClosed == false))
                readerTemp.Close();

            connTemp.Close();
        }
        private void btnEsci_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }
        private void RiempiLista()
        {
            OdbcCommand cmd = null;
            OdbcDataReader rdr = null;
            try
            {
                listView.Items.Clear();
                string query = $"SELECT UTENTI.USERNAME AS USERNAME, UTENTI.ID, GRUPPI.PERMESSI AS PERMESSI " +
                    $"FROM UTENTI INNER JOIN GRUPPI ON UTENTI.ID_GRUPPO = GRUPPI.ID " +
                    $"WHERE UTENTI.ID NOT IN " +
                    $"(SELECT OPERATORI.ID_UTENTE FROM OPERATORI " +
                    $"INNER JOIN OPERATORI_SEDI ON OPERATORI.ID = OPERATORI_SEDI.IDOPERATORE " +
                    $"WHERE OPERATORI_SEDI.IDSEDE = {KleanTrak.Globals.IDSEDE} AND OPERATORI.ID_UTENTE IS NOT NULL) " +
                    $"AND GRUPPI.NOME <> 'ADMINISTRATORS' ORDER BY UTENTI.USERNAME";
                cmd = new OdbcCommand(query, DBUtil.GetODBCConnection());
                rdr = cmd.ExecuteReader();
                ListViewItem lvItem;
                if (rdr == null || rdr.HasRows == false)
                {
                    MessageBox.Show("Nessun Utente da inserire nella lista Operatori Sterilizzatori", "Clean Track", MessageBoxButtons.OK, MessageBoxIcon.Question);
                    this.Close();
                }
                while (rdr.Read())
                {
                    lvItem = listView.Items.Add(rdr.GetString(rdr.GetOrdinal("username")));
                    lvItem.Tag = rdr.GetIntEx("ID");
                    if (rdr.GetString(rdr.GetOrdinal("permessi")) == "1") //LIVELLO DI PERMESSO DELL'UTENTE
                    { lvItem.SubItems.Add("Utente Supervisore"); }
                    else if (rdr.GetString(rdr.GetOrdinal("permessi")) == "0")
                    { lvItem.SubItems.Add("Utente Normale"); }
                }
            }
            catch (Exception e)
            {
                Logger.Error($"{KleanTrak.Globals.GetLocalIPAddress()} - CheckUtente.RiempiLista", e);
                throw;
            }
            finally
            {
                if (rdr != null && !rdr.IsClosed)
                    rdr.Close();
                if (cmd != null)
                    cmd.Dispose();
            }
        }

        private void checkutente_Load(object sender, System.EventArgs e)
        {
            RiempiLista();
        }

        private void buttonExit_Click(object sender, System.EventArgs e)
        {
            KleanTrak.Globals.Insert_check_utente = -1;
            this.Close();
        }

        private void buttonConfirm_Click(object sender, System.EventArgs e)
        {
            if (listView.SelectedIndices.Count == 0)
            {
                MessageBox.Show(KleanTrak.Globals.strTable[141],
                    KleanTrak.Globals.strTable[140],
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Stop);
                return;
            }
            if (MessageBox.Show("Inserire il nuovo utente selezionato?", "Clean Track", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                KleanTrak.Globals.Insert_check_utente = -1;
                return;
            }
            //inserimento effettivo sulla lista operatori
            string query = $"INSERT INTO OPERATORI " +
                $"(MATRICOLA, DISATTIVATO, ID_UTENTE, COGNOME, TAG) " +
                $"VALUES " +
                $"('', '0', '{listView.SelectedItems[0].Tag}', '', NULL)";
            OdbcCommand cmd = null;
            try
            {
                cmd = new OdbcCommand(query, DBUtil.GetODBCConnection());
                cmd.ExecuteNonQuery();
                cmd.CommandText = "SELECT MAX (ID) FROM OPERATORI";
                int max_id = cmd.ExecuteScalarInt();
                if (max_id == -1)
                    throw new ApplicationException("insert into operatori failed");
                cmd.CommandText = $"INSERT INTO OPERATORI_SEDI (IDOPERATORE, IDSEDE) VALUES ({max_id}, {KleanTrak.Globals.IDSEDE})";
                if (1 != cmd.ExecuteNonQuery())
                    throw new ApplicationException("insert into operatori_sedi failed");
                cmd.CommandText = $"INSERT INTO LOG (UTENTE, TABELLA, OPERAZIONE, DATA, RECORD ) " +
                    $"VALUES " +
                    $"('{Globals.m_strUser}', 'OPERATORI', 'Inserimento', '{Globals.ConvertDateTime(DateTime.Now)}', '{max_id}')";
                if (1 != cmd.ExecuteNonQuery())
                    throw new ApplicationException("insert into log failed");
                Globals.Insert_check_utente = max_id;
                Close();
            }
            catch (OdbcException Ex)
            {
                MessageBox.Show(Ex.Message, "Clean Track", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (cmd != null)
                    cmd.Dispose();
            }
        }
    }
}
