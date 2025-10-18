using NRedberry.Core.Transformations.Symmetrization;

namespace NRedberry.Core.Tensors;

/*
 * Original: ./core/src/main/java/cc/redberry/core/tensor/Expression.java
 */

public class Expression : Tensor, ITransformation
{
    public Expression(Indices.Indices indices, Tensor left, Tensor right)
    {
        throw new NotImplementedException();
    }

    public override Indices.Indices Indices
    {
        get { throw new NotImplementedException(); }
    }

    public override Tensor this[int i]
    {
        get { throw new NotImplementedException(); }
    }

    public override int Size
    {
        get { throw new NotImplementedException(); }
    }

    public override string ToString(OutputFormat outputFormat)
    {
        throw new NotImplementedException();
    }

    public override TensorBuilder GetBuilder()
    {
        throw new NotImplementedException();
    }

    public override TensorFactory? GetFactory()
    {
        throw new NotImplementedException();
    }

    public Tensor Transform(Tensor t)
    {
        throw new NotImplementedException();
    }

    public bool IsIdentity()
    {
        throw new NotImplementedException();
    }

    public Expression Transpose()
    {
        throw new NotImplementedException();
    }
}
