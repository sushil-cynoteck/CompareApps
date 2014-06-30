using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;

namespace CompareApps.Areas.Admin.Logging
{
    public interface ILoggerInstance
    {
        void WriteLine(string line);
        void WriteLine(string line, params object[] args);
        void WriteException(Exception exception);
        void WriteException(Exception exception, string message);
    }

    public static class Logger
    {
        private const string INDENTER = @"|     ";
        private const string Normalizer = "\n   ";

        public static TextWriter AdditionalOutput;

        [ThreadStatic]
        private static string ms_lastRecordedTimestamp;

#if NONBUFFEREDLOG
        private static bool ms_bufferedOutput = false;
#else
        private static bool ms_bufferedOutput = true;
#endif

        volatile private static LinkedList<BufferEntry> ms_currentBuffer;

        private static StringBuilder ms_buffer;

        public static bool BufferedOutput
        {
            get { return ms_bufferedOutput; }
            set
            {
                if (ms_bufferedOutput != value)
                {
                    if (ms_bufferedOutput)
                        Flush();
                    ms_bufferedOutput = value;
                }
            }
        }

        public static string LastRecordedTimestamp
        {
            get { return ms_lastRecordedTimestamp; }
            set { ms_lastRecordedTimestamp = value; }
        }

        private static void Push(string value)
        {
            if (ms_currentBuffer == null)
            {
                ms_currentBuffer = new LinkedList<BufferEntry>();
            }
            ms_currentBuffer.AddLast(new BufferEntry(value, null));
        }

        private static void Push(string value, params object[] args)
        {
            if (ms_currentBuffer == null)
            {
                ms_currentBuffer = new LinkedList<BufferEntry>();
            }
            ms_currentBuffer.AddLast(new BufferEntry(value, args));
        }

        private static void EnsureNotTooLarge()
        {
            if (ms_currentBuffer.Count > 100)
            {
                ms_currentBuffer.AddLast(new BufferEntry("*** Flusing log buffer to release used memory ***", null));
                Flush();
            }
        }

        private static void InternalWriteExceptionToTextLog(Exception e, string extraExplainMessage)
        {
            using (Scope("Exception: " + e.Message))
            {
                if (!string.IsNullOrEmpty(extraExplainMessage))
                {
                    WriteLine("Exception explain msg: ", extraExplainMessage);
                }
                WriteLine(e.StackTrace);
                if (e.InnerException != null)
                {
                    InternalWriteExceptionToTextLog(e.InnerException, null);
                }
            }
        }

