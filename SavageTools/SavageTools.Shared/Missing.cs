using System;
using Tortuga.Anchor.Collections;

namespace SavageTools
{
    public static class Missing
    {
        /// <summary>
        /// Removes all the elements that match the conditions defined by the specified predicate.
        /// </summary>
        /// <param name="match"> The System.Predicate`1 delegate that defines the conditions of the elements to remove.</param>
        /// <returns>The number of elements removed.</returns>
        public static int RemoveAll<T>(this ObservableCollectionExtended<T> list, Predicate<T> match)
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
    }


}


