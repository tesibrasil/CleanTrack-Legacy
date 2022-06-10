using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Threading;
using System.Data.Odbc;

namespace LibLog
{
    public class LogItem
    {
        public string Level { set; get; }
        public string Date { set; get; }
        public string Description { set; get; }

    }

    public class LogList : List<LogItem>
    {
    }

    public class Logger
    {
        public enum LogLevel
        {
            Info,
            Warning,
            Error
        }

        public enum VerbosityEnum
        {
            All,
            ExcludeInfo,
            OnlyError
        }

        private static bool _COMObject = false;
        public static bool COMObject 
        { 
            set{_COMObject=value;}
            get { return _COMObject; }
        }

        public VerbosityEnum Verbosity { get; set; }

        public static Logger Get()
        {
            if (_instance == null)
                _instance = new Logger();
            return _instance;
        }

        protected Logger()
        {
            Verbosity = VerbosityEnum.All;

        }

        public void Stop()
        {
            _threadstop.Set();
            _thread.Join();
        }

        public void ActivateDatabaseDestination(string connectionString)
        {
            _databaseconnection = connectionString;
            _databasedestination = true;

            if (_thread == null)
                StartThread();
        }

        public void ActivateFileDestination(string dir, string filename, bool writeOnModuleDir, bool divideByDay, string path = "")
        {
            //if (!COMObject)
            {
                _logfiledir = dir;
                _logfilename = filename;
                _filedestination = true;

                _writeOnModuleDir = writeOnModuleDir;
                _divideByDay = divideByDay;

                try
                {
                    if (writeOnModuleDir)
                        _logfiledir = new FileInfo(new Uri(System.Reflection.Assembly.GetEntryAssembly().CodeBase).LocalPath).DirectoryName;
                    if (divideByDay)
                        _logfilename = null;
                }
                catch (Exception)
                {
                }

                if (_thread == null)
                    StartThread();
            }
        }

        protected void StartThread()
        {
            _thread = new Thread(WorkerThread);
            _thread.Start();
        }

		public void Write(Exception e, string host, params string[] log_lines)
		{
			string origin = (log_lines.Length > 0) ? "parameters:" + Environment.NewLine : "";
			foreach (string line in log_lines)
				origin += line + Environment.NewLine;
			Write(host, origin, e.ToString(), null, LogLevel.Error);
		}
		public void Write(string host, string origin, string description, byte[] attachment, LogLevel level)
        {
            Console.WriteLine(DateTime.Now.ToString() + " " + origin + " " + description);

            if (!_filedestination && !_databasedestination)
                return;

            if (level == LogLevel.Info && Verbosity == VerbosityEnum.ExcludeInfo)
                return;

            if (level == LogLevel.Info && Verbosity == VerbosityEnum.OnlyError)
                return;

            if (level == LogLevel.Warning && Verbosity == VerbosityEnum.OnlyError)
                return;

            LogEvent eventToWrite = new LogEvent();
            eventToWrite.CurrentHost = System.Environment.MachineName;
            if (!COMObject)
            {
                eventToWrite.ModuleName = new Uri(System.Reflection.Assembly.GetEntryAssembly().CodeBase).LocalPath;
            }
            eventToWrite.CallerHost = host;
            eventToWrite.Origin = origin;
            eventToWrite.Description = description;
            eventToWrite.Level = level;

            if (eventToWrite.Description.Length > _maxdescriptionsize)
                eventToWrite.Description = eventToWrite.Description.Substring(0, _maxdescriptionsize);

            if (attachment != null)
            {
                eventToWrite.Attachment = System.Convert.ToBase64String(attachment);
                if (eventToWrite.Description.Length > _maxattachmentsize)
                    eventToWrite.Attachment = eventToWrite.Attachment.Substring(0, _maxattachmentsize);
            }

            lock (_listToWrite)
            {
                _listToWrite.Add(eventToWrite);
            }
        }

        // Private members
        static Logger _instance;

        bool _databasedestination = false;
        bool _filedestination = false;

        string _logfiledir, _logfilename;
        bool _writeOnModuleDir, _divideByDay;

        string _databaseconnection;
        OdbcConnection _dbconnection;

        const int _maxdescriptionsize = 1024;
        const int _maxattachmentsize = 1024 * 1024;

        ManualResetEvent _threadstop = new ManualResetEvent(false);
        Thread _thread;

        class LogEvent
        {
            public LogEvent()
            {
                Date = System.DateTime.Now;
            }

            public string CurrentHost { get; set; }
            public string ModuleName { get; set; }
            public string CallerHost { get; set; }
            public string Origin { get; set; }
            public string Description { get; set; }
            public string Attachment { get; set; }
            public LogLevel Level { get; set; }
            public DateTime Date { get; set; }
        }
        List<LogEvent> _listToWrite = new List<LogEvent>();

        private void WorkerThread()
        {
            for (; ; )
            {
                List<LogEvent> listEventToWrite = new List<LogEvent>();
                lock (_listToWrite)
                {
                    while (_listToWrite.Count > 0)
                    {
                        listEventToWrite.Add(_listToWrite[0]);
                        _listToWrite.RemoveAt(0);
                    }
                }

                for (int i = 0; i < listEventToWrite.Count; i++)
                {
                    if (_databasedestination)
                        WriteDatabase(listEventToWrite[i]);
                    if (_filedestination)
                        WriteFile(listEventToWrite[i]);
                }

                lock (_listToWrite)
                {
                    if (_listToWrite.Count == 0 && _threadstop.WaitOne(100))
                        break;
                }
            }
        }

