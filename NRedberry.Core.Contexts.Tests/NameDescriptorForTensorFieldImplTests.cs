using NRedberry;
using NRedberry.Contexts;
using NRedberry.Indices;
using Shouldly;
using Xunit;

namespace NRedberry.Core.Contexts.Tests;

public sealed class NameDescriptorForTensorFieldImplTests
{
    [Fact(DisplayName = "Should expose keys and name")]
    public void ShouldExposeKeysAndName()
    {
        // Arrange
        byte type = IndexType.LatinLower.GetType_();
        StructureOfIndices main = StructureOfIndices.Create(type, 1, true);
        StructureOfIndices arg = StructureOfIndices.Create(type, 1, false);
        var nameManager = new NameManager(1, "d", "g");
        var descriptor = (NameDescriptorForTensorFieldImpl)nameManager.MapNameDescriptor("F", main, arg);

        // Act
        NameAndStructureOfIndices[] keys = descriptor.GetKeys();

        // Assert
        descriptor.GetName(null, OutputFormat.Redberry).ShouldBe("F");
        keys.Length.ShouldBe(1);
        keys[0].Name.ShouldBe("F");
        keys[0].Structure.ShouldBe(descriptor.IndexTypeStructures);
        descriptor.IsDerivative().ShouldBeFalse();
        descriptor.GetParent().ShouldBeSameAs(descriptor);
    }

    [Fact(DisplayName = "Should return self for zero derivative orders")]
    public void ShouldReturnSelfForZeroDerivativeOrders()
    {
        // Arrange
        byte type = IndexType.LatinLower.GetType_();
        StructureOfIndices main = StructureOfIndices.Create(type, 1, true);
        StructureOfIndices arg = StructureOfIndices.Create(type, 1, false);
        var nameManager = new NameManager(1, "d", "g");
        var descriptor = (NameDescriptorForTensorFieldImpl)nameManager.MapNameDescriptor("F", main, arg);

        // Act
        NameDescriptorForTensorField result = descriptor.GetDerivative(0);

        // Assert
        result.ShouldBeSameAs(descriptor);
    }

    [Fact(DisplayName = "Should throw for invalid derivative orders")]
    public void ShouldThrowForInvalidDerivativeOrders()
    {
        // Arrange
        byte type = IndexType.LatinLower.GetType_();
        StructureOfIndices main = StructureOfIndices.Create(type, 1, true);
        StructureOfIndices arg = StructureOfIndices.Create(type, 1, false);
        var nameManager = new NameManager(1, "d", "g");
        var descriptor = (NameDescriptorForTensorFieldImpl)nameManager.MapNameDescriptor("F", main, arg);

        // Act + Assert
        Should.Throw<ArgumentNullException>(() => descriptor.GetDerivative(null!));
        Should.Throw<ArgumentException>(() => descriptor.GetDerivative(1, 0));
        Should.Throw<ArgumentException>(() => descriptor.GetDerivative(-1));
    }

    [Fact(DisplayName = "Should cache non-zero derivatives")]
    public void ShouldCacheNonZeroDerivatives()
    {
        // Arrange
        byte type = IndexType.LatinLower.GetType_();
        StructureOfIndices main = StructureOfIndices.Create(type, 1, true);
        StructureOfIndices arg = StructureOfIndices.Create(type, 1, false);
        var nameManager = new NameManager(1, "d", "g");
        var descriptor = (NameDescriptorForTensorFieldImpl)nameManager.MapNameDescriptor("F", main, arg);

        // Act
        NameDescriptorForTensorField first = descriptor.GetDerivative(1);
        NameDescriptorForTensorField second = descriptor.GetDerivative(1);

        // Assert
        first.ShouldBeSameAs(second);
        first.IsDerivative().ShouldBeTrue();
        first.GetParent().ShouldBeSameAs(descriptor);
    }
}
