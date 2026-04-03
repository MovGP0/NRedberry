using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Arith;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;
using Xunit;
using SystemBigInteger = System.Numerics.BigInteger;

namespace NRedberry.JAS.Tests;

public sealed class StructureContractsTests
{
    [Fact]
    public void ShouldSatisfyElementAndFactoryContractsWithBigRational()
    {
        Element<BigRational> element = new BigRational(3, 2);
        ElemFactory<BigRational> elementFactory = new BigRational();
        RingFactory<BigRational> ringFactory = new BigRational();
        MonoidFactory<BigRational> monoidFactory = ringFactory;
        BigRational clone = element.Clone();

        element.CompareTo(clone).ShouldBe(0);
        clone.ShouldBe(element);
        element.Factory().ShouldBeOfType<BigRational>();
        elementFactory.IsFinite().ShouldBeFalse();
        elementFactory.FromInteger(5).ToString().ShouldBe("5");
        elementFactory.FromInteger(new SystemBigInteger(7)).ToString().ShouldBe("7");
        elementFactory.Generators().ShouldNotBeEmpty();
        BigRational.Clone((BigRational)element).ToString().ShouldBe("3/2");
        monoidFactory.IsCommutative().ShouldBeTrue();
        monoidFactory.IsAssociative().ShouldBeTrue();
        ringFactory.IsField().ShouldBeTrue();
        ringFactory.Characteristic().ShouldBe(SystemBigInteger.Zero);
    }

    [Fact]
    public void ShouldSatisfyAdditiveMultiplicativeAndGcdContracts()
    {
        AbelianGroupElem<BigRational> additive = new BigRational(-3, 2);
        MonoidElem<BigRational> multiplicative = new BigRational(6);
        RingElem<BigRational> left = new BigRational(6);
        GcdRingElem<BigRational> right = new BigRational(4);
        BigRational[] egcd = left.Egcd((BigRational)right);

        additive.IsZero().ShouldBeFalse();
        additive.Signum().ShouldBe(-1);
        additive.Sum(new BigRational(2)).ToString().ShouldBe("1/2");
        additive.Subtract(new BigRational(1)).ToString().ShouldBe("-5/2");
        additive.Negate().ToString().ShouldBe("3/2");
        additive.Abs().ToString().ShouldBe("3/2");
        multiplicative.IsOne().ShouldBeFalse();
        multiplicative.IsUnit().ShouldBeTrue();
        multiplicative.Multiply(new BigRational(3, 2)).ToString().ShouldBe("9");
        multiplicative.Divide(new BigRational(3, 2)).ToString().ShouldBe("4");
        multiplicative.Remainder(new BigRational(5)).ToString().ShouldBe("0");
        multiplicative.Inverse().ToString().ShouldBe("1/6");
        left.Gcd((BigRational)right).ShouldBe(BigRational.One);
        egcd[0].ShouldBe(BigRational.One);
        egcd[1].Multiply((BigRational)left).Sum(egcd[2].Multiply((BigRational)right)).ShouldBe(BigRational.One);
    }
}
