using System.Reflection;
using NRedberry.Transformations.Options;
using Xunit;

namespace NRedberry.Core.Tests.Transformations.Options;

public sealed class CreatorAttributeTests
{
    [Fact]
    public void ShouldTargetConstructorsOnly()
    {
        AttributeUsageAttribute usage = Assert.IsType<AttributeUsageAttribute>(
            typeof(CreatorAttribute).GetCustomAttribute(typeof(AttributeUsageAttribute)));

        Assert.Equal(AttributeTargets.Constructor, usage.ValidOn);
    }

    [Fact]
    public void ShouldExposeConfigurableFlags()
    {
        CreatorAttribute attribute = new()
        {
            Vararg = true,
            HasArgs = true
        };

        Assert.True(attribute.Vararg);
        Assert.True(attribute.HasArgs);
    }
}
