using NRedberry;
using NRedberry.Indices;
using NRedberry.Numbers;
using NRedberry.Tensors;

namespace NRedberry.Parsers;

/*
 * Original: ./core/src/main/java/cc/redberry/core/parser/ParseTokenNumber.java
 */

public class ParseTokenNumber : ParseToken
{
    public Complex Value { get; }

    public ParseTokenNumber(Complex value)
        : base(TokenType.Number)
    {
        ArgumentNullException.ThrowIfNull(value);

        Value = value;
    }

    public override Indices.Indices GetIndices()
    {
        return IndicesFactory.EmptyIndices;
    }

    public override string ToString()
    {
        return Value.ToString();
    }

    public override string ToString(OutputFormat mode)
    {
        return "(" + Value.ToString(mode) + ")";
    }

    public override Tensor ToTensor()
    {
        return Value;
    }

    public override bool Equals(object? obj)
    {
        if (obj is null || obj.GetType() != GetType())
        {
            return false;
        }

        var other = (ParseTokenNumber)obj;
        return Equals(Value, other.Value);
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }
}
