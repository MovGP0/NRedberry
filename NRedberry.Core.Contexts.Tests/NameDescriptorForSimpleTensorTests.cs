using System.Reflection;
using NRedberry;
using NRedberry.Contexts;
using NRedberry.Indices;
using NRedberry.Tensors;
using Shouldly;
using Xunit;

namespace NRedberry.Core.Contexts.Tests;

public sealed class NameDescriptorForSimpleTensorTests
{
    [Fact(DisplayName = "Should return name and key for simple tensors")]
    public void ShouldReturnNameAndKeyForSimpleTensors()
    {
        // Arrange
        byte type = IndexType.LatinLower.GetType_();
        StructureOfIndices structure = StructureOfIndices.Create(type, 1, true);
        var nameManager = new NameManager(1, "d", "g");

        // Act
        NameDescriptor descriptor = nameManager.MapNameDescriptor("T", structure);
        NameAndStructureOfIndices[] keys = descriptor.GetKeys();

        // Assert
        descriptor.GetName(null, OutputFormat.Redberry).ShouldBe("T");
        keys.Length.ShouldBe(1);
        keys[0].Name.ShouldBe("T");
        keys[0].Structure.ShouldBe(descriptor.IndexTypeStructures);
    }

    [Fact(DisplayName = "Should manage cached symbol once")]
    public void ShouldManageCachedSymbolOnce()
    {
        // Arrange
        byte type = IndexType.LatinLower.GetType_();
        StructureOfIndices structure = StructureOfIndices.Create(type, 1, true);
        var nameManager = new NameManager(1, "d", "g");
        NameDescriptor descriptor = nameManager.MapNameDescriptor("T", structure);

        descriptor.GetType().Name.ShouldBe("NameDescriptorForSimpleTensor");

        PropertyInfo? cachedSymbol = descriptor.GetType().GetProperty("CachedSymbol");
        cachedSymbol.ShouldNotBeNull();

        // Act + Assert
        cachedSymbol!.GetValue(descriptor).ShouldBeNull();
        var nullException = Should.Throw<TargetInvocationException>(() => cachedSymbol.SetValue(descriptor, null));
        nullException.InnerException.ShouldBeOfType<ArgumentNullException>();

        var tensor = new SimpleTensor(1, IndicesFactory.EmptySimpleIndices);
        cachedSymbol.SetValue(descriptor, tensor);
        cachedSymbol.GetValue(descriptor).ShouldBe(tensor);

        var secondTensor = new SimpleTensor(2, IndicesFactory.EmptySimpleIndices);
        var exception = Should.Throw<TargetInvocationException>(() => cachedSymbol.SetValue(descriptor, secondTensor));
        exception.InnerException.ShouldBeOfType<InvalidOperationException>();
    }
}
