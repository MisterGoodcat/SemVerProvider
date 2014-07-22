using System;
using System.Runtime.Serialization;

namespace VersionProvider.Core.Persistence
{
    [Serializable]
    public class ConcurrentPersistenceException : Exception
    {
        public ConcurrentPersistenceException()
        {
        }

        public ConcurrentPersistenceException(string message) : base(message)
        {
        }

        public ConcurrentPersistenceException(string message, Exception inner) : base(message, inner)
        {
        }

        protected ConcurrentPersistenceException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}