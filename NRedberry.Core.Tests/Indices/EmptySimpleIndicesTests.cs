using System.Reflection;
using System.Runtime.CompilerServices;
using NRedberry.Indices;

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

        s_singletonField.ShouldNotBeNull();
        IndicesFactory.EmptySimpleIndices.ShouldBeSameAs(singleton);
    }

    [Fact]
    public void ShouldReturnSameInstanceFromInvertedFreeOfTypeUpperLowerAndApplyIndexMapping()
    {
        object singleton = Singleton;

        GetPropertyValue(singleton, "Inverted").ShouldBeSameAs(singleton);
        GetPropertyValue(singleton, "Free").ShouldBeSameAs(singleton);
        GetPropertyValue(singleton, "Upper").ShouldBeSameAs(singleton);
        GetPropertyValue(singleton, "Lower").ShouldBeSameAs(singleton);
        InvokeMethod(singleton, "OfType", IndexType.LatinLower).ShouldBeSameAs(singleton);
        InvokeMethod(singleton, "ApplyIndexMapping", [null!]).ShouldBeSameAs(singleton);
    }

    [Fact]
    public void ShouldExposeSymmetriesGetterReturningStaticEmptySymmetriesField()
    {
        MethodInfo getter = s_emptySimpleIndicesType.GetProperty("Symmetries", BindingFlags.Public | BindingFlags.Instance)!.GetMethod!;
        FieldInfo emptySymmetriesField = typeof(IndicesSymmetries).GetField("EmptySymmetries", BindingFlags.Public | BindingFlags.Static)!;

        MethodBodyContainsMetadataTokenWithOpcode(getter, emptySymmetriesField.MetadataToken, 0x7E).ShouldBeTrue();
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

        Exception exception = Should.Throw<Exception>(() => symmetriesProperty.SetValue(singleton, nonEmptySymmetries));
        Exception unwrapped = UnwrapInvocationException(exception);

        unwrapped.ShouldBeOfType<InvalidOperationException>();
    }

    [Fact]
    public void ShouldMatchOnlySingletonInEqualsWithSymmetries()
    {
        object singleton = Singleton;
        int index = IndicesUtils.CreateIndex(2, IndexType.GreekUpper, true);
        SimpleIndices nonEmptyIndices = IndicesFactory.CreateSimple(null, index);

        bool matchesSingleton = (bool)InvokeMethod(singleton, "EqualsWithSymmetries", singleton);
        bool matchesNonEmpty = (bool)InvokeMethod(singleton, "EqualsWithSymmetries", nonEmptyIndices);

        matchesSingleton.ShouldBeTrue();
        matchesNonEmpty.ShouldBeFalse();
    }

    [Fact]
    public void ShouldExposeStructureOfIndicesGetterReturningStaticEmptyStructure()
    {
        MethodInfo getter = s_emptySimpleIndicesType.GetProperty("StructureOfIndices", BindingFlags.Public | BindingFlags.Instance)!.GetMethod!;
        MethodInfo emptyGetter = typeof(StructureOfIndices).GetProperty("Empty", BindingFlags.Public | BindingFlags.Static)!.GetMethod!;

        MethodBodyContainsMetadataTokenWithOpcode(getter, emptyGetter.MetadataToken, 0x28).ShouldBeTrue();
    }

    [Fact]
    public void ShouldImplementEqualsAndHashCodeForEmptyIndicesSemantics()
    {
        object singleton = Singleton;
        int index = IndicesUtils.CreateIndex(3, IndexType.LatinUpper, false);
        SimpleIndices nonEmptyIndices = IndicesFactory.CreateSimple(null, index);
        object anotherEmpty = EmptyIndices.EmptyIndicesInstance;

        singleton.Equals(singleton).ShouldBeTrue();
        singleton.Equals(anotherEmpty).ShouldBeTrue();
        singleton.Equals(nonEmptyIndices).ShouldBeFalse();
        singleton.GetHashCode().ShouldBe(453679);
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
