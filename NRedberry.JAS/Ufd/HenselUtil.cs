using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Arith;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Ufd;

/// <summary>
/// Hensel utilities for unique factorization domains (UFDs).
/// </summary>
/// <remarks>
/// Original Java file: cc.redberry.core.transformations.factor.jasfactor.edu.jas.ufd.HenselUtil
/// </remarks>
public static partial class HenselUtil
{
    private const bool Debug = false;

    internal static ModularRingFactory<MOD> CreateModularRingFactory<MOD>(BigInteger modulus, bool? isField = null)
        where MOD : GcdRingElem<MOD>, Modular
    {
        ArgumentNullException.ThrowIfNull(modulus);

        if (typeof(MOD) == typeof(ModInteger))
        {
            ModIntegerRing ring = isField switch
            {
                true => new ModIntegerRing(modulus, true),
                false => new ModIntegerRing(modulus, false),
                _ => new ModIntegerRing(modulus)
            };
            return (ModularRingFactory<MOD>)(object)ring;
        }

        if (typeof(MOD) == typeof(ModLong))
        {
            if (ModLongRing.MaxLong.CompareTo(modulus.Val) <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(modulus), modulus, "modul too large for ModLong");
            }

            ModLongRing ring = isField switch
            {
                true => new ModLongRing(modulus.Val, true),
                false => new ModLongRing(modulus.Val, false),
                _ => new ModLongRing(modulus.Val)
            };
            return (ModularRingFactory<MOD>)(object)ring;
        }

        throw new NotSupportedException($"Unsupported modular coefficient type '{typeof(MOD).FullName}'.");
    }

    internal static GenPolynomialRing<BigInteger> CreateIntegerPolynomialRing<C>(GenPolynomialRing<C> template)
        where C : RingElem<C>
    {
        ArgumentNullException.ThrowIfNull(template);
        return new GenPolynomialRing<BigInteger>(
            new BigInteger(),
            template.Nvar,
            template.Tord,
            template.GetVars());
    }

    internal static GenPolynomialRing<T> CreatePolynomialRingFromTemplate<T, C>(
        RingFactory<T> coefficientFactory,
        GenPolynomialRing<C> template)
        where T : RingElem<T>
        where C : RingElem<C>
    {
        ArgumentNullException.ThrowIfNull(coefficientFactory);
        ArgumentNullException.ThrowIfNull(template);
        return new GenPolynomialRing<T>(
            coefficientFactory,
            template.Nvar,
            template.Tord,
            template.GetVars());
    }
}
