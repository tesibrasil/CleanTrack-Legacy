using System.Windows.Forms;

namespace KleanTrak
{
    partial class ConfigurazioneStatiDispositivi
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
			this.components = new System.ComponentModel.Container();
			this.imageList = new System.Windows.Forms.ImageList(this.components);
			this.listView = new ListViewEx.ListViewEx();
			this.headID = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.headDescrizione = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.headDescrizioneAzione = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.headBarcode = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.headColore = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.headVisibileMenu = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.headVisibileLista = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.headVISIBILELISTADETTAGLIO = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.headInizioCiclo = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.headInizioSterilizzazione = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.headFineSterilizzazione = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.headInizioStoccaggio = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.headFineStoccaggio = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.headStartPreWash = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.hedEndPreWash = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.headSCELTARAPIDA = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.headVisibilePrincipale = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.maintoolstripcontainer = new System.Windows.Forms.ToolStripContainer();
			this.maintoolstrip = new System.Windows.Forms.ToolStrip();
			this.tsbnew = new System.Windows.Forms.ToolStripButton();
			this.tsbupdate = new System.Windows.Forms.ToolStripButton();
			this.tsbdelete = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.tsbclose = new System.Windows.Forms.ToolStripButton();
			this.maintoolstripcontainer.ContentPanel.SuspendLayout();
			this.maintoolstripcontainer.TopToolStripPanel.SuspendLayout();
			this.maintoolstripcontainer.SuspendLayout();
			this.maintoolstrip.SuspendLayout();
			this.SuspendLayout();
			// 
			// imageList
			// 
			this.imageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth24Bit;
			this.imageList.ImageSize = new System.Drawing.Size(46, 46);
			this.imageList.TransparentColor = System.Drawing.Color.Magenta;
			// 
			// listView
			// 
			this.listView.BackColor = System.Drawing.SystemColors.Window;
			this.listView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.headID,
            this.headDescrizione,
            this.headDescrizioneAzione,
            this.headBarcode,
            this.headColore,
            this.headVisibileMenu,
            this.headVisibileLista,
            this.headVISIBILELISTADETTAGLIO,
            this.headInizioCiclo,
            this.headInizioSterilizzazione,
            this.headFineSterilizzazione,
            this.headInizioStoccaggio,
            this.headFineStoccaggio,
            this.headStartPreWash,
            this.hedEndPreWash,
            this.headSCELTARAPIDA,
            this.headVisibilePrincipale});
			this.listView.Dock = System.Windows.Forms.DockStyle.Fill;
			this.listView.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.listView.FullRowSelect = true;
			this.listView.GridLines = true;
			this.listView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this.listView.HideSelection = false;
			this.listView.Location = new System.Drawing.Point(0, 0);
			this.listView.MultiSelect = false;
			this.listView.Name = "listView";
			this.listView.Size = new System.Drawing.Size(1454, 559);
			this.listView.SmallImageList = this.imageList;
			this.listView.TabIndex = 36;
			this.listView.UseCompatibleStateImageBehavior = false;
			this.listView.View = System.Windows.Forms.View.Details;
			this.listView.SelectedIndexChanged += new System.EventHandler(this.listView_SelectedIndexChanged);
			// 
			// headID
			// 
			this.headID.Text = "ID";
			// 
			// headDescrizione
			// 
			this.headDescrizione.Text = "Descrizione";
			// 
			// headDescrizioneAzione
			// 
			this.headDescrizioneAzione.Text = "Descrizione azione";
			// 
			// headBarcode
			// 
			this.headBarcode.Text = "Barcode";
			// 
			// headColore
			// 
			this.headColore.Text = "Colore";
			this.headColore.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// headVisibileMenu
			// 
			this.headVisibileMenu.Text = "Ordine menù";
			this.headVisibileMenu.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// headVisibileLista
			// 
			this.headVisibileLista.Text = "Ordine lista";
			this.headVisibileLista.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// headVISIBILELISTADETTAGLIO
			// 
			this.headVISIBILELISTADETTAGLIO.Text = "Ordine lista dettaglio";
			this.headVISIBILELISTADETTAGLIO.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// headInizioCiclo
			// 
			this.headInizioCiclo.Text = "Inizio ciclo";
			this.headInizioCiclo.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// headInizioSterilizzazione
			// 
			this.headInizioSterilizzazione.Text = "Inizio sterilizzazione";
			this.headInizioSterilizzazione.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// headFineSterilizzazione
			// 
			this.headFineSterilizzazione.Text = "Fine sterilizzazione";
			this.headFineSterilizzazione.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// headInizioStoccaggio
			// 
			this.headInizioStoccaggio.Text = "Inizio stoccaggio";
			this.headInizioStoccaggio.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// headFineStoccaggio
			// 
			this.headFineStoccaggio.Text = "Fine stoccaggio";
			this.headFineStoccaggio.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// headStartPreWash
			// 
			this.headStartPreWash.Text = "Inizio Prelavaggio";
			this.headStartPreWash.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// hedEndPreWash
			// 
			this.hedEndPreWash.Text = "Fine Prelavaggio";
			this.hedEndPreWash.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// headSCELTARAPIDA
			// 
			this.headSCELTARAPIDA.Text = "Scelta rapida";
			this.headSCELTARAPIDA.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// headVisibilePrincipale
			// 
			this.headVisibilePrincipale.Text = "Visibile principale";
			this.headVisibilePrincipale.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// maintoolstripcontainer
			// 
			// 
			// maintoolstripcontainer.ContentPanel
			// 
			this.maintoolstripcontainer.ContentPanel.Controls.Add(this.listView);
			this.maintoolstripcontainer.ContentPanel.Size = new System.Drawing.Size(1454, 559);
			this.maintoolstripcontainer.Dock = System.Windows.Forms.DockStyle.Fill;
			this.maintoolstripcontainer.Location = new System.Drawing.Point(0, 0);
			this.maintoolstripcontainer.Name = "maintoolstripcontainer";
			this.maintoolstripcontainer.Size = new System.Drawing.Size(1454, 618);
			this.maintoolstripcontainer.TabIndex = 41;
			this.maintoolstripcontainer.Text = "toolStripContainer1";
			// 
			// maintoolstripcontainer.TopToolStripPanel
			// 
			this.maintoolstripcontainer.TopToolStripPanel.Controls.Add(this.maintoolstrip);
			// 
			// maintoolstrip
			// 
			this.maintoolstrip.Dock = System.Windows.Forms.DockStyle.None;
			this.maintoolstrip.ImageScalingSize = new System.Drawing.Size(52, 52);
			this.maintoolstrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbnew,
            this.tsbupdate,
            this.tsbdelete,
            this.toolStripSeparator1,
            this.tsbclose});
			this.maintoolstrip.Location = new System.Drawing.Point(3, 0);
			this.maintoolstrip.Name = "maintoolstrip";
			this.maintoolstrip.Size = new System.Drawing.Size(242, 59);
			this.maintoolstrip.TabIndex = 0;
			// 
			// tsbnew
			// 
			this.tsbnew.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.tsbnew.Image = global::kleanTrak.Properties.Resources.add;
			this.tsbnew.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsbnew.Name = "tsbnew";
			this.tsbnew.Size = new System.Drawing.Size(56, 56);
			this.tsbnew.Text = "toolStripButton1";
			this.tsbnew.Click += new System.EventHandler(this.tsbnew_Click);
			// 
			// tsbupdate
			// 
			this.tsbupdate.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.tsbupdate.Image = global::kleanTrak.Properties.Resources.update;
			this.tsbupdate.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsbupdate.Name = "tsbupdate";
			this.tsbupdate.Size = new System.Drawing.Size(56, 56);
			this.tsbupdate.Text = "toolStripButton2";
			this.tsbupdate.Click += new System.EventHandler(this.tsbupdate_Click);
			// 
			// tsbdelete
			// 
			this.tsbdelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.tsbdelete.Image = global::kleanTrak.Properties.Resources.delete;
			this.tsbdelete.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsbdelete.Name = "tsbdelete";
			this.tsbdelete.Size = new System.Drawing.Size(56, 56);
			this.tsbdelete.Text = "toolStripButton3";
			this.tsbdelete.Click += new System.EventHandler(this.tsbdelete_Click);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(6, 59);
			// 
			// tsbclose
			// 
			this.tsbclose.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.tsbclose.Image = global::kleanTrak.Properties.Resources.close;
			this.tsbclose.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsbclose.Name = "tsbclose";
			this.tsbclose.Size = new System.Drawing.Size(56, 56);
			this.tsbclose.Text = "toolStripButton1";
			this.tsbclose.Click += new System.EventHandler(this.tsbclose_Click);
			// 
			// ConfigurazioneStatiDispositivi
			// 
			this.ClientSize = new System.Drawing.Size(1454, 618);
			this.ControlBox = false;
			this.Controls.Add(this.maintoolstripcontainer);
			this.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Name = "ConfigurazioneStatiDispositivi";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.Text = "Configurazione stati dispositivi";
			this.Load += new System.EventHandler(this.ConfigurazioneStatiDispositivi_Load);
			this.ClientSizeChanged += new System.EventHandler(this.ConfigurazioneStatiDispositivi_ClientSizeChanged);
			this.maintoolstripcontainer.ContentPanel.ResumeLayout(false);
			this.maintoolstripcontainer.TopToolStripPanel.ResumeLayout(false);
			this.maintoolstripcontainer.TopToolStripPanel.PerformLayout();
			this.maintoolstripcontainer.ResumeLayout(false);
			this.maintoolstripcontainer.PerformLayout();
			this.maintoolstrip.ResumeLayout(false);
			this.maintoolstrip.PerformLayout();
			this.ResumeLayout(false);

        }

		#endregion
		private ImageList imageList;
        private ColumnHeader headVisibilePrincipale;
		private ColumnHeader headStartPreWash;
		private ColumnHeader hedEndPreWash;
		private ToolStripContainer maintoolstripcontainer;
		private ToolStrip maintoolstrip;
		private ToolStripButton tsbnew;
		private ToolStripButton tsbupdate;
		private ToolStripButton tsbdelete;
		private ToolStripSeparator toolStripSeparator1;
		private ToolStripButton tsbclose;
	}
}