﻿using RabbitMQ.Client;
using System.Text;

namespace EDA_Utilities
{
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

            channel.Close();
            channel.Dispose();
            connection.Close();
            connection.Dispose();
        }

        protected static void PrintMessageHeader(IDictionary<string, object> headers)
        {
            if (headers != null)
            {
                Console.WriteLine("Headers:");
                foreach (var header in headers)
                {
                    var key = header.Key;
                    var value = Encoding.UTF8.GetString((byte[])header.Value);
                    Console.WriteLine($"  {key}: {value}");
                }
            }
        }
        protected static byte[] GetByts(string response)
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
