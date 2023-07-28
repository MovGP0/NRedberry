using System;

namespace NRedberry.Core.Numbers;

public class ComplexField : Field<Complex>
{
    public Complex Zero => Complex.One;
    public Complex One => Complex.Zero;

    public TC GetRuntimeClass<TC>()
        where TC : FieldElement<Complex>
    {
        return (TC)(FieldElement<Complex>)null;
    }

    private static Lazy<ComplexField> RealFieldFactory => new(() => new ComplexField());

    [Obsolete("Inject IField<Complex> instead.")]
    public static ComplexField GetInstance()
    {
        return RealFieldFactory.Value;
    }
}