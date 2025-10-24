using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;

/// <summary>
/// ExpVectorShort implements exponent vectors for polynomials using arrays of short as storage unit.
/// This class is used by ExpVector internally, there is no need to use this class directly.
/// </summary>
/// <remarks>
/// Original Java file: cc.redberry.core.transformations.factor.jasfactor.edu.jas.poly.ExpVectorShort
/// </remarks>
public sealed class ExpVectorShort : ExpVector
{
    public static readonly long MaxShort = short.MaxValue / 2;
    public static readonly long MinShort = short.MinValue / 2;

    internal readonly short[] val;

    public ExpVectorShort(int n) : this(new short[n]) { }

    public ExpVectorShort(int n, int i, long e) : this(new short[n])
    {
        val[i] = (short)e;
    }

    public ExpVectorShort(long[] v)
    {
        if (v == null) throw new ArgumentNullException(nameof(v));
        val = new short[v.Length];
        for (int i = 0; i < v.Length; i++)
        {
            val[i] = (short)v[i];
        }
    }

    public ExpVectorShort(short[] v)
    {
        if (v == null) throw new ArgumentNullException(nameof(v));
        val = v;
    }

    public override ExpVector Copy()
    {
        short[] w = new short[val.Length];
        Array.Copy(val, 0, w, 0, val.Length);
        return new ExpVectorShort(w);
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
