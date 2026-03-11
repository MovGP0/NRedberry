using NRedberry.Indices;
using NRedberry.Numbers;
using NRedberry.Tensors;
using TensorType = NRedberry.Tensors.Tensor;
using Xunit;

namespace NRedberry.Core.Tests.Tensor;

public sealed class FastTensorsTest
{
    [Fact]
    public void ShouldMultiplySumElementsOnFactor()
    {
        Sum sum = CreateNumericSum(Complex.One, Complex.Two);

        TensorType result = FastTensors.MultiplySumElementsOnFactor(sum, Complex.Two);

        Assert.True(TensorUtils.EqualsExactly(result, CreateNumericSum(Complex.Two, new Complex(4))));
    }

    [Fact]
    public void ShouldMultiplySumElementsOnFactorWithImaginaryUnit()
    {
        Sum sum = CreateNumericSum(Complex.One, Complex.Two);

        TensorType result = FastTensors.MultiplySumElementsOnFactor(sum, Complex.ImaginaryOne);

        Assert.True(TensorUtils.EqualsExactly(result, CreateNumericSum(Complex.ImaginaryOne, new Complex(0, 2))));
    }

    [Fact]
    public void ShouldMultiplySumElementsOnFactorAndExpand()
    {
        Sum sum = CreateNumericSum(Complex.One, Complex.Two);

        TensorType result = FastTensors.MultiplySumElementsOnFactorAndExpand(sum, Complex.Two);

        Assert.True(TensorUtils.EqualsExactly(result, CreateNumericSum(Complex.Two, new Complex(4))));
    }

    [Fact]
    public void ShouldReturnZeroWhenFactorIsZero()
    {
        Sum sum = CreateNumericSum(Complex.One, Complex.Two);

        TensorType result = FastTensors.MultiplySumElementsOnFactor(sum, Complex.Zero);

        Assert.Same(Complex.Zero, result);
    }

    [Fact]
    public void ShouldReturnSameInstanceWhenFactorIsOne()
    {
        Sum sum = CreateNumericSum(Complex.One, Complex.Two);

        TensorType result = FastTensors.MultiplySumElementsOnFactor(sum, Complex.One);

        Assert.Same(sum, result);
    }

    [Fact]
    public void ShouldChainMultiplyAndExpandOperations()
    {
        Sum sum = CreateNumericSum(Complex.One, Complex.Two);

        TensorType first = FastTensors.MultiplySumElementsOnFactor(sum, Complex.Two);
        TensorType second = FastTensors.MultiplySumElementsOnFactorAndExpand((Sum)first, Complex.Two);

        Assert.True(TensorUtils.EqualsExactly(second, CreateNumericSum(new Complex(4), new Complex(8))));
    }

    [Fact]
    public void ShouldThrowWhenExpandingWithIndexedSumFactor()
    {
        Sum sum = CreateNumericSum(Complex.One, Complex.Two);
        Sum indexedFactor = new(
            [
                CreateSimpleTensor(1, Lower(1)),
                CreateSimpleTensor(2, Lower(1))
            ],
            IndicesFactory.Create(Lower(1)));

        Assert.Throws<ArgumentException>(() => FastTensors.MultiplySumElementsOnFactorAndExpand(sum, indexedFactor));
    }

    private static Sum CreateNumericSum(params Complex[] terms)
    {
        TensorType[] data = terms.Cast<TensorType>().ToArray();
        return new Sum(data, IndicesFactory.EmptyIndices);
    }

    private static SimpleTensor CreateSimpleTensor(int name, params int[] indices)
    {
        return new(name, IndicesFactory.CreateSimple(null, indices));
    }

    private static int Lower(int name)
    {
        return IndicesUtils.CreateIndex(name, (byte)0, false);
    }
}
