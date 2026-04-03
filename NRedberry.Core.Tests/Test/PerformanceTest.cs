namespace NRedberry.Core.Tests.Test;

[AttributeUsage(AttributeTargets.Method, Inherited = false)]
public sealed class PerformanceTestAttribute : Attribute;

public sealed class PerformanceTestAttributeTests
{
    [Fact]
    public void ShouldTargetMethodsOnly()
    {
        AttributeUsageAttribute attributeUsage = Attribute.GetCustomAttribute(typeof(PerformanceTestAttribute), typeof(AttributeUsageAttribute)).ShouldBeOfType<AttributeUsageAttribute>();

        attributeUsage.ValidOn.ShouldBe(AttributeTargets.Method);
        attributeUsage.Inherited.ShouldBeFalse();
    }
}
