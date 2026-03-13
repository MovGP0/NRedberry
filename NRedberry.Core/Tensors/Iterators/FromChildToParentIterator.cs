namespace NRedberry.Tensors.Iterators;

public sealed class FromChildToParentIterator : TreeIteratorAbstract
{
    public FromChildToParentIterator(Tensor tensor, TraverseGuide guide)
        : base(tensor, guide, TraverseState.Leaving)
    {
    }

    public FromChildToParentIterator(Tensor tensor)
        : base(tensor, TraverseState.Leaving)
    {
    }
}

/// <summary>
/// An iterator for tensors that allows the programmer to traverse the tensor
/// tree structure, modify the tensor during iteration, and obtain information
/// about iterator current position in the tensor.
/// A <see cref="TensorTreeIterator"/> has current element,
/// so all methods are defined in terms of the cursor position.
/// </summary>
/// <example>
/// <code>
/// Tensor tensor = Tensors.Parse("Cos[a+b+Sin[x]]");
/// TensorIterator states = new TreeTraverseIterator(tensor);
/// TraverseState state;
/// foreach (var state in states)
///     Console.WriteLine(state + " " + state.Depth + " " + state.Current);
/// </code> This code will print: <code>
/// Entering   Cos[a + b + Sin[y * c + x]]
/// Entering   a+b+Sin[y * c + x]
/// Entering   a
/// Leaving   a
/// Entering   b
/// Leaving   b
/// Entering   Sin[y * c + x]
/// Entering   y* c+x
/// Entering   y* c
/// Entering   y
/// Leaving   y
/// Entering   c
/// Leaving   c
/// Leaving   y* c
/// Entering   x
/// Leaving   x
/// Leaving   y* c+x
/// Leaving   Sin[y * c + x]
/// Leaving   a+b+Sin[y * c + x]
/// Leaving   Cos[a + b + Sin[y * c + x]]
/// </code>
/// </example>
public sealed class TreeTraverseIterator(Tensor tensor, TraverseGuide? guide = null)
{
    private TreeTraverseIterator<NoPayload> Iterator { get; } = guide is null
        ? new TreeTraverseIterator<NoPayload>(tensor)
        : new TreeTraverseIterator<NoPayload>(tensor, guide);

    public int Depth => Iterator.Depth();

    public TraverseState? Next()
    {
        return Iterator.Next();
    }

    public Tensor Current()
    {
        return Iterator.Current();
    }

    public void Set(Tensor tensor1)
    {
        Iterator.Set(tensor1);
    }

    public Tensor Result()
    {
        return Iterator.Result();
    }

    private sealed class NoPayload : Payload<NoPayload>
    {
        public Tensor OnLeaving(StackPosition<NoPayload> stackPosition)
        {
            return null!;
        }
    }
}
