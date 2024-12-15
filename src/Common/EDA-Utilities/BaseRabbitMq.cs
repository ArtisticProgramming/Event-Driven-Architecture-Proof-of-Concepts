using RabbitMQ.Client;
using System.Text;

namespace EDA_Utilities
{
    public class BaseRabbitMq : CustomeLog
    {
        public static IModel channel { get; set; }
        public static ConnectionFactory factory { get; set; }
        public static IConnection connection { get; set; }
        public static void Start()
        {
            LogInfo("Connecting to RabbitMq...");
            factory = new ConnectionFactory() { HostName = "localhost" };
            connection = factory.CreateConnection();
            channel = connection.CreateModel();
        }

        public static void HandleException(Exception ex)
        {
            LogError(ex.ToString());
            WaitForEnter();
        }

        public static void Stop()
        {
            LogWarning("Disconnecting the RabbitMq...");
            if (channel != null)
            {
                channel.Close();
                channel.Dispose();
            }
            if (connection !=null)
            {
                connection.Close();
                connection.Dispose();
            }
        }

        protected static void PrintMessageHeader(IDictionary<string, object> headers)
        {
            if (headers != null)
            {
                LogInfo("Headers:");
                foreach (var header in headers)
                {
                    var key = header.Key;
                    var value = Encoding.UTF8.GetString((byte[])header.Value);
                    Log($" {key}: {value}");
                }
            }
        }
        protected static byte[] GetBytes(string response)
        {
            if (response == null) return new byte[0];
            return Encoding.UTF8.GetBytes(response);
        }

        protected static string GetString(byte[] body)
        {

            return Encoding.UTF8.GetString(body);
        }
    }
}
