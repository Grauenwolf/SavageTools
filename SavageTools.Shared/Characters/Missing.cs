using System;
using System.Collections.Generic;

namespace Tortuga.Anchor
{
    public static class Missing
    {
        /// <summary>
        /// In place sort.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="entries"></param>
        /// <param name="comparer"></param>
        public static void Sort<T, TKey>(this IList<T> entries, Func<T, TKey> comparer) where TKey : IComparable<TKey>
        {
            if (entries == null)
                throw new ArgumentNullException("entries", "entries is null.");
            if (comparer == null)
                throw new ArgumentNullException("comparer", "comparer is null.");

            entries.Sort(new CompareByKey<T, TKey>(comparer));
        }

        class CompareByKey<T, TKey> : IComparer<T> where TKey : IComparable<TKey>
        {
            private Func<T, TKey> m_GetKey;

            public CompareByKey(Func<T, TKey> getKey)
            {
                m_GetKey = getKey;
            }

            int IComparer<T>.Compare(T x, T y)
            {
                return m_GetKey(x).CompareTo(m_GetKey(y));
            }
        }
    }
}
