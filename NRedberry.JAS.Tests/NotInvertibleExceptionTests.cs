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

        exception.Message.ShouldBe("NotInvertibleException");
        custom.Message.ShouldBe("custom");
    }

    [Fact]
    public void ShouldPreserveInnerExceptions()
    {
        InvalidOperationException inner = new("inner");
        NotInvertibleException withMessage = new("outer", inner);
        NotInvertibleException withDefaultMessage = new(inner);

        withMessage.InnerException.ShouldBeSameAs(inner);
        withDefaultMessage.InnerException.ShouldBeSameAs(inner);
        withMessage.Message.ShouldBe("outer");
        withDefaultMessage.Message.ShouldBe("NotInvertibleException");
    }
}
