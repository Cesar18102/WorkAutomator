using System;
using System.Linq;
using System.Collections.Generic;

namespace WorkAutomatorLogic.Extensions
{
    public static class EnumExtensions
    {
        public static IEnumerable<TEnum> GetFlags<TEnum>(this TEnum e) where TEnum : Enum
        {
            return Enum.GetValues(e.GetType()).Cast<Enum>().Where(e.HasFlag).Cast<TEnum>();
        }
    }
}
