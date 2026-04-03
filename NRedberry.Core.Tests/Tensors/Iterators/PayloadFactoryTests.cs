using NRedberry.Tensors.Iterators;
using TensorApi = NRedberry.Tensors.Tensors;
using TensorType = NRedberry.Tensors.Tensor;

namespace NRedberry.Core.Tests.Tensors.Iterators;

public sealed class PayloadFactoryTests
{
    [Fact]
    public void ShouldRespectLazyInitializationSetting()
    {
        TensorType tensor = TensorApi.Parse("a+b");

        TreeTraverseIterator<TrackingPayload> lazyIterator = new(
            tensor,
            TraverseGuide.All,
            new TrackingPayloadFactory(allowLazyInitialization: true));
        lazyIterator.Next().ShouldBe(TraverseState.Entering);
        StackPosition<TrackingPayload> lazyStackPosition = lazyIterator.CurrentStackPosition();
        lazyStackPosition.IsPayloadInitialized().ShouldBeFalse();
        lazyStackPosition.GetPayload().CapturedTensor.ShouldBe("a+b");
        lazyStackPosition.IsPayloadInitialized().ShouldBeTrue();

        TreeTraverseIterator<TrackingPayload> eagerIterator = new(
            tensor,
            TraverseGuide.All,
            new TrackingPayloadFactory(allowLazyInitialization: false));
        eagerIterator.Next().ShouldBe(TraverseState.Entering);
        StackPosition<TrackingPayload> eagerStackPosition = eagerIterator.CurrentStackPosition();
        eagerStackPosition.IsPayloadInitialized().ShouldBeTrue();
        eagerStackPosition.GetPayload().CapturedTensor.ShouldBe("a+b");
    }

    private sealed class TrackingPayload(string capturedTensor) : Payload<TrackingPayload>
    {
        public string CapturedTensor { get; } = capturedTensor;

        public NRedberry.Tensors.Tensor OnLeaving(StackPosition<TrackingPayload> stackPosition)
        {
            return null!;
        }
    }

    private sealed class TrackingPayloadFactory(bool allowLazyInitialization) : PayloadFactory<TrackingPayload>
    {
        public bool AllowLazyInitialization()
        {
            return allowLazyInitialization;
        }

        public TrackingPayload Create(StackPosition<TrackingPayload> stackPosition)
        {
            return new TrackingPayload(stackPosition.GetTensor().ToString(OutputFormat.Redberry));
        }
    }
}
