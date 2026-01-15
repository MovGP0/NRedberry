using NRedberry.Indices;
using NRedberry.Tensors;
using NRedberry.Tensors.Functions;

namespace NRedberry.Parsers;

/*
 * Original: ./core/src/main/java/cc/redberry/core/parser/ParseTokenScalarFunction.java
 */

public sealed class ParseTokenScalarFunction : ParseToken
{
    public string Function { get; }

    public ParseTokenScalarFunction(string function, params ParseToken[] content)
        : base(TokenType.ScalarFunction, content)
    {
        ArgumentNullException.ThrowIfNull(function);

        if (content.Length != 1)
        {
            throw new ArgumentException(nameof(content));
        }

        Function = function;
    }

    public override Indices.Indices GetIndices()
    {
        return IndicesFactory.EmptyIndices;
    }

    public override string ToString()
    {
        return Function + "[" + Content[0] + "]";
    }

    public override Tensor ToTensor()
    {
        if (Content.Length != 1)
        {
            throw new ArgumentException("Wrong scalar function node.", nameof(Content));
        }

        Tensor arg = Content[0].ToTensor();
        switch (Function.ToLowerInvariant())
        {
            case "sin":
                return SinFactory.Factory.Create(arg);

            case "cos":
                return CosFactory.Factory.Create(arg);

            case "tan":
                return TanFactory.Factory.Create(arg);

            case "cot":
                return CotFactory.Factory.Create(arg);

            case "arcsin":
                return ArcSinFactory.Factory.Create(arg);

            case "arccos":
                return ArcCosFactory.Factory.Create(arg);

            case "arctan":
                return ArcTanFactory.Factory.Create(arg);

            case "arccot":
                return ArcCotFactory.Factory.Create(arg);

            case "log":
                return LogFactory.Factory.Create(arg);

            case "exp":
                return ExpFactory.Factory.Create(arg);
        }

        throw new InvalidOperationException($"Unknown scalar function \"{Function}\".");
    }

    public override bool Equals(object? obj)
    {
        if (!base.Equals(obj))
        {
            return false;
        }

        var other = (ParseTokenScalarFunction)obj!;
        return Function == other.Function;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(base.GetHashCode(), Function);
    }
}
