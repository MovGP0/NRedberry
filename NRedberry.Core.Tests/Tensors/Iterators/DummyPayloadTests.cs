using NRedberry.Tensors.Iterators;

namespace NRedberry.Core.Tests.Tensors.Iterators;

public sealed class DummyPayloadTests
{
    [Fact]
    public void ShouldReturnNullOnLeaving()
    {
        DummyPayload<TestPayload> payload = new();
        TestStackPosition stackPosition = new();

        NRedberry.Tensors.Tensor result = payload.OnLeaving(stackPosition);

        result.ShouldBeNull();
    }

    private sealed class TestPayload : Payload<TestPayload>
    {
        public NRedberry.Tensors.Tensor OnLeaving(StackPosition<TestPayload> stackPosition)
        {
            return stackPosition.GetTensor();
        }
    }

    private sealed class TestStackPosition : StackPosition<TestPayload>
    {
        public NRedberry.Tensors.Tensor GetInitialTensor()
        {
            return NRedberry.Numbers.Complex.One;
        }

        public NRedberry.Tensors.Tensor GetTensor()
        {
            return NRedberry.Numbers.Complex.One;
        }

        public bool IsModified()
        {
            return false;
        }

        public StackPosition<TestPayload> Previous()
        {
            return null!;
        }

        public StackPosition<TestPayload> Previous(int level)
        {
            return null!;
        }

        public TestPayload GetPayload()
        {
            return new TestPayload();
        }

        public bool IsPayloadInitialized()
        {
            return true;
        }

        public int GetDepth()
        {
            return 0;
        }

        public bool IsUnder(NRedberry.Core.Utils.IIndicator<NRedberry.Tensors.Tensor> indicator, int searchDepth)
        {
            return false;
        }

        public int CurrentIndex()
        {
            return 0;
        }
    }
}
