using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;
using Xunit;

namespace NRedberry.JAS.Tests;

public sealed class ExpVectorIntegerTests
{
    [Fact]
    public void ShouldExtendContractAndAbsValues()
    {
        ExpVectorInteger vector = new([1L, -2L, 3L]);

        Assert.Equal([4L, 1L, -2L, 3L], vector.Extend(1, 0, 4).GetVal());
        Assert.Equal([-2L, 3L], vector.Contract(1, 2).GetVal());
        Assert.Equal([1L, 2L, 3L], vector.Abs().GetVal());
    }
}
