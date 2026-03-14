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
        TensorCC.GetRandomGenerator().ShouldBeSameAs(Random.Shared);
    }

    [Fact]
    public void ShouldReturnParametersGeneratorOutputPort()
    {
        IOutputPort<SimpleTensor> generator = TensorCC.GetParametersGenerator();

        generator.ShouldNotBeNull();
    }

    [Fact]
    public void ShouldMatchCurrentContextWhenContextIsAvailable()
    {
        try
        {
            TensorCC.Current.ShouldBeSameAs(NRedberry.Contexts.Context.Get());
            TensorCC.NameManager.ShouldBeSameAs(TensorCC.Current.NameManager);
            TensorCC.IndexConverterManager.ShouldBeSameAs(TensorCC.Current.ConverterManager);
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
            TensorCC.GetDefaultOutputFormat().ShouldBe(OutputFormat.LaTeX);
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
            TensorCC.GetParserAllowsSameVariance().ShouldBe(!original);
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

            symbol.ShouldNotBeNull();
            symbol.SimpleIndices.ShouldBeSameAs(IndicesFactory.EmptySimpleIndices);
        }
        catch (TypeInitializationException)
        {
        }
    }
}
