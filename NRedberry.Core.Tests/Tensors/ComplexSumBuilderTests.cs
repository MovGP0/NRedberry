using System.Reflection;
using NRedberry.Numbers;
using NRedberry.Tensors;
using TensorType = NRedberry.Tensors.Tensor;
using Xunit;

namespace NRedberry.Core.Tests.Tensors;

public sealed class ComplexSumBuilderTests
{
    [Fact]
    public void ShouldBuildZeroByDefault()
    {
        object builder = Activator.CreateInstance(BuilderType)!;

        Complex result = Assert.IsType<Complex>(Build(builder));

        Assert.Equal(Complex.Zero, result);
    }

    [Fact]
    public void ShouldBuildSeededComplex()
    {
        object builder = Activator.CreateInstance(BuilderType, new object[] { Complex.Two })!;

        Complex result = Assert.IsType<Complex>(Build(builder));

        Assert.Equal(Complex.Two, result);
    }

    [Fact]
    public void ShouldAccumulatePutValues()
    {
        object builder = Activator.CreateInstance(BuilderType)!;

        Put(builder, Complex.One);
        Put(builder, Complex.Two);

        Complex result = Assert.IsType<Complex>(Build(builder));

        Assert.Equal(new Complex(3), result);
    }

    [Fact]
    public void ShouldCloneWithoutSharingState()
    {
        object builder = Activator.CreateInstance(BuilderType, new object[] { Complex.One })!;
        object clone = Clone(builder);

        Put(clone, Complex.Two);

        Complex originalResult = Assert.IsType<Complex>(Build(builder));
        Complex cloneResult = Assert.IsType<Complex>(Build(clone));

        Assert.Equal(Complex.One, originalResult);
        Assert.Equal(new Complex(3), cloneResult);
    }

    private static TensorType Build(object builder)
    {
        return (TensorType)BuilderType.GetMethod("Build")!.Invoke(builder, null)!;
    }

    private static void Put(object builder, TensorType tensor)
    {
        BuilderType.GetMethod("Put")!.Invoke(builder, [tensor]);
    }

    private static object Clone(object builder)
    {
        return BuilderType.GetMethod("Clone")!.Invoke(builder, null)!;
    }

    private static Type BuilderType => typeof(SumBuilder).Assembly.GetType("NRedberry.Tensors.ComplexSumBuilder", true)!;
}
