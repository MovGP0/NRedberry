using NRedberry.Indices;
using NRedberry.Transformations.Substitutions;
using NRedberry.Transformations.Symmetrization;

namespace NRedberry.Tensors;

/*
 * Original: ./core/src/main/java/cc/redberry/core/tensor/Expression.java
 */

public class Expression : Tensor, ITransformation
{
    private readonly Tensor _right;
    private readonly Tensor _left;
    private readonly Indices.Indices _indices;

    public Expression(Indices.Indices indices, Tensor left, Tensor right)
    {
        ArgumentNullException.ThrowIfNull(indices);
        ArgumentNullException.ThrowIfNull(left);
        ArgumentNullException.ThrowIfNull(right);

        _indices = indices;
        _right = right;
        _left = left;
    }

    public override int GetHashCode()
    {
        unchecked
        {
            return (3 * _left.GetHashCode()) - (7 * _right.GetHashCode());
        }
    }

    public override Indices.Indices Indices => _indices;

    public override Tensor this[int i]
    {
        get
        {
            return i switch
            {
                0 => _left,
                1 => _right,
                _ => throw new ArgumentOutOfRangeException(nameof(i), i, "must be 0 or 1")
            };
        }
    }

    public override int Size => 2;

    public override string ToString(OutputFormat outputFormat)
    {
        string eq = outputFormat.Is(OutputFormat.Maple) ? " := " : " = ";
        return _left.ToString(outputFormat) + eq + _right.ToString(outputFormat);
    }

    public override TensorBuilder GetBuilder()
    {
        return new ExpressionBuilder();
    }

    public override TensorFactory? GetFactory()
    {
        return ExpressionFactory.Instance;
    }

    public Tensor Transform(Tensor t)
    {
        return new SubstitutionTransformation(this).Transform(t);
    }

    public bool IsIdentity()
    {
        return TensorUtils.Equals(_left, _right);
    }

    public Expression Transpose()
    {
        return new Expression(_indices, _right, _left);
    }
}
