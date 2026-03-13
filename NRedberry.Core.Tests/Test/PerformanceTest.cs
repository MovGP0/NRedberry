using System.Reflection;
using Xunit;

namespace NRedberry.Core.Tests.Test;

[AttributeUsage(AttributeTargets.Method, Inherited = false)]
public sealed class PerformanceTestAttribute : Attribute
{
}

public sealed class PerformanceTestAttributeTests
{
    [Fact]
    public void ShouldTargetMethodsOnly()
    {
        AttributeUsageAttribute attributeUsage = Assert.IsType<AttributeUsageAttribute>(
            Attribute.GetCustomAttribute(typeof(PerformanceTestAttribute), typeof(AttributeUsageAttribute)));

        Assert.Equal(AttributeTargets.Method, attributeUsage.ValidOn);
        Assert.False(attributeUsage.Inherited);
    }
}
