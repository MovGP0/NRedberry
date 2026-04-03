using NRedberry.Core.Utils;
using NRedberry.Transformations.Powerexpand;
using TensorApi = NRedberry.Tensors.Tensors;
using TensorType = NRedberry.Tensors.Tensor;

namespace NRedberry.Core.Tests.Transformations.Powerexpand;

public sealed class PowerExpandUtilsTests
{
    [Fact]
    public void ShouldDetectApplicableProductPower()
    {
        bool isApplicable = PowerExpandUtils.PowerExpandApplicable(
            TensorApi.Parse("Power[a*b, x]"),
            new TrueIndicator<TensorType>());

        isApplicable.ShouldBeTrue();
    }

    [Fact]
    public void ShouldDetectApplicableIndexedPowerUnfold()
    {
        bool isApplicable = PowerExpandUtils.PowerUnfoldApplicable(
            TensorApi.Parse("Power[A_a, x]"),
            new TrueIndicator<TensorType>());

        isApplicable.ShouldBeTrue();
    }

    [Fact]
    public void ShouldExpandToArrayForSelectedVariable()
    {
        NRedberry.Tensors.Power power = TensorApi.Parse("Power[a*b*c, d]")
            .ShouldBeOfType<NRedberry.Tensors.Power>();

        string[] actual = PowerExpandUtils.PowerExpandToArray(power, TensorApi.ParseSimple("a"))
            .Select(static tensor => tensor.ToString())
            .OrderBy(static tensor => tensor, StringComparer.Ordinal)
            .ToArray();

        string[] expected =
        [
            TensorApi.Parse("a**d").ToString(),
            TensorApi.Parse("(b*c)**d").ToString()
        ];
        Array.Sort(expected, StringComparer.Ordinal);

        actual.ShouldBe(expected);
    }

    [Fact]
    public void ShouldCreateIndicatorForNamedVariables()
    {
        IIndicator<TensorType> indicator = PowerExpandUtils.VarsToIndicator(
        [
            TensorApi.Parse("x").ShouldBeOfType<NRedberry.Tensors.SimpleTensor>()
        ]);

        indicator.Is(TensorApi.Parse("x")).ShouldBeTrue();
        indicator.Is(TensorApi.Parse("y")).ShouldBeFalse();
        indicator.Is(TensorApi.Parse("Power[x, a]")).ShouldBeTrue();
        indicator.Is(TensorApi.Parse("Power[y, a]")).ShouldBeFalse();
    }
}
