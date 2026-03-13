using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;
using Xunit;

namespace NRedberry.JAS.Tests;

public sealed class TermOrderTests
{
    [Fact]
    public void ShouldDescribeReverseAndCloneWeightedOrders()
    {
        TermOrder lex = new(TermOrder.LEX);
        TermOrder reversed = lex.Reverse();
        TermOrder weighted = new([1L, 2L, 3L]);
        long[][] weight = weighted.GetWeight()!;

        weight[0][0] = 99;

        Assert.Equal("LEX", lex.ToString());
        Assert.Equal(TermOrder.REVLEX, reversed.GetEvord());
        Assert.Equal(TermOrder.REVTDEG, TermOrder.Revert(TermOrder.GRLEX));
        Assert.Contains("W(", weighted.ToString(), System.StringComparison.Ordinal);
        Assert.Equal(1L, weighted.GetWeight()![0][0]);
    }

    [Fact]
    public void ShouldSupportSplitOrdersExtensionContractionAndComparators()
    {
        TermOrder split = new(TermOrder.LEX, TermOrder.GRLEX, 4, 2);
        TermOrder extended = split.Extend(4, 1);
        TermOrder contracted = split.Contract(0, 2);
        ExpVector left = ExpVector.Create([1L, 0L, 0L, 0L]);
        ExpVector right = ExpVector.Create([0L, 1L, 0L, 0L]);

        int descend = split.GetDescendComparator().Compare(left, right);
        int ascend = split.GetAscendComparator().Compare(left, right);

        Assert.Equal(TermOrder.GRLEX, split.GetEvord2());
        Assert.Equal(2, split.GetSplit());
        Assert.Equal(3, extended.GetSplit());
        Assert.Equal(TermOrder.LEX, contracted.GetEvord());
        Assert.NotEqual(0, descend);
        Assert.Equal(-descend, ascend);
    }

    [Fact]
    public void ShouldCompareEqualOrdersByValue()
    {
        TermOrder first = new([[1L, 0L], [0L, 1L]]);
        TermOrder second = new([[1L, 0L], [0L, 1L]]);
        TermOrder third = new(TermOrder.INVLEX);

        Assert.Equal(first, second);
        Assert.Equal(first.GetHashCode(), second.GetHashCode());
        Assert.NotEqual(first, third);
    }
}
