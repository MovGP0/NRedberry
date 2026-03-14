using NRedberry;
using NRedberry.Contexts;
using NRedberry.Indices;
using Shouldly;
using Xunit;

namespace NRedberry.Core.Contexts.Tests;

public sealed class NameAndStructureOfIndicesTests
{
    [Fact(DisplayName = "Should throw when name or structure is null")]
    public void ShouldThrowWhenNameOrStructureIsNull()
    {
        // Arrange
        byte type = IndexType.LatinLower.GetType_();
        StructureOfIndices[] structure = [StructureOfIndices.Create(type, 1, true)];

        // Act + Assert
        Should.Throw<ArgumentNullException>(() => _ = new NameAndStructureOfIndices(null!, structure));
        Should.Throw<ArgumentNullException>(() => _ = new NameAndStructureOfIndices("A", null!));
    }

    [Fact(DisplayName = "Should compare name and structure for equality")]
    public void ShouldCompareNameAndStructureForEquality()
    {
        // Arrange
        byte type = IndexType.LatinLower.GetType_();
        StructureOfIndices leftStructure = StructureOfIndices.Create(type, 2, true, false);
        StructureOfIndices rightStructure = StructureOfIndices.Create(type, 2, true, false);
        StructureOfIndices differentStructure = StructureOfIndices.Create(type, 1, true);

        var left = new NameAndStructureOfIndices("T", [leftStructure]);
        var right = new NameAndStructureOfIndices("T", [rightStructure]);
        var differentName = new NameAndStructureOfIndices("S", [rightStructure]);
        var differentStructureValue = new NameAndStructureOfIndices("T", [differentStructure]);

        // Assert
        left.Equals(right).ShouldBeTrue();
        left.ShouldBe(right);
        (left == right).ShouldBeTrue();
        (left != right).ShouldBeFalse();
        left.GetHashCode().ShouldBe(right.GetHashCode());

        left.Equals(differentName).ShouldBeFalse();
        left.Equals(differentStructureValue).ShouldBeFalse();
        (left == differentName).ShouldBeFalse();
        (left != differentStructureValue).ShouldBeTrue();
    }

    [Fact(DisplayName = "Should handle reference equality")]
    public void ShouldHandleReferenceEquality()
    {
        // Arrange
        byte type = IndexType.LatinLower.GetType_();
        StructureOfIndices structure = StructureOfIndices.Create(type, 1, true);
        var value = new NameAndStructureOfIndices("T", [structure]);

        // Act + Assert
        value.Equals(value).ShouldBeTrue();
        value.Equals((object)value).ShouldBeTrue();
    }
}
