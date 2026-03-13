using System;
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

        Assert.Contains(nameof(IndexConverterException), withoutMessage.Message, StringComparison.Ordinal);
        Assert.Equal("broken", withMessage.Message);
        Assert.Equal("outer", withInner.Message);
        Assert.Same(inner, withInner.InnerException);
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

        Assert.Contains(nameof(InconsistentGeneratorsException), withoutMessage.Message, StringComparison.Ordinal);
        Assert.Equal("broken", withMessage.Message);
        Assert.Equal("outer", withInner.Message);
        Assert.Same(inner, withInner.InnerException);
    }
}
