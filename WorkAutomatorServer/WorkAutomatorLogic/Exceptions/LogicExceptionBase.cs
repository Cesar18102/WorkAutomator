using System;

using Newtonsoft.Json;

namespace WorkAutomatorLogic.Exceptions
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class LogicExceptionBase : Exception 
    {
        public LogicExceptionBase() : base() { }
        public LogicExceptionBase(string message) : base(message) { }

        [JsonProperty("message")]
        public override string Message => base.Message;
    }
}
