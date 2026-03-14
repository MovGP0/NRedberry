using System.IO;
using System.Text;
using Shouldly;
using Xunit;

namespace NRedberry.Core.Tests.Groups.Permutations;

public sealed class GapGroupsInterfaceTest
{
    [Fact]
    public void ShouldConvertPermutationToGapList()
    {
        GapGroupsInterface.ConvertToGapList([0, 2, 1]).ShouldBe("[1, 3, 2]");
    }

    [Theory]
    [InlineData("Group(())", "Group(());")]
    [InlineData("Group(());", "Group(());")]
    public void ShouldNormalizeCommandsForGap(string input, string expected)
    {
        GapGroupsInterface.NormalizeCommandForGap(input).ShouldBe(expected);
    }

    [Theory]
    [InlineData("Group(())", "Group(())")]
    [InlineData("Group(());", "Group(())")]
    public void ShouldNormalizeCommandsFromGap(string input, string expected)
    {
        GapGroupsInterface.NormalizeCommandFromGap(input).ShouldBe(expected);
    }

    [Fact]
    public void ShouldReadGapOutputBatchesUntilEof()
    {
        byte[] bytes = Encoding.UTF8.GetBytes("gap> abc\\\n gap> def\nEOF\n");
        using MemoryStream stream = new(bytes);
        GapOutputReader reader = new(stream);

        reader.Run();

        reader.Buffer.TryTake(out string? result).ShouldBeTrue();
        result.ShouldBe("abcdef");
    }
}

public sealed class TestWithGAPAttributeTests
{
    [Fact]
    public void ShouldBeUsableAsAttribute()
    {
        TestWithGAPAttribute attribute = new();

        attribute.ShouldBeAssignableTo<Attribute>();
    }
}
