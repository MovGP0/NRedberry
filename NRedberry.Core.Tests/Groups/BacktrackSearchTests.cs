using NRedberry.Groups;
using Xunit;

namespace NRedberry.Core.Tests.Groups;

public sealed class BacktrackSearchTests
{
    [Fact(DisplayName = "Should throw for empty BSGS")]
    public void ShouldThrowForEmptyBsgs()
    {
        // Act + Assert
        Assert.Throws<ArgumentException>(() => _ = new BacktrackSearch([]));
    }

    [Fact(DisplayName = "Should enumerate group elements")]
    public void ShouldEnumerateGroupElements()
    {
        // Arrange
        var bsgs = AlgorithmsBase.CreateSymmetricGroupBSGS(2);
        var search = new BacktrackSearch(bsgs);
        int count = 0;

        // Act
        while (search.Take() is not null)
        {
            count++;
        }

        // Assert
        Assert.Equal(2, count);
    }
}
