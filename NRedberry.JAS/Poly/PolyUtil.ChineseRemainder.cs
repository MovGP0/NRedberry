using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Arith;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;

public static partial class PolyUtil
{
    public static GenPolynomial<C> ChineseRemainder<C>(GenPolynomialRing<C> ring, GenPolynomial<C> first, C modulusInverse, GenPolynomial<C> second)
        where C : RingElem<C>, Modular
    {
        ArgumentNullException.ThrowIfNull(ring);
        ArgumentNullException.ThrowIfNull(first);
        ArgumentNullException.ThrowIfNull(modulusInverse);
        ArgumentNullException.ThrowIfNull(second);

        ModularRingFactory<C> modularFactory = (ModularRingFactory<C>)ring.CoFac;
        GenPolynomial<C> result = new (ring);
        GenPolynomial<C> firstCopy = first.Clone();

        SortedDictionary<ExpVector, C> firstTerms = firstCopy.Terms;
        SortedDictionary<ExpVector, C> secondTerms = second.Terms;
        SortedDictionary<ExpVector, C> resultTerms = result.Terms;

        C zeroFirst = first.Ring.CoFac.FromInteger(0);
        C zeroSecond = second.Ring.CoFac.FromInteger(0);

        foreach (KeyValuePair<ExpVector, C> term in secondTerms)
        {
            ExpVector exponent = term.Key;
            C secondCoefficient = term.Value;
            if (firstTerms.TryGetValue(exponent, out C firstCoefficient))
            {
                firstTerms.Remove(exponent);
                C coefficient = modularFactory.ChineseRemainder(firstCoefficient, modulusInverse, secondCoefficient);
                if (!coefficient.IsZero())
                {
                    resultTerms[exponent] = coefficient;
                }
            }
            else
            {
                C coefficient = modularFactory.ChineseRemainder(zeroFirst, modulusInverse, secondCoefficient);
                if (!coefficient.IsZero())
                {
                    resultTerms[exponent] = coefficient;
                }
            }
        }

        foreach (KeyValuePair<ExpVector, C> term in firstTerms)
        {
            C coefficient = modularFactory.ChineseRemainder(term.Value, modulusInverse, zeroSecond);
            if (!coefficient.IsZero())
            {
                resultTerms[term.Key] = coefficient;
            }
        }

        return result;
    }
}
