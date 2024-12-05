namespace DoubleWritingApp.Producer.v2
{
    public class EmailEvent : IEmailEvent
    {
        public string To { get; private set; }
        public string Subject { get; private set; }
        public string Body { get; private set; }
        public DateTime CreatedDateTime { get; private set; }

        public ISingnuture Signature { get; private set; }

        public EmailEvent(string to, string subject, string body, ISingnuture signature)
        {
            To = to;
            Subject = subject;
            Body = body;
            CreatedDateTime = DateTime.Now;
            Signature = signature;
        }
    }
}
