using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeyStoneEngine.Entities
{
    public class EntityFormatException : Exception
    {
        public EntityFormatException() : base() { }
        public EntityFormatException(string message) : base(message) { }
        public EntityFormatException(string message, Exception inner) : base(message, inner) { }
    }
}
