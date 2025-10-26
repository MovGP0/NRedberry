using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;

/// <summary>
/// ExpVector implements exponent vectors for polynomials. 
/// Objects of this class are intended to be immutable. The different storage unit implementations are
/// ExpVectorLong, ExpVectorInteger, ExpVectorShort and ExpVectorByte.
/// </summary>
/// <remarks>
/// Original Java file: cc.redberry.core.transformations.factor.jasfactor.edu.jas.poly.ExpVector
/// </remarks>
public abstract class ExpVector : AbelianGroupElem<ExpVector>
{
    protected int hash = 0;
    private static readonly Random random = new();

    public enum StorUnit
    {
        LONG, INT, SHORT, BYTE
    }

    public static readonly StorUnit Storunit = StorUnit.LONG;

    public ExpVector()
    {
        hash = 0;
    }

    public static ExpVector Create(int n) { throw new NotImplementedException(); }
    public static ExpVector Create(int n, int i, long e) { throw new NotImplementedException(); }
    public static ExpVector Create(long[] v) { throw new NotImplementedException(); }
    public static ExpVector Create(ICollection<long> v) { throw new NotImplementedException(); }
    
    public abstract AbelianGroupFactory<ExpVector> Factory();
    public abstract ExpVector Clone();
    public abstract long GetVal(int i);
    public abstract long[] GetVal();
    public abstract int Length();
    public abstract ExpVector Extend(int n, int i, long e);
    public abstract ExpVector Abs();
    public abstract ExpVector Negate();
    public abstract int Signum();
    public abstract ExpVector Subtract(ExpVector V);
    public abstract ExpVector Sum(ExpVector V);
    public abstract bool IsZero();
    public abstract int CompareTo(ExpVector? v);
    public abstract override bool Equals(object? B);
    public abstract override int GetHashCode();

    ElemFactory<ExpVector> Element<ExpVector>.Factory() => Factory();
}
