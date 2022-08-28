using System;
using System.IO;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Lotlab.PluginCommon
{
    public enum LogLevel
    {
        TRACE,
        DEBUG,
        INFO,
        WARNING,
        ERROR,
    }

    /// <summary>
    /// 简易日志记录器
    /// </summary>
    public class SimpleLogger : IDisposable
    {
        public readonly object logLock = new object();
        List<LogItem> logs = new List<LogItem>();
        FileStream logFile = null;
        StreamWriter logFileWriter = null;

        int maxObserveLog = 1000;

        public ObservableCollection<LogItem> ObserveLogs { get; } = new ObservableCollection<LogItem>();

        public string LogFilePath { get; }

        public string LogFileBackupPath => LogFilePath + ".bak";

        /// <summary>`
        /// 创建一个日志记录器
        /// </summary>
        /// <param name="file">日志文件</param>
        /// <param name="filter">默认过滤等级</param>
        public SimpleLogger(string file, LogLevel filter = LogLevel.INFO, int maxObserveLogs = 1000)
        {
            LogFilePath = file;
            filterLevel = filter;
            maxObserveLog = maxObserveLogs;

            try
            {
                if (!string.IsNullOrEmpty(file))
                {
                    // file backup
                    try
                    {
                        if (File.Exists(LogFilePath))
                        {
                            if (File.Exists(LogFileBackupPath))
                                File.Delete(LogFileBackupPath);

                            File.Move(LogFilePath, LogFileBackupPath);
                        }
                    }
                    catch (Exception e)
                    {
                        LogError("日志文件备份出错", e);
                    }

                    logFile = File.Open(file, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
                    logFileWriter = new StreamWriter(logFile);
                }
            }
            catch (Exception e)
            {
                LogError("日志文件打开出错，将无法写日志文件", e);
            }

        }

        public void Dispose()
        {
            Close();
        }

        public void Close()
        {
            logFileWriter?.Flush();
            logFileWriter?.Close();
            logFileWriter = null;

            logFile?.Close();
            logFile = null;
        }

        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="level"></param>
        /// <param name="content"></param>
        public void Log(LogLevel level, string content)
        {
            var item = new LogItem(level, content);
            Log(item);
        }

        /// <summary>
        /// Log
        /// </summary>
        /// <param name="item"></param>
        public void Log(LogItem item)
        {
            lock (logLock)
            {
                logs.Add(item);
                if (item.Level >= filterLevel)
                {
                    if (ObserveLogs.Count > maxObserveLog)
                        ObserveLogs.RemoveAt(0);

                    ObserveLogs.Add(item);
                }
                if (logFileWriter != null)
                {
                    logFileWriter.Write(item.ToString());
                    logFileWriter.Write("\n");
                    logFileWriter.Flush();
                }
            }
        }

        /// <summary>
        /// 当前过滤等级
        /// </summary>
        LogLevel filterLevel = LogLevel.INFO;

        /// <summary>
        /// 设置过滤等级
        /// </summary>
        /// <param name="level"></param>
        public void SetFilter(LogLevel level)
        {
            if (filterLevel != level)
            {
                filterLevel = level;

                lock (logLock)
                {
                    ObserveLogs.Clear();
                    foreach (var item in logs)
                    {
                        if (item.Level >= filterLevel)
                        {
                            if (ObserveLogs.Count > maxObserveLog)
                                ObserveLogs.RemoveAt(0);

                            ObserveLogs.Add(item);
                        }
                    }
                }
            }
        }

        public void LogTrace(string content, Exception e = null)
        {
            Log(new LogItem(LogLevel.TRACE, content, e));
        }
        public void LogDebug(string content, Exception e = null)
        {
            Log(new LogItem(LogLevel.DEBUG, content, e));
        }
        public void LogInfo(string content, Exception e = null)
        {
            Log(new LogItem(LogLevel.INFO, content, e));
        }
        public void LogWarning(string content, Exception e = null)
        {
            Log(new LogItem(LogLevel.WARNING, content, e));
        }
        public void LogError(string content, Exception e = null)
        {
            Log(new LogItem(LogLevel.ERROR, content, e));
        }
        public void LogError(Exception e)
        {
            Log(new LogItem(LogLevel.ERROR, e.Message, e));
        }
    }

    public class LogItem
    {
        public DateTime Time { get; }

        public LogLevel Level { get; }

        public string Content { get; }
        
        public Exception Exception { get; }

        public LogItem(LogLevel level, string content, Exception e = null)
        {
            Level = level;
            Content = content;
            Time = DateTime.Now;
            Exception = e;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append(Time.ToString("O"));
            sb.Append(" - [");
            sb.Append(Level.ToString());
            sb.Append("] ");
            sb.Append(Content);
            if (Exception != null)
            {
                sb.Append(" ");
                sb.Append(Exception);
            }
            return sb.ToString();
        }

        public string ToShortString()
        {
            var sb = new StringBuilder();
            sb.Append("[");
            sb.Append(Time.ToLongTimeString());
            sb.Append("] [");
            sb.Append(Level.ToString());
            sb.Append("] ");
            sb.Append(Content);
            if (Exception != null)
            {
                sb.Append(" ");
                sb.Append(Exception.Message);
            }
            return sb.ToString();
        }
    }

}
