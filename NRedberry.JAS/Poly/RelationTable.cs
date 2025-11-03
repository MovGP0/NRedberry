using System;
using System.Collections.Generic;
using System.Linq;
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
    public Dictionary<List<int>, List<object>> Table { get; }
    public GenSolvablePolynomialRing<C> Ring { get; }

    public RelationTable(GenSolvablePolynomialRing<C> ring)
    {
        Ring = ring ?? throw new ArgumentNullException(nameof(ring));
        Table = new Dictionary<List<int>, List<object>>(new SequenceComparer());
    }

    public override bool Equals(object? p)
    {
        if (ReferenceEquals(this, p))
        {
            return true;
        }

        if (p is not RelationTable<C> other)
        {
            return false;
        }

        if (!Ring.Equals(other.Ring))
        {
            return false;
        }

        if (Table.Count != other.Table.Count)
        {
            return false;
        }

        foreach ((List<int> key, List<object> value) in Table)
        {
            if (!other.Table.TryGetValue(key, out List<object>? otherValue))
            {
                return false;
            }

            if (!value.SequenceEqual(otherValue))
            {
                return false;
            }
        }

        return true;
    }

    public override int GetHashCode()
    {
        HashCode hash = new ();
        hash.Add(Ring);
        foreach ((List<int> key, List<object> value) in Table)
        {
            hash.Add(key.Count);
            foreach (int item in key)
            {
                hash.Add(item);
            }

            hash.Add(value.Count);
        }

        return hash.ToHashCode();
    }

    public override string ToString()
    {
        if (Table.Count == 0)
        {
            return "RelationTable[]";
        }

        List<string> entries = new ();
        foreach ((List<int> key, List<object> value) in Table)
        {
            entries.Add($"{FormatKey(key)}={value.Count}");
        }

        return $"RelationTable[{string.Join(", ", entries)}]";
    }

    public string ToString(string[]? vars)
    {
        return ToString();
    }

    public void Update(ExpVector e, ExpVector f, GenSolvablePolynomial<C> p)
    {
        List<int> key = new (new[] { e.Length(), f.Length() });
        Table[key] = new List<object> { e, f, p };
    }

    public void Update(GenPolynomial<C> E, GenPolynomial<C> F, GenSolvablePolynomial<C> p)
    {
        List<int> key = new (new[] { E.Terms.Count, F.Terms.Count });
        Table[key] = new List<object> { E, F, p };
    }

    public object Lookup(ExpVector e, ExpVector f)
    {
        List<int> key = new (new[] { e.Length(), f.Length() });
        return Table.TryGetValue(key, out List<object>? value) ? value : Array.Empty<object>();
    }

    public int Size()
    {
        return Table.Count;
    }

    private static string FormatKey(IEnumerable<int> key)
    {
        return $"({string.Join(",", key)})";
    }

    private sealed class SequenceComparer : IEqualityComparer<List<int>>
    {
        public bool Equals(List<int>? x, List<int>? y)
        {
            if (ReferenceEquals(x, y))
            {
                return true;
            }

            if (x is null || y is null || x.Count != y.Count)
            {
                return false;
            }

            for (int i = 0; i < x.Count; i++)
            {
                if (x[i] != y[i])
                {
                    return false;
                }
            }

            return true;
        }

        public int GetHashCode(List<int> obj)
        {
            HashCode hash = new ();
            foreach (int value in obj)
            {
                hash.Add(value);
            }

            return hash.ToHashCode();
        }
    }
}
