using System.Numerics;
using NRedberry.Solver.Frobenius;
using Xunit;

namespace NRedberry.Core.Tests.Solver.Frobenius;

public sealed class FrobeniusNumberTest
{
    [Fact]
    public void ShouldComputeFrobeniusNumberForSamples()
    {
        FrobeniusNumber.Calculate([112, 432, 123, 7]).ShouldBe(new BigInteger(731));
        FrobeniusNumber.Calculate([112, 432, 123, 7, 3]).ShouldBe(new BigInteger(11));
        FrobeniusNumber.Calculate([112, 432, 122, 8, 31]).ShouldBe(new BigInteger(147));
    }

    [Fact]
    public void ShouldReturnNegativeOneWhenNoFiniteResult()
    {
        FrobeniusNumber.Calculate([112, 432, 122, 8, 32]).ShouldBe(new BigInteger(-1));
    }

    [Fact]
    public void ShouldMatchMathematicaSamplesSecondBatch()
    {
        FrobeniusNumber.Calculate([5956, 388, 8234, 6312, 5379]).ShouldBe(new BigInteger(71845));
        FrobeniusNumber.Calculate([5967, 612, 1169, 7841, 196]).ShouldBe(new BigInteger(21530));
        FrobeniusNumber.Calculate([46, 4967, 50, 9208, 6921]).ShouldBe(new BigInteger(6021));
    }
}
