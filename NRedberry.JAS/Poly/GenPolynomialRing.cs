using System.Collections;
using System.Numerics;
using System.Text;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;

/// <summary>
/// GenPolynomialRing generic polynomial factory implementing RingFactory;
/// Factory for n-variate ordered polynomials over C. Almost immutable object, except variable names.
/// </summary>
/// <typeparam name="C">coefficient type</typeparam>
/// <remarks>
/// Original Java file: cc.redberry.core.transformations.factor.jasfactor.edu.jas.poly.GenPolynomialRing
/// </remarks>
public class GenPolynomialRing<C> : RingFactory<GenPolynomial<C>>, IEnumerable<GenPolynomial<C>> where C : RingElem<C>
{
    private static readonly HashSet<string> KnownVars = new(StringComparer.Ordinal);

    public readonly RingFactory<C> CoFac;
    public readonly int Nvar;
    public readonly TermOrder Tord;
    protected bool partial;
    public bool IsPartial => partial;

    protected string[]? vars;
    private static GenPolynomial<C> ZERO;
    private static GenPolynomial<C> ONE;
    public readonly ExpVector Evzero;
    protected static readonly Random random = new();
    protected int isField = -1;

    public GenPolynomialRing(RingFactory<C> cf, int n)
        : this(cf, n, new TermOrder(), null)
    {
    }

    public GenPolynomialRing(RingFactory<C> cf, int n, TermOrder t)
        : this(cf, n, t, null)
    {
    }

    public GenPolynomialRing(RingFactory<C> cf, string[] v)
        : this(cf, v?.Length ?? throw new ArgumentNullException(nameof(v)), v)
    {
    }

    public GenPolynomialRing(RingFactory<C> cf, int n, string[] v)
        : this(cf, n, new TermOrder(), v)
    {
    }

    public GenPolynomialRing(RingFactory<C> cf, int n, TermOrder t, string[]? v)
    {
        ArgumentNullException.ThrowIfNull(cf);
        ArgumentNullException.ThrowIfNull(t);
        if (n < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(n), "Number of variables must be non-negative.");
        }

        CoFac = cf;
        Nvar = n;
        Tord = t;
        partial = false;

        if (v == null)
        {
            vars = NewVarsInternal("x", n);
        }
        else
        {
            if (v.Length != n)
            {
                throw new ArgumentException($"Incompatible variable size {v.Length}, expected {n}.", nameof(v));
            }

            vars = v.ToArray();
            AddVars(vars);
        }

        Evzero = ExpVector.Create(n);
        ZERO = new GenPolynomial<C>(this);
        C coeffOne = cf.FromInteger(1);
        ONE = new GenPolynomial<C>(this, coeffOne, Evzero);
    }

    public GenPolynomialRing(RingFactory<C> cf, GenPolynomialRing<C> other)
        : this(cf, other?.Nvar ?? throw new ArgumentNullException(nameof(other)), other.Tord, other.vars)
    {
    }

    public GenPolynomialRing(GenPolynomialRing<C> other, TermOrder to)
        : this(other?.CoFac ?? throw new ArgumentNullException(nameof(other)), other.Nvar, to, other.vars)
    {
    }

    public GenPolynomialRing<C> Copy()
    {
        return new GenPolynomialRing<C>(CoFac, this);
    }

    public override string ToString()
    {
        StringBuilder builder = new ();
        builder.Append(CoFac);
        builder.Append(" ( ");
        builder.Append(VarsToString());
        builder.Append(" ) ");
        builder.Append(Tord);
        return builder.ToString();
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(this, obj))
        {
            return true;
        }

        if (obj is not GenPolynomialRing<C> other)
        {
            return false;
        }

        if (Nvar != other.Nvar)
        {
            return false;
        }

        if (!CoFac.Equals(other.CoFac))
        {
            return false;
        }

        if (!Tord.Equals(other.Tord))
        {
            return false;
        }

        if (vars == null && other.vars == null)
        {
            return true;
        }

        if (vars == null || other.vars == null)
        {
            return false;
        }

        return vars.SequenceEqual(other.vars);
    }

    public override int GetHashCode()
    {
        HashCode hash = new ();
        hash.Add(Nvar);
        hash.Add(CoFac.GetHashCode());
        hash.Add(Tord);
        if (vars != null)
        {
            foreach (string variable in vars)
            {
                hash.Add(variable);
            }
        }

        return hash.ToHashCode();
    }

    public string[]? GetVars()
    {
        return vars?.ToArray();
    }

    public string[] SetVars(string[] v)
    {
        ArgumentNullException.ThrowIfNull(v);
        if (v.Length != Nvar)
        {
            throw new ArgumentException($"Variable array does not match number of variables: {v.Length} vs {Nvar}", nameof(v));
        }

        string[] previous = vars ?? Array.Empty<string>();
        vars = v.ToArray();
        AddVars(vars);
        return previous;
    }

    public string VarsToString()
    {
        if (vars == null)
        {
            return "#" + Nvar;
        }

        return ExpVector.VarsToString(vars);
    }

    public GenPolynomial<C> ValueOf(C a)
    {
        ArgumentNullException.ThrowIfNull(a);
        return new GenPolynomial<C>(this, a);
    }

    public GenPolynomial<C> ValueOf(ExpVector e)
    {
        ArgumentNullException.ThrowIfNull(e);
        return new GenPolynomial<C>(this, CoFac.FromInteger(1), e);
    }

    public GenPolynomial<C> ValueOf(C a, ExpVector e)
    {
        ArgumentNullException.ThrowIfNull(a);
        ArgumentNullException.ThrowIfNull(e);
        return new GenPolynomial<C>(this, a, e);
    }

    public GenPolynomial<C> FromInteger(long a)
    {
        return new GenPolynomial<C>(this, CoFac.FromInteger(a), Evzero);
    }

    public GenPolynomial<C> FromInteger(BigInteger a)
    {
        return new GenPolynomial<C>(this, CoFac.FromInteger(a), Evzero);
    }

    public GenPolynomial<C> Random()
    {
        return Random(3, 4, 3, 0.5f);
    }

    public GenPolynomial<C> Random(int n)
    {
        return Random(n, random);
    }

    public GenPolynomial<C> Random(int n, Random rnd)
    {
        ArgumentNullException.ThrowIfNull(rnd);
        if (Nvar == 1)
        {
            return Random(3, n, n, 0.7f, rnd);
        }

        return Random(5, n, 3, 0.3f, rnd);
    }

    public GenPolynomial<C> Random(int k, int l, int d, float q)
    {
        return Random(k, l, d, q, random);
    }

    public GenPolynomial<C> Random(int k, int l, int d, float q, Random rnd)
    {
        ArgumentNullException.ThrowIfNull(rnd);
        GenPolynomial<C> result = ZERO;
        for (int i = 0; i < l; i++)
        {
            ExpVector exponent = ExpVector.Random(Nvar, d, q, rnd);
            C coefficient = CoFac.Random(k, rnd);
            result = result.Sum(coefficient, exponent);
        }

        return result;
    }

    public GenPolynomial<C> Copy(GenPolynomial<C> polynomial)
    {
        ArgumentNullException.ThrowIfNull(polynomial);
        return new GenPolynomial<C>(this, polynomial.Terms);
    }

    public GenPolynomial<C> Univariate(int index)
    {
        return Univariate(0, index, 1L);
    }

    public GenPolynomial<C> Univariate(int index, long exponent)
    {
        return Univariate(0, index, exponent);
    }

    public GenPolynomial<C> Univariate(int moduleVariables, int index, long exponent)
    {
        GenPolynomial<C> polynomial = ZERO;
        int ringVariables = Nvar - moduleVariables;
        if (index >= 0 && index < ringVariables)
        {
            C one = CoFac.FromInteger(1);
            ExpVector vector = ExpVector.Create(ringVariables, index, exponent);
            if (moduleVariables > 0)
            {
                vector = vector.Extend(moduleVariables, 0, 0L);
            }

            polynomial = polynomial.Sum(one, vector);
        }

        return polynomial;
    }

    public List<GenPolynomial<C>> Generators()
    {
        List<C> coefficientGenerators = CoFac.Generators();
        List<GenPolynomial<C>> univariates = UnivariateList().ToList();
        List<GenPolynomial<C>> generators = new (coefficientGenerators.Count + univariates.Count);
        foreach (C coefficient in coefficientGenerators)
        {
            generators.Add(ONE.Multiply(coefficient));
        }

        generators.AddRange(univariates);
        return generators;
    }

    public bool IsFinite()
    {
        return Nvar == 0 && CoFac.IsFinite();
    }

    public bool IsCommutative()
    {
        return CoFac.IsCommutative();
    }

    public bool IsAssociative()
    {
        return CoFac.IsAssociative();
    }

    public static GenPolynomial<C> Zero => ZERO;

    public static GenPolynomial<C> One => ONE;

    public C GetZeroCoefficient()
    {
        return CoFac.FromInteger(0);
    }

    public C GetOneCoefficient()
    {
        return CoFac.FromInteger(1);
    }
    
    public static GenPolynomial<C> Clone(GenPolynomial<C> polynomial)
    {
        ArgumentNullException.ThrowIfNull(polynomial);
        return polynomial.Clone();
    }

    public bool IsField()
    {
        if (isField > 0)
        {
            return true;
        }

        if (isField == 0)
        {
            return false;
        }

        if (CoFac.IsField() && Nvar == 0)
        {
            isField = 1;
            return true;
        }

        isField = 0;
        return false;
    }

    public BigInteger Characteristic()
    {
        return CoFac.Characteristic();
    }

    public List<GenPolynomial<C>> UnivariateList()
    {
        return UnivariateList(0, 1L).ToList();
    }

    public IEnumerable<GenPolynomial<C>> UnivariateList(int moduleVariables, long exponent)
    {
        List<GenPolynomial<C>> polynomials = new (Nvar);
        int ringVariables = Nvar - moduleVariables;
        for (int i = 0; i < ringVariables; i++)
        {
            GenPolynomial<C> polynomial = Univariate(moduleVariables, ringVariables - 1 - i, exponent);
            polynomials.Add(polynomial);
        }

        return polynomials;
    }

    public GenPolynomialRing<C> Extend(int count)
    {
        string[] newVariables = NewVars("e", count);
        return Extend(newVariables);
    }

    public GenPolynomialRing<C> Extend(string[] vn)
    {
        ArgumentNullException.ThrowIfNull(vn);
        if (vars == null)
        {
            throw new InvalidOperationException("Variable names are not initialized.");
        }

        string[] newVars = new string[vars.Length + vn.Length];
        Array.Copy(vars, newVars, vars.Length);
        Array.Copy(vn, 0, newVars, vars.Length, vn.Length);

        TermOrder newOrder = Tord.Extend(Nvar, vn.Length);
        return new GenPolynomialRing<C>(CoFac, Nvar + vn.Length, newOrder, newVars);
    }

    public GenPolynomialRing<C> Contract(int count)
    {
        string[]? contractedVars = null;
        if (vars != null)
        {
            contractedVars = new string[vars.Length - count];
            Array.Copy(vars, contractedVars, contractedVars.Length);
        }

        TermOrder newOrder = Tord.Contract(count, Nvar - count);
        return new GenPolynomialRing<C>(CoFac, Nvar - count, newOrder, contractedVars);
    }

    public GenPolynomialRing<GenPolynomial<C>> Recursive(int mainVariables)
    {
        if (mainVariables <= 0 || mainVariables >= Nvar)
        {
            throw new ArgumentException($"Invalid number of main variables: {mainVariables}.", nameof(mainVariables));
        }

        GenPolynomialRing<C> coefficientRing = Contract(mainVariables);
        string[]? recursiveVars = null;
        if (vars != null)
        {
            recursiveVars = new string[mainVariables];
            int index = 0;
            for (int i = Nvar - mainVariables; i < Nvar; i++)
            {
                recursiveVars[index++] = vars[i];
            }
        }

        TermOrder recursiveOrder = Tord.Contract(0, mainVariables);
        return new GenPolynomialRing<GenPolynomial<C>>(coefficientRing, mainVariables, recursiveOrder, recursiveVars);
    }

    public GenPolynomialRing<C> Reverse()
    {
        return Reverse(false);
    }

    public GenPolynomialRing<C> Reverse(bool partialReverse)
    {
        string[]? reversedVars = null;
        if (vars != null)
        {
            reversedVars = new string[vars.Length];
            if (partialReverse)
            {
                int split = Tord.GetSplit();
                if (split < vars.Length)
                {
                    for (int i = 0; i < split; i++)
                    {
                        reversedVars[vars.Length - split + i] = vars[vars.Length - 1 - i];
                    }

                    Array.Copy(vars, 0, reversedVars, 0, vars.Length - split);
                }
                else
                {
                    for (int i = 0; i < vars.Length; i++)
                    {
                        reversedVars[i] = vars[vars.Length - 1 - i];
                    }
                }
            }
            else
            {
                for (int i = 0; i < vars.Length; i++)
                {
                    reversedVars[i] = vars[vars.Length - 1 - i];
                }
            }
        }

        TermOrder newOrder = Tord.Reverse(partialReverse);
        GenPolynomialRing<C> ring = new GenPolynomialRing<C>(CoFac, Nvar, newOrder, reversedVars);
        ring.partial = partialReverse;
        return ring;
    }

    public PolynomialComparator<C> GetComparator()
    {
        return new PolynomialComparator<C>(Tord, false);
    }

    public static string[] NewVars(string prefix, int count)
    {
        ArgumentNullException.ThrowIfNull(prefix);
        if (count < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(count));
        }

        return NewVarsInternal(prefix, count);
    }

    public string[] NewVars(string prefix)
    {
        return NewVars(prefix, Nvar);
    }

    public static void AddVars(string[] variables)
    {
        if (variables == null)
        {
            return;
        }

        foreach (string variable in variables)
        {
            KnownVars.Add(variable);
        }
    }

    public IEnumerator<GenPolynomial<C>> GetEnumerator()
    {
        return Enumerable.Empty<GenPolynomial<C>>().GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    private static string[] NewVarsInternal(string prefix, int count)
    {
        string[] result = new string[count];
        int index = KnownVars.Count;
        int produced = 0;
        while (produced < count)
        {
            string name = prefix + index;
            if (KnownVars.Add(name))
            {
                result[produced++] = name;
            }

            index++;
        }

        return result;
    }
}
