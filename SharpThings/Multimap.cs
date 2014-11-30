using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SharpThings.Prelude;

namespace SharpThings {
    /// <summary>
    /// Implements a collection of mappings from keys to sets of corresponding values.
    /// </summary>
    /// <typeparam name="TKey">The type of the keys in the multimap.</typeparam>
    /// <typeparam name="TElement">The type of the values in the multimap.</typeparam>
    public class Multimap<TKey, TElement> : ILookup<TKey, TElement> {
        static readonly IEnumerable<TElement> Empty = Enumerable.Empty<TElement>();

        readonly Dictionary<TKey, LookupGrouping> groups;

        /// <summary>
        /// The number of dictinct keys in the map.
        /// </summary>
        public int Count => groups.Count;

        /// <summary>
        /// Gets the set of values for the given key.
        /// </summary>
        public IEnumerable<TElement> this[TKey key] => groups.Get(key) ?? Empty;

        /// <summary>
        /// Enumerates the set of values in the map, in sorted order.
        /// </summary>
        public IEnumerable<TElement> Values {
            get {
                foreach (var group in groups.Values) {
                    foreach (var item in group)
                        yield return item;
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Multimap&lt;TKey,TElement&gt;" /> class.
        /// </summary>
        public Multimap ()
            : this(null) {
            groups = new Dictionary<TKey, LookupGrouping>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Multimap&lt;TKey,TElement&gt;" /> class.
        /// </summary>
        /// <param name="keyComparer">The key comparer.</param>
        public Multimap (IEqualityComparer<TKey> keyComparer) {
            groups = new Dictionary<TKey, LookupGrouping>(keyComparer);
        }

        /// <summary>
        /// Determines whether a specified key exists in the <see cref="T:System.Linq.ILookup`2" />.
        /// </summary>
        /// <param name="key">The key to search for in the <see cref="T:System.Linq.ILookup`2" />.</param>
        /// <returns>
        /// true if <paramref name="key" /> is in the <see cref="T:System.Linq.ILookup`2" />; otherwise, false.
        /// </returns>
        public bool Contains (TKey key) => groups.Get(key)?.Count > 0;

        /// <summary>
        /// Determines whether a specified key/value pair exists in the <see cref="T:System.Linq.ILookup`2" />.
        /// </summary>
        /// <param name="key">The key to search for in the <see cref="T:System.Linq.ILookup`2" />.</param>
        /// <param name="value">The value to search for.</param>
        /// <returns>
        /// true if the pair is in the <see cref="T:System.Linq.ILookup`2" />; otherwise, false.
        /// </returns>
        public bool Contains (TKey key, TElement value) => groups.Get(key)?.Contains(value) == true;

        /// <summary>
        /// Adds the specified key to the multimap.
        /// </summary>
        /// <param name="key">The key to add.</param>
        /// <param name="value">The value to add.</param>
        public void Add (TKey key, TElement value) {
            LookupGrouping group;
            if (!groups.TryGetValue(key, out group)) {
                group = new LookupGrouping(key);
                groups.Add(key, group);
            }

            group.Add(value);
        }

        /// <summary>
        /// Adds a range of values against a single key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="values">The values to add.</param>
        /// <remarks>
        /// Any values already present will be duplicated
        /// </remarks>
        public void AddRange (TKey key, IEnumerable<TElement> values) {
            assert(values);

            LookupGrouping group;
            if (!groups.TryGetValue(key, out group)) {
                group = new LookupGrouping(key);
                groups.Add(key, group);
            }

            foreach (var value in values)
                group.Add(value);

            if (group.Count == 0)
                groups.Remove(key);
        }

        /// <summary>
        /// Add all key/value pairs from the supplied lookup to the current map.
        /// </summary>
        /// <param name="lookup">The lookup to add.</param>
        /// <remarks>
        /// Any values already present will be duplicated
        /// </remarks>
        public void AddRange (ILookup<TKey, TElement> lookup) {
            assert(lookup);

            foreach (var group in lookup)
                AddRange(group.Key, group);
        }

        /// <summary>
        /// Remove all values from the map for the given key.
        /// </summary>
        /// <param name="key">The key to remove.</param>
        /// <returns>
        /// True if any items were removed; otherwise, false.
        /// </returns>
        public bool Remove (TKey key) => groups.Remove(key);

        /// <summary>
        /// Remove the specific key/value pair from the lookup.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value to remove.</param>
        /// <returns>
        /// True if the item was found, else false
        /// </returns>
        public bool Remove (TKey key, TElement value) {
            LookupGrouping group;
            if (groups.TryGetValue(key, out group)) {
                bool removed = group.Remove(value);
                if (removed && group.Count == 0)
                    groups.Remove(key);

                return removed;
            }

            return false;
        }

        /// <summary>
        /// Trims the inner data-structure to remove any surplus space.
        /// </summary>
        public void TrimExcess () {
            foreach (var group in groups.Values)
                group.TrimExcess();
        }

        /// <summary>
        /// Clears the collection of all elements.
        /// </summary>
        public void Clear () => groups.Clear();

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1" /> that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<IGrouping<TKey, TElement>> GetEnumerator () {
            foreach (var group in groups.Values)
                yield return group;
        }

        IEnumerator IEnumerable.GetEnumerator () => GetEnumerator();

        sealed class LookupGrouping : IGrouping<TKey, TElement> {
            List<TElement> items = new List<TElement>();

            public TKey Key { get; }
            public int Count => items.Count;

            public LookupGrouping (TKey key) {
                Key = key;
            }

            public void Add (TElement item) => items.Add(item);
            public bool Contains (TElement item) => items.Contains(item);
            public bool Remove (TElement item) => items.Remove(item);
            public void TrimExcess () => items.TrimExcess();
            public IEnumerator<TElement> GetEnumerator () => items.GetEnumerator();
            IEnumerator IEnumerable.GetEnumerator () => GetEnumerator();
        }
    }
}
