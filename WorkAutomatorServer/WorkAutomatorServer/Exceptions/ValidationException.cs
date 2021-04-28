using System.Collections.Generic;
using System.Runtime.Remoting;

using Newtonsoft.Json;

namespace WorkAutomatorServer.Exceptions
{
    public class ValidationException : ServerExceptionBase
    {
        public override string Message => "Some fields are invalid or required not presented";

        [JsonProperty("invalid_messages")]
        public IEnumerable<string> InvalidMessages { get; private set; }

        public ValidationException(IEnumerable<string> messages)
        {
            InvalidMessages = messages;
        }

        public ValidationException(params string[] messages) :
            this(messages as IEnumerable<string>)
        { }
    }
}