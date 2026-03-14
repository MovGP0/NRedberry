using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Arith;
using Xunit;

namespace NRedberry.JAS.Tests;

public sealed class ModularContractsTests
{
    [Fact]
    public void ShouldExposeModularSymmetricIntegers()
    {
        Modular modInteger = new ModInteger(new ModIntegerRing(new BigInteger(7)), 5);
        Modular modLong = new ModLong(new ModLongRing(7), 5);

        modInteger.GetSymmetricInteger().ToString().ShouldBe("-2");
        modLong.GetSymmetricInteger().ToString().ShouldBe("-2");
    }

    [Fact]
    public void ShouldExposeModularRingFactoryContract()
    {
        ModularRingFactory<ModInteger> integerRing = new ModIntegerRing(new BigInteger(7));
        ModularRingFactory<ModLong> longRing = new ModLongRing(7);

        integerRing.GetIntegerModul().ToString().ShouldBe("7");
        longRing.GetIntegerModul().ToString().ShouldBe("7");
    }
}
