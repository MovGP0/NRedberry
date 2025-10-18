using NRedberry.Core.Tensors;

namespace NRedberry.Core.Tensors.Iterators;

/*
 * Original: ./core/src/main/java/cc/redberry/core/tensor/iterator/Payload.java
 */

public interface Payload<T> where T : Payload<T>
{
    Tensor OnLeaving(StackPosition<T> stackPosition);
}
