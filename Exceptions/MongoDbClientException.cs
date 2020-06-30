using System;
using System.Runtime.Serialization;

namespace MongoDbClient.Exceptions
{
    public class MongoDbClientException : Exception
    {
        public MongoDbClientException()
        {
        }

        public MongoDbClientException(string message) : base(message)
        {
        }

        public MongoDbClientException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected MongoDbClientException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
