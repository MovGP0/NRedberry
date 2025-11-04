using NRedberry.Core.Indices;

namespace NRedberry.Core.Tensors;

/// <summary>
/// Representation of mathematical power <i>A^B</i>.
/// </summary>
public sealed class Power : Tensor
{
    private Tensor argument { get; set; }
    public Tensor power { get; set; }

    public Power(Tensor a, Tensor power)
    {
        argument = a;
        this.power = power;
    }

    public override int GetHashCode()
    {
        throw new NotImplementedException();
    }

    public override Indices.Indices Indices => IndicesFactory.EmptyIndices;

    public override Tensor this[int i]
    {
        get
        {
            switch (i)
            {
                case 0:
                    return argument;

                case 1:
                    return power;

                default:
                    throw new ArgumentOutOfRangeException(nameof(i), i, "must be 0 or 1");
            }
        }
    }

    public override int Size { get; }

    public override string ToString(OutputFormat outputFormat)
    {
        throw new NotImplementedException();
    }

    public override TensorBuilder GetBuilder()
    {
        throw new NotImplementedException();
    }

    public override TensorFactory GetFactory()
    {
        throw new NotImplementedException();
    }
}
