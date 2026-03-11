using NRedberry.Numbers;
using NRedberry.Tensors;
using TensorFactory = NRedberry.Tensors.Tensors;
using Xunit;

namespace NRedberry.Core.Tests.Tensor;

public sealed class PowerBuilderTest
{
    [Fact]
    public void ShouldComputePowerForPositiveIntegers()
    {
        PowerBuilder builder = new();
        builder.Put(new Complex(2));
        builder.Put(new Complex(2));

        NRedberry.Tensors.Tensor result = builder.Build();

        Assert.Equal(new Complex(4), result);
    }

    [Fact]
    public void ShouldComputeLargeIntegerPower()
    {
        PowerBuilder builder = new();
        builder.Put(new Complex(3));
        builder.Put(new Complex(12));

        NRedberry.Tensors.Tensor result = builder.Build();

        Assert.Equal(new Complex(531441), result);
    }

    [Fact]
    public void ShouldComputeOddPowerOfNegativeBase()
    {
        PowerBuilder builder = new();
        builder.Put(new Complex(-3));
        builder.Put(new Complex(11));

        NRedberry.Tensors.Tensor result = builder.Build();

        Assert.Equal(new Complex(-177147), result);
    }

    [Fact]
    public void ShouldThrowWhenTensorIsNotScalar()
    {
        PowerBuilder builder = new();

        ArgumentException exception = Assert.Throws<ArgumentException>(() => builder.Put(TensorFactory.Parse("T_a")));

        Assert.Contains("Non-scalar tensor", exception.Message);
    }

    [Fact]
    public void ShouldThrowWhenBuildIsIncomplete()
    {
        PowerBuilder builder = new();
        builder.Put(new Complex(2));

        Assert.Throws<InvalidOperationException>(() => builder.Build());
    }

    [Fact]
    public void ShouldRejectThirdPut()
    {
        PowerBuilder builder = new();
        builder.Put(new Complex(2));
        builder.Put(new Complex(3));

        Assert.Throws<InvalidOperationException>(() => builder.Put(new Complex(4)));
    }

    [Fact]
    public void ShouldCloneCurrentState()
    {
        PowerBuilder builder = new();
        builder.Put(new Complex(2));
        TensorBuilder clone = builder.Clone();

        clone.Put(new Complex(5));

        Assert.Equal(new Complex(32), clone.Build());
        Assert.Throws<InvalidOperationException>(() => builder.Build());
    }
}
