using System.Text;
using ContextCC = NRedberry.Contexts.CC;
using NRedberry.Contexts;
using NRedberry.Indices;
using NRedberry.Tensors;

namespace NRedberry.Parsers;

/*
 * Original: ./core/src/main/java/cc/redberry/core/parser/ParseTokenSimpleTensor.java
 */

public class ParseTokenSimpleTensor : ParseToken
{
    public SimpleIndices Indices { get; internal set; }

    public string Name { get; }

    protected ParseTokenSimpleTensor(SimpleIndices indices, string name, TokenType type, params ParseToken[] content)
        : base(type, content)
    {
        ArgumentNullException.ThrowIfNull(indices);
        ArgumentNullException.ThrowIfNull(name);

        Indices = indices;
        Name = name;
    }

    public ParseTokenSimpleTensor(SimpleIndices indices, string name)
        : base(TokenType.SimpleTensor)
    {
        ArgumentNullException.ThrowIfNull(indices);
        ArgumentNullException.ThrowIfNull(name);

        Indices = indices;
        Name = name;
    }

    public virtual NameAndStructureOfIndices GetIndicesTypeStructureAndName()
    {
        return new NameAndStructureOfIndices(Name, [StructureOfIndices.Create(Indices)]);
    }

    public override Indices.Indices GetIndices()
    {
        return Indices;
    }

    public override string ToString()
    {
        return Name + Indices;
    }

    public override Tensor ToTensor()
    {
        return NRedberry.Tensors.Tensors.SimpleTensor(Name, Indices);
    }

    public override bool Equals(object? obj)
    {
        if (!base.Equals(obj))
        {
            return false;
        }

        var other = (ParseTokenSimpleTensor)obj!;
        if (!Equals(Indices, other.Indices))
        {
            return false;
        }

        return Name == other.Name;
    }

    public bool IsKroneckerOrMetric()
    {
        return Name == ContextCC.NameManager.KroneckerName || Name == ContextCC.NameManager.MetricName;
    }

    public bool IsKronecker()
    {
        return Name == ContextCC.NameManager.KroneckerName;
    }

    public override string ToString(OutputFormat mode)
    {
        StringBuilder sb = new();

        if (mode.Is(OutputFormat.Maple) && IsKroneckerOrMetric())
        {
            if (IsKronecker())
            {
                sb.Append("KroneckerDelta");
            }
            else
            {
                sb.Append("g_");
            }
        }
        else
        {
            sb.Append(Name);
        }

        if (Indices.Size() == 0)
        {
            return sb.ToString();
        }

        bool external = mode.Is(OutputFormat.WolframMathematica) || mode.Is(OutputFormat.Maple);
        if (external)
        {
            sb.Append('[');
        }

        sb.Append(Indices.ToString(mode));

        if (external)
        {
            sb.Append(']');
        }

        return sb.ToString();
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(base.GetHashCode(), Indices, Name);
    }
}
