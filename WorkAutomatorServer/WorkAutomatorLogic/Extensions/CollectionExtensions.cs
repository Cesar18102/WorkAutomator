using System.Collections.Generic;

namespace WorkAutomatorLogic.Extensions
{
    public static class CollectionExtensions
    {
        public static ICollection<T> AddRange<T>(this ICollection<T> target, IEnumerable<T> source)
        {
            foreach (T item in source)
                target.Add(item);

            return target;
        }
    }
}
