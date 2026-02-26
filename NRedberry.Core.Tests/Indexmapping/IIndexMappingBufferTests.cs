using System;
using System.Collections.Generic;
using System.Linq;
using NRedberry.IndexMapping;
using Xunit;

namespace NRedberry.Core.Tests.Indexmapping;

public sealed class IIndexMappingBufferTests
{
    [Fact]
    public void InterfaceShouldInheritICloneable()
    {
        Type type = typeof(IIndexMappingBuffer);

        Assert.Contains(typeof(ICloneable), type.GetInterfaces());
    }

    [Fact]
    public void InterfaceShouldDefineRequiredMethodSignatures()
    {
        Type type = typeof(IIndexMappingBuffer);

        AssertMethod(type, nameof(IIndexMappingBuffer.TryMap), typeof(bool), typeof(int), typeof(int));
        AssertMethod(type, nameof(IIndexMappingBuffer.AddSign), typeof(void), typeof(bool));
        AssertMethod(type, nameof(IIndexMappingBuffer.RemoveContracted), typeof(void));
        AssertMethod(type, nameof(IIndexMappingBuffer.IsEmpty), typeof(bool));
        AssertMethod(type, nameof(IIndexMappingBuffer.GetSign), typeof(bool));
        AssertMethod(type, nameof(IIndexMappingBuffer.Export), typeof(object));
        AssertMethod(type, nameof(IIndexMappingBuffer.GetMap), typeof(IDictionary<int, IndexMappingBufferRecord>));
    }

    private static void AssertMethod(Type type, string name, Type returnType, params Type[] parameterTypes)
    {
        var method = type.GetMethod(name, parameterTypes);

        Assert.NotNull(method);
        Assert.Equal(returnType, method.ReturnType);

        Type[] interfaceParameterTypes = method.GetParameters().Select(parameter => parameter.ParameterType).ToArray();
        Assert.Equal(parameterTypes, interfaceParameterTypes);
    }
}
