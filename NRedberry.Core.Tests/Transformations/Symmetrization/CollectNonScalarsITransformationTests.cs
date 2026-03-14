using NRedberry.Transformations.Symmetrization;
using SumBuilderType = NRedberry.Tensors.SumBuilderSplitingScalars;
using TensorType = NRedberry.Tensors.Tensor;
using TensorApi = NRedberry.Tensors.Tensors;
using TensorUtilsApi = NRedberry.Tensors.TensorUtils;
using Xunit;

namespace NRedberry.Core.Tests.Transformations.Symmetrization;

public sealed class CollectNonScalarsITransformationTests
{
    [Fact]
    public void ShouldReturnSingletonInstance()
    {
        Assert.Same(
            CollectNonScalarsITransformation.Instance,
            CollectNonScalarsITransformation.Instance);
    }

    [Fact]
    public void ShouldCollectNonScalarSummandsThroughStaticHelper()
    {
        TensorType tensor = TensorApi.Parse("-c1*a**(-1)*k_{i}*k^{i}*d_{b}^{c}+(c0-c0*a**(-1))*k_{i}*k^{i}*k_{b}*k^{c}+c1*k_{b}*k^{c}");
        SumBuilderType builder = new();
        foreach (TensorType item in tensor)
        {
            builder.Put(item);
        }

        TensorType collected = CollectNonScalarsITransformation.CollectNonScalars(tensor);
        Assert.True(TensorUtilsApi.Equals(builder.Build(), collected));
    }

    [Fact]
    public void ShouldCollectNonScalarSummandsThroughInstance()
    {
        TensorType tensor = TensorApi.Parse("-c1*a**(-1)*k_{i}*k^{i}*d_{b}^{c}+(c0-c0*a**(-1))*k_{i}*k^{i}*k_{b}*k^{c}+c1*k_{b}*k^{c}");
        SumBuilderType builder = new();
        foreach (TensorType item in tensor)
        {
            builder.Put(item);
        }

        TensorType collected = CollectNonScalarsITransformation.Instance.Transform(tensor);
        Assert.True(TensorUtilsApi.Equals(builder.Build(), collected));
    }
}
