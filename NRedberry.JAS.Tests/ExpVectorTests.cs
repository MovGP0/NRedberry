using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;
using Xunit;

namespace NRedberry.JAS.Tests;

public sealed class ExpVectorTests
{
    [Fact]
    public void ShouldCreateReverseAndCompareVectors()
    {
        ExpVector vector = ExpVector.Create([1L, 2L, 3L]);

        Assert.Equal([1L, 2L, 3L], vector.GetVal());
        Assert.Equal([3L, 2L, 1L], vector.Reverse().GetVal());
        Assert.Equal(6L, vector.Degree());
        Assert.True(ExpVector.EvIlcp(vector, ExpVector.Create([1L, 2L, 2L])) > 0);
    }
}
