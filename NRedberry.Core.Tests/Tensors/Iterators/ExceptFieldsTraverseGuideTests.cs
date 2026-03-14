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
        TensorField field = TensorApi.Parse("f[x]").ShouldBeOfType<TensorField>();
        NRedberry.Tensors.Tensor scalar = TensorApi.Parse("x");

        guide.GetPermission(field, parent, 0).ShouldBe(TraversePermission.ShowButNotEnter);
        guide.GetPermission(scalar, parent, 1).ShouldBe(TraversePermission.Enter);
    }
}
