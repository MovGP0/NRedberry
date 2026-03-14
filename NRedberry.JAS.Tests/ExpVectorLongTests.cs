using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;
using Shouldly;
using Xunit;

namespace NRedberry.JAS.Tests;

public sealed class ExpVectorLongTests
{
    [Fact]
    public void ShouldCombineAndCheckMultiples()
    {
        ExpVectorLong left = new([1L, 2L]);
        ExpVectorLong right = new([3L, 4L]);

        left.Combine(right).GetVal().ShouldBe([1L, 2L, 3L, 4L]);
        new ExpVectorLong([3L, 4L]).MultipleOf(new ExpVectorLong([1L, 4L])).ShouldBeTrue();
        left.Sum(new ExpVectorLong([3L, 4L])).GetVal().ShouldBe([4L, 6L]);
    }
}
