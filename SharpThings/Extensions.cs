using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using SharpThings.Prelude;

namespace SharpThings {
    /// <summary>
    /// Contains extension methods for various types.
    /// </summary>
    public static class Extensions {
        /// <summary>
        /// Creates a set from the given source sequence.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the sequence.</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <returns>The set containing the elements of the sequence.</returns>
        public static HashSet<T> ToSet<T>(this IEnumerable<T> source) => new HashSet<T>(source);

        /// <summary>
        /// Gets the value mapped to the given key, or the default value if none is found.
        /// </summary>
        /// <typeparam name="TKey">The type of the keys in the dictionary.</typeparam>
        /// <typeparam name="TValue">The type of the values in the dictionary.</typeparam>
        /// <param name="source">The source dictionary.</param>
        /// <param name="key">The key to get.</param>
        /// <returns>The item for the corresponding key.</returns>
        public static TValue Get<TKey, TValue>(this IDictionary<TKey, TValue> source, TKey key) {
            assert(source);

            TValue value;
            if (source.TryGetValue(key, out value))
                return value;

            return default(TValue);
        }

        /// <summary>
        /// Returns the top element of the stack, or default(T) if it is empty.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the stack.</typeparam>
        /// <param name="source">The source stack.</param>
        /// <returns>The top element on the stack, or default(T) if it is empty.</returns>
        public static T PeekOrDefault<T>(this Stack<T> source) {
            assert(source);

            if (source.Count == 0)
                return default(T);

            return source.Peek();
        }

        /// <summary>
        /// Removes and returns the top element of the stack, or default(T) if it is empty.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the stack.</typeparam>
        /// <param name="source">The source stack.</param>
        /// <returns>The top element on the stack, or default(T) if it is empty.</returns>
        public static T PopOrDefault<T>(this Stack<T> source) {
            assert(source);

            if (source.Count == 0)
                return default(T);

            return source.Pop();
        }

        /// <summary>
        /// Finds the element in a sequence with the largest selected value.
        /// </summary>
        /// <typeparam name="TKey">The type of the elements in the sequence.</typeparam>
        /// <typeparam name="TValue">The type of each element's value.</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <param name="selector">A selector function to get an element's value.</param>
        /// <returns>The element in the sequence with the largest value.</returns>
        public static TKey MaxElement<TKey, TValue>(this IEnumerable<TKey> source, Func<TKey, TValue> selector) {
            assert(source);
            assert(selector);

            var comparer = Comparer<TValue>.Default;
            var max = default(TKey);
            var value = default(TValue);

            foreach (var element in source) {
                var currentValue = selector(element);
                if (comparer.Compare(value, currentValue) < 0) {
                    max = element;
                    value = currentValue;
                }
            }

            return max;
        }

        /// <summary>
        /// Finds the element in a sequence with the smallest selected value.
        /// </summary>
        /// <typeparam name="TKey">The type of the elements in the sequence.</typeparam>
        /// <typeparam name="TValue">The type of each element's value.</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <param name="selector">A selector function to get an element's value.</param>
        /// <returns>The element in the sequence with the smallest value.</returns>
        public static TKey MinElement<TKey, TValue>(this IEnumerable<TKey> source, Func<TKey, TValue> selector) {
            assert(source);
            assert(selector);

            var comparer = Comparer<TValue>.Default;
            var min = default(TKey);
            var value = default(TValue);

            bool first = true;
            foreach (var element in source) {
                var currentValue = selector(element);
                if (first || comparer.Compare(value, currentValue) > 0) {
                    min = element;
                    value = currentValue;
                    first = false;
                }
            }

            return min;
        }

        /// <summary>
        /// Removes the specified key from the dictionary.
        /// </summary>
        /// <typeparam name="TKey">The type of keys in the dictionary.</typeparam>
        /// <typeparam name="TValue">The type of values in the dictionary.</typeparam>
        /// <param name="map">The dictionary from which to remove a value.</param>
        /// <param name="key">The key of the item to remove.</param>
        /// <returns><c>true</c> if the item was removed; otherwise, <c>false</c>.</returns>
        public static bool Remove<TKey, TValue>(this ConcurrentDictionary<TKey, TValue> map, TKey key) {
            assert(map);

            TValue value;
            return map.TryRemove(key, out value);
        }
    }
}
