namespace DoubleWritingApp.Producer.v1
{
    public interface IEmailEvent
    {
        string To { get; }
        string Subject { get; }
        string Body { get; }
        //DateTime CreatedDateTime { get; }

    }
}
