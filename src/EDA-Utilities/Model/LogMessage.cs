using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDA_Utilities.Model
{
    public class LogMessage
    {
        public LogMessage(string source, string logLevel, string message, string priority)
        {
            Source=source;
            LogLevel=logLevel;
            Message=message;
            Priority=priority;
        }
        public LogMessage()
        {

        }

        public string Source { get; set; }
        public string LogLevel { get; set; }
        public string Message { get; set; }
        public string Priority { get; set; }
    }
}
