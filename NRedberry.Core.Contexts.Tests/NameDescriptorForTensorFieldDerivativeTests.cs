using NRedberry.Contexts;
using NRedberry.Indices;
using Xunit;

namespace NRedberry.Core.Contexts.Tests;

public sealed class NameDescriptorForTensorFieldDerivativeTests
{
    [Fact(DisplayName = "Should expose derivative metadata and formatting")]
    public void ShouldExposeDerivativeMetadataAndFormatting()
    {
        // Arrange
        byte type = IndexType.LatinLower.GetType_();
        StructureOfIndices main = StructureOfIndices.Create(type, 1, true);
        StructureOfIndices arg1 = StructureOfIndices.Create(type, 1, false);
        StructureOfIndices arg2 = StructureOfIndices.Create(type, 1, true);
        var nameManager = new NameManager(1, "d", "g");
        var parent = (NameDescriptorForTensorFieldImpl)nameManager.MapNameDescriptor("F", main, arg1, arg2);

        // Act
        NameDescriptorForTensorField derivative = parent.GetDerivative(1, 0);

        // Assert
        derivative.IsDerivative().ShouldBeTrue();
        derivative.GetParent().ShouldBeSameAs(parent);
        derivative.GetKeys().ShouldBeEmpty();
        derivative.GetDerivativeOrders().ShouldBe([1, 0]);
        derivative.GetName(null, OutputFormat.Redberry).ShouldBe("F~(1,0)");
        derivative.GetName(null, OutputFormat.WolframMathematica).ShouldBe("Derivative[1,0][F]");
        derivative.GetName(null, OutputFormat.Maple).ShouldBe("D[1](F)");
    }

    [Fact(DisplayName = "Should combine derivative orders")]
    public void ShouldCombineDerivativeOrders()
    {
        // Arrange
        byte type = IndexType.LatinLower.GetType_();
        StructureOfIndices main = StructureOfIndices.Create(type, 1, true);
        StructureOfIndices arg1 = StructureOfIndices.Create(type, 1, false);
        var nameManager = new NameManager(1, "d", "g");
        var parent = (NameDescriptorForTensorFieldImpl)nameManager.MapNameDescriptor("F", main, arg1);
        NameDescriptorForTensorField derivative = parent.GetDerivative(1);

        // Act
        NameDescriptorForTensorField combined = derivative.GetDerivative(1);

        // Assert
        combined.GetDerivativeOrders().ShouldBe([2]);
    }
}