        private void WriteDatabase(LogEvent eventToWrite)
        {
            string strDateHour = string.Format("{0:d4}{1:d2}{2:d2}{3:d2}{4:d2}{5:d2}",
                                               eventToWrite.Date.Year,
                                               eventToWrite.Date.Month,
                                               eventToWrite.Date.Day,
                                               eventToWrite.Date.Hour,
                                               eventToWrite.Date.Minute,
                                               eventToWrite.Date.Second);

            if (_dbconnection == null)
                _dbconnection = new OdbcConnection(_databaseconnection);

            try
            {
                if (_dbconnection.State != System.Data.ConnectionState.Open)
                    _dbconnection.Open();

                OdbcCommand command = new OdbcCommand("INSERT INTO LOGEXT (DATEEVENT, CURRENTHOST, MODULENAME, CALLERHOST, ORIGIN, LOGLEVEL, DESCRIPTION, ATTACHMENT) VALUES (?, ?, ?, ?, ?, ?, ?, ?)", _dbconnection);
                command.Parameters.Add("DATEEVENT", OdbcType.VarChar, 14).Value = strDateHour;
                command.Parameters.Add("CURRENTHOST", OdbcType.VarChar, 255).Value = eventToWrite.CurrentHost;
                command.Parameters.Add("MODULENAME", OdbcType.VarChar, 255).Value = eventToWrite.ModuleName;
                command.Parameters.Add("CALLERHOST", OdbcType.VarChar, 255).Value = (eventToWrite.CallerHost != null) ? eventToWrite.CallerHost : "";
                command.Parameters.Add("ORIGIN", OdbcType.VarChar, 255).Value = eventToWrite.Origin;
                command.Parameters.Add("LOGLEVEL", OdbcType.VarChar, 255).Value = eventToWrite.Level.ToString();
                command.Parameters.Add("DESCRIPTION", OdbcType.VarChar, _maxdescriptionsize).Value = eventToWrite.Description;
                command.Parameters.Add("ATTACHMENT", OdbcType.Text, _maxattachmentsize).Value = (eventToWrite.Attachment != null) ? eventToWrite.Attachment : ""; ;
                command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                writeLog("log exception" + e.ToString());

                _dbconnection.Close();
                _dbconnection.Dispose();
                _dbconnection = null;
            }
        }

        private void writeLog(string text)
        {
            try
            {
                string logName = "C:\\TESILOG" + "\\" + DateTime.Now.ToString("yyyyMMdd") + ".log";

                StreamWriter sw = new StreamWriter(logName, true);
                sw.WriteLine(DateTime.Now.ToString("HH:mm:ss") + " - " + text);
                sw.Close();
            }
            catch
            {
            }
        }

        private void WriteFile(LogEvent eventToWrite)
        {
            string strDateHour = string.Format("{0:d4}-{1:d2}-{2:d2} {3:d2}.{4:d2}.{5:d2}",
                                               eventToWrite.Date.Year,
                                               eventToWrite.Date.Month,
                                               eventToWrite.Date.Day,
                                               eventToWrite.Date.Hour,
                                               eventToWrite.Date.Minute,
                                               eventToWrite.Date.Second);

            string header = strDateHour + " - " +
                          (eventToWrite.Origin + " " + eventToWrite.CallerHost).Trim() + " - " +
                          eventToWrite.Level.ToString();

            string attach = "Attachment: ";
            string spaces = new string(' ', header.Length - attach.Length);

            StreamWriter log;

            string logfile = _logfiledir + "\\" + _logfilename;
            if (_divideByDay)
                logfile = string.Format("{0:s}\\{1:d4}{2:d2}{3:d2}.log",
                                        _logfiledir,
                                        eventToWrite.Date.Year,
                                        eventToWrite.Date.Month,
                                        eventToWrite.Date.Day);

            try
            {
                if (!File.Exists(logfile))
                    log = new StreamWriter(logfile);
                else
                    log = File.AppendText(logfile);

                log.Write(header + " - " + eventToWrite.Description + "\r\n");
                if (eventToWrite.Attachment != null)
                    log.Write(spaces + attach + eventToWrite.Attachment + "\r\n");

                log.Close();
            }
            catch (Exception)
            {
            }
        }

        public LogList GetLog(int maxNumItems)
        {
            if (!_databasedestination)
                return null;

            var result = new LogList();

            if (_dbconnection == null)
                _dbconnection = new OdbcConnection(_databaseconnection);

            try
            {
                if (_dbconnection.State != System.Data.ConnectionState.Open)
                    _dbconnection.Open();

                string query = "SELECT TOP " + maxNumItems.ToString() + " LOGLEVEL, DATEEVENT, DESCRIPTION FROM LOGEXT WHERE MODULENAME = '" + new Uri(System.Reflection.Assembly.GetEntryAssembly().CodeBase).LocalPath + "' ORDER BY ID DESC";
                OdbcCommand command = new OdbcCommand(query, _dbconnection);
                OdbcDataReader rdr = null;
                rdr = command.ExecuteReader();

                while (rdr.Read())
                {
                    result.Add(new LogItem()
                    {
                        Level = rdr.GetString(0),
                        Date = rdr.GetString(1),
                        Description = rdr.GetString(2)
                    });
                }

            }
            catch (Exception)
            {
                _dbconnection.Close();
                _dbconnection.Dispose();
                _dbconnection = null;
            }

            return result;
        }
    }
}
