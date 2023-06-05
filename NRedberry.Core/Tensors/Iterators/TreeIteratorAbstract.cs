namespace NRedberry.Core.Tensors.Iterators;

public abstract class TreeIteratorAbstract : ITreeIterator
{
    private TreeTraverseIterator Iterator { get; }
    private TraverseState State { get; }

    protected TreeIteratorAbstract(Tensor tensor, TraverseGuide guide, TraverseState state)
    {
        Iterator = new TreeTraverseIterator(tensor, guide);
        State = state;
    }

    protected TreeIteratorAbstract(Tensor tensor, TraverseState state)
    {
        Iterator = new TreeTraverseIterator(tensor);
        State = state;
    }

    public Tensor Next()
    {
        while (Iterator.Next() != State)
        {
        }
        return Iterator.Current();
    }

    public void Set(Tensor tensor)
    {
        Iterator.Set(tensor);
    }

    public Tensor Result()
    {
        return Iterator.Result();
    }

    public int Depth => Iterator.Depth;
}