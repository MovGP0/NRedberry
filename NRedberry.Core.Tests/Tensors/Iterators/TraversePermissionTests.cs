using NRedberry.Tensors.Iterators;
using Xunit;

namespace NRedberry.Core.Tests.Tensors.Iterators;

public sealed class TraversePermissionTests
{
    [Fact]
    public void ShouldExposeExpectedPermissionValues()
    {
        ((int)TraversePermission.Enter).ShouldBe(0);
        ((int)TraversePermission.ShowButNotEnter).ShouldBe(1);
        ((int)TraversePermission.DontShow).ShouldBe(2);
        Enum.GetNames<TraversePermission>().ShouldBe(["Enter", "ShowButNotEnter", "DontShow"]);
    }
}
