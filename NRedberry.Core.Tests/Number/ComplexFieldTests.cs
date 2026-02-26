using NRedberry.Numbers;
using Xunit;

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

        Assert.Same(instance, fromGetInstance);
        Assert.Same(instance, ComplexField.Instance);
#pragma warning disable CS0618
        Assert.Same(fromGetInstance, ComplexField.GetInstance());
#pragma warning restore CS0618
    }

    [Fact]
    public void ShouldReturnOneAndZero()
    {
        var field = ComplexField.Instance;

        Assert.Same(Complex.One, field.One);
        Assert.Same(Complex.Zero, field.Zero);
    }

    [Fact]
    public void ShouldReturnComplexRuntimeClass()
    {
        var field = ComplexField.Instance;

        Assert.Equal(typeof(Complex), field.GetRuntimeClass());
    }
}
