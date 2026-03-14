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
        SubstitutionIterator iterator = new(TensorApi.Parse("a+b"));

        iterator.Next().ToString(OutputFormat.Redberry).ShouldBe("a");
        iterator.Next().ToString(OutputFormat.Redberry).ShouldBe("b");
        iterator.Next().ToString(OutputFormat.Redberry).ShouldBe("a+b");
        iterator.Next().ShouldBeNull();
    }

    [Fact]
    public void ShouldAllowUnsafeReplacementOfCurrentNode()
    {
        SubstitutionIterator iterator = new(TensorApi.Parse("a+b"));

        _ = iterator.Next();
        iterator.UnsafeSet(TensorApi.Parse("c"));

        iterator.IsCurrentModified().ShouldBeTrue();
        iterator.Next().ToString(OutputFormat.Redberry).ShouldBe("b");
        iterator.Next().ToString(OutputFormat.Redberry).ShouldBe("c+b");
        iterator.Next().ShouldBeNull();
        iterator.Result().ToString(OutputFormat.Redberry).ShouldBe("c+b");
    }

    [Fact]
    public void ShouldExposeEmptyForbiddenSetWithCurrentPort()
    {
        SubstitutionIterator iterator = new(TensorApi.Parse("a+b"), TraverseGuide.All);

        iterator.GetForbidden().ShouldBeEmpty();
    }
}
