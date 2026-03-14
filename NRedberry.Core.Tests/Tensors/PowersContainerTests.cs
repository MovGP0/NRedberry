using System.Linq;
using NRedberry.Tensors;
using Shouldly;
using TensorFactory = NRedberry.Tensors.Tensors;
using Xunit;

namespace NRedberry.Core.Tests.Tensors;

public sealed class PowersContainerTests
{
    [Fact]
    public void ShouldStartEmpty()
    {
        PowersContainer container = new();

        container.IsEmpty().ShouldBeTrue();
        container.Sign.ShouldBeFalse();
        container.Count.ShouldBe(0);
        container.ShouldBeEmpty();
    }

    [Fact]
    public void ShouldKeepSeparateEntriesForPlainAndPoweredBase()
    {
        PowersContainer container = new();

        container.Put(TensorFactory.Parse("a"));
        container.Put(TensorFactory.Parse("a**2"));

        NRedberry.Tensors.Tensor[] tensors = container.ToArray();

        tensors.Length.ShouldBe(2);
        tensors.ShouldContain(t => t.ToString(OutputFormat.Redberry) == "a");
        tensors.ShouldContain(t => t.ToString(OutputFormat.Redberry) == "a**2");
        container.Sign.ShouldBeFalse();
    }

    [Fact]
    public void ShouldKeepOppositeBasesAsSeparateEntries()
    {
        PowersContainer container = new();

        container.Put(TensorFactory.Parse("a-b"));
        container.Put(TensorFactory.Parse("(b-a)**3"));

        NRedberry.Tensors.Tensor[] tensors = container.ToArray();

        tensors.Length.ShouldBe(2);
        tensors.ShouldContain(t => t.ToString(OutputFormat.Redberry) == "a-b");
        tensors.ShouldContain(t => t.ToString(OutputFormat.Redberry) == "(b-a)**3");
        container.Sign.ShouldBeFalse();
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
        left.IsEmpty().ShouldBeFalse();
        tensors.Length.ShouldBe(3);
        tensors.ShouldContain(t => t.ToString(OutputFormat.Redberry) == "a");
        tensors.ShouldContain(t => t.ToString(OutputFormat.Redberry) == "a**2");
        tensors.ShouldContain(t => t.ToString(OutputFormat.Redberry) == "b");
    }
}
