using NRedberry.Core.Indices;
using NRedberry.Core.Tensors;

namespace NRedberry.Core.Tensors.Playground;

/*
 * Original: ./core/src/main/java/cc/redberry/core/tensor/playground/ProductData.java
 */

public sealed class ProductData
{
    public ProductData(Tensor[] data, Indices.Indices indices)
    {
        Data = data;
        Indices = indices;
    }

    public Tensor[] Data { get; }

    public Indices.Indices Indices { get; }
}
