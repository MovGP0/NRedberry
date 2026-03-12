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

        Assert.Contains("[Sum", info);
        Assert.Contains("size = 2", info);
        Assert.Contains("symbolsCount = 3", info);
    }

    [Fact]
    public void ShouldDetectBasicTensorProperties()
    {
        Assert.True(TensorUtils.IsSymbol(TensorApi.Parse("a")));
        Assert.True(TensorUtils.IsSymbolOrNumber(Complex.One));
        Assert.True(TensorUtils.IsIndexless(TensorApi.Parse("a"), TensorApi.Parse("b")));
        Assert.True(TensorUtils.IsScalar(TensorApi.Parse("a*b")));
        Assert.True(TensorUtils.IsSymbolic(TensorApi.Parse("a*(b+c)")));
        Assert.True(TensorUtils.IsOne(Complex.One));
        Assert.True(TensorUtils.IsZero(Complex.Zero));
        Assert.True(TensorUtils.IsMinusOne(Complex.MinusOne));
        Assert.True(TensorUtils.IsPositiveIntegerPower(TensorApi.Pow(TensorApi.Parse("a"), 2)));
        Assert.True(TensorUtils.IsNegativeIntegerPower(TensorApi.Reciprocal(TensorApi.Parse("a"))));
    }

    [Fact]
    public void ShouldCollectIndicesAndDummyNames()
    {
        NRedberry.Tensors.Tensor tensor = TensorApi.Parse("f_m*g^m");

        Assert.True(TensorUtils.PassOutDummies(tensor));
        Assert.Single(TensorUtils.GetAllDummyIndicesT(tensor));
        Assert.Single(TensorUtils.GetAllIndicesNamesT(tensor));
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

        Assert.Equal(2, TensorUtils.TreeDepth(first));
        Assert.True(TensorUtils.ContainsFractions(fraction));
        Assert.True(TensorUtils.ShareSimpleTensors(first, second));
        Assert.Equal(2, TensorUtils.GetAllNamesOfSymbols(symbolA1, symbolA2, symbolB).Count);
        Assert.Equal(2, TensorUtils.GetAllDiffSimpleTensors(symbolA1, symbolA2, symbolB).Count);
    }
}
