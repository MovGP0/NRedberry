using SystemBigInteger = System.Numerics.BigInteger;
using NRedberry.Tensors;
using NRedberry.Transformations.Symmetrization;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Arith;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Ufd;
using TensorComplex = NRedberry.Numbers.Complex;
using TensorRational = NRedberry.Rational;
using JasBigInteger = NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Arith.BigInteger;
using JasBigRational = NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Arith.BigRational;

namespace NRedberry.Transformations.Factor;

/// <summary>
/// Skeleton port of cc.redberry.core.transformations.factor.JasFactor.
/// </summary>
public sealed class JasFactor : ITransformation
{
    public static JasFactor Engine { get; } = new();

    private JasFactor()
    {
    }

    public Tensor Transform(Tensor tensor)
    {
        return Factor1(tensor);
    }

    internal static readonly char StartChar = 'a';

    internal static Tensor Factor1(Tensor t)
    {
        if (t is not MultiTensor && t is not Power)
        {
            return t;
        }

        Dictionary<int, JasVar> vars = GetVars(t);
        JasVar[] varsArray = vars.Values.ToArray();
        Array.Sort(varsArray);
        string[] forFactoryNames = new string[varsArray.Length];
        for (int i = 0; i < varsArray.Length; ++i)
        {
            varsArray[i].Position = i;
            string name = ((char)(StartChar + i)).ToString();
            varsArray[i].PolyName = name;
            forFactoryNames[i] = name;
        }

        var factory = new GenPolynomialRing<JasBigInteger>(JasBigInteger.One, forFactoryNames);
        GenPolynomial<JasBigInteger> poly;
        SystemBigInteger gcd;
        SystemBigInteger lcm;
        if (ContainsRationals(t))
        {
            var ratFactory = new GenPolynomialRing<JasBigRational>(JasBigRational.One, forFactoryNames);
            GenPolynomial<JasBigRational> polyRat = TensorToPoly(t, ratFactory, vars, ConvertRational);
            object[] factors = PolyUtil.IntegerFromRationalCoefficientsFactor(factory, polyRat);
            gcd = (SystemBigInteger)factors[0];
            lcm = (SystemBigInteger)factors[1];
            poly = (GenPolynomial<JasBigInteger>)factors[2];
        }
        else
        {
            gcd = SystemBigInteger.One;
            lcm = SystemBigInteger.One;
            poly = TensorToPoly(t, factory, vars, ConvertInteger);
        }

        if (poly.IsZero())
        {
            return TensorComplex.Zero;
        }

        FactorAbstract<JasBigInteger> jasFactor = FactorFactory.GetImplementation(JasBigInteger.One);
        SortedDictionary<GenPolynomial<JasBigInteger>, long> map = jasFactor.Factors(poly);

        if (!jasFactor.IsFactorization(poly, map))
        {
            return t;
        }

        var toMultiply = new List<Tensor>(map.Count);
        foreach (KeyValuePair<GenPolynomial<JasBigInteger>, long> entry in map)
        {
            toMultiply.Add(Tensors.Tensors.Pow(PolyToTensor(entry.Key, varsArray), new TensorComplex(entry.Value)));
        }

        if (gcd != SystemBigInteger.One || lcm != SystemBigInteger.One)
        {
            toMultiply.Add(new TensorComplex(new TensorRational(gcd, lcm)));
        }

        return Tensors.Tensors.Multiply(toMultiply.ToArray());
    }

    private static GenPolynomial<T> TensorToPoly<T>(
        Tensor tensor,
        GenPolynomialRing<T> factory,
        Dictionary<int, JasVar> vars,
        Func<TensorComplex, T> numberConverter)
        where T : RingElem<T>
    {
        if (tensor.GetType() == typeof(SimpleTensor))
        {
            int position = vars[((SimpleTensor)tensor).Name].Position;
            return new GenPolynomial<T>(factory, factory.GetOneCoefficient(), ExpVector.Create(vars.Count, position, 1));
        }

        if (tensor.GetType() == typeof(Power))
        {
            long pow = ((TensorComplex)tensor[1]).LongValue();
            if (tensor[0] is SimpleTensor)
            {
                int position = vars[((SimpleTensor)tensor[0]).Name].Position;
                return new GenPolynomial<T>(factory, factory.GetOneCoefficient(), ExpVector.Create(vars.Count, position, pow));
            }

            GenPolynomial<T> result = new(factory, factory.GetOneCoefficient());
            GenPolynomial<T> basePoly = TensorToPoly(tensor[0], factory, vars, numberConverter);
            while (pow > 0)
            {
                if ((pow & 0x1) != 0)
                {
                    result = result.Multiply(basePoly);
                }

                basePoly = basePoly.Multiply(basePoly);
                pow >>= 1;
            }

            return result;
        }

        if (tensor.GetType() == typeof(Sum))
        {
            GenPolynomial<T> result = new(factory);
            foreach (Tensor t in tensor)
            {
                result = result.Sum(TensorToPoly(t, factory, vars, numberConverter));
            }

            return result;
        }

        if (tensor.GetType() == typeof(Product))
        {
            GenPolynomial<T> result = new(factory, factory.GetOneCoefficient());
            foreach (Tensor t in tensor)
            {
                result = result.Multiply(TensorToPoly(t, factory, vars, numberConverter));
            }

            return result;
        }

        if (tensor.GetType() == typeof(TensorComplex))
        {
            return new GenPolynomial<T>(factory, numberConverter((TensorComplex)tensor));
        }

        throw new InvalidOperationException();
    }

