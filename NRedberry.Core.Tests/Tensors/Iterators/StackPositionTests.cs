using NRedberry.Core.Utils;
using NRedberry.Tensors.Iterators;
using TensorApi = NRedberry.Tensors.Tensors;

namespace NRedberry.Core.Tests.Tensors.Iterators;

public sealed class StackPositionTests
{
    [Fact]
    public void ShouldExposeCurrentTraversalContext()
    {
        var sum = TensorApi.Parse("a+b");
        string parentText = sum.ToString(OutputFormat.Redberry);
        string currentChild = sum[0].ToString(OutputFormat.Redberry);
        string siblingChild = sum[1].ToString(OutputFormat.Redberry);
        TreeTraverseIterator<TrackingPayload> iterator = new(
            sum,
            TraverseGuide.All,
            new TrackingPayloadFactory());

        iterator.Next().ShouldBe(TraverseState.Entering);
        iterator.Next().ShouldBe(TraverseState.Entering);

        StackPosition<TrackingPayload> stackPosition = iterator.CurrentStackPosition();

        stackPosition.ShouldSatisfyAllConditions(
            () => stackPosition.GetInitialTensor().ToString(OutputFormat.Redberry).ShouldBe(currentChild),
            () => stackPosition.GetTensor().ToString(OutputFormat.Redberry).ShouldBe(currentChild),
            () => stackPosition.IsModified().ShouldBeFalse(),
            () => stackPosition.Previous().GetTensor().ToString(OutputFormat.Redberry).ShouldBe(parentText),
            () => stackPosition.Previous(0).ShouldBeSameAs(stackPosition),
            () => stackPosition.Previous(1).GetTensor().ToString(OutputFormat.Redberry).ShouldBe(parentText),
            () => stackPosition.IsPayloadInitialized().ShouldBeFalse(),
            () => stackPosition.GetPayload().CapturedTensor.ShouldBe(currentChild),
            () => stackPosition.IsPayloadInitialized().ShouldBeTrue(),
            () => stackPosition.GetDepth().ShouldBe(1),
            () => stackPosition.IsUnder(new TensorTextIndicator(parentText), 1).ShouldBeTrue(),
            () => stackPosition.IsUnder(new TensorTextIndicator(siblingChild), 0).ShouldBeFalse(),
            () => stackPosition.CurrentIndex().ShouldBe(-1));
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
