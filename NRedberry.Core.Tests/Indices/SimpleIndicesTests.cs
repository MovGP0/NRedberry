using System.Reflection;
using NRedberry.Indices;
using IndicesContract = NRedberry.Indices.Indices;

namespace NRedberry.Core.Tests.Indices;

public sealed class SimpleIndicesTests
{
    [Fact]
    public void ShouldExtendIndicesInterface()
    {
        Type interfaceType = typeof(SimpleIndices);

        interfaceType.IsInterface.ShouldBeTrue();
        interfaceType.GetInterfaces().ShouldContain(typeof(IndicesContract));
    }

    [Fact]
    public void ShouldExposeSymmetriesPropertyWithGetterAndSetter()
    {
        PropertyInfo property = GetRequiredProperty(nameof(SimpleIndices.Symmetries));

        property.PropertyType.ShouldBe(typeof(IndicesSymmetries));
        property.GetMethod.ShouldNotBeNull();
        property.SetMethod.ShouldNotBeNull();
    }

    [Fact]
    public void ShouldExposeEqualsWithSymmetriesMethodContract()
    {
        MethodInfo method = GetRequiredMethod(
            nameof(SimpleIndices.EqualsWithSymmetries),
            [typeof(SimpleIndices)]);

        method.ReturnType.ShouldBe(typeof(bool));
    }

    [Fact]
    public void ShouldExposeStructureOfIndicesGetterOnlyProperty()
    {
        PropertyInfo property = GetRequiredProperty(nameof(SimpleIndices.StructureOfIndices));

        property.PropertyType.ShouldBe(typeof(StructureOfIndices));
        property.GetMethod.ShouldNotBeNull();
        property.SetMethod.ShouldBeNull();
    }

    private static PropertyInfo GetRequiredProperty(string propertyName)
    {
        PropertyInfo? property = typeof(SimpleIndices).GetProperty(
            propertyName,
            BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);

        property.ShouldNotBeNull();
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

        method.ShouldNotBeNull();
        return method!;
    }
}
