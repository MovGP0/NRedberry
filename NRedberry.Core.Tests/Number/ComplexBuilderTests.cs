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

        var exception = Should.Throw<TargetInvocationException>(() => Activator.CreateInstance(builderType, [null]));
        ArgumentNullException argumentNullException = exception.InnerException.ShouldBeOfType<ArgumentNullException>();
        argumentNullException.ParamName.ShouldBe("complex");
    }

    [Fact]
    public void BuildShouldReturnSameComplexInstance()
    {
        Complex complex = new(3, 4);
        TensorBuilder builder = complex.GetBuilder();

        var result = builder.Build();

        result.ShouldBeSameAs(complex);
    }

    [Fact]
    public void CloneShouldReturnSameBuilderInstance()
    {
        Complex complex = new(2, -1);
        TensorBuilder builder = complex.GetBuilder();

        TensorBuilder clone = builder.Clone();

        clone.ShouldBeSameAs(builder);
    }

    [Fact]
    public void PutShouldThrowInvalidOperationException()
    {
        Complex complex = new(1, 1);
        TensorBuilder builder = complex.GetBuilder();

        InvalidOperationException exception = Should.Throw<InvalidOperationException>(() => builder.Put(Complex.Zero));
        exception.Message.ShouldContain("Can not put to Complex tensor builder");
    }
}
