using System;
using System.Collections.Generic;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;

/// <summary>
/// GenSolvablePolynomial generic solvable polynomials implementing RingElem.
/// n-variate ordered solvable polynomials over C. Objects of this class are intended to be immutable.
/// </summary>
/// <typeparam name="C">coefficient type</typeparam>
/// <remarks>
/// Original Java file: cc.redberry.core.transformations.factor.jasfactor.edu.jas.poly.GenSolvablePolynomial
/// </remarks>
public class GenSolvablePolynomial<C> : GenPolynomial<C> where C : RingElem<C>
{
    public new GenSolvablePolynomialRing<C> Ring { get; }

    public GenSolvablePolynomial(GenSolvablePolynomialRing<C> ring)
        : base(ring)
    {
        Ring = ring ?? throw new ArgumentNullException(nameof(ring));
    }

    public GenSolvablePolynomial(GenSolvablePolynomialRing<C> ring, C coefficient, ExpVector exponent)
        : base(ring, coefficient, exponent)
    {
        Ring = ring ?? throw new ArgumentNullException(nameof(ring));
    }

    internal GenSolvablePolynomial(GenSolvablePolynomialRing<C> ring, IDictionary<ExpVector, C> terms, bool copy = true)
        : base(ring, terms, copy)
    {
        Ring = ring ?? throw new ArgumentNullException(nameof(ring));
    }

    public new GenSolvablePolynomialRing<C> Factory()
    {
        return Ring;
    }

    public new GenSolvablePolynomial<C> Copy()
    {
        return new GenSolvablePolynomial<C>(Ring, CloneTerms(), copy: false);
    }

    public override bool Equals(object? obj)
    {
        if (obj is not GenSolvablePolynomial<C>)
        {
            return false;
        }

        return base.Equals(obj);
    }

    public GenSolvablePolynomial<C> Multiply(GenSolvablePolynomial<C> other)
    {
        throw new NotImplementedException("Solvable polynomial multiplication pending port.");
    }

    public GenSolvablePolynomial<C> Multiply(GenSolvablePolynomial<C> left, GenSolvablePolynomial<C> right)
    {
        throw new NotImplementedException("Solvable polynomial multiplication pending port.");
    }

    public new GenSolvablePolynomial<C> Multiply(C coefficient)
    {
        ArgumentNullException.ThrowIfNull(coefficient);
        GenPolynomial<C> product = base.Multiply(coefficient);
        return new GenSolvablePolynomial<C>(Ring, product.Terms, copy: false);
    }

    public GenSolvablePolynomial<C> Multiply(C left, C right)
    {
        ArgumentNullException.ThrowIfNull(left);
        ArgumentNullException.ThrowIfNull(right);
        return Multiply(left).Multiply(right);
    }
}
