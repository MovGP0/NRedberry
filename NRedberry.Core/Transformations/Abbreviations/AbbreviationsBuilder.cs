using System.Text.Json;
using NRedberry.Core.Utils;
using NRedberry.Indices;
using NRedberry.Numbers;
using NRedberry.Tensors;
using NRedberry.Tensors.Iterators;
using NRedberry.Transformations.Substitutions;
using NRedberry.Transformations.Symmetrization;
using TensorFunctions = NRedberry.Tensors.Tensors;

namespace NRedberry.Transformations.Abbreviations;

/// <summary>
/// Port of cc.redberry.core.transformations.abbreviations.AbbreviationsBuilder.
/// </summary>
public sealed class AbbreviationsBuilder : ITransformation
{
    public const int DefaultAbbrSize = 50;
    public const string DefaultAbbrPrefix = "abbr";

    private static readonly IComparer<Abbreviation> TopologicalSortComparer =
        Comparer<Abbreviation>.Create((left, right) => left.Index.CompareTo(right.Index));

    private readonly Dictionary<int, List<Abbreviation>> _abbreviations = new();
    private int _abbrCounter;

    public AbbreviationsBuilder()
    {
        Filter = new TrueIndicator<Tensor>();
        AbbreviationFilter = new TrueIndicator<FromChildToParentIterator>();
        MaxSumSize = DefaultAbbrSize;
        AbbrPrefix = DefaultAbbrPrefix;
        AbbreviateScalars = true;
        AbbreviateScalarsSeparately = false;
        AbbreviateTopLevel = false;
        Locked = false;
    }

    public int MaxSumSize { get; set; }

    public string AbbrPrefix { get; set; }

    public bool AbbreviateScalars { get; set; }

    public bool AbbreviateScalarsSeparately { get; set; }

    public bool AbbreviateTopLevel { get; set; }

    public bool Locked { get; set; }

    public IIndicator<Tensor> Filter { get; set; }

    public IIndicator<FromChildToParentIterator> AbbreviationFilter { get; set; }

    public AbbreviationsBuilder Add(SimpleTensor pattern, Tensor replacement)
    {
        ArgumentNullException.ThrowIfNull(pattern);
        ArgumentNullException.ThrowIfNull(replacement);

        AddAbbreviation(new Abbreviation(0, replacement, pattern));
        return this;
    }

    public AbbreviationsBuilder Add(Tensor pattern, Tensor replacement)
    {
        ArgumentNullException.ThrowIfNull(pattern);
        ArgumentNullException.ThrowIfNull(replacement);

        AddAbbreviation(new Abbreviation(0, replacement, pattern));
        return this;
    }

    public ITransformation Build()
    {
        return this;
    }

    public Tensor Transform(Tensor tensor)
    {
        ArgumentNullException.ThrowIfNull(tensor);

        FromChildToParentIterator iterator = new(tensor);
        Tensor? current;
        while ((current = iterator.Next()) is not null)
        {
            if (!AbbreviateTopLevel && iterator.Depth == 0)
            {
                continue;
            }

            if (current is Product product && AbbreviateScalars)
            {
                iterator.Set(AbbreviateProduct(product));
            }

            if (!Filter.Is(current))
            {
                continue;
            }

            if (!AbbreviationFilter.Is(iterator))
            {
                continue;
            }

            if (current is Sum && current.Size < MaxSumSize && TensorUtils.IsSymbolic(current))
            {
                iterator.Set(Abbreviate(current));
            }
        }

        return iterator.Result();
    }

    private Tensor AbbreviateProduct(Product product)
    {
        ProductContent content = product.Content;
        Tensor[] scalars = content.Scalars;
        if (scalars.Length == 0)
        {
            return product;
        }

        Tensor nonScalar = content.NonScalar ?? Complex.One;
        Tensor abbr;
        if (AbbreviateScalarsSeparately)
        {
            List<Tensor> scalarFactors = new(scalars.Length);
            foreach (Tensor scalar in scalars)
            {
                scalarFactors.Add(Filter.Is(scalar) ? Abbreviate(scalar) : scalar);
            }

            abbr = TensorFunctions.Multiply(scalarFactors);
        }
        else
        {
            abbr = Abbreviate(TensorFunctions.Multiply(scalars));
        }

        return TensorFunctions.Multiply(GetIndexlessSubProduct(product), abbr, nonScalar);
    }

    private Tensor Abbreviate(Tensor tensor)
    {
        int hashCode = tensor.GetHashCode();
        if (!_abbreviations.TryGetValue(hashCode, out List<Abbreviation>? list))
        {
            list = new List<Abbreviation>();
            _abbreviations.Add(hashCode, list);
        }

        foreach (Abbreviation abbreviation in list)
        {
            bool? compare = TensorUtils.Compare1(abbreviation.Definition, tensor);
            if (compare is not null)
            {
                abbreviation.Count++;
                return compare.Value ? abbreviation.NegatedAbbreviation : abbreviation.AbbreviationTensor;
            }
        }

        if (Locked)
        {
            return tensor;
        }

        Abbreviation next = NextAbbreviation(tensor);
        list.Add(next);
        return next.AbbreviationTensor;
    }

