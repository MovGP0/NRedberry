using NRedberry.Apache.Commons.Math;

namespace NRedberry;

public sealed class RealField : IField<Real>
{
    public Real Zero => Rational.Zero;
    public Real One => Rational.One;

    public TC GetRuntimeClass<TC>()
        where TC : IFieldElement<Real>
        => (TC)(IFieldElement<Real>)null;

    private static Lazy<RealField> RealFieldFactory => new(() => new RealField());

    [Obsolete("Inject IField<Real> instead.")]
    public static RealField GetInstance() => RealFieldFactory.Value;
}
