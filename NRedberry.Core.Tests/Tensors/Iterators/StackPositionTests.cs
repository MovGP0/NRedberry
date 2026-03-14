using NRedberry.Core.Utils;
using NRedberry.Tensors;
using NRedberry.Tensors.Iterators;
using TensorApi = NRedberry.Tensors.Tensors;
using Xunit;

namespace NRedberry.Core.Tests.Tensors.Iterators;

public sealed class StackPositionTests
{
    [Fact]
    public void ShouldExposeCurrentTraversalContext()
    {
        TreeTraverseIterator<TrackingPayload> iterator = new(
            TensorApi.Parse("a+b"),
            TraverseGuide.All,
            new TrackingPayloadFactory());

        iterator.Next().ShouldBe(TraverseState.Entering);
        iterator.Next().ShouldBe(TraverseState.Entering);

        StackPosition<TrackingPayload> stackPosition = iterator.CurrentStackPosition();

        stackPosition.GetInitialTensor().ToString(OutputFormat.Redberry).ShouldBe("a");
        stackPosition.GetTensor().ToString(OutputFormat.Redberry).ShouldBe("a");
        stackPosition.IsModified().ShouldBeFalse();
        stackPosition.Previous().GetTensor().ToString(OutputFormat.Redberry).ShouldBe("a+b");
        stackPosition.Previous(0).ShouldBeSameAs(stackPosition);
        stackPosition.Previous(1).GetTensor().ToString(OutputFormat.Redberry).ShouldBe("a+b");
        stackPosition.IsPayloadInitialized().ShouldBeFalse();
        stackPosition.GetPayload().CapturedTensor.ShouldBe("a");
        stackPosition.IsPayloadInitialized().ShouldBeTrue();
        stackPosition.GetDepth().ShouldBe(1);
        stackPosition.IsUnder(new TensorTextIndicator("a+b"), 1).ShouldBeTrue();
        stackPosition.IsUnder(new TensorTextIndicator("b"), 0).ShouldBeFalse();
        stackPosition.CurrentIndex().ShouldBe(-1);
    }

    private sealed class TrackingPayload(string capturedTensor) : Payload<TrackingPayload>
    {
        public string CapturedTensor { get; } = capturedTensor;

        public NRedberry.Tensors.Tensor OnLeaving(StackPosition<TrackingPayload> stackPosition)
        {
            return null!;
        }
    }

    private sealed class TrackingPayloadFactory : PayloadFactory<TrackingPayload>
    {
        public bool AllowLazyInitialization()
        {
            return true;
        }

        public TrackingPayload Create(StackPosition<TrackingPayload> stackPosition)
        {
            return new TrackingPayload(stackPosition.GetTensor().ToString(OutputFormat.Redberry));
        }
    }

    private sealed class TensorTextIndicator(string expectedText) : IIndicator<NRedberry.Tensors.Tensor>
    {
        public bool Is(NRedberry.Tensors.Tensor @object)
        {
            return @object.ToString(OutputFormat.Redberry) == expectedText;
        }
    }
}
