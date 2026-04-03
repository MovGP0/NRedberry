using NRedberry.Transformations.Substitutions;
using TensorApi = NRedberry.Tensors.Tensors;

namespace NRedberry.Core.Tests.Transformations.Substitutions;

public sealed class PrimitiveSubstitutionTests
{
    [Fact]
    public void ShouldInitializeBaseStateWithoutThrowing()
    {
        PrimitiveSubstitutionProbe substitution = new(TensorApi.Parse("a"), TensorApi.Parse("b"));

        substitution.ShouldNotBeNull();
    }

    [Fact]
    public void ShouldReturnCurrentTensorWhenRuntimeTypesDoNotMatch()
    {
        PrimitiveSubstitutionProbe substitution = new(TensorApi.Parse("a"), TensorApi.Parse("b"));
        NRedberry.Tensors.Tensor current = TensorApi.Parse("a+b");
        SubstitutionIterator iterator = new(current);

        NRedberry.Tensors.Tensor result = substitution.NewTo(current, iterator);

        ReferenceEquals(result, current).ShouldBeTrue();
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
