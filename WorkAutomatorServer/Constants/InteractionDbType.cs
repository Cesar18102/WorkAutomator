using System;

namespace Constants
{
    [Flags]
    public enum InteractionDbType : byte
    {
        READ = 1,
        CREATE = 2,
        UPDATE = 4,
        DELETE = 8
    }
}
