using NRedberry.Numbers;
using NRedberry.Tensors;
using TensorFactory = NRedberry.Tensors.Tensors;

namespace NRedberry.Core.Tests.Tensors;

public sealed class ScalarsBackedProductBuilderTests
{
    [Fact]
    public void ShouldBuildPureNumericFactor()
    {
        ScalarsBackedProductBuilder builder = new();
        builder.Put(new Complex(2));
        builder.Put(new Complex(3));

        builder.Build().ShouldBe(new Complex(6));
    }

    [Fact]
    public void ShouldCombineScalarAndTensorParts()
    {
        ScalarsBackedProductBuilder builder = new();
        builder.Put(TensorFactory.Parse("2*a"));
        builder.Put(TensorFactory.Parse("3*T_i"));

        builder.Build().ToString(OutputFormat.Redberry).ShouldBe("6*a*T_{i}");
    }

    [Fact]
    public void ShouldCloneStateIndependently()
    {
        ScalarsBackedProductBuilder builder = new();
        builder.Put(TensorFactory.Parse("a"));

        TensorBuilder clone = builder.Clone();
        clone.Put(TensorFactory.Parse("b"));

        builder.Build().ToString(OutputFormat.Redberry).ShouldBe("a");
        TensorUtils.Equals(clone.Build(), TensorFactory.Parse("a*b")).ShouldBeTrue();
    }

    [Fact]
    public void ShouldThrowWhenTensorIsNull()
    {
        ScalarsBackedProductBuilder builder = new();

        Should.Throw<ArgumentNullException>(() => builder.Put(null!));
    }
}
