using System;
using System.Reflection;
using NRedberry.Numbers;
using NRedberry.Tensors;
using Xunit;

namespace NRedberry.Core.Tests.Number;

public sealed class ComplexBuilderTests
{
    [Fact]
    public void ConstructorShouldThrowWhenComplexIsNull()
    {
        Type builderType = typeof(Complex).Assembly.GetType("NRedberry.Numbers.ComplexBuilder", throwOnError: true)!;

        var exception = Assert.Throws<TargetInvocationException>(() => Activator.CreateInstance(builderType, [null]));
        ArgumentNullException argumentNullException = Assert.IsType<ArgumentNullException>(exception.InnerException);
        Assert.Equal("complex", argumentNullException.ParamName);
    }

    [Fact]
    public void BuildShouldReturnSameComplexInstance()
    {
        Complex complex = new(3, 4);
        TensorBuilder builder = complex.GetBuilder();

        var result = builder.Build();

        Assert.Same(complex, result);
    }

    [Fact]
    public void CloneShouldReturnSameBuilderInstance()
    {
        Complex complex = new(2, -1);
        TensorBuilder builder = complex.GetBuilder();

        TensorBuilder clone = builder.Clone();

        Assert.Same(builder, clone);
    }

    [Fact]
    public void PutShouldThrowInvalidOperationException()
    {
        Complex complex = new(1, 1);
        TensorBuilder builder = complex.GetBuilder();

        InvalidOperationException exception = Assert.Throws<InvalidOperationException>(() => builder.Put(Complex.Zero));
        Assert.Contains("Can not put to Complex tensor builder", exception.Message, StringComparison.Ordinal);
    }
}
