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

        Assert.Equal(TraverseState.Entering, iterator.Next());
        Assert.Equal(TraverseState.Entering, iterator.Next());

        StackPosition<TrackingPayload> stackPosition = iterator.CurrentStackPosition();

        Assert.Equal("a", stackPosition.GetInitialTensor().ToString(OutputFormat.Redberry));
        Assert.Equal("a", stackPosition.GetTensor().ToString(OutputFormat.Redberry));
        Assert.False(stackPosition.IsModified());
        Assert.Equal("a+b", stackPosition.Previous().GetTensor().ToString(OutputFormat.Redberry));
        Assert.Same(stackPosition, stackPosition.Previous(0));
        Assert.Equal("a+b", stackPosition.Previous(1).GetTensor().ToString(OutputFormat.Redberry));
        Assert.False(stackPosition.IsPayloadInitialized());
        Assert.Equal("a", stackPosition.GetPayload().CapturedTensor);
        Assert.True(stackPosition.IsPayloadInitialized());
        Assert.Equal(1, stackPosition.GetDepth());
        Assert.True(stackPosition.IsUnder(new TensorTextIndicator("a+b"), 1));
        Assert.False(stackPosition.IsUnder(new TensorTextIndicator("b"), 0));
        Assert.Equal(-1, stackPosition.CurrentIndex());
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
