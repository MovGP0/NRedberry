using NRedberry.Indices;
using NRedberry.Tensors;

namespace NRedberry.Core.Tests.Tensors;

public sealed class SimpleTensorExtensionsTests
{
    [Fact]
    public void ShouldCreateKroneckerForOppositeStateIndices()
    {
        NRedberry.Contexts.Context currentContext = NRedberry.Contexts.Context.Get();

        SimpleTensor tensor = currentContext.CreateKronecker(IndicesUtils.ParseIndex("^a"), IndicesUtils.ParseIndex("_a"));

        currentContext.IsKronecker(tensor).ShouldBeTrue();
        currentContext.IsKroneckerOrMetric(tensor).ShouldBeTrue();
        currentContext.IsMetric(tensor).ShouldBeFalse();
    }

    [Fact]
    public void ShouldCreateMetricForSameStateMetricIndices()
    {
        NRedberry.Contexts.Context currentContext = NRedberry.Contexts.Context.Get();

        SimpleTensor tensor = currentContext.CreateMetric(IndicesUtils.ParseIndex("_a"), IndicesUtils.ParseIndex("_b"));

        currentContext.IsMetric(tensor).ShouldBeTrue();
        currentContext.IsKroneckerOrMetric(tensor).ShouldBeTrue();
        currentContext.IsKronecker(tensor).ShouldBeFalse();
    }

    [Fact]
    public void ShouldCreateMetricOrKroneckerFromIndexStates()
    {
        NRedberry.Contexts.Context currentContext = NRedberry.Contexts.Context.Get();

        SimpleTensor metric = currentContext.CreateMetricOrKronecker(IndicesUtils.ParseIndex("_a"), IndicesUtils.ParseIndex("_b"));
        SimpleTensor kronecker = currentContext.CreateMetricOrKronecker(IndicesUtils.ParseIndex("^a"), IndicesUtils.ParseIndex("_a"));

        currentContext.IsMetric(metric).ShouldBeTrue();
        currentContext.IsKronecker(kronecker).ShouldBeTrue();
    }

    [Fact]
    public void ShouldThrowForInvalidMetricAndKroneckerArguments()
    {
        NRedberry.Contexts.Context currentContext = NRedberry.Contexts.Context.Get();

        Should.Throw<ArgumentException>(() => currentContext.CreateKronecker(IndicesUtils.ParseIndex("_a"), IndicesUtils.ParseIndex("_b")));
        Should.Throw<ArgumentException>(() => currentContext.CreateMetric(IndicesUtils.ParseIndex("^a"), IndicesUtils.ParseIndex("_a")));
    }
}
