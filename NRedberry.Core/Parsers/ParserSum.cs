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

    public override ParseToken? ParseToken(string expression, Parser parser)
    {
        throw new NotImplementedException();
    }

    public override int Priority => throw new NotImplementedException();

    protected override ParseToken Compile(IReadOnlyList<ParseToken> nodes)
    {
        throw new NotImplementedException();
    }

    protected override ParseToken InverseOperation(ParseToken tensor)
    {
        throw new NotImplementedException();
    }
}
