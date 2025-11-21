using System.Collections;

namespace NRedberry;

public sealed class TypeData(int from, int length, BitArray states)
{
    public int From { get; } = from;
    public int Length { get; } = length;
    public BitArray States { get; } = (BitArray)states.Clone();
}
