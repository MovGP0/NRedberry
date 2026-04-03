namespace NRedberry.Core.Tests.Test;

[AttributeUsage(AttributeTargets.Method, Inherited = false)]
public sealed class LongTestAttribute : Attribute;

public sealed class LongTestAttributeTests
{
    [Fact]
    public void ShouldTargetMethodsOnly()
    {
        AttributeUsageAttribute attributeUsage = Attribute.GetCustomAttribute(typeof(LongTestAttribute), typeof(AttributeUsageAttribute)).ShouldBeOfType<AttributeUsageAttribute>();

        attributeUsage.ValidOn.ShouldBe(AttributeTargets.Method);
        attributeUsage.Inherited.ShouldBeFalse();
    }
}
