using NRedberry.Tensors;
using NRedberry.Tensors.Functions;
using TensorApi = NRedberry.Tensors.Tensors;

namespace NRedberry.Core.Tests.Tensors;

public sealed class ScalarFunctionTests
{
    [Fact]
    public void ShouldExposeScalarArgumentAndFormatAcrossOutputs()
    {
        NRedberry.Tensors.Tensor argument = TensorApi.Parse("a");
        TestScalarFunction function = new(argument);

        function.Indices.Size().ShouldBe(0);
        function[0].ShouldBeSameAs(argument);
        function.Size.ShouldBe(1);
        function.ToString(OutputFormat.Redberry).ShouldBe("TestFn[a]");
        function.ToString(OutputFormat.LaTeX).ShouldBe("\\testfn(a)");
        function.ToString(OutputFormat.UTF8).ShouldBe("TestFn(a)");
    }

    [Fact]
    public void ShouldRejectNonScalarArguments()
    {
        Should.Throw<TensorException>(() => new TestScalarFunction(TensorApi.Parse("f_m")));
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
