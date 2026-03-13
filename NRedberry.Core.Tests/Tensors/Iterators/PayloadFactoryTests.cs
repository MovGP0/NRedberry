using NRedberry.Tensors;
using NRedberry.Tensors.Iterators;
using TensorApi = NRedberry.Tensors.Tensors;
using TensorType = NRedberry.Tensors.Tensor;
using Xunit;

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
        Assert.Equal(TraverseState.Entering, lazyIterator.Next());
        StackPosition<TrackingPayload> lazyStackPosition = lazyIterator.CurrentStackPosition();
        Assert.False(lazyStackPosition.IsPayloadInitialized());
        Assert.Equal("a+b", lazyStackPosition.GetPayload().CapturedTensor);
        Assert.True(lazyStackPosition.IsPayloadInitialized());

        TreeTraverseIterator<TrackingPayload> eagerIterator = new(
            tensor,
            TraverseGuide.All,
            new TrackingPayloadFactory(allowLazyInitialization: false));
        Assert.Equal(TraverseState.Entering, eagerIterator.Next());
        StackPosition<TrackingPayload> eagerStackPosition = eagerIterator.CurrentStackPosition();
        Assert.True(eagerStackPosition.IsPayloadInitialized());
        Assert.Equal("a+b", eagerStackPosition.GetPayload().CapturedTensor);
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
