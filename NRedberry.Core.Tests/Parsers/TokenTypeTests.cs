using System;
using System.Linq;
using NRedberry.Parsers;
using Xunit;

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

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ShouldAssignSequentialUnderlyingValuesStartingAtZero()
    {
        var values = Enum.GetValues<TokenType>();

        Assert.Equal(Enumerable.Range(0, values.Length), values.Select(static value => (int)value));
    }

    [Theory]
    [MemberData(nameof(GetAllTokenTypes))]
    public void ShouldRoundTripNameParsing(TokenType tokenType)
    {
        var name = tokenType.ToString();
        var isParsed = Enum.TryParse<TokenType>(name, out var parsed);

        Assert.True(isParsed);
        Assert.Equal(tokenType, parsed);
    }

    [Fact]
    public void ShouldNotParseUnknownOrWrongCaseValues()
    {
        Assert.False(Enum.TryParse<TokenType>("Unknown", out _));
        Assert.False(Enum.TryParse<TokenType>("sum", out _));
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
