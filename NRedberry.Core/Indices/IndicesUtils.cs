using System;

namespace NRedberry.Core.Indices
{
    public sealed class IndicesUtils
    {
        public static byte getType(uint index)
        {
            throw new NotImplementedException();
        }

        public static uint getRawStateInt(uint index)
        {
            return index & 0x80000000;
        }

        public static uint getStateInt(uint index)
        {
            return (index & 0x80000000) >> 31;
        }

        public static bool getState(uint index)
        {
            return (index & 0x80000000) == 0x80000000;
        }

        public static uint inverseIndexState(uint index)
        {
            return 0x80000000 ^ index;
        }

        public static bool haveEqualStates(uint tIndex, uint index)
        {
            throw new NotImplementedException();
        }
    }
}
