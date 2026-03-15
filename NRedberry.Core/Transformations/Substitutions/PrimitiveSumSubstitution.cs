using NRedberry.Tensors;

namespace NRedberry.Transformations.Substitutions;

/// <summary>
/// Port of cc.redberry.core.transformations.substitutions.PrimitiveSumSubstitution.
/// </summary>
public sealed class PrimitiveSumSubstitution : PrimitiveSubstitution
{
    public PrimitiveSumSubstitution(Tensor from, Tensor to)
        : base(from, to)
    {
    }

    protected override Tensor NewToCore(Tensor currentNode, SubstitutionIterator iterator)
    {
        Tensor current = currentNode;
        Tensor? old = null;
        while (!ReferenceEquals(old, current))
        {
            old = current;

            BijectionContainer? bijectionContainer = new SumBijectionPort(From, current).Take();
            if (bijectionContainer is null)
            {
                return current;
            }

            Tensor newTo = ApplyIndexMappingToTo(current, To, bijectionContainer.Mapping, iterator);

            SumBuilder builder = new();
            int[] bijection = bijectionContainer.Bijection;
            builder.Put(newTo);

            Array.Sort(bijection);
            int pivot = 0;
            for (int i = 0; i < current.Size; ++i)
            {
                if (pivot >= bijection.Length || i != bijection[pivot])
                {
                    builder.Put(current[i]);
                }
                else
                {
                    ++pivot;
                }
            }

            current = builder.Build();
        }

        return current;
    }
}
