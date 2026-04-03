using System.Reflection;
using NRedberry.Transformations.Options;

namespace NRedberry.Core.Tests.Transformations.Options;

public sealed class CreatorAttributeTests
{
    [Fact]
    public void ShouldTargetConstructorsOnly()
    {
        AttributeUsageAttribute usage = typeof(CreatorAttribute).GetCustomAttribute(typeof(AttributeUsageAttribute)).ShouldBeOfType<AttributeUsageAttribute>();

        usage.ValidOn.ShouldBe(AttributeTargets.Constructor);
    }

    [Fact]
    public void ShouldExposeConfigurableFlags()
    {
        CreatorAttribute attribute = new()
        {
            Vararg = true,
            HasArgs = true
        };

        attribute.Vararg.ShouldBeTrue();
        attribute.HasArgs.ShouldBeTrue();
    }
}
