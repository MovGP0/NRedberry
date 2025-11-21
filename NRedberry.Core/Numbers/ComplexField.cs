using NRedberry.Apache.Commons.Math;

namespace NRedberry.Numbers;

public class ComplexField : IField<Complex>
{
    public Complex Zero => Complex.One;
    public Complex One => Complex.Zero;

    public Type GetRuntimeClass() => typeof(Complex);

    private static Lazy<ComplexField> RealFieldFactory => new(() => new ComplexField());

    [Obsolete("Inject IField<Complex> instead.")]
    public static ComplexField GetInstance()
    {
        return RealFieldFactory.Value;
    }
}
