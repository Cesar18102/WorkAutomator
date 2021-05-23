using System;

using Newtonsoft.Json;

namespace WorkAutomatorServer.Output
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class ErrorPart
    {
        [JsonProperty("code")]
        public int Code { get; set; }

        [JsonProperty("exception")]
        public Exception Exception { get; set; }

        public ErrorPart(int code, Exception exception)
        {
            Code = code;
            Exception = exception;
        }
    }
}