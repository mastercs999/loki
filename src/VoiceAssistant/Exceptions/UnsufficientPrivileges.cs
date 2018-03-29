using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace VoiceAssistant.Exceptions
{
    public class UnsufficientPrivileges : Exception
    {
        public UnsufficientPrivileges()
        {
        }

        public UnsufficientPrivileges(string message) : base(message)
        {
        }

        public UnsufficientPrivileges(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected UnsufficientPrivileges(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
