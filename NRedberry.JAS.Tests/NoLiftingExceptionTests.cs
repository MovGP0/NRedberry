using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Ufd;
using Xunit;

namespace NRedberry.JAS.Tests;

public sealed class NoLiftingExceptionTests
{
    [Fact]
    public void ShouldSupportStandardExceptionConstructors()
    {
        InvalidOperationException inner = new("inner");
        NoLiftingException empty = new();
        NoLiftingException withMessage = new("failed");
        NoLiftingException withInner = new("failed", inner);

        empty.ShouldNotBeNull();
        withMessage.Message.ShouldBe("failed");
        withInner.InnerException.ShouldBeSameAs(inner);
    }
}
