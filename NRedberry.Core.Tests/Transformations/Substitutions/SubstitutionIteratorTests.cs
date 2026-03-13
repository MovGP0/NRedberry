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

        Assert.Equal("a", iterator.Next().ToString(OutputFormat.Redberry));
        Assert.Equal("b", iterator.Next().ToString(OutputFormat.Redberry));
        Assert.Equal("a+b", iterator.Next().ToString(OutputFormat.Redberry));
        Assert.Null(iterator.Next());
    }

    [Fact]
    public void ShouldAllowUnsafeReplacementOfCurrentNode()
    {
        SubstitutionIterator iterator = new(TensorApi.Parse("a+b"));

        _ = iterator.Next();
        iterator.UnsafeSet(TensorApi.Parse("c"));

        Assert.True(iterator.IsCurrentModified());
        Assert.Equal("b", iterator.Next().ToString(OutputFormat.Redberry));
        Assert.Equal("c+b", iterator.Next().ToString(OutputFormat.Redberry));
        Assert.Null(iterator.Next());
        Assert.Equal("c+b", iterator.Result().ToString(OutputFormat.Redberry));
    }

    [Fact]
    public void ShouldExposeEmptyForbiddenSetWithCurrentPort()
    {
        SubstitutionIterator iterator = new(TensorApi.Parse("a+b"), TraverseGuide.All);

        Assert.Empty(iterator.GetForbidden());
    }
}
