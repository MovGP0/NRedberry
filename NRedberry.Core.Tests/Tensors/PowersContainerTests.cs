using System.Linq;
using NRedberry.Tensors;
using TensorFactory = NRedberry.Tensors.Tensors;
using Xunit;

namespace NRedberry.Core.Tests.Tensors;

public sealed class PowersContainerTests
{
    [Fact]
    public void ShouldStartEmpty()
    {
        PowersContainer container = new();

        Assert.True(container.IsEmpty());
        Assert.False(container.Sign);
        Assert.Equal(0, container.Count);
        Assert.Empty(container);
    }

    [Fact]
    public void ShouldKeepSeparateEntriesForPlainAndPoweredBase()
    {
        PowersContainer container = new();

        container.Put(TensorFactory.Parse("a"));
        container.Put(TensorFactory.Parse("a**2"));

        NRedberry.Tensors.Tensor[] tensors = container.ToArray();

        Assert.Equal(2, tensors.Length);
        Assert.Contains(tensors, t => t.ToString(OutputFormat.Redberry) == "a");
        Assert.Contains(tensors, t => t.ToString(OutputFormat.Redberry) == "a**2");
        Assert.False(container.Sign);
    }

    [Fact]
    public void ShouldKeepOppositeBasesAsSeparateEntries()
    {
        PowersContainer container = new();

        container.Put(TensorFactory.Parse("a-b"));
        container.Put(TensorFactory.Parse("(b-a)**3"));

        NRedberry.Tensors.Tensor[] tensors = container.ToArray();

        Assert.Equal(2, tensors.Length);
        Assert.Contains(tensors, t => t.ToString(OutputFormat.Redberry) == "a-b");
        Assert.Contains(tensors, t => t.ToString(OutputFormat.Redberry) == "(b-a)**3");
        Assert.False(container.Sign);
    }

    [Fact]
    public void ShouldMergeContainers()
    {
        PowersContainer left = new();
        PowersContainer right = new();

        left.Put(TensorFactory.Parse("a"));
        right.Put(TensorFactory.Parse("a**2"));
        right.Put(TensorFactory.Parse("b"));

        left.Merge(right);

        NRedberry.Tensors.Tensor[] tensors = left.ToArray();
        Assert.False(left.IsEmpty());
        Assert.Equal(3, tensors.Length);
        Assert.Contains(tensors, t => t.ToString(OutputFormat.Redberry) == "a");
        Assert.Contains(tensors, t => t.ToString(OutputFormat.Redberry) == "a**2");
        Assert.Contains(tensors, t => t.ToString(OutputFormat.Redberry) == "b");
    }
}
