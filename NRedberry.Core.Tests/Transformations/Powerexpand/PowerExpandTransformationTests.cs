using NRedberry.Core.Utils;
using NRedberry.Transformations.Powerexpand;
using NRedberry.Transformations.Symmetrization;
using TensorApi = NRedberry.Tensors.Tensors;
using Xunit;

namespace NRedberry.Core.Tests.Transformations.Powerexpand;

public sealed class PowerExpandTransformationTests
{
    [Fact]
    public void ShouldExpandAllFactorsByDefault()
    {
        NRedberry.Tensors.Tensor tensor = TensorApi.Parse("(a*b*c)**d");

        NRedberry.Tensors.Tensor actual = PowerExpandTransformation.Instance.Transform(tensor);

        Assert.True(NRedberry.Tensors.TensorUtils.EqualsExactly(actual, TensorApi.Parse("a**d*b**d*c**d")));
    }

    [Fact]
    public void ShouldExpandOnlySelectedVariable()
    {
        NRedberry.Tensors.Tensor tensor = TensorApi.Parse("(a*b*c)**d");
        NRedberry.Tensors.SimpleTensor variable = TensorApi.ParseSimple("a");
        PowerExpandTransformation transformation = new(variable);

        NRedberry.Tensors.Tensor actual = transformation.Transform(tensor);

        Assert.True(NRedberry.Tensors.TensorUtils.EqualsExactly(actual, TensorApi.Parse("a**d*(b*c)**d")));
    }

    [Fact]
    public void ShouldExpandVariablePowersThroughNestedProducts()
    {
        NRedberry.Tensors.Tensor tensor = TensorApi.Parse("((a**r*g)**e*b*c)**d");
        PowerExpandTransformation transformation = new(
            TensorApi.ParseSimple("a"),
            TensorApi.ParseSimple("g"));

        NRedberry.Tensors.Tensor actual = transformation.Transform(tensor);

        Assert.True(NRedberry.Tensors.TensorUtils.EqualsExactly(actual, TensorApi.Parse("a**(r*e*d)*g**(e*d)*(b*c)**d")));
    }

    [Fact]
    public void ShouldExpandOnlyTargetedFactorsInsideLargerExpression()
    {
        NRedberry.Tensors.Tensor tensor = TensorApi.Parse("((a**r*g)**e*b*c)**d*(a+b) + x");
        PowerExpandTransformation transformation = new(
            TensorApi.ParseSimple("a"),
            TensorApi.ParseSimple("c"));

        NRedberry.Tensors.Tensor actual = transformation.Transform(tensor);

        Assert.True(NRedberry.Tensors.TensorUtils.EqualsExactly(actual, TensorApi.Parse("a**(r*e*d)*c**d*(g**e*b)**d*(a+b) + x")));
    }

    [Fact]
    public void ShouldExpandToArrayForAllFactors()
    {
        NRedberry.Tensors.Power power = Assert.IsType<NRedberry.Tensors.Power>(TensorApi.Parse("(a*b*c)**d"));

        NRedberry.Tensors.Tensor[] actual = PowerExpandUtils.PowerExpandToArray(power);

        Assert.True(NRedberry.Tensors.TensorUtils.EqualsExactly(TensorApi.Multiply(actual), TensorApi.Parse("a**d*b**d*c**d")));
    }

    [Fact]
    public void ShouldTreatPowerOfTrackedSimpleTensorAsTracked()
    {
        IIndicator<NRedberry.Tensors.Tensor> indicator = PowerExpandUtils.VarsToIndicator([TensorApi.ParseSimple("a")]);

        Assert.True(indicator.Is(TensorApi.ParseSimple("a")));
        Assert.True(indicator.Is(TensorApi.Parse("a**r")));
        Assert.False(indicator.Is(TensorApi.Parse("b**r")));
        Assert.False(indicator.Is(TensorApi.Parse("(a*b)**r")));
    }

    [Fact]
    public void ShouldUsePowerExpandName()
    {
        Assert.Equal("PowerExpand", PowerExpandTransformation.Instance.ToString(OutputFormat.Redberry));
    }

    [Fact]
    public void ShouldAllowCustomIndicator()
    {
        PowerExpandTransformation transformation = new(new TrueIndicator<NRedberry.Tensors.Tensor>());
        NRedberry.Tensors.Tensor tensor = TensorApi.Parse("(a*b)**x");

        NRedberry.Tensors.Tensor actual = transformation.Transform(tensor);

        Assert.True(NRedberry.Tensors.TensorUtils.EqualsExactly(actual, TensorApi.Parse("a**x*b**x")));
    }

    [Fact]
    public void ShouldSupportTransformationCollection()
    {
        TransformationCollection collection = new(PowerExpandTransformation.Instance);
        NRedberry.Tensors.Tensor tensor = TensorApi.Parse("(a*b)**x");

        NRedberry.Tensors.Tensor actual = collection.Transform(tensor);

        Assert.True(NRedberry.Tensors.TensorUtils.EqualsExactly(actual, TensorApi.Parse("a**x*b**x")));
    }
}
