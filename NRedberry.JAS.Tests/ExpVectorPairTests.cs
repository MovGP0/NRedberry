using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;
using Xunit;

namespace NRedberry.JAS.Tests;

public sealed class ExpVectorPairTests
{
    [Fact]
    public void ShouldCompareAndCheckMultiples()
    {
        ExpVectorPair pair = new(new ExpVectorLong([2L, 3L]), new ExpVectorLong([4L, 5L]));
        ExpVectorPair same = new(new ExpVectorLong([2L, 3L]), new ExpVectorLong([4L, 5L]));
        ExpVectorPair smaller = new(new ExpVectorLong([1L, 3L]), new ExpVectorLong([2L, 5L]));

        Assert.Equal(pair, same);
        Assert.True(pair.IsMultiple(smaller));
        Assert.Contains("ExpVectorPair", pair.ToString(), System.StringComparison.Ordinal);
    }
}
