using System;
using NRedberry.Parsers;
using Xunit;

namespace NRedberry.Core.Tests.Parsers;

public sealed class ParserExceptionTests
{
    [Fact]
    public void ShouldDeriveFromExceptionWhenTypeIsInspected()
    {
        Assert.True(typeof(Exception).IsAssignableFrom(typeof(ParserException)));
    }

    [Fact]
    public void ShouldHaveNoInnerExceptionWhenCreatedWithoutArguments()
    {
        var exception = new ParserException();

        Assert.Null(exception.InnerException);
        Assert.False(string.IsNullOrWhiteSpace(exception.Message));
    }

    [Fact]
    public void ShouldSetMessageWhenCreatedWithMessage()
    {
        const string expectedMessage = "parser failed";
        var exception = new ParserException(expectedMessage);

        Assert.Equal(expectedMessage, exception.Message);
        Assert.Null(exception.InnerException);
    }

    [Fact]
    public void ShouldSetMessageAndInnerExceptionWhenCreatedWithMessageAndInnerException()
    {
        const string expectedMessage = "parser failed";
        var innerException = new InvalidOperationException("inner");

        var exception = new ParserException(expectedMessage, innerException);

        Assert.Equal(expectedMessage, exception.Message);
        Assert.Same(innerException, exception.InnerException);
    }
}
