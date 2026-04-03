using NRedberry.Tensors;
using TensorApi = NRedberry.Tensors.Tensors;

namespace NRedberry.Core.Tests.Tensors;

public sealed class TensorWrapperTests
{
    [Fact]
    public void ShouldWrapSingleTensorAndExposeWrapperInfrastructure()
    {
        NRedberry.Tensors.Tensor inner = TensorApi.Parse("a");
        TensorWrapper wrapper = TensorWrapper.Wrap(inner);

        wrapper[0].ShouldBeSameAs(inner);
        wrapper.Size.ShouldBe(1);
        wrapper.ToString(OutputFormat.Redberry).ShouldBe("@[a]");
        wrapper.GetHashCode().ShouldBe(-1);
        wrapper.GetBuilder().GetType().Name.ShouldBe("TensorWrapperBuilder");
        wrapper.GetFactory()!.GetType().Name.ShouldBe("TensorWrapperFactory");
        Should.Throw<IndexOutOfRangeException>(() => _ = wrapper[1]);
        Should.Throw<NotSupportedException>(() => _ = wrapper.Indices);
    }

    [Fact]
    public void ShouldCompareWrappedTensors()
    {
        TensorWrapper left = TensorWrapper.Wrap(TensorApi.Parse("a"));
        TensorWrapper right = TensorWrapper.Wrap(TensorApi.Parse("a"));

        left.CompareTo(right).ShouldBe(0);
        (left.CompareTo(null) < 0).ShouldBeTrue();
    }
}
