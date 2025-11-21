using NRedberry.Tensors;
using NRedberry.Tensors.Iterators;

namespace NRedberry.Transformations.Symmetrization;

public class Transformer(TraverseState state, ITransformation[] transformations, TraverseGuide guide)
    : ITransformation
{
    public Transformer(TraverseState state, ITransformation[] transformations)
        : this(state, transformations, TraverseGuide.All)
    {
    }

    public Tensor Transform(Tensor t)
    {
        var iterator = new TreeTraverseIterator(t, guide);
        TraverseState? currentState;
        while ((currentState = iterator.Next()) != null)
        {
            if (currentState != state)
                continue;

            Tensor newTensor = iterator.Current();
            var currentTensor = newTensor;

            foreach (ITransformation transformation in transformations)
                newTensor = transformation.Transform(newTensor);

            if (currentTensor != newTensor)
                iterator.Set(newTensor);
        }

        return iterator.Result();
    }
}
