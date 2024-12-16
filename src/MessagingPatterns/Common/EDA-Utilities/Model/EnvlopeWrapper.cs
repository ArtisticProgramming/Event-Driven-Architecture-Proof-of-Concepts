using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQExchanges.BuildingBlocks.Model
{
    public class EnvlopeWrapper : IEnvlopeWrapper
    {
        public EnvlopeWrapper(string envlope, bool encrypted)
        {
            EnvlopeContent=envlope;
            IsEncrypted=encrypted;
        }
        public EnvlopeWrapper()
        {
                
        }

        public string EnvlopeContent { get; set; }
        public bool IsEncrypted { get; set; }
    }
}
