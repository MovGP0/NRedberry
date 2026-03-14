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

        factory.Create(argument).ShouldBeOfType<TestScalarFunction>();
        Should.Throw<ArgumentException>(() => factory.Create());
        Should.Throw<ArgumentException>(() => factory.Create(argument, argument));
        Should.Throw<ArgumentException>(() => factory.Create(TensorApi.Parse("f_m")));
        Should.Throw<ArgumentNullException>(() => factory.Create(null!));
    }
}
