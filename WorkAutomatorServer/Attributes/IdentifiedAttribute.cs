using System;

namespace Attributes
{
    [AttributeUsage(AttributeTargets.Parameter)]
    public class IdentifiedAttribute : Attribute
    {
        public int Depth { get; set; } = 0;
    }
}
