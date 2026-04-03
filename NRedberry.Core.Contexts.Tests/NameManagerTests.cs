using NRedberry.Contexts;
using NRedberry.Indices;
using Xunit;

namespace NRedberry.Core.Contexts.Tests;

public sealed class NameManagerTests
{
    [Fact(DisplayName = "Should map name descriptors deterministically")]
    public void ShouldMapNameDescriptorsDeterministically()
    {
        // Arrange
        byte type = IndexType.LatinLower.GetType_();
        StructureOfIndices structure = StructureOfIndices.Create(type, 1, true);
        var nameManager = new NameManager(1, "d", "g");

        // Act
        NameDescriptor first = nameManager.MapNameDescriptor("T", structure);
        NameDescriptor second = nameManager.MapNameDescriptor("T", structure);

        // Assert
        first.ShouldBeSameAs(second);
    }

    [Fact(DisplayName = "Should resolve descriptors by id")]
    public void ShouldResolveDescriptorsById()
    {
        // Arrange
        byte type = IndexType.LatinLower.GetType_();
        StructureOfIndices structure = StructureOfIndices.Create(type, 1, true);
        var nameManager = new NameManager(1, "d", "g");
        NameDescriptor descriptor = nameManager.MapNameDescriptor("T", structure);

        // Act
        NameDescriptor resolved = nameManager.GetNameDescriptor(descriptor.Id);

        // Assert
        resolved.ShouldBeSameAs(descriptor);
    }

    [Fact(DisplayName = "Should track Kronecker or metric ids")]
    public void ShouldTrackKroneckerOrMetricIds()
    {
        // Arrange
        byte type = IndexType.LatinLower.GetType_();
        StructureOfIndices structure = StructureOfIndices.Create(type, 2, true, false);
        var nameManager = new NameManager(1, "d", "g");

        // Act
        NameDescriptor descriptor = nameManager.MapNameDescriptor("d", structure);

        // Assert
        nameManager.IsKroneckerOrMetric(descriptor.Id).ShouldBeTrue();
    }

    [Fact(DisplayName = "Should generate new symbols and reset state")]
    public void ShouldGenerateNewSymbolsAndResetState()
    {
        // Arrange
        var nameManager = new NameManager(1, "d", "g");

        // Act
        nameManager.GenerateNewSymbolDescriptor();

        // Assert
        nameManager.Size().ShouldBe(1);

        // Act
        nameManager.Reset(42);

        // Assert
        nameManager.Size().ShouldBe(0);
        nameManager.Seed.ShouldBe(42);
    }
}
