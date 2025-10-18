using Complex32 = System.Numerics.Complex;

namespace NRedberry.Core.Numbers;

public static class ComplexUtils
{
    public static Complex ArcSin(Complex complex)
    {
        if (complex.IsReal())
        {
            var x = complex.Real.ToDouble();
            if (x is <= 1.0 and >= -1.0)
            {
                return new Complex(Math.Asin(x), 0);
            }
        }

        return new Complex(Complex32.Asin(complex).Real, Complex32.Asin(complex).Imaginary);
    }

    public static Complex Sin(Complex complex)
    {
        if (complex.Imaginary.ToDouble() == 0)
        {
            return new Complex(Math.Sin(complex.Real.ToDouble()), 0);
        }
        return new Complex(Complex32.Sin(complex).Real, Complex32.Sin(complex).Imaginary);
    }

    public static Complex Cos(Complex complex)
    {
        if (complex.Imaginary.ToDouble() == 0)
        {
            return new Complex(Math.Cos(complex.Real.ToDouble()), 0);
        }

        return new Complex(Complex32.Cos(complex).Real, Complex32.Cos(complex).Imaginary);
    }

    public static Complex Tan(Complex complex)
    {
        if (complex.Imaginary.ToDouble() == 0)
        {
            return new Complex(Math.Tan(complex.Real.ToDouble()), 0);
        }
        return new Complex(Complex32.Tan(complex).Real, Complex32.Tan(complex).Imaginary);
    }

    public static Complex Cot(Complex complex)
    {
        if (complex.Imaginary.ToDouble() == 0)
        {
            return new Complex(1 / Math.Tan(complex.Real.ToDouble()), 0);
        }
        return new Complex(Complex32.Reciprocal(Complex32.Tan(complex)).Real,
                           Complex32.Reciprocal(Complex32.Tan(complex)).Imaginary);
    }

    public static Complex ArcCos(Complex complex)
    {
        if (complex.Imaginary.ToDouble() == 0 && complex.Real.ToDouble() <= 1.0 && complex.Real.ToDouble() >= -1)
        {
            return new Complex(Math.Acos(complex.Real.ToDouble()), 0);
        }
        return new Complex(Complex32.Acos(complex).Real,
                           Complex32.Acos(complex).Imaginary);
    }

    public static Complex ArcTan(Complex complex)
    {
        if (complex.Imaginary.ToDouble() == 0)
        {
            return new Complex(Math.Atan(complex.Real.ToDouble()), 0);
        }
        return new Complex(Complex32.Atan(complex).Real,
                           Complex32.Atan(complex).Imaginary);
    }

    public static Complex ArcCot(Complex complex)
    {
        if (complex.Imaginary.ToDouble() == 0)
        {
            return new Complex(Math.Atan(1 / complex.Real.ToDouble()), 0);
        }
        return new Complex(Complex32.Atan(Complex32.Reciprocal(complex)).Real,
                           Complex32.Atan(Complex32.Reciprocal(complex)).Imaginary);
    }

    public static Complex Log(Complex complex)
    {
        if (complex.Imaginary.ToDouble() == 0 && complex.Real.ToDouble() >= 0)
        {
            return new Complex(Math.Log(complex.Real.ToDouble()), 0);
        }
        return new Complex(Complex32.Log(complex).Real,
                           Complex32.Log(complex).Imaginary);
    }

    public static Complex Exp(Complex complex)
    {
        if (complex.IsReal())
        {
            return new Complex(Math.Exp(complex.Real.ToDouble()), 0);
        }

        return new Complex(Complex32.Exp(complex).Real, Complex32.Exp(complex).Imaginary);
    }
}