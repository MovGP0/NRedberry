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
        for (int i = 0; i < 30; i++)
        {
            TensorCC.Reset();
            TensorType tensor = TensorFactory.Parse("DiracDelta[q,p-l]*DiracDelta[l,f]*(q**2-2)");
            TensorType actual = ApplyDiracDeltasTransformation.Instance.Transform(tensor);
            TensorType expected = TensorFactory.Parse("(p-f)**2-2");
            Assert.True(TensorUtils.Equals(expected, actual));
        }
    }

    [Fact]
    public void ShouldApplyDiracDeltasToThreeFactorChain()
    {
        for (int i = 0; i < 30; i++)
        {
            TensorCC.Reset();
            TensorType tensor = TensorFactory.Parse("DiracDelta[q,p-l]*DiracDelta[l,f-k]*DiracDelta[-k,r]*(q**2-2)");
            TensorType actual = ApplyDiracDeltasTransformation.Instance.Transform(tensor);
            TensorType expected = TensorFactory.Parse("(p-f-r)**2-2");
            Assert.True(TensorUtils.Equals(expected, actual));
        }
    }

    [Fact]
    public void ShouldApplyDiracDeltasToSignedPair()
    {
        TensorType tensor = TensorFactory.Parse("DiracDelta[q,f]*DiracDelta[-q,p]*(q**2-2)");
        TensorType actual = ApplyDiracDeltasTransformation.Instance.Transform(tensor);
        TensorType expected = TensorFactory.Parse("p**2-2");
        Assert.True(TensorUtils.Equals(expected, actual));
    }
}
