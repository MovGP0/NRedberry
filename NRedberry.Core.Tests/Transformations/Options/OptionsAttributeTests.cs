using System.Reflection;
using NRedberry.Transformations.Options;
using Xunit;

namespace NRedberry.Core.Tests.Transformations.Options;

public sealed class OptionsAttributeTests
{
    [Fact]
    public void ShouldTargetFieldsAndParameters()
    {
        AttributeUsageAttribute usage = Assert.IsType<AttributeUsageAttribute>(
            typeof(OptionsAttribute).GetCustomAttribute(typeof(AttributeUsageAttribute)));

        Assert.Equal(AttributeTargets.Parameter | AttributeTargets.Field, usage.ValidOn);
    }
}
