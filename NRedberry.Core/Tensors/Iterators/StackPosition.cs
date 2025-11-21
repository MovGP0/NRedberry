using NRedberry.Core.Utils;

namespace NRedberry.Tensors.Iterators;

/*
 * Original: ./core/src/main/java/cc/redberry/core/tensor/iterator/StackPosition.java
 */

public interface StackPosition<T> where T : Payload<T>
{
    Tensor GetInitialTensor();

    Tensor GetTensor();

    bool IsModified();

    StackPosition<T> Previous();

    StackPosition<T> Previous(int level);

    T GetPayload();

    bool IsPayloadInitialized();

    int GetDepth();

    bool IsUnder(IIndicator<Tensor> indicator, int searchDepth);

    int CurrentIndex();
}
