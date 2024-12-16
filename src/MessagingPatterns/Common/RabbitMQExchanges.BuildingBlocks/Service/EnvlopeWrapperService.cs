using RabbitMQExchanges.BuildingBlocks.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Security.Cryptography;

namespace RabbitMQExchanges.BuildingBlocks.Service
{
    public static class EnvlopeWrapperService
    {
        private static string key => "kH9f3nVtYx8pLz2bWq4dJr7mGt6vB1cD";

        public static EnvlopeWrapper XmlWrap<T>(T message) => XmlWrap<T>(message, false);
        public static EnvlopeWrapper XmlWrapWithEncryption<T>(T message) => XmlWrap<T>(message, true);
        private static EnvlopeWrapper XmlWrap<T>(T message, bool mustBeEncrypted)
        {
            var envlopeContent = ConvertObjectToXML(message);
            if (mustBeEncrypted == true)
            {
                envlopeContent = EncryptString(envlopeContent, key);
                Console.WriteLine("\nEncrypted XML:\n" + envlopeContent);
            }
            return new EnvlopeWrapper(envlopeContent, mustBeEncrypted);
        }
        public static T XmlUnWrap<T>(string message) => XmlUnWrap<T>(message, false);
        public static T XmlUnWrapWithDecryption<T>(string message) => XmlUnWrap<T>(message, true);
        private static T XmlUnWrap<T>(string envlopeContent, bool isEncrypted)
        {
            var message = envlopeContent;
            if (isEncrypted == true)
            {
                message = DecryptString(message, key);
                Console.WriteLine("\nUncrypted XML:\n" + message);
            }

            var obj = DeserializeObject<T>(message);
            Console.WriteLine("\nDeserialize to Object\n" );
            return obj;
        }

        public static T DeserializeObject<T>(string toDeserialize)
        {
            var xmlSerializer = new XmlSerializer(typeof(T));
            using (var stringReader = new StringReader(toDeserialize))
            {
                return (T)xmlSerializer.Deserialize(stringReader);
            }
        }

        private static string ConvertXmlToObject<T>(object envlopeContent)
        {
            throw new NotImplementedException();
        }

        private static string ConvertObjectToXML<T>(T message)
        {
            string xmlString = "";
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using (StringWriter stringWriter = new StringWriter())
            {
                serializer.Serialize(stringWriter, message);
                xmlString = stringWriter.ToString();
                Console.WriteLine("Serialized XML:\n" + xmlString);
            }
            return xmlString;
        }


        private static string EncryptString(string plainText, string key)
        {
            using (var aesAlg = Aes.Create())
            {
                aesAlg.Key = Encoding.UTF8.GetBytes(key);
                aesAlg.IV = new byte[16];

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
                using (var msEncrypt = new MemoryStream())
                {
                    using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    using (var swEncrypt = new StreamWriter(csEncrypt))
                    {
                        swEncrypt.Write(plainText);
                    }
                    return Convert.ToBase64String(msEncrypt.ToArray());
                }
            }
        }

        private static string DecryptString(string cipherText, string key)
        {
            using (var aesAlg = Aes.Create())
            {
                aesAlg.Key = Encoding.UTF8.GetBytes(key);
                aesAlg.IV = new byte[16];

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                using (var msDecrypt = new MemoryStream(Convert.FromBase64String(cipherText)))
                {
                    using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    using (var srDecrypt = new StreamReader(csDecrypt))
                    {
                        return srDecrypt.ReadToEnd();
                    }
                }
            }
        }

    }
}
