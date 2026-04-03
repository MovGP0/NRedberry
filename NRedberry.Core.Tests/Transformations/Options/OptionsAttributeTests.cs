using System.Reflection;
using NRedberry.Transformations.Options;

namespace NRedberry.Core.Tests.Transformations.Options;

public sealed class OptionsAttributeTests
{
    [Fact]
    public void ShouldTargetFieldsAndParameters()
    {
        AttributeUsageAttribute usage = typeof(OptionsAttribute).GetCustomAttribute(typeof(AttributeUsageAttribute)).ShouldBeOfType<AttributeUsageAttribute>();

        usage.ValidOn.ShouldBe(AttributeTargets.Parameter | AttributeTargets.Field);
    }
}
