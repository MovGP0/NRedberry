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

        lex.ToString().ShouldBe("LEX");
        reversed.GetEvord().ShouldBe(TermOrder.REVLEX);
        TermOrder.Revert(TermOrder.GRLEX).ShouldBe(TermOrder.REVTDEG);
        weighted.ToString().ShouldContain("W(");
        weighted.GetWeight()![0][0].ShouldBe(1L);
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

        split.GetEvord2().ShouldBe(TermOrder.GRLEX);
        split.GetSplit().ShouldBe(2);
        extended.GetSplit().ShouldBe(3);
        contracted.GetEvord().ShouldBe(TermOrder.LEX);
        descend.ShouldNotBe(0);
        ascend.ShouldBe(-descend);
    }

    [Fact]
    public void ShouldCompareEqualOrdersByValue()
    {
        TermOrder first = new([[1L, 0L], [0L, 1L]]);
        TermOrder second = new([[1L, 0L], [0L, 1L]]);
        TermOrder third = new(TermOrder.INVLEX);

        second.ShouldBe(first);
        second.GetHashCode().ShouldBe(first.GetHashCode());
        third.ShouldNotBe(first);
    }
}
