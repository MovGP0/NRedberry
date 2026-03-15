using NRedberry.IndexMapping;
using NRedberry.Transformations.Substitutions;
using Shouldly;
using TensorApi = NRedberry.Tensors.Tensors;
using Xunit;

namespace NRedberry.Core.Tests.Transformations.Substitutions;

public sealed class PrimitiveSumSubstitutionTests
{
    [Fact]
    public void ShouldSubstituteMatchingSummands()
    {
        NRedberry.Tensors.Tensor current = TensorApi.Parse("a+b+d");
        PrimitiveSumSubstitution substitution = new(TensorApi.Parse("a+b"), TensorApi.Parse("c"));

        NRedberry.Tensors.Tensor result = substitution.NewTo(current, new SubstitutionIterator(current));

        IndexMappings.Equals(result, TensorApi.Parse("c+d")).ShouldBeTrue();
    }

    [Fact]
    public void ShouldRepeatedlySubstituteMatchingSummands()
    {
        NRedberry.Tensors.Tensor current = TensorApi.Parse("a+b+a+b");
        PrimitiveSumSubstitution substitution = new(TensorApi.Parse("a+b"), TensorApi.Parse("c"));

        NRedberry.Tensors.Tensor result = substitution.NewTo(current, new SubstitutionIterator(current));

        IndexMappings.Equals(result, TensorApi.Parse("2*c")).ShouldBeTrue();
    }

    [Fact]
    public void ShouldReturnSameTensorWhenNoBijectionExists()
    {
        NRedberry.Tensors.Tensor current = TensorApi.Parse("a+d");
        PrimitiveSumSubstitution substitution = new(TensorApi.Parse("a+b"), TensorApi.Parse("c"));

        NRedberry.Tensors.Tensor result = substitution.NewTo(current, new SubstitutionIterator(current));

        ReferenceEquals(result, current).ShouldBeTrue();
    }
}
