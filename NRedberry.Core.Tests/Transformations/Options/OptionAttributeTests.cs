using System.Reflection;
using NRedberry.Transformations.Options;

namespace NRedberry.Core.Tests.Transformations.Options;

public sealed class OptionAttributeTests
{
    [Fact]
    public void ShouldTargetFieldsAndParameters()
    {
        AttributeUsageAttribute usage = typeof(OptionAttribute).GetCustomAttribute(typeof(AttributeUsageAttribute)).ShouldBeOfType<AttributeUsageAttribute>();

        usage.ValidOn.ShouldBe(AttributeTargets.Field | AttributeTargets.Parameter);
    }

    [Fact]
    public void ShouldStoreNameAndIndex()
    {
        OptionAttribute attribute = new()
        {
            Name = "alpha",
            Index = 2
        };

        attribute.Name.ShouldBe("alpha");
        attribute.Index.ShouldBe(2);
    }
}
