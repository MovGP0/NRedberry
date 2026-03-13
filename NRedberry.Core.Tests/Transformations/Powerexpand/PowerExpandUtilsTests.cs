using NRedberry.Core.Utils;
using NRedberry.Transformations.Powerexpand;
using TensorApi = NRedberry.Tensors.Tensors;
using Xunit;

namespace NRedberry.Core.Tests.Transformations.Powerexpand;

public sealed class PowerExpandUtilsTests
{
    [Fact]
    public void ShouldThrowForApplicabilityChecks()
    {
        Assert.Throws<NotImplementedException>(() =>
            PowerExpandUtils.PowerExpandApplicable(
                TensorApi.Parse("Power[a+b,2]"),
                new TrueIndicator<NRedberry.Tensors.Tensor>()));
    }

    [Fact]
    public void ShouldThrowForExpansionHelpers()
    {
        Assert.Throws<NotImplementedException>(() =>
            PowerExpandUtils.PowerExpandToArray(
                TensorApi.Parse("Power[a+b,2]"),
                new TrueIndicator<NRedberry.Tensors.Tensor>()));
        Assert.Throws<NotImplementedException>(() =>
            PowerExpandUtils.PowerExpandToArray1(
                TensorApi.Parse("Power[a+b,2]"),
                new TrueIndicator<NRedberry.Tensors.Tensor>()));
        Assert.Throws<NotImplementedException>(() =>
            PowerExpandUtils.VarsToIndicator([Assert.IsType<NRedberry.Tensors.SimpleTensor>(TensorApi.Parse("x"))]));
    }
}
