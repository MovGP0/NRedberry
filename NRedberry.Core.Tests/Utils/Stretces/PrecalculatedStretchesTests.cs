using NRedberry.Core.Utils.Stretces;

namespace NRedberry.Core.Tests.Utils.Stretces;

public sealed class PrecalculatedStretchesTests
{
    [Fact]
    public void ShouldExposeCopiedRawValues()
    {
        PrecalculatedStretches stretches = new(1, 1, 2, 3);

        int[] values = stretches.RawValues;
        values[0] = 99;

        stretches.RawValues.ShouldBe([1, 1, 2, 3]);
    }

    [Fact]
    public void ShouldCreateRawValuesFromProviderAndEnumerateStretches()
    {
        PrecalculatedStretches stretches = new(["aa", "bbb", "c"], new StringLengthProvider());

        stretches.RawValues.ShouldBe([2, 3, 1]);
        stretches.ToArray().ShouldBe([new Stretch(0, 1), new Stretch(1, 1), new Stretch(2, 1)]);
    }
}

internal sealed class StringLengthProvider : IIntObjectProvider
{
    public int Get(object element)
    {
        return ((string)element).Length;
    }
}
