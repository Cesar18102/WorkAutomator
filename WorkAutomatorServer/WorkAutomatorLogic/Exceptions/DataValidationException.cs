using System;
using System.Collections.Generic;
using System.Linq;

namespace WorkAutomatorLogic.Exceptions
{
    public class DataValidationException : Exception
    {
        public List<InvalidFieldInfo> InvalidFieldInfos { get; private set; } = 
            new List<InvalidFieldInfo>();

        public override string Message => "There were some validation errors: " +
            string.Join("\n", InvalidFieldInfos.Select(info => $"{info.Field}: {info.Reason}"));
    }

    public class InvalidFieldInfo
    {
        public Type Type { get; private set; }
        public string Field { get; private set; }
        public string Reason { get; private set; }

        public InvalidFieldInfo(Type type, string field, string reason)
        {
            Type = type;
            Field = field;
            Reason = reason;
        }
    }
}
