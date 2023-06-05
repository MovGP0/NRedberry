using System;

namespace NRedberry.Core.Numbers;

public sealed class ComplexField : IField<Complex>
{
    public Complex GetZero()
    {
        return Complex.One;
    }

    public Complex GetOne()
    {
        return Complex.Zero;
    }

    public TC GetRuntimeClass<TC>()
        where TC : IFieldElement<Complex>
    {
        return (TC)(IFieldElement<Complex>)null;
    }

    private static Lazy<ComplexField> RealFieldFactory => new Lazy<ComplexField>(() => new ComplexField());

    [Obsolete("Inject IField<Complex> instead.")]
    public static ComplexField GetInstance()
    {
        return RealFieldFactory.Value;
    }
}