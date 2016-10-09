using System;
using System.Runtime.Serialization;

namespace PR2PS.Common.Exceptions
{
    [Serializable]
    public class PR2Exception : Exception
    {
        public PR2Exception() { }

        public PR2Exception(String message) : base(message) { }

        public PR2Exception(String message, Exception inner) : base(message, inner) { }

        protected PR2Exception(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
