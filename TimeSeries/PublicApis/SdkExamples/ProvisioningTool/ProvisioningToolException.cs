using System;
using System.Runtime.Serialization;

namespace ProvisioningTool
{
    class ProvisioningToolException : Exception
    {
        public ProvisioningToolException()
        {
        }

        public ProvisioningToolException(string message) : base(message)
        {
        }

        public ProvisioningToolException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ProvisioningToolException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
