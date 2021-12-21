using System;
using System.Linq;
using System.Collections.Generic;

namespace Calc
{
    public static class GeneralC
    {
        public static T[] RemoveAt<T>(this T[] source, int index)
        {
            T[] result = new T[source.Length - 1];
            if (index > 0)
                Array.Copy(source, 0, result, 0, index);

            if (index < source.Length - 1)
                Array.Copy(source, index + 1, result, index, source.Length - index - 1);

            return result;
        }

        public static List<T> CreateList<T>(params T[] values)
        {
            return new List<T>(values);
        }

        public static IEnumerable<IEnumerable<T>> GetPermutations<T>(IEnumerable<T> list, int length)
        {
            if (length == 1) return list.Select(t => new T[] { t });

            return GetPermutations(list, length - 1)
                .SelectMany(t => list, (t1, t2) => t1.Concat(new T[] { t2 }));
        }
    }
}
