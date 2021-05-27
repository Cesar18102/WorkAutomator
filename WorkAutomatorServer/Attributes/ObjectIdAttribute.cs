using System;

using Constants;

namespace Attributes
{
    public class ObjectIdAttribute : Attribute
    {
        public DbTable Table { get; private set; }

        public ObjectIdAttribute(DbTable table)
        {
            Table = table;
        }
    }
}
