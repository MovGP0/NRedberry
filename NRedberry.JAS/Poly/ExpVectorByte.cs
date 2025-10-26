using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;

/// <summary>
/// ExpVectorByte implements exponent vectors for polynomials using arrays of byte as storage unit.
/// This class is used by ExpVector internally, there is no need to use this class directly.
/// </summary>
/// <remarks>
/// Original Java file: cc.redberry.core.transformations.factor.jasfactor.edu.jas.poly.ExpVectorByte
/// </remarks>
public sealed class ExpVectorByte : ExpVector
{
    public static readonly long MaxByte = sbyte.MaxValue / 2;
    public static readonly long MinByte = sbyte.MinValue / 2;

    internal readonly sbyte[] val;

    public ExpVectorByte(int n) : this(new sbyte[n]) { }

    public ExpVectorByte(int n, int i, long e) : this(new sbyte[n])
    {
        val[i] = (sbyte)e;
    }

    public ExpVectorByte(long[] v)
    {
        if (v == null) throw new ArgumentNullException(nameof(v));
        val = new sbyte[v.Length];
        for (int i = 0; i < v.Length; i++)
        {
            val[i] = (sbyte)v[i];
        }
    }

    public ExpVectorByte(sbyte[] v)
    {
        if (v == null) throw new ArgumentNullException(nameof(v));
        val = v;
    }

    public override ExpVector Clone()
    {
        sbyte[] w = new sbyte[val.Length];
        Array.Copy(val, 0, w, 0, val.Length);
        return new ExpVectorByte(w);
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
    public override bool IsZero() { throw new NotImplementedException(); }
    public override int CompareTo(ExpVector? v) { throw new NotImplementedException(); }
    public override bool Equals(object? B) { throw new NotImplementedException(); }
    public override int GetHashCode() { throw new NotImplementedException(); }
    public override AbelianGroupFactory<ExpVector> Factory() { throw new NotImplementedException(); }
}
