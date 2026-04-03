using System.Reflection;
using NRedberry.Concurrent;
using NRedberry.IndexMapping;

namespace NRedberry.Core.Tests.Indexmapping;

#pragma warning disable CS0618
public sealed class MinusIndexMappingProviderTests
{
    [Fact]
    public void ConstructorShouldThrowWhenOutputPortIsNull()
    {
        Type providerType = GetProviderType();
        ConstructorInfo constructor = providerType.GetConstructor(
            BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
            binder: null,
            [typeof(IOutputPort<IIndexMappingBuffer>)],
            modifiers: null)!;

        TargetInvocationException exception = Should.Throw<TargetInvocationException>(() =>
            constructor.Invoke([null!]));

        ArgumentNullException argumentNullException = exception.InnerException.ShouldBeOfType<ArgumentNullException>();
        argumentNullException.ParamName.ShouldBe("outputPort");
    }

    [Fact]
    public void TakeShouldReturnNullWhenCurrentBufferAbsent()
    {
        MinusIndexMappingProviderTestsOutputPort outputPort = new([]);
        object provider = CreateProvider(outputPort);

        IIndexMappingBuffer? buffer = (IIndexMappingBuffer?)InvokeTake(provider);

        buffer.ShouldBeNull();
    }

    [Fact]
    public void TakeShouldReturnSameBufferAddSignOnceAndClearAfterTick()
    {
        MinusIndexMappingProviderTestsBuffer buffer = new();
        MinusIndexMappingProviderTestsOutputPort outputPort = new([buffer]);
        object provider = CreateProvider(outputPort);

        bool tickResult = InvokeTick(provider);
        IIndexMappingBuffer? firstTake = (IIndexMappingBuffer?)InvokeTake(provider);
        IIndexMappingBuffer? secondTake = (IIndexMappingBuffer?)InvokeTake(provider);

        tickResult.ShouldBeTrue();
        firstTake.ShouldBeSameAs(buffer);
        buffer.AddSignCallCount.ShouldBe(1);
        buffer.AddSignTrueCallCount.ShouldBe(1);
        secondTake.ShouldBeNull();
    }

    private static Type GetProviderType()
    {
        return typeof(IndexMappingProviderAbstract).Assembly.GetType(
            "NRedberry.IndexMapping.MinusIndexMappingProvider",
            throwOnError: true)!;
    }

    private static object CreateProvider(IOutputPort<IIndexMappingBuffer> outputPort)
    {
        return Activator.CreateInstance(GetProviderType(), outputPort)!;
    }

    private static bool InvokeTick(object provider)
    {
        MethodInfo tickMethod = provider.GetType().GetMethod(nameof(IIndexMappingProvider.Tick), Type.EmptyTypes)!;
        return (bool)tickMethod.Invoke(provider, null)!;
    }

    private static object? InvokeTake(object provider)
    {
        MethodInfo takeMethod = provider.GetType().GetMethod(nameof(IIndexMappingProvider.Take), Type.EmptyTypes)!;
        return takeMethod.Invoke(provider, null);
    }
}
#pragma warning restore CS0618

file sealed class MinusIndexMappingProviderTestsOutputPort(IEnumerable<IIndexMappingBuffer?> values)
    : IOutputPort<IIndexMappingBuffer>
{
    private readonly Queue<IIndexMappingBuffer?> _values = new(values);

    public IIndexMappingBuffer Take()
    {
        if (_values.Count == 0)
        {
            return null!;
        }

        return _values.Dequeue()!;
    }
}

file sealed class MinusIndexMappingProviderTestsBuffer : IIndexMappingBuffer
{
    public int AddSignCallCount { get; private set; }

    public int AddSignTrueCallCount { get; private set; }

    public bool TryMap(int from, int to)
    {
        throw new NotSupportedException();
    }

    public void AddSign(bool sign)
    {
        AddSignCallCount++;

        if (sign)
        {
            AddSignTrueCallCount++;
        }
    }

    public void RemoveContracted()
    {
        throw new NotSupportedException();
    }

    public bool IsEmpty()
    {
        throw new NotSupportedException();
    }

    public bool GetSign()
    {
        throw new NotSupportedException();
    }

    public object Export()
    {
        throw new NotSupportedException();
    }

    public IDictionary<int, IndexMappingBufferRecord> GetMap()
    {
        throw new NotSupportedException();
    }

    public object Clone()
    {
        throw new NotSupportedException();
    }
}
