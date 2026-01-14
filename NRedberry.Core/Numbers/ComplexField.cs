using NRedberry.Apache.Commons.Math;

namespace NRedberry.Numbers;

public sealed class ComplexField : IField<Complex>
{
    private static readonly Lazy<ComplexField> s_instance = new(() => new ComplexField());

    private ComplexField()
    {
    }

    public Complex One => Complex.One;

    public Complex Zero => Complex.Zero;

    public Type GetRuntimeClass() => typeof(Complex);

    [Obsolete("Inject IField<Complex> instead.")]
    public static ComplexField GetInstance()
    {
        return s_instance.Value;
    }

    public static ComplexField Instance => s_instance.Value;
}
