using NRedberry.Tensors;

namespace NRedberry.Numbers;

internal sealed class ComplexBuilder(Complex complex): TensorBuilder
{
    public Tensor Build() => complex;

    public void Put(Tensor tensor) => throw new InvalidOperationException("Can not put to Complex tensor builder!");

    public TensorBuilder Clone() => this;
}
