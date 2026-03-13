using NRedberry.Groovy;
using NRedberry.Tensors;
using Xunit;
using TensorApi = NRedberry.Tensors.Tensors;

namespace NRedberry.Groovy.Tests;

public sealed class DSLTransformationInstTests
{
    [Fact]
    public void ShouldDelegateTransformAndFormattingToSuppliedInstance()
    {
        TestTransformation transformation = new();
        DSLTransformationInst<TestTransformation> instance = new(transformation);
        Tensor tensor = TensorApi.Parse("a+b");

        Tensor transformed = instance.Transform(tensor);

        Assert.Same(tensor, transformed);
        Assert.Equal("test-transformation", instance.ToString());
    }
}
