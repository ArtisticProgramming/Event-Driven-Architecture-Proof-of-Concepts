using System.Data;

namespace DoubleWritingApp.Producer.v1
{
    public class EmailEvent : IEmailEvent
    {
        public string To { get; private set; }
        public string Subject { get; private set; }
        public string Body { get; private set; }

        public EmailEvent(string to, string subject, string body)
        {
            To = to;
            Subject = subject;
            Body = body;
        }
    }
}