    private Abbreviation NextAbbreviation(Tensor tensor)
    {
        int index = _abbrCounter++;
        return new Abbreviation(
            index,
            tensor,
            TensorFunctions.SimpleTensor(AbbrPrefix + index, IndicesFactory.EmptySimpleIndices));
    }

    public Abbreviation AddAbbreviation(Abbreviation other)
    {
        ArgumentNullException.ThrowIfNull(other);

        int hashCode = other.Definition.GetHashCode();
        if (!_abbreviations.TryGetValue(hashCode, out List<Abbreviation>? list))
        {
            list = new List<Abbreviation>();
            _abbreviations.Add(hashCode, list);
        }

        for (int i = 0; i < list.Count; ++i)
        {
            Abbreviation abbreviation = list[i];
            bool? compare = TensorUtils.Compare1(abbreviation.Definition, other.Definition);
            if (compare is not null)
            {
                Tensor definition = compare.Value ? TensorFunctions.Negate(other.Definition) : other.Definition;
                Abbreviation updated = new(
                    abbreviation.Count,
                    abbreviation.Index,
                    definition,
                    other.AbbreviationTensor,
                    TensorFunctions.Negate(other.AbbreviationTensor));
                list[i] = updated;
                return abbreviation;
            }
        }

        Abbreviation created = new(_abbrCounter++, other.Definition, other.AbbreviationTensor);
        list.Add(created);
        return created;
    }

    public void MergeFrom(AbbreviationsBuilder other)
    {
        ArgumentNullException.ThrowIfNull(other);

        foreach (Abbreviation abbreviation in other.GetAbbreviations())
        {
            AddAbbreviation(abbreviation);
        }
    }

    public List<Abbreviation> GetAbbreviations()
    {
        List<Abbreviation> result = new();
        foreach (List<Abbreviation> abbreviations in _abbreviations.Values)
        {
            result.AddRange(abbreviations);
        }

        result.Sort(TopologicalSortComparer);
        return result;
    }

    public SubstitutionTransformation AbbreviationReplacements()
    {
        List<Abbreviation> abbreviations = GetAbbreviations();
        Expression[] substitutions = new Expression[abbreviations.Count];
        for (int i = 0; i < abbreviations.Count; ++i)
        {
            substitutions[i] = abbreviations[i].AsSubstitution();
        }

        return new SubstitutionTransformation(substitutions, true);
    }

    public long AbbreviationsSymbolCount()
    {
        long total = 0;
        foreach (List<Abbreviation> abbreviations in _abbreviations.Values)
        {
            foreach (Abbreviation abbreviation in abbreviations)
            {
                total += TensorUtils.SymbolsCount(abbreviation.Definition);
            }
        }

        return total;
    }

    public TransformationCollection GetReplacements()
    {
        List<Abbreviation> abbreviations = GetAbbreviations();
        List<Expression> substitutions = new(abbreviations.Count);
        foreach (Abbreviation abbreviation in abbreviations)
        {
            substitutions.Add(abbreviation.AsSubstitution().Transpose());
        }

        return new TransformationCollection(substitutions);
    }

    public void WriteToFile(string file)
    {
        ArgumentNullException.ThrowIfNull(file);
        WriteToFile(this, new System.IO.FileInfo(file));
    }

    public void WriteToFile(System.IO.FileInfo file)
    {
        ArgumentNullException.ThrowIfNull(file);
        WriteToFile(this, file);
    }

    public static void WriteToFile(AbbreviationsBuilder abbreviations, string file)
    {
        ArgumentNullException.ThrowIfNull(abbreviations);
        ArgumentNullException.ThrowIfNull(file);
        WriteToFile(abbreviations, new System.IO.FileInfo(file));
    }

    public static void WriteToFile(AbbreviationsBuilder abbreviations, System.IO.FileInfo file)
    {
        ArgumentNullException.ThrowIfNull(abbreviations);
        ArgumentNullException.ThrowIfNull(file);

        List<AbbreviationRecord> records = new(abbreviations.GetAbbreviations().Count);
        foreach (Abbreviation abbreviation in abbreviations.GetAbbreviations())
        {
            records.Add(new AbbreviationRecord(
                abbreviation.Index,
                abbreviation.Count,
                abbreviation.Definition.ToString(),
                abbreviation.AbbreviationTensor.ToString()));
        }

        string json = JsonSerializer.Serialize(records);
        System.IO.File.WriteAllText(file.FullName, json);
    }

    public static AbbreviationsBuilder ReadFromFile(string file)
    {
        ArgumentNullException.ThrowIfNull(file);
        return ReadFromFile(new System.IO.FileInfo(file));
    }

