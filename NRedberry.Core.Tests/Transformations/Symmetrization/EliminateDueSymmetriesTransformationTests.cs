using NRedberry.Numbers;
using NRedberry.Tensors;
using NRedberry.Transformations.Symmetrization;
using Shouldly;
using Xunit;
using TensorApi = NRedberry.Tensors.Tensors;
using TensorType = NRedberry.Tensors.Tensor;

namespace NRedberry.Core.Tests.Transformations.Symmetrization;

public sealed class EliminateDueSymmetriesTransformationTests
{
    [Fact]
    public void ShouldExposeSingletonInstance()
    {
        EliminateDueSymmetriesTransformation.Instance.ShouldNotBeNull();
    }

    [Fact]
    public void ShouldEliminateProductThatIsZeroDueToSymmetry()
    {
        SetSymmetric("A_ab");
        SetAntiSymmetric("B_ab");

        TensorType tensor = TensorApi.Parse("A_ab*B^ab");

        TensorType actual = EliminateDueSymmetriesTransformation.Instance.Transform(tensor);

        TensorUtils.Equals(Complex.Zero, actual).ShouldBeTrue();
    }

    [Fact]
    public void ShouldRemoveZeroSummandCreatedBySymmetry()
    {
        SetSymmetric("C_ab");
        SetAntiSymmetric("D_ab");

        TensorType tensor = TensorApi.Parse("x + C_ab*D^ab");

        TensorType actual = EliminateDueSymmetriesTransformation.Instance.Transform(tensor);

        TensorUtils.Equals(TensorApi.Parse("x"), actual).ShouldBeTrue();
    }

    [Fact]
    public void ShouldLeaveNonZeroExpressionUnchanged()
    {
        SetSymmetric("E_ab");

        TensorType tensor = TensorApi.Parse("E_ab*E^ab");

        TensorType actual = EliminateDueSymmetriesTransformation.Instance.Transform(tensor);

        TensorUtils.Equals(tensor, actual).ShouldBeTrue();
    }

    private static void SetSymmetric(string tensor)
    {
        TensorApi.ParseSimple(tensor).SimpleIndices.Symmetries.SetSymmetric();
    }

    private static void SetAntiSymmetric(string tensor)
    {
        TensorApi.ParseSimple(tensor).SimpleIndices.Symmetries.SetAntiSymmetric();
    }
}
