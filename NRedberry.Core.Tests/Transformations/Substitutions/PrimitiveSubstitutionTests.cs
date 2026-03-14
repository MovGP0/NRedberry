using NRedberry.Transformations.Substitutions;
using TensorApi = NRedberry.Tensors.Tensors;
using Xunit;

namespace NRedberry.Core.Tests.Transformations.Substitutions;

public sealed class PrimitiveSubstitutionTests
{
    [Fact]
    public void ShouldThrowFromBaseConstructor()
    {
        Should.Throw<NotImplementedException>(() =>
            new PrimitiveSubstitutionProbe(TensorApi.Parse("a"), TensorApi.Parse("b")));
    }
}

public sealed class PrimitiveSubstitutionProbe(NRedberry.Tensors.Tensor from, NRedberry.Tensors.Tensor to)
    : PrimitiveSubstitution(from, to)
{
    protected override NRedberry.Tensors.Tensor NewToCore(
        NRedberry.Tensors.Tensor currentNode,
        SubstitutionIterator iterator)
    {
        return currentNode;
    }
}
