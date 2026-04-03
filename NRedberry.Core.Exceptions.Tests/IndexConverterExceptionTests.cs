using Xunit;

namespace NRedberry.Core.Exceptions.Tests;

public sealed class IndexConverterExceptionTests
{
    [Fact]
    public void ShouldExposeStandardExceptionConstructors()
    {
        InvalidOperationException inner = new("inner");
        IndexConverterException withoutMessage = new();
        IndexConverterException withMessage = new("broken");
        IndexConverterException withInner = new("outer", inner);

        withoutMessage.Message.ShouldContain(nameof(IndexConverterException));
        withMessage.Message.ShouldBe("broken");
        withInner.Message.ShouldBe("outer");
        withInner.InnerException.ShouldBeSameAs(inner);
    }
}

public sealed class InconsistentGeneratorsExceptionTests
{
    [Fact]
    public void ShouldExposeStandardExceptionConstructors()
    {
        InvalidOperationException inner = new("inner");
        InconsistentGeneratorsException withoutMessage = new();
        InconsistentGeneratorsException withMessage = new("broken");
        InconsistentGeneratorsException withInner = new("outer", inner);

        withoutMessage.Message.ShouldContain(nameof(InconsistentGeneratorsException));
        withMessage.Message.ShouldBe("broken");
        withInner.Message.ShouldBe("outer");
        withInner.InnerException.ShouldBeSameAs(inner);
    }
}
