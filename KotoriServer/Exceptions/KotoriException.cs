using System;

namespace KotoriServer.Exceptions
{
    public class KotoriException : Exception
    {
        public KotoriException()
        {                
        }

        public KotoriException(string message) : base(message)
        {
        }
    }
}
