using NRedberry.Core.Tensors;

namespace NRedberry.Core.Numbers;

internal sealed class ComplexBuilder(Complex complex): TensorBuilder
{
    public Tensor Build() => complex;

    public void Put(Tensor tensor) => throw new InvalidOperationException("Can not put to Complex tensor builder!");

    public TensorBuilder Clone() => this;
}
