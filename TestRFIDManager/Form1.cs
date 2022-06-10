using amrfidmgrex;
using System;
using System.Windows.Forms;

namespace TestRFIDManager
{
	public partial class Form1 : Form
	{
		string deviceTag = "317";
		string userTag = "123";
		int cleanerID = 98;

		string odbcConn = "Driver={SQL Server};UID=sa;PWD=nautilus;SERVER=127.0.0.1,1433;DATABASE=klynntruck";

		public Form1()
		{
			InitializeComponent();

			Device.ODBCConnectionString = odbcConn;
			Device.Refresh();
			Operator.ODBCConnectionString = odbcConn;
			Operator.Refresh();
		}

		private void btnTest_Click(object sender, EventArgs e)
		{
			var newState = amrfidmgrex.Types.State.Dirty;
			
			int idExamToSave = -1;

         int oldState = DBUtilities.getStateFromTag(deviceTag, odbcConn);

			DBUtilities.insertnewCycleEx(deviceTag, 
				userTag, 
				cleanerID, 
				(int)newState, 
				oldState, 
				idExamToSave, 
				odbcConn);
		}

		private void button1_Click(object sender, EventArgs e)
		{
			var newState = amrfidmgrex.Types.State.PreWashing;

			int idExamToSave = -1;

			int oldState = DBUtilities.getStateFromTag(deviceTag, odbcConn);

			DBUtilities.insertnewCycleEx(deviceTag,
				userTag,
				cleanerID,
				(int)newState,
				oldState,
				idExamToSave,
				odbcConn);

		}

		private void button2_Click(object sender, EventArgs e)
		{
			var newState = amrfidmgrex.Types.State.Washing;

			int idExamToSave = -1;

			int oldState = DBUtilities.getStateFromTag(deviceTag, odbcConn);

			DBUtilities.insertnewCycleEx(deviceTag,
				userTag,
				cleanerID,
				(int)newState,
				oldState,
				idExamToSave,
				odbcConn);
		}

		private void button3_Click(object sender, EventArgs e)
		{
			var newState = amrfidmgrex.Types.State.Clean;

			int idExamToSave = -1;

			int oldState = DBUtilities.getStateFromTag(deviceTag, odbcConn);

			DBUtilities.insertnewCycleEx(deviceTag,
				userTag,
				cleanerID,
				(int)newState,
				oldState,
				idExamToSave,
				odbcConn);

		}
	}
}
