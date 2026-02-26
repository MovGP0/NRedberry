using System;
using System.Reflection;
using NRedberry.Indices;
using Xunit;
using IndicesContract = NRedberry.Indices.Indices;

namespace NRedberry.Core.Tests.Indices;

public sealed class SimpleIndicesTests
{
    [Fact]
    public void ShouldExtendIndicesInterface()
    {
        Type interfaceType = typeof(SimpleIndices);

        Assert.True(interfaceType.IsInterface);
        Assert.Contains(typeof(IndicesContract), interfaceType.GetInterfaces());
    }

    [Fact]
    public void ShouldExposeSymmetriesPropertyWithGetterAndSetter()
    {
        PropertyInfo property = GetRequiredProperty(nameof(SimpleIndices.Symmetries));

        Assert.Equal(typeof(IndicesSymmetries), property.PropertyType);
        Assert.NotNull(property.GetMethod);
        Assert.NotNull(property.SetMethod);
    }

    [Fact]
    public void ShouldExposeEqualsWithSymmetriesMethodContract()
    {
        MethodInfo method = GetRequiredMethod(
            nameof(SimpleIndices.EqualsWithSymmetries),
            [typeof(SimpleIndices)]);

        Assert.Equal(typeof(bool), method.ReturnType);
    }

    [Fact]
    public void ShouldExposeStructureOfIndicesGetterOnlyProperty()
    {
        PropertyInfo property = GetRequiredProperty(nameof(SimpleIndices.StructureOfIndices));

        Assert.Equal(typeof(StructureOfIndices), property.PropertyType);
        Assert.NotNull(property.GetMethod);
        Assert.Null(property.SetMethod);
    }

    private static PropertyInfo GetRequiredProperty(string propertyName)
    {
        PropertyInfo? property = typeof(SimpleIndices).GetProperty(
            propertyName,
            BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);

        Assert.NotNull(property);
        return property!;
    }

    private static MethodInfo GetRequiredMethod(string methodName, Type[] parameterTypes)
    {
        MethodInfo? method = typeof(SimpleIndices).GetMethod(
            methodName,
            BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly,
            null,
            parameterTypes,
            null);

        Assert.NotNull(method);
        return method!;
    }
}
