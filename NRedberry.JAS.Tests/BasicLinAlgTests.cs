using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Arith;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Vector;
using Shouldly;
using Xunit;

namespace NRedberry.JAS.Tests;

public sealed class BasicLinAlgTests
{
    [Fact]
    public void ShouldAddVectorsElementWiseUntilShortestInputEnds()
    {
        BasicLinAlg<BigRational> linAlg = new();
        List<BigRational> left = [new(1), new(2), new(3)];
        List<BigRational> right = [new(10), new(20)];

        List<BigRational>? sum = linAlg.VectorAdd(left, right);

        sum.ShouldBe([new BigRational(11), new BigRational(22)]);
    }

    [Fact]
    public void ShouldTreatNullEntriesAsZeroChecksOnlyForNonNullValues()
    {
        BasicLinAlg<BigRational> linAlg = new();

        linAlg.IsZero([new BigRational(0), null]).ShouldBeTrue();
        linAlg.IsZero([new BigRational(0), new BigRational(1)]).ShouldBeFalse();
    }
}
