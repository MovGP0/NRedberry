using NRedberry;
using NRedberry.Contexts;
using NRedberry.Indices;
using Shouldly;
using Xunit;

namespace NRedberry.Core.Contexts.Tests;

public sealed class NameDescriptorForMetricAndKroneckerTests
{
    [Fact(DisplayName = "Should expose both Kronecker and metric keys")]
    public void ShouldExposeBothKroneckerAndMetricKeys()
    {
        // Arrange
        byte type = IndexType.LatinLower.GetType_();
        StructureOfIndices structure = StructureOfIndices.Create(type, 2, true, false);
        var nameManager = new NameManager(1, "d", "g");

        // Act
        NameDescriptor descriptor = nameManager.MapNameDescriptor("d", structure);
        NameAndStructureOfIndices[] keys = descriptor.GetKeys();

        // Assert
        keys.Length.ShouldBe(2);
        keys[0].Name.ShouldBe("d");
        keys[1].Name.ShouldBe("g");
        keys[0].Structure.ShouldBe(descriptor.IndexTypeStructures);
        keys[1].Structure.ShouldBe(descriptor.IndexTypeStructures);
    }

    [Fact(DisplayName = "Should return Kronecker or metric name based on states")]
    public void ShouldReturnKroneckerOrMetricNameBasedOnStates()
    {
        // Arrange
        byte type = IndexType.LatinLower.GetType_();
        StructureOfIndices structure = StructureOfIndices.Create(type, 2, true, false);
        var nameManager = new NameManager(1, "d", "g");
        NameDescriptor descriptor = nameManager.MapNameDescriptor("d", structure);

        int upperA = IndicesUtils.CreateIndex(0, type, true);
        int upperB = IndicesUtils.CreateIndex(1, type, true);
        int lowerA = IndicesUtils.CreateIndex(0, type, false);

        SimpleIndices metricIndices = IndicesFactory.CreateSimple(null, upperA, upperB);
        SimpleIndices kroneckerIndices = IndicesFactory.CreateSimple(null, upperA, lowerA);

        // Act
        string metricName = descriptor.GetName(metricIndices, OutputFormat.Redberry);
        string kroneckerName = descriptor.GetName(kroneckerIndices, OutputFormat.Redberry);

        // Assert
        metricName.ShouldBe("g");
        kroneckerName.ShouldBe("d");
    }

    [Fact(DisplayName = "Should throw when indices are null")]
    public void ShouldThrowWhenIndicesAreNull()
    {
        // Arrange
        byte type = IndexType.LatinLower.GetType_();
        StructureOfIndices structure = StructureOfIndices.Create(type, 2, true, false);
        var nameManager = new NameManager(1, "d", "g");
        NameDescriptor descriptor = nameManager.MapNameDescriptor("d", structure);

        // Act + Assert
        Should.Throw<ArgumentNullException>(() => descriptor.GetName(null, OutputFormat.Redberry));
    }
}
