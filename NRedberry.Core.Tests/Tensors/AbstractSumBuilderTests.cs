using NRedberry.Numbers;
using NRedberry.Tensors;
using TensorType = NRedberry.Tensors.Tensor;
using Xunit;

namespace NRedberry.Core.Tests.Tensors;

public sealed class AbstractSumBuilderTests
{
    [Fact]
    public void ShouldThrowWhenPutArgumentIsNull()
    {
        TestSumBuilder builder = new();

        Assert.Throws<ArgumentNullException>(() => builder.Put(null!));
    }

    [Fact]
    public void ShouldReturnZeroWhenNoSummandsAdded()
    {
        TestSumBuilder builder = new();

        TensorType result = builder.Build();

        Assert.Same(Complex.Zero, result);
    }

    [Fact]
    public void ShouldAccumulateComplexSummands()
    {
        TestSumBuilder builder = new();
        builder.Put(Complex.One);
        builder.Put(Complex.Two);

        TensorType result = builder.Build();

        Assert.Equal(new Complex(3), result);
    }

    [Fact]
    public void ShouldReturnNaNWhenIndeterminateWasAdded()
    {
        TestSumBuilder builder = new();
        builder.Put(Complex.ComplexNaN);
        builder.Put(Complex.One);

        TensorType result = builder.Build();

        Complex complex = Assert.IsType<Complex>(result);
        Assert.True(complex.IsNaN());
    }

    [Fact]
    public void ShouldIgnoreFurtherInputWhenNaNAlreadyPresent()
    {
        TestSumBuilder builder = new();
        builder.Put(Complex.ComplexNaN);
        builder.Put(new Complex(100));

        TensorType result = builder.Build();

        Complex complex = Assert.IsType<Complex>(result);
        Assert.True(complex.IsNaN());
    }

    private sealed class TestSumBuilder : AbstractSumBuilder
    {
        protected override Split Split(TensorType tensor)
        {
            return new Split(Complex.One, tensor);
        }

        public override TensorBuilder Clone()
        {
            return new TestSumBuilder();
        }
    }
}
