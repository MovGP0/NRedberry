using NRedberry.Transformations.Substitutions;
using TensorApi = NRedberry.Tensors.Tensors;
using Xunit;

namespace NRedberry.Core.Tests.Transformations.Substitutions;

public sealed class ProductsBijectionsPortTests
{
    [Fact]
    public void ShouldThrowWhileProductsBijectionsPortIsUnimplemented()
    {
        NRedberry.Tensors.Product from = TensorApi.Parse("a*b").ShouldBeOfType<NRedberry.Tensors.Product>();
        NRedberry.Tensors.Product to = TensorApi.Parse("a*b").ShouldBeOfType<NRedberry.Tensors.Product>();

        Should.Throw<NotImplementedException>(() => new ProductsBijectionsPort(from.Content, to.Content));
    }
}
