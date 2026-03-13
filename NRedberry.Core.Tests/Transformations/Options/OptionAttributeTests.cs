using System.Reflection;
using NRedberry.Transformations.Options;
using Xunit;

namespace NRedberry.Core.Tests.Transformations.Options;

public sealed class OptionAttributeTests
{
    [Fact]
    public void ShouldTargetFieldsAndParameters()
    {
        AttributeUsageAttribute usage = Assert.IsType<AttributeUsageAttribute>(
            typeof(OptionAttribute).GetCustomAttribute(typeof(AttributeUsageAttribute)));

        Assert.Equal(AttributeTargets.Field | AttributeTargets.Parameter, usage.ValidOn);
    }

    [Fact]
    public void ShouldStoreNameAndIndex()
    {
        OptionAttribute attribute = new()
        {
            Name = "alpha",
            Index = 2
        };

        Assert.Equal("alpha", attribute.Name);
        Assert.Equal(2, attribute.Index);
    }
}
