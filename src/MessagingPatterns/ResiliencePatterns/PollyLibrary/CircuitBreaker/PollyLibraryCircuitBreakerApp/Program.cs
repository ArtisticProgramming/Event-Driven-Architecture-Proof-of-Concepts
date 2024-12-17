using Polly;
using Polly.CircuitBreaker;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using System.Net.Sockets;
using System.Text;

public class Program
{
    private static AsyncCircuitBreakerPolicy CircuitBreakerPolicy = Policy
        .Handle<BrokerUnreachableException>()
        .Or<SocketException>()
        .CircuitBreakerAsync(exceptionsAllowedBeforeBreaking: 3,
        durationOfBreak: TimeSpan.FromSeconds(15), onBreak: (exception, breakDelay) =>
            {
                Console.WriteLine($"Circuit broken! Exception: {exception.Message}. " +
                    $"Break duration: {breakDelay.TotalSeconds} seconds.");
            },
        onReset: () => Console.WriteLine("Circuit reset."),
        onHalfOpen:()=> Console.WriteLine("Circuit OnHalfOpen Status."));

    public static async Task Main(string[] args)
    {
        Console.WriteLine(@"
   ____ _                _ _      ____                 _             
  / ___(_)_ __ ___ _   _(_) |_   | __ ) _ __ ___  __ _| | _____ _ __ 
 | |   | | '__/ __| | | | | __|  |  _ \| '__/ _ \/ _` | |/ / _ \ '__|
 | |___| | | | (__| |_| | | |_   | |_) | | |  __/ (_| |   <  __/ |   
  \____|_|_|  \___|\__,_|_|\__|  |____/|_|  \___|\__,_|_|\_\___|_|   
");
        while (true)
        {
            try
            {
                await CircuitBreakerPolicy.ExecuteAsync(async () =>
                {
                    var factory = new ConnectionFactory() { HostName = "localhost" };
                    using var connection = factory.CreateConnection();
                    using var channel = connection.CreateModel();

                    // Declare a queue
                    channel.QueueDeclare(queue: "retry_queue",
                                         durable: true,
                                         exclusive: false,
                                         autoDelete: false,
                                         arguments: null);

                    // Publish a message
                    var message = "Hello, RabbitMQ!";
                    var body = Encoding.UTF8.GetBytes(message);

                    channel.BasicPublish(exchange: "",
                                         routingKey: "retry_queue",
                                         basicProperties: null,
                                         body: body);

                    Console.WriteLine($" [x] Sent '{message}'");
                });
            }
            catch (BrokenCircuitException ex)
            {
                Console.WriteLine($"Request failed due to an open circuit: {ex.Message}");
            }
            catch (BrokerUnreachableException ex)
            {
                Console.WriteLine($"Request failed: {ex.Message}");
            }
            catch (SocketException ex)
            {
                Console.WriteLine($"Request failed: {ex.Message}");
            }
            Thread.Sleep(1000);
        }
    }
}

