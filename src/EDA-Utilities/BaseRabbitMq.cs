using RabbitMQ.Client;

public class BaseRabbitMq
{
    public static IModel channel { get; set; }
    public static ConnectionFactory factory { get; set; }
    public static IConnection connection { get; set; }
    public static void Start()
    {
        Console.WriteLine("Connecting to RabbitMq...");
        factory = new ConnectionFactory() { HostName = "localhost" };
        connection = factory.CreateConnection();
        channel = connection.CreateModel();
    }

    public static void Stop()
    {
        Console.WriteLine("Disconnecting the RabbitMq...");

        channel.Dispose();
        connection.Dispose();
    }
}