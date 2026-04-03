using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Arith;
using Xunit;

namespace NRedberry.JAS.Tests;

public sealed class CombinatoricTests
{
    [Theory]
    [InlineData(0, "1")]
    [InlineData(1, "1")]
    [InlineData(5, "120")]
    public void ShouldComputeFactorial(long value, string expected)
    {
        BigInteger factorial = Combinatoric.Factorial(value);

        factorial.ToString().ShouldBe(expected);
    }
}
