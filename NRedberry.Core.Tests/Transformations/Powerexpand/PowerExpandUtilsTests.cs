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
        Should.Throw<NotImplementedException>(() =>
            PowerExpandUtils.PowerExpandApplicable(
                TensorApi.Parse("Power[a+b,2]"),
                new TrueIndicator<NRedberry.Tensors.Tensor>()));
    }

    [Fact]
    public void ShouldThrowForExpansionHelpers()
    {
        Should.Throw<NotImplementedException>(() =>
            PowerExpandUtils.PowerExpandToArray(
                TensorApi.Parse("Power[a+b,2]"),
                new TrueIndicator<NRedberry.Tensors.Tensor>()));
        Should.Throw<NotImplementedException>(() =>
            PowerExpandUtils.PowerExpandToArray1(
                TensorApi.Parse("Power[a+b,2]"),
                new TrueIndicator<NRedberry.Tensors.Tensor>()));
        Should.Throw<NotImplementedException>(() =>
            PowerExpandUtils.VarsToIndicator([TensorApi.Parse("x").ShouldBeOfType<NRedberry.Tensors.SimpleTensor>()]));
    }
}
