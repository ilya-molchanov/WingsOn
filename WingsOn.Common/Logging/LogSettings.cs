using System.Collections.Generic;

namespace WingsOn.Common.Logging
{
    public class LogSettings
    {
        public string LogDirectory { get; set; }
        public IDictionary<string, string> Loggers { get; set; }
        public bool IsTraceEnable { get; set; }
    }
}