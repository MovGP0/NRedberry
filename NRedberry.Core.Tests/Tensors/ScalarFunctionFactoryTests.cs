using NRedberry.Tensors;
using NRedberry.Tensors.Functions;
using TensorApi = NRedberry.Tensors.Tensors;
using Xunit;

namespace NRedberry.Core.Tests.Tensors;

public sealed class ScalarFunctionFactoryTests
{
    [Fact]
    public void ShouldCreateScalarFunctionsAndGuardInputs()
    {
        ScalarFunctionFactory factory = TestScalarFunctionFactory.Instance;
        NRedberry.Tensors.Tensor argument = TensorApi.Parse("a");

        Assert.IsType<TestScalarFunction>(factory.Create(argument));
        Assert.Throws<ArgumentException>(() => factory.Create());
        Assert.Throws<ArgumentException>(() => factory.Create(argument, argument));
        Assert.Throws<ArgumentException>(() => factory.Create(TensorApi.Parse("f_m")));
        Assert.Throws<ArgumentNullException>(() => factory.Create(null!));
    }
}
