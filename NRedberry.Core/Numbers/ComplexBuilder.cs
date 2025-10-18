using NRedberry.Core.Tensors;

namespace NRedberry.Core.Numbers;

internal sealed class ComplexBuilder : TensorBuilder
{
    private readonly Complex _complex;
    public ComplexBuilder(Complex complex) => _complex = complex;
    public Tensor Build() => _complex;

    public void Put(Tensor tensor) => throw new InvalidOperationException("Cannot put to Complex tensor builder!");

    /// <summary>
    /// This method just returns the current instance because the ComplexBuilder is immutable
    /// </summary>
    /// <returns></returns>
    public TensorBuilder Clone() => this;
}