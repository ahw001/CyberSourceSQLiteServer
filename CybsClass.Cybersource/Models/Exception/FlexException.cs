using System.Runtime.Serialization;

namespace CybsClass.Cybersource.Models.Exception
{
    public abstract class FlexException : System.Exception, ISerializable
    {
        /// <summary>
        /// Constructs a new instance of the exception with a specified error message.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception</param>
        public FlexException(string message) : base(message) { }

        /// <summary>
        /// Constructs a new instance of the exception with a specified error message 
        /// and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception</param>
        /// <param name="inner">The exception that is the cause of the current exception</param>
        public FlexException(string message, System.Exception inner) : base(message, inner) { }

        protected FlexException(SerializationInfo serializationInfo, StreamingContext streamingContext)
        {
            // Custom deserialization logic can be added here if needed
        }

        [Obsolete("This method overrides an obsolete member in the base class.")]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }
    }
}
