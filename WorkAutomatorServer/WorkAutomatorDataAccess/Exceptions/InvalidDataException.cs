using System;
using System.Collections.Generic;

using WorkAutomatorDataAccess.Entities;

namespace WorkAutomatorDataAccess.Exceptions
{
    public class InvalidDataException<TEntity> : Exception where TEntity : EntityBase
    {
        public List<InvalidFieldInfo<TEntity>> InvalidFieldInfos { get; private set; } = 
            new List<InvalidFieldInfo<TEntity>>();
    }

    public class InvalidFieldInfo<TEntity> where TEntity : EntityBase
    {
        public string FieldName { get; private set; }
        public string InvalidReason { get; private set; }

        public InvalidFieldInfo(string fieldName, string invalidReason)
        {
            FieldName = fieldName;
            InvalidReason = invalidReason;
        }
    }
}
