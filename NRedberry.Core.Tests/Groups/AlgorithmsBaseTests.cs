using NRedberry.Core.Combinatorics;
using NRedberry.Groups;
using Xunit;

namespace NRedberry.Core.Tests.Groups;

public sealed class AlgorithmsBaseTests
{
    [Fact(DisplayName = "Should create trivial structures and validate degree")]
    public void ShouldCreateTrivialStructuresAndValidateDegree()
    {
        // Act + Assert
        Assert.Throws<ArgumentException>(() => AlgorithmsBase.CreateSymmetricGroupBSGS(0));
        Assert.NotNull(AlgorithmsBase.TrivialBsgs);
        Assert.Single(AlgorithmsBase.TrivialBsgs);
    }

    [Fact(DisplayName = "Should create raw BSGS candidate for identity generators")]
    public void ShouldCreateRawBsgsCandidateForIdentityGenerators()
    {
        // Arrange
        Permutation identity = NRedberry.Groups.Permutations.GetIdentityPermutation();

        // Act
        var candidates = AlgorithmsBase.CreateRawBSGSCandidate(identity);

        // Assert
        Assert.Empty(candidates);
    }

    [Fact(DisplayName = "Should validate membership for trivial group")]
    public void ShouldValidateMembershipForTrivialGroup()
    {
        // Arrange
        Permutation identity = NRedberry.Groups.Permutations.GetIdentityPermutation();

        // Act
        bool isMember = AlgorithmsBase.MembershipTest(AlgorithmsBase.TrivialBsgs, identity);

        // Assert
        Assert.True(isMember);
    }

    [Fact(DisplayName = "Should return base points as array")]
    public void ShouldReturnBasePointsAsArray()
    {
        // Arrange
        var bsgs = AlgorithmsBase.CreateSymmetricGroupBSGS(2);

        // Act
        int[] baseArray = AlgorithmsBase.GetBaseAsArray(bsgs);

        // Assert
        Assert.Single(baseArray);
        Assert.Equal(0, baseArray[0]);
    }
}
