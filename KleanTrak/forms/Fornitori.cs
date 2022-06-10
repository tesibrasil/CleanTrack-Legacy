using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Data.Odbc;
using ExtModules;
using OdbcExtensions;

namespace KleanTrak
{
	public class Fornitori : System.Windows.Forms.Form
	{
		private ListViewEx.ListViewEx listView;
		private System.Windows.Forms.ColumnHeader columnFornitore;
		private System.Windows.Forms.ColumnHeader columnMatricola;
		private System.Windows.Forms.Button btnElimina;
		private System.Windows.Forms.Button btnNuovo;
		private System.Windows.Forms.Button btnChiudi;
		private System.ComponentModel.Container components = null;

		public Fornitori()
		{
			InitializeComponent();

			listView.SetSubItemType(0, ListViewEx.ListViewEx.FieldType.textbox, null);
			listView.SetSubItemType(1, ListViewEx.ListViewEx.FieldType.textbox, null);
			listView.SetEndEditCallback(new ListViewEx.ListViewEx.EndEditingCallbackEdit(EndEditOperatore), null, null, null);
		}

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

		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Fornitori));
			this.btnElimina = new System.Windows.Forms.Button();
			this.btnNuovo = new System.Windows.Forms.Button();
			this.btnChiudi = new System.Windows.Forms.Button();
			this.listView = new ListViewEx.ListViewEx();
			this.columnFornitore = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnMatricola = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.SuspendLayout();
			// 
			// btnElimina
			// 
			this.btnElimina.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnElimina.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btnElimina.Location = new System.Drawing.Point(110, 580);
			this.btnElimina.Name = "btnElimina";
			this.btnElimina.Size = new System.Drawing.Size(100, 30);
			this.btnElimina.TabIndex = 41;
			this.btnElimina.Text = "Elimina";
			this.btnElimina.Click += new System.EventHandler(this.btnElimina_Click);
			// 
			// btnNuovo
			// 
			this.btnNuovo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnNuovo.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btnNuovo.Location = new System.Drawing.Point(5, 580);
			this.btnNuovo.Name = "btnNuovo";
			this.btnNuovo.Size = new System.Drawing.Size(100, 30);
			this.btnNuovo.TabIndex = 38;
			this.btnNuovo.Text = "Nuovo";
			this.btnNuovo.Click += new System.EventHandler(this.btnNuovo_Click);
			// 
			// btnChiudi
			// 
			this.btnChiudi.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnChiudi.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btnChiudi.Location = new System.Drawing.Point(885, 580);
			this.btnChiudi.Name = "btnChiudi";
			this.btnChiudi.Size = new System.Drawing.Size(100, 30);
			this.btnChiudi.TabIndex = 37;
			this.btnChiudi.Text = "Esci";
			this.btnChiudi.Click += new System.EventHandler(this.btnChiudi_Click);
			// 
			// listView
			// 
			this.listView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.listView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnFornitore,
            this.columnMatricola});
			this.listView.FullRowSelect = true;
			this.listView.GridLines = true;
			this.listView.HideSelection = false;
			this.listView.Location = new System.Drawing.Point(5, 5);
			this.listView.MultiSelect = false;
			this.listView.Name = "listView";
			this.listView.Size = new System.Drawing.Size(980, 570);
			this.listView.TabIndex = 36;
			this.listView.UseCompatibleStateImageBehavior = false;
			this.listView.View = System.Windows.Forms.View.Details;
			this.listView.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.listView_ColumnClick);
			this.listView.SelectedIndexChanged += new System.EventHandler(this.listView_SelectedIndexChanged);
			this.listView.KeyDown += new System.Windows.Forms.KeyEventHandler(this.listView_KeyDown);
			// 
			// columnFornitore
			// 
			this.columnFornitore.Text = "Fornitore";
			this.columnFornitore.Width = 378;
			// 
			// columnMatricola
			// 
			this.columnMatricola.Text = "Codice";
			this.columnMatricola.Width = 200;
			// 
			// Fornitori
			// 
			this.ClientSize = new System.Drawing.Size(990, 615);
			this.ControlBox = false;
			this.Controls.Add(this.btnElimina);
			this.Controls.Add(this.btnNuovo);
			this.Controls.Add(this.btnChiudi);
			this.Controls.Add(this.listView);
			this.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.KeyPreview = true;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "Fornitori";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "CLEAN TRACK - Fornitori";
			this.Load += new System.EventHandler(this.Fornitori_Load);
			this.ResumeLayout(false);

		}
		#endregion

		public int riempilista_nuovo = 0;


		private bool EndEditOperatore(int iItemNum, int iSubitemNum, string strText)
		{
			bool bReturn = false;

			//OdbcConnection connTemp = new OdbcConnection(CleanTrack.Globals.strDatabase);
			//connTemp.Open();

			OdbcConnection connTemp = DBUtil.GetODBCConnection();
			OdbcCommand commTemp = new OdbcCommand("", connTemp);

			if (strText == "Nuovo fornitore") //cancello la riga se non viene editati niente
			{
				commTemp = new OdbcCommand("Delete from  Fornitori WHERE ID=" + listView.Items[iItemNum].Tag.ToString(), connTemp);
				commTemp.ExecuteNonQuery();
				RiempiLista();
				return bReturn;
			}

			switch (iSubitemNum)
			{
				case 0:
					{
						commTemp = new OdbcCommand("UPDATE Fornitori SET Descrizione = '" + strText.Replace("'", "''").ToUpper() + "' WHERE ID=" + listView.Items[iItemNum].Tag.ToString(), connTemp);


						//INSERIMENTO NEL LOG DI SISTEMA
						//valore originale
						string ValoreOriginale = "";
						OdbcCommand commValoreOriginale = new OdbcCommand("SELECT Descrizione as Descrizione FROM Fornitori WHERE ID=" + listView.Items[iItemNum].Tag.ToString(), connTemp);
						OdbcDataReader readerValoreOriginale = commValoreOriginale.ExecuteReader();
						if (readerValoreOriginale.Read())
						{
							if (!readerValoreOriginale.IsDBNull(readerValoreOriginale.GetOrdinal("Descrizione")))
							{
								ValoreOriginale = readerValoreOriginale.GetValue(readerValoreOriginale.GetOrdinal("Descrizione")).ToString();

								if ((readerValoreOriginale != null) && (readerValoreOriginale.IsClosed == false))
									readerValoreOriginale.Close();
							}
							if ((readerValoreOriginale != null) && (readerValoreOriginale.IsClosed == false))
								readerValoreOriginale.Close();
						}

						if (ValoreOriginale == strText.ToString()) //se non viene modificato il valore non effettuo la query nel LOG.
						{ break; }

						OdbcCommand commTemp_LOG = new OdbcCommand("INSERT INTO LOG (UTENTE, TABELLA, OPERAZIONE, DATA, RECORD, NOMECAMPO, VALOREORIGINALE, VALOREMODIFICATO )" +
								  " VALUES ('" + KleanTrak.Globals.m_strUser.ToUpper() + "', 'FORNITORI', 'Modifica', '" + KleanTrak.Globals.ConvertDateTime(DateTime.Now).ToUpper() + "', '" + listView.Items[iItemNum].Tag.ToString().ToUpper() + "', 'DESCRIZIONE', '" + ValoreOriginale.Replace("'", "''").ToUpper() + "', '" + strText.Replace("'", "''").ToUpper() + "')", connTemp);
						commTemp_LOG.ExecuteNonQuery();

						break;
					}
				case 1:
					{
						commTemp = new OdbcCommand("UPDATE Fornitori SET matricola = '" + strText.Replace("'", "''").ToUpper() + "' WHERE ID=" + listView.Items[iItemNum].Tag.ToString(), connTemp);


						//INSERIMENTO NEL LOG DI SISTEMA
						//valore originale
						string ValoreOriginale = "";
						OdbcCommand commValoreOriginale = new OdbcCommand("SELECT matricola as matricola FROM Fornitori WHERE ID=" + listView.Items[listView.SelectedIndices[0]].Tag.ToString(), connTemp);
						OdbcDataReader readerValoreOriginale = commValoreOriginale.ExecuteReader();
						if (readerValoreOriginale.Read())
						{
							if (!readerValoreOriginale.IsDBNull(readerValoreOriginale.GetOrdinal("matricola")))
							{
								ValoreOriginale = readerValoreOriginale.GetValue(readerValoreOriginale.GetOrdinal("matricola")).ToString();

								if ((readerValoreOriginale != null) && (readerValoreOriginale.IsClosed == false))
									readerValoreOriginale.Close();
							}

							if ((readerValoreOriginale != null) && (readerValoreOriginale.IsClosed == false))
								readerValoreOriginale.Close();
						}

						if (ValoreOriginale == strText.ToString()) //se non viene modificato il valore non effettuo la query nel LOG.
						{ break; }

						OdbcCommand commTemp_LOG = new OdbcCommand("INSERT INTO LOG (UTENTE, TABELLA, OPERAZIONE, DATA, RECORD, NOMECAMPO, VALOREORIGINALE, VALOREMODIFICATO )" +
								  " VALUES ('" + KleanTrak.Globals.m_strUser + "', 'FORNITORI', 'Modifica', '" + KleanTrak.Globals.ConvertDateTime(DateTime.Now) + "', '" + listView.Items[listView.SelectedIndices[0]].Tag.ToString() + "', 'NUMERO DI SERIE', '" + ValoreOriginale.Replace("'", "''") + "', '" + strText.Replace("'", "''") + "')", connTemp);
						commTemp_LOG.ExecuteNonQuery();

						break;
					}
			}

			try
			{
				commTemp.ExecuteNonQuery();
				bReturn = true;
			}
			catch (OdbcException Ex)
			{
				MessageBox.Show(Ex.Message, "Clean Track", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}

			connTemp.Close();

			return bReturn;
		}


		private void btnNuovo_Click(object sender, System.EventArgs e)
		{
			OdbcCommand cmd = null;
			try
			{
				string strCommand = $"INSERT INTO FORNITORI " +
					$"(DESCRIZIONE, MATRICOLA) VALUES ('Nuovo fornitore', '0')";
				cmd = new OdbcCommand(strCommand, DBUtil.GetODBCConnection());
				cmd.ExecuteNonQuery();
				riempilista_nuovo = 1;
				RiempiLista();
				listView.EnsureVisible(listView.Items.Count - 1);
				listView.EditCell(listView.Items.Count - 1, 0);
			}
			catch (OdbcException Ex)
			{
				Globals.WarnAndLog(Ex);
			}
			finally
			{
				if (cmd != null)
					cmd.Dispose();
			}
		}

		private void btnModifica_Click(object sender, System.EventArgs e)
		{
			listView.Enabled = false;
			btnNuovo.Enabled = false;
			btnElimina.Enabled = false;
			btnChiudi.Enabled = false;
		}


		private void btnChiudi_Click(object sender, System.EventArgs e)
		{
			Close();
		}

		private void listView_SelectedIndexChanged(object sender, System.EventArgs e)
		{
		}

		private void Fornitori_Load(object sender, System.EventArgs e)
		{
			Globals.LocalizzaDialog(this);
            listView.ListViewItemSorter = new ListViewEx.ListViewExComparer(listView);
            RiempiLista();
			Cursor.Current = Cursors.Default;
			WindowState = FormWindowState.Maximized;
			MinimumSize = Size;
		}

		private void RiempiLista()
		{
			listView.Items.Clear();
			string order_field = (riempilista_nuovo == 1) ? "ID" : "DESCRIZIONE";
			riempilista_nuovo = (riempilista_nuovo == 1) ? 0 : riempilista_nuovo;
			string query = $"SELECT * FROM FORNITORI ORDER BY {order_field}";
			OdbcCommand cmd = null;
			OdbcDataReader rdr = null;
			try
			{
				cmd = new OdbcCommand(query, DBUtil.GetODBCConnection());
				rdr = cmd.ExecuteReader();
				ListViewItem lvItem;
				while (rdr.Read())
				{
					lvItem = listView.Items.Add(rdr.GetString(rdr.GetOrdinal("Descrizione")));
					lvItem.Tag = rdr.GetIntEx("ID");
					lvItem.SubItems.Add(rdr.GetString(rdr.GetOrdinal("Matricola")));
				}
				if ((rdr != null) && (rdr.IsClosed == false))
					rdr.Close();
				listView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Clean Track", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			finally
			{
				if (rdr != null && !rdr.IsClosed)
					rdr.Close();
				if (cmd != null)
					cmd.Dispose();
			}
		}

		private void listView_ColumnClick(object sender, System.Windows.Forms.ColumnClickEventArgs e)
		{
            ((ListViewEx.ListViewExComparer)listView.ListViewItemSorter).Column = e.Column;
        }

        private void listView_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{//elimino un fornotore dalla lista
			if (listView.SelectedIndices.Count > 0)
			{
				if (e.KeyCode == Keys.Delete && (listView.SelectedIndices.Count > 0))
				{
					if (MessageBox.Show(KleanTrak.Globals.strTable[37], "Clean Track", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
					{
						OdbcConnection connTemp = DBUtil.GetODBCConnection();

						//verifica che il fornitore non sia già stato utilizzato
						OdbcCommand commTemp_check = new OdbcCommand("SELECT Dispositivi.idfornitore from Dispositivi " +
							"LEFT OUTER JOIN Fornitori ON Dispositivi.idfornitore = Fornitori.ID " +
									 "where Fornitori.ID  = " + listView.Items[listView.SelectedIndices[0]].Tag.ToString() + "", connTemp);

						OdbcDataReader readerTemp = commTemp_check.ExecuteReader();

						if (readerTemp.Read())
						{
							MessageBox.Show(KleanTrak.Globals.strTable[38], "Clean Track", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
							return;
						}
						if (readerTemp != null && !readerTemp.IsClosed)
							readerTemp.Close();
						//

						OdbcCommand commTemp = new OdbcCommand("DELETE FROM Fornitori WHERE ID=" + listView.Items[listView.SelectedIndices[0]].Tag.ToString(), connTemp);
						commTemp.ExecuteNonQuery();

						//LOG DI SISTEMA
						OdbcCommand commTemp_LOG = new OdbcCommand("INSERT INTO LOG (UTENTE, TABELLA, OPERAZIONE, DATA, RECORD ) VALUES ('" + KleanTrak.Globals.m_strUser + "', 'FORNITORI', 'Cancellazione', '" + KleanTrak.Globals.ConvertDateTime(DateTime.Now) + "', '" + listView.Items[listView.SelectedIndices[0]].Tag.ToString() + "')", connTemp);
						commTemp_LOG.ExecuteNonQuery();

						listView.Items.RemoveAt(listView.SelectedIndices[0]);

						connTemp.Close();
					}
				}
			}

		}

		private void btnElimina_Click(object sender, System.EventArgs e)
		{//elimino un fornotore dalla lista
			if (listView.SelectedIndices.Count > 0)
			{
				if (MessageBox.Show(KleanTrak.Globals.strTable[39], "Clean Track", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
				{
					OdbcConnection connTemp = DBUtil.GetODBCConnection();

					//verifica che il fornitore non sia già stato utilizzato
					OdbcCommand commTemp_check = new OdbcCommand("SELECT Dispositivi.idfornitore from Dispositivi " +
						"LEFT OUTER JOIN Fornitori ON Dispositivi.idfornitore = Fornitori.ID " +
								"where Fornitori.ID  = " + listView.Items[listView.SelectedIndices[0]].Tag.ToString() + "", connTemp);

					OdbcDataReader readerTemp = commTemp_check.ExecuteReader();
					if (readerTemp.Read())
					{
						MessageBox.Show(KleanTrak.Globals.strTable[40], "Clean Track", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
						return;
					}
					if (readerTemp != null && !readerTemp.IsClosed)
						readerTemp.Close();
					//

					OdbcCommand commTemp = new OdbcCommand("DELETE FROM Fornitori WHERE ID=" + listView.Items[listView.SelectedIndices[0]].Tag.ToString(), connTemp);
					commTemp.ExecuteNonQuery();

					//LOG DI SISTEMA
					OdbcCommand commTemp_LOG = new OdbcCommand("INSERT INTO LOG (UTENTE, TABELLA, OPERAZIONE, DATA, RECORD ) VALUES ('" + KleanTrak.Globals.m_strUser + "', 'FORNITORI', 'Cancellazione', '" + KleanTrak.Globals.ConvertDateTime(DateTime.Now) + "', '" + listView.Items[listView.SelectedIndices[0]].Tag.ToString() + "')", connTemp);
					commTemp_LOG.ExecuteNonQuery();

					listView.Items.RemoveAt(listView.SelectedIndices[0]);

					connTemp.Close();
				}
			}
		}
	}
}
