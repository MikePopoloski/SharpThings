using System;
using System.Threading;

namespace SharpThings {
    /// <summary>
    /// Defines a key that can be used in a sorted collection to allow duplicates in a stable sort.
    /// </summary>
    public struct StableKey<T> : IComparable<StableKey<T>>, IEquatable<StableKey<T>>, IComparable where T : IComparable {
        static long next;
        long sequence;

        /// <summary>
        /// The key that will be compared.
        /// </summary>
        public T Key { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="StableKey{T}"/> struct.
        /// </summary>
        /// <param name="key">The key.</param>
        public StableKey (T key) {
            sequence = Interlocked.Increment(ref next);
            Key = key;
        }

        /// <summary>
        /// Compares the current object with another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// A value that indicates the relative order of the objects being compared. The return value has the following meanings: Value Meaning Less than zero This object is less than the <paramref name="other" /> parameter.Zero This object is equal to <paramref name="other" />. Greater than zero This object is greater than <paramref name="other" />.
        /// </returns>
        public int CompareTo (StableKey<T> other) {
            var cmp = Key.CompareTo(other.Key);
            if (cmp == 0)
                cmp = sequence.CompareTo(other.sequence);

            return cmp;
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.
        /// </returns>
        public bool Equals (StableKey<T> other) => sequence == other.sequence && Key.Equals(other.Key);

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/>, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals (object obj) {
            var other = obj as StableKey<T>?;
            if (other == null)
                return false;

            return Equals(other.Value);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode () => sequence.GetHashCode() ^ Key.GetHashCode();

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString () => Key.ToString();

        /// <summary>
        /// Performs an implicit conversion from the source key type to <see cref="StableKey{T}"/>.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator StableKey<T>(T key) => new StableKey<T>(key);

        /// <summary>
        /// Performs an implicit conversion from <see cref="StableKey{T}"/> to the source key type.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator T (StableKey<T> key) => key.Key;

        /// <summary>
        /// Implements the equality operator.
        /// </summary>
        /// <param name="left">The left side of the operator.</param>
        /// <param name="right">The right side of the operator.</param>
        /// <returns>
        /// <c>true</c> if the two sides are equal; otherwise, <c>false</c>.
        /// </returns>
        public static bool operator ==(StableKey<T> left, StableKey<T> right) => left.Equals(right);

        /// <summary>
        /// Implements the inequality operator.
        /// </summary>
        /// <param name="left">The left side of the operator.</param>
        /// <param name="right">The right side of the operator.</param>
        /// <returns>
        /// <c>true</c> if the two sides are not equal; otherwise, <c>false</c>.
        /// </returns>
        public static bool operator !=(StableKey<T> left, StableKey<T> right) => !(left == right);

        /// <summary>
        /// Implements the less-than operator.
        /// </summary>
        /// <param name="left">The left side of the operator.</param>
        /// <param name="right">The right side of the operator.</param>
        /// <returns>
        /// <c>true</c> if the left side is less than the right side; otherwise, <c>false</c>.
        /// </returns>
        public static bool operator <(StableKey<T> left, StableKey<T> right) => left.CompareTo(right) < 0;

        /// <summary>
        /// Implements the greater-than operator.
        /// </summary>
        /// <param name="left">The left side of the operator.</param>
        /// <param name="right">The right side of the operator.</param>
        /// <returns>
        /// <c>true</c> if the left side is greater than the right side; otherwise, <c>false</c>.
        /// </returns>
        public static bool operator >(StableKey<T> left, StableKey<T> right) => left.CompareTo(right) > 0;

        int IComparable.CompareTo (object obj) => CompareTo((StableKey<T>)obj);
    }
}
