using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;

/// <summary>
/// ExpVectorInteger implements exponent vectors for polynomials using arrays of int as storage unit.
/// This class is used by ExpVector internally, there is no need to use this class directly.
/// </summary>
/// <remarks>
/// Original Java file: cc.redberry.core.transformations.factor.jasfactor.edu.jas.poly.ExpVectorInteger
/// </remarks>
public sealed class ExpVectorInteger : ExpVector
{
    public static readonly long MaxInt = int.MaxValue / 2;
    public static readonly long MinInt = int.MinValue / 2;

    internal readonly int[] val;

    public ExpVectorInteger(int n) : this(new int[n]) { }

    public ExpVectorInteger(int n, int i, long e) : this(new int[n])
    {
        val[i] = (int)e;
    }

    public ExpVectorInteger(long[] v)
    {
        if (v == null) throw new ArgumentNullException(nameof(v));
        val = new int[v.Length];
        for (int i = 0; i < v.Length; i++)
        {
            val[i] = (int)v[i];
        }
    }

    public ExpVectorInteger(int[] v)
    {
        if (v == null) throw new ArgumentNullException(nameof(v));
        val = v;
    }

    public override ExpVector Copy()
    {
        int[] w = new int[val.Length];
        Array.Copy(val, 0, w, 0, val.Length);
        return new ExpVectorInteger(w);
    }

    public override long[] GetVal()
    {
        long[] result = new long[val.Length];
        for (int i = 0; i < val.Length; i++)
        {
            result[i] = val[i];
        }
        return result;
    }

    public override long GetVal(int i) => val[i];
    public override int Length() => val.Length;

    public override ExpVector Extend(int n, int i, long e) { throw new NotImplementedException(); }
    public override ExpVector Abs() { throw new NotImplementedException(); }
    public override ExpVector Negate() { throw new NotImplementedException(); }
    public override int Signum() { throw new NotImplementedException(); }
    public override ExpVector Subtract(ExpVector V) { throw new NotImplementedException(); }
    public override ExpVector Sum(ExpVector V) { throw new NotImplementedException(); }
    public override bool IsZERO() { throw new NotImplementedException(); }
    public override int CompareTo(ExpVector? v) { throw new NotImplementedException(); }
    public override bool Equals(object? B) { throw new NotImplementedException(); }
    public override int GetHashCode() { throw new NotImplementedException(); }
    public override AbelianGroupFactory<ExpVector> Factory() { throw new NotImplementedException(); }
}
