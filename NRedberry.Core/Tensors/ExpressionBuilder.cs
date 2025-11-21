namespace NRedberry.Tensors;

/*
 * Original: ./core/src/main/java/cc/redberry/core/tensor/ExpressionBuilder.java
 */

public sealed class ExpressionBuilder : TensorBuilder
{
    public Tensor Build()
    {
        throw new NotImplementedException();
    }

    public void Put(Tensor tensor)
    {
        throw new NotImplementedException();
    }

    public TensorBuilder Clone()
    {
        throw new NotImplementedException();
    }
}
