using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;
using Xunit;

namespace NRedberry.JAS.Tests;

public sealed class ExpVectorTests
{
    [Fact]
    public void ShouldCreateReverseAndCompareVectors()
    {
        ExpVector vector = ExpVector.Create([1L, 2L, 3L]);

        vector.GetVal().ShouldBe([1L, 2L, 3L]);
        vector.Reverse().GetVal().ShouldBe([3L, 2L, 1L]);
        vector.Degree().ShouldBe(6L);
        (ExpVector.EvIlcp(vector, ExpVector.Create([1L, 2L, 2L])) > 0).ShouldBeTrue();
    }
}
