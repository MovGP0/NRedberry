using NRedberry.Tensors;
using NRedberry.Transformations.Substitutions;
using Shouldly;
using TensorApi = NRedberry.Tensors.Tensors;
using Xunit;

namespace NRedberry.Core.Tests.Transformations.Substitutions;

public sealed class ProductsBijectionsPortTests
{
    [Fact(Skip = "Runtime parity for ProductsBijectionsPort is still under investigation.")]
    public void ShouldConstructProductsBijectionsPort()
    {
        Product from = TensorApi.Parse("A_a*B^a").ShouldBeOfType<Product>();
        Product to = TensorApi.Parse("A_a*B^a").ShouldBeOfType<Product>();

        ProductsBijectionsPort port = new(from.Content, to.Content);

        port.Take().ShouldBeNull();
    }
}
