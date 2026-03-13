using NRedberry.Tensors;
using NRedberry.Tensors.Functions;
using NRedberry.Tensors.Iterators;
using TensorApi = NRedberry.Tensors.Tensors;
using Xunit;

namespace NRedberry.Core.Tests.Tensors.Iterators;

public sealed class ExceptFunctionsAndFieldsTraverseGuideTests
{
    [Fact]
    public void ShouldHideFunctionsShowFieldsAndEnterOtherTensors()
    {
        ExceptFunctionsAndFieldsTraverseGuide guide = new();
        NRedberry.Tensors.Tensor parent = TensorApi.Parse("a+b");
        ScalarFunction function = Assert.IsAssignableFrom<ScalarFunction>(TensorApi.Parse("Sin[x]"));
        TensorField field = Assert.IsType<TensorField>(TensorApi.Parse("f[x]"));
        NRedberry.Tensors.Tensor scalar = TensorApi.Parse("x");

        Assert.Equal(TraversePermission.DontShow, guide.GetPermission(function, parent, 0));
        Assert.Equal(TraversePermission.ShowButNotEnter, guide.GetPermission(field, parent, 1));
        Assert.Equal(TraversePermission.Enter, guide.GetPermission(scalar, parent, 2));
    }
}
