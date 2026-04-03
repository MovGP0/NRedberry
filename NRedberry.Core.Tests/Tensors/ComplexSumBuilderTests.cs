using NRedberry.Numbers;
using NRedberry.Tensors;
using TensorType = NRedberry.Tensors.Tensor;

namespace NRedberry.Core.Tests.Tensors;

public sealed class ComplexSumBuilderTests
{
    [Fact]
    public void ShouldBuildZeroByDefault()
    {
        object builder = Activator.CreateInstance(BuilderType)!;

        Complex result = Build(builder).ShouldBeOfType<Complex>();

        result.ShouldBe(Complex.Zero);
    }

    [Fact]
    public void ShouldBuildSeededComplex()
    {
        object builder = Activator.CreateInstance(BuilderType, new object[] { Complex.Two })!;

        Complex result = Build(builder).ShouldBeOfType<Complex>();

        result.ShouldBe(Complex.Two);
    }

    [Fact]
    public void ShouldAccumulatePutValues()
    {
        object builder = Activator.CreateInstance(BuilderType)!;

        Put(builder, Complex.One);
        Put(builder, Complex.Two);

        Complex result = Build(builder).ShouldBeOfType<Complex>();

        result.ShouldBe(new Complex(3));
    }

    [Fact]
    public void ShouldCloneWithoutSharingState()
    {
        object builder = Activator.CreateInstance(BuilderType, new object[] { Complex.One })!;
        object clone = Clone(builder);

        Put(clone, Complex.Two);

        Complex originalResult = Build(builder).ShouldBeOfType<Complex>();
        Complex cloneResult = Build(clone).ShouldBeOfType<Complex>();

        originalResult.ShouldBe(Complex.One);
        cloneResult.ShouldBe(new Complex(3));
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
