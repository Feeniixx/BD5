using System;
using System.Runtime.Serialization;

namespace CW_5.Services
{
    [Serializable]
    internal class Exception404 : Exception
    {
        public Exception404()
        {
        }

        public Exception404(string message) : base(message)
        {
        }

        public Exception404(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected Exception404(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}