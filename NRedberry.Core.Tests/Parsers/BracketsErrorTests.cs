using NRedberry.Parsers;
using Xunit;

namespace NRedberry.Core.Tests.Parsers;

public sealed class BracketsErrorTests
{
    [Fact]
    public void ShouldSetDefaultMessageWhenCreatedWithoutArguments()
    {
        var error = new BracketsError();

        error.Message.ShouldBe("Unbalanced brackets.");
    }

    [Fact]
    public void ShouldSetContextualMessageWhenCreatedWithMessage()
    {
        var error = new BracketsError("expression");

        error.Message.ShouldBe("Unbalanced brackets in expression");
    }
}
