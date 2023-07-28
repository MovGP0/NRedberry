using NRedberry.Core.Tensors;
using NRedberry.Core.Tensors.Iterators;

namespace NRedberry.Core.Transformations.Symmetrization;

public class Transformer : Transformation
{
    private readonly TraverseState state;
    private readonly ITransformation[] transformations;
    private readonly TraverseGuide guide;

    public Transformer(TraverseState state, ITransformation[] transformations, TraverseGuide guide)
    {
        this.state = state;
        this.transformations = transformations;
        this.guide = guide;
    }

    public Transformer(TraverseState state, ITransformation[] transformations)
        : this(state, transformations, TraverseGuide.All)
    {
    }

    public Tensor Transform(Tensor t)
    {
        var iterator = new TreeTraverseIterator(t, guide);
        TraverseState currentState;
        Tensor currentTensor, newTensor;
        while ((currentState = iterator.Next()) != null)
        {
            if (currentState != state)
                continue;
            currentTensor = newTensor = iterator.Current();

            foreach (ITransformation transformation in transformations)
                newTensor = transformation.Transform(newTensor);
            if (currentTensor != newTensor)
                iterator.Set(newTensor);
        }

        return iterator.Result();
    }
}