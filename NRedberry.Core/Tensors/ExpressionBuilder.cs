using NRedberry.Indices;

namespace NRedberry.Tensors;

/*
 * Original: ./core/src/main/java/cc/redberry/core/tensor/ExpressionBuilder.java
 */

public sealed class ExpressionBuilder : TensorBuilder
{
    private Tensor? _left;
    private Tensor? _right;
    private Indices.Indices? _indices;

    public ExpressionBuilder()
    {
    }

    private ExpressionBuilder(Tensor? left, Tensor? right, Indices.Indices? indices)
    {
        _left = left;
        _right = right;
        _indices = indices;
    }

    public Tensor Build()
    {
        if (_left is null || _right is null || _indices is null)
        {
            throw new InvalidOperationException("Expression is not fully constructed.");
        }

        return new Expression(_indices, _left, _right);
    }

    public void Put(Tensor tensor)
    {
        ArgumentNullException.ThrowIfNull(tensor);

        if (_left is null)
        {
            _left = tensor;
            _indices = IndicesFactory.Create(_left.Indices.GetFree());
        }
        else if (_right is null)
        {
            _right = tensor;
            if (!TensorUtils.IsZero(_right)
                && !TensorUtils.IsIndeterminate(_right)
                && _indices?.EqualsRegardlessOrder(_right.Indices.GetFree()) != true)
            {
                throw new TensorException("Inconsistent indices in expression.", tensor);
            }
        }
        else
        {
            throw new TensorException("Expression have only two parts.");
        }
    }

    public TensorBuilder Clone()
    {
        return new ExpressionBuilder(_left, _right, _indices);
    }
}
