using Newtonsoft.Json;

using System.Collections.Generic;
using System.Linq;

namespace WorkAutomatorLogic.Exceptions
{
    public class NotPermittedException : LogicExceptionBase
    {
        public override string Message => "Not enough permissions";

        [JsonProperty("required_permissions")]
        public ICollection<string> RequiredPermissions { get; private set; }

        public NotPermittedException(params string[] requiredPermissions)
        {
            RequiredPermissions = requiredPermissions.ToList();
        }
    }
}
