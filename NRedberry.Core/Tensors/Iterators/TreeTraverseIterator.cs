using System;
using NRedberry.Core.Tensors;
using NRedberry.Core.Utils;

namespace NRedberry.Core.Tensors.Iterators;

/*
 * Original: ./core/src/main/java/cc/redberry/core/tensor/iterator/TreeTraverseIterator.java
 */

public sealed class TreeTraverseIterator<T> where T : Payload<T>
{
    public TreeTraverseIterator(Tensor tensor, TraverseGuide guide, PayloadFactory<T> payloadFactory)
    {
        throw new NotImplementedException();
    }

    public TreeTraverseIterator(Tensor tensor, TraverseGuide guide)
    {
        throw new NotImplementedException();
    }

    public TreeTraverseIterator(Tensor tensor)
    {
        throw new NotImplementedException();
    }

    public TreeTraverseIterator(Tensor tensor, PayloadFactory<T> payloadFactory)
    {
        throw new NotImplementedException();
    }

    public TraverseState Next()
    {
        throw new NotImplementedException();
    }

    public void Set(Tensor tensor)
    {
        throw new NotImplementedException();
    }

    public int Depth()
    {
        throw new NotImplementedException();
    }

    public bool IsUnder(IIndicator<Tensor> indicator, int searchDepth)
    {
        throw new NotImplementedException();
    }

    public Tensor Current()
    {
        throw new NotImplementedException();
    }

    public Tensor Result()
    {
        throw new NotImplementedException();
    }

    public StackPosition<T> CurrentStackPosition()
    {
        throw new NotImplementedException();
    }
}
