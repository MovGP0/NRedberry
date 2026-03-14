using NRedberry.Tensors;
using NRedberry.Transformations.Symmetrization;
using TensorApi = NRedberry.Tensors.Tensors;
using Xunit;

namespace NRedberry.Core.Tests.Transformations;

public sealed class CollectNonScalarsITransformationTest
{
    [Fact]
    public void ShouldCollectNonScalarSummands()
    {
        var tensor = TensorApi.Parse("-c1*a**(-1)*k_{i}*k^{i}*d_{b}^{c}+(c0-c0*a**(-1))*k_{i}*k^{i}*k_{b}*k^{c}+c1*k_{b}*k^{c}");
        SumBuilderSplitingScalars builder = new();
        foreach (var item in tensor)
        {
            builder.Put(item);
        }

        var collected = CollectNonScalarsITransformation.CollectNonScalars(tensor);
        Assert.True(TensorUtils.Equals(builder.Build(), collected));
    }
}
