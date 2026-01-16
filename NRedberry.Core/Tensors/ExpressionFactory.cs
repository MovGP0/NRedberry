using NRedberry.Indices;

namespace NRedberry.Tensors;

/*
 * Original: ./core/src/main/java/cc/redberry/core/tensor/ExpressionFactory.java
 */

public sealed class ExpressionFactory : TensorFactory
{
    public static ExpressionFactory Instance { get; } = new();

    private ExpressionFactory()
    {
    }

    public Tensor Create(params Tensor[] tensor)
    {
        ArgumentNullException.ThrowIfNull(tensor);
        if (tensor.Length != 2)
        {
            throw new ArgumentException("Wrong number of arguments.");
        }

        ArgumentNullException.ThrowIfNull(tensor[0]);
        ArgumentNullException.ThrowIfNull(tensor[1]);

        if (!TensorUtils.IsZero(tensor[1])
            && !TensorUtils.IsIndeterminate(tensor[1])
            && !tensor[0].Indices.GetFree().EqualsRegardlessOrder(tensor[1].Indices.GetFree()))
        {
            throw new TensorException(
                $"Inconsistent indices in expression: {tensor[0].Indices.GetFree()} != {tensor[1].Indices.GetFree()}");
        }

        return new Expression(IndicesFactory.Create(tensor[0].Indices.GetFree()), tensor[0], tensor[1]);
    }
}
