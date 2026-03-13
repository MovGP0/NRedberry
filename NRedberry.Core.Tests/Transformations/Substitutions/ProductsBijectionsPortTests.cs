using NRedberry.Transformations.Substitutions;
using TensorApi = NRedberry.Tensors.Tensors;
using Xunit;

namespace NRedberry.Core.Tests.Transformations.Substitutions;

public sealed class ProductsBijectionsPortTests
{
    [Fact]
    public void ShouldThrowWhileProductsBijectionsPortIsUnimplemented()
    {
        NRedberry.Tensors.Product from = Assert.IsType<NRedberry.Tensors.Product>(TensorApi.Parse("a*b"));
        NRedberry.Tensors.Product to = Assert.IsType<NRedberry.Tensors.Product>(TensorApi.Parse("a*b"));

        Assert.Throws<NotImplementedException>(() => new ProductsBijectionsPort(from.Content, to.Content));
    }
}
