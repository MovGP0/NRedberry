using NRedberry.Numbers.Parser;

namespace NRedberry.Core.Tests.Number.Parser;

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

    [Fact]
    public void ShouldSetInnerExceptionWhenCreatedWithMessageAndInnerException()
    {
        var innerException = new InvalidOperationException("inner");
        var error = new BracketsError("expression", innerException);

        error.Message.ShouldBe("Unbalanced brackets in expression");
        error.InnerException.ShouldBeSameAs(innerException);
    }
}
