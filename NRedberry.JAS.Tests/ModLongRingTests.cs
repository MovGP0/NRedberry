using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Arith;
using Xunit;

namespace NRedberry.JAS.Tests;

public sealed class ModLongRingTests
{
    [Fact]
    public void ShouldRecognizePrimeAndCompositeModuli()
    {
        ModLongRing primeRing = new(7);
        ModLongRing compositeRing = new(8);

        primeRing.IsField().ShouldBeTrue();
        compositeRing.IsField().ShouldBeFalse();
    }

    [Fact]
    public void ShouldEnumerateElementsInRange()
    {
        ModLongRing ring = new(5);

        ring.Select(x => x.ToString()).ToArray().ShouldBe(["0", "1", "2", "3", "4"]);
    }
}
