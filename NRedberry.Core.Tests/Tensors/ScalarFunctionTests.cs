using NRedberry.Tensors;
using NRedberry.Tensors.Functions;
using TensorApi = NRedberry.Tensors.Tensors;
using Xunit;

namespace NRedberry.Core.Tests.Tensors;

public sealed class ScalarFunctionTests
{
    [Fact]
    public void ShouldExposeScalarArgumentAndFormatAcrossOutputs()
    {
        NRedberry.Tensors.Tensor argument = TensorApi.Parse("a");
        TestScalarFunction function = new(argument);

        Assert.Equal(0, function.Indices.Size());
        Assert.Same(argument, function[0]);
        Assert.Equal(1, function.Size);
        Assert.Equal("TestFn[a]", function.ToString(OutputFormat.Redberry));
        Assert.Equal("\\testfn(a)", function.ToString(OutputFormat.LaTeX));
        Assert.Equal("TestFn(a)", function.ToString(OutputFormat.UTF8));
    }

    [Fact]
    public void ShouldRejectNonScalarArguments()
    {
        Assert.Throws<TensorException>(() => new TestScalarFunction(TensorApi.Parse("f_m")));
    }
}

internal sealed class TestScalarFunction(NRedberry.Tensors.Tensor argument) : ScalarFunction(argument)
{
    protected override string FunctionName() => "TestFn";

    public override NRedberry.Tensors.Tensor Derivative() => Argument;

    public override int GetHashCode() => 17 * Argument.GetHashCode();

    public override TensorBuilder GetBuilder() => new ScalarFunctionBuilder(TestScalarFunctionFactory.Instance);

    public override TensorFactory GetFactory() => TestScalarFunctionFactory.Instance;
}

internal sealed class TestScalarFunctionFactory : ScalarFunctionFactory
{
    public static TestScalarFunctionFactory Instance { get; } = new();

    private TestScalarFunctionFactory()
    {
    }

    protected override NRedberry.Tensors.Tensor Create1(NRedberry.Tensors.Tensor tensor)
    {
        return new TestScalarFunction(tensor);
    }
}
