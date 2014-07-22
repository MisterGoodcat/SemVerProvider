using System;
using System.Runtime.Serialization;

namespace VersionProvider.Core
{
    [Serializable]
    public class InvalidVersionException : Exception
    {
        public InvalidVersionException()
        {
        }

        public InvalidVersionException(string message) : base(message)
        {
        }

        public InvalidVersionException(string message, Exception inner) : base(message, inner)
        {
        }

        protected InvalidVersionException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}