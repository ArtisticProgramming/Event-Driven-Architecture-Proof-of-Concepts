using Polly.Retry;
using RabbitMQ.Client.Exceptions;
using System.Net.Sockets;

public class Program
{
    private static readonly RetryPolicy _RetryPolicy =
         Policy
         .Handle<BrokerUnreachableException>()
         .Or<SocketException>()
         .WaitAndRetry(5, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
           (exception, timeSpan, retryCount, context) =>
           {
               Console.WriteLine($"Retry {retryCount} " +
                   $"encountered an error: {exception.Message}. " +
                   $"Waiting {timeSpan} before next retry.");
           });

    public static void Main(string[] args)
    {
        Console.WriteLine(@"
 __   ___ ___  __      
|__) |__   |  |__) \ / 
|  \ |___  |  |  \  |  

");
        _RetryPolicy.Execute(() =>
        {

            //Connecting to RabbitMq
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();
            Console.WriteLine("Hello!");
            Console.ReadLine();
        });
    }
}
