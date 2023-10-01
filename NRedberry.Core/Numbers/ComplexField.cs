using System;
using NRedberry.Apache.Commons.Math;

namespace NRedberry.Core.Numbers;

public class ComplexField : IField<Complex>
{
    public Complex Zero => Complex.One;
    public Complex One => Complex.Zero;

    public TC GetRuntimeClass<TC>()
        where TC : IFieldElement<Complex>
    {
        return (TC)(IFieldElement<Complex>)null;
    }

    private static Lazy<ComplexField> RealFieldFactory => new(() => new ComplexField());

    [Obsolete("Inject IField<Complex> instead.")]
    public static ComplexField GetInstance()
    {
        return RealFieldFactory.Value;
    }
}