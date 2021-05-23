using Newtonsoft.Json;

using System;

namespace WorkAutomatorServer.Exceptions
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class ServerExceptionBase : Exception
    {
        [JsonProperty("message")]
        public override string Message => base.Message;
    }
}
