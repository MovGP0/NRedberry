using NRedberry.Numbers;

namespace NRedberry.Core.Tests.Number;

public sealed class ComplexFieldTests
{
    [Fact]
    public void ShouldReturnSameSingletonForInstanceAndGetInstance()
    {
        var instance = ComplexField.Instance;
#pragma warning disable CS0618
        var fromGetInstance = ComplexField.GetInstance();
#pragma warning restore CS0618

        fromGetInstance.ShouldBeSameAs(instance);
        ComplexField.Instance.ShouldBeSameAs(instance);
#pragma warning disable CS0618
        ComplexField.GetInstance().ShouldBeSameAs(fromGetInstance);
#pragma warning restore CS0618
    }

    [Fact]
    public void ShouldReturnOneAndZero()
    {
        var field = ComplexField.Instance;

        field.One.ShouldBeSameAs(Complex.One);
        field.Zero.ShouldBeSameAs(Complex.Zero);
    }

    [Fact]
    public void ShouldReturnComplexRuntimeClass()
    {
        var field = ComplexField.Instance;

        field.GetRuntimeClass().ShouldBe(typeof(Complex));
    }
}
