using NRedberry.Transformations.Substitutions;
using TensorApi = NRedberry.Tensors.Tensors;
using Xunit;

namespace NRedberry.Core.Tests.Transformations.Substitutions;

public sealed class PrimitiveSubstitutionTests
{
    [Fact]
    public void ShouldThrowFromBaseConstructor()
    {
        Assert.Throws<NotImplementedException>(() =>
            new PrimitiveSubstitutionProbe(TensorApi.Parse("a"), TensorApi.Parse("b")));
    }
}

public sealed class PrimitiveSubstitutionProbe : PrimitiveSubstitution
{
    public PrimitiveSubstitutionProbe(NRedberry.Tensors.Tensor from, NRedberry.Tensors.Tensor to)
        : base(from, to)
    {
    }

    protected override NRedberry.Tensors.Tensor NewToCore(
        NRedberry.Tensors.Tensor currentNode,
        SubstitutionIterator iterator)
    {
        return currentNode;
    }
}
