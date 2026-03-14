using System.Reflection;
using NRedberry.IndexMapping;
using Shouldly;
using Xunit;
using TensorType = NRedberry.Tensors.Tensor;

namespace NRedberry.Core.Tests.Indexmapping;

public sealed class IIndexMappingProviderFactoryTests
{
    [Fact]
    public void CreateShouldExistWithExpectedSignatureAndReturnType()
    {
        MethodInfo? method = typeof(IIndexMappingProviderFactory).GetMethod(
            "Create",
            [
                typeof(IIndexMappingProvider),
                typeof(TensorType),
                typeof(TensorType)
            ]);

        method.ShouldNotBeNull();
        method.ReturnType.ShouldBe(typeof(IIndexMappingProvider));
    }

    [Fact]
    public void InterfaceShouldDeclareExactlyOneMethodNamedCreate()
    {
        MethodInfo[] methods = typeof(IIndexMappingProviderFactory).GetMethods(
            BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);

        methods.ShouldHaveSingleItem();
        methods[0].Name.ShouldBe("Create");
    }
}
