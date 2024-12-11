class Program : BaseRabbitMq
{
    static void Main(string[] args)
    {
        try
        {
            Start();



            WaitForEnter();
        }
        catch (Exception ex)
        {
            HandleException(ex);
        }
        finally
        {
            Stop();
        }
    }
}

