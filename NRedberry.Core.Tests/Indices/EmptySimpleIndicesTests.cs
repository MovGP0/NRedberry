using System.Reflection;
using System.Runtime.CompilerServices;
using NRedberry;
using NRedberry.Indices;
using Xunit;

namespace NRedberry.Core.Tests.Indices;

public class EmptySimpleIndicesTests
{
    private static readonly Type s_emptySimpleIndicesType =
        typeof(IndicesFactory).Assembly.GetType("NRedberry.Indices.EmptySimpleIndices", throwOnError: true)!;

    private static readonly FieldInfo s_singletonField =
        s_emptySimpleIndicesType.GetField("emptySimpleIndicesInstance", BindingFlags.Public | BindingFlags.Static)!;

    private static object Singleton
    {
        get
        {
            return s_singletonField.GetValue(null)!;
        }
    }

    [Fact]
    public void ShouldExposeSingletonFieldWithFactoryInstance()
    {
        object singleton = Singleton;

        Assert.NotNull(s_singletonField);
        Assert.Same(singleton, IndicesFactory.EmptySimpleIndices);
    }

    [Fact]
    public void ShouldReturnSameInstanceFromInvertedFreeOfTypeUpperLowerAndApplyIndexMapping()
    {
        object singleton = Singleton;

        Assert.Same(singleton, GetPropertyValue(singleton, "Inverted"));
        Assert.Same(singleton, GetPropertyValue(singleton, "Free"));
        Assert.Same(singleton, GetPropertyValue(singleton, "Upper"));
        Assert.Same(singleton, GetPropertyValue(singleton, "Lower"));
        Assert.Same(singleton, InvokeMethod(singleton, "OfType", IndexType.LatinLower));
        Assert.Same(singleton, InvokeMethod(singleton, "ApplyIndexMapping", new object?[] { null! }));
    }

    [Fact]
    public void ShouldExposeSymmetriesGetterReturningStaticEmptySymmetriesField()
    {
        MethodInfo getter = s_emptySimpleIndicesType.GetProperty("Symmetries", BindingFlags.Public | BindingFlags.Instance)!.GetMethod!;
        FieldInfo emptySymmetriesField = typeof(IndicesSymmetries).GetField("EmptySymmetries", BindingFlags.Public | BindingFlags.Static)!;

        Assert.True(MethodBodyContainsMetadataTokenWithOpcode(getter, emptySymmetriesField.MetadataToken, 0x7E));
    }

    [Fact(Skip = "Blocked by existing context initialization failure: IndexConverterManager throws 'Several converters for same type'.")]
    public void ShouldAcceptEmptySymmetriesOnSetter()
    {
        object singleton = Singleton;
        PropertyInfo symmetriesProperty = s_emptySimpleIndicesType.GetProperty("Symmetries", BindingFlags.Public | BindingFlags.Instance)!;
        object emptySymmetries = CreateUninitializedSymmetriesWithStructureSize(0);

        symmetriesProperty.SetValue(singleton, emptySymmetries);
    }

    [Fact(Skip = "Blocked by existing context initialization failure: IndexConverterManager throws 'Several converters for same type'.")]
    public void ShouldThrowWhenSettingNonEmptySymmetries()
    {
        object singleton = Singleton;
        PropertyInfo symmetriesProperty = s_emptySimpleIndicesType.GetProperty("Symmetries", BindingFlags.Public | BindingFlags.Instance)!;
        object nonEmptySymmetries = CreateUninitializedSymmetriesWithStructureSize(1);

        Exception exception = Assert.ThrowsAny<Exception>(() => symmetriesProperty.SetValue(singleton, nonEmptySymmetries));
        Exception unwrapped = UnwrapInvocationException(exception);

        Assert.IsType<InvalidOperationException>(unwrapped);
    }

    [Fact]
    public void ShouldMatchOnlySingletonInEqualsWithSymmetries()
    {
        object singleton = Singleton;
        int index = IndicesUtils.CreateIndex(2, IndexType.GreekUpper, true);
        SimpleIndices nonEmptyIndices = IndicesFactory.CreateSimple(null, index);

        bool matchesSingleton = (bool)InvokeMethod(singleton, "EqualsWithSymmetries", singleton);
        bool matchesNonEmpty = (bool)InvokeMethod(singleton, "EqualsWithSymmetries", nonEmptyIndices);

        Assert.True(matchesSingleton);
        Assert.False(matchesNonEmpty);
    }

    [Fact]
    public void ShouldExposeStructureOfIndicesGetterReturningStaticEmptyStructure()
    {
        MethodInfo getter = s_emptySimpleIndicesType.GetProperty("StructureOfIndices", BindingFlags.Public | BindingFlags.Instance)!.GetMethod!;
        MethodInfo emptyGetter = typeof(StructureOfIndices).GetProperty("Empty", BindingFlags.Public | BindingFlags.Static)!.GetMethod!;

        Assert.True(MethodBodyContainsMetadataTokenWithOpcode(getter, emptyGetter.MetadataToken, 0x28));
    }

    [Fact]
    public void ShouldImplementEqualsAndHashCodeForEmptyIndicesSemantics()
    {
        object singleton = Singleton;
        int index = IndicesUtils.CreateIndex(3, IndexType.LatinUpper, false);
        SimpleIndices nonEmptyIndices = IndicesFactory.CreateSimple(null, index);
        object anotherEmpty = EmptyIndices.EmptyIndicesInstance;

        Assert.True(singleton.Equals(singleton));
        Assert.True(singleton.Equals(anotherEmpty));
        Assert.False(singleton.Equals(nonEmptyIndices));
        Assert.Equal(453679, singleton.GetHashCode());
    }

    private static object GetPropertyValue(object instance, string propertyName)
    {
        PropertyInfo property = s_emptySimpleIndicesType.GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance)!;
        return property.GetValue(instance)!;
    }

    private static object InvokeMethod(object instance, string methodName, params object?[] arguments)
    {
        MethodInfo method = s_emptySimpleIndicesType.GetMethod(methodName, BindingFlags.Public | BindingFlags.Instance)!;
        return method.Invoke(instance, arguments)!;
    }

    private static Exception UnwrapInvocationException(Exception exception)
    {
        return exception is TargetInvocationException { InnerException: not null } targetInvocationException
            ? targetInvocationException.InnerException!
            : exception;
    }

    private static object CreateUninitializedSymmetriesWithStructureSize(int size)
    {
        object structure = RuntimeHelpers.GetUninitializedObject(typeof(StructureOfIndices));
        FieldInfo sizeField = typeof(StructureOfIndices).GetField("<Size>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance)!;
        sizeField.SetValue(structure, size);

        object symmetries = RuntimeHelpers.GetUninitializedObject(typeof(IndicesSymmetries));
        FieldInfo structureField = typeof(IndicesSymmetries).GetField("<StructureOfIndices>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance)!;
        structureField.SetValue(symmetries, structure);
        return symmetries;
    }

    private static bool MethodBodyContainsMetadataTokenWithOpcode(MethodInfo method, int token, byte opcode)
    {
        byte[] il = method.GetMethodBody()!.GetILAsByteArray()!;

        for (int i = 0; i + 4 < il.Length; i++)
        {
            if (il[i] != opcode)
            {
                continue;
            }

            int value = il[i + 1];
            value |= il[i + 2] << 8;
            value |= il[i + 3] << 16;
            value |= il[i + 4] << 24;

            if (value == token)
            {
                return true;
            }
        }

        return false;
    }
}
