using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Odbc;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using KleanTrak.Core;
using KleanTrak.Model;
using OdbcExtensions;

namespace KleanTrak
{
    public partial class FrmReceiptsView : Form
    {
        private enum GridColumns
        {
            id,
            data,
            filename
        }
        private List<Query.ComboboxItem> _devices = new List<Query.ComboboxItem>();
        private OdbcDataAdapter _adapter = null;
        private DataTable _table = new DataTable();
        private OdbcParameter _p_id_sterilizzatrice = new OdbcParameter("@idsterilizzatrice", OdbcType.Int);
        private OdbcParameter _p_datetime_start = new OdbcParameter("@datetimestart", OdbcType.NVarChar);
        private OdbcParameter _p_datetime_end = new OdbcParameter("@datetimeend", OdbcType.NVarChar);
        private DateTime _start_date = DateTime.Now;

        public FrmReceiptsView()
        {
            InitializeComponent();
            FillWashersCombo();
            dgv_receipts.DataSource = _table;
            SetupAdapter();
            SetStartDate(_start_date);
        }

        public FrmReceiptsView(int cycleId)
        {
            InitializeComponent();
            FillWashersCombo();
            dgv_receipts.DataSource = _table;
            SetupAdapter(cycleId);
            SetStartDate(_start_date);
            DbConnection db = new DbConnection();
            string query_count = $"SELECT * FROM CICLI WHERE ID = {cycleId}";
            DbRecordset dataset = db.ExecuteReader(query_count);
            int sterilizer;
            if (dataset.Count == 1)
                sterilizer = dataset[0].GetInt("IDSTERILIZZATRICE").Value;
            else
                throw new ApplicationException($"Cycle with ID={cycleId} not found");
            foreach (Query.ComboboxItem cbItem in tscombowasher.Items)
            {
                if (cbItem.Value == sterilizer)
                {
                    tscombowasher.SelectedItem = cbItem;
                    break;
                }
            }
            maintoolstrip.Visible = false;
        }

