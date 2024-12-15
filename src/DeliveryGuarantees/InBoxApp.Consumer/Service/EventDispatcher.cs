using RabbitMQExchanges.BuildingBlocks.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InBoxApp.Consumer.Service
{
    public class EventDispatcher
    {
        public EventDispatcher()
        {

        }

        public void Dispatch(string payload, string type)
        {
            Console.WriteLine($"Dispatch Called. poyload:{payload}, type:{type}");

            switch (type)
            {
                case nameof(UserRegisteredEvent):
                    var eventModel = JsonConvert.DeserializeObject<UserRegisteredEvent>(payload);
                    Proccess(eventModel);
                    break;
                default:
                    break;
            }
        }

        private void Proccess(UserRegisteredEvent eventModel)
        {
            Console.WriteLine($"{DateTime.Now.ToLongTimeString()} |UserRegisteredEvent Proccessed. {eventModel.FirstName} has {eventModel.Age} years old.");
        }
    }
}
