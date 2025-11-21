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
        throw new NotImplementedException();
    }
}
