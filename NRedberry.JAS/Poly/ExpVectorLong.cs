using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;

/// <summary>
/// ExpVectorLong implements exponent vectors for polynomials using arrays of long as storage unit.
/// This class is used by ExpVector internally, there is no need to use this class directly.
/// </summary>
/// <remarks>
/// Original Java file: cc.redberry.core.transformations.factor.jasfactor.edu.jas.poly.ExpVectorLong
/// </remarks>
public sealed class ExpVectorLong : ExpVector
{
    internal readonly long[] val;

    public ExpVectorLong(int n) : this(new long[n]) { }

    public ExpVectorLong(int n, int i, long e) : this(new long[n])
    {
        val[i] = e;
    }

    public ExpVectorLong(long[] v)
    {
        if (v == null) throw new ArgumentNullException(nameof(v));
        val = v;
    }

    public override ExpVector Clone()
    {
        long[] w = new long[val.Length];
        Array.Copy(val, 0, w, 0, val.Length);
        return new ExpVectorLong(w);
    }

    public override long[] GetVal() => val;
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
