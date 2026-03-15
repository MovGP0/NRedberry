using NRedberry.IndexMapping;
using NRedberry.Transformations.Substitutions;
using Shouldly;
using TensorApi = NRedberry.Tensors.Tensors;
using Xunit;

namespace NRedberry.Core.Tests.Transformations.Substitutions;

public sealed class PrimitiveProductSubstitutionTests
{
    [Fact]
    public void ShouldSubstituteIndexlessProductFactors()
    {
        NRedberry.Tensors.Tensor current = TensorApi.Parse("a*b*d");
        PrimitiveProductSubstitution substitution = new(TensorApi.Parse("a*b"), TensorApi.Parse("c"));

        NRedberry.Tensors.Tensor result = substitution.NewTo(current, new SubstitutionIterator(current));

        IndexMappings.Equals(result, TensorApi.Parse("c*d")).ShouldBeTrue();
    }

    [Fact]
    public void ShouldRepeatedlySubstituteMatchingIndexlessProductFactors()
    {
        NRedberry.Tensors.Tensor current = TensorApi.Parse("a*b*a*b");
        PrimitiveProductSubstitution substitution = new(TensorApi.Parse("a*b"), TensorApi.Parse("c"));

        NRedberry.Tensors.Tensor result = substitution.NewTo(current, new SubstitutionIterator(current));

        IndexMappings.Equals(result, TensorApi.Parse("c**2")).ShouldBeTrue();
    }
}
