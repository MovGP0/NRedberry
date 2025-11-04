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

    private GenSolvablePolynomial<C> FromBase(GenPolynomial<C> polynomial)
    {
        if (polynomial is GenSolvablePolynomial<C> solvable && solvable.Ring.Equals(Ring))
        {
            return solvable;
        }

        return new GenSolvablePolynomial<C>(Ring, polynomial.CloneTerms(), copy: false);
    }

    public new GenSolvablePolynomialRing<C> Factory()
    {
        return Ring;
    }

    public GenSolvablePolynomial<C> Copy()
    {
        return new GenSolvablePolynomial<C>(Ring, CloneTerms(), copy: false);
    }

    public GenSolvablePolynomial<C> Sum(GenSolvablePolynomial<C> other)
    {
        ArgumentNullException.ThrowIfNull(other);
        return FromBase(base.Sum(other));
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
        ArgumentNullException.ThrowIfNull(other);
        if (other.IsZero())
        {
            return GenSolvablePolynomialRing<C>.Zero.Copy();
        }

        if (IsZero())
        {
            return this;
        }

        GenSolvablePolynomial<C> result = GenSolvablePolynomialRing<C>.Zero.Copy();
        C unit = Ring.GetOneCoefficient();
        ExpVector zeroVector = Ring.Evzero;
        foreach (KeyValuePair<ExpVector, C> leftTerm in Terms)
        {
            ExpVector e = leftTerm.Key;
            C a = leftTerm.Value;
            int[] leftDependencies = e.DependencyOnVariables();
            int leftFirst = leftDependencies.Length > 0 ? leftDependencies[0] : Ring.Nvar + 1;
            int leftShift = Ring.Nvar + 1 - leftFirst;
            foreach (KeyValuePair<ExpVector, C> rightTerm in other.Terms)
            {
                ExpVector f = rightTerm.Key;
                C b = rightTerm.Value;
                int[] rightDependencies = f.DependencyOnVariables();
                int rightLast = rightDependencies.Length > 0 ? rightDependencies[^1] : 0;
                int rightShift = Ring.Nvar + 1 - rightLast;
                GenSolvablePolynomial<C> product;
                if (leftShift <= rightShift)
                {
                    ExpVector g = e.Sum(f);
                    product = new GenSolvablePolynomial<C>(Ring, unit, g);
                }
                else
                {
                    ExpVector e1 = e.Subst(leftFirst, 0);
                    ExpVector e2 = zeroVector.Subst(leftFirst, e.GetVal(leftFirst));
                    ExpVector f1 = f.Subst(rightLast, 0);
                    ExpVector f2 = zeroVector.Subst(rightLast, f.GetVal(rightLast));
                    TableRelation<C> relation = Ring.Table.Lookup(e2, f2);
                    product = relation.Product;
                    if (relation.Right is not null)
                    {
                        GenSolvablePolynomial<C> c2 = new (Ring, unit, relation.Right);
                        product = product.Multiply(c2);
                        ExpVector e4 = relation.Left is null ? e2 : e2.Subtract(relation.Left);
                        Ring.Table.Update(e4, f2, product);
                    }

                    if (relation.Left is not null)
                    {
                        GenSolvablePolynomial<C> c1 = new (Ring, unit, relation.Left);
                        product = c1.Multiply(product);
                        Ring.Table.Update(e2, f2, product);
                    }

                    if (!f1.IsZero())
                    {
                        GenSolvablePolynomial<C> c2 = new (Ring, unit, f1);
                        product = product.Multiply(c2);
                    }

                    if (!e1.IsZero())
                    {
                        GenSolvablePolynomial<C> c1 = new (Ring, unit, e1);
                        product = c1.Multiply(product);
                    }
                }

                product = product.Multiply(a, b);
                result = result.Sum(product);
            }
        }

        return result;
    }

    public GenSolvablePolynomial<C> Multiply(GenSolvablePolynomial<C> left, GenSolvablePolynomial<C> right)
    {
        ArgumentNullException.ThrowIfNull(left);
        ArgumentNullException.ThrowIfNull(right);
        if (left.IsZero() || right.IsZero() || IsZero())
        {
            return GenSolvablePolynomialRing<C>.Zero.Copy();
        }

        if (left.IsOne())
        {
            return Multiply(right);
        }

        if (right.IsOne())
        {
            return left.Multiply(this);
        }

        return left.Multiply(this).Multiply(right);
    }

    public new GenSolvablePolynomial<C> Multiply(C coefficient)
    {
        ArgumentNullException.ThrowIfNull(coefficient);
        return FromBase(base.Multiply(coefficient));
    }

    public GenSolvablePolynomial<C> Multiply(C left, C right)
    {
        ArgumentNullException.ThrowIfNull(left);
        ArgumentNullException.ThrowIfNull(right);
        return Multiply(left).Multiply(right);
    }

    public new GenSolvablePolynomial<C> Multiply(ExpVector exponent)
    {
        ArgumentNullException.ThrowIfNull(exponent);
        if (exponent.IsZero())
        {
            return this;
        }

        GenSolvablePolynomial<C> monomial = new (Ring, Ring.GetOneCoefficient(), exponent);
        return Multiply(monomial);
    }

    public new GenSolvablePolynomial<C> Multiply(C coefficient, ExpVector exponent)
    {
        ArgumentNullException.ThrowIfNull(coefficient);
        ArgumentNullException.ThrowIfNull(exponent);
        if (coefficient.IsZero())
        {
            return GenSolvablePolynomialRing<C>.Zero.Copy();
        }

        GenSolvablePolynomial<C> monomial = new (Ring, coefficient, exponent);
        return Multiply(monomial);
    }

    public GenSolvablePolynomial<C> Multiply(ExpVector leftExponent, ExpVector rightExponent)
    {
        ArgumentNullException.ThrowIfNull(leftExponent);
        ArgumentNullException.ThrowIfNull(rightExponent);
        if (leftExponent.IsZero())
        {
            return Multiply(rightExponent);
        }

        if (rightExponent.IsZero())
        {
            return Multiply(leftExponent);
        }

        C unit = Ring.GetOneCoefficient();
        return Multiply(unit, leftExponent, unit, rightExponent);
    }

    public GenSolvablePolynomial<C> Multiply(C leftCoefficient, ExpVector leftExponent, C rightCoefficient, ExpVector rightExponent)
    {
        ArgumentNullException.ThrowIfNull(leftCoefficient);
        ArgumentNullException.ThrowIfNull(rightCoefficient);
        ArgumentNullException.ThrowIfNull(leftExponent);
        ArgumentNullException.ThrowIfNull(rightExponent);
        if (leftCoefficient.IsZero() || rightCoefficient.IsZero())
        {
            return GenSolvablePolynomialRing<C>.Zero.Copy();
        }

        GenSolvablePolynomial<C> left = new (Ring, leftCoefficient, leftExponent);
        GenSolvablePolynomial<C> right = new (Ring, rightCoefficient, rightExponent);
        return Multiply(left, right);
    }
}
