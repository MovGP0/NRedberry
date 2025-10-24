using System.Numerics;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;

/// <summary>
/// Power class to compute powers of RingElem.
/// </summary>
/// <typeparam name="C">ring element type</typeparam>
/// <remarks>
/// Original Java file: cc.redberry.core.transformations.factor.jasfactor.edu.jas.structure.Power
/// </remarks>
public class Power<C> where C : RingElem<C>
{
    private readonly RingFactory<C>? fac;

    public Power()
    {
        this.fac = null;
    }

    public Power(RingFactory<C>? fac)
    {
        this.fac = fac;
    }

    public static C PositivePower(C a, long n)
    {
        throw new NotImplementedException();
    }

    public static C PositivePower(C a, BigInteger n)
    {
        throw new NotImplementedException();
    }

    public static C PowerMethod(RingFactory<C> fac, C a, long n)
    {
        throw new NotImplementedException();
    }

    public static C PowerMethod<TMonoid>(MonoidFactory<TMonoid> fac, TMonoid a, long n) where TMonoid : MonoidElem<TMonoid>
    {
        throw new NotImplementedException();
    }

    public static C ModPower<TMonoid>(MonoidFactory<TMonoid> fac, TMonoid a, BigInteger n, TMonoid m) where TMonoid : MonoidElem<TMonoid>
    {
        throw new NotImplementedException();
    }

    public C PowerMethod(C a, long n)
    {
        throw new NotImplementedException();
    }

    public C ModPower(C a, BigInteger n, C m)
    {
        throw new NotImplementedException();
    }

    public static long Logarithm(C p, C a)
    {
        throw new NotImplementedException();
    }

    public static C Multiply(RingFactory<C> fac, List<C> A)
    {
        throw new NotImplementedException();
    }

    public static C Multiply<TMonoid>(MonoidFactory<TMonoid> fac, List<TMonoid> A) where TMonoid : MonoidElem<TMonoid>
    {
        throw new NotImplementedException();
    }

    public static C Sum(RingFactory<C> fac, List<C> A)
    {
        throw new NotImplementedException();
    }

    public static C Sum<TAbelian>(AbelianGroupFactory<TAbelian> fac, List<TAbelian> A) where TAbelian : AbelianGroupElem<TAbelian>
    {
        throw new NotImplementedException();
    }
}
