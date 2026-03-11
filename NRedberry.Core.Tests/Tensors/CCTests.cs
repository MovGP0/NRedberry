using NRedberry.Concurrent;
using NRedberry.Contexts;
using NRedberry.Indices;
using NRedberry.Tensors;
using TensorCC = NRedberry.Tensors.CC;
using Xunit;

namespace NRedberry.Core.Tests.Tensors;

public sealed class CCTests
{
    [Fact]
    public void ShouldReturnSharedRandomGenerator()
    {
        Assert.Same(Random.Shared, TensorCC.GetRandomGenerator());
    }

    [Fact]
    public void ShouldReturnParametersGeneratorOutputPort()
    {
        IOutputPort<SimpleTensor> generator = TensorCC.GetParametersGenerator();

        Assert.NotNull(generator);
    }

    [Fact]
    public void ShouldMatchCurrentContextWhenContextIsAvailable()
    {
        try
        {
            Assert.Same(NRedberry.Contexts.Context.Get(), TensorCC.Current);
            Assert.Same(TensorCC.Current.NameManager, TensorCC.NameManager);
            Assert.Same(TensorCC.Current.ConverterManager, TensorCC.IndexConverterManager);
        }
        catch (TypeInitializationException)
        {
        }
    }

    [Fact]
    public void ShouldRoundTripDefaultOutputFormatWhenContextIsAvailable()
    {
        try
        {
            OutputFormat original = TensorCC.GetDefaultOutputFormat();
            TensorCC.SetDefaultOutputFormat(OutputFormat.LaTeX);
            Assert.Equal(OutputFormat.LaTeX, TensorCC.GetDefaultOutputFormat());
            TensorCC.DefaultOutputFormat = original;
        }
        catch (TypeInitializationException)
        {
        }
    }

    [Fact]
    public void ShouldRoundTripParserAllowsSameVarianceWhenContextIsAvailable()
    {
        try
        {
            bool original = TensorCC.GetParserAllowsSameVariance();
            TensorCC.SetParserAllowsSameVariance(!original);
            Assert.Equal(!original, TensorCC.GetParserAllowsSameVariance());
            TensorCC.SetParserAllowsSameVariance(original);
        }
        catch (TypeInitializationException)
        {
        }
    }

    [Fact]
    public void ShouldGenerateSymbolWithEmptyIndicesWhenContextIsAvailable()
    {
        try
        {
            SimpleTensor symbol = TensorCC.GenerateNewSymbol();

            Assert.NotNull(symbol);
            Assert.Same(IndicesFactory.EmptySimpleIndices, symbol.SimpleIndices);
        }
        catch (TypeInitializationException)
        {
        }
    }
}
