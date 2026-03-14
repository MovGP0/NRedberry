using NRedberry.Core.Utils;
using Xunit;

namespace NRedberry.Core.Tests.Utils;

public sealed class ArrayIteratorTests
{
    [Fact]
    public void ShouldRemainMarkedObsoleteWithError()
    {
        Type? type = typeof(IntTimSort).Assembly.GetType("NRedberry.Core.Utils.ArrayIterator`1");

        type.ShouldNotBeNull();

        ObsoleteAttribute obsolete = Attribute.GetCustomAttribute(type!, typeof(ObsoleteAttribute)).ShouldBeOfType<ObsoleteAttribute>();

        obsolete.IsError.ShouldBeTrue();
        obsolete.Message.ShouldContain("enumerated directly");
    }
}
