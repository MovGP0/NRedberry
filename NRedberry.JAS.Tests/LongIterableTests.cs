using System.Linq;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Util;
using Xunit;

namespace NRedberry.JAS.Tests;

public sealed class LongIterableTests
{
    [Fact]
    public void ShouldEnumerateNonNegativeValuesUpToUpperBound()
    {
        LongIterable iterable = new();
        iterable.SetUpperBound(3);

        iterable.ToList().ShouldBe([0L, 1L, 2L, 3L]);
    }

    [Fact]
    public void ShouldEnumerateAlternatingSignedValues()
    {
        LongIterable iterable = new();
        iterable.SetAllIterator();
        iterable.SetUpperBound(3);

        iterable.ToList().ShouldBe([0L, 1L, -1L, 2L, -2L, 3L, -3L]);
    }
}
