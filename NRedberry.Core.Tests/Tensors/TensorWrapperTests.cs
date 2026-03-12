using NRedberry.Tensors;
using TensorApi = NRedberry.Tensors.Tensors;
using Xunit;

namespace NRedberry.Core.Tests.Tensors;

public sealed class TensorWrapperTests
{
    [Fact]
    public void ShouldWrapSingleTensorAndExposeWrapperInfrastructure()
    {
        NRedberry.Tensors.Tensor inner = TensorApi.Parse("a");
        TensorWrapper wrapper = TensorWrapper.Wrap(inner);

        Assert.Same(inner, wrapper[0]);
        Assert.Equal(1, wrapper.Size);
        Assert.Equal("@[a]", wrapper.ToString(OutputFormat.Redberry));
        Assert.Equal(-1, wrapper.GetHashCode());
        Assert.Equal("TensorWrapperBuilder", wrapper.GetBuilder().GetType().Name);
        Assert.Equal("TensorWrapperFactory", wrapper.GetFactory()!.GetType().Name);
        Assert.Throws<IndexOutOfRangeException>(() => _ = wrapper[1]);
        Assert.Throws<NotSupportedException>(() => _ = wrapper.Indices);
    }

    [Fact]
    public void ShouldCompareWrappedTensors()
    {
        TensorWrapper left = TensorWrapper.Wrap(TensorApi.Parse("a"));
        TensorWrapper right = TensorWrapper.Wrap(TensorApi.Parse("a"));

        Assert.Equal(0, left.CompareTo(right));
        Assert.True(left.CompareTo(null) < 0);
    }
}
