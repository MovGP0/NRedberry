namespace NRedberry.Tensors.Playground;

/*
 * Original: ./core/src/main/java/cc/redberry/core/tensor/playground/ProductData.java
 */

public sealed class ProductData(Tensor[] data, Indices.Indices indices)
{
    public Tensor[] Data { get; } = data;

    public Indices.Indices Indices { get; } = indices;
}
