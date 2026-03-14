using System;
using NRedberry.Parsers;
using Xunit;

namespace NRedberry.Core.Tests.Parsers;

public sealed class ParserExceptionTests
{
    [Fact]
    public void ShouldDeriveFromExceptionWhenTypeIsInspected()
    {
        typeof(Exception).IsAssignableFrom(typeof(ParserException)).ShouldBeTrue();
    }

    [Fact]
    public void ShouldHaveNoInnerExceptionWhenCreatedWithoutArguments()
    {
        var exception = new ParserException();

        exception.InnerException.ShouldBeNull();
        string.IsNullOrWhiteSpace(exception.Message).ShouldBeFalse();
    }

    [Fact]
    public void ShouldSetMessageWhenCreatedWithMessage()
    {
        const string expectedMessage = "parser failed";
        var exception = new ParserException(expectedMessage);

        exception.Message.ShouldBe(expectedMessage);
        exception.InnerException.ShouldBeNull();
    }

    [Fact]
    public void ShouldSetMessageAndInnerExceptionWhenCreatedWithMessageAndInnerException()
    {
        const string expectedMessage = "parser failed";
        var innerException = new InvalidOperationException("inner");

        var exception = new ParserException(expectedMessage, innerException);

        exception.Message.ShouldBe(expectedMessage);
        exception.InnerException.ShouldBeSameAs(innerException);
    }
}
