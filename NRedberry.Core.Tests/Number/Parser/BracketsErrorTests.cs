using NRedberry.Numbers.Parser;
using Xunit;

namespace NRedberry.Core.Tests.Number.Parser;

public sealed class BracketsErrorTests
{
    [Fact]
    public void ShouldSetDefaultMessageWhenCreatedWithoutArguments()
    {
        var error = new BracketsError();

        Assert.Equal("Unbalanced brackets.", error.Message);
    }

    [Fact]
    public void ShouldSetContextualMessageWhenCreatedWithMessage()
    {
        var error = new BracketsError("expression");

        Assert.Equal("Unbalanced brackets in expression", error.Message);
    }

    [Fact]
    public void ShouldSetInnerExceptionWhenCreatedWithMessageAndInnerException()
    {
        var innerException = new InvalidOperationException("inner");
        var error = new BracketsError("expression", innerException);

        Assert.Equal("Unbalanced brackets in expression", error.Message);
        Assert.Same(innerException, error.InnerException);
    }
}
