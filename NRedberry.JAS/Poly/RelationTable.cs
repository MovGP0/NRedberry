using System.Text;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;

/// <summary>
/// RelationTable for solvable polynomials. This class maintains the non-commutative multiplication relations
/// of solvable polynomial rings. The table entries are initialized with relations of the form
/// x_j * x_i = p_ij. During multiplication the relations are updated by relations of the form
/// x_j^k * x_i^l = p_ijkl. If no relation for x_j * x_i is found in the table, the multiplication is assumed
/// to be commutative.
/// </summary>
/// <typeparam name="C">coefficient type</typeparam>
/// <remarks>
/// Original Java file: cc.redberry.core.transformations.factor.jasfactor.edu.jas.poly.RelationTable
/// </remarks>
public class RelationTable<C>(GenSolvablePolynomialRing<C> ring)
    where C : RingElem<C>
{
    private readonly object _syncRoot = new ();

    public Dictionary<List<int>, List<RelationEntry>> Table { get; } = new(new SequenceComparer());
    public GenSolvablePolynomialRing<C> Ring { get; } = ring ?? throw new ArgumentNullException(nameof(ring));

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(this, obj))
        {
            return true;
        }

        if (obj is not RelationTable<C> other)
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

        foreach ((List<int> key, List<RelationEntry> value) in Table)
        {
            if (!other.Table.TryGetValue(key, out List<RelationEntry>? otherValue))
            {
                return false;
            }

            if (!EntriesEqual(value, otherValue))
            {
                return false;
            }
        }

        foreach ((List<int> key, List<RelationEntry> value) in other.Table)
        {
            if (!Table.TryGetValue(key, out List<RelationEntry>? thisValue))
            {
                return false;
            }

            if (!EntriesEqual(value, thisValue))
            {
                return false;
            }
        }

        return true;
    }

    public override int GetHashCode()
    {
        int ringHash = Ring.GetHashCode();
        int tableHash = 0;
        foreach ((List<int> key, List<RelationEntry> value) in Table)
        {
            int keyHash = SequenceComparer.ComputeHash(key);
            int valueHash = RelationEntry.ComputeHash(value);
            tableHash ^= HashCode.Combine(keyHash, valueHash);
        }

        return HashCode.Combine(ringHash, tableHash);
    }

    public override string ToString()
    {
        if (Table.Count == 0)
        {
            return "RelationTable[]";
        }

        StringBuilder builder = new ();
        builder.Append("RelationTable[");
        bool first = true;
        foreach ((List<int> key, List<RelationEntry> value) in Table)
        {
            if (!first)
            {
                builder.Append(", ");
            }

            builder
                .Append(FormatKey(key))
                .Append('=')
                .Append('(');
            bool innerFirst = true;
            foreach (RelationEntry entry in value)
            {
                if (!innerFirst)
                {
                    builder.Append(", ");
                }

                builder.Append(entry);
                innerFirst = false;
            }

            builder.Append(')');
            first = false;
        }

        builder.Append(']');
        return builder.ToString();
    }

    public string ToString(string[]? vars)
    {
        if (vars is null)
        {
            return ToString();
        }

        StringBuilder builder = new ();
        builder.Append("RelationTable\n(");
        bool first = true;
        foreach (List<RelationEntry> part in Table.Values)
        {
            foreach (RelationEntry entry in part)
            {
                if (!first)
                {
                    builder.Append(",\n");
                }
                else
                {
                    builder.Append('\n');
                }

                string left = entry.Pair.GetFirst().ToString(vars);
                string right = entry.Pair.GetSecond().ToString(vars);
                string polynomial = entry.Polynomial.ToString();
                builder.Append("( ").Append(left).Append(" ), ");
                builder.Append("( ").Append(right).Append(" ), ");
                builder.Append("( ").Append(polynomial).Append(" )");
                first = false;
            }
        }

        builder.Append("\n)\n");
        return builder.ToString();
    }

    /// <summary>
    /// Inserts or updates the non-commutative relation between the two exponent vectors.
    /// </summary>
    /// <param name="e">Left exponent vector.</param>
    /// <param name="f">Right exponent vector.</param>
    /// <param name="p">Solvable polynomial describing their product.</param>
    public void Update(ExpVector e, ExpVector f, GenSolvablePolynomial<C> p)
    {
        ArgumentNullException.ThrowIfNull(e);
        ArgumentNullException.ThrowIfNull(f);
        ArgumentNullException.ThrowIfNull(p);

        lock (_syncRoot)
        {
            ExpVector left = e;
            ExpVector right = f;
            GenSolvablePolynomial<C> product = p;

            if (left.TotalDeg() == 1 && right.TotalDeg() == 1)
            {
                int[] leftDependency = left.DependencyOnVariables();
                int[] rightDependency = right.DependencyOnVariables();
                if (leftDependency.Length == 0 || rightDependency.Length == 0)
                {
                    throw new ArgumentException("RelationTable update requires non-constant exponents.");
                }

                if (leftDependency[0] == rightDependency[0])
                {
                    throw new ArgumentException("RelationTable update e == f");
                }

                if (leftDependency[0] > rightDependency[0])
                {
                    (left, right) = (right, left);
                    product = AdjustProductForSwap(product);
                }
            }

            ExpVector expected = left.Sum(right);
            ExpVector? leading = product.LeadingExpVector();
            if (leading is null || !expected.Equals(leading))
            {
                throw new ArgumentException("RelationTable update e * f does not match lt(p)");
            }

            List<int> key = MakeKey(left, right);
            RelationEntry entry = new (new ExpVectorPair(left, right), product);

            if (!Table.TryGetValue(key, out List<RelationEntry>? part))
            {
                Table[key] = [entry];
                return;
            }

            int insertIndex = -1;
            for (int i = 0; i < part.Count; i++)
            {
                if (part[i].Pair.IsMultiple(entry.Pair))
                {
                    insertIndex = i + 1;
                }
            }

            if (insertIndex < 0)
            {
                insertIndex = 0;
            }

            part.Insert(insertIndex, entry);
        }
    }

    /// <summary>
    /// Updates the relation table using the leading terms of two polynomials.
    /// </summary>
    /// <param name="first">First polynomial.</param>
    /// <param name="second">Second polynomial.</param>
    /// <param name="product">Solvable polynomial product.</param>
    public void Update(GenPolynomial<C> first, GenPolynomial<C> second, GenSolvablePolynomial<C> product)
    {
        ArgumentNullException.ThrowIfNull(first);
        ArgumentNullException.ThrowIfNull(second);
        ArgumentNullException.ThrowIfNull(product);

        if (first.IsZero() || second.IsZero())
        {
            throw new ArgumentException("Polynomials may not be zero.");
        }

        if (!first.LeadingBaseCoefficient().IsOne() || !second.LeadingBaseCoefficient().IsOne())
        {
            throw new ArgumentException("Leading base coefficients must be one.");
        }

        ExpVector? firstLeading = first.LeadingExpVector();
        ExpVector? secondLeading = second.LeadingExpVector();
        if (firstLeading is null || secondLeading is null)
        {
            throw new InvalidOperationException("Polynomials must have leading monomials.");
        }

        Update(firstLeading, secondLeading, product);
    }

    public TableRelation<C> Lookup(ExpVector e, ExpVector f)
    {
        ArgumentNullException.ThrowIfNull(e);
        ArgumentNullException.ThrowIfNull(f);

        List<int> key = MakeKey(e, f);
        if (!Table.TryGetValue(key, out List<RelationEntry>? part))
        {
            ExpVector exponent = e.Sum(f);
            C coefficient = Ring.CoFac.FromInteger(1);
            GenSolvablePolynomial<C> symmetric = new (Ring, coefficient, exponent);
            return new TableRelation<C>(null, null, symmetric);
        }

        ExpVectorPair lookupPair = new (e, f);
        foreach (RelationEntry entry in part)
        {
            if (!lookupPair.IsMultiple(entry.Pair))
            {
                continue;
            }

            ExpVector remainderLeft = e.Subtract(entry.Pair.GetFirst());
            ExpVector remainderRight = f.Subtract(entry.Pair.GetSecond());

            ExpVector? left = remainderLeft.IsZero() ? null : remainderLeft;
            ExpVector? right = remainderRight.IsZero() ? null : remainderRight;

            return new TableRelation<C>(left, right, entry.Polynomial);
        }

        throw new InvalidOperationException($"No entry found in relation table for {lookupPair}.");
    }

    public int Size()
    {
        return Table.Values.Sum(list => list.Count);
    }

    public void Extend(RelationTable<C> table)
    {
        ArgumentNullException.ThrowIfNull(table);
        if (table.Table.Count == 0)
        {
            return;
        }

        int extendBy = Ring.Nvar - table.Ring.Nvar;
        const int index = 0;
        const long exponent = 0L;

        foreach (List<RelationEntry> entries in table.Table.Values)
        {
            foreach (RelationEntry entry in entries)
            {
                ExpVector extendedLeft = entry.Pair.GetFirst().Extend(extendBy, index, exponent);
                ExpVector extendedRight = entry.Pair.GetSecond().Extend(extendBy, index, exponent);
                GenSolvablePolynomial<C> extendedProduct = ExtendPolynomial(entry.Polynomial, Ring, extendBy, index, exponent);
                Update(extendedLeft, extendedRight, extendedProduct);
            }
        }
    }

    public void Contract(RelationTable<C> table)
    {
        ArgumentNullException.ThrowIfNull(table);
        if (table.Table.Count == 0)
        {
            return;
        }

        int contractBy = table.Ring.Nvar - Ring.Nvar;
        foreach (List<RelationEntry> entries in table.Table.Values)
        {
            foreach (RelationEntry entry in entries)
            {
                ExpVector contractedLeft = entry.Pair.GetFirst().Contract(contractBy, entry.Pair.GetFirst().Length() - contractBy);
                ExpVector contractedRight = entry.Pair.GetSecond().Contract(contractBy, entry.Pair.GetSecond().Length() - contractBy);

                if (contractedLeft.IsZero() || contractedRight.IsZero())
                {
                    continue;
                }

                SortedDictionary<ExpVector, GenSolvablePolynomial<C>> contracted = ContractPolynomial(entry.Polynomial, Ring, contractBy);
                if (contracted.Count != 1)
                {
                    continue;
                }

                GenSolvablePolynomial<C> result = contracted.Values.First();
                Update(contractedLeft, contractedRight, result);
            }
        }
    }

    public void Reverse(RelationTable<C> table)
    {
        ArgumentNullException.ThrowIfNull(table);
        if (table.Table.Count == 0)
        {
            return;
        }

        if (Table.Count != 0)
        {
            throw new InvalidOperationException("Target relation table must be empty when reversing.");
        }

        int split = -1;
        if (Ring.Tord.GetEvord2() != 0 && Ring.IsPartial)
        {
            split = Ring.Tord.GetSplit();
        }

        foreach (List<RelationEntry> entries in table.Table.Values)
        {
            foreach (RelationEntry entry in entries)
            {
                ExpVector originalLeft = entry.Pair.GetFirst();
                ExpVector originalRight = entry.Pair.GetSecond();
                GenSolvablePolynomial<C> reversedPolynomial = ReversePolynomial(entry.Polynomial, Ring, split);

                ExpVector reversedLeft = split >= 0 ? originalLeft.Reverse(split) : originalLeft.Reverse();
                ExpVector reversedRight = split >= 0 ? originalRight.Reverse(split) : originalRight.Reverse();

                bool change = true;
                if (split >= 0)
                {
                    int[] leftDependencies = reversedLeft.DependencyOnVariables();
                    if (leftDependencies.Length == 0 || leftDependencies[0] >= split)
                    {
                        change = false;
                    }

                    int[] rightDependencies = reversedRight.DependencyOnVariables();
                    if (rightDependencies.Length == 0 || rightDependencies[0] >= split)
                    {
                        change = false;
                    }
                }

                if (!change)
                {
                    Update(originalLeft, originalRight, reversedPolynomial);
                }
                else
                {
                    Update(reversedRight, reversedLeft, reversedPolynomial);
                }
            }
        }
    }

    private static bool EntriesEqual(IReadOnlyList<RelationEntry> left, IReadOnlyList<RelationEntry> right)
    {
        if (ReferenceEquals(left, right))
        {
            return true;
        }

        if (left.Count != right.Count)
        {
            return false;
        }

        for (int i = 0; i < left.Count; i++)
        {
            if (!left[i].Equals(right[i]))
            {
                return false;
            }
        }

        return true;
    }

    private static RelationEntry CreateRelationEntry(ExpVector e, ExpVector f, GenSolvablePolynomial<C> p)
    {
        return new RelationEntry(new ExpVectorPair(e, f), p);
    }

    private static List<int> MakeKey(ExpVector e, ExpVector f)
    {
        int[] leftDependencies = e.DependencyOnVariables();
        int[] rightDependencies = f.DependencyOnVariables();

        List<int> key = new (leftDependencies.Length + rightDependencies.Length);
        key.AddRange(leftDependencies);
        key.AddRange(rightDependencies);
        return key;
    }

    private static GenSolvablePolynomial<C> AdjustProductForSwap(GenSolvablePolynomial<C> product)
    {
        GenPolynomial<C> polynomial = product;
        ExpVector? exponent = polynomial.LeadingExpVector();
        if (exponent is null)
        {
            throw new InvalidOperationException("Product must have a leading exponent.");
        }

        C coefficient = polynomial.LeadingBaseCoefficient();
        GenPolynomial<C> withoutLeading = polynomial.Sum(coefficient.Negate(), exponent);
        GenPolynomial<C> negated = withoutLeading.Negate();
        GenPolynomial<C> adjusted = negated.Sum(coefficient, exponent);

        return new GenSolvablePolynomial<C>(product.Ring, adjusted.CloneTerms(), copy: false);
    }

    private static GenSolvablePolynomial<C> ExtendPolynomial(GenSolvablePolynomial<C> polynomial, GenSolvablePolynomialRing<C> targetRing, int extendBy, int index, long exponent)
    {
        if (polynomial.Ring.Equals(targetRing))
        {
            return polynomial;
        }

        SortedDictionary<ExpVector, C> terms = new (targetRing.Tord.GetDescendComparator());
        if (!polynomial.IsZero())
        {
            foreach (KeyValuePair<ExpVector, C> term in polynomial.Terms)
            {
                ExpVector extended = term.Key.Extend(extendBy, index, exponent);
                terms.Add(extended, term.Value);
            }
        }

        return new GenSolvablePolynomial<C>(targetRing, terms, copy: false);
    }

    private static SortedDictionary<ExpVector, GenSolvablePolynomial<C>> ContractPolynomial(GenSolvablePolynomial<C> polynomial, GenSolvablePolynomialRing<C> targetRing, int contractBy)
    {
        SortedDictionary<ExpVector, GenSolvablePolynomial<C>> result = new (new TermOrder(TermOrder.INVLEX).GetAscendComparator());
        if (polynomial.IsZero())
        {
            return result;
        }

        foreach (KeyValuePair<ExpVector, C> term in polynomial.Terms)
        {
            ExpVector head = term.Key.Contract(0, contractBy);
            ExpVector tail = term.Key.Contract(contractBy, term.Key.Length() - contractBy);

            if (!result.TryGetValue(head, out GenSolvablePolynomial<C>? existing))
            {
                existing = GenSolvablePolynomialRing<C>.Zero;
            }

            GenPolynomial<C> basePolynomial = existing;
            GenPolynomial<C> updated = basePolynomial.Sum(term.Value, tail);
            result[head] = new GenSolvablePolynomial<C>(targetRing, updated.CloneTerms(), copy: false);
        }

        return result;
    }

    private static GenSolvablePolynomial<C> ReversePolynomial(GenSolvablePolynomial<C> polynomial, GenSolvablePolynomialRing<C> targetRing, int split)
    {
        SortedDictionary<ExpVector, C> terms = new (targetRing.Tord.GetDescendComparator());
        if (!polynomial.IsZero())
        {
            foreach (KeyValuePair<ExpVector, C> term in polynomial.Terms)
            {
                ExpVector reversed = split >= 0 ? term.Key.Reverse(split) : term.Key.Reverse();
                terms.Add(reversed, term.Value);
            }
        }

        return new GenSolvablePolynomial<C>(targetRing, terms, copy: false);
    }

    private static string FormatKey(IEnumerable<int> key)
    {
        return $"({string.Join(',', key)})";
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
            return ComputeHash(obj);
        }

        public static int ComputeHash(IEnumerable<int> values)
        {
            HashCode hash = new ();
            foreach (int value in values)
            {
                hash.Add(value);
            }

            return hash.ToHashCode();
        }
    }

    public sealed class RelationEntry(ExpVectorPair pair, GenSolvablePolynomial<C> polynomial)
    {
        public ExpVectorPair Pair { get; } = pair ?? throw new ArgumentNullException(nameof(pair));
        public GenSolvablePolynomial<C> Polynomial { get; } = polynomial ?? throw new ArgumentNullException(nameof(polynomial));

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj is not RelationEntry other)
            {
                return false;
            }

            return Pair.Equals(other.Pair) && Polynomial.Equals(other.Polynomial);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Pair, Polynomial);
        }

        public override string ToString()
        {
            return $"TableRelation[{Pair.GetFirst()} | {Pair.GetSecond()} = {Polynomial}]";
        }

        public static int ComputeHash(IEnumerable<RelationEntry> entries)
        {
            HashCode hash = new ();
            foreach (RelationEntry entry in entries)
            {
                hash.Add(entry);
            }

            return hash.ToHashCode();
        }
    }
}

public sealed class TableRelation<C>(ExpVector? first, ExpVector? second, GenSolvablePolynomial<C> product)
    where C : RingElem<C>
{
    public ExpVector? Left { get; } = first;
    public ExpVector? Right { get; } = second;
    public GenSolvablePolynomial<C> Product { get; } = product ?? throw new ArgumentNullException(nameof(product));

    internal ExpVectorPair Pair => new (Left ?? Product.LeadingExpVector() ?? Product.Ring.Evzero, Right ?? Product.Ring.Evzero);

    public override string ToString()
    {
        StringBuilder builder = new ();
        builder.Append("TableRelation[");
        builder.Append(Left?.ToString() ?? "null");
        builder.Append(" | ");
        builder.Append(Right?.ToString() ?? "null");
        builder.Append(" = ");
        builder.Append(Product);
        builder.Append(']');
        return builder.ToString();
    }
}
