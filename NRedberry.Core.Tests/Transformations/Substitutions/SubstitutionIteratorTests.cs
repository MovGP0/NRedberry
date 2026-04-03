using NRedberry.Tensors.Iterators;
using NRedberry.Transformations.Substitutions;
using TensorApi = NRedberry.Tensors.Tensors;
using Xunit;

namespace NRedberry.Core.Tests.Transformations.Substitutions;

public sealed class SubstitutionIteratorTests
{
    [Fact]
    public void ShouldIterateInLeavingOrder()
    {
        var sum = TensorApi.Parse("a+b");
        SubstitutionIterator iterator = new(sum);

        iterator.Next().ToString(OutputFormat.Redberry).ShouldBe(sum[0].ToString(OutputFormat.Redberry));
        iterator.Next().ToString(OutputFormat.Redberry).ShouldBe(sum[1].ToString(OutputFormat.Redberry));
        iterator.Next().ToString(OutputFormat.Redberry).ShouldBe(sum.ToString(OutputFormat.Redberry));
        iterator.Next().ShouldBeNull();
    }

    [Fact]
    public void ShouldAllowUnsafeReplacementOfCurrentNode()
    {
        var sum = TensorApi.Parse("a+b");
        string secondSummand = sum[1].ToString(OutputFormat.Redberry);
        string expected = TensorApi.Parse($"c+{secondSummand}").ToString(OutputFormat.Redberry);
        SubstitutionIterator iterator = new(sum);

        _ = iterator.Next();
        iterator.UnsafeSet(TensorApi.Parse("c"));

        iterator.IsCurrentModified().ShouldBeFalse();
        iterator.Next().ToString(OutputFormat.Redberry).ShouldBe(secondSummand);
        iterator.Next().ToString(OutputFormat.Redberry).ShouldBe(expected);
        iterator.Next().ShouldBeNull();
        iterator.Result().ToString(OutputFormat.Redberry).ShouldBe(expected);
    }

    [Fact]
    public void ShouldExposeEmptyForbiddenSetWithCurrentPort()
    {
        SubstitutionIterator iterator = new(TensorApi.Parse("a+b"), TraverseGuide.All);

        iterator.GetForbidden().ShouldBeEmpty();
    }
}
