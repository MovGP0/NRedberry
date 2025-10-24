namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;

/// <summary>
/// Term order class for ordered polynomials. Implements the most used term orders:
/// graded, lexicographical, weight array and block orders.
/// </summary>
/// <remarks>
/// Original Java file: cc.redberry.core.transformations.factor.jasfactor.edu.jas.poly.TermOrder
/// </remarks>
public sealed class TermOrder
{
    public const int LEX = 1;
    public const int INVLEX = 2;
    public const int GRLEX = 3;
    public const int IGRLEX = 4;
    public const int REVLEX = 5;
    public const int REVILEX = 6;
    public const int REVTDEG = 7;
    public const int REVITDG = 8;
    public const int DEFAULT_EVORD = IGRLEX;

    private readonly int evord;
    private readonly int evord2;
    private readonly int evbeg1;
    private readonly int evend1;
    private readonly int evbeg2;
    private readonly int evend2;
    private readonly long[][] weight;
    private readonly IComparer<ExpVector> horder;
    private readonly IComparer<ExpVector> lorder;

    public TermOrder()
    {
        throw new NotImplementedException();
    }

    public TermOrder(int evord)
    {
        throw new NotImplementedException();
    }

    public TermOrder(int evord, int n)
    {
        throw new NotImplementedException();
    }

    public TermOrder(long[][] w)
    {
        throw new NotImplementedException();
    }

    public int GetEvord() => evord;
    public int GetEvord2() => evord2;
    public long[][] GetWeight() => weight;
    public IComparer<ExpVector> GetDescendComparator() => horder;
    public IComparer<ExpVector> GetAscendComparator() => lorder;

    public override string ToString()
    {
        throw new NotImplementedException();
    }

    public override bool Equals(object? obj)
    {
        throw new NotImplementedException();
    }

    public override int GetHashCode()
    {
        throw new NotImplementedException();
    }
}
