using System.Reflection;
using Xunit;

namespace NRedberry.Core.Tests.Test;

[AttributeUsage(AttributeTargets.Method, Inherited = false)]
public sealed class LongTestAttribute : Attribute
{
}

public sealed class LongTestAttributeTests
{
    [Fact]
    public void ShouldTargetMethodsOnly()
    {
        AttributeUsageAttribute attributeUsage = Assert.IsType<AttributeUsageAttribute>(
            Attribute.GetCustomAttribute(typeof(LongTestAttribute), typeof(AttributeUsageAttribute)));

        Assert.Equal(AttributeTargets.Method, attributeUsage.ValidOn);
        Assert.False(attributeUsage.Inherited);
    }
}
