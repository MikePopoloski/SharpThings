using System;
using System.Text;

namespace SharpThings {
    /// <summary>
    /// Contains utility methods for generating hash values.
    /// </summary>
    public unsafe static class Hash {
        const uint intSize = sizeof(int);

        [ThreadStatic]
        static readonly int[] tempSpace = new int[8];
        static readonly uint AppSeed = (uint)new Random().Next();

        /// <summary>
        /// Generates a hash for a set of members using the Murmur3 algorithm.
        /// </summary>
        /// <typeparam name="TMember1">The type of the first argument.</typeparam>
        /// <typeparam name="TMember2">The type of the second argument.</typeparam>
        /// <typeparam name="TMember3">The type of the third argument.</typeparam>
        /// <param name="t">The first member.</param>
        /// <param name="u">The second member.</param>
        /// <param name="v">The third member.</param>
        /// <returns>
        /// The hash value for the given members.
        /// </returns>
        public static int Murmur3<TMember1, TMember2, TMember3>(TMember1 t, TMember2 u, TMember3 v) {
            fixed (int* ptr = tempSpace)
            {
                *ptr = t.GetHashCode();
                *(ptr + 1) = u.GetHashCode();
                *(ptr + 2) = v.GetHashCode();
                return Murmur3((byte*)ptr, intSize * 3, AppSeed);
            }
        }

        /// <summary>
        /// Generates a hash for a set of members using the Murmur3 algorithm.
        /// </summary>
        /// <typeparam name="TMember1">The type of the first argument.</typeparam>
        /// <typeparam name="TMember2">The type of the second argument.</typeparam>
        /// <typeparam name="TMember3">The type of the third argument.</typeparam>
        /// <typeparam name="TMember4">The type of the fourth argument.</typeparam>
        /// <param name="t">The first member.</param>
        /// <param name="u">The second member.</param>
        /// <param name="v">The third member.</param>
        /// <param name="w">The fourth member.</param>
        /// <returns>
        /// The hash value for the given members.
        /// </returns>
        public static int Murmur3<TMember1, TMember2, TMember3, TMember4>(TMember1 t, TMember2 u, TMember3 v, TMember4 w) {
            fixed (int* ptr = tempSpace)
            {
                *ptr = t.GetHashCode();
                *(ptr + 1) = u.GetHashCode();
                *(ptr + 2) = v.GetHashCode();
                *(ptr + 3) = w.GetHashCode();
                return Murmur3((byte*)ptr, intSize * 4, AppSeed);
            }
        }

        /// <summary>
        /// Generates a hash for a set of members using the Murmur3 algorithm.
        /// </summary>
        /// <typeparam name="TMember1">The type of the first argument.</typeparam>
        /// <typeparam name="TMember2">The type of the second argument.</typeparam>
        /// <typeparam name="TMember3">The type of the third argument.</typeparam>
        /// <typeparam name="TMember4">The type of the fourth argument.</typeparam>
        /// <typeparam name="TMember5">The type of the fifth member.</typeparam>
        /// <typeparam name="TMember6">The type of the sixth member.</typeparam>
        /// <typeparam name="TMember7">The type of the seventh member.</typeparam>
        /// <param name="t">The first member.</param>
        /// <param name="u">The second member.</param>
        /// <param name="v">The third member.</param>
        /// <param name="w">The fourth member.</param>
        /// <param name="x">The fifth member.</param>
        /// <param name="y">The sixth member.</param>
        /// <param name="z">The seventh member.</param>
        /// <returns>
        /// The hash value for the given members.
        /// </returns>
        public static int Murmur3<
            TMember1,
            TMember2,
            TMember3,
            TMember4,
            TMember5,
            TMember6,
            TMember7
            >(TMember1 t, TMember2 u, TMember3 v, TMember4 w, TMember5 x, TMember6 y, TMember7 z) {
            fixed (int* ptr = tempSpace)
            {
                *ptr = t.GetHashCode();
                *(ptr + 1) = u.GetHashCode();
                *(ptr + 2) = v.GetHashCode();
                *(ptr + 3) = w.GetHashCode();
                *(ptr + 4) = x.GetHashCode();
                *(ptr + 5) = y.GetHashCode();
                *(ptr + 6) = z.GetHashCode();
                return Murmur3((byte*)ptr, intSize * 7, AppSeed);
            }
        }

        /// <summary>
        /// Generates a hash for a string using the Murmur3 algorithm.
        /// </summary>
        /// <param name="input">The input string to hash.</param>
        /// <param name="seed">The starting seed.</param>
        /// <returns>The hashed string.</returns>
        public static int Murmur3 (string input, int seed) {
            var bytes = Encoding.UTF8.GetBytes(input);
            fixed (byte* ptr = bytes)
                return Murmur3(ptr, (uint)bytes.Length, (uint)seed);
        }

        /// <summary>
        /// Generates a hash for a chunk of bytes using the Murmur3 algorithm.
        /// </summary>
        /// <param name="input">The input bytes to hash.</param>
        /// <param name="length">The number of bytes to take from the input.</param>
        /// <param name="seed">The starting seed.</param>
        /// <returns>The hashed bytes.</returns>
        public static int Murmur3 (byte[] input, int length, int seed) {
            fixed (byte* ptr = input)
                return Murmur3(ptr, (uint)length, (uint)seed);
        }

        static int Murmur3 (byte* data, uint length, uint seed) {
            const uint c1 = 0xcc9e2d51;
            const uint c2 = 0x1b873593;
            const uint c3 = 0xe6546b64;
            const uint c4 = 0x85ebca6b;
            const uint c5 = 0xc2b2ae35;

            uint h1 = seed;
            uint k1 = 0;
            uint nblocks = (length / intSize) * intSize;
            uint i = 0;
            for (; i < nblocks; i += intSize) {
                k1 = (uint)(data[0] | data[1] << 8 | data[2] << 16 | data[3] << 24);
                k1 *= c1;
                k1 = (k1 << 15) | (k1 >> (32 - 15));
                k1 *= c2;
                h1 ^= k1;
                h1 = (h1 << 13) | (h1 >> (32 - 13));
                h1 = h1 * 5 + c3;
                data += 4;
            }

            k1 = 0;
            switch (length - i) {
                case 3: k1 = (uint)(data[1] << 8 | data[2] << 16); goto case 1;
                case 2: k1 = (uint)(data[1] << 8); goto case 1;
                case 1:
                    k1 |= (uint)data[0];
                    k1 *= c1;
                    k1 = (k1 << 15) | (k1 >> (32 - 15));
                    k1 *= c2;
                    h1 ^= k1;
                    break;
            }

            // finalization, magic chants to wrap it all up (avalanche)
            h1 ^= length;
            h1 ^= h1 >> 16;
            h1 *= c4;
            h1 ^= h1 >> 13;
            h1 *= c5;
            h1 ^= h1 >> 16;

            return unchecked((int)h1);
        }
    }
}
