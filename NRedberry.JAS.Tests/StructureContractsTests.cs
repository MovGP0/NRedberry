using System;
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

        Assert.Equal(0, element.CompareTo(clone));
        Assert.Equal(element, clone);
        Assert.IsType<BigRational>(element.Factory());
        Assert.False(elementFactory.IsFinite());
        Assert.Equal("5", elementFactory.FromInteger(5).ToString());
        Assert.Equal("7", elementFactory.FromInteger(new SystemBigInteger(7)).ToString());
        Assert.NotEmpty(elementFactory.Generators());
        Assert.Equal("3/2", BigRational.Clone((BigRational)element).ToString());
        Assert.True(monoidFactory.IsCommutative());
        Assert.True(monoidFactory.IsAssociative());
        Assert.True(ringFactory.IsField());
        Assert.Equal(SystemBigInteger.Zero, ringFactory.Characteristic());
    }

    [Fact]
    public void ShouldSatisfyAdditiveMultiplicativeAndGcdContracts()
    {
        AbelianGroupElem<BigRational> additive = new BigRational(-3, 2);
        MonoidElem<BigRational> multiplicative = new BigRational(6);
        RingElem<BigRational> left = new BigRational(6);
        GcdRingElem<BigRational> right = new BigRational(4);
        BigRational[] egcd = left.Egcd((BigRational)right);

        Assert.False(additive.IsZero());
        Assert.Equal(-1, additive.Signum());
        Assert.Equal("1/2", additive.Sum(new BigRational(2)).ToString());
        Assert.Equal("-5/2", additive.Subtract(new BigRational(1)).ToString());
        Assert.Equal("3/2", additive.Negate().ToString());
        Assert.Equal("3/2", additive.Abs().ToString());
        Assert.False(multiplicative.IsOne());
        Assert.True(multiplicative.IsUnit());
        Assert.Equal("9", multiplicative.Multiply(new BigRational(3, 2)).ToString());
        Assert.Equal("4", multiplicative.Divide(new BigRational(3, 2)).ToString());
        Assert.Equal("0", multiplicative.Remainder(new BigRational(5)).ToString());
        Assert.Equal("1/6", multiplicative.Inverse().ToString());
        Assert.Equal(BigRational.One, left.Gcd((BigRational)right));
        Assert.Equal(BigRational.One, egcd[0]);
        Assert.Equal(
            BigRational.One,
            egcd[1].Multiply((BigRational)left).Sum(egcd[2].Multiply((BigRational)right)));
    }
}
