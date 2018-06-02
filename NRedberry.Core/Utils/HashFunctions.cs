namespace NRedberry.Core.Utils
{
    /// <summary>
    /// Hashing algorithms. The algorithms was taken from different open sources.
    /// <p/>
    /// <h3>Links:</h3>
    /// <a href="http://www.concentric.net/~ttwang/tech/inthash.htm">http://www.concentric.net/~ttwang/tech/inthash.htm</a><br/>
    /// <a href="http://www.burtleburtle.net/bob/hash/doobs.html">http://www.burtleburtle.net/bob/hash/doobs.html</a><br/>
    /// <a href="http://bretm.home.comcast.net/~bretm/hash">http://bretm.home.comcast.net/~bretm/hash</a><br/>
    /// <a href="http://www.isthe.com/chongo/tech/comp/fnv/">http://www.isthe.com/chongo/tech/comp/fnv/</a><br/>
    /// <a href="https://en.wikipedia.org/wiki/Jenkins_hash_function">https://en.wikipedia.org/wiki/Jenkins_hash_function</a><br/>
    /// <a href="http://sites.google.com/site/murmurhash/">http://sites.google.com/site/murmurhash/</a><br/>
    /// <a href="http://dmy999.com/article/50/murmurhash-2-java-port">http://dmy999.com/article/50/murmurhash-2-java-port</a><br/>
    /// <a href="http://en.wikipedia.org/wiki/MurmurHash">http://en.wikipedia.org/wiki/MurmurHash</a><br/>
    /// </summary>
    public static class HashFunctions
    {
        /// <summary>
        /// Robert Jenkins' 96 bit Mix Function.
        /// <p/>
        /// Variable 'c' contains the input key.When the mixing is complete, variable
        /// 'c' also contains the hash result.Variable 'a', and 'b' contain initialized
        /// random bits. Notice the total number of internal state is 96 bits, much
        /// larger than the final output of 32 bits.Also notice the sequence of
        /// subtractions rolls through variable 'a' to variable 'c' three times.Each
        /// row will act on one variable, mixing in information from the other two
        /// variables, followed by a shift operation.
        ///
        /// <p/>
        /// <p>Subtraction is similar to multiplication in that changes in upper bits
        /// of the key do not influence lower bits of the addition.The 9 bit shift
        /// operations in Robert Jenkins' mixing algorithm shifts the key to the right
        /// 61 bits in total, and shifts the key to the left 34 bits in total.As the
        /// calculation is chained, each exclusive-or doubles the number of states.
        /// There are at least 2^9 different combined versions of the original key,
        ///
        /// shifted by various amounts.That is why a single bit change in the key
        /// can influence widely apart bits in the hash result.
        ///
        /// <p/>
        /// <p>The uniform distribution of the hash function can be determined from
        /// the nature of the subtraction operation.Look at a single bit subtraction
        /// operation between a key, and a random bit. If the random bit is 0, then
        /// the key remains unchanged.If the random bit is 1, then the key will be
        /// flipped. A carry will occur in the case where both the key bit and the
        /// random bit are 1. Subtracting the random bits will cause about half of
        /// the key bits to be flipped.So even if the key is not uniform, subtracting
        /// the random bits will result in uniform distribution.
        /// <p/>
        /// <h3>Links:</h3>
        /// <a href = "http://www.concentric.net/~ttwang/tech/inthash.htm" > http://www.concentric.net/~ttwang/tech/inthash.htm</a><br/>
        /// <a href = "http://www.burtleburtle.net/bob/hash/doobs.html" > http://www.burtleburtle.net/bob/hash/doobs.html</a><br/>
        /// <a href = "http://www.isthe.com/chongo/tech/comp/fnv/" > http://www.isthe.com/chongo/tech/comp/fnv/</a><br/>
        /// <a href = "http://en.wikipedia.org/wiki/Jenkins_hash_function's" > http://en.wikipedia.org/wiki/Jenkins_hash_function's</a><br/>
        /// <a href = "http://en.wikipedia.org/wiki/Fowler%E2%80%93Noll%E2%80%93Vo_hash_function" > http://en.wikipedia.org/wiki/Fowler%E2%80%93Noll%E2%80%93Vo_hash_function</a><br/>
        /// </summary>
        /// <param name="a">initialized random bits</param>
        /// <param name="b">initialized random bits</param>
        /// <param name="c">key to be hashed</param>
        /// <returns>randomized c bits (hashed c)</returns>
        public static int mix(int a, int b, int c)
        {
            unchecked
            {
                a -= b;
                a -= c;
                a ^= c >> 13;
                b -= c;
                b -= a;
                b ^= a << 8;
                c -= a;
                c -= b;
                c ^= b >> 13;
                a -= b;
                a -= c;
                a ^= c >> 12;
                b -= c;
                b -= a;
                b ^= a << 16;
                c -= a;
                c -= b;
                c ^= b >> 5;
                a -= b;
                a -= c;
                a ^= c >> 3;
                b -= c;
                b -= a;
                b ^= a << 10;
                c -= a;
                c -= b;
                c ^= b >> 15;
                return c;
            }
        }

        ///<summary>
        /// Based on an original suggestion on Robert Jenkin's part in 1997 and
        /// Thomas Wang 2007 updates.
        /// <p/>
        /// <h3>Links</h3>
        /// <a href = "http://www.concentric.net/~ttwang/tech/inthash.htm" > http://www.concentric.net/~ttwang/tech/inthash.htm</a><br/>
        /// <a href = "http://en.wikipedia.org/wiki/Jenkins_hash_function's" > http://en.wikipedia.org/wiki/Jenkins_hash_function's</a><br/>
        /// </summary>
        /// <param name="key">key to be hashed</param>
        /// <returns>hashed value</returns>
        public static int JenkinWang32shift(int key)
        {
            unchecked
            {
                // key = (key << 15) - key - 1;
                // (~x) + y is equivalent to y - x - 1 in two's complement representation.
                key = ~key + (key << 15);
                key ^= key >> 12;
                key += key << 2;
                key ^= key >> 4;
                key *= 2057; // key = (key + (key << 3)) + (key << 11);
                key ^= key >> 16;
                return key;
            }
        }

        ///<summary>
        /// This method uses a combination of bit shifts and integer multiplication
        /// to hash the input key.
        /// <p/>
        /// <h3>Links</h3>
        /// <a href="http://www.concentric.net/~ttwang/tech/inthash.htm">http://www.concentric.net/~ttwang/tech/inthash.htm</a><br/>
        /// </summary>
        /// <param name="key">key to be hashed</param>
        /// <returns>hashed value</returns>
        public static int Wang32shiftmult(int key)
        {
            unchecked
            {
                const int c2 = 0x27d4eb2d; // a prime or an odd constant
                key = key ^ 61 ^ (key >> 16);
                key += key << 3;
                key ^= key >> 4;
                key *= c2;
                key ^= key >> 15;
                return key;
            }
        }

        /// <summary>
        /// Based on an original suggestion on Robert Jenkin's part in 1997 and
        /// Thomas Wang 2007 updates.
        /// <p/>
        /// <h3>Links</h3>
        /// <a href = "http://www.concentric.net/~ttwang/tech/inthash.htm" > http://www.concentric.net/~ttwang/tech/inthash.htm</a><br/>
        /// <a href = "http://en.wikipedia.org/wiki/Jenkins_hash_function's" > http://en.wikipedia.org/wiki/Jenkins_hash_function's</a><br/>
        /// </summary>
        /// <param name="key">key to be hashed</param>
        /// <returns>hashed value</returns>
        public static long JenkinWang64shift(long key)
        {
            unchecked
            {
                key = ~key + (key << 21); // key = (key << 21) - key - 1;
                key ^= key >> 24;
                key = key + (key << 3) + (key << 8); // key * 265
                key ^= key >> 14;
                key = key + (key << 2) + (key << 4); // key * 21
                key ^= key >> 28;
                key += key << 31;
                return key;
            }
        }

        /// <summary>
        /// Hashing long to int.
        /// <p/>
        /// <h3>Links</h3>
        /// <a href = "http://www.concentric.net/~ttwang/tech/inthash.htm" > http://www.concentric.net/~ttwang/tech/inthash.htm</a><br/>
        /// </summary>
        /// <param name="key">key to be hashed</param>
        /// <returns>hashed value</returns>
        public static int Wang64to32shift(long key)
        {
            unchecked
            {
                key = ~key + (key << 18); // key = (key << 18) - key - 1;
                key ^= key >> 31;
                key *= 21; // key = (key + (key << 2)) + (key << 4);
                key ^= key >> 11;
                key += key << 6;
                key ^= key >> 22;
                return (int)key;
            }
        }

        /// <summary>
        /// Fowler/Noll/Vo hash algorithms FNV_BASIS constant
        /// <p/>
        /// <h3>Links</h3>
        /// <a href="http://www.isthe.com/chongo/tech/comp/fnv/#FNV-param">http://www.isthe.com/chongo/tech/comp/fnv/#FNV-param</a><br/>
        /// </summary>
        public const long FNV_BASIS = 0x811c9dc5;

        /// <summary>
        /// Fowler/Noll/Vo hash algorithms FNV_PRIME constant for 32 bit hash
        /// <p/>
        /// <h3>Links</h3>
        /// <a href="http://www.isthe.com/chongo/tech/comp/fnv/#FNV-param">http://www.isthe.com/chongo/tech/comp/fnv/#FNV-param</a><br/>
        /// </summary>
        public const long FNV_PRIME_32 = 16777619;

        /// <summary>
        /// Fowler/Noll/Vo hash algorithms FNV_PRIME constant for 64 bit hash
        /// <p/>
        /// <h3>Links</h3>
        /// <a href="http://www.isthe.com/chongo/tech/comp/fnv/#FNV-param">http://www.isthe.com/chongo/tech/comp/fnv/#FNV-param</a><br/>
        /// </summary>
        public const long FNV_PRIME_64 = 1099511628211L;

        /// <summary>
        /// Fowler-Noll-Vo 32 bit hash(FNV-1a) for bytes array.<br/>
        /// <p/>
        /// <h3>Algorithm</h3>
        /// <p/>
        /// <pre>
        /// hash = offset_basis
        /// for each octet_of_data to be hashed
        /// hash = hash xor octet_of_data
        ///    hash = hash * FNV_prime
        /// return hash</pre>
        ///
        /// <h3>Links</h3>
        /// <a href="http://www.isthe.com/chongo/tech/comp/fnv/">http://www.isthe.com/chongo/tech/comp/fnv/</a><br/>
        /// <a href="http://en.wikipedia.org/wiki/Fowler%E2%80%93Noll%E2%80%93Vo_hash_function">http://en.wikipedia.org/wiki/Fowler%E2%80%93Noll%E2%80%93Vo_hash_function</a><br/>
        /// </summary>
        /// <param name="bytes">bytes array to hash</param>
        /// <returns>hash of the initial bytes array</returns>
        public static int FVN32hash(byte[] bytes)
        {
            unchecked
            {
                var hash = FNV_BASIS;
                foreach (var b in bytes)
                {
                    hash ^= 0xFF & b;
                    hash *= FNV_PRIME_32;
                }
                return (int)hash;
            }
        }

        /// <summary>
        /// Fowler-Noll-Vo 32 bit hash(FNV-1a) for integer key.This is big-endian version (native endianess of JVM).<br/>
        /// <p/>
        /// <h3>Algorithm</h3>
        /// <p/>
        /// <pre>
        /// hash = offset_basis
        /// for each octet_of_data to be hashed
        /// hash = hash xor octet_of_data
        ///    hash = hash* FNV_prime
        /// return hash</pre>
        ///
        /// <h3>Links</h3>
        /// <a href = "http://www.isthe.com/chongo/tech/comp/fnv/" > http://www.isthe.com/chongo/tech/comp/fnv/</a><br/>
        /// <a href = "http://en.wikipedia.org/wiki/Fowler%E2%80%93Noll%E2%80%93Vo_hash_function" > http://en.wikipedia.org/wiki/Fowler%E2%80%93Noll%E2%80%93Vo_hash_function</a><br/>
        /// </summary>
        /// <param name="c">integer key to be hashed</param>
        /// <returns>32 bit hash</returns>
        public static int FVN32hash(int c)
        {
            unchecked
            {
                var hash = FNV_BASIS;
                hash ^= c >> 24;
                hash *= FNV_PRIME_32;
                hash ^= 0xFF & (c >> 16);
                hash *= FNV_PRIME_32;
                hash ^= 0xFF & (c >> 8);
                hash *= FNV_PRIME_32;
                hash ^= 0xFF & c;
                hash *= FNV_PRIME_32;
                return (int)hash;
            }
        }

        /// <summary>
        /// Fowler-Noll-Vo 64 bit hash(FNV-1a) for bytes array.<br/>
        /// <p/>
        /// <h3>Algorithm</h3>
        /// <p/>
        /// <pre>
        /// hash = offset_basis
        /// for each octet_of_data to be hashed
        /// hash = hash xor octet_of_data
        ///    hash = hash * FNV_prime
        /// return hash</pre>
        ///
        /// <h3>Links</h3>
        /// <a href = "http://www.isthe.com/chongo/tech/comp/fnv/" > http://www.isthe.com/chongo/tech/comp/fnv/</a><br/>
        /// <a href = "http://en.wikipedia.org/wiki/Fowler%E2%80%93Noll%E2%80%93Vo_hash_function" > http://en.wikipedia.org/wiki/Fowler%E2%80%93Noll%E2%80%93Vo_hash_function</a><br/>
        /// </summary>
        /// <param name="bytes">bytes array to hash</param>
        /// <returns>64 bit hash</returns>
        public static long FVN64hash(byte[] bytes)
        {
            unchecked
            {
                var hash = FNV_BASIS;
                for (var i = 0; i < bytes.Length; i++)
                {
                    hash ^= 0xFF & bytes[i];
                    hash *= FNV_PRIME_64;
                }
                return hash;
            }
        }

        /// <summary>
        /// Fowler-Noll-Vo 64 bit hash(FNV-1a) for long key.This is big-endian version (native endianess of JVM).<br/>
        /// <p/>
        /// <h3>Algorithm</h3>
        /// <p/>
        /// <pre>
        /// hash = offset_basis
        /// for each octet_of_data to be hashed
        /// hash = hash xor octet_of_data
        ///    hash = hash* FNV_prime
        /// return hash</pre>
        ///
        /// <h3>Links</h3>
        /// <a href = "http://www.isthe.com/chongo/tech/comp/fnv/">http://www.isthe.com/chongo/tech/comp/fnv/</a><br/>
        /// <a href = "http://en.wikipedia.org/wiki/Fowler%E2%80%93Noll%E2%80%93Vo_hash_function">http://en.wikipedia.org/wiki/Fowler%E2%80%93Noll%E2%80%93Vo_hash_function</a><br/>
        /// </summary>
        /// <param name="c">long key to be hashed</param>
        /// <returns>64 bit hash</returns>
        public static long FVN64hash(long c)
        {
            unchecked
            {
                var hash = FNV_BASIS;
                hash ^= c >> 56;
                hash *= FNV_PRIME_64;
                hash ^= 0xFFL & (c >> 48);
                hash *= FNV_PRIME_64;
                hash ^= 0xFFL & (c >> 40);
                hash *= FNV_PRIME_64;
                hash ^= 0xFFL & (c >> 32);
                hash *= FNV_PRIME_64;
                hash ^= 0xFFL & (c >> 24);
                hash *= FNV_PRIME_64;
                hash ^= 0xFFL & (c >> 16);
                hash *= FNV_PRIME_64;
                hash ^= 0xFFL & (c >> 8);
                hash *= FNV_PRIME_64;
                hash ^= 0xFFL & c;
                hash *= FNV_PRIME_64;
                return hash;
            }
        }

        /// <summary>
        /// Fowler-Noll-Vo 32 bit hash(FNV-1a) for long key.This is big-endian version (native endianess of JVM).<br/>
        /// <p/>
        /// <h3>Algorithm</h3>
        /// <p/>
        /// <pre>
        /// hash = offset_basis
        /// for each octet_of_data to be hashed
        /// hash = hash xor octet_of_data
        ///    hash = hash* FNV_prime
        /// return hash</pre>
        /// <h3>Links</h3>
        /// <a href = "http://www.isthe.com/chongo/tech/comp/fnv/">http://www.isthe.com/chongo/tech/comp/fnv/</a><br/>
        /// <a href = "http://en.wikipedia.org/wiki/Fowler%E2%80%93Noll%E2%80%93Vo_hash_function">http://en.wikipedia.org/wiki/Fowler%E2%80%93Noll%E2%80%93Vo_hash_function</a><br/>
        /// </summary>
        /// <param name="c">long key to be hashed</param>
        /// <returns>32 bit hash</returns>
        public static int FVN64to32hash(long c)
        {
            unchecked
            {
                var hash = FNV_BASIS;
                hash ^= c >> 56;
                hash *= FNV_PRIME_32;
                hash ^= 0xFFL & (c >> 48);
                hash *= FNV_PRIME_32;
                hash ^= 0xFFL & (c >> 40);
                hash *= FNV_PRIME_32;
                hash ^= 0xFFL & (c >> 32);
                hash *= FNV_PRIME_32;
                hash ^= 0xFFL & (c >> 24);
                hash *= FNV_PRIME_32;
                hash ^= 0xFFL & (c >> 16);
                hash *= FNV_PRIME_32;
                hash ^= 0xFFL & (c >> 8);
                hash *= FNV_PRIME_32;
                hash ^= 0xFFL & c;
                hash *= FNV_PRIME_32;
                return (int)hash;
            }
        }

        /// <summary>
        /// MurmurHash hash function for bytes array.
        /// <p/>
        /// <h3>Links</h3>
        /// <a href = "http://sites.google.com/site/murmurhash/">http://sites.google.com/site/murmurhash/</a><br/>
        /// <a href = "http://dmy999.com/article/50/murmurhash-2-java-port">http://dmy999.com/article/50/murmurhash-2-java-port</a><br/>
        /// <a href = "http://en.wikipedia.org/wiki/MurmurHash">http://en.wikipedia.org/wiki/MurmurHash</a><br/>
        /// </summary>
        /// <param name="data">bytes to be hashed</param>
        /// <param name="seed">seed parameter</param>
        /// <returns>32 bit hash</returns>
        public static int MurmurHash2(byte[] data, int seed)
        {
            unchecked
            {
                // 'm' and 'r' are mixing constants generated offline.
                // They're not really 'magic', they just happen to work well.
                const int m = 0x5bd1e995;
                const int r = 24;

                // Initialize the hash to a 'random' value
                var len = data.Length;
                var h = seed ^ len;

                var i = 0;
                while (len >= 4)
                {
                    var k = data[i] & 0xFF;
                    k |= (data[i + 1] & 0xFF) << 8;
                    k |= (data[i + 2] & 0xFF) << 16;
                    k |= (data[i + 3] & 0xFF) << 24;

                    k *= m;
                    k ^= k >> r;
                    k *= m;

                    h *= m;
                    h ^= k;

                    i += 4;
                    len -= 4;
                }

                switch (len)
                {
                    case 3:
                        h ^= (data[i + 2] & 0xFF) << 16;
                        h ^= (data[i + 1] & 0xFF) << 8;
                        h ^= (data[i] & 0xFF);
                        h *= m;
                        break;
                    case 2:
                        h ^= (data[i + 1] & 0xFF) << 8;
                        h ^= (data[i] & 0xFF);
                        h *= m;
                        break;
                    case 1:
                        h ^= (data[i] & 0xFF);
                        h *= m;
                        break;
                }

                h ^= h >> 13;
                h *= m;
                h ^= h >> 15;

                return h;
            }
        }

        /// <summary>
        /// MurmurHash hash function integer.
        /// <p/>
        /// <h3>Links</h3>
        /// <a href = "http://sites.google.com/site/murmurhash/" > http://sites.google.com/site/murmurhash/</a><br/>
        /// <a href = "http://dmy999.com/article/50/murmurhash-2-java-port" > http://dmy999.com/article/50/murmurhash-2-java-port</a><br/>
        /// <a href = "http://en.wikipedia.org/wiki/MurmurHash" > http://en.wikipedia.org/wiki/MurmurHash</a><br/>
        /// </summary>
        /// <param name="c">int to be hashed</param>
        /// <param name="seed">seed parameter</param>
        /// <returns>32 bit hash</returns>
        public static int MurmurHash2(int c, int seed)
        {
            unchecked
            {
                // 'm' and 'r' are mixing constants generated offline.
                // They're not really 'magic', they just happen to work well.
                const int m = 0x5bd1e995;
                // Initialize the hash to a 'random' value
                var h = seed ^ 4;
                c *= m;
                c ^= c >> 24;
                c *= m;
                h *= m;
                h ^= c;
                h ^= h >> 13;
                h *= m;
                h ^= h >> 15;
                return h;
            }
        }

        /// <summary>
        /// MurmurHash hash function for bytes array with default seed value equals 0x2f1a32b3.
        /// <p/>
        /// <h3>Links</h3>
        /// <a href = "http://sites.google.com/site/murmurhash/" > http://sites.google.com/site/murmurhash/</a><br/>
        /// <a href = "http://dmy999.com/article/50/murmurhash-2-java-port" > http://dmy999.com/article/50/murmurhash-2-java-port</a><br/>
        /// <a href = "http://en.wikipedia.org/wiki/MurmurHash" > http://en.wikipedia.org/wiki/MurmurHash</a><br/>
        /// </summary>
        /// <param name="data">bytes to be hashed</param>
        /// <returns>32 bit hash</returns>
        public static int MurmurHash2(byte[] data)
        {
            return MurmurHash2(data, 0x2f1a32b3);
        }

        /// <summary> 
        /// MurmurHash hash function integer with default seed value equals to 0x2f1a32b3.
        /// <p/>
        /// <h3>Links</h3>
        /// <a href = "http://sites.google.com/site/murmurhash/" > http://sites.google.com/site/murmurhash/</a><br/>
        /// <a href = "http://dmy999.com/article/50/murmurhash-2-java-port" > http://dmy999.com/article/50/murmurhash-2-java-port</a><br/>
        /// <a href = "http://en.wikipedia.org/wiki/MurmurHash" > http://en.wikipedia.org/wiki/MurmurHash</a><br/>
        /// </summary>
        /// <param name="c">int to be hashed</param>
        /// <returns>32 bit hash</returns>
        public static int MurmurHash2(int c)
        {
            return MurmurHash2(c, 0x2f1a32b3);
        }
    }
}
