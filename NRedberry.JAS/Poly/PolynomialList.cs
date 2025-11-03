using System;
using System.Collections.Generic;
using System.Text;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;

/// <summary>
/// List of polynomials with their polynomial ring factory.
/// </summary>
/// <typeparam name="C">coefficient type</typeparam>
/// <remarks>
/// Original Java file: cc.redberry.core.transformations.factor.jasfactor.edu.jas.poly.PolynomialList
/// </remarks>
public class PolynomialList<C> : IComparable<PolynomialList<C>> where C : RingElem<C>
{
    public readonly GenPolynomialRing<C> Ring;
    public readonly List<GenPolynomial<C>> Polynomials;
    
    public PolynomialList(GenPolynomialRing<C> r, List<GenPolynomial<C>> l)
    {
        Ring = r ?? throw new ArgumentNullException(nameof(r));
        Polynomials = l ?? throw new ArgumentNullException(nameof(l));
    }

    public PolynomialList<C> Copy()
    {
        return new PolynomialList<C>(Ring, new List<GenPolynomial<C>>(Polynomials));
    }

    public override bool Equals(object? other)
    {
        if (ReferenceEquals(this, other))
        {
            return true;
        }

        if (other is not PolynomialList<C> list)
        {
            return false;
        }

        if (!Ring.Equals(list.Ring))
        {
            return false;
        }

        return CompareTo(list) == 0;
    }

    public int CompareTo(PolynomialList<C>? other)
    {
        if (other is null)
        {
            return 1;
        }

        List<GenPolynomial<C>> left = OrderedPolynomialList<C>.Sort(Ring, Polynomials);
        List<GenPolynomial<C>> right = OrderedPolynomialList<C>.Sort(Ring, other.Polynomials);
        int limit = Math.Min(left.Count, right.Count);
        for (int i = 0; i < limit; i++)
        {
            int comparison = left[i].CompareTo(right[i]);
            if (comparison != 0)
            {
                return comparison;
            }
        }

        if (left.Count > limit)
        {
            return 1;
        }

        if (right.Count > limit)
        {
            return -1;
        }

        return 0;
    }

    public override int GetHashCode()
    {
        HashCode hash = new ();
        hash.Add(Ring);
        foreach (GenPolynomial<C> polynomial in Polynomials)
        {
            hash.Add(polynomial);
        }

        return hash.ToHashCode();
    }

    public override string ToString()
    {
        StringBuilder builder = new ();
        builder.Append(Ring);
        string[]? variables = Ring.GetVars();
        builder.Append('\n').Append('(').Append('\n');
        bool first = true;
        foreach (GenPolynomial<C> polynomial in Polynomials)
        {
            string text = variables is null ? polynomial.ToString() : polynomial.ToString(variables);
            if (!first)
            {
                builder.Append(',');
                if (text.Length > 10)
                {
                    builder.Append('\n');
                }
                else
                {
                    builder.Append(' ');
                }
            }
            else
            {
                first = false;
            }

            builder.Append("( ").Append(text).Append(" )");
            if (text.Length > 10)
            {
                builder.Append('\n');
            }
        }

        builder.Append('\n').Append(')');
        return builder.ToString();
    }
}
