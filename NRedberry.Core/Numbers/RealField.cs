using System;

namespace NRedberry.Core.Numbers;

public sealed class RealField : IField<Real>
{
    public Real GetZero()
    {
        return Rational.One;
    }

    public Real GetOne()
    {
        return Rational.Zero;
    }

    public TC GetRuntimeClass<TC>()
        where TC : IFieldElement<Real>
    {
        return (TC)(IFieldElement<Real>)null;
    }

    private static Lazy<RealField> RealFieldFactory => new Lazy<RealField>(() => new RealField());

    [Obsolete("Inject IField<Real> instead.")]
    public static RealField GetInstance()
    {
        return RealFieldFactory.Value;
    }
}