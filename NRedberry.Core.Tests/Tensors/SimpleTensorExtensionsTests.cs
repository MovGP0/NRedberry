using NRedberry.Contexts;
using NRedberry.Indices;
using NRedberry.Tensors;
using Xunit;

namespace NRedberry.Core.Tests.Tensors;

public sealed class SimpleTensorExtensionsTests
{
    [Fact]
    public void ShouldCreateKroneckerForOppositeStateIndices()
    {
        NRedberry.Contexts.Context currentContext = NRedberry.Contexts.Context.Get();

        SimpleTensor tensor = currentContext.CreateKronecker(IndicesUtils.ParseIndex("^a"), IndicesUtils.ParseIndex("_a"));

        Assert.True(currentContext.IsKronecker(tensor));
        Assert.True(currentContext.IsKroneckerOrMetric(tensor));
        Assert.False(currentContext.IsMetric(tensor));
    }

    [Fact]
    public void ShouldCreateMetricForSameStateMetricIndices()
    {
        NRedberry.Contexts.Context currentContext = NRedberry.Contexts.Context.Get();

        SimpleTensor tensor = currentContext.CreateMetric(IndicesUtils.ParseIndex("_a"), IndicesUtils.ParseIndex("_b"));

        Assert.True(currentContext.IsMetric(tensor));
        Assert.True(currentContext.IsKroneckerOrMetric(tensor));
        Assert.False(currentContext.IsKronecker(tensor));
    }

    [Fact]
    public void ShouldCreateMetricOrKroneckerFromIndexStates()
    {
        NRedberry.Contexts.Context currentContext = NRedberry.Contexts.Context.Get();

        SimpleTensor metric = currentContext.CreateMetricOrKronecker(IndicesUtils.ParseIndex("_a"), IndicesUtils.ParseIndex("_b"));
        SimpleTensor kronecker = currentContext.CreateMetricOrKronecker(IndicesUtils.ParseIndex("^a"), IndicesUtils.ParseIndex("_a"));

        Assert.True(currentContext.IsMetric(metric));
        Assert.True(currentContext.IsKronecker(kronecker));
    }

    [Fact]
    public void ShouldThrowForInvalidMetricAndKroneckerArguments()
    {
        NRedberry.Contexts.Context currentContext = NRedberry.Contexts.Context.Get();

        Assert.Throws<ArgumentException>(() => currentContext.CreateKronecker(IndicesUtils.ParseIndex("_a"), IndicesUtils.ParseIndex("_b")));
        Assert.Throws<ArgumentException>(() => currentContext.CreateMetric(IndicesUtils.ParseIndex("^a"), IndicesUtils.ParseIndex("_a")));
    }
}
