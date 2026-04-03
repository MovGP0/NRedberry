using NRedberry.Tensors;
using TensorFactory = NRedberry.Tensors.Tensors;

namespace NRedberry.Core.Tests.Tensors;

public sealed class TensorExceptionTests
{
    [Fact]
    public void ShouldIncludeSeedWhenNoTensorsAreProvided()
    {
        TensorException exception = new("failure", 1234);

        exception.ShouldSatisfyAllConditions(
            e => e.Message.ShouldContain("failure"),
            e => e.Message.ShouldContain("nmseed: 1234"),
            e => e.Tensors.ShouldBeEmpty());
    }

    [Fact]
    public void ShouldIncludeTensorTextAndStoreTensors()
    {
        NRedberry.Tensors.Tensor tensor = TensorFactory.Parse("a+b");
        string tensorText = tensor.ToString(OutputFormat.Redberry);
        TensorException exception = new("failure", 77, tensor);

        exception.ShouldSatisfyAllConditions(
            e => e.Message.ShouldContain("\"failure\" in tensors"),
            e => e.Message.ShouldContain(tensorText),
            e => e.Message.ShouldContain("nmseed: 77"),
            e => e.Tensors.ShouldHaveSingleItem(),
            e => e.Tensors[0].ShouldBeSameAs(tensor));
    }

    [Fact]
    public void ShouldPreserveInnerException()
    {
        InvalidOperationException inner = new("boom");
        TensorException exception = new("failure", inner);

        exception.ShouldSatisfyAllConditions(
            e => e.InnerException.ShouldBeSameAs(inner),
            e => e.Message.ShouldContain("failure"));
    }
}
