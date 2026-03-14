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
        ScalarFunction function = TensorApi.Parse("Sin[x]").ShouldBeAssignableTo<ScalarFunction>();
        TensorField field = TensorApi.Parse("f[x]").ShouldBeOfType<TensorField>();
        NRedberry.Tensors.Tensor scalar = TensorApi.Parse("x");

        guide.GetPermission(function, parent, 0).ShouldBe(TraversePermission.DontShow);
        guide.GetPermission(field, parent, 1).ShouldBe(TraversePermission.ShowButNotEnter);
        guide.GetPermission(scalar, parent, 2).ShouldBe(TraversePermission.Enter);
    }
}
