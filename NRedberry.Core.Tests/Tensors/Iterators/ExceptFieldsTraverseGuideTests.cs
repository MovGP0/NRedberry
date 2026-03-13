using NRedberry.Tensors;
using NRedberry.Tensors.Iterators;
using TensorApi = NRedberry.Tensors.Tensors;
using Xunit;

namespace NRedberry.Core.Tests.Tensors.Iterators;

public sealed class ExceptFieldsTraverseGuideTests
{
    [Fact]
    public void ShouldShowTensorFieldsWithoutEnteringAndEnterOtherTensors()
    {
        ExceptFieldsTraverseGuide guide = new();
        NRedberry.Tensors.Tensor parent = TensorApi.Parse("a+b");
        TensorField field = Assert.IsType<TensorField>(TensorApi.Parse("f[x]"));
        NRedberry.Tensors.Tensor scalar = TensorApi.Parse("x");

        Assert.Equal(TraversePermission.ShowButNotEnter, guide.GetPermission(field, parent, 0));
        Assert.Equal(TraversePermission.Enter, guide.GetPermission(scalar, parent, 1));
    }
}
