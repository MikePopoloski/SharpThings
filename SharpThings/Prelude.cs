using System;
using System.Diagnostics;

namespace SharpThings {
    /// <summary>
    /// A set of static methods designed to be imported into the global namespace.
    /// </summary>
    public static class Prelude {
        /// <summary>
        /// Asserts that the given reference is not null.
        /// </summary>
        /// <typeparam name="T">The type of the reference.</typeparam>
        /// <param name="value">The reference to check.</param>
        [Conditional("DEBUG")]
        public static void assert<T>(T value) where T : class {
            if (value == null)
                throw new AssertionFailedException("Reference of type '\{typeof(T)}' is null.");
        }

        /// <summary>
        /// Asserts that the given struct is not null.
        /// </summary>
        /// <typeparam name="T">The type of the struct.</typeparam>
        /// <param name="value">The value to check.</param>
        [Conditional("DEBUG")]
        public static void assert<T>(Nullable<T> value) where T : struct {
            if (!value.HasValue)
                throw new AssertionFailedException("Nullable struct of type '\{typeof(T)}' is null.");
        }

        /// <summary>
        /// Asserts that the given string is not null or empty.
        /// </summary>
        /// <param name="value">The string to check.</param>
        [Conditional("DEBUG")]
        public static void assert (string value) {
            if (string.IsNullOrEmpty(value))
                throw new AssertionFailedException("String is null or empty.");
        }

        /// <summary>
        /// Asserts that the given value is true.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <param name="message">The message to display if the value is false.</param>
        [Conditional("DEBUG")]
        public static void assert (bool value, string message = "Expression evaluated to false.") {
            if (!value)
                throw new AssertionFailedException(message);
        }
    }
}
