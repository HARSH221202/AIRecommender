using System;
using System.Runtime.Serialization;

namespace AIRecommender.CoreEngine
{
    [Serializable]
    public class Inputnull : ApplicationException
    {
        public Inputnull()
        {
        }

        public Inputnull(string message) : base(message)
        {
        }

        public Inputnull(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected Inputnull(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}