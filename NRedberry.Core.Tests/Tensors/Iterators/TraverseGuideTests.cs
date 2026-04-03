using NRedberry.Tensors;
using NRedberry.Tensors.Functions;
using NRedberry.Tensors.Iterators;
using TensorApi = NRedberry.Tensors.Tensors;

namespace NRedberry.Core.Tests.Tensors.Iterators;

public sealed class TraverseGuideTests
{
    [Fact]
    public void ShouldExposeDefaultGuideInstances()
    {
        NRedberry.Tensors.Tensor parent = TensorApi.Parse("a+b");
        NRedberry.Tensors.Tensor scalar = TensorApi.Parse("x");
        ScalarFunction function = TensorApi.Parse("Sin[x]").ShouldBeAssignableTo<ScalarFunction>();
        TensorField field = TensorApi.Parse("f[x]").ShouldBeOfType<TensorField>();

        TraverseGuide.All.ShouldBeOfType<AllTraverseGuide>();
        TraverseGuide.ExceptFunctionsAndFields.ShouldBeOfType<ExceptFunctionsAndFieldsTraverseGuide>();
        TraverseGuide.ExceptFields.ShouldBeOfType<ExceptFieldsTraverseGuide>();

        TraverseGuide.All.GetPermission(scalar, parent, 0).ShouldBe(TraversePermission.Enter);
        TraverseGuide.ExceptFunctionsAndFields.GetPermission(function, parent, 1).ShouldBe(TraversePermission.DontShow);
        TraverseGuide.ExceptFields.GetPermission(field, parent, 2).ShouldBe(TraversePermission.ShowButNotEnter);
    }
}
