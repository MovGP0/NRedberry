using System.Reflection;
using NRedberry.IndexMapping;
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

        Assert.NotNull(method);
        Assert.Equal(typeof(IIndexMappingProvider), method.ReturnType);
    }

    [Fact]
    public void InterfaceShouldDeclareExactlyOneMethodNamedCreate()
    {
        MethodInfo[] methods = typeof(IIndexMappingProviderFactory).GetMethods(
            BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);

        Assert.Single(methods);
        Assert.Equal("Create", methods[0].Name);
    }
}
