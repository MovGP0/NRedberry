namespace NRedberry.Core.Contexts
{
    public static class LongExtensions
    {
        public static int BitCount(this long i)
        {
            i = i - ((i >> 1) & 0x5555555555555555);
            i = (i & 0x3333333333333333) + ((i >> 2) & 0x3333333333333333);
            return (int)(unchecked(((i + (i >> 4)) & 0xF0F0F0F0F0F0F0F) * 0x101010101010101) >> 56);
        }

        public static int NumberOfTrailingZeros(this long n)
        {
            const int numberOfBits = 64;
            var mask = 1;
            for (var i = 0; i < numberOfBits; i++, mask <<= 1)
            {
                if ((n & mask) != 0)
                    return i;
            }

            return numberOfBits;
        }
    }
}