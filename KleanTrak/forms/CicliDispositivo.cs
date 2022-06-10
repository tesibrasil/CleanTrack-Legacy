using ListViewEx;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;
using System.Linq;
using OdbcExtensions;

namespace KleanTrak
{
    public class CicliDispositivo : System.Windows.Forms.Form
    {
        private Button btnChiudi;
        private ListViewEx.ListViewEx listView;
        private System.ComponentModel.Container components = null;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label labelDescrizione;
        private Label labelRFID;
        private Label labelTipo;
        private Label labelFornitore;
        private Label labelEsamiEseguiti;
        private int m_nDispositivo = 0;
        private string sDescrizioneDispositivoPrinting;
        private Button btnCycleReport;
        private PrintDocument printDocument1;
        public CicliDispositivo(int nIDDispositivo)
        {
            InitializeComponent();

            m_nDispositivo = nIDDispositivo;
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
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CicliDispositivo));
            this.btnChiudi = new System.Windows.Forms.Button();
            this.listView = new ListViewEx.ListViewEx();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.labelDescrizione = new System.Windows.Forms.Label();
            this.labelRFID = new System.Windows.Forms.Label();
            this.labelTipo = new System.Windows.Forms.Label();
            this.labelFornitore = new System.Windows.Forms.Label();
            this.labelEsamiEseguiti = new System.Windows.Forms.Label();
            this.printDocument1 = new System.Drawing.Printing.PrintDocument();
            this.btnCycleReport = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnChiudi
            // 
            this.btnChiudi.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnChiudi.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnChiudi.Location = new System.Drawing.Point(1330, 670);
            this.btnChiudi.Name = "btnChiudi";
            this.btnChiudi.Size = new System.Drawing.Size(90, 30);
            this.btnChiudi.TabIndex = 38;
            this.btnChiudi.Text = "Indietro";
            this.btnChiudi.Click += new System.EventHandler(this.btnChiudi_Click);
            // 
            // listView
            // 
            this.listView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listView.BackColor = System.Drawing.SystemColors.Window;
            this.listView.FullRowSelect = true;
            this.listView.GridLines = true;
            this.listView.HideSelection = false;
            this.listView.Location = new System.Drawing.Point(4, 67);
            this.listView.Margin = new System.Windows.Forms.Padding(33, 33, 3, 3);
            this.listView.MultiSelect = false;
            this.listView.Name = "listView";
            this.listView.Size = new System.Drawing.Size(1416, 598);
            this.listView.TabIndex = 39;
            this.listView.UseCompatibleStateImageBehavior = false;
            this.listView.View = System.Windows.Forms.View.Details;
            this.listView.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.listView_ColumnClick);
            this.listView.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.listView_ItemSelectionChanged);
            this.listView.DoubleClick += new System.EventHandler(this.listView_DoubleClick);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(12, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(138, 24);
            this.label1.TabIndex = 40;
            this.label1.Text = "Dispositivo:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(396, 6);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(138, 24);
            this.label2.TabIndex = 41;
            this.label2.Text = "Codice:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(12, 36);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(138, 24);
            this.label3.TabIndex = 42;
            this.label3.Text = "Tipo dipositivo:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(396, 36);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(138, 24);
            this.label4.TabIndex = 43;
            this.label4.Text = "Fornitore:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(780, 6);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(138, 24);
            this.label5.TabIndex = 44;
            this.label5.Text = "N° esami:";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelDescrizione
            // 
            this.labelDescrizione.Location = new System.Drawing.Point(150, 6);
            this.labelDescrizione.Name = "labelDescrizione";
            this.labelDescrizione.Size = new System.Drawing.Size(240, 24);
            this.labelDescrizione.TabIndex = 45;
            this.labelDescrizione.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelRFID
            // 
            this.labelRFID.Location = new System.Drawing.Point(534, 6);
            this.labelRFID.Name = "labelRFID";
            this.labelRFID.Size = new System.Drawing.Size(240, 24);
            this.labelRFID.TabIndex = 46;
            this.labelRFID.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelTipo
            // 
            this.labelTipo.Location = new System.Drawing.Point(150, 36);
            this.labelTipo.Name = "labelTipo";
            this.labelTipo.Size = new System.Drawing.Size(240, 24);
            this.labelTipo.TabIndex = 47;
            this.labelTipo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelFornitore
            // 
            this.labelFornitore.Location = new System.Drawing.Point(534, 36);
            this.labelFornitore.Name = "labelFornitore";
            this.labelFornitore.Size = new System.Drawing.Size(240, 24);
            this.labelFornitore.TabIndex = 48;
            this.labelFornitore.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelEsamiEseguiti
            // 
            this.labelEsamiEseguiti.Location = new System.Drawing.Point(924, 6);
            this.labelEsamiEseguiti.Name = "labelEsamiEseguiti";
            this.labelEsamiEseguiti.Size = new System.Drawing.Size(84, 24);
            this.labelEsamiEseguiti.TabIndex = 49;
            this.labelEsamiEseguiti.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnCycleReport
            // 
            this.btnCycleReport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCycleReport.Enabled = false;
            this.btnCycleReport.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnCycleReport.Location = new System.Drawing.Point(5, 670);
            this.btnCycleReport.Name = "btnCycleReport";
            this.btnCycleReport.Size = new System.Drawing.Size(90, 30);
            this.btnCycleReport.TabIndex = 51;
            this.btnCycleReport.Text = "Report Ciclo";
            this.btnCycleReport.UseVisualStyleBackColor = true;
            this.btnCycleReport.Click += new System.EventHandler(this.btnCycleReport_Click);
            // 
            // CicliDispositivo
            // 
            this.ClientSize = new System.Drawing.Size(1426, 703);
            this.ControlBox = false;
            this.Controls.Add(this.btnCycleReport);
            this.Controls.Add(this.labelEsamiEseguiti);
            this.Controls.Add(this.labelFornitore);
            this.Controls.Add(this.labelTipo);
            this.Controls.Add(this.labelRFID);
            this.Controls.Add(this.labelDescrizione);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.listView);
            this.Controls.Add(this.btnChiudi);
            this.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CicliDispositivo";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "CLEAN TRACK - Scheda dispositivo";
            this.Load += new System.EventHandler(this.CicliDispositivo_Load);
            this.ResumeLayout(false);

        }
        void listView_DoubleClick(object sender, EventArgs e)
        {
            if (listView.SelectedItems.Count == 1)
            {
                CicliDispositivoDettaglio dlg = new CicliDispositivoDettaglio((int)listView.SelectedItems[0].Tag);
                if (dlg.ShowDialog() == DialogResult.OK)
                    StampaDettaglioCiclo((int)listView.SelectedItems[0].Tag, m_nDispositivo, printDocument1);
            }
        }
        private void btnChiudi_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }
        private void CicliDispositivo_Load(object sender, System.EventArgs e)
        {
            Globals.LocalizzaDialog(this);

            listView.ListViewItemSorter = new ListViewExComparer(listView);

            RiempiRiepilogo();
            RiempiLista();
        }
        private void RiempiRiepilogo()
        {
            OdbcConnection connTemp = null;
            OdbcDataReader readerTemp = null;
            OdbcCommand commTemp = null;
            try
            {
                string query = "SELECT * FROM VistaDispositivi WHERE ID = " + m_nDispositivo.ToString();
                connTemp = DBUtil.GetODBCConnection();
                commTemp = new OdbcCommand(query, connTemp);
                readerTemp = commTemp.ExecuteReader();
                if (readerTemp.Read())
                {
                    labelDescrizione.Text = readerTemp.GetStringEx("Descrizione");
                    labelRFID.Text = readerTemp.GetStringEx("Matricola");
                    labelTipo.Text = readerTemp.GetStringEx("TipoDispositivo");
                    labelFornitore.Text = readerTemp.GetStringEx("Fornitore");
                    labelEsamiEseguiti.Text = readerTemp.GetIntEx("CicliEseguiti").ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (readerTemp != null)
                {
                    if (readerTemp.IsClosed == false)
                        readerTemp.Close();
                    readerTemp.Dispose();
                }
                if (commTemp != null)
                    commTemp.Dispose();
                if (connTemp != null)
                {
                    connTemp.Close();
                    connTemp.Dispose();
                }
            }
        }
        private void RiempiLista()
        {
            listView.Items.Clear();

            List<State> stateToView = StateList.Instance.GetDetailList();
            foreach (var state in stateToView)
            {
                ColumnHeader col = new ColumnHeader() { Text = state.ActionDescription, TextAlign = HorizontalAlignment.Left, Width = 140 };
                col.SetColumnType(ColumnType.Date);
                listView.Columns.Add(col);

                col = new ColumnHeader() { Text = KleanTrak.Globals.strTable[122] + " " + state.ActionDescription, TextAlign = HorizontalAlignment.Left, Width = 180 };
                col.SetColumnType(ColumnType.String);
                listView.Columns.Add(col);
            }

            string query = "SELECT * FROM VistaDispositiviCicli WHERE ID = " + m_nDispositivo.ToString() + " ORDER BY IDCiclo DESC, DataOra";

            OdbcConnection connTemp = null;
            OdbcDataReader readerTemp = null;
            OdbcCommand commTemp = null;

            try
            {
                connTemp = DBUtil.GetODBCConnection();
                commTemp = new OdbcCommand(query, connTemp);
                readerTemp = commTemp.ExecuteReader();

                while (readerTemp.Read())
                {
                    int nIDCiclo = readerTemp.GetIntEx("IDCiclo");

                    ListViewItem lvItem = null;
                    for (int i = 0; i < listView.Items.Count; i++)
                    {
                        if (((int)listView.Items[i].Tag) == nIDCiclo)
                        {
                            lvItem = listView.Items[i];
                            break;
                        }
                    }

                    if (lvItem == null)
                    {
                        lvItem = listView.Items.Add("");
                        lvItem.Tag = nIDCiclo;

                        for (int i = 0; i < listView.Columns.Count; i++)
                            lvItem.SubItems.Add("");
                    }

                    int nIDStatoNew = readerTemp.GetIntEx("IDStatoNew");
                    string sDataOra = readerTemp.GetStringEx("DataOra");
                    string sCognome = readerTemp.GetStringEx("Cognome");
                    string sNome = readerTemp.GetStringEx("Nome");
                    for (int i = 0; i < stateToView.Count; i++)
                    {
                        if (stateToView[i].ID == nIDStatoNew)
                        {
                            lvItem.SubItems[i * 2].Text = KleanTrak.Globals.ConvertDateTime(sDataOra);
                            lvItem.SubItems[i * 2 + 1].Text = sNome + " " + sCognome;
                            break;
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
                MessageBox.Show(query);
                MessageBox.Show(exc.StackTrace);
            }
            finally
            {
                if (readerTemp != null)
                {
                    if (readerTemp.IsClosed == false)
                        readerTemp.Close();
                    readerTemp.Dispose();
                }

                if (commTemp != null)
                    commTemp.Dispose();

                if (connTemp != null)
                {
                    connTemp.Close();
                    connTemp.Dispose();
                }
            }
        }
        private void listView_ColumnClick(object sender, System.Windows.Forms.ColumnClickEventArgs e)
        {
            ((ListViewExComparer)listView.ListViewItemSorter).Column = e.Column;
        }
        public static void StampaDettaglioCiclo(int id_cycle, int device_id, PrintDocument print_document)
        {
            if (id_cycle <= 0)
                return;
            if (!DBUtil.GetPrevCycleId(device_id, id_cycle, out int id_prev_cycle, out string descr, out string dev_type, out string matricola))
                return;
            PrintDialog prDiag = new PrintDialog();
            if (prDiag.ShowDialog() == DialogResult.OK)
            {
                print_document.PrinterSettings = prDiag.PrinterSettings;
                print_document.DocumentName = $"cycleid_{id_prev_cycle}_matricola_{matricola}";
                try
                {
                    _lines_to_print = GetLinesToPrint(device_id, id_cycle, id_prev_cycle, descr, dev_type, matricola);
                    print_document.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(PrintPage);
                    print_document.Print();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    print_document.PrintPage -= PrintPage;
                }
            }
        }
        public static List<PrintLine> GetLinesToPrint(int id_device, int id_cycle, int id_prev_cycle, string descr, string dev_type, string matricola)
        {
            var retlist = new List<PrintLine>();
            try
            {
                //titolo report
                retlist.Add(new PrintLine { value = Globals.strTable[157].ToUpper(), is_title = true, center = false });
                retlist.Add(new PrintLine());
                //strumento
                retlist.Add(new PrintLine { key = Globals.strTable[158].ToUpper(), value = string.IsNullOrWhiteSpace(descr) ? "-" : descr.ToUpper(), is_title = false, center = false });
                //categoria
                retlist.Add(new PrintLine { key = Globals.strTable[159].ToUpper(), value = string.IsNullOrWhiteSpace(dev_type) ? "-" : dev_type.ToUpper(), is_title = false, center = false });
                //matricola
                retlist.Add(new PrintLine { key = Globals.strTable[160].ToUpper(), value = string.IsNullOrWhiteSpace(matricola) ? "-" : matricola.ToUpper(), is_title = false, center = false });
                retlist.Add(new PrintLine());
                retlist.AddRange(PrintCycleData(id_cycle, Globals.strTable[161]));
                retlist.Add(new PrintLine());
                retlist.AddRange(PrintCycleData(id_prev_cycle, Globals.strTable[164], true));
                retlist.Add(new PrintLine());
                retlist.AddRange(PrintExtCycleData(id_prev_cycle));
                return retlist;
            }
            catch (Exception e)
            {
                Globals.Log(e,
                    $"id_device: {id_device}",
                    $"id_cycle: {id_cycle}",
                    $"id_prev_cycle: {id_prev_cycle}",
                    $"desc: {descr}",
                    $"dev_type: {dev_type}",
                    $"matricola: {matricola}");
                throw;
            }
        }
        private static List<PrintLine> _lines_to_print = null;
        private static float next_rect_y = 0;
        private const int START_X = 30;
        private static int printable_width = 0;
        private static int printable_height = 0;
        private static Font title_font = new Font("Tahoma", 15, FontStyle.Bold);
        private static Brush title_brush = Brushes.DarkRed;
        private static Font normal_font = new Font("Tahoma", 10);
        private static Font normal_font_bold = new Font("Tahoma", 10, FontStyle.Bold);
        private static Brush normal_brush = Brushes.Black;
        private static StringFormat center_format = new StringFormat { Alignment = StringAlignment.Center };
        private static RectangleF title_rect = new RectangleF();
        private static RectangleF normal_small_rect = new RectangleF();
        private static RectangleF normal_rect = new RectangleF();
        private static List<State> _states = StateList.Instance.GetDetailList();
        private static int GetLineHeight(Graphics g, Font font_to_write) =>
            (int)Math.Ceiling(g.MeasureString("A", font_to_write).Height);
        private static bool UpdateNextLinePos(PrintPageEventArgs e, Font font_to_write)
        {
            int line_height = GetLineHeight(e.Graphics, font_to_write);
            next_rect_y += line_height;
            return next_rect_y < printable_height;
        }
        public static void PrintPage(object sender, PrintPageEventArgs e)
        {
            try
            {
                var g = e.Graphics;
                printable_width = (int)g.VisibleClipBounds.Width - (2 * START_X);
                printable_height = (int)g.VisibleClipBounds.Height - 30;
                title_rect = new RectangleF(START_X, next_rect_y, printable_width, GetLineHeight(g, title_font));
                normal_small_rect = new RectangleF(START_X, next_rect_y, printable_width / 3, GetLineHeight(g, normal_font));
                normal_rect = new RectangleF(START_X, next_rect_y, printable_width, GetLineHeight(g, normal_font));
                Font current_font = title_font;
                next_rect_y = 10;
                while (_lines_to_print.Count > 0)
                {
                    var line = _lines_to_print[0];
                    if (line.is_title)
                    {
                        DrawTitle(e, line.value);
                        current_font = title_font;
                    }
                    else if (line.date_time.Length > 0)
                    {
                        DrawKeyValueTime(e, line.key, line.value, line.date_time);
                        current_font = normal_font_bold;
                    }
                    else if (line.key.Length > 0 && line.value.Length > 0)
                    {
                        DrawKeyValue(e, line.key, line.value);
                        current_font = normal_font_bold;
                    }
                    else if (line.key.Length != 0 || line.value.Length != 0)
                    {
                        DrawLine(e, (line.key.Length > 0) ? line.key : line.value, line.center);
                    }
                    else
                    {
                        DrawEmptyLine(e, (line.is_title) ? title_font : normal_font);
                        current_font = normal_font;
                    }
                    _lines_to_print.RemoveAt(0);
                    if (!UpdateNextLinePos(e, current_font))
                    {
                        e.HasMorePages = _lines_to_print.Count > 0;
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                Globals.WarnAndLog(ex, $"_lines_to_print.Count: {_lines_to_print.Count}");
                e.Cancel = true;
            }
        }
        private static List<PrintLine> PrintExtCycleData(int id_cycle)
        {
            var retlist = new List<PrintLine>();
            OdbcCommand cmd = null;
            OdbcDataReader rdr = null;
            try
            {
                string query = $"SELECT * FROM CICLIEXT WHERE IDCICLO = {id_cycle} ORDER BY ID";
                cmd = new OdbcCommand(query, DBUtil.GetODBCConnection());
                rdr = cmd.ExecuteReader();
                if (!rdr.HasRows)
                    return retlist;
                //spazio prima del titolo
                retlist.Add(new PrintLine { value = Globals.strTable[165].ToUpper(), is_title = false, center = true });
                while (rdr.Read())
                {
                    retlist.Add(new PrintLine
                    {
                        key = rdr.GetStringEx("DESCRIZIONE"),
                        value = rdr.GetStringEx("VALORE"),
                        date_time = rdr.GetStringEx("DATA"),
                        center = false,
                        is_title = false
                    });
                }
                return retlist;
            }
            catch (Exception ex)
            {
                Globals.Log(ex, $"id_cycle: {id_cycle}");
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
        private static void DrawTitle(PrintPageEventArgs e, string title)
        {
            Graphics g = e.Graphics;
            title_rect.X = START_X;
            title_rect.Y = next_rect_y;
            g.DrawString(title, title_font, title_brush, title_rect, center_format);
        }
        private static void DrawLine(PrintPageEventArgs e, string line, bool center)
        {
            Graphics g = e.Graphics;
            normal_rect.X = START_X;
            normal_rect.Y = next_rect_y;
            if (center)
                g.DrawString(line, normal_font, title_brush, normal_rect, center_format);
            else
                g.DrawString(line, normal_font, title_brush, normal_rect);
        }
        private static void DrawKeyValue(PrintPageEventArgs e, string key, string value)
        {
            Graphics g = e.Graphics;
            normal_small_rect.Y = next_rect_y;
            normal_small_rect.X = START_X;
            g.DrawString(key.ToUpper(), normal_font_bold, normal_brush, normal_small_rect);
            normal_small_rect.X += printable_width / 3;
            g.DrawString(value.ToUpper(), normal_font, normal_brush, normal_small_rect);
        }
        private static void DrawKeyValueTime(PrintPageEventArgs e, string key, string value, string dt)
        {
            Graphics g = e.Graphics;
            normal_small_rect.Y = next_rect_y;
            normal_small_rect.X = START_X;
            g.DrawString(key.ToUpper(), normal_font_bold, normal_brush, normal_small_rect);
            normal_small_rect.X += printable_width / 3;
            g.DrawString(value.ToUpper(), normal_font, normal_brush, normal_small_rect);
            normal_small_rect.X += printable_width / 3;
            g.DrawString(Globals.ConvertDateTime(dt), normal_font, normal_brush, normal_small_rect);
        }
        private static void DrawEmptyLine(PrintPageEventArgs e, Font font) => UpdateNextLinePos(e, font);
        private static List<PrintLine> PrintCycleData(int id_cycle, string cycle_descr, bool printFailedField = false)
        {
            var retlist = new List<PrintLine>();
            OdbcCommand cmd = null;
            OdbcDataReader rdr = null;
            try
            {
                string query = $"SELECT * FROM VISTADISPOSITIVICICLI WHERE IDCICLO = {id_cycle}";
                cmd = new OdbcCommand(query, DBUtil.GetODBCConnection());
                rdr = cmd.ExecuteReader();
                // check in table CICLI for complete information
                string query2 = $"SELECT * FROM CICLI WHERE ID = {id_cycle}";
                var cmd2 = new OdbcCommand(query2, DBUtil.GetODBCConnection());
                var rdr2 = cmd2.ExecuteReader();
                string machineCycleId = null;
                bool failed = true;
                if (rdr2.Read())
                {
                    machineCycleId = rdr2.GetStringEx("MACHINECYCLEID");
                    failed = rdr2.GetBoolEx("FAILED");
                    // TODO read other fields
                }
                if (!rdr.HasRows || !rdr2.HasRows)
                    return retlist;
                //spazio prima del titolo
                retlist.Add(new PrintLine { value = $"{cycle_descr.ToUpper()} (ID {id_cycle})", center = true, is_title = false });
                if (printFailedField)
                    retlist.Add(new PrintLine { key = Globals.strTable[219], value = failed ? Globals.strTable[220] : Globals.strTable[135], center = false, is_title = false });
                if (!string.IsNullOrWhiteSpace(machineCycleId))
                    retlist.Add(new PrintLine { key = Globals.strTable[218], value = machineCycleId, center = false, is_title = false });
                int id_new_state = -1;
                while (rdr.Read())
                {
                    id_new_state = rdr.GetIntEx("IDSTATONEW");
                    if (id_new_state <= 0)
                        continue;
                    string operator_name = $"{rdr.GetStringEx("NOME")} {rdr.GetStringEx("COGNOME")}";
                    string date_time = rdr.GetStringEx("DATAORA");
                    State s = _states.Where(sl => sl.ID == id_new_state).SingleOrDefault();
                    // la lista degli stati è fatta dagli stati visibili in dettaglio
                    // alcuni stati potrebbero non essere presenti.
                    if (s == null)
                        continue;
                    retlist.Add(new PrintLine { key = s.ActionDescription, value = operator_name, date_time = date_time, center = false, is_title = false });
                    int id_exam = rdr.GetIntEx("IDESAME");
                    if (!s.StartCycle || id_exam <= 0)
                        continue;
                    retlist.Add(new PrintLine { key = Globals.strTable[162], value = id_exam.ToString(), center = false, is_title = false });
                    string salaEsame = rdr.GetStringEx("SALA");
                    retlist.Add(new PrintLine { key = Globals.strTable[163], value = string.IsNullOrWhiteSpace(salaEsame) ? "-" : salaEsame, center = false, is_title = false });
                }
                return retlist;
            }
            catch (Exception ex)
            {
                Globals.Log(ex, $"id_cycle: {id_cycle}");
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
        public static int PrintReport(PrintPageEventArgs e, int id_curr_cycle, int id_prev_cycle, string descr, string type, string matricola, int id_last_row)
        {
            if (id_last_row > 0)
            {
                // DATI ESTESI //
                string sQuery = "SELECT * FROM CICLIEXT WHERE IDCICLO = " + id_prev_cycle.ToString() + " AND ID > " + id_last_row.ToString() + " ORDER BY ID";
                id_last_row = 0;
                using (OdbcConnection connTemp = DBUtil.GetODBCConnection())
                {
                    using (OdbcCommand commTemp = new OdbcCommand(sQuery, connTemp))
                    {
                        using (OdbcDataReader readerTemp = commTemp.ExecuteReader())
                        {
                            if (readerTemp.HasRows)
                            {
                                e.Graphics.PageUnit = GraphicsUnit.Millimeter;

                                Font fontNormal = new Font("Calibri", 11);
                                Font fontBold = new Font("Calibri", 11, FontStyle.Bold);

                                int iAltezzaRiga = (int)Math.Ceiling(e.Graphics.MeasureString("AAA", fontNormal).Height);

                                Rectangle rectPage = new Rectangle((int)e.Graphics.VisibleClipBounds.X, (int)e.Graphics.VisibleClipBounds.Y, (int)e.Graphics.VisibleClipBounds.Width, (int)e.Graphics.VisibleClipBounds.Height);

                                Brush brushBlack = new SolidBrush(Color.Black);
                                Brush brushRed = new SolidBrush(Color.Red);
                                RectangleF rect = new RectangleF(rectPage.X, rectPage.Y, rectPage.Width, iAltezzaRiga);
                                StringFormat format = new StringFormat();
                                rect.X = 0;
                                rect.Width = rectPage.Width;
                                rect.Y += iAltezzaRiga;
                                // righe scontrino //
                                while (readerTemp.Read())
                                {
                                    if (!readerTemp.IsDBNull(readerTemp.GetOrdinal("Descrizione")))
                                    {
                                        Brush bTemp = brushBlack;
                                        if (readerTemp.GetBoolEx(("Error")))
                                            bTemp = brushRed;
                                        rect.X = 0;
                                        rect.Width = rectPage.Width / 3;
                                        if (!readerTemp.IsDBNull(readerTemp.GetOrdinal("Data")))
                                            e.Graphics.DrawString(KleanTrak.Globals.ConvertDateTime(readerTemp.GetStringEx("Data")), fontNormal, bTemp, rect, format);
                                        rect.X += rectPage.Width / 3;
                                        e.Graphics.DrawString(readerTemp.GetStringEx("Descrizione") + "_MARIO_", fontNormal, bTemp, rect, format);
                                        rect.X += rectPage.Width / 3;
                                        e.Graphics.DrawString(readerTemp.GetStringEx("Valore"), fontNormal, bTemp, rect, format);
                                        rect.Y += iAltezzaRiga;
                                        if ((rect.Y + rect.Height) > rectPage.Height)
                                        {
                                            id_last_row = readerTemp.GetIntEx("ID");
                                            e.HasMorePages = true;
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                string sQuery1 = "SELECT * FROM VistaDispositiviCicli WHERE IDCICLO = " + id_prev_cycle.ToString() + " ORDER BY DataOra";
                List<State> stateToView = StateList.Instance.GetDetailList();
                OdbcConnection connTemp1 = null;
                OdbcDataReader readerTemp1 = null;
                OdbcCommand commTemp1 = null;
                OdbcConnection connTemp2 = null;
                OdbcDataReader readerTemp2 = null;
                OdbcCommand commTemp2 = null;
                try
                {
                    connTemp1 = DBUtil.GetODBCConnection();
                    commTemp1 = new OdbcCommand(sQuery1, connTemp1);
                    readerTemp1 = commTemp1.ExecuteReader();

                    if (readerTemp1.Read())
                    {
                        e.Graphics.PageUnit = GraphicsUnit.Millimeter;

                        Font fontNormal = new Font("Calibri", 11);
                        Font fontBold = new Font("Calibri", 11, FontStyle.Bold);

                        int iAltezzaRiga = (int)Math.Ceiling(e.Graphics.MeasureString("AAA", fontNormal).Height);

                        Rectangle rectPage = new Rectangle((int)e.Graphics.VisibleClipBounds.X, (int)e.Graphics.VisibleClipBounds.Y, (int)e.Graphics.VisibleClipBounds.Width, (int)e.Graphics.VisibleClipBounds.Height);

                        Brush brushBlack = new SolidBrush(Color.Black);
                        Brush brushRed = new SolidBrush(Color.Red);
                        RectangleF rect = new RectangleF(rectPage.X, rectPage.Y, rectPage.Width, iAltezzaRiga);
                        StringFormat format = new StringFormat();

                        // DISPOSITIVO //

                        format.Alignment = StringAlignment.Near;
                        // format.Alignment = StringAlignment.Center;
                        e.Graphics.DrawString("REPORT CICLO STERILIZZAZIONE", fontBold, brushBlack, rect, format);
                        // format.Alignment = StringAlignment.Near;

                        rect.Y += iAltezzaRiga;
                        rect.Y += iAltezzaRiga;

                        // STRUMENTO
                        rect.X = 0;
                        rect.Width = rectPage.Width / 3;
                        e.Graphics.DrawString("STRUMENTO", fontBold, brushBlack, rect, format);

                        rect.X += rectPage.Width / 3;
                        e.Graphics.DrawString(descr, fontNormal, brushBlack, rect, format);

                        rect.Y += iAltezzaRiga;

                        // CATEGORIA
                        rect.X = 0;
                        rect.Width = rectPage.Width / 3;
                        e.Graphics.DrawString("CATEGORIA", fontBold, brushBlack, rect, format);

                        rect.X += rectPage.Width / 3;
                        e.Graphics.DrawString(type, fontNormal, brushBlack, rect, format);

                        rect.Y += iAltezzaRiga;

                        // MATRICOLA
                        rect.X = 0;
                        rect.Width = rectPage.Width / 3;
                        e.Graphics.DrawString("MATRICOLA", fontBold, brushBlack, rect, format);

                        rect.X += rectPage.Width / 3;
                        e.Graphics.DrawString(matricola, fontNormal, brushBlack, rect, format);

                        rect.Y += iAltezzaRiga;

                        // CICLO
                        rect.X = 0;
                        rect.Width = rectPage.Width / 3;
                        e.Graphics.DrawString("CICLO", fontBold, brushBlack, rect, format);

                        rect.X += rectPage.Width / 3;
                        e.Graphics.DrawString(id_prev_cycle.ToString(), fontNormal, brushBlack, rect, format);

                        rect.Y += iAltezzaRiga;

                        do
                        {
                            int nIDStatoNew = readerTemp1.GetIntEx("IDStatoNew");
                            foreach (var state in stateToView)
                            {
                                if (state.ID != nIDStatoNew)
                                    continue;

                                // stato //
                                rect.X = 0;
                                rect.Width = rectPage.Width / 3;
                                // format.Alignment = StringAlignment.Center;
                                e.Graphics.DrawString(state.ActionDescription.ToUpper(), fontBold, brushBlack, rect, format);
                                // format.Alignment = StringAlignment.Near;
                                format.Trimming = StringTrimming.Character;

                                // rect.Y += iAltezzaRiga;

                                // operatore //
                                rect.X += rectPage.Width / 3;
                                rect.Width = rectPage.Width / 3;
                                // e.Graphics.DrawString("Operatore", fontBold, brushBlack, rect, format);

                                // rect.X += rectPage.Width / 3;
                                e.Graphics.DrawString(readerTemp1.GetStringEx("Nome") + " " + readerTemp1.GetStringEx("Cognome"), fontNormal, brushBlack, rect, format);

                                // rect.Y += iAltezzaRiga;

                                // orario //
                                rect.X += rectPage.Width / 3;
                                rect.Width = rectPage.Width / 3;
                                // e.Graphics.DrawString("Data ora", fontBold, brushBlack, rect, format);

                                // rect.X += rectPage.Width / 3;
                                e.Graphics.DrawString(KleanTrak.Globals.ConvertDateTime(readerTemp1.GetStringEx("DataOra")), fontNormal, brushBlack, rect, format);

                                // rect.Y += iAltezzaRiga;

                                // riga vuota //
                                rect.Y += iAltezzaRiga;

                                if (state.StartCycle)
                                {
                                    // PAZIENTE
                                    int nIDPaziente = readerTemp1.GetIntEx("IDEsame");
                                    if (nIDPaziente > 0)
                                    {
                                        rect.X = 0;
                                        rect.Width = rectPage.Width / 2;
                                        e.Graphics.DrawString("ESAME", fontBold, brushBlack, rect, format);

                                        rect.X += rectPage.Width / 2;
                                        e.Graphics.DrawString(nIDPaziente.ToString(), fontNormal, brushBlack, rect, format);

                                        rect.Y += iAltezzaRiga;
                                    }

                                    // sala //
                                    string sSala = readerTemp1.GetStringEx("Sala");
                                    if (sSala != null && sSala.Length > 0)
                                    {
                                        rect.X = 0;
                                        rect.Width = rectPage.Width / 2;
                                        e.Graphics.DrawString("SALA ESAME", fontBold, brushBlack, rect, format);

                                        rect.X += rectPage.Width / 2;
                                        e.Graphics.DrawString(sSala, fontNormal, brushBlack, rect, format);

                                        rect.Y += iAltezzaRiga;
                                    }
                                }

                                // riga vuota //
                                // rect.Y += iAltezzaRiga; asd
                            }
                        }
                        while (readerTemp1.Read());

                        rect.Y += iAltezzaRiga;

                        // SCONTRINO //
                        string sQuery2 = "SELECT * FROM CICLIEXT WHERE IDCICLO = " + id_prev_cycle + " ORDER BY ID";

                        connTemp2 = DBUtil.GetODBCConnection();
                        commTemp2 = new OdbcCommand(sQuery2, connTemp2);
                        readerTemp2 = commTemp2.ExecuteReader();

                        if (readerTemp2.HasRows)
                        {
                            rect.X = 0;
                            rect.Width = rectPage.Width;
                            // format.Alignment = StringAlignment.Center;
                            e.Graphics.DrawString("DATI STERILIZZATRICE", fontBold, brushBlack, rect, format);
                            // format.Alignment = StringAlignment.Near;

                            rect.Y += iAltezzaRiga;

                            // righe scontrino //
                            while (readerTemp2.Read())
                            {
                                if (!readerTemp2.IsDBNull(readerTemp2.GetOrdinal("Descrizione")))
                                {
                                    Brush bTemp = brushBlack;

                                    if (readerTemp2.GetBoolEx("Error"))
                                        bTemp = brushRed;

                                    rect.X = 0;
                                    rect.Width = rectPage.Width / 3;
                                    if (!readerTemp2.IsDBNull(readerTemp2.GetOrdinal("Data")))
                                        e.Graphics.DrawString(KleanTrak.Globals.ConvertDateTime(readerTemp2.GetStringEx("Data")), fontNormal, bTemp, rect, format);

                                    rect.X += rectPage.Width / 3;
                                    e.Graphics.DrawString(readerTemp2.GetStringEx("Descrizione"), fontNormal, bTemp, rect, format);

                                    rect.X += rectPage.Width / 3;
                                    e.Graphics.DrawString(readerTemp2.GetStringEx("Valore"), fontNormal, bTemp, rect, format);

                                    rect.Y += iAltezzaRiga;

                                    if ((rect.Y + rect.Height) > rectPage.Height)
                                    {
                                        id_last_row = readerTemp2.GetIntEx("ID");
                                        if (readerTemp2.Read())
                                            e.HasMorePages = true;
                                        else
                                            id_last_row = 0;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + "\n\n" + ex.StackTrace);
                }
                finally
                {
                    if (readerTemp1 != null)
                    {
                        if (readerTemp1.IsClosed == false)
                            readerTemp1.Close();
                        readerTemp1.Dispose();
                    }
                    if (readerTemp2 != null)
                    {
                        if (readerTemp2.IsClosed == false)
                            readerTemp2.Close();
                        readerTemp2.Dispose();
                    }

                    if (commTemp1 != null)
                        commTemp1.Dispose();
                    if (commTemp2 != null)
                        commTemp2.Dispose();

                    if (connTemp1 != null)
                    {
                        connTemp1.Close();
                        connTemp1.Dispose();
                    }
                    if (connTemp2 != null)
                    {
                        connTemp2.Close();
                        connTemp2.Dispose();
                    }
                }
            }
            return id_last_row;
        }
        private void btnCycleReport_Click(object sender, EventArgs e)
        {
            if (listView.SelectedItems.Count <= 0)
            {
                MessageBox.Show("Seleziona un dispositivo");
                return;
            }
            sDescrizioneDispositivoPrinting = listView.SelectedItems[0].SubItems[2].Text;
            StampaDettaglioCiclo((int)listView.SelectedItems[0].Tag, m_nDispositivo, printDocument1);
        }
        private void listView_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            btnCycleReport.Enabled = (listView.SelectedItems.Count > 0);
        }
    }
}
