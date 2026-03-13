using NRedberry.Tensors;
using NRedberry.Tensors.Functions;
using NRedberry.Tensors.Iterators;
using TensorApi = NRedberry.Tensors.Tensors;
using Xunit;

namespace NRedberry.Core.Tests.Tensors.Iterators;

public sealed class TraverseGuideTests
{
    [Fact]
    public void ShouldExposeDefaultGuideInstances()
    {
        NRedberry.Tensors.Tensor parent = TensorApi.Parse("a+b");
        NRedberry.Tensors.Tensor scalar = TensorApi.Parse("x");
        ScalarFunction function = Assert.IsAssignableFrom<ScalarFunction>(TensorApi.Parse("Sin[x]"));
        TensorField field = Assert.IsType<TensorField>(TensorApi.Parse("f[x]"));

        Assert.IsType<AllTraverseGuide>(TraverseGuide.All);
        Assert.IsType<ExceptFunctionsAndFieldsTraverseGuide>(TraverseGuide.ExceptFunctionsAndFields);
        Assert.IsType<ExceptFieldsTraverseGuide>(TraverseGuide.ExceptFields);

        Assert.Equal(TraversePermission.Enter, TraverseGuide.All.GetPermission(scalar, parent, 0));
        Assert.Equal(TraversePermission.DontShow, TraverseGuide.ExceptFunctionsAndFields.GetPermission(function, parent, 1));
        Assert.Equal(TraversePermission.ShowButNotEnter, TraverseGuide.ExceptFields.GetPermission(field, parent, 2));
    }
}
