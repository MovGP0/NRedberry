using NRedberry.Apache.Commons.Math;

namespace NRedberry;

public sealed class RealField : IField<Real>
{
    private static readonly Lazy<RealField> s_instance = new(() => new RealField());

    private RealField()
    {
    }

    public Real Zero => Rational.Zero;
    public Real One => Rational.One;

    public Type GetRuntimeClass() => typeof(Real);

    public static RealField Instance => s_instance.Value;
}
