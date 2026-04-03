using NRedberry.Parsers;

namespace NRedberry.Core.Tests.Parsers;

public sealed class TokenTypeTests
{
    [Fact]
    public void ShouldExposeExpectedTokenTypeMembersInDeclaredOrder()
    {
        var expected = new[]
        {
            nameof(TokenType.SimpleTensor),
            nameof(TokenType.TensorField),
            nameof(TokenType.Product),
            nameof(TokenType.Sum),
            nameof(TokenType.Expression),
            nameof(TokenType.Power),
            nameof(TokenType.Number),
            nameof(TokenType.ScalarFunction),
            nameof(TokenType.Derivative),
            nameof(TokenType.Trace),
            nameof(TokenType.Dummy)
        };

        var actual = Enum.GetNames<TokenType>();

        actual.ShouldBe(expected);
    }

    [Fact]
    public void ShouldAssignSequentialUnderlyingValuesStartingAtZero()
    {
        var values = Enum.GetValues<TokenType>();

        values.Select(static value => (int)value).ShouldBe(Enumerable.Range(0, values.Length));
    }

    [Theory]
    [MemberData(nameof(GetAllTokenTypes))]
    public void ShouldRoundTripNameParsing(TokenType tokenType)
    {
        var name = tokenType.ToString();
        var isParsed = Enum.TryParse<TokenType>(name, out var parsed);

        isParsed.ShouldBeTrue();
        parsed.ShouldBe(tokenType);
    }

    [Fact]
    public void ShouldNotParseUnknownOrWrongCaseValues()
    {
        Enum.TryParse<TokenType>("Unknown", out _).ShouldBeFalse();
        Enum.TryParse<TokenType>("sum", out _).ShouldBeFalse();
    }

    public static TheoryData<TokenType> GetAllTokenTypes()
    {
        var data = new TheoryData<TokenType>();

        foreach (var tokenType in Enum.GetValues<TokenType>())
        {
            data.Add(tokenType);
        }

        return data;
    }
}
