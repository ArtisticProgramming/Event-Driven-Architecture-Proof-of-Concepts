
using EDA_Utilities;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Threading.Channels;

public class Program : BaseRabbitMq
{
    public static void Main()
    {
        try
        {
            Start();

            channel.QueueDeclare(queue: "rpc_queue", durable: false, exclusive: false, autoDelete: false, arguments: null);
            channel.BasicQos(0, 1, false);

            var consumer = new EventingBasicConsumer(channel);
            channel.BasicConsume(queue: "rpc_queue", autoAck: false, consumer: consumer);

            Console.WriteLine("Waiting RPC requests...");

            consumer.Received += (model, ea) =>
            {
                string? response = null;
                byte[] body = ea.Body.ToArray();
                IBasicProperties props = ea.BasicProperties;
                IBasicProperties replyProps = channel.CreateBasicProperties();
                //Set the same CorrelationId for response
                replyProps.CorrelationId= props.CorrelationId;

                try
                {
                    string message = GetString(body);
                    Console.WriteLine($"[X] Recived the message {message} | CorrelationId: {replyProps.CorrelationId}");
                    response =$"The reponse for this requst '{message}' would be nothing.";
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                    response = "Error";
                }
                finally
                {
                    byte[] responseByts = GetBytes(response);
                    channel.BasicPublish(exchange: "", routingKey: props.ReplyTo, props, responseByts);
                    channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                }
            };

            Console.ReadLine();

        }
        catch (Exception)
        {

            throw;
        }
        finally
        {
            Stop();
        }
    }
}