using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;
using Shouldly;
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

        pair.ShouldBe(same);
        pair.IsMultiple(smaller).ShouldBeTrue();
        pair.ToString().ShouldContain("ExpVectorPair");
    }
}
