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

        firstInstance.ShouldNotBeNull();
        secondInstance.ShouldBeSameAs(firstInstance);
        firstInstance.ShouldBeAssignableTo<IIndexMappingProviderFactory>();
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

        privateConstructor.ShouldNotBeNull();
        privateConstructor.IsPrivate.ShouldBeTrue();
        providerPowerType.GetConstructors(BindingFlags.Instance | BindingFlags.Public).ShouldBeEmpty();
        Should.Throw<MissingMethodException>(() => Activator.CreateInstance(providerPowerType));
    }

    [Fact]
    public void CreateShouldThrowWhenProviderIsNull()
    {
        IIndexMappingProviderFactory factory = GetFactory();
        Power from = CreatePowerTensor();
        Power to = CreatePowerTensor();

        ArgumentNullException exception = Should.Throw<ArgumentNullException>(() => factory.Create(null!, from, to));

        exception.ParamName.ShouldBe("provider");
    }

    [Fact]
    public void CreateShouldThrowWhenFromIsNull()
    {
        IIndexMappingProviderFactory factory = GetFactory();
        IIndexMappingProvider provider = new FakeIndexMappingProvider();
        Power to = CreatePowerTensor();

        ArgumentNullException exception = Should.Throw<ArgumentNullException>(() => factory.Create(provider, null!, to));

        exception.ParamName.ShouldBe("from");
    }

    [Fact]
    public void CreateShouldThrowWhenToIsNull()
    {
        IIndexMappingProviderFactory factory = GetFactory();
        IIndexMappingProvider provider = new FakeIndexMappingProvider();
        Power from = CreatePowerTensor();

        ArgumentNullException exception = Should.Throw<ArgumentNullException>(() => factory.Create(provider, from, null!));

        exception.ParamName.ShouldBe("to");
    }

    [Fact]
    public void CreateShouldReturnDummyProviderForEqualPositivePowers()
    {
        IIndexMappingProviderFactory factory = GetFactory();
        IIndexMappingProvider provider = new FakeIndexMappingProvider();
        Power from = CreatePowerTensor();
        Power to = CreatePowerTensor();

        IIndexMappingProvider result = factory.Create(provider, from, to);

        result.GetType().Name.ShouldBe("DummyIndexMappingProvider");
    }

    private static Type GetProviderPowerType()
    {
        Type? providerPowerType = typeof(IndexMappingProviderAbstract).Assembly
            .GetType("NRedberry.IndexMapping.ProviderPower", throwOnError: false);

        providerPowerType.ShouldNotBeNull();
        return providerPowerType;
    }

    private static object GetInstance()
    {
        Type providerPowerType = GetProviderPowerType();
        PropertyInfo? instanceProperty = providerPowerType.GetProperty("Instance", BindingFlags.Public | BindingFlags.Static);
        instanceProperty.ShouldNotBeNull();

        object? instance = instanceProperty.GetValue(obj: null);
        instance.ShouldNotBeNull();
        return instance;
    }

    private static IIndexMappingProviderFactory GetFactory()
    {
        return GetInstance().ShouldBeAssignableTo<IIndexMappingProviderFactory>();
    }

    private static Power CreatePowerTensor()
    {
        return new Power(Complex.Two, new Complex(3));
    }
}