    private static JasBigRational ConvertRational(TensorComplex complex)
    {
        TensorRational rational = (TensorRational)complex.Real;
        return new JasBigRational(new JasBigInteger(rational.Numerator), new JasBigInteger(rational.Denominator));
    }

    private static JasBigInteger ConvertInteger(TensorComplex complex)
    {
        return new JasBigInteger(((TensorRational)complex.Real).Numerator);
    }

    private static bool ContainsRationals(Tensor tensor)
    {
        if (tensor is TensorComplex complex)
        {
            return !complex.IsInteger();
        }

        foreach (Tensor t in tensor)
        {
            if (ContainsRationals(t))
            {
                return true;
            }
        }

        return false;
    }

    internal static Tensor PolyToTensor(GenPolynomial<JasBigInteger> poly, JasVar[] varsArray)
    {
        if (poly.Length() == 0)
        {
            return TensorComplex.Zero;
        }

        var temp = new List<Tensor>();
        var sum = new List<Tensor>(poly.Length());
        foreach (Monomial<JasBigInteger> monomial in poly)
        {
            JasBigInteger coefficient = monomial.Coefficient();
            ExpVector exp = monomial.Exponent();
            temp.Clear();

            temp.Add(new TensorComplex(new TensorRational(coefficient.GetVal())));
            for (int i = 0; i < exp.Length(); ++i)
            {
                long power = exp.GetVal(i);
                if (power != 0)
                {
                    temp.Add(Tensors.Tensors.Pow(varsArray[i].SimpleTensor, new TensorComplex(power)));
                }
            }

            sum.Add(Tensors.Tensors.Multiply(temp.ToArray()));
        }

        return Tensors.Tensors.Sum(sum.ToArray());
    }

    internal static Dictionary<int, JasVar> GetVars(params Tensor[] tensors)
    {
        var vars = new Dictionary<int, JasVar>();
        foreach (Tensor t in tensors)
        {
            AddVars(t, vars, 1);
        }

        return vars;
    }

    internal static void AddVars(Tensor tensor, Dictionary<int, JasVar> vars, long power)
    {
        if (power < 0)
        {
            throw new ArgumentException("Negative powers.", nameof(power));
        }

        if (tensor.GetType() == typeof(SimpleTensor))
        {
            if (tensor.Indices.Size() != 0)
            {
                throw new ArgumentException();
            }

            int name = ((SimpleTensor)tensor).Name;
            if (!vars.TryGetValue(name, out JasVar? varRef))
            {
                varRef = new JasVar((SimpleTensor)tensor);
                vars.Add(name, varRef);
            }

            varRef.MaxPower = Math.Max(power, varRef.MaxPower);
            return;
        }

        if (tensor.GetType() == typeof(Power))
        {
            if (!TensorUtils.IsNaturalNumber(tensor[1]))
            {
                throw new ArgumentException(tensor.ToString());
            }

            long pow = power * ((TensorComplex)tensor[1]).LongValue();
            AddVars(tensor[0], vars, pow);
            return;
        }

        if (tensor is MultiTensor)
        {
            foreach (Tensor t in tensor)
            {
                AddVars(t, vars, power);
            }

            return;
        }

        if (tensor.GetType() == typeof(TensorComplex))
        {
            var complex = (TensorComplex)tensor;
            if (complex.IsNumeric() || !complex.IsReal())
            {
                throw new ArgumentException("Illegal coefficient: " + tensor);
            }

            return;
        }

        throw new ArgumentException();
    }

    internal static GenPolynomial<JasBigInteger> TensorToPoly(Tensor t)
    {
        Dictionary<int, JasVar> vars = GetVars(t);
        JasVar[] varsArray = vars.Values.ToArray();
        Array.Sort(varsArray);
        string[] forFactoryNames = new string[varsArray.Length];
        for (int i = 0; i < varsArray.Length; ++i)
        {
            varsArray[i].Position = i;
            string name = ((char)(StartChar + i)).ToString();
            varsArray[i].PolyName = name;
            forFactoryNames[i] = name;
        }

        var factory = new GenPolynomialRing<JasBigInteger>(JasBigInteger.One, forFactoryNames);
        if (ContainsRationals(t))
        {
            var ratFactory = new GenPolynomialRing<JasBigRational>(JasBigRational.One, forFactoryNames);
            GenPolynomial<JasBigRational> polyRat = TensorToPoly(t, ratFactory, vars, ConvertRational);
            object[] factors = PolyUtil.IntegerFromRationalCoefficientsFactor(factory, polyRat);
            return (GenPolynomial<JasBigInteger>)factors[2];
        }

        return TensorToPoly(t, factory, vars, ConvertInteger);
    }
}

internal sealed class JasVar : IComparable<JasVar>
{
    public int Name { get; }
    public string PolyName { get; set; } = string.Empty;
    public int Position { get; set; }
    public long MaxPower { get; set; }
    public SimpleTensor SimpleTensor { get; }

    public JasVar(SimpleTensor simpleTensor)
    {
        SimpleTensor = simpleTensor;
        Name = simpleTensor.Name;
    }

    public int CompareTo(JasVar? other)
    {
        if (other is null)
        {
            return 1;
        }

        return other.MaxPower.CompareTo(MaxPower);
    }
}
