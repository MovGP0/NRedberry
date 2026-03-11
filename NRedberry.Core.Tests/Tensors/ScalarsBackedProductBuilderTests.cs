using NRedberry.Numbers;
using NRedberry.Tensors;
using TensorFactory = NRedberry.Tensors.Tensors;
using Xunit;

namespace NRedberry.Core.Tests.Tensors;

public sealed class ScalarsBackedProductBuilderTests
{
    [Fact]
    public void ShouldBuildPureNumericFactor()
    {
        ScalarsBackedProductBuilder builder = new();
        builder.Put(new Complex(2));
        builder.Put(new Complex(3));

        Assert.Equal(new Complex(6), builder.Build());
    }

    [Fact]
    public void ShouldCombineScalarAndTensorParts()
    {
        ScalarsBackedProductBuilder builder = new();
        builder.Put(TensorFactory.Parse("2*a"));
        builder.Put(TensorFactory.Parse("3*T_i"));

        Assert.Equal("6*a*T_{i}", builder.Build().ToString(OutputFormat.Redberry));
    }

    [Fact]
    public void ShouldCloneStateIndependently()
    {
        ScalarsBackedProductBuilder builder = new();
        builder.Put(TensorFactory.Parse("a"));

        TensorBuilder clone = builder.Clone();
        clone.Put(TensorFactory.Parse("b"));

        Assert.Equal("a", builder.Build().ToString(OutputFormat.Redberry));
        Assert.Equal("a*b", clone.Build().ToString(OutputFormat.Redberry));
    }

    [Fact]
    public void ShouldThrowWhenTensorIsNull()
    {
        ScalarsBackedProductBuilder builder = new();

        Assert.Throws<ArgumentNullException>(() => builder.Put(null!));
    }
}
