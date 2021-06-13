using System;
using System.Runtime.Serialization;

namespace CW_5.Services
{
    [Serializable]
    internal class NoRowsException : Exception
    {
        public NoRowsException()
        {
        }

        public NoRowsException(string message) : base(message)
        {
        }

        public NoRowsException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected NoRowsException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}