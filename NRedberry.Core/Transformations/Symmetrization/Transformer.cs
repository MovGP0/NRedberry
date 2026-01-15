using NRedberry.Tensors;
using NRedberry.Tensors.Iterators;

namespace NRedberry.Transformations.Symmetrization;

public class Transformer : ITransformation
{
    private readonly TraverseState _state;
    private readonly ITransformation[] _transformations;
    private readonly TraverseGuide _guide;

    public Transformer(TraverseState state, ITransformation[] transformations, TraverseGuide guide)
    {
        ArgumentNullException.ThrowIfNull(transformations);
        ArgumentNullException.ThrowIfNull(guide);

        _state = state;
        _transformations = transformations;
        _guide = guide;
    }

    public Transformer(TraverseState state, ITransformation[] transformations)
        : this(state, transformations, TraverseGuide.All)
    {
    }

    public Tensor Transform(Tensor tensor)
    {
        ArgumentNullException.ThrowIfNull(tensor);

        TreeTraverseIterator iterator = new(tensor, _guide);
        TraverseState? currentState;
        while ((currentState = iterator.Next()) is not null)
        {
            if (currentState != _state)
            {
                continue;
            }

            Tensor newTensor = iterator.Current();
            Tensor currentTensor = newTensor;

            foreach (ITransformation transformation in _transformations)
            {
                newTensor = transformation.Transform(newTensor);
            }

            if (!ReferenceEquals(currentTensor, newTensor))
            {
                iterator.Set(newTensor);
            }
        }

        return iterator.Result();
    }
}
