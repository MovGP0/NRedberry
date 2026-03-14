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

        Should.Throw<ArgumentNullException>(() => new GeneratedTensor(null!, tensor));
    }

    [Fact]
    public void ShouldThrowWhenTensorIsNull()
    {
        var coefficients = new[] { (SimpleTensor)RuntimeHelpers.GetUninitializedObject(typeof(SimpleTensor)) };

        Should.Throw<ArgumentNullException>(() => new GeneratedTensor(coefficients, null!));
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

        generated.Coefficients.ShouldBeSameAs(coefficients);
        generated.Tensor.ShouldBeSameAs(tensor);
    }
}

public sealed class SymbolsGeneratorTests
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void ShouldThrowWhenNameIsNullOrEmpty(string? name)
    {
        Should.Throw<ArgumentException>(() => new SymbolsGenerator(name!));
    }

    [Fact]
    public void ShouldGenerateSequentialSymbolsAfterMoveNext()
    {
        SymbolsGenerator generator = new("a");

        generator.MoveNext().ShouldBeTrue();
        CountProperty.GetValue(generator).ShouldBe(1);
        generator.MoveNext().ShouldBeTrue();
        CountProperty.GetValue(generator).ShouldBe(2);
    }

    [Fact]
    public void ShouldResetCounter()
    {
        SymbolsGenerator generator = new("z");

        generator.MoveNext().ShouldBeTrue();
        CountProperty.GetValue(generator).ShouldBe(1);
        generator.MoveNext().ShouldBeTrue();
        CountProperty.GetValue(generator).ShouldBe(2);

        generator.Reset();
        CountProperty.GetValue(generator).ShouldBe(0);

        generator.MoveNext().ShouldBeTrue();
        CountProperty.GetValue(generator).ShouldBe(1);
    }

    private static PropertyInfo CountProperty => typeof(SymbolsGenerator).GetProperty("Count", BindingFlags.Instance | BindingFlags.NonPublic)!;
}
