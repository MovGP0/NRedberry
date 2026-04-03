using NRedberry.Indices;
using NRedberry.Numbers;
using NRedberry.Tensors;
using TensorType = NRedberry.Tensors.Tensor;

namespace NRedberry.Core.Tests.Tensor;

public sealed class FastTensorsTest
{
    [Fact]
    public void ShouldMultiplySumElementsOnFactor()
    {
        Sum sum = CreateNumericSum(Complex.One, Complex.Two);

        TensorType result = FastTensors.MultiplySumElementsOnFactor(sum, Complex.Two);

        TensorUtils.EqualsExactly(result, CreateNumericSum(Complex.Two, new Complex(4))).ShouldBeTrue();
    }

    [Fact]
    public void ShouldMultiplySumElementsOnFactorWithImaginaryUnit()
    {
        Sum sum = CreateNumericSum(Complex.One, Complex.Two);

        TensorType result = FastTensors.MultiplySumElementsOnFactor(sum, Complex.ImaginaryOne);

        TensorUtils.EqualsExactly(result, CreateNumericSum(Complex.ImaginaryOne, new Complex(0, 2))).ShouldBeTrue();
    }

    [Fact]
    public void ShouldMultiplySumElementsOnFactorAndExpand()
    {
        Sum sum = CreateNumericSum(Complex.One, Complex.Two);

        TensorType result = FastTensors.MultiplySumElementsOnFactorAndExpand(sum, Complex.Two);

        TensorUtils.EqualsExactly(result, CreateNumericSum(Complex.Two, new Complex(4))).ShouldBeTrue();
    }

    [Fact]
    public void ShouldReturnZeroWhenFactorIsZero()
    {
        Sum sum = CreateNumericSum(Complex.One, Complex.Two);

        TensorType result = FastTensors.MultiplySumElementsOnFactor(sum, Complex.Zero);

        result.ShouldBeSameAs(Complex.Zero);
    }

    [Fact]
    public void ShouldReturnSameInstanceWhenFactorIsOne()
    {
        Sum sum = CreateNumericSum(Complex.One, Complex.Two);

        TensorType result = FastTensors.MultiplySumElementsOnFactor(sum, Complex.One);

        result.ShouldBeSameAs(sum);
    }

    [Fact]
    public void ShouldChainMultiplyAndExpandOperations()
    {
        Sum sum = CreateNumericSum(Complex.One, Complex.Two);

        TensorType first = FastTensors.MultiplySumElementsOnFactor(sum, Complex.Two);
        TensorType second = FastTensors.MultiplySumElementsOnFactorAndExpand((Sum)first, Complex.Two);

        TensorUtils.EqualsExactly(second, CreateNumericSum(new Complex(4), new Complex(8))).ShouldBeTrue();
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

        Should.Throw<ArgumentException>(() => FastTensors.MultiplySumElementsOnFactorAndExpand(sum, indexedFactor));
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
