namespace NRedberry.Tensors.Iterators;

/*
 * Original: ./core/src/main/java/cc/redberry/core/tensor/iterator/PayloadFactory.java
 */

public interface PayloadFactory<T> where T : Payload<T>
{
    bool AllowLazyInitialization();

    T Create(StackPosition<T> stackPosition);
}
