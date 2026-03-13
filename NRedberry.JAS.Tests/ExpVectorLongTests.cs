using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;
using Xunit;

namespace NRedberry.JAS.Tests;

public sealed class ExpVectorLongTests
{
    [Fact]
    public void ShouldCombineAndCheckMultiples()
    {
        ExpVectorLong left = new([1L, 2L]);
        ExpVectorLong right = new([3L, 4L]);

        Assert.Equal([1L, 2L, 3L, 4L], left.Combine(right).GetVal());
        Assert.True(new ExpVectorLong([3L, 4L]).MultipleOf(new ExpVectorLong([1L, 4L])));
        Assert.Equal([4L, 6L], left.Sum(new ExpVectorLong([3L, 4L])).GetVal());
    }
}
