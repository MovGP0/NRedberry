using NRedberry.Core.Tests.Extensions;
using NRedberry.Tensors;
using TensorFactory = NRedberry.Tensors.Tensors;

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

        Should.SatisfyAllConditions(
            () => tensors.Length.ShouldBe(1),
            () => tensors[0].ToString(OutputFormat.Redberry).ShouldBe("a**3"),
            () => container.Sign.ShouldBeFalse());
    }

    [Fact]
    public void ShouldKeepOppositeBasesAsSeparateEntries()
    {
        PowersContainer container = new();
        string firstExpected = TensorFactory.Parse("a-b").ToString(OutputFormat.Redberry);
        string secondExpected = TensorFactory.Parse("(b-a)**3").ToString(OutputFormat.Redberry);

        container.Put(TensorFactory.Parse("a-b"));
        container.Put(TensorFactory.Parse("(b-a)**3"));

        NRedberry.Tensors.Tensor[] tensors = container.ToArray();
        string[] texts = tensors.Select(t => t.ToString(OutputFormat.Redberry)).ToArray();

        Should.SatisfyAllConditions(
            () => tensors.Length.ShouldBe(2),
            () => texts.ShouldContain(firstExpected),
            () => texts.ShouldContain(secondExpected),
            () => container.Sign.ShouldBeFalse());
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

        Should.SatisfyAllConditions(
            () => left.IsEmpty().ShouldBeFalse(),
            () => tensors.Length.ShouldBe(2),
            () => tensors.ShouldContain(t => t.ToString(OutputFormat.Redberry) == "a**3"),
            () => tensors.ShouldContain(t => t.ToString(OutputFormat.Redberry) == "b"));
    }
}
