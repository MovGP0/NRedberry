using NRedberry.Tensors;
using NRedberry.Tensors.Functions;
using TensorApi = NRedberry.Tensors.Tensors;
using Xunit;

namespace NRedberry.Core.Tests.Tensors;

public sealed class ScalarFunctionBuilderTests
{
    [Fact]
    public void ShouldBuildAndCloneWithSingleScalarArgument()
    {
        ScalarFunctionBuilder builder = new(TestScalarFunctionFactory.Instance);
        builder.Put(TensorApi.Parse("a"));

        TensorBuilder clone = builder.Clone();

        builder.Build().ToString(OutputFormat.Redberry).ShouldBe("TestFn[a]");
        clone.Build().ToString(OutputFormat.Redberry).ShouldBe("TestFn[a]");
    }

    [Fact]
    public void ShouldGuardInvalidBuilderUsage()
    {
        ScalarFunctionBuilder builder = new(TestScalarFunctionFactory.Instance);

        Should.Throw<InvalidOperationException>(() => builder.Build());
        Should.Throw<ArgumentException>(() => builder.Put(TensorApi.Parse("f_m")));

        builder.Put(TensorApi.Parse("a"));

        Should.Throw<InvalidOperationException>(() => builder.Put(TensorApi.Parse("b")));
    }
}
