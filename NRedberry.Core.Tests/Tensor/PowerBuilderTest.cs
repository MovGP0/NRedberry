using NRedberry.Numbers;
using NRedberry.Tensors;
using TensorFactory = NRedberry.Tensors.Tensors;

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

        result.ShouldBe(new Complex(4));
    }

    [Fact]
    public void ShouldComputeLargeIntegerPower()
    {
        PowerBuilder builder = new();
        builder.Put(new Complex(3));
        builder.Put(new Complex(12));

        NRedberry.Tensors.Tensor result = builder.Build();

        result.ShouldBe(new Complex(531441));
    }

    [Fact]
    public void ShouldComputeOddPowerOfNegativeBase()
    {
        PowerBuilder builder = new();
        builder.Put(new Complex(-3));
        builder.Put(new Complex(11));

        NRedberry.Tensors.Tensor result = builder.Build();

        result.ShouldBe(new Complex(-177147));
    }

    [Fact]
    public void ShouldThrowWhenTensorIsNotScalar()
    {
        PowerBuilder builder = new();

        ArgumentException exception = Should.Throw<ArgumentException>(() => builder.Put(TensorFactory.Parse("T_a")));

        exception.Message.ShouldContain("Non-scalar tensor");
    }

    [Fact]
    public void ShouldThrowWhenBuildIsIncomplete()
    {
        PowerBuilder builder = new();
        builder.Put(new Complex(2));

        Should.Throw<InvalidOperationException>(() => builder.Build());
    }

    [Fact]
    public void ShouldRejectThirdPut()
    {
        PowerBuilder builder = new();
        builder.Put(new Complex(2));
        builder.Put(new Complex(3));

        Should.Throw<InvalidOperationException>(() => builder.Put(new Complex(4)));
    }

    [Fact]
    public void ShouldCloneCurrentState()
    {
        PowerBuilder builder = new();
        builder.Put(new Complex(2));
        TensorBuilder clone = builder.Clone();

        clone.Put(new Complex(5));

        clone.Build().ShouldBe(new Complex(32));
        Should.Throw<InvalidOperationException>(() => builder.Build());
    }
}
