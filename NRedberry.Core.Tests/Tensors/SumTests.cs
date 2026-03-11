using NRedberry.Numbers;
using NRedberry.Tensors;
using TensorFactory = NRedberry.Tensors.Tensors;
using Xunit;

namespace NRedberry.Core.Tests.Tensors;

public sealed class SumTests
{
    [Fact]
    public void ShouldExposeChildrenSizeBuilderAndFactory()
    {
        Sum sum = Assert.IsType<Sum>(TensorFactory.Parse("a+b"));

        Assert.Equal(2, sum.Size);
        Assert.IsType<SumBuilder>(sum.GetBuilder());
        Assert.Same(SumFactory.Factory, sum.GetFactory());
    }

    [Fact]
    public void ShouldWrapStringInsideProductsAndPowers()
    {
        Sum sum = Assert.IsType<Sum>(TensorFactory.Parse("a+b"));

        Assert.Equal("(a+b)", sum.ToStringWith<Product>(OutputFormat.Redberry));
        Assert.Equal("(a+b)", sum.ToStringWith<Power>(OutputFormat.Redberry));
    }

    [Fact]
    public void ShouldRemoveAndSelectTerms()
    {
        Sum sum = Assert.IsType<Sum>(TensorFactory.Parse("a+b+c"));

        NRedberry.Tensors.Tensor removed = sum.Remove(1);
        NRedberry.Tensors.Tensor selected = sum.Select([0, 2]);

        Assert.Equal(2, removed.Size);
        Assert.Contains("a", removed.ToString(OutputFormat.Redberry));
        Assert.Contains("c", removed.ToString(OutputFormat.Redberry));
        Assert.DoesNotContain("b", removed.ToString(OutputFormat.Redberry));
        Assert.Equal(2, selected.Size);
    }

    [Fact]
    public void ShouldUseSumSpecificSetSemantics()
    {
        Sum sum = Assert.IsType<Sum>(TensorFactory.Parse("a+b+c"));

        NRedberry.Tensors.Tensor removed = sum.Set(1, Complex.Zero);
        NRedberry.Tensors.Tensor unchanged = sum.Set(0, sum[0]);

        Assert.Equal(2, removed.Size);
        Assert.Same(sum, unchanged);
    }
}
