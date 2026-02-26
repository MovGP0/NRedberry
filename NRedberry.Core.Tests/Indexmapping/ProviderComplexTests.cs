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

        Assert.Equal("NRedberry.IndexMapping.PlusMinusIndexMappingProvider", result.GetType().FullName);
    }

    [Fact]
    public void CreateShouldReturnDummyProviderWhenFromEqualsToAndIsNonZero()
    {
        IIndexMappingProvider provider = new FakeIndexMappingProvider();

        IIndexMappingProvider result = InvokeProviderComplexCreate(provider, Complex.One, Complex.One);

        Assert.Equal("NRedberry.IndexMapping.DummyIndexMappingProvider", result.GetType().FullName);
    }

    [Fact]
    public void CreateShouldReturnMinusProviderWhenFromEqualsNegativeOfTo()
    {
        IIndexMappingProvider provider = new FakeIndexMappingProvider();

        IIndexMappingProvider result = InvokeProviderComplexCreate(provider, Complex.One, Complex.MinusOne);

        Assert.Equal("NRedberry.IndexMapping.MinusIndexMappingProvider", result.GetType().FullName);
    }

    [Fact]
    public void CreateShouldReturnEmptyProviderWhenNoMatchConditionApplies()
    {
        IIndexMappingProvider provider = new FakeIndexMappingProvider();

        IIndexMappingProvider result = InvokeProviderComplexCreate(provider, Complex.One, Complex.Two);

        Assert.Same(IndexMappingProviderUtil.EmptyProvider, result);
    }

    [Fact]
    public void FactoryPropertyShouldImplementFactoryInterfaceAndCreateShouldDelegateToProviderComplexCreate()
    {
        IIndexMappingProvider provider = new FakeIndexMappingProvider();
        IIndexMappingProviderFactory factory = GetFactoryFromProviderComplexProperty();

        Assert.True(factory.GetType().FullName == "NRedberry.IndexMapping.ProviderComplexFactory");

        AssertEquivalentCreate(provider, Complex.Zero, Complex.Zero, factory, "NRedberry.IndexMapping.PlusMinusIndexMappingProvider");
        AssertEquivalentCreate(provider, Complex.One, Complex.One, factory, "NRedberry.IndexMapping.DummyIndexMappingProvider");
        AssertEquivalentCreate(provider, Complex.One, Complex.MinusOne, factory, "NRedberry.IndexMapping.MinusIndexMappingProvider");

        IIndexMappingProvider direct = InvokeProviderComplexCreate(provider, Complex.One, Complex.Two);
        IIndexMappingProvider fromFactory = factory.Create(provider, Complex.One, Complex.Two);
        Assert.Same(IndexMappingProviderUtil.EmptyProvider, direct);
        Assert.Same(IndexMappingProviderUtil.EmptyProvider, fromFactory);
        Assert.Same(direct, fromFactory);
    }

    private static void AssertEquivalentCreate(
        IIndexMappingProvider provider,
        TensorType from,
        TensorType to,
        IIndexMappingProviderFactory factory,
        string expectedProviderTypeName)
    {
        IIndexMappingProvider direct = InvokeProviderComplexCreate(provider, from, to);
        IIndexMappingProvider fromFactory = factory.Create(provider, from, to);

        Assert.Equal(expectedProviderTypeName, direct.GetType().FullName);
        Assert.Equal(expectedProviderTypeName, fromFactory.GetType().FullName);
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

        Assert.NotNull(method);
        object? result = method.Invoke(obj: null, [provider, from, to]);
        return Assert.IsAssignableFrom<IIndexMappingProvider>(result);
    }

    private static IIndexMappingProviderFactory GetFactoryFromProviderComplexProperty()
    {
        Type providerComplexType = GetProviderComplexType();
        PropertyInfo property = providerComplexType.GetProperty("Factory", BindingFlags.Public | BindingFlags.Static)!;

        Assert.NotNull(property);
        object? instance = property.GetValue(obj: null);
        return Assert.IsAssignableFrom<IIndexMappingProviderFactory>(instance);
    }

    private static Type GetProviderComplexType()
    {
        Type? providerComplexType = typeof(IndexMappingProviderAbstract).Assembly
            .GetType("NRedberry.IndexMapping.ProviderComplex", throwOnError: false);

        Assert.True(providerComplexType is not null);
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
