using System;
using System.Runtime.Serialization;

namespace Altairis.AskMe.Web.DotVVM {
    public class ObjectNotFoundException : Exception {
        public ObjectNotFoundException() {
        }

        public ObjectNotFoundException(string message) : base(message) {
        }

        public ObjectNotFoundException(string message, Exception innerException) : base(message, innerException) {
        }

        protected ObjectNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context) {
        }
    }
}
