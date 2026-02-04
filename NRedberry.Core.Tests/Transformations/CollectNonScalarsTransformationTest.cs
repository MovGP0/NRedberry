using NRedberry.Tensors;
using NRedberry.Transformations.Symmetrization;
using TensorFactory = NRedberry.Tensors.Tensors;
using TensorType = NRedberry.Tensors.Tensor;
using Xunit;

namespace NRedberry.Core.Tests.Transformations;

public sealed class CollectNonScalarsTransformationTest
{
    [Fact(Skip = "CollectNonScalarsTransformation is not yet implemented.")]
    public void ShouldCollectNonScalars()
    {
        TensorType tensor = TensorFactory.Parse("-c1*a**(-1)*k_{i}*k^{i}*d_{b}^{c}+(c0-c0*a**(-1))*k_{i}*k^{i}*k_{b}*k^{c}+c1*k_{b}*k^{c}");
        SumBuilderSplitingScalars builder = new();
        foreach (TensorType item in tensor)
        {
            builder.Put(item);
        }

        TensorType built = builder.Build();
        TensorType collected = CollectNonScalarsITransformation.CollectNonScalars(tensor);
        Assert.True(TensorUtils.Equals(built, collected));
    }
}
