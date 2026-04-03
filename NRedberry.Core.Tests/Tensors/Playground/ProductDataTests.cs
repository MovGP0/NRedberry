using NRedberry.Indices;
using NRedberry.Tensors.Playground;
using TensorApi = NRedberry.Tensors.Tensors;

namespace NRedberry.Core.Tests.Tensors.Playground;

public sealed class ProductDataTests
{
    [Fact]
    public void ShouldStoreConstructorArguments()
    {
        NRedberry.Tensors.Tensor[] data = [TensorApi.Parse("a")];
        NRedberry.Indices.Indices indices = IndicesFactory.EmptyIndices;

        ProductData productData = new(data, indices);

        productData.Data.ShouldBeSameAs(data);
        productData.Indices.ShouldBeSameAs(indices);
    }
}
