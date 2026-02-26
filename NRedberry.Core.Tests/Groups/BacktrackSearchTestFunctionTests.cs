using NRedberry.Core.Combinatorics;
using NRedberry.Groups;
using Xunit;

namespace NRedberry.Core.Tests.Groups;

public sealed class BacktrackSearchTestFunctionTests
{
    [Fact(DisplayName = "Should return singleton true test function")]
    public void ShouldReturnSingletonTrueTestFunction()
    {
        // Act
        var first = IBacktrackSearchTestFunction.True;
        var second = IBacktrackSearchTestFunction.True;

        // Assert
        Assert.Same(first, second);
        Assert.IsType<TrueBacktrackSearchTestFunction>(first);
    }

    [Theory(DisplayName = "Should always return true for default test function")]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(-1)]
    [InlineData(10)]
    public void ShouldAlwaysReturnTrueForDefaultTestFunction(int level)
    {
        // Arrange
        var testFunction = IBacktrackSearchTestFunction.True;
        Permutation permutation = NRedberry.Groups.Permutations.GetIdentityPermutation();

        // Act
        bool result = testFunction.Test(permutation, level);

        // Assert
        Assert.True(result);
    }
}
