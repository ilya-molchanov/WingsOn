using log4net;
using Microsoft.Extensions.Logging;

namespace WingsOn.Common.Logging.Log4NetCore
{
    public static class Log4NetLoggerExtensions
    {
        private const string Log4NetConfigFile = "log4net.config";
        private const string GlobalContextPropertieBaseDir = "BaseDir";

        public static ILoggingBuilder AddLog4Net(this ILoggingBuilder builder)
        {
            builder.SetMinimumLevel(LogLevel.Trace);
            builder.AddProvider(new Log4NetProvider(Log4NetConfigFile));
            return builder;
        }

        public static ILoggingBuilder AddLog4Net(this ILoggingBuilder builder, LogSettings logSettings)
        {
            GlobalContext.Properties[GlobalContextPropertieBaseDir] = logSettings.LogDirectory;

            builder.SetMinimumLevel(LogLevel.Trace);
            builder.AddProvider(new Log4NetProvider(logSettings.Loggers, Log4NetConfigFile));
            return builder;
        }
    }
}
