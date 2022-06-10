using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace KleanTrak
{
	/// <summary>
	/// Summary description for FormAssociaTag.
	/// </summary>
	public class FormAssociaTag : System.Windows.Forms.Form
	{
		public string m_RFID = "";

		private System.Windows.Forms.Label label1;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		amrfidmgrex.RFIDManager manager;
		RFIDEventsExtension ext;


		public FormAssociaTag()
		{
			InitializeComponent();
			manager = new amrfidmgrex.RFIDManager();
			ext = new RFIDEventsExtension();
			ext.BadgeDetectedCTRK += ext_BadgeDetectedCTRK;
			manager.addBadgeListener(ext);
		}

		void ext_BadgeDetectedCTRK(string id)
		{
			m_RFID = id;

			DialogResult = DialogResult.OK;

			//this.Close();
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose(bool disposing)
		{
			ext.BadgeDetectedCTRK -= ext_BadgeDetectedCTRK;
			manager.cleanUpListeners();

			//perché ricreo RFIDManager
			// manager.cleanInternalListenersReferences();

			if (disposing)
			{
				if (components != null)
				{
					components.Dispose();
				}
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
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.ForeColor = System.Drawing.Color.Blue;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(667, 252);
            this.label1.TabIndex = 0;
            this.label1.Text = "Passare tag";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // FormAssociaTag
            // 
            this.ClientSize = new System.Drawing.Size(950, 551);
            this.Controls.Add(this.label1);
			this.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormAssociaTag";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Associazione tag";
            this.Closed += new System.EventHandler(this.FormAssociaTag_Closed);
            this.Load += new System.EventHandler(this.FormAssociaTag_Load);
            this.ResumeLayout(false);

		}
		#endregion

		private void FormAssociaTag_Load(object sender, System.EventArgs e)
		{
			//m_RFIDManager.Start(this.Handle);

			manager.readData(KleanTrak.Globals.strBadgeAddressIP + ":" + KleanTrak.Globals.iBadgeAddressPort);

		}

		private void FormAssociaTag_Closed(object sender, System.EventArgs e)
		{
			manager.stopListening();
			manager.cleanUpListeners();
		}
	}
}
