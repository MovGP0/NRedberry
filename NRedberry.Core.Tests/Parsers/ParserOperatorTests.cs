using NRedberry.Parsers;
using RedberryParser = NRedberry.Parsers.Parser;
using Xunit;

namespace NRedberry.Core.Tests.Parsers;

public sealed class ParserOperatorTests
{
    [Fact]
    public void ShouldReturnNullWhenExpressionHasNoTopLevelOperator()
    {
        var parserOperator = new TestParserOperator('+', '-');
        var parser = CreateParser(parserOperator);

        var result = parserOperator.ParseToken("(a+b)", parser);

        Assert.Null(result);
        Assert.Empty(parserOperator.CompileNodes);
    }

    [Fact]
    public void ShouldThrowWhenBracketsAreUnbalanced()
    {
        var parserOperator = new TestParserOperator('+', '-');
        var parser = CreateParser(parserOperator);

        Assert.Throws<BracketsError>(() => parserOperator.ParseToken(")a+b", parser));
    }

    [Fact]
    public void ShouldSplitTopLevelTermsAndApplyInverseOperation()
    {
        var parserOperator = new TestParserOperator('+', '-');
        var parser = CreateParser(parserOperator);

        var result = parserOperator.ParseToken("a-b+c", parser);

        Assert.NotNull(result);
        Assert.Equal(TokenType.Sum, result.TokenType);
        Assert.Equal(["a", "b", "c"], parserOperator.ParsedExpressions);
        Assert.Equal(1, parserOperator.InverseCalls);
        Assert.Equal(TokenType.Number, parserOperator.CompileNodes[0].TokenType);
        Assert.Equal(TokenType.Product, parserOperator.CompileNodes[1].TokenType);
        Assert.Equal(TokenType.Number, parserOperator.CompileNodes[2].TokenType);
    }

    [Fact]
    public void ShouldIgnoreWhitespaceOutsideIndicesLevelOnly()
    {
        var parserOperator = new TestParserOperator('+', '-');
        var parser = CreateParser(parserOperator);

        var result = parserOperator.ParseToken(" a_{i j} + b ", parser);

        Assert.NotNull(result);
        Assert.Equal(["a_{i j}", "b"], parserOperator.ParsedExpressions);
    }

    [Fact]
    public void ShouldNormalizeAdjacentSignsBeforeSplitting()
    {
        var parserOperator = new TestParserOperator('+', '-');
        var parser = CreateParser(parserOperator);

        var result = parserOperator.ParseToken("a--b++c+-d-+e", parser);

        Assert.NotNull(result);
        Assert.Equal(["a", "b", "c", "d", "e"], parserOperator.ParsedExpressions);
        Assert.Equal(2, parserOperator.InverseCalls);
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

        Assert.NotNull(result);
        Assert.Equal(["a**b", "c"], parserOperator.ParsedExpressions);
        Assert.Equal(TokenType.Sum, result.TokenType);
    }

    [Fact]
    public void ShouldReturnNullWhenAllCandidateOperatorsAreRejectedByTestOperator()
    {
        var parserOperator = new TestParserOperator('*', '/', (_, _) => false);
        var parser = CreateParser(parserOperator);

        var result = parserOperator.ParseToken("a*b", parser);

        Assert.Null(result);
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
