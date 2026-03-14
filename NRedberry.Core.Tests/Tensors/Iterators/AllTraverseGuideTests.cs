using NRedberry.Tensors.Iterators;
using TensorApi = NRedberry.Tensors.Tensors;
using Xunit;

namespace NRedberry.Core.Tests.Tensors.Iterators;

public sealed class AllTraverseGuideTests
{
    [Fact]
    public void ShouldAlwaysAllowTraversal()
    {
        AllTraverseGuide guide = new();
        NRedberry.Tensors.Tensor tensor = TensorApi.Parse("a");
        NRedberry.Tensors.Tensor parent = TensorApi.Parse("b");

        TraversePermission permission = guide.GetPermission(tensor, parent, 0);

        permission.ShouldBe(TraversePermission.Enter);
    }
}
