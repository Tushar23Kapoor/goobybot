using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace goobybot.Core.Util
{
    class LogManager
    {
        //Woo singleton
        private static readonly Lazy<LogManager> lazy =
      new Lazy<LogManager>(() => new LogManager());

        public static LogManager Instance { get { return lazy.Value; } }

        public string CurrentLoggingPath { get; private set; }

        Task loggerTask;
        Queue<_LogMessage> messages = new Queue<_LogMessage>();
        AutoResetEvent messagesWaiting = new AutoResetEvent(false);

        private LogManager()
        {
        }

        public void Init(string logPathDestination)
        {
            DirectoryInfo di = new DirectoryInfo(logPathDestination);
            if (!di.Exists) di.Create();

            Thread taskThread = new Thread(LoggerFunc);
            taskThread.IsBackground = true;
            taskThread.Start(logPathDestination);
        }

        public void Log(string message, LogType type)
        {
            lock (messages)
            {
                messages.Enqueue(new _LogMessage { Message = message, Severity = type });
            }
            messagesWaiting.Set();
        }

        public void LogI(string message) => Log(message, LogType.Info);
        public void LogW(string message) => Log(message, LogType.Warning);
        public void LogE(string message) => Log(message, LogType.Critical);
        public void LogM(string message) => Log(message, LogType.Message);
        public void LogD(string message) => Log(message, LogType.Debug);

        private void LoggerFunc(object param)
        {
            string logPathDestination = param as string;
            try
            {
                CurrentLoggingPath = Path.Combine(logPathDestination, $"{DateTime.Now:yyMMdd_HHmmss}_{System.Diagnostics.Process.GetCurrentProcess().Id}.log");
                using (FileStream fs = new FileStream(CurrentLoggingPath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite))
                using (StreamWriter writer = new StreamWriter(fs))
                {
                    for (; ; )
                    {
                        messagesWaiting.WaitOne();
                        _LogMessage[] messagesToLog = null;
                        lock (messages)
                        {
                            messagesToLog = messages.ToArray();
                            messages.Clear();
                        }

                        foreach (var message in messagesToLog)
                        {
                            writer.WriteLine(message.ToString());
                        }
                        writer.Flush();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Uh oh, logger crashed. {e}");
            }
        }

        class _LogMessage
        {
            public string Message { get; set; }
            public LogType Severity { get; set; }
            private DateTime timeStamp = DateTime.Now;

            public override string ToString()
            {
                return $"[{timeStamp:s}] [{Severity}] {Message}";
            }
        }
    }

    enum LogType
    {
        Message,
        Debug,
        Info,
        Warning,
        Critical
    }
}