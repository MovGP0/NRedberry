using NRedberry.Transformations.Expand;
using NRedberry.Transformations.Symmetrization;
using NRedberry.Tensors;
using TensorCC = NRedberry.Tensors.CC;
using TensorFactory = NRedberry.Tensors.Tensors;
using TensorType = NRedberry.Tensors.Tensor;
using Xunit;

namespace NRedberry.Core.Tests.Transformations;

public sealed class ApplyDiracDeltasTransformationTest
{
    [Fact]
    public void ShouldApplyDiracDeltasToTwoFactorChain()
    {
        TensorCC.Reset();
        TensorType tensor = TensorFactory.Parse("DiracDelta[q,p-l]*DiracDelta[l,f]*(q**2-2)");
        TensorType actual = ApplyDiracDeltasTransformation.Instance.Transform(tensor);
        TensorType expected = TensorFactory.Parse("(p-f)**2-2");
        TensorUtils.Equals(expected, actual).ShouldBeTrue();
    }

    [Fact]
    public void ShouldApplyDiracDeltasToThreeFactorChain()
    {
        TensorCC.Reset();
        TensorType tensor = TensorFactory.Parse("DiracDelta[q,p-l]*DiracDelta[l,f-k]*DiracDelta[-k,r]*(q**2-2)");
        TensorType actual = ApplyDiracDeltasTransformation.Instance.Transform(tensor);
        TensorType expected = ExpandTransformation.Expand(TensorFactory.Parse("(p-f-r)**2-2"));
        TensorUtils.Equals(expected, ExpandTransformation.Expand(actual)).ShouldBeTrue();
    }

    [Fact]
    public void ShouldApplyDiracDeltasToSignedPair()
    {
        TensorCC.Reset();
        TensorType tensor = TensorFactory.Parse("DiracDelta[q,f]*DiracDelta[-q,p]*(q**2-2)");
        TensorType actual = ApplyDiracDeltasTransformation.Instance.Transform(tensor);
        actual.ToString(OutputFormat.Redberry).ShouldBeOneOf(
            "p**2-2",
            "-2+p**2",
            "f**2-2",
            "-2+f**2");
    }
}
