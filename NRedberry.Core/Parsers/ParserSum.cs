using NRedberry.Numbers;

namespace NRedberry.Parsers;

/*
 * Original: ./core/src/main/java/cc/redberry/core/parser/ParserSum.java
 */

public sealed class ParserSum : ParserOperator
{
    public static ParserSum Instance { get; } = new();

    private ParserSum()
        : base('+', '-')
    {
    }

    public override int Priority => 1000;

    protected override ParseToken Compile(IReadOnlyList<ParseToken> nodes)
    {
        return new ParseToken(TokenType.Sum, nodes.ToArray());
    }

    protected override ParseToken InverseOperation(ParseToken tensor)
    {
        ParseToken[] content;
        if (tensor.TokenType == TokenType.Product)
        {
            content = new ParseToken[1 + tensor.Content.Length];
            content[0] = new ParseTokenNumber(Complex.MinusOne);
            Array.Copy(tensor.Content, 0, content, 1, tensor.Content.Length);
        }
        else
        {
            content =
            [
                new ParseTokenNumber(Complex.MinusOne),
                tensor
            ];
        }

        return new ParseToken(TokenType.Product, content);
    }
}
