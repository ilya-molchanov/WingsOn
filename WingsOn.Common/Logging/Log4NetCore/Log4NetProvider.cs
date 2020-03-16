using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using log4net;
using log4net.Config;
using Microsoft.Extensions.Logging;

namespace WingsOn.Common.Logging.Log4NetCore
{
    public class Log4NetProvider : ILoggerProvider
    {
        private readonly string _configFileName;
        private readonly IDictionary<string, string> _loggersConfig = new Dictionary<string, string>();
        private readonly ConcurrentDictionary<string, Log4NetLogger> _loggers = new ConcurrentDictionary<string, Log4NetLogger>();

        public Log4NetProvider(string log4NetConfigFile)
        {
            _configFileName = log4NetConfigFile;
        }

        public Log4NetProvider(IDictionary<string, string> loggersConfig, string log4NetConfigFile)
        {
            _configFileName = log4NetConfigFile;
            _loggersConfig = loggersConfig;
        }

        public ILogger CreateLogger(string categoryName)
        {
            return _loggers.GetOrAdd(categoryName, CreateLoggerImplementation);
        }

        public void Dispose()
        {
            _loggers.Clear();
        }

        private Log4NetLogger CreateLoggerImplementation(string categoryName)
        {
            var repository = LogManager.GetRepository(Assembly.GetEntryAssembly());

            if (!LogManager.GetCurrentLoggers(repository.Name).Any())
            {
                XmlConfigurator.Configure(repository, new FileInfo(_configFileName));
            }

            return new Log4NetLogger(repository, _loggersConfig, categoryName);
        }
    }
}
