using System.Linq;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Arith;
using Xunit;

namespace NRedberry.JAS.Tests;

public sealed class ModIntegerRingTests
{
    [Fact]
    public void ShouldRecognizePrimeAndCompositeModuli()
    {
        ModIntegerRing primeRing = new(new BigInteger(7));
        ModIntegerRing compositeRing = new(new BigInteger(8));

        primeRing.IsField().ShouldBeTrue();
        compositeRing.IsField().ShouldBeFalse();
    }

    [Fact]
    public void ShouldEnumerateElementsInRange()
    {
        ModIntegerRing ring = new(new BigInteger(5));

        ring.Select(x => x.ToString()).ToArray().ShouldBe(["0", "1", "2", "3", "4"]);
    }
}
