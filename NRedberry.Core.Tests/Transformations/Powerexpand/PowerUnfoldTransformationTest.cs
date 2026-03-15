using NRedberry.Transformations.Powerexpand;
using Shouldly;
using TensorApi = NRedberry.Tensors.Tensors;
using Xunit;

namespace NRedberry.Core.Tests.Transformations.Powerexpand;

public sealed class PowerUnfoldTransformationTest
{
    [Fact(Skip = "Blocked by skeleton PowerUnfoldTransformation.Transform(), which still returns the input tensor unchanged.")]
    public void ShouldUnfoldSimplePowers()
    {
        PowerUnfoldTransformation transformation = PowerUnfoldTransformation.Instance;

        transformation.Transform(TensorApi.Parse("(a_m*b^m*c)**2"))
            .ToString()
            .ShouldBe("c**2*a_{m}*a_{a}*b^{m}*b^{a}");

        transformation.Transform(TensorApi.Parse("(a_m*a^m*c)**2"))
            .ToString()
            .ShouldBe("c**2*a_{m}*a_{a}*a^{m}*a^{a}");
    }

    [Fact(Skip = "Blocked by skeleton PowerUnfoldTransformation.Transform(), which still returns the input tensor unchanged.")]
    public void ShouldUnfoldWithVariablesOnly()
    {
        PowerUnfoldTransformation transformation = new([TensorApi.ParseSimple("A_m")]);

        transformation.Transform(TensorApi.Parse("(A_m*A^m)**2"))
            .ToString()
            .ShouldBe("A_{m}*A_{a}*A^{m}*A^{a}");
    }

    [Fact(Skip = "Blocked by skeleton PowerUnfoldTransformation.Transform(), which still returns the input tensor unchanged.")]
    public void ShouldUnfoldWithVariablesAndScalar()
    {
        PowerUnfoldTransformation transformation = new([TensorApi.ParseSimple("A_m")]);

        transformation.Transform(TensorApi.Parse("(A_m*A^m*c)**2"))
            .ToString()
            .ShouldBe("c**2*A_{m}*A_{a}*A^{m}*A^{a}");
    }

    [Fact(Skip = "Blocked by skeleton PowerUnfoldTransformation.Transform(), which still returns the input tensor unchanged.")]
    public void ShouldUnfoldRepeatedIndexPower()
    {
        PowerUnfoldTransformation transformation = PowerUnfoldTransformation.Instance;

        transformation.Transform(TensorApi.Parse("A_i^i**2"))
            .ToString()
            .ShouldBe(TensorApi.Parse("A_i^i*A_j^j").ToString());
    }
}
