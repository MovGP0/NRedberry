using System;
using System.Collections.Generic;
using System.Linq;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;

/// <summary>
/// List of polynomials with optimized variable order.
/// </summary>
/// <typeparam name="C">coefficient type</typeparam>
/// <remarks>
/// Original Java file: cc.redberry.core.transformations.factor.jasfactor.edu.jas.poly.OptimizedPolynomialList
/// </remarks>
public class OptimizedPolynomialList<C> : PolynomialList<C> where C : RingElem<C>
{
    public readonly List<int> Perm;

    public OptimizedPolynomialList(List<int> P, GenPolynomialRing<C> R, List<GenPolynomial<C>> L)
        : base(R, L)
    {
        Perm = P ?? throw new ArgumentNullException(nameof(P));
    }

    public override string ToString()
    {
        return "permutation = " + string.Join(", ", Perm) + Environment.NewLine + base.ToString();
    }

    public override bool Equals(object? B)
    {
        if (B is not OptimizedPolynomialList<C> other)
        {
            return false;
        }

        if (!base.Equals(other))
        {
            return false;
        }

        return Perm.SequenceEqual(other.Perm);
    }
}
