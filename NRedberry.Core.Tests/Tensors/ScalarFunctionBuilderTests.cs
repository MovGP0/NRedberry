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

        Assert.Equal("TestFn[a]", builder.Build().ToString(OutputFormat.Redberry));
        Assert.Equal("TestFn[a]", clone.Build().ToString(OutputFormat.Redberry));
    }

    [Fact]
    public void ShouldGuardInvalidBuilderUsage()
    {
        ScalarFunctionBuilder builder = new(TestScalarFunctionFactory.Instance);

        Assert.Throws<InvalidOperationException>(() => builder.Build());
        Assert.Throws<ArgumentException>(() => builder.Put(TensorApi.Parse("f_m")));

        builder.Put(TensorApi.Parse("a"));

        Assert.Throws<InvalidOperationException>(() => builder.Put(TensorApi.Parse("b")));
    }
}
