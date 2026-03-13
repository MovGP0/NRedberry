using System.Numerics;
using NRedberry.Solver.Frobenius;
using Xunit;

namespace NRedberry.Core.Tests.Solver.Frobenius;

public sealed class FrobeniusNumberTest
{
    [Fact]
    public void ShouldComputeFrobeniusNumberForSamples()
    {
        Assert.Equal(new BigInteger(731), FrobeniusNumber.Calculate([112, 432, 123, 7]));
        Assert.Equal(new BigInteger(11), FrobeniusNumber.Calculate([112, 432, 123, 7, 3]));
        Assert.Equal(new BigInteger(147), FrobeniusNumber.Calculate([112, 432, 122, 8, 31]));
    }

    [Fact]
    public void ShouldReturnNegativeOneWhenNoFiniteResult()
    {
        Assert.Equal(new BigInteger(-1), FrobeniusNumber.Calculate([112, 432, 122, 8, 32]));
    }

    [Fact]
    public void ShouldMatchMathematicaSamplesSecondBatch()
    {
        Assert.Equal(new BigInteger(71845), FrobeniusNumber.Calculate([5956, 388, 8234, 6312, 5379]));
        Assert.Equal(new BigInteger(21530), FrobeniusNumber.Calculate([5967, 612, 1169, 7841, 196]));
        Assert.Equal(new BigInteger(6021), FrobeniusNumber.Calculate([46, 4967, 50, 9208, 6921]));
    }
}
