using NRedberry.Tensors;
using NRedberry.Transformations.Substitutions;
using Shouldly;
using TensorApi = NRedberry.Tensors.Tensors;
using Xunit;

namespace NRedberry.Core.Tests.Transformations.Substitutions;

public sealed class ProductsBijectionsPortTests
{
    [Fact]
    public void ShouldReturnIdentityBijectionForMatchingContractedProduct()
    {
        Product from = TensorApi.Parse("A_a*B^a").ShouldBeOfType<Product>();
        Product to = TensorApi.Parse("A_a*B^a").ShouldBeOfType<Product>();

        ProductsBijectionsPort port = new(from.Content, to.Content);

        List<string> bijections = [];
        int[]? bijection;
        while ((bijection = port.Take()) is not null)
        {
            bijections.Add(string.Join(",", (int[])bijection.Clone()));
        }

        bijections.ShouldBe(["0,1"]);
    }

    [Fact]
    public void ShouldReturnNullWhenNoCompatibleContractionTargetExists()
    {
        Product from = TensorApi.Parse("A_a*B^a").ShouldBeOfType<Product>();
        Product to = TensorApi.Parse("A_a*C^a").ShouldBeOfType<Product>();

        ProductsBijectionsPort port = new(from.Content, to.Content);

        port.Take().ShouldBeNull();
    }
}
