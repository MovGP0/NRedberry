using System;

namespace NRedberry.Core.Tensors.Iterators;

public sealed class FromChildToParentIterator : TreeIteratorAbstract
{
    public FromChildToParentIterator(Tensor tensor, TraverseGuide guide) : base(tensor, guide, TraverseState.Leaving)
    {
    }

    public FromChildToParentIterator(Tensor tensor) : base(tensor, TraverseState.Leaving)
    {
    }
}

/// <summary>
/// An iterator for tensors that allows the programmer to traverse the tensor
/// tree structure, modify the tensor during iteration, and obtain information
/// about iterator current position in the tensor.A { @code TensorTreeIterator }
/// has current element, so all methods are defined in terms of the cursor
/// position. *<p>Example: <blockquote><pre>
///      Tensor tensor = Tensors.parse("Cos[a+b+Sin[x]]");
/// TensorIterator iterator = new TreeTraverseIterator(tensor);
/// TraverseState state;
///      while ((state = iterator.next()) != null)
/// System.out.println(state + " " + iterator.depth() + " " + iterator.current());
/// </pre></blockquote> This code will print: <blockquote><pre>
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
/// </pre></blockquote> </p>
/// </summary>
public sealed class TreeTraverseIterator
{
    private Tensor tensor;
    private TraverseGuide guide;

    public TreeTraverseIterator(Tensor tensor)
    {
        this.tensor = tensor;
    }

    public TreeTraverseIterator(Tensor tensor, TraverseGuide guide)
    {
        this.tensor = tensor;
        this.guide = guide;
    }

    public int Depth { get; set; }

    public TraverseState Next()
    {
        throw new NotImplementedException();
    }

    public Tensor Current()
    {
        throw new NotImplementedException();
    }

    public void Set(Tensor tensor1)
    {
        throw new NotImplementedException();
    }

    public Tensor Result()
    {
        throw new NotImplementedException();
    }
}