using NRedberry.Transformations.Symmetrization;
using TensorApi = NRedberry.Tensors.Tensors;

namespace NRedberry.Core.Tests.Transformations.Symmetrization;

public sealed class DifferentiateTransformationTests
{
    [Fact]
    public void ShouldConstructAndExposeName()
    {
        NRedberry.Tensors.SimpleTensor x = TensorApi.Parse("x").ShouldBeOfType<NRedberry.Tensors.SimpleTensor>();

        DifferentiateTransformation transformation = new(x);

        transformation.ToString(OutputFormat.Redberry).ShouldBe("Differentiate[x]");
    }

    [Fact]
    public void ShouldDifferentiateSimpleSymbol()
    {
        NRedberry.Tensors.SimpleTensor x = TensorApi.Parse("x").ShouldBeOfType<NRedberry.Tensors.SimpleTensor>();

        NRedberry.Tensors.Tensor differentiated = DifferentiateTransformation.Differentiate(TensorApi.Parse("x"), x, 1);

        differentiated.ToString(OutputFormat.Redberry).ShouldBe("1");
    }
}
