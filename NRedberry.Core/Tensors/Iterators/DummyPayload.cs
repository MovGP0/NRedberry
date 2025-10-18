using System;
using NRedberry.Core.Tensors;

namespace NRedberry.Core.Tensors.Iterators;

/*
 * Original: ./core/src/main/java/cc/redberry/core/tensor/iterator/DummyPayload.java
 */

public sealed class DummyPayload<T> : Payload<T> where T : Payload<T>
{
    public Tensor OnLeaving(StackPosition<T> stackPosition)
    {
        throw new NotImplementedException();
    }
}
