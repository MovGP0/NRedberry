using System.Reflection;
using NRedberry.IndexMapping;
using NRedberry.Numbers;
using NRedberry.Tensors;
using Xunit;
using TensorType = NRedberry.Tensors.Tensor;

namespace NRedberry.Core.Tests.Indexmapping;

public sealed class ProviderComplexTests
{
    [Fact]
    public void CreateShouldReturnPlusMinusProviderWhenFromEqualsToAndBothAreZero()
    {
        IIndexMappingProvider provider = new FakeIndexMappingProvider();

        IIndexMappingProvider result = InvokeProviderComplexCreate(provider, Complex.Zero, Complex.Zero);

        result.GetType().FullName.ShouldBe("NRedberry.IndexMapping.PlusMinusIndexMappingProvider");
    }

    [Fact]
    public void CreateShouldReturnDummyProviderWhenFromEqualsToAndIsNonZero()
    {
        IIndexMappingProvider provider = new FakeIndexMappingProvider();

        IIndexMappingProvider result = InvokeProviderComplexCreate(provider, Complex.One, Complex.One);

        result.GetType().FullName.ShouldBe("NRedberry.IndexMapping.DummyIndexMappingProvider");
    }

    [Fact]
    public void CreateShouldReturnMinusProviderWhenFromEqualsNegativeOfTo()
    {
        IIndexMappingProvider provider = new FakeIndexMappingProvider();

        IIndexMappingProvider result = InvokeProviderComplexCreate(provider, Complex.One, Complex.MinusOne);

        result.GetType().FullName.ShouldBe("NRedberry.IndexMapping.MinusIndexMappingProvider");
    }

    [Fact]
    public void CreateShouldReturnEmptyProviderWhenNoMatchConditionApplies()
    {
        IIndexMappingProvider provider = new FakeIndexMappingProvider();

        IIndexMappingProvider result = InvokeProviderComplexCreate(provider, Complex.One, Complex.Two);

        result.ShouldBeSameAs(IndexMappingProviderUtil.EmptyProvider);
    }

    [Fact]
    public void FactoryPropertyShouldImplementFactoryInterfaceAndCreateShouldDelegateToProviderComplexCreate()
    {
        IIndexMappingProvider provider = new FakeIndexMappingProvider();
        IIndexMappingProviderFactory factory = GetFactoryFromProviderComplexProperty();

        (factory.GetType().FullName == "NRedberry.IndexMapping.ProviderComplexFactory").ShouldBeTrue();

        ShouldCreateEquivalentProvider(provider, Complex.Zero, Complex.Zero, factory, "NRedberry.IndexMapping.PlusMinusIndexMappingProvider");
        ShouldCreateEquivalentProvider(provider, Complex.One, Complex.One, factory, "NRedberry.IndexMapping.DummyIndexMappingProvider");
        ShouldCreateEquivalentProvider(provider, Complex.One, Complex.MinusOne, factory, "NRedberry.IndexMapping.MinusIndexMappingProvider");

        IIndexMappingProvider direct = InvokeProviderComplexCreate(provider, Complex.One, Complex.Two);
        IIndexMappingProvider fromFactory = factory.Create(provider, Complex.One, Complex.Two);
        direct.ShouldBeSameAs(IndexMappingProviderUtil.EmptyProvider);
        fromFactory.ShouldBeSameAs(IndexMappingProviderUtil.EmptyProvider);
        fromFactory.ShouldBeSameAs(direct);
    }

    private static void ShouldCreateEquivalentProvider(
        IIndexMappingProvider provider,
        TensorType from,
        TensorType to,
        IIndexMappingProviderFactory factory,
        string expectedProviderTypeName)
    {
        IIndexMappingProvider direct = InvokeProviderComplexCreate(provider, from, to);
        IIndexMappingProvider fromFactory = factory.Create(provider, from, to);

        direct.GetType().FullName.ShouldBe(expectedProviderTypeName);
        fromFactory.GetType().FullName.ShouldBe(expectedProviderTypeName);
    }

    private static IIndexMappingProvider InvokeProviderComplexCreate(
        IIndexMappingProvider provider,
        TensorType from,
        TensorType to)
    {
        Type providerComplexType = GetProviderComplexType();
        MethodInfo method = providerComplexType.GetMethod(
            "Create",
            BindingFlags.Public | BindingFlags.Static,
            binder: null,
            [typeof(IIndexMappingProvider), typeof(TensorType), typeof(TensorType)],
            modifiers: null)!;

        method.ShouldNotBeNull();
        object? result = method.Invoke(obj: null, [provider, from, to]);
        return result.ShouldBeAssignableTo<IIndexMappingProvider>();
    }

    private static IIndexMappingProviderFactory GetFactoryFromProviderComplexProperty()
    {
        Type providerComplexType = GetProviderComplexType();
        PropertyInfo property = providerComplexType.GetProperty("Factory", BindingFlags.Public | BindingFlags.Static)!;

        property.ShouldNotBeNull();
        object? instance = property.GetValue(obj: null);
        return instance.ShouldBeAssignableTo<IIndexMappingProviderFactory>();
    }

    private static Type GetProviderComplexType()
    {
        Type? providerComplexType = typeof(IndexMappingProviderAbstract).Assembly
            .GetType("NRedberry.IndexMapping.ProviderComplex", throwOnError: false);

        (providerComplexType is not null).ShouldBeTrue();
        return providerComplexType;
    }
}

internal sealed class FakeIndexMappingProvider : IIndexMappingProvider
{
    private readonly IIndexMappingBuffer _buffer = new FakeIndexMappingBuffer();

    public bool Tick()
    {
        return false;
    }

    public IIndexMappingBuffer Take()
    {
        return _buffer;
    }
}

internal sealed class FakeIndexMappingBuffer : IIndexMappingBuffer
{
    public bool TryMap(int from, int to)
    {
        return true;
    }

    public void AddSign(bool sign)
    {
    }

    public void RemoveContracted()
    {
    }

    public bool IsEmpty()
    {
        return true;
    }

    public bool GetSign()
    {
        return false;
    }

    public object Export()
    {
        return new object();
    }

    public IDictionary<int, IndexMappingBufferRecord> GetMap()
    {
        return new Dictionary<int, IndexMappingBufferRecord>();
    }

    public object Clone()
    {
        return this;
    }
}
