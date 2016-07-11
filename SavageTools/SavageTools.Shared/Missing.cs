using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SavageTools
{
    public static class Missing
    {
        /// <summary>
        /// Removes all the elements that match the conditions defined by the specified predicate.
        /// </summary>
        /// <param name="match"> The System.Predicate`1 delegate that defines the conditions of the elements to remove.</param>
        /// <returns>The number of elements removed.</returns>
        public static int RemoveAll<T>(this ObservableCollection<T> list, Predicate<T> match)
        {
            if (match == null)
                throw new ArgumentNullException(nameof(match), $"{nameof(match)} is null.");

            var count = 0;
            for (int i = list.Count - 1; i >= 0; i--)
            {
                if (match(list[i]))
                {
                    list.RemoveAt(i);
                    count += 1;
                }
            }
            return count;
        }

        /// <summary>
        /// Creates an IComparer&lt;T&gt; from an IComparable&lt;T&gt;
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="comparable">Used for type inference, may be null.</param>
        /// <returns>IComparer&lt;T&gt;.</returns>
        public static IComparer<T> GetComparer<T>(this T comparable) where T : IComparable<T>
        {
            return new ComparableToComparer<T>();
        }

        /// <summary>
        /// Creates an IComparer&lt;T&gt; from an IComparable&lt;T&gt;
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>IComparer&lt;T&gt;.</returns>
        public static IComparer<T> GetComparer<T>() where T : IComparable<T>
        {
            return new ComparableToComparer<T>();
        }

        private const int InsertionSortLimit = 16;


        public static void Sort<T>(this IList<T> entries) where T : IComparable<T>
        {
            if (entries == null)
                throw new ArgumentNullException("entries", "entries is null.");

            Sort(entries, 0, entries.Count - 1, new Random(), GetComparer<T>());
        }

        public static void Sort<T>(this IList<T> entries, IComparer<T> comparer)
        {
            if (entries == null)
                throw new ArgumentNullException("entries", "entries is null.");
            if (comparer == null)
                throw new ArgumentNullException("comparer", "comparer is null.");

            Sort(entries, 0, entries.Count - 1, new Random(), comparer);
        }

        public static void Sort<T>(this IList<T> entries, Func<T, T, int> comparer)
        {
            if (entries == null)
                throw new ArgumentNullException("entries", "entries is null.");
            if (comparer == null)
                throw new ArgumentNullException("comparer", "comparer is null.");

            Sort(entries, 0, entries.Count - 1, new Random(), new Comparer<T>(comparer));
        }

        public static void Sort<T, TKey>(this IList<T> entries, Func<T, TKey> comparer) where TKey : IComparable<TKey>
        {
            if (entries == null)
                throw new ArgumentNullException("entries", "entries is null.");
            if (comparer == null)
                throw new ArgumentNullException("comparer", "comparer is null.");

            Sort(entries, 0, entries.Count - 1, new Random(), new CompareByKey<T, TKey>(comparer));
        }

        class ComparableToComparer<T> : IComparer<T> where T : IComparable<T>
        {
            int IComparer<T>.Compare(T x, T y)
            {
                return x.CompareTo(y);
            }
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

        class Comparer<T> : IComparer<T>
        {
            private Func<T, T, int> m_Comparer;

            public Comparer(Func<T, T, int> comparer)
            {
                if (comparer == null)
                    throw new ArgumentNullException("comparer", "comparer is null.");

                m_Comparer = comparer;
            }

            int IComparer<T>.Compare(T x, T y)
            {
                return m_Comparer(x, y);
            }
        }


        //static void Sort<T>(IList<T> entries, int first, int last, Random random) where T : IComparable<T>
        //{

        //    var length = last + 1 - first;
        //    while (length > 1)
        //    {
        //        if (length < InsertionSortLimit)
        //        {
        //            InsertionSort(entries, first, last);
        //            return;
        //        }

        //        var median = Pivot(entries, first, last, random);

        //        var left = first;
        //        var right = last;

        //        //partition
        //        while (true)
        //        {
        //            while (median.CompareTo(entries[left]) > 0)
        //                left++;
        //            while (median.CompareTo(entries[right]) < 0)
        //                right--;

        //            if (right <= left)
        //                break;

        //            Swap(entries, left, right);

        //            left++;
        //            right--;
        //        }

        //        if (left == right)
        //        {
        //            left++;
        //            right--;
        //        }


        //        var leftLength = right + 1 - first;
        //        var rightLength = last + 1 - left;


        //        //recursion, shorter partition first
        //        if (leftLength < rightLength)
        //        {
        //            Sort(entries, first, right, random);
        //            first = left;
        //            length = rightLength;
        //        }
        //        else
        //        {
        //            Sort(entries, left, last, random);
        //            last = right;
        //            length = leftLength;
        //        }
        //    }
        //}

        static void Sort<T>(IList<T> entries, int first, int last, Random random, IComparer<T> comparer)
        {
            var length = last + 1 - first;
            while (length > 1)
            {
                if (length < InsertionSortLimit)
                {
                    InsertionSort(entries, first, last, comparer);
                    return;
                }

                var median = Pivot(entries, first, last, random, comparer);

                var left = first;
                var right = last;

                //partition
                while (true)
                {
                    while (comparer.Compare(median, entries[left]) > 0)
                        left++;
                    while (comparer.Compare(median, entries[right]) < 0)
                        right--;

                    if (right <= left)
                        break;

                    Swap(entries, left, right);

                    left++;
                    right--;
                }

                if (left == right)
                {
                    left++;
                    right--;
                }


                var leftLength = right + 1 - first;
                var rightLength = last + 1 - left;

                //recursion, shorter partition first
                if (leftLength < rightLength)
                {
                    Sort(entries, first, right, random, comparer);
                    first = left;
                    length = rightLength;
                }
                else
                {
                    Sort(entries, left, last, random, comparer);
                    last = right;
                    length = leftLength;
                }
            }
        }

        //static T Pivot<T>(IList<T> entries, int first, int last, Random random) where T : IComparable<T>
        //{

        //    var length = last + 1 - first;
        //    var pivotSamples = 2 * (int)Math.Log10(length) + 1;
        //    var sampleSize = Math.Min(pivotSamples, length);
        //    var right = first + sampleSize - 1;
        //    for (var left = first; left <= right; left++)
        //    {
        //        // Random sampling avoids pathological cases
        //        Swap(entries, left, random.Next(left, last + 1));
        //    }

        //    InsertionSort(entries, first, right);
        //    var median = entries[first + sampleSize / 2];
        //    return median;
        //}

        static T Pivot<T>(IList<T> entries, int first, int last, Random random, IComparer<T> comparer)
        {

            var length = last + 1 - first;
            var pivotSamples = 2 * (int)Math.Log10(length) + 1;
            var sampleSize = Math.Min(pivotSamples, length);
            var right = first + sampleSize - 1;
            for (var left = first; left <= right; left++)
            {
                // Random sampling avoids pathological cases

                Swap(entries, left, random.Next(left, last + 1));
            }

            InsertionSort(entries, first, right, comparer);
            var median = entries[first + sampleSize / 2];
            return median;
        }

        static void Swap<T>(IList<T> entries, int index1, int index2)
        {
            if (index1 == index2)
                return;

            var entry = entries[index1];
            entries[index1] = entries[index2];
            entries[index2] = entry;
        }

        static void InsertionSort<T>(IList<T> entries, int first, int last, IComparer<T> comparer)
        {
            for (var i = first + 1; i <= last; i++)
            {
                var entry = entries[i];
                var j = i;
                while (j > first && comparer.Compare(entries[j - 1], entry) > 0)
                    entries[j] = entries[--j];
                entries[j] = entry;
            }
        }

        //static void InsertionSort<T>(IList<T> entries, int first, int last) where T : IComparable<T>
        //{
        //    for (var i = first + 1; i <= last; i++)
        //    {
        //        var entry = entries[i];
        //        var j = i;
        //        while (j > first && entries[j - 1].CompareTo(entry) > 0)
        //            entries[j] = entries[--j];
        //        entries[j] = entry;
        //    }
        //}



    }
}



