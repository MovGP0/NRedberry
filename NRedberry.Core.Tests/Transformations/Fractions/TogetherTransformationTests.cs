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

        actual.ToString(OutputFormat.Redberry).ShouldBe(tensor.ToString(OutputFormat.Redberry));
    }

    [Fact]
    public void ShouldUseIdentityFactorWhenOptionsAreEmpty()
    {
        TogetherTransformation transformation = new(new TogetherTransformation.TogetherOptions());

        transformation.ToString(OutputFormat.Redberry).ShouldBe("Together");
    }
}