        private static void InternalWriteExceptionToExceptionLog(Exception e, string extraExplainMessage)
        {
            var fullText = new StringBuilder();
            fullText.AppendFormat("========= {0} =========", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff %K"));
            fullText.AppendLine();
            if (extraExplainMessage != null)
                fullText.AppendLine("Extra Msg: " + extraExplainMessage);
            //
            var shortText = new StringBuilder();
            while (e != null)
            {
                fullText.AppendLine(e.Message);
                fullText.AppendLine(e.StackTrace);
                shortText.AppendLine(e.StackTrace);
                e = e.InnerException;
            }
            fullText.AppendLine();
            fullText.AppendLine();
            //
            var exHash = shortText.ToString().GetHashCode();
            var locationInfo = new LogExceptionInfo(exHash);
            LogFileManager.FlushLogBuffer(fullText.ToString(), locationInfo);
        }

        public static void WriteException(Exception e, string extraExplainMessage)
        {
            InternalWriteExceptionToExceptionLog(e, extraExplainMessage);
            InternalWriteExceptionToTextLog(e, extraExplainMessage);
            EnsureNotTooLarge();
        }

        public static void WriteException(Exception e)
        {
            InternalWriteExceptionToExceptionLog(e, null);
            InternalWriteExceptionToTextLog(e, null);
            EnsureNotTooLarge();
        }

        private static string Normalize(string line)
        {
            if (string.IsNullOrEmpty(line))
            {
                return "";
            }
            return line.Replace("\n", Normalizer + ms_indent);
        }

        public static void Write(string line)
        {
            InternalWrite(line, false);
        }

        private static void WriteLine(string line)
        {
            InternalWrite(line, true);
        }

        private static void InternalWrite(string line, bool isWriteLine)
        {
            if (!IsLocked())
            {
                lock (typeof(Logger))
                {
                    if (isWriteLine)
                    {
                        EnsureLineStart();
                    }
                    line = Normalize(line);

                    Push(line);
                    if (isWriteLine)
                    {
                        Push("\r\n");
                    }

                    if (!BufferedOutput)
                    {
                        Console.Error.WriteLine(line);
                    }

                    if (AdditionalOutput != null)
                    {
                        AdditionalOutput.WriteLine(line);
                    }
                    if (isWriteLine)
                    {
                        ResetLine();
                    }
                }
            }
        }

        private static void ResetLine()
        {
            ms_lineStarted = false;
        }

        private static void EnsureLineStart()
        {
            if (!BufferedOutput)
            {
                Console.ForegroundColor = ms_color == ConsoleColor.Black ? ConsoleColor.DarkRed : ms_color;
            }
            if (!ms_lineStarted)
            {
                lock (typeof(Logger))
                {
                    Push("{0,2}|", Thread.CurrentThread.ManagedThreadId);
                    Push("{0:HH:mm:ss}|", DateTime.Now);
                    Push(ms_indent);
                    Indenter indenter = Indenter.Current;
                    if (indenter != null)
                    {
                        Push(indenter.GetTiming());
                    }

                    if (!BufferedOutput)
                    {
                        Console.Error.Write("{0,2}:", Thread.CurrentThread.ManagedThreadId);
                        Console.Error.Write(ms_indent);
                        if (indenter != null)
                        {
                            Console.Error.Write(indenter.GetTiming());
                        }
                    }
                    if (AdditionalOutput != null)
                    {
                        AdditionalOutput.Write("{0,2}:", Thread.CurrentThread.ManagedThreadId);
                        AdditionalOutput.Write(ms_indent);
                    }
                    ms_lineStarted = true;
                }
            }
        }

        public static void WriteLine(string line, params object[] args)
        {
            if (!IsLocked())
            {
                WriteLine(string.Format(line, args));
                EnsureNotTooLarge();
            }
        }

        [ThreadStatic]
        private static int ms_lockCounter;

        [ThreadStatic]
        private static string ms_indent = "";

        [ThreadStatic]
        private static bool ms_lineStarted;

        private static int ms_numberOfRequests;

        [ThreadStatic]
        private static ConsoleColor ms_color;

        [ThreadStatic]
        private static long ms_sqlTime;

        public static void Reset()
        {
            int n = Interlocked.Increment(ref ms_numberOfRequests);
            switch (n % 4)
            {
                case 0:
                    ms_color = ConsoleColor.Gray;
                    break;
                case 1:
                    ms_color = ConsoleColor.Blue;
                    break;
                case 2:
                    ms_color = ConsoleColor.DarkGreen;
                    break;
                case 3:
                    ms_color = ConsoleColor.DarkYellow;
                    break;
            }
            ms_indent = "";
            ms_lineStarted = false;
            if (ms_currentBuffer != null)
            {
                ms_currentBuffer.Clear();
            }
            Indenter.Current = null;
        }

        public static void Lock()
        {
            ms_lockCounter++;
        }

        public static void Unlock()
        {
            ms_lockCounter--;
        }

        public static bool IsLocked()
        {
            return ms_lockCounter > 0;
        }

        public sealed class Locker : IDisposable
        {
            private readonly bool m_doLockMode;

            public Locker(bool doLockMode)
            {
                m_doLockMode = doLockMode;
                if (m_doLockMode)
                    Lock();
            }
            public void Dispose()
            {
                if (m_doLockMode)
                    Unlock();
            }
        }

        private sealed class Indenter : IDisposable
        {
            [ThreadStatic]
            private static Indenter ms_current;

            private readonly Indenter m_old;

            private readonly string m_message;

            private readonly Stopwatch m_sw = new Stopwatch();
            private readonly long m_sqlTimeStart;

            public Indenter(string message)
            {
                m_sqlTimeStart = ms_sqlTime;
                m_message = message;
                WriteLine(message);
                m_old = ms_current;
                ms_current = this;
                Indent();
                m_sw.Start();
            }

            public static Indenter Current
            {
                get { return ms_current; }
                set { ms_current = value; }
            }

            public void Dispose()
            {
                m_sw.Stop();
                WriteLine(m_message);
                EnsureNotTooLarge();
                Unindent();
                ms_current = m_old;
            }

            public string GetTiming()
            {
                return string.Format("[{0}]({1})  ", m_sw.ElapsedMilliseconds, ms_sqlTime - m_sqlTimeStart);
            }
        }

        private static void Unindent()
        {
            // this "if' is required due to site reinitialize during some tests
            if (ms_indent != null && ms_indent.Length >= INDENTER.Length)
            {
                ms_indent = ms_indent.Substring(INDENTER.Length);
            }
        }

        private static void Indent()
        {
            ms_indent += INDENTER;
        }

        public static IDisposable Scope(string message)
        {
            return new Indenter(message);
        }

        public static IDisposable Scope(string message, params object[] args)
        {
            return new Indenter(string.Format(message, args));
        }

        public static void CountSql(long milliseconds)
        {
            ms_sqlTime += milliseconds;
        }

        public static void Flush()
        {
            if (ms_currentBuffer != null)
            {
                lock (typeof(Logger))
                {
                    StringBuilder buffer = ms_buffer;
                    const int bufSize = 32 * 1024;
                    if (buffer == null)
                    {
                        ms_buffer = buffer = new StringBuilder(bufSize);
                    }
                    string buf;
                    try
                    {
                        buf = ConvertCrrentBufferToString(buffer);
                    }
                    finally
                    {
                        buffer.Length = 0;
                        if (buffer.Capacity > bufSize)
                        {
                            buffer.Capacity = bufSize;
                        }
                        ms_currentBuffer.Clear();
                    }
                    LogFileManager.FlushLogBuffer(buf, LogLocationInfo.Current ?? LogLocationInfo.Default);
                }
            }
        }

        public static string GetCurrent()
        {
            return ConvertCrrentBufferToString(new StringBuilder());
        }

        private static string ConvertCrrentBufferToString(StringBuilder buffer)
        {
            foreach (BufferEntry entry in ms_currentBuffer)
            {
                if (entry.Args != null)
                {
                    buffer.AppendFormat(entry.FormatString, entry.Args);
                }
                else
                {
                    buffer.Append(entry.FormatString);
                }
            }
            string buf = buffer.ToString();
            return buf;
        }

        public static void RecordEvent(EventLogEntryType entryType, string message, int eventId)
        {
            //    try
            //    {
            //        string timeStamp = mts_LastRecordedTimestamp;
            //        string logFileName = LogFileManager.EnsureCurrentFoldersAndFiles(UserInfo.Current) ?? "(unavailable)";
            //        EventLog.WriteEntry(ms_EventSource ?? "caesar2", message + string.Format("\r\n    More details are available at: {0}", logFileName)
            //            + (string.IsNullOrEmpty(timeStamp) ? "" : string.Format("\r\n    Last recorded {0}", timeStamp)),
            //                                               entryType, eventId);
            //    }
            //    catch (Exception ex)
            //    {
            //        WriteException(ex);
            //    }
        }

        //[StaticField(StaticFieldUsage.SiteTolerant)]
        //private static string ms_EventSource;

        //public static void SetEventSource(string eventSource)
        //{
        //    ms_EventSource = eventSource;
        //}
    }

    public struct BufferEntry
    {
        private readonly string m_formatString;
        private readonly object[] m_args;

        public BufferEntry(string formatString, object[] args)
        {
            m_formatString = formatString;
            m_args = args;
        }

        public string FormatString
        {
            get { return m_formatString; }
        }

        public object[] Args
        {
            get { return m_args; }
        }
    }

    public class LogWriter : TextWriter
    {
        public override Encoding Encoding
        {
            get { return Encoding.Unicode; }
        }

        public override void Write(char value)
        {
            Logger.WriteLine(Normalize(value.ToString()));
        }

        public override void Write(char[] buffer, int index, int count)
        {
            Logger.WriteLine(Normalize(new string(buffer, index, count)));
        }

        public override void Write(string value)
        {
            Logger.WriteLine(Normalize(value));
        }

        private static string Normalize(string value)
        {
            value = value.Replace("-- ", @"|     ");
            return value.Replace("\r\n", string.Empty);
        }
    }
}