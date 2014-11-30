using System.Collections.Generic;
using System.Globalization;

namespace SharpThings {
    /// <summary>
    /// Contains utility functions for various numeric operations.
    /// </summary>
    public static class Numeric {
        static readonly string[] ByteSuffixes = { "bytes", "KB", "MB", "GB", "TB" };

        /// <summary>
        /// Formats a byte count into a human-friendly string.
        /// </summary>
        /// <param name="bytes">The number of bytes.</param>
        /// <returns>The formatted string.</returns>
        public static string FormatBytes (double bytes) {
            int i = 0;
            while (bytes >= 1024) {
                bytes /= 1024.0;
                i++;
            }

            return string.Format(CultureInfo.CurrentCulture, "{0:G3} {1}", bytes, ByteSuffixes[i]);
        }

        /// <summary>
        /// Clamps the input to the specified range.
        /// </summary>
        /// <typeparam name="T">The type of values being clamped.</typeparam>
        /// <param name="input">The input to clamp.</param>
        /// <param name="min">The minimum value of the range.</param>
        /// <param name="max">The maximum value of the range.</param>
        /// <returns>The clamped value.</returns>
        public static T Clamp<T>(T input, T min, T max) {
            var comparer = Comparer<T>.Default;
            if (comparer.Compare(input, min) < 0)
                return min;

            if (comparer.Compare(input, max) > 0)
                return max;

            return input;
        }
    }
}
