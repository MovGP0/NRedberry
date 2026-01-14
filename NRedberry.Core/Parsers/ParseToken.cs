using System.Text;
using NRedberry;
using NRedberry.Indices;
using NRedberry.Tensors;

namespace NRedberry.Parsers;

public class ParseToken : IEquatable<ParseToken>
{
    public TokenType TokenType { get; }
    public ParseToken[] Content { get; }
    public ParseToken? Parent { get; private set; }

    public ParseToken(TokenType tokenType, params ParseToken[] content)
    {
        ArgumentNullException.ThrowIfNull(content);

        TokenType = tokenType;
        Content = content;
        foreach (var node in content)
        {
            node.SetParent(this);
        }
    }

    private void SetParent(ParseToken parent)
    {
        Parent = parent;
    }

    public virtual Indices.Indices GetIndices()
    {
        switch (TokenType)
        {
            case TokenType.Product:
            {
                var builder = new IndicesBuilder();
                foreach (var node in Content)
                {
                    builder.Append(node.GetIndices());
                }

                return builder.Indices;
            }

            case TokenType.Sum:
                return IndicesFactory.Create(Content[0].GetIndices());

            case TokenType.Power:
                return IndicesFactory.EmptyIndices;
        }

        throw new ParserException($"Unknown tensor type: {TokenType}");
    }

    public override string ToString()
    {
        var builder = new StringBuilder();
        builder.Append(TokenType).Append('[');
        foreach (var node in Content)
        {
            builder.Append(node).Append(", ");
        }

        builder.Remove(builder.Length - 2, 2).Append(']');
        return builder.ToString();
    }

    public virtual string ToString(OutputFormat mode)
    {
        var sb = new StringBuilder();
        switch (TokenType)
        {
            case TokenType.Product:
            {
                var operatorChar = mode.Is(OutputFormat.LaTeX) ? ' ' : '*';
                for (var i = 0; ; ++i)
                {
                    sb.Append(Content[i].ToString(mode));
                    if (i == Content.Length - 1)
                        return sb.ToString();
                    sb.Append(operatorChar);
                }
            }

            case TokenType.Sum:
            {
                sb.Append('(');
                for (var i = 0; ; ++i)
                {
                    var temp = Content[i].ToString(mode);
                    if ((temp[0] == '-' || temp[0] == '+') && sb.Length != 0)
                        sb.Remove(sb.Length - 1, 1);
                    sb.Append(temp);
                    if (i == Content.Length - 1)
                        return sb.Append(')').ToString();
                    sb.Append('+');
                }
            }
        }

        throw new InvalidOperationException("Unsupported token type.");
    }

    protected Tensor[] ContentToTensors()
    {
        var tensors = new Tensor[Content.Length];
        for (var i = 0; i < Content.Length; ++i)
            tensors[i] = Content[i].ToTensor();
        return tensors;
    }

    public virtual Tensor ToTensor()
    {
        switch (TokenType)
        {
            case TokenType.Sum:
                return NRedberry.Tensors.Tensors.Sum(ContentToTensors());

            case TokenType.Power:
            {
                if (Content.Length != 2)
                    throw new ParserException("Power token should have exactly 2 arguments.");

                return Content[0].ToTensor().Pow(Content[1].ToTensor());
            }

            case TokenType.Product:
                return Tensor.MultiplyAndRenameConflictingDummies(ContentToTensors());

            default:
                throw new ParserException($"Unknown tensor type: {TokenType}");
        }
    }

    public bool Equals(ParseToken? other)
    {
        if (other is null || GetType() != other.GetType())
            return false;
        if (TokenType != other.TokenType)
            return false;
        if (Content.Length != other.Content.Length)
            return false;

        for (var i = 0; i < Content.Length; ++i)
        {
            if (!Equals(Content[i], other.Content[i]))
                return false;
        }

        return true;
    }

    public override bool Equals(object? obj) => Equals(obj as ParseToken);

    public override int GetHashCode()
    {
        HashCode hashCode = new();
        hashCode.Add(TokenType);
        foreach (var node in Content)
            hashCode.Add(node);
        return hashCode.ToHashCode();
    }

    public static bool operator ==(ParseToken? left, ParseToken? right) => Equals(left, right);

    public static bool operator !=(ParseToken? left, ParseToken? right) => !Equals(left, right);
}
