using NRedberry.Core.Indices;
using NRedberry.Core.Maths;
using NRedberry.Core.Numbers;

namespace NRedberry.Core.Tensors;

public abstract class MultiTensor : Tensor
{
    public override IIndices Indices { get; }

    protected MultiTensor(IIndices indices)
    {
        Indices = indices;
    }

    public abstract Tensor Remove(uint position);

    public Tensor Remove(uint[] positions)
    {
        var p = positions.GetSortedDistinct();
        Tensor temp = this;
        for(var i = p.Length -1; i >= 0; --i)
        {
            if (temp is MultiTensor mt)
            {
                temp = mt.Remove(p[i]);
            }
            else
            {
                temp = GetNeutral();
            }
        }

        return temp;
    }

    protected abstract Complex GetNeutral();
    protected abstract Tensor Select1(uint[] positions);
}