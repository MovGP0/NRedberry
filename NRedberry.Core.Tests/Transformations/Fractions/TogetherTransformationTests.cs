using NRedberry.Transformations.Fractions;
using TensorApi = NRedberry.Tensors.Tensors;
using Xunit;

namespace NRedberry.Core.Tests.Transformations.Fractions;

public sealed class TogetherTransformationTests
{
    [Fact]
    public void ShouldLeaveSimpleSumUntouched()
    {
        NRedberry.Tensors.Tensor tensor = TensorApi.Parse("a+b");

        NRedberry.Tensors.Tensor actual = TogetherTransformation.Together(tensor);

        Assert.Equal(
            tensor.ToString(OutputFormat.Redberry),
            actual.ToString(OutputFormat.Redberry));
    }

    [Fact]
    public void ShouldUseIdentityFactorWhenOptionsAreEmpty()
    {
        TogetherTransformation transformation = new(new TogetherTransformation.TogetherOptions());

        Assert.Equal("Together", transformation.ToString(OutputFormat.Redberry));
    }
}
