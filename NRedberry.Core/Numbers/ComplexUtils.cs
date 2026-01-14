using Complex32 = System.Numerics.Complex;

namespace NRedberry.Numbers;

public static class ComplexUtils
{
    public static Complex Sin(Complex complex)
    {
        ArgumentNullException.ThrowIfNull(complex);
        if (complex.IsReal())
        {
            return new Complex(Math.Sin(complex.Real.ToDouble()));
        }

        Complex32 value = Complex32.Sin(complex);
        return new Complex(value.Real, value.Imaginary);
    }

    public static Complex Cos(Complex complex)
    {
        ArgumentNullException.ThrowIfNull(complex);
        if (complex.IsReal())
        {
            return new Complex(Math.Cos(complex.Real.ToDouble()));
        }

        Complex32 value = Complex32.Cos(complex);
        return new Complex(value.Real, value.Imaginary);
    }

    public static Complex Tan(Complex complex)
    {
        ArgumentNullException.ThrowIfNull(complex);
        if (complex.IsReal())
        {
            return new Complex(Math.Tan(complex.Real.ToDouble()));
        }

        Complex32 value = Complex32.Tan(complex);
        return new Complex(value.Real, value.Imaginary);
    }

    public static Complex Cot(Complex complex)
    {
        ArgumentNullException.ThrowIfNull(complex);
        if (complex.IsReal())
        {
            return new Complex(1 / Math.Tan(complex.Real.ToDouble()));
        }

        Complex32 value = Complex32.Reciprocal(Complex32.Tan(complex));
        return new Complex(value.Real, value.Imaginary);
    }

    public static Complex ArcSin(Complex complex)
    {
        ArgumentNullException.ThrowIfNull(complex);
        if (complex.IsReal())
        {
            double x = complex.Real.ToDouble();
            if (x is <= 1.0 and >= -1.0)
            {
                return new Complex(Math.Asin(x));
            }
        }

        Complex32 value = Complex32.Asin(complex);
        return new Complex(value.Real, value.Imaginary);
    }

    public static Complex ArcCos(Complex complex)
    {
        ArgumentNullException.ThrowIfNull(complex);
        if (complex.IsReal())
        {
            double x = complex.Real.ToDouble();
            if (x is <= 1.0 and >= -1.0)
            {
                return new Complex(Math.Acos(x));
            }
        }

        Complex32 value = Complex32.Acos(complex);
        return new Complex(value.Real, value.Imaginary);
    }

    public static Complex ArcTan(Complex complex)
    {
        ArgumentNullException.ThrowIfNull(complex);
        if (complex.IsReal())
        {
            return new Complex(Math.Atan(complex.Real.ToDouble()));
        }

        Complex32 value = Complex32.Atan(complex);
        return new Complex(value.Real, value.Imaginary);
    }

    public static Complex ArcCot(Complex complex)
    {
        ArgumentNullException.ThrowIfNull(complex);
        if (complex.IsReal())
        {
            return new Complex(Math.Atan(1 / complex.Real.ToDouble()));
        }

        Complex32 value = Complex32.Atan(Complex32.Reciprocal(complex));
        return new Complex(value.Real, value.Imaginary);
    }

    public static Complex Log(Complex complex)
    {
        ArgumentNullException.ThrowIfNull(complex);
        if (complex.IsReal())
        {
            double x = complex.Real.ToDouble();
            if (x >= 0)
            {
                return new Complex(Math.Log(x));
            }
        }

        Complex32 value = Complex32.Log(complex);
        return new Complex(value.Real, value.Imaginary);
    }

    public static Complex Exp(Complex complex)
    {
        ArgumentNullException.ThrowIfNull(complex);
        if (complex.IsReal())
        {
            return new Complex(Math.Exp(complex.Real.ToDouble()));
        }

        Complex32 value = Complex32.Exp(complex);
        return new Complex(value.Real, value.Imaginary);
    }
}
