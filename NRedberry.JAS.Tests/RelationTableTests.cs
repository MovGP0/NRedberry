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

        Assert.Null(relation.Left);
        Assert.Null(relation.Right);
        Assert.Equal([1L, 1L], relation.Product.LeadingExpVector()!.GetVal());
        Assert.Contains("null | null", relation.ToString(), System.StringComparison.Ordinal);
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

        Assert.Equal(1, firstRing.Table.Size());
        Assert.Equal(relationProduct.ToString(["x", "y"]), relation.Product.ToString(["x", "y"]));
        Assert.Equal(firstRing.Table, secondRing.Table);
        Assert.Equal(firstRing.Table.GetHashCode(), secondRing.Table.GetHashCode());
        Assert.Contains("RelationTable", firstRing.Table.ToString(["x", "y"]), System.StringComparison.Ordinal);
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

        Assert.Equal(1, extended.Table.Size());
        Assert.Equal(1, contracted.Table.Size());
        Assert.Equal(1, reversed.Table.Size());
    }

    private static GenSolvablePolynomialRing<BigRational> CreateRing()
    {
        return new GenSolvablePolynomialRing<BigRational>(new BigRational(), 2, new TermOrder(TermOrder.INVLEX), ["x", "y"]);
    }
}
