using System.Linq;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Arith;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Ufd;
using Xunit;

namespace NRedberry.JAS.Tests;

public sealed class SquarefreeInfiniteFieldCharPTests
{
    [Fact]
    public void ShouldFactorNumeratorAndDenominatorInCharacteristicP()
    {
        QuotientRing<ModLong> quotientRing = CreateInfiniteQuotientRing();
        SquarefreeInfiniteFieldCharP<ModLong> squarefree = new(quotientRing);
        GenPolynomialRing<ModLong> baseRing = quotientRing.Ring;
        GenPolynomial<ModLong> t = baseRing.Univariate(0);
        Quotient<ModLong> coefficient = new(
            quotientRing,
            t.Sum(baseRing.FromInteger(1)).Multiply(t.Sum(baseRing.FromInteger(1))),
            t);

        SortedDictionary<Quotient<ModLong>, long> factors = squarefree.SquarefreeFactors(coefficient);

        factors.Count.ShouldBe(2);
        factors.Single(entry => entry.Key.Equals(new Quotient<ModLong>(quotientRing, t.Sum(baseRing.FromInteger(1))))).Value.ShouldBe(2L);
        factors.Single(entry => entry.Key.Equals(new Quotient<ModLong>(quotientRing, baseRing.FromInteger(1), t))).Value.ShouldBe(1L);
    }

    [Fact]
    public void ShouldReturnIdentityFactorForOne()
    {
        QuotientRing<ModLong> quotientRing = CreateInfiniteQuotientRing();
        SquarefreeInfiniteFieldCharP<ModLong> squarefree = new(quotientRing);

        SortedDictionary<Quotient<ModLong>, long> factors = squarefree.SquarefreeFactors(quotientRing.One);

        factors.ShouldHaveSingleItem();
        factors[quotientRing.One].ShouldBe(1L);
    }

    private static QuotientRing<ModLong> CreateInfiniteQuotientRing()
    {
        return new QuotientRing<ModLong>(
            new GenPolynomialRing<ModLong>(
                new ModLongRing(2, true),
                1,
                new TermOrder(TermOrder.INVLEX),
                ["t"]),
            true);
    }
}
