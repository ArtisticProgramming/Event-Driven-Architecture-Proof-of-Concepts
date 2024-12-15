using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoubleWritingApp.Producer.v2
{
    internal class Singnuture : ISingnuture
    {
        public Singnuture(string fullName, string description)
        {
            FullName=fullName;
            Description=description;
        }

        public string FullName { get; }
        public string Description { get; }
    }
}
