using System;
using System.Collections.Generic;
using log4net;
using log4net.Repository;
using Microsoft.Extensions.Logging;

namespace WingsOn.Common.Logging.Log4NetCore
{
    public class Log4NetLogger : Microsoft.Extensions.Logging.ILogger
    {
        private const string DefaultLoggerName = "Default";

        private readonly ILog _log;

        public Log4NetLogger(ILoggerRepository repository, IDictionary<string, string> loggersConfig, string name)
        {
            _log = LogManager.GetLogger(repository.Name, loggersConfig != null && loggersConfig.ContainsKey(name) ? loggersConfig[name] : DefaultLoggerName);
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel))
            {
                return;
            }

            if (formatter == null)
            {
                throw new ArgumentException(nameof(formatter));
            }

            var message = formatter(state, exception);
            if (!string.IsNullOrEmpty(message) || exception != null)
            {
                WriteMessage(logLevel, eventId.Id, message, exception);
            }
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            switch (logLevel)
            {
                case LogLevel.Trace:
                case LogLevel.Debug:
                    return _log.IsDebugEnabled;
                case LogLevel.Information:
                    return _log.IsInfoEnabled;
                case LogLevel.Warning:
                    return _log.IsWarnEnabled;
                case LogLevel.Error:
                    return _log.IsErrorEnabled;
                case LogLevel.Critical:
                    return _log.IsFatalEnabled;
                default:
                    throw new ArgumentOutOfRangeException(nameof(logLevel));
            }
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        private void WriteMessage(LogLevel logLevel, int eventId, string message, Exception exception)
        {
            var evtId = eventId == 0 ? string.Empty : $" [{eventId}]";
            switch (logLevel)
            {
                case LogLevel.Trace:
                case LogLevel.Debug:
                    _log.Debug($"{message}{evtId}", exception);
                    break;
                case LogLevel.Information:
                    _log.Info($"{message}{evtId}", exception);
                    break;
                case LogLevel.Warning:
                    _log.Warn($"{message}{evtId}", exception);
                    break;
                case LogLevel.Error:
                    _log.Error($"{message}{evtId}", exception);
                    break;
                case LogLevel.Critical:
                    _log.Fatal($"{message}{evtId}", exception);
                    break;
                default:
                    _log.Info($"{message}{evtId}", exception);
                    break;
            }
        }
    }
}