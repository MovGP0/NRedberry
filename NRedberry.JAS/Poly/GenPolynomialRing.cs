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

    public RingFactory<C> CoFac { get; }
    public int Nvar { get; }
    public TermOrder Tord { get; }

    protected bool partial;

    public bool IsPartial => partial;

    protected string[]? vars;
    private static GenPolynomial<C> ZERO;
    private static GenPolynomial<C> ONE;
    public readonly ExpVector Evzero;
    protected static readonly Random random = new();
    protected int isField = -1;

    /// <summary>
    /// Creates a polynomial factory with the default term order and <paramref name="n"/> variables.
    /// </summary>
    /// <param name="cf">Coefficient factory.</param>
    /// <param name="n">Number of variables.</param>
    public GenPolynomialRing(RingFactory<C> cf, int n)
        : this(cf, n, new TermOrder(), null)
    {
    }

    /// <summary>
    /// Creates a polynomial factory with the supplied term order.
    /// </summary>
    /// <param name="cf">Coefficient factory.</param>
    /// <param name="n">Number of variables.</param>
    /// <param name="t">Term order.</param>
    public GenPolynomialRing(RingFactory<C> cf, int n, TermOrder t)
        : this(cf, n, t, null)
    {
    }

    /// <summary>
    /// Creates a polynomial factory with named variables.
    /// </summary>
    /// <param name="cf">Coefficient factory.</param>
    /// <param name="v">Variable names.</param>
    public GenPolynomialRing(RingFactory<C> cf, string[] v)
        : this(cf, v?.Length ?? throw new ArgumentNullException(nameof(v)), v)
    {
    }

    /// <summary>
    /// Creates a polynomial factory with explicit variable count and names.
    /// </summary>
    public GenPolynomialRing(RingFactory<C> cf, int n, string[] v)
        : this(cf, n, new TermOrder(), v)
    {
    }

    /// <summary>
    /// Creates a polynomial factory with the specified term order and optional names.
    /// </summary>
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

    /// <summary>
    /// Creates a new ring by copying another polynomial product factory.
    /// </summary>
    public GenPolynomialRing(RingFactory<C> cf, GenPolynomialRing<C> other)
        : this(cf, other?.Nvar ?? throw new ArgumentNullException(nameof(other)), other.Tord, other.vars)
    {
    }

    /// <summary>
    /// Constructs a copy with a new term order.
    /// </summary>
    public GenPolynomialRing(GenPolynomialRing<C> other, TermOrder to)
        : this(other?.CoFac ?? throw new ArgumentNullException(nameof(other)), other.Nvar, to, other.vars)
    {
    }

    /// <summary>
    /// Creates a ring for coefficient factory <paramref name="cf"/> from an algebraic number ring.
    /// </summary>
    public GenPolynomialRing(RingFactory<C> cf, GenPolynomialRing<AlgebraicNumber<C>> other)
        : this(cf, other?.Nvar ?? throw new ArgumentNullException(nameof(other)), other.Tord, other.vars)
    {
    }

    /// <summary>
    /// Returns a shallow clone of this polynomial factory.
    /// </summary>
    public GenPolynomialRing<C> Copy()
    {
        return new GenPolynomialRing<C>(CoFac, this);
    }

    /// <summary>
    /// Formats the factory including coefficient ring, variables, and term order.
    /// </summary>
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

    /// <summary>
    /// Equality compares variable count, coefficient factory, term order, and variable names.
    /// </summary>
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

    /// <summary>
    /// Computes a hash code from the coefficient factory, term order, variables, and caching flag.
    /// </summary>
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

    /// <summary>
    /// Returns the names of the variables in this ring.
    /// </summary>
    public string[]? GetVars()
    {
        return vars?.ToArray();
    }

    /// <summary>
    /// Sets new variable names for the ring.
    /// </summary>
    /// <param name="v">New names (must match <see cref="Nvar"/>).</param>
    public string[] SetVars(string[] v)
    {
        ArgumentNullException.ThrowIfNull(v);
        if (v.Length != Nvar)
        {
            throw new ArgumentException($"Variable array does not match number of variables: {v.Length} vs {Nvar}", nameof(v));
        }

        string[] previous = vars ?? [];
        vars = v.ToArray();
        AddVars(vars);
        return previous;
    }

    /// <summary>
    /// Formats the currently assigned variable names.
    /// </summary>
    public string VarsToString()
    {
        if (vars == null)
        {
            return "#" + Nvar;
        }

        return ExpVector.VarsToString(vars);
    }

    /// <summary>
    /// Creates the constant polynomial with coefficient <paramref name="a"/>.
    /// </summary>
    public GenPolynomial<C> ValueOf(C a)
    {
        ArgumentNullException.ThrowIfNull(a);
        return new GenPolynomial<C>(this, a);
    }

    /// <summary>
    /// Creates a monomial for the given exponent vector.
    /// </summary>
    public GenPolynomial<C> ValueOf(ExpVector e)
    {
        ArgumentNullException.ThrowIfNull(e);
        return new GenPolynomial<C>(this, CoFac.FromInteger(1), e);
    }

    /// <summary>
    /// Creates the monomial <c>a * x^e</c>.
    /// </summary>
    public GenPolynomial<C> ValueOf(C a, ExpVector e)
    {
        ArgumentNullException.ThrowIfNull(a);
        ArgumentNullException.ThrowIfNull(e);
        return new GenPolynomial<C>(this, a, e);
    }

    /// <summary>
    /// Embeds a 64-bit integer constant into the polynomial ring.
    /// </summary>
    public GenPolynomial<C> FromInteger(long a)
    {
        return new GenPolynomial<C>(this, CoFac.FromInteger(a), Evzero);
    }

    /// <summary>
    /// Embeds a <see cref="BigInteger"/> constant into the polynomial ring.
    /// </summary>
    public GenPolynomial<C> FromInteger(BigInteger a)
    {
        return new GenPolynomial<C>(this, CoFac.FromInteger(a), Evzero);
    }

    /// <summary>
    /// Creates a random polynomial with the default bit length.
    /// </summary>
    public GenPolynomial<C> Random()
    {
        return Random(3, 4, 3, 0.5f);
    }

    /// <summary>
    /// Creates a random polynomial with the specified degree bound.
    /// </summary>
    public GenPolynomial<C> Random(int n)
    {
        return Random(n, random);
    }

    /// <summary>
    /// Creates a random polynomial using the provided RNG.
    /// </summary>
    public GenPolynomial<C> Random(int n, Random rnd)
    {
        ArgumentNullException.ThrowIfNull(rnd);
        if (Nvar == 1)
        {
            return Random(3, n, n, 0.7f, rnd);
        }

        return Random(5, n, 3, 0.3f, rnd);
    }

    /// <summary>
    /// Generates a random polynomial with controlled sparsity/density.
    /// </summary>
    public GenPolynomial<C> Random(int k, int l, int d, float q)
    {
        return Random(k, l, d, q, random);
    }

    /// <summary>
    /// Generates a random polynomial with the supplied RNG.
    /// </summary>
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

    /// <summary>
    /// Copies the provided polynomial while keeping the same ring context.
    /// </summary>
    public GenPolynomial<C> Copy(GenPolynomial<C> polynomial)
    {
        ArgumentNullException.ThrowIfNull(polynomial);
        return new GenPolynomial<C>(this, polynomial.Terms);
    }

    /// <summary>
    /// Returns the univariate generator for the specified index.
    /// </summary>
    public GenPolynomial<C> Univariate(int index)
    {
        return Univariate(0, index, 1L);
    }

    /// <summary>
    /// Returns the monomial x<sub>index</sub><sup>exponent</sup>.
    /// </summary>
    public GenPolynomial<C> Univariate(int index, long exponent)
    {
        return Univariate(0, index, exponent);
    }

    /// <summary>
    /// Returns the monomial after embedding into additional module variables.
    /// </summary>
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

    /// <summary>
    /// Returns the generating polynomials for this ring.
    /// </summary>
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

    /// <summary>
    /// Determines whether the coefficient factory yields a finite ring.
    /// </summary>
    public bool IsFinite()
    {
        return Nvar == 0 && CoFac.IsFinite();
    }

    /// <summary>
    /// Queries whether the coefficient factory is commutative.
    /// </summary>
    public bool IsCommutative()
    {
        return CoFac.IsCommutative();
    }

    /// <summary>
    /// Queries whether the coefficient factory is associative.
    /// </summary>
    public bool IsAssociative()
    {
        return CoFac.IsAssociative();
    }

    /// <summary>
    /// Zero polynomial singleton for this ring.
    /// </summary>
    public static GenPolynomial<C> Zero => ZERO;

    /// <summary>
    /// One polynomial singleton for this ring.
    /// </summary>
    public static GenPolynomial<C> One => ONE;

    /// <summary>
    /// Returns the additive identity from the coefficient factory.
    /// </summary>
    public C GetZeroCoefficient()
    {
        return CoFac.FromInteger(0);
    }

    /// <summary>
    /// Returns the multiplicative identity from the coefficient factory.
    /// </summary>
    public C GetOneCoefficient()
    {
        return CoFac.FromInteger(1);
    }

    /// <summary>
    /// Clones a polynomial while keeping this ring context.
    /// </summary>
    public static GenPolynomial<C> Clone(GenPolynomial<C> polynomial)
    {
        ArgumentNullException.ThrowIfNull(polynomial);
        return polynomial.Clone();
    }

    /// <summary>
    /// Determines whether this polynomial ring forms a field.
    /// </summary>
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

    /// <summary>
    /// Returns the characteristic of the coefficient factory.
    /// </summary>
    public BigInteger Characteristic()
    {
        return CoFac.Characteristic();
    }

    /// <summary>
    /// Returns univariate generators for each variable.
    /// </summary>
    public List<GenPolynomial<C>> UnivariateList()
    {
        return UnivariateList(0, 1L).ToList();
    }

    /// <summary>
    /// Returns a list of univariate generators for extended module variables.
    /// </summary>
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

    /// <summary>
    /// Adds <paramref name="count"/> new variables at the front of the ring.
    /// </summary>
    public GenPolynomialRing<C> Extend(int count)
    {
        string[] newVariables = NewVars("e", count);
        return Extend(newVariables);
    }

    /// <summary>
    /// Adds new variables with the supplied names.
    /// </summary>
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

    /// <summary>
    /// Removes <paramref name="count"/> leading variables from the ring.
    /// </summary>
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

    /// <summary>
    /// Constructs a recursive polynomial ring nested <paramref name="mainVariables"/> times.
    /// </summary>
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

    /// <summary>
    /// Reverses the variable order completely.
    /// </summary>
    public GenPolynomialRing<C> Reverse()
    {
        return Reverse(false);
    }

    /// <summary>
    /// Reverses variables depending on <paramref name="partialReverse"/>.
    /// </summary>
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

    /// <summary>
    /// Returns the comparator tied to the ring's term order.
    /// </summary>
    public PolynomialComparator<C> GetComparator()
    {
        return new PolynomialComparator<C>(Tord, false);
    }

    /// <summary>
    /// Generates a fresh set of variable names using the given prefix.
    /// </summary>
    public static string[] NewVars(string prefix, int count)
    {
        ArgumentNullException.ThrowIfNull(prefix);
        if (count < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(count));
        }

        return NewVarsInternal(prefix, count);
    }

    /// <summary>
    /// Generates new variable names for this ring, starting from <paramref name="prefix"/>.
    /// </summary>
    public string[] NewVars(string prefix)
    {
        return NewVars(prefix, Nvar);
    }

    /// <summary>
    /// Records the variable names globally so they stay unique.
    /// </summary>
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

    /// <summary>
    /// Enumerates polynomials (implements IEnumerable).
    /// </summary>
    public IEnumerator<GenPolynomial<C>> GetEnumerator()
    {
        return Enumerable.Empty<GenPolynomial<C>>().GetEnumerator();
    }

    /// <summary>
    /// Explicit interface implementation forwarding to <see cref="GetEnumerator"/>.
    /// </summary>
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
