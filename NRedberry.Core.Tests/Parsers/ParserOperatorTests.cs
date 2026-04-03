using NRedberry.Parsers;
using RedberryParser = NRedberry.Parsers.Parser;

namespace NRedberry.Core.Tests.Parsers;

public sealed class ParserOperatorTests
{
    [Fact]
    public void ShouldReturnNullWhenExpressionHasNoTopLevelOperator()
    {
        var parserOperator = new TestParserOperator('+', '-');
        var parser = CreateParser(parserOperator);

        var result = parserOperator.ParseToken("(a+b)", parser);

        result.ShouldBeNull();
        parserOperator.CompileNodes.ShouldBeEmpty();
    }

    [Fact]
    public void ShouldThrowWhenBracketsAreUnbalanced()
    {
        var parserOperator = new TestParserOperator('+', '-');
        var parser = CreateParser(parserOperator);

        Should.Throw<BracketsError>(() => parserOperator.ParseToken(")a+b", parser));
    }

    [Fact]
    public void ShouldSplitTopLevelTermsAndApplyInverseOperation()
    {
        var parserOperator = new TestParserOperator('+', '-');
        var parser = CreateParser(parserOperator);

        var result = parserOperator.ParseToken("a-b+c", parser);

        result.ShouldNotBeNull();
        result.TokenType.ShouldBe(TokenType.Sum);
        parserOperator.ParsedExpressions.ShouldBe(["a", "b", "c"]);
        parserOperator.InverseCalls.ShouldBe(1);
        parserOperator.CompileNodes[0].TokenType.ShouldBe(TokenType.Number);
        parserOperator.CompileNodes[1].TokenType.ShouldBe(TokenType.Product);
        parserOperator.CompileNodes[2].TokenType.ShouldBe(TokenType.Number);
    }

    [Fact]
    public void ShouldIgnoreWhitespaceOutsideIndicesLevelOnly()
    {
        var parserOperator = new TestParserOperator('+', '-');
        var parser = CreateParser(parserOperator);

        var result = parserOperator.ParseToken(" a_{i j} + b ", parser);

        result.ShouldNotBeNull();
        parserOperator.ParsedExpressions.ShouldBe(["a_{i j}", "b"]);
    }

    [Fact]
    public void ShouldNormalizeAdjacentSignsBeforeSplitting()
    {
        var parserOperator = new TestParserOperator('+', '-');
        var parser = CreateParser(parserOperator);

        var result = parserOperator.ParseToken("a--b++c+-d-+e", parser);

        result.ShouldNotBeNull();
        parserOperator.ParsedExpressions.ShouldBe(["a", "b", "c", "d", "e"]);
        parserOperator.InverseCalls.ShouldBe(2);
    }

    [Fact]
    public void ShouldRespectTestOperatorOverride()
    {
        Func<char[], int, bool> testOperator = (expressionChars, position) =>
        {
            return !((position + 1 < expressionChars.Length && expressionChars[position + 1] == '*')
                || (position - 1 >= 0 && expressionChars[position - 1] == '*'));
        };

        var parserOperator = new TestParserOperator('*', '/', testOperator);
        var parser = CreateParser(parserOperator);

        var result = parserOperator.ParseToken("a**b*c", parser);

        result.ShouldNotBeNull();
        parserOperator.ParsedExpressions.ShouldBe(["a**b", "c"]);
        result.TokenType.ShouldBe(TokenType.Sum);
    }

    [Fact]
    public void ShouldReturnNullWhenAllCandidateOperatorsAreRejectedByTestOperator()
    {
        var parserOperator = new TestParserOperator('*', '/', (_, _) => false);
        var parser = CreateParser(parserOperator);

        var result = parserOperator.ParseToken("a*b", parser);

        result.ShouldBeNull();
    }

    private static RedberryParser CreateParser(TestParserOperator parserOperator)
    {
        return new RedberryParser(parserOperator, new LeafTokenParser(parserOperator.ParsedExpressions));
    }
}

internal sealed class TestParserOperator(
    char operatorSymbol,
    char operatorInverseSymbol,
    Func<char[], int, bool>? testOperator = null)
    : ParserOperator(operatorSymbol, operatorInverseSymbol)
{
    private readonly Func<char[], int, bool> _testOperator = testOperator ?? ((_, _) => true);

    public override int Priority => 100;

    public List<ParseToken> CompileNodes { get; } = [];

    public List<string> ParsedExpressions { get; } = [];

    public int InverseCalls { get; private set; }

    protected override ParseToken Compile(IReadOnlyList<ParseToken> nodes)
    {
        CompileNodes.Clear();
        CompileNodes.AddRange(nodes);
        return new ParseToken(TokenType.Sum, nodes.ToArray());
    }

    protected override ParseToken InverseOperation(ParseToken tensor)
    {
        InverseCalls++;
        return new ParseToken(TokenType.Product, tensor);
    }

    protected override bool TestOperator(char[] expressionChars, int position)
    {
        return _testOperator(expressionChars, position);
    }
}

internal sealed class LeafTokenParser(List<string> parsedExpressions) : ITokenParser
{
    public int Priority => 0;

    public ParseToken ParseToken(string expression, RedberryParser parser)
    {
        parsedExpressions.Add(expression);
        return new ParseToken(TokenType.Number);
    }
}
