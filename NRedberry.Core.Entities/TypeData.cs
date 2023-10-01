using System.Collections;

namespace NRedberry;

public sealed class TypeData
{
    public int From { get; }
    public int Length { get; }
    public BitArray States { get; }

    public TypeData(int from, int length, BitArray states)
    {
        From = from;
        Length = length;
        States = (BitArray)states.Clone();
    }
}