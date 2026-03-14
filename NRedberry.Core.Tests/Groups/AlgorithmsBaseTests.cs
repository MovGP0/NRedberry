using NRedberry.Core.Combinatorics;
using NRedberry.Groups;
using Shouldly;
using Xunit;

namespace NRedberry.Core.Tests.Groups;

public sealed class AlgorithmsBaseTests
{
    [Fact(DisplayName = "Should create trivial structures and validate degree")]
    public void ShouldCreateTrivialStructuresAndValidateDegree()
    {
        // Act + Assert
        Should.Throw<ArgumentException>(() => AlgorithmsBase.CreateSymmetricGroupBSGS(0));
        AlgorithmsBase.TrivialBsgs.ShouldNotBeNull();
        AlgorithmsBase.TrivialBsgs.ShouldHaveSingleItem();
    }

    [Fact(DisplayName = "Should create raw BSGS candidate for identity generators")]
    public void ShouldCreateRawBsgsCandidateForIdentityGenerators()
    {
        // Arrange
        Permutation identity = NRedberry.Groups.Permutations.GetIdentityPermutation();

        // Act
        var candidates = AlgorithmsBase.CreateRawBSGSCandidate(identity);

        // Assert
        candidates.ShouldBeEmpty();
    }

    [Fact(DisplayName = "Should validate membership for trivial group")]
    public void ShouldValidateMembershipForTrivialGroup()
    {
        // Arrange
        Permutation identity = NRedberry.Groups.Permutations.GetIdentityPermutation();

        // Act
        bool isMember = AlgorithmsBase.MembershipTest(AlgorithmsBase.TrivialBsgs, identity);

        // Assert
        isMember.ShouldBeTrue();
    }

    [Fact(DisplayName = "Should return base points as array")]
    public void ShouldReturnBasePointsAsArray()
    {
        // Arrange
        var bsgs = AlgorithmsBase.CreateSymmetricGroupBSGS(2);

        // Act
        int[] baseArray = AlgorithmsBase.GetBaseAsArray(bsgs);

        // Assert
        baseArray.ShouldHaveSingleItem();
        baseArray[0].ShouldBe(0);
    }
}
