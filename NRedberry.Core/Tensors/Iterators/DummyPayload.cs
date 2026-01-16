namespace NRedberry.Tensors.Iterators;

/*
 * Original: ./core/src/main/java/cc/redberry/core/tensor/iterator/DummyPayload.java
 */

public sealed class DummyPayload<T> : Payload<T> where T : Payload<T>
{
    public Tensor OnLeaving(StackPosition<T> stackPosition)
    {
        return null!;
    }
}
