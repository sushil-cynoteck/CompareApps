using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;

namespace CompareApps.Areas.Admin.Logging
{
    public abstract class LogLocationInfo
    {
        
        volatile private static LogLocationInfo ms_mstCurrent;

        public static LogLocationInfo Current
        {
            get { return ms_mstCurrent; }
            internal set { ms_mstCurrent = value; }
        }

        public static LogLocationInfo Default { get; internal set; }

        private static string ms_folderByInstance;
        private const string DefaultInstanceName = "Instance";
        protected static string FolderByInstance
        {
            get
            {
                if(string.IsNullOrEmpty(ms_folderByInstance))
                {
                    ms_folderByInstance = ConfigurationManager.AppSettings["Log.Instance"];
                    if (string.IsNullOrEmpty(ms_folderByInstance))
                    {
                        ms_folderByInstance = DefaultInstanceName;
                    }
                }
                return ms_folderByInstance;
            }
        }

        protected static string FolderByDate
        {
            get { return DateTime.Today.ToString(@"yyyyMMdd"); }
        }

        public abstract IEnumerable<string> GetLogFolderPath();
        public abstract string GetLogFile();
    }

    public class LogServiceInfo : LogLocationInfo
    {
        private readonly string m_fileName;

        public LogServiceInfo()
        {
            var now = DateTime.Now;
            m_fileName = string.Format(@"{0:00}{1:00}{2:00}-{3:00}{4:0}0.log", now.Year % 100, now.Month, now.Day, now.Hour, now.Minute / 10);
        }

        public override IEnumerable<string> GetLogFolderPath()
        {
            yield return FolderByInstance;
            yield return FolderByDate;
        }

        public override string GetLogFile()
        {
            return m_fileName;
        }
    }

    public class LogExceptionInfo : LogLocationInfo
    {
        private static readonly string ExceptionFolderByInstance = FolderByInstance + ".exceptions";
        private readonly string m_hashFile;

        public LogExceptionInfo(int hash1)
        {
            m_hashFile = string.Format("{0:X8}", hash1);
        }

        public override IEnumerable<string> GetLogFolderPath()
        {
            yield return ExceptionFolderByInstance;
            yield return FolderByDate;
        }

        public override string GetLogFile()
        {
            return m_hashFile;
        }
    }

    public static class LogFileManager
    {
        private static string ms_folder;
        private const string DefaultFolderPath = @"d:\logs";

        public static string Folder
        {
            get { return ms_folder; }
        }

        public static void Initialize()
        {
            if (ms_folder == null)
            {
                LogLocationInfo.Default = new LogServiceInfo();
                string logFolder = ConfigurationManager.AppSettings["Log.Folder"];
                if (string.IsNullOrEmpty(logFolder))
                {
                    logFolder = DefaultFolderPath;
                }
                if (!Directory.Exists(logFolder))
                {
                    Directory.CreateDirectory(logFolder);
                }
                ms_folder = logFolder;
                RequestLogWriter.InitalizeRequestLogWriter();
            }
        }

        public static void FinalizeLogFileManager()
        {
            RequestLogWriter.FinalizeLogWriter();
        }

        public static string EnsureCurrentFoldersAndFiles(LogLocationInfo logLocationInfo)
        {
            string path;
            try
            {
                path = Path.GetFullPath(ms_folder);
                if (Directory.Exists(path))
                {
                    foreach (var dirName in logLocationInfo.GetLogFolderPath())
                    {
                        path = Path.Combine(path, dirName);
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }
                    }
                    path = Path.Combine(path, logLocationInfo.GetLogFile());
                }
                else
                {
                    path = null;
                }
            }
            catch (Exception)
            {
                //TODO: think what to do here
                path = null;
            }
            return path;
        }

        public static void FlushLogBuffer(string buffer, LogLocationInfo userInfo)
        {
            RequestLogWriter.FlushLogBuffer(buffer, userInfo);
        }

        private static class RequestLogWriter
        {
            private static readonly AutoResetEvent NotifyWriterEvent = new AutoResetEvent(false);
            private static readonly AutoResetEvent NotifyWriterStoppedEvent = new AutoResetEvent(false);
            private static bool ms_stopWriter;

            private static readonly Queue<LogWriterRequest> Queue = new Queue<LogWriterRequest>();

            public static void InitalizeRequestLogWriter()
            {
                TimerCallback(null, false);
            }

            private static void TimerCallback(object data, bool expired)
            {
                Logger.Flush();
                lock (Queue)
                {
                    if (Queue.Count == 0)
                    {
                        Queue.Enqueue(new LogWriterRequest(LogLocationInfo.Default));
                    }
                    while (Queue.Count > 0)
                    {
                        LogWriterRequest request = Queue.Dequeue();
                        Monitor.Exit(Queue);
                        try
                        {
                            WriteRequest(request);
                        }
                        finally
                        {
                            Monitor.Enter(Queue);
                        }
                    }
                }
                if (ms_stopWriter)
                {
                    NotifyWriterStoppedEvent.Set();
                }
                else
                {
                    ThreadPool.RegisterWaitForSingleObject(NotifyWriterEvent, TimerCallback, null,
                                                           new TimeSpan(0, 1, 0), true);
                }
            }

            private static void WriteRequest(LogWriterRequest request)
            {
                try
                {
                    string fileName = EnsureCurrentFoldersAndFiles(request.LogLocationInfo);
                    if (fileName != null)
                    {
                        Encoding utf8 = new UTF8Encoding(!File.Exists(fileName));
                        //
                        using (var stream = new FileStream(fileName, FileMode.Append, FileAccess.Write, FileShare.Read))
                        {
                            using (TextWriter writer = new StreamWriter(stream, utf8))
                            {
                                writer.Write(request.Buffer ?? "\r\n");
                                writer.WriteLine();
                            }
                        }
                    }
                }
                catch
                {
                    // log exception to error/warning log
                }
            }

            public static void FlushLogBuffer(string buffer, LogLocationInfo userInfo)
            {
                var request = new LogWriterRequest(buffer, userInfo);
#if UNITTESTS
                WriteRequest(request);
                return;
#endif
                if (!string.IsNullOrEmpty(buffer))
                    lock (Queue)
                    {
                        Queue.Enqueue(request);
                NotifyWriterEvent.Set();
                    }
            }

            public static void FinalizeLogWriter()
            {
                Logger.WriteLine("!!! FinalizeLogWriter !!!");
                ms_stopWriter = true;
                Logger.Flush();
#if !UNITTESTS
                NotifyWriterStoppedEvent.WaitOne();
#endif
            }
        }

        private class LogWriterRequest
        {
            private readonly string m_buffer;
            private readonly LogLocationInfo m_logLocationInfo;

            public LogWriterRequest(string buffer, LogLocationInfo logLocationInfo)
            {
                m_buffer = buffer;
                m_logLocationInfo = logLocationInfo;
            }

            public LogWriterRequest(LogLocationInfo logLocationInfo)
            {
                m_buffer = string.Format("========= {0} =========\r\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff %K"));
                m_logLocationInfo = logLocationInfo;
            }

            public string Buffer
            {
                get { return m_buffer; }
            }

            public LogLocationInfo LogLocationInfo
            {
                get { return m_logLocationInfo; }
            }
        }

    }
}