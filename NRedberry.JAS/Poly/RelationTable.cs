using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;

/// <summary>
/// RelationTable for solvable polynomials. This class maintains the non-commutative multiplication relations
/// of solvable polynomial rings. The table entries are initialized with relations of the form
/// x_j * x_i = p_ij. During multiplication the relations are updated by relations of the form
/// x_j^k * x_i^l = p_ijkl.
/// </summary>
/// <typeparam name="C">coefficient type</typeparam>
/// <remarks>
/// Original Java file: cc.redberry.core.transformations.factor.jasfactor.edu.jas.poly.RelationTable
/// </remarks>
public class RelationTable<C> where C : RingElem<C>
{
    public readonly Dictionary<List<int>, List<object>> Table;
    public readonly GenSolvablePolynomialRing<C> Ring;

    protected RelationTable(GenSolvablePolynomialRing<C> r)
    {
        Table = new Dictionary<List<int>, List<object>>();
        Ring = r ?? throw new ArgumentException("RelationTable no ring");
    }

    public override bool Equals(object? p)
    {
        throw new NotImplementedException();
    }

    public override int GetHashCode()
    {
        throw new NotImplementedException();
    }

    public override string ToString()
    {
        throw new NotImplementedException();
    }

    public string ToString(string[] vars)
    {
        throw new NotImplementedException();
    }

    public void Update(ExpVector e, ExpVector f, GenSolvablePolynomial<C> p)
    {
        throw new NotImplementedException();
    }

    public void Update(GenPolynomial<C> E, GenPolynomial<C> F, GenSolvablePolynomial<C> p)
    {
        throw new NotImplementedException();
    }

    public object Lookup(ExpVector e, ExpVector f)
    {
        throw new NotImplementedException();
    }

    public int Size()
    {
        throw new NotImplementedException();
    }
}
