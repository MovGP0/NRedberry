using NRedberry.Tensors.Iterators;
using Xunit;

namespace NRedberry.Core.Tests.Tensors.Iterators;

public sealed class TraversePermissionTests
{
    [Fact]
    public void ShouldExposeExpectedPermissionValues()
    {
        Assert.Equal(0, (int)TraversePermission.Enter);
        Assert.Equal(1, (int)TraversePermission.ShowButNotEnter);
        Assert.Equal(2, (int)TraversePermission.DontShow);
        Assert.Equal(
            ["Enter", "ShowButNotEnter", "DontShow"],
            Enum.GetNames<TraversePermission>());
    }
}
