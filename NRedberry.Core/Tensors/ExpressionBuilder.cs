using System;

namespace NRedberry.Core.Tensors;

/*
 * Original: ./core/src/main/java/cc/redberry/core/tensor/ExpressionBuilder.java
 */

public sealed class ExpressionBuilder : TensorBuilder
{
    public ExpressionBuilder()
    {
    }

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