    public static AbbreviationsBuilder ReadFromFile(System.IO.FileInfo file)
    {
        ArgumentNullException.ThrowIfNull(file);

        string json = System.IO.File.ReadAllText(file.FullName);
        List<AbbreviationRecord>? records = JsonSerializer.Deserialize<List<AbbreviationRecord>>(json);
        AbbreviationsBuilder builder = new();
        if (records is null)
        {
            return builder;
        }

        builder._abbreviations.Clear();
        int maxIndex = -1;
        foreach (AbbreviationRecord record in records)
        {
            Tensor definition = TensorFunctions.Parse(record.Definition);
            Tensor abbreviation = TensorFunctions.Parse(record.Abbreviation);
            Abbreviation abbr = new(
                record.Count,
                record.Index,
                definition,
                abbreviation,
                TensorFunctions.Negate(abbreviation));
            builder.AddAbbreviationInternal(abbr);
            if (record.Index > maxIndex)
            {
                maxIndex = record.Index;
            }
        }

        builder._abbrCounter = maxIndex + 1;
        return builder;
    }

    public override string ToString()
    {
        System.Text.StringBuilder sb = new();
        sb.AppendLine("AbbreviationsBuilder{");
        sb.Append("\tabbrs=").Append(_abbreviations).AppendLine();
        sb.Append("\tmaxSumSize=").Append(MaxSumSize).AppendLine();
        sb.Append("\tabbrPrefix='").Append(AbbrPrefix).Append('\'').AppendLine();
        sb.Append("\tabbreviateScalars=").Append(AbbreviateScalars).AppendLine();
        sb.Append("\tabbreviateScalarsSeparately=").Append(AbbreviateScalarsSeparately).AppendLine();
        sb.Append("\tabbreviateTopLevel=").Append(AbbreviateTopLevel).AppendLine();
        sb.Append("\tfilter=").Append(Filter).AppendLine();
        sb.Append("\taFilter=").Append(AbbreviationFilter).AppendLine();
        sb.Append("\tabbrCounter=").Append(_abbrCounter).AppendLine();
        sb.Append('}');
        return sb.ToString();
    }

    private void AddAbbreviationInternal(Abbreviation abbreviation)
    {
        int hashCode = abbreviation.Definition.GetHashCode();
        if (!_abbreviations.TryGetValue(hashCode, out List<Abbreviation>? list))
        {
            list = new List<Abbreviation>();
            _abbreviations.Add(hashCode, list);
        }

        list.Add(abbreviation);
    }

    private static Tensor GetIndexlessSubProduct(Product product)
    {
        if (product.IndexlessData.Length == 0)
        {
            return product.Factor;
        }

        if (product.Factor == Complex.One && product.IndexlessData.Length == 1)
        {
            return product.IndexlessData[0];
        }

        return new Product(
            product.Factor,
            product.IndexlessData,
            Array.Empty<Tensor>(),
            ProductContent.EmptyInstance,
            IndicesFactory.EmptyIndices);
    }

    private sealed record class AbbreviationRecord(int Index, long Count, string Definition, string Abbreviation);
}

public sealed class Abbreviation : IEquatable<Abbreviation>
{
    public Abbreviation(int index, Tensor definition, Tensor abbreviation)
        : this(1, index, definition, abbreviation, TensorFunctions.Negate(abbreviation))
    {
    }

    public Abbreviation(long count, int index, Tensor definition, Tensor abbreviation, Tensor negatedAbbreviation)
    {
        Count = count;
        Index = index;
        Definition = definition;
        AbbreviationTensor = abbreviation;
        NegatedAbbreviation = negatedAbbreviation;
    }

    public long Count { get; set; }

    public int Index { get; }

    public Tensor Definition { get; }

    public Tensor AbbreviationTensor { get; }

    public Tensor NegatedAbbreviation { get; }

    public Expression AsSubstitution()
    {
        return TensorFunctions.Expression(AbbreviationTensor, Definition);
    }

    public override string ToString()
    {
        return "(" + Count + ") " + AbbreviationTensor + " = " + Definition;
    }

    public static bool Equals(Abbreviation? left, Abbreviation? right)
    {
        if (ReferenceEquals(left, right))
        {
            return true;
        }

        if (left is null || right is null)
        {
            return false;
        }

        return left.Equals(right);
    }

    public bool Equals(Abbreviation? other)
    {
        if (other is null)
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return AbbreviationTensor.Equals(other.AbbreviationTensor)
            && TensorUtils.Equals(Definition, other.Definition);
    }

    public override bool Equals(object? obj)
    {
        return obj is Abbreviation other && Equals(other);
    }

    public override int GetHashCode()
    {
        HashCode hashCode = new();
        hashCode.Add(Definition);
        hashCode.Add(AbbreviationTensor);
        return hashCode.ToHashCode();
    }

    public static bool operator ==(Abbreviation? left, Abbreviation? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(Abbreviation? left, Abbreviation? right)
    {
        return !Equals(left, right);
    }
}
