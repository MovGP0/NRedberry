using NRedberry.Groups;
using Xunit;
using GroupPermutations = NRedberry.Groups.Permutations;

namespace NRedberry.Core.Tests.Groups;

public sealed class SchreierVectorTests
{
    [Fact(DisplayName = "Constructor enforces minimum identity length and initializes with -2")]
    public void ConstructorEnforcesMinimumIdentityLengthAndInitializesWithMinusTwo()
    {
        // Arrange + Act
        SchreierVector vector = new(1);

        // Assert
        Assert.Equal(GroupPermutations.DefaultIdentityLength, vector.Length);
        for (int i = 0; i < vector.Length; i++)
        {
            Assert.Equal(-2, vector[i]);
        }
    }

    [Fact(DisplayName = "Indexer get returns -2 for out-of-range positions")]
    public void IndexerGetReturnsMinusTwoForOutOfRangePositions()
    {
        // Arrange
        SchreierVector vector = new(2);

        // Act
        int value = vector[vector.Length + 5];

        // Assert
        Assert.Equal(-2, value);
    }

    [Fact(DisplayName = "Indexer set auto-expands and writes value")]
    public void IndexerSetAutoExpandsAndWritesValue()
    {
        // Arrange
        SchreierVector vector = new(2);
        int position = vector.Length + 5;

        // Act
        vector[position] = 17;

        // Assert
        Assert.True(vector.Length >= position + 1);
        Assert.Equal(17, vector[position]);
        Assert.Equal(-2, vector[position - 1]);
    }

    [Fact(DisplayName = "Reset restores all positions to -2")]
    public void ResetRestoresAllPositionsToMinusTwo()
    {
        // Arrange
        SchreierVector vector = new(2);
        int farPosition = vector.Length + 3;
        vector[0] = 9;
        vector[farPosition] = 4;

        // Act
        vector.Reset();

        // Assert
        for (int i = 0; i < vector.Length; i++)
        {
            Assert.Equal(-2, vector[i]);
        }
    }

    [Fact(DisplayName = "Clone returns deep copy independent from original")]
    public void CloneReturnsDeepCopyIndependentFromOriginal()
    {
        // Arrange
        SchreierVector original = new(2);
        original[0] = 1;
        int originalFarPosition = original.Length + 2;
        original[originalFarPosition] = 8;

        // Act
        SchreierVector clone = original.Clone();
        clone[0] = 7;
        clone[clone.Length + 1] = 99;
        original[originalFarPosition] = 5;

        // Assert
        Assert.NotSame(original, clone);
        Assert.Equal(1, original[0]);
        Assert.Equal(7, clone[0]);
        Assert.Equal(5, original[originalFarPosition]);
        Assert.Equal(8, clone[originalFarPosition]);
        Assert.True(clone.Length > original.Length);
    }
}
