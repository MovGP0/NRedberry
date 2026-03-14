using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Arith;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;
using Xunit;

namespace NRedberry.JAS.Tests;

public sealed class RelationTableTests
{
    [Fact]
    public void ShouldReturnDefaultLookupForCommutativePairs()
    {
        GenSolvablePolynomialRing<BigRational> ring = CreateRing();
        ExpVector x = ring.Univariate(0).LeadingExpVector()!;
        ExpVector y = ring.Univariate(1).LeadingExpVector()!;

        TableRelation<BigRational> relation = ring.Table.Lookup(x, y);

        relation.Left.ShouldBeNull();
        relation.Right.ShouldBeNull();
        relation.Product.LeadingExpVector()!.GetVal().ShouldBe([1L, 1L]);
        relation.ToString().ShouldContain("null | null");
    }

    [Fact]
    public void ShouldStoreRelationsAndCompareTablesByValue()
    {
        GenSolvablePolynomialRing<BigRational> firstRing = CreateRing();
        GenSolvablePolynomialRing<BigRational> secondRing = CreateRing();
        ExpVector x = firstRing.Univariate(0).LeadingExpVector()!;
        ExpVector y = firstRing.Univariate(1).LeadingExpVector()!;
        GenSolvablePolynomial<BigRational> relationProduct = new(firstRing, new BigRational(1), ExpVector.Create([1L, 1L]));
        relationProduct = relationProduct.Sum(firstRing.FromInteger(1));

        firstRing.Table.Update(x, y, relationProduct);
        GenSolvablePolynomial<BigRational> secondRelationProduct = new(secondRing, new BigRational(1), ExpVector.Create([1L, 1L]));
        secondRelationProduct = secondRelationProduct.Sum(secondRing.FromInteger(1));
        secondRing.Table.Update(
            secondRing.Univariate(0).LeadingExpVector()!,
            secondRing.Univariate(1).LeadingExpVector()!,
            secondRelationProduct);

        TableRelation<BigRational> relation = firstRing.Table.Lookup(x, y);

        firstRing.Table.Size().ShouldBe(1);
        relation.Product.ToString(["x", "y"]).ShouldBe(relationProduct.ToString(["x", "y"]));
        secondRing.Table.ShouldBe(firstRing.Table);
        secondRing.Table.GetHashCode().ShouldBe(firstRing.Table.GetHashCode());
        firstRing.Table.ToString(["x", "y"]).ShouldContain("RelationTable");
    }

    [Fact]
    public void ShouldPreserveRelationsAcrossRingTransforms()
    {
        GenSolvablePolynomialRing<BigRational> ring = CreateRing();
        ExpVector x = ring.Univariate(0).LeadingExpVector()!;
        ExpVector y = ring.Univariate(1).LeadingExpVector()!;
        GenSolvablePolynomial<BigRational> relationProduct = new(ring, new BigRational(1), ExpVector.Create([1L, 1L]));
        relationProduct = relationProduct.Sum(ring.FromInteger(1));

        ring.Table.Update(x, y, relationProduct);

        GenSolvablePolynomialRing<BigRational> extended = ring.Extend(1);
        GenSolvablePolynomialRing<BigRational> contracted = extended.Contract(1);
        GenSolvablePolynomialRing<BigRational> reversed = ring.Reverse();

        extended.Table.Size().ShouldBe(1);
        contracted.Table.Size().ShouldBe(1);
        reversed.Table.Size().ShouldBe(1);
    }

    private static GenSolvablePolynomialRing<BigRational> CreateRing()
    {
        return new GenSolvablePolynomialRing<BigRational>(new BigRational(), 2, new TermOrder(TermOrder.INVLEX), ["x", "y"]);
    }
}
