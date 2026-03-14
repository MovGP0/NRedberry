using NRedberry.Numbers;
using NRedberry.Tensors;
using NRedberry.Tensors.Iterators;
using TensorApi = NRedberry.Tensors.Tensors;
using TensorType = NRedberry.Tensors.Tensor;
using Xunit;

namespace NRedberry.Core.Tests.Tensors.Iterators;

public sealed class PayloadTests
{
    [Fact]
    public void ShouldAllowPayloadToReplaceVisitedTensorOnLeaving()
    {
        TreeTraverseIterator<ReplacingPayload> iterator = new(
            TensorApi.Parse("a+b"),
            TraverseGuide.All,
            new ReplacingPayloadFactory());

        while (iterator.Next() is not null)
        {
        }

        TensorType result = iterator.Result();

        result.ToString(OutputFormat.Redberry).ShouldBe("b");
    }

    private sealed class ReplacingPayload : Payload<ReplacingPayload>
    {
        public NRedberry.Tensors.Tensor OnLeaving(StackPosition<ReplacingPayload> stackPosition)
        {
            return stackPosition.GetTensor().ToString(OutputFormat.Redberry) == "a"
                ? Complex.Zero
                : null!;
        }
    }

    private sealed class ReplacingPayloadFactory : PayloadFactory<ReplacingPayload>
    {
        public bool AllowLazyInitialization()
        {
            return false;
        }

        public ReplacingPayload Create(StackPosition<ReplacingPayload> stackPosition)
        {
            return new ReplacingPayload();
        }
    }
}
