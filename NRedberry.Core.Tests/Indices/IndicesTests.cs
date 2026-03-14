using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using NRedberry.Indices;
using Shouldly;
using Xunit;
using IndicesContract = NRedberry.Indices.Indices;

namespace NRedberry.Core.Tests.Indices;

public sealed class IndicesTests
{
    [Fact]
    public void ShouldImplementExpectedBaseInterfaces()
    {
        Type indicesType = typeof(IndicesContract);
        Type[] implementedInterfaces = indicesType.GetInterfaces();

        indicesType.IsInterface.ShouldBeTrue();
        implementedInterfaces.ShouldContain(typeof(IEnumerable<int>));
        implementedInterfaces.ShouldContain(typeof(IEquatable<object>));
    }

    [Fact]
    public void ShouldExposeExpectedImmutableArrayProperties()
    {
        AssertImmutableArrayProperty(nameof(IndicesContract.UpperIndices));
        AssertImmutableArrayProperty(nameof(IndicesContract.LowerIndices));
        AssertImmutableArrayProperty(nameof(IndicesContract.AllIndices));
    }

    [Fact]
    public void ShouldExposeExpectedSizeAndIndexerSignatures()
    {
        Type indicesType = typeof(IndicesContract);

        AssertMethod(indicesType, nameof(IndicesContract.Size), typeof(int));
        AssertMethod(indicesType, nameof(IndicesContract.Size), typeof(int), typeof(IndexType));

        PropertyInfo[] indexers = indicesType.GetProperties()
            .Where(static property => property.GetIndexParameters().Length > 0)
            .ToArray();

        indexers.ShouldContain(property => HasIndexerSignature(property, typeof(int), typeof(int)));

        indexers.ShouldContain(property => HasIndexerSignature(property, typeof(int), typeof(IndexType), typeof(int)));
    }

    [Fact]
    public void ShouldExposeExpectedBehavioralMethods()
    {
        Type indicesType = typeof(IndicesContract);

        AssertMethod(indicesType, nameof(IndicesContract.GetOfType), indicesType, typeof(IndexType));
        AssertMethod(indicesType, nameof(IndicesContract.GetFree), indicesType);
        AssertMethod(indicesType, nameof(IndicesContract.GetInverted), indicesType);
        AssertMethod(indicesType, nameof(IndicesContract.EqualsRegardlessOrder), typeof(bool), indicesType);
        AssertMethod(indicesType, nameof(IndicesContract.TestConsistentWithException), typeof(void));
        AssertMethod(indicesType, nameof(IndicesContract.ApplyIndexMapping), indicesType, typeof(IIndexMapping));
        AssertMethod(indicesType, nameof(IndicesContract.ToString), typeof(string), typeof(OutputFormat));
        AssertMethod(indicesType, nameof(IndicesContract.GetDiffIds), typeof(short[]));
    }

    private static void AssertImmutableArrayProperty(string propertyName)
    {
        PropertyInfo property = GetRequiredProperty(typeof(IndicesContract), propertyName);

        property.PropertyType.ShouldBe(typeof(ImmutableArray<int>));
        property.GetMethod.ShouldNotBeNull();
        property.SetMethod.ShouldBeNull();
    }

    private static void AssertMethod(Type declaringType, string name, Type returnType, params Type[] parameterTypes)
    {
        MethodInfo method = GetRequiredMethod(declaringType, name, parameterTypes);

        method.ReturnType.ShouldBe(returnType);
    }

    private static MethodInfo GetRequiredMethod(Type declaringType, string name, params Type[] parameterTypes)
    {
        MethodInfo? method = declaringType.GetMethod(name, parameterTypes);
        method.ShouldNotBeNull();
        return method!;
    }

    private static PropertyInfo GetRequiredProperty(Type declaringType, string name)
    {
        PropertyInfo? property = declaringType.GetProperty(name, BindingFlags.Public | BindingFlags.Instance);
        property.ShouldNotBeNull();
        return property!;
    }

    private static bool HasIndexerSignature(PropertyInfo property, Type returnType, params Type[] indexParameterTypes)
    {
        ParameterInfo[] parameters = property.GetIndexParameters();
        if (property.PropertyType != returnType || parameters.Length != indexParameterTypes.Length)
        {
            return false;
        }

        for (int i = 0; i < parameters.Length; i++)
        {
            if (parameters[i].ParameterType != indexParameterTypes[i])
            {
                return false;
            }
        }

        return property.GetMethod is not null && property.SetMethod is null;
    }
}
