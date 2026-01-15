namespace NRedberry.Tensors.Iterators;

public abstract class TreeIteratorAbstract : ITreeIterator
{
    private TreeTraverseIterator _iterator { get; }
    private TraverseState _state { get; }

    protected TreeIteratorAbstract(Tensor tensor, TraverseGuide guide, TraverseState state)
    {
        _iterator = new TreeTraverseIterator(tensor, guide);
        _state = state;
    }

    protected TreeIteratorAbstract(Tensor tensor, TraverseState state)
    {
        _iterator = new TreeTraverseIterator(tensor);
        _state = state;
    }

    public Tensor Next()
    {
        TraverseState? nextState;
        while ((nextState = _iterator.Next()) != _state && nextState != null)
        {
        }

        return nextState == null ? null : _iterator.Current();
    }

    public void Set(Tensor tensor)
    {
        _iterator.Set(tensor);
    }

    public Tensor Result()
    {
        return _iterator.Result();
    }

    public int Depth => _iterator.Depth;
}
