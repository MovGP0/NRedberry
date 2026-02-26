using NRedberry.Parsers;
using Xunit;

namespace NRedberry.Core.Tests.Parsers;

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
}
