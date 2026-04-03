using NRedberry.Numbers;
using NRedberry.Tensors;
using TensorFactory = NRedberry.Tensors.Tensors;

namespace NRedberry.Core.Tests.Tensors;

public sealed class SplitNumbersTests
{
    [Fact]
    public void ShouldBeUsedForScalarOnlySplitScalars()
    {
        Split split = Split.SplitScalars(TensorFactory.Parse("2*a"));

        split.GetType().Name.ShouldBe("SplitNumbers");
        split.Factor.ToString(OutputFormat.Redberry).ShouldBe("2*a");
        split.Summand.ToString(OutputFormat.Redberry).ShouldBe("1");
        split.GetBuilder().GetType().Name.ShouldBe("ComplexSumBuilder");
    }

    [Fact]
    public void ShouldBeUsedForIndexlessSplitIndexless()
    {
        Split split = Split.SplitIndexless(TensorFactory.Parse("2*a"));

        split.GetType().Name.ShouldBe("SplitNumbers");
        split.Factor.ToString(OutputFormat.Redberry).ShouldBe("a");
        split.Summand.ShouldBe(new Complex(2));
        split.GetBuilder().GetType().Name.ShouldBe("ComplexSumBuilder");
    }
}
