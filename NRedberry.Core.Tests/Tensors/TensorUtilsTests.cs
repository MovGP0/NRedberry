using NRedberry.Numbers;
using NRedberry.Tensors;
using TensorApi = NRedberry.Tensors.Tensors;
using Xunit;

namespace NRedberry.Core.Tests.Tensors;

public sealed class TensorUtilsTests
{
    [Fact]
    public void ShouldReportInfoAndSymbolCounts()
    {
        NRedberry.Tensors.Tensor tensor = TensorApi.Parse("a+b*c");

        string info = TensorUtils.Info(tensor);

        info.ShouldContain("[Sum");
        info.ShouldContain("size = 2");
        info.ShouldContain("symbolsCount = 3");
    }

    [Fact]
    public void ShouldDetectBasicTensorProperties()
    {
        TensorUtils.IsSymbol(TensorApi.Parse("a")).ShouldBeTrue();
        TensorUtils.IsSymbolOrNumber(Complex.One).ShouldBeTrue();
        TensorUtils.IsIndexless(TensorApi.Parse("a"), TensorApi.Parse("b")).ShouldBeTrue();
        TensorUtils.IsScalar(TensorApi.Parse("a*b")).ShouldBeTrue();
        TensorUtils.IsSymbolic(TensorApi.Parse("a*(b+c)")).ShouldBeTrue();
        TensorUtils.IsOne(Complex.One).ShouldBeTrue();
        TensorUtils.IsZero(Complex.Zero).ShouldBeTrue();
        TensorUtils.IsMinusOne(Complex.MinusOne).ShouldBeTrue();
        TensorUtils.IsPositiveIntegerPower(TensorApi.Pow(TensorApi.Parse("a"), 2)).ShouldBeTrue();
        TensorUtils.IsNegativeIntegerPower(TensorApi.Reciprocal(TensorApi.Parse("a"))).ShouldBeTrue();
    }

    [Fact]
    public void ShouldCollectIndicesAndDummyNames()
    {
        NRedberry.Tensors.Tensor tensor = TensorApi.Parse("f_m*g^m");

        TensorUtils.PassOutDummies(tensor).ShouldBeTrue();
        TensorUtils.GetAllDummyIndicesT(tensor).ShouldHaveSingleItem();
        TensorUtils.GetAllIndicesNamesT(tensor).ShouldHaveSingleItem();
    }

    [Fact]
    public void ShouldComputeDepthFractionsAndSharedSymbols()
    {
        NRedberry.Tensors.Tensor first = TensorApi.Parse("a*(b+c)");
        NRedberry.Tensors.Tensor second = TensorApi.Parse("a*d");
        NRedberry.Tensors.Tensor fraction = TensorApi.Multiply(
            TensorApi.Parse("a"),
            TensorApi.Reciprocal(TensorApi.Parse("b")));
        NRedberry.Tensors.Tensor symbolA1 = TensorApi.Parse("a");
        NRedberry.Tensors.Tensor symbolA2 = TensorApi.Parse("a");
        NRedberry.Tensors.Tensor symbolB = TensorApi.Parse("b");

        TensorUtils.TreeDepth(first).ShouldBe(2);
        TensorUtils.ContainsFractions(fraction).ShouldBeTrue();
        TensorUtils.ShareSimpleTensors(first, second).ShouldBeTrue();
        TensorUtils.GetAllNamesOfSymbols(symbolA1, symbolA2, symbolB).Count.ShouldBe(2);
        TensorUtils.GetAllDiffSimpleTensors(symbolA1, symbolA2, symbolB).Count.ShouldBe(2);
    }
}
