using System.IO;
using System.Text;
using Xunit;

namespace NRedberry.Core.Tests.Groups.Permutations;

public sealed class GapGroupsInterfaceTest
{
    [Fact]
    public void ShouldConvertPermutationToGapList()
    {
        Assert.Equal("[1, 3, 2]", GapGroupsInterface.ConvertToGapList([0, 2, 1]));
    }

    [Theory]
    [InlineData("Group(())", "Group(());")]
    [InlineData("Group(());", "Group(());")]
    public void ShouldNormalizeCommandsForGap(string input, string expected)
    {
        Assert.Equal(expected, GapGroupsInterface.NormalizeCommandForGap(input));
    }

    [Theory]
    [InlineData("Group(())", "Group(())")]
    [InlineData("Group(());", "Group(())")]
    public void ShouldNormalizeCommandsFromGap(string input, string expected)
    {
        Assert.Equal(expected, GapGroupsInterface.NormalizeCommandFromGap(input));
    }

    [Fact]
    public void ShouldReadGapOutputBatchesUntilEof()
    {
        byte[] bytes = Encoding.UTF8.GetBytes("gap> abc\\\n gap> def\nEOF\n");
        using MemoryStream stream = new(bytes);
        GapOutputReader reader = new(stream);

        reader.Run();

        Assert.True(reader.Buffer.TryTake(out string? result));
        Assert.Equal("abcdef", result);
    }
}

public sealed class TestWithGAPAttributeTests
{
    [Fact]
    public void ShouldBeUsableAsAttribute()
    {
        TestWithGAPAttribute attribute = new();

        Assert.IsAssignableFrom<Attribute>(attribute);
    }
}
