using System;
using System.Collections.Generic;
using System.Reflection;
using NRedberry.Concurrent;
using NRedberry.IndexMapping;
using Shouldly;
using Xunit;

namespace NRedberry.Core.Tests.Indexmapping;

#pragma warning disable CS0618
public sealed class MappingsPortRemovingContractedTests
{
    [Fact]
    public void ConstructorShouldThrowArgumentNullExceptionWhenProviderIsNull()
    {
        Type type = GetTargetType();

        TargetInvocationException exception = Should.Throw<TargetInvocationException>(() =>
            Activator.CreateInstance(type, args: [null!]));

        ArgumentNullException innerException = exception.InnerException.ShouldBeOfType<ArgumentNullException>();
        innerException.ParamName.ShouldBe("provider");
    }

    [Fact]
    public void TakeShouldCallProviderAndRemoveContractedOnReturnedBuffer()
    {
        var buffer = new MappingsPortRemovingContractedTestsBufferDouble();
        var provider = new MappingsPortRemovingContractedTestsProviderDouble(buffer);
        IOutputPort<IIndexMappingBuffer> sut = CreateSut(provider);

        IIndexMappingBuffer result = sut.Take();

        provider.TakeCallCount.ShouldBe(1);
        buffer.RemoveContractedCallCount.ShouldBe(1);
        result.ShouldBeSameAs(buffer);
    }

    [Fact]
    public void TakeShouldReturnNullWhenProviderReturnsNull()
    {
        var provider = new MappingsPortRemovingContractedTestsProviderDouble(bufferToReturn: null);
        IOutputPort<IIndexMappingBuffer> sut = CreateSut(provider);

        IIndexMappingBuffer? result = sut.Take();

        provider.TakeCallCount.ShouldBe(1);
        result.ShouldBeNull();
    }

    private static IOutputPort<IIndexMappingBuffer> CreateSut(IOutputPort<IIndexMappingBuffer> provider)
    {
        Type type = GetTargetType();
        var instance = Activator.CreateInstance(type, provider);

        instance.ShouldNotBeNull();
        instance.ShouldBeAssignableTo<IOutputPort<IIndexMappingBuffer>>();
        return (IOutputPort<IIndexMappingBuffer>)instance;
    }

    private static Type GetTargetType()
    {
        var type = typeof(IndexMappingProviderUtil).Assembly.GetType("NRedberry.IndexMapping.MappingsPortRemovingContracted");

        type.ShouldNotBeNull();

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
