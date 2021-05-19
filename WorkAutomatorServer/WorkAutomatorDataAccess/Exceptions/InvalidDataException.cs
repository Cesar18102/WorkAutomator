using System;
using System.Collections.Generic;

using WorkAutomatorDataAccess.Entities;

namespace WorkAutomatorDataAccess.Exceptions
{
    public class InvalidDataException : Exception
    {
        public List<InvalidFieldInfo> InvalidFieldInfos { get; private set; } = 
            new List<InvalidFieldInfo>();
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
