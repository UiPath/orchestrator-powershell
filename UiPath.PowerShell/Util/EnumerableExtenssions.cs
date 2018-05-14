using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace UiPath.PowerShell.Util
{
    public static class EnumerableExtenssions
    {
        public static IEnumerable<T> MergeAddRemove<T, K>(
            this IEnumerable<T> existing,
            IEnumerable<T> toAdd,
            IEnumerable<T> toRemove,
            Func<T, K> getKey)
        {
            return MergeAddRemove(existing, toAdd, toRemove, getKey, Comparer<K>.Default);
        }


        public static IEnumerable<T> MergeAddRemove<T, K>(
            this IEnumerable<T> existing,
            IEnumerable<T> toAdd,
            IEnumerable<T> toRemove,
            Func<T, K> getKey,
            IComparer<K> comparer)
        {
            // Allow for NULLs because PowerShell
            toAdd = toAdd ?? Enumerable.Empty<T>();
            toRemove = toRemove ?? Enumerable.Empty<T>();

            var sortedExisting = existing.OrderBy(getKey);
            var sortedAdd = toAdd.OrderBy(getKey);
            var sortedRemove = toRemove.OrderBy(getKey);

            var existingIT = sortedExisting.GetEnumerator();
            var addIT = sortedAdd.GetEnumerator();
            var removeIT = sortedRemove.GetEnumerator();

            bool hasExisting = existingIT.MoveNext();
            bool hasAdd = addIT.MoveNext();
            bool hasRemove = removeIT.MoveNext();

            while (hasExisting || hasAdd || hasRemove)
            {
                K existingKey = hasExisting ? getKey(existingIT.Current) : default(K);
                K addKey = hasAdd ? getKey(addIT.Current) : default(K);
                K removeKey = hasRemove ? getKey(removeIT.Current) : default(K);

                int compareExistingRemove = comparer.Compare(existingKey, removeKey);
                int compareExistingAdd = comparer.Compare(existingKey, addKey);

                // The current ADD key is less than current EXISTING key then return the ADD and advance only ADD
                // or EXISTING is exhausted
                //
                if ((hasExisting && hasAdd && 1 == compareExistingAdd) ||
                    (!hasExisting && hasAdd))
                {
                    yield return addIT.Current;
                    hasAdd = addIT.MoveNext();
                    continue;
                }

                // The current ADD is equal to current EXISTING: return the ADD value and advance both
                //
                if (hasExisting && hasAdd && 0 == compareExistingAdd)
                {
                    yield return addIT.Current;
                    hasAdd = addIT.MoveNext();
                    hasExisting = existingIT.MoveNext();

                    // also advance Remove iterator if the key match. Both ADD and REMOVE conflict ADD wins
                    if (hasRemove && 0 == compareExistingRemove)
                    {
                        hasRemove = removeIT.MoveNext();
                    }
                    continue;
                }

                // The current remove is smaller than existing or existing is exhausted, advance remove
                //
                if ((hasExisting && hasRemove && 1 == compareExistingRemove) ||
                    (!hasExisting && hasRemove))
                {
                    hasRemove = removeIT.MoveNext();
                    continue;
                }

                // the current existing is removed
                //
                if (hasExisting && hasRemove && 0 == compareExistingRemove)
                {
                    // no yield return, just advance
                    hasExisting = existingIT.MoveNext();
                    hasRemove = removeIT.MoveNext();
                    continue;
                }

                // The current existing is smaller than remove, or remove is exhausted
                //
                if ((hasExisting && hasRemove && -1 == compareExistingRemove) ||
                    (hasExisting && !hasRemove))
                {
                    yield return existingIT.Current;
                    hasExisting = existingIT.MoveNext();
                    continue;
                }

                // Should never reach here
                //
                Debug.Assert(false);
            }
        }
    }
}
