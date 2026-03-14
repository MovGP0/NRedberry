using NRedberry.Tensors;
using TensorFactory = NRedberry.Tensors.Tensors;
using Xunit;

namespace NRedberry.Core.Tests.Tensors;

public sealed class SimpleTensorBuilderTests
{
    [Fact]
    public void ShouldReturnWrappedTensorFromBuild()
    {
        SimpleTensor tensor = TensorFactory.Parse("T_a").ShouldBeOfType<SimpleTensor>();
        SimpleTensorBuilder builder = new(tensor);

        builder.Build().ShouldBeSameAs(tensor);
    }

    [Fact]
    public void ShouldRejectPutOperations()
    {
        SimpleTensor tensor = TensorFactory.Parse("T_a").ShouldBeOfType<SimpleTensor>();
        SimpleTensorBuilder builder = new(tensor);

        Should.Throw<NotSupportedException>(() => builder.Put(TensorFactory.Parse("a")));
    }

    [Fact]
    public void ShouldReturnSameInstanceFromClone()
    {
        SimpleTensor tensor = TensorFactory.Parse("T_a").ShouldBeOfType<SimpleTensor>();
        SimpleTensorBuilder builder = new(tensor);

        builder.Clone().ShouldBeSameAs(builder);
    }
}
