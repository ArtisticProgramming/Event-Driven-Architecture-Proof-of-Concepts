using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQExchanges.BuildingBlocks.Model
{
    public class UserRegisteredEvent : IUserRegisteredEvent
    {
        public UserRegisteredEvent(string firstName, string lastName, int age)
        {
            FirstName=firstName;
            LastName=lastName;
            Age=age;
        }

        public string FirstName { get; }
        public string LastName { get; }
        public int Age { get; }
    }
}
