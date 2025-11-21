using NRedberry.Apache.Commons.Math;

namespace NRedberry;

public sealed class RealField : IField<Real>
{
    public Real Zero => Rational.Zero;
    public Real One => Rational.One;

    public Type GetRuntimeClass() => typeof(Real);

    private static Lazy<RealField> RealFieldFactory => new(() => new RealField());

    public static RealField Instance => RealFieldFactory.Value;
}
