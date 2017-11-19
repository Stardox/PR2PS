using System;
using System.Runtime.Serialization;

namespace PR2PS.LevelImporter.Core
{
    [Serializable]
    public class DbValidationException : Exception
    {
        public DbValidationException() { }

        public DbValidationException(String message) : base(message) { }

        public DbValidationException(String message, Exception inner) : base(message, inner) { }

        protected DbValidationException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
