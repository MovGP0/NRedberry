using NRedberry.TensorGenerators;
using NRedberry.Tensors;
using System.Reflection;
using System.Runtime.CompilerServices;
using TensorType = NRedberry.Tensors.Tensor;
using Xunit;

namespace NRedberry.Core.Tests.Tensorgenerator;

public sealed class GeneratedTensorTests
{
    [Fact]
    public void ShouldThrowWhenCoefficientsAreNull()
    {
        TensorType tensor = (Expression)RuntimeHelpers.GetUninitializedObject(typeof(Expression));

        Assert.Throws<ArgumentNullException>(() => new GeneratedTensor(null!, tensor));
    }

    [Fact]
    public void ShouldThrowWhenTensorIsNull()
    {
        var coefficients = new[] { (SimpleTensor)RuntimeHelpers.GetUninitializedObject(typeof(SimpleTensor)) };

        Assert.Throws<ArgumentNullException>(() => new GeneratedTensor(coefficients, null!));
    }

    [Fact]
    public void ShouldExposeConstructorValues()
    {
        var coefficients =
            new[]
            {
                (SimpleTensor)RuntimeHelpers.GetUninitializedObject(typeof(SimpleTensor)),
                (SimpleTensor)RuntimeHelpers.GetUninitializedObject(typeof(SimpleTensor))
            };
        TensorType tensor = (Expression)RuntimeHelpers.GetUninitializedObject(typeof(Expression));

        GeneratedTensor generated = new(coefficients, tensor);

        Assert.Same(coefficients, generated.Coefficients);
        Assert.Same(tensor, generated.Tensor);
    }
}

public sealed class SymbolsGeneratorTests
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void ShouldThrowWhenNameIsNullOrEmpty(string? name)
    {
        Assert.Throws<ArgumentException>(() => new SymbolsGenerator(name!));
    }

    [Fact]
    public void ShouldGenerateSequentialSymbolsAfterMoveNext()
    {
        SymbolsGenerator generator = new("a");

        Assert.True(generator.MoveNext());
        Assert.Equal(1, CountProperty.GetValue(generator));
        Assert.True(generator.MoveNext());
        Assert.Equal(2, CountProperty.GetValue(generator));
    }

    [Fact]
    public void ShouldResetCounter()
    {
        SymbolsGenerator generator = new("z");

        Assert.True(generator.MoveNext());
        Assert.Equal(1, CountProperty.GetValue(generator));
        Assert.True(generator.MoveNext());
        Assert.Equal(2, CountProperty.GetValue(generator));

        generator.Reset();
        Assert.Equal(0, CountProperty.GetValue(generator));

        Assert.True(generator.MoveNext());
        Assert.Equal(1, CountProperty.GetValue(generator));
    }

    private static PropertyInfo CountProperty => typeof(SymbolsGenerator).GetProperty("Count", BindingFlags.Instance | BindingFlags.NonPublic)!;
}
