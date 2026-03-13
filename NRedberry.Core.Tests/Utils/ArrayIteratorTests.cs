using NRedberry.Core.Utils;
using Xunit;

namespace NRedberry.Core.Tests.Utils;

public sealed class ArrayIteratorTests
{
    [Fact]
    public void ShouldRemainMarkedObsoleteWithError()
    {
        Type? type = typeof(IntTimSort).Assembly.GetType("NRedberry.Core.Utils.ArrayIterator`1");

        Assert.NotNull(type);

        ObsoleteAttribute obsolete = Assert.IsType<ObsoleteAttribute>(
            Attribute.GetCustomAttribute(type!, typeof(ObsoleteAttribute)));

        Assert.True(obsolete.IsError);
        Assert.Contains("enumerated directly", obsolete.Message);
    }
}
