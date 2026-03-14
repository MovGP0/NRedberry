using System;
using System.Collections.Generic;
using System.Reflection;
using NRedberry.Concurrent;
using NRedberry.IndexMapping;
using Xunit;

namespace NRedberry.Core.Tests.Indexmapping;

#pragma warning disable CS0618
public sealed class MappingsPortRemovingContractedTests
{
    [Fact]
    public void ConstructorShouldThrowArgumentNullExceptionWhenProviderIsNull()
    {
        Type type = GetTargetType();

        TargetInvocationException exception = Assert.Throws<TargetInvocationException>(() =>
            Activator.CreateInstance(type, args: [null!]));

        ArgumentNullException innerException = Assert.IsType<ArgumentNullException>(exception.InnerException);
        Assert.Equal("provider", innerException.ParamName);
    }

    [Fact]
    public void TakeShouldCallProviderAndRemoveContractedOnReturnedBuffer()
    {
        var buffer = new MappingsPortRemovingContractedTestsBufferDouble();
        var provider = new MappingsPortRemovingContractedTestsProviderDouble(buffer);
        IOutputPort<IIndexMappingBuffer> sut = CreateSut(provider);

        IIndexMappingBuffer result = sut.Take();

        Assert.Equal(1, provider.TakeCallCount);
        Assert.Equal(1, buffer.RemoveContractedCallCount);
        Assert.Same(buffer, result);
    }

    [Fact]
    public void TakeShouldReturnNullWhenProviderReturnsNull()
    {
        var provider = new MappingsPortRemovingContractedTestsProviderDouble(bufferToReturn: null);
        IOutputPort<IIndexMappingBuffer> sut = CreateSut(provider);

        IIndexMappingBuffer? result = sut.Take();

        Assert.Equal(1, provider.TakeCallCount);
        Assert.Null(result);
    }

    private static IOutputPort<IIndexMappingBuffer> CreateSut(IOutputPort<IIndexMappingBuffer> provider)
    {
        Type type = GetTargetType();
        var instance = Activator.CreateInstance(type, provider);

        return Assert.IsAssignableFrom<IOutputPort<IIndexMappingBuffer>>(instance);
    }

    private static Type GetTargetType()
    {
        var type = typeof(IndexMappingProviderUtil).Assembly.GetType("NRedberry.IndexMapping.MappingsPortRemovingContracted");

        Assert.NotNull(type);

        return type!;
    }
}

internal sealed class MappingsPortRemovingContractedTestsProviderDouble(IIndexMappingBuffer? bufferToReturn)
    : IOutputPort<IIndexMappingBuffer>
{
    public int TakeCallCount { get; private set; }

    public IIndexMappingBuffer Take()
    {
        TakeCallCount++;

        return bufferToReturn!;
    }
}

internal sealed class MappingsPortRemovingContractedTestsBufferDouble : IIndexMappingBuffer
{
    public int RemoveContractedCallCount { get; private set; }

    public bool TryMap(int from, int to)
    {
        return true;
    }

    public void AddSign(bool sign)
    {
    }

    public void RemoveContracted()
    {
        RemoveContractedCallCount++;
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
        return new MappingsPortRemovingContractedTestsBufferDouble();
    }
}
#pragma warning restore CS0618