        private void SetupAdapter(int cycleId = -1)
        {
            try
            {
                if (cycleId < 0)
                {
                    var cmd = new OdbcCommand($"SELECT ID, DATAPARSING, FILENAME " +
                        $"FROM STERILIZZATRICIPARSING " +
                        $"WHERE IDSTERILIZZATRICE=? " +
                        $"AND DATAPARSING >= ? " +
                        $"AND DATAPARSING <= ? " +
                        $"ORDER BY DATAPARSING DESC", DBUtil.GetODBCConnection());
                    cmd.Parameters.Add(_p_id_sterilizzatrice);
                    cmd.Parameters.Add(_p_datetime_start);
                    cmd.Parameters.Add(_p_datetime_end);
                    _adapter = new OdbcDataAdapter(cmd);
                    _adapter.SelectCommand = cmd;
                }
                else
                {
                    string q = "SELECT ID, DATAPARSING, FILENAME " +
                        "FROM STERILIZZATRICIPARSING s " +
                        "WHERE s.ID IN ("+
                        "   SELECT MAX(sp.ID)"+
                        "   FROM STERILIZZATRICIPARSING sp"+
                        "   WHERE sp.IDSTERILIZZATRICE IN (" +
                        "       SELECT c.IDSTERILIZZATRICE FROM CICLI c" +
                        $"      WHERE c.ID = {cycleId}" +
                        "   )" +
                        "   AND sp.IDDISPOSITIVO IN ( " +
                        "       SELECT c.IDDISPOSITIVO FROM CICLI c " +
                        $"      WHERE c.ID = {cycleId}" +
                        "   )" +
                        "   GROUP BY sp.FILENAME" +
                        ")";
                    var cmd = new OdbcCommand(q, DBUtil.GetODBCConnection());
                    _adapter = new OdbcDataAdapter(cmd);
                    _adapter.SelectCommand = cmd;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "CleanTrack", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FillTable()
        {
            try
            {
                _table.Clear();
                if (tscombowasher.SelectedItem == null)
                    return;
                if (!CheckDates())
                {
                    MessageBox.Show(KleanTrak.Globals.strTable[133],
                        "CleanTrack",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Stop);
                    return;
                };
                _p_id_sterilizzatrice.Value = ((Query.ComboboxItem)tscombowasher.SelectedItem).Value;
                _p_datetime_start.Value = ToStringDate(_start_date);
                _p_datetime_end.Value = ToStringDate(_start_date.AddDays(30));
                _adapter.Fill(_table);
                foreach (DataRow row in _table.Rows)
                    row[(int)GridColumns.data] = DateFromString((string)row[(int)GridColumns.data]);
                SetupGridHeaders();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "CleanTrack", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool CheckDates() => _start_date <= DateTime.Now;

        private DateTime DateFromString(string db_val) =>
            new DateTime(int.Parse(db_val.Substring(0, 4)),
                int.Parse(db_val.Substring(4, 2)),
                int.Parse(db_val.Substring(6, 2)),
                int.Parse(db_val.Substring(8, 2)),
                int.Parse(db_val.Substring(10, 2)),
                int.Parse(db_val.Substring(12, 2)));

        private string ToStringDate(DateTime date) =>
            date.Year.ToString() +
            date.Month.ToString().PadLeft(2, '0') +
            date.Day.ToString().PadLeft(2, '0') +
            date.Hour.ToString().PadLeft(2, '0') +
            date.Minute.ToString().PadLeft(2, '0') +
            date.Second.ToString().PadLeft(2, '0');

        private void SetupGridHeaders()
        {
            dgv_receipts.EnableHeadersVisualStyles = false;
            DataGridViewCellStyle headerstyle = new DataGridViewCellStyle();
            headerstyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            foreach (DataGridViewColumn column in dgv_receipts.Columns)
            {
                column.HeaderCell.Style = headerstyle;
                column.Resizable = DataGridViewTriState.True;
                if (column.Index == (int)GridColumns.data)
                    column.HeaderText = KleanTrak.Globals.strTable[131];
                if (column.Index == (int)GridColumns.filename)
                    column.HeaderText = KleanTrak.Globals.strTable[132];
            }
            dgv_receipts.Columns[(int)GridColumns.id].Visible = false;
            dgv_receipts.Columns[(int)GridColumns.data].Width = dgv_receipts.Width * 3 / 10;
            dgv_receipts.Columns[(int)GridColumns.filename].Width = dgv_receipts.Width * 7 / 10;
        }

        private void FillWashersCombo()
        {
            try
            {
                //per usare washers.get si deve impostare la connessione anche nel core
                DbConnection.ConnectionString = Globals.strDatabase;
                _devices.AddRange(DBUtil.LoadWashers(Globals.IDSEDE));
                tscombowasher.Items.AddRange(_devices.ToArray());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "CleanTrack", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FrmReceiptsView_Load(object sender, EventArgs e)
        {
            try
            {
                tslblwasher.Text = KleanTrak.Globals.strTable[130];
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "CleanTrack", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void tscombowasher_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillTable();
        }

        private void SetStartDate(DateTime date)
        {
            try
            {
                _start_date = date;
                var end_date = date.AddDays(30);
                tsl_selected_dates.Text = $"{_start_date.ToShortDateString()} - {end_date.ToShortDateString() }";
                _p_datetime_start.Value = ToStringDate(_start_date);
                _p_datetime_end.Value = ToStringDate(end_date);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "CleanTrack", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void tsb_calendar_Click(object sender, EventArgs e)
        {
            try
            {
                var frmdate = new FrmDateSelect();
                frmdate.SetDate(_start_date);
                frmdate.ShowDialog();
                if (!frmdate.DataValid)
                    return;
                SetStartDate(frmdate.SelectedDate);
                FillTable();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "CleanTrack", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgv_receipts_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            OdbcConnection conn = null;
            OdbcCommand cmd = null;
            OdbcDataReader rdr = null;
            try
            {
                conn = DBUtil.GetODBCConnection();
                var ID = _table.Rows[e.RowIndex][(int)GridColumns.id];
                cmd = new OdbcCommand($"SELECT FILENAME, CONTENUTOFILE FROM STERILIZZATRICIPARSING WHERE ID = {ID}", conn);
                rdr = cmd.ExecuteReader();
                string file_content = "";
                string file_name = "";
                if (rdr.Read())
                {
                    file_name = rdr.GetStringEx("FILENAME");
                    file_content = rdr.GetStringEx("CONTENUTOFILE");
                    if (file_name.ToUpper().Contains(".CSV"))
                    {
                        var lines = file_content.Split(',');
                        file_content = "";
                        foreach (var line in lines)
                            file_content += line + Environment.NewLine;
                    }
                }
                (new FrmRecepitDetails() { ReceiptContent = file_content }).ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "CleanTrack", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (rdr != null && !rdr.IsClosed)
                    rdr.Close();
                if (cmd != null)
                    cmd.Dispose();
                if (conn != null)
                    conn.Close();
            }
        }
    }
}
