using System;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;
using Xunit;

namespace NRedberry.JAS.Tests;

public sealed class NotInvertibleExceptionTests
{
    [Fact]
    public void ShouldExposeDefaultAndMessageConstructors()
    {
        NotInvertibleException exception = new();
        NotInvertibleException custom = new("custom");

        Assert.Equal("NotInvertibleException", exception.Message);
        Assert.Equal("custom", custom.Message);
    }

    [Fact]
    public void ShouldPreserveInnerExceptions()
    {
        InvalidOperationException inner = new("inner");
        NotInvertibleException withMessage = new("outer", inner);
        NotInvertibleException withDefaultMessage = new(inner);

        Assert.Same(inner, withMessage.InnerException);
        Assert.Same(inner, withDefaultMessage.InnerException);
        Assert.Equal("outer", withMessage.Message);
        Assert.Equal("NotInvertibleException", withDefaultMessage.Message);
    }
}
