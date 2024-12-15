namespace DoubleWritingApp.Producer.v2
{
    public interface IEmailEvent
    {
        string To { get; }
        string Subject { get; }
        string Body { get; }
        DateTime CreatedDateTime { get;}
        ISingnuture Signature { get; }
    }
}
