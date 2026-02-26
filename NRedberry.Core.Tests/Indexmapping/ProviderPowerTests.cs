using System.Reflection;
using NRedberry.IndexMapping;
using NRedberry.Numbers;
using NRedberry.Tensors;
using Xunit;

namespace NRedberry.Core.Tests.Indexmapping;

public sealed class ProviderPowerTests
{
    [Fact]
    public void InstanceShouldBeNonNullSingleton()
    {
        object firstInstance = GetInstance();
        object secondInstance = GetInstance();

        Assert.NotNull(firstInstance);
        Assert.Same(firstInstance, secondInstance);
        Assert.IsAssignableFrom<IIndexMappingProviderFactory>(firstInstance);
    }

    [Fact]
    public void ConstructorShouldBePrivateAndNotPubliclyAccessible()
    {
        Type providerPowerType = GetProviderPowerType();

        ConstructorInfo? privateConstructor = providerPowerType.GetConstructor(
            BindingFlags.Instance | BindingFlags.NonPublic,
            binder: null,
            Type.EmptyTypes,
            modifiers: null);

        Assert.NotNull(privateConstructor);
        Assert.True(privateConstructor.IsPrivate);
        Assert.Empty(providerPowerType.GetConstructors(BindingFlags.Instance | BindingFlags.Public));
        Assert.Throws<MissingMethodException>(() => Activator.CreateInstance(providerPowerType));
    }

    [Fact]
    public void CreateShouldThrowWhenProviderIsNull()
    {
        IIndexMappingProviderFactory factory = GetFactory();
        Power from = CreatePowerTensor();
        Power to = CreatePowerTensor();

        ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => factory.Create(null!, from, to));

        Assert.Equal("provider", exception.ParamName);
    }

    [Fact]
    public void CreateShouldThrowWhenFromIsNull()
    {
        IIndexMappingProviderFactory factory = GetFactory();
        IIndexMappingProvider provider = new FakeIndexMappingProvider();
        Power to = CreatePowerTensor();

        ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => factory.Create(provider, null!, to));

        Assert.Equal("from", exception.ParamName);
    }

    [Fact]
    public void CreateShouldThrowWhenToIsNull()
    {
        IIndexMappingProviderFactory factory = GetFactory();
        IIndexMappingProvider provider = new FakeIndexMappingProvider();
        Power from = CreatePowerTensor();

        ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => factory.Create(provider, from, null!));

        Assert.Equal("to", exception.ParamName);
    }

    [Fact]
    public void CreateShouldThrowNotImplementedForCurrentNonNullPath()
    {
        IIndexMappingProviderFactory factory = GetFactory();
        IIndexMappingProvider provider = new FakeIndexMappingProvider();
        Power from = CreatePowerTensor();
        Power to = CreatePowerTensor();

        Assert.Throws<NotImplementedException>(() => factory.Create(provider, from, to));
    }

    private static Type GetProviderPowerType()
    {
        Type? providerPowerType = typeof(IndexMappingProviderAbstract).Assembly
            .GetType("NRedberry.IndexMapping.ProviderPower", throwOnError: false);

        Assert.NotNull(providerPowerType);
        return providerPowerType;
    }

    private static object GetInstance()
    {
        Type providerPowerType = GetProviderPowerType();
        PropertyInfo? instanceProperty = providerPowerType.GetProperty("Instance", BindingFlags.Public | BindingFlags.Static);
        Assert.NotNull(instanceProperty);

        object? instance = instanceProperty.GetValue(obj: null);
        Assert.NotNull(instance);
        return instance;
    }

    private static IIndexMappingProviderFactory GetFactory()
    {
        return Assert.IsAssignableFrom<IIndexMappingProviderFactory>(GetInstance());
    }

    private static Power CreatePowerTensor()
    {
        return new Power(Complex.Two, new Complex(3));
    }
}
