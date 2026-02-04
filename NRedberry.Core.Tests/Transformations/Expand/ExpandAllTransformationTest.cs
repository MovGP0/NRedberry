using NRedberry.Tensors;
using NRedberry.Transformations.Expand;
using NRedberry.Transformations.Symmetrization;
using TensorFactory = NRedberry.Tensors.Tensors;
using TensorType = NRedberry.Tensors.Tensor;
using Xunit;

namespace NRedberry.Core.Tests.Transformations.Expand;

public sealed class ExpandAllTransformationTest
{
    [Fact]
    public void ShouldLeaveSimpleReciprocalUnchanged()
    {
        TensorType tensor = TensorFactory.Parse("1/(a+b)");
        TensorType actual = ExpandAllTransformation.Expand(tensor);
        Assert.True(ReferenceEquals(actual, tensor));
    }

    [Fact]
    public void ShouldExpandDenominatorPower()
    {
        TensorType actual = ExpandAllTransformation.Expand(TensorFactory.Parse("1/(a+b)**2"));
        TensorType expected = TensorFactory.Parse("1/(a**2+2*a*b+b**2)");
        Assert.True(TensorUtils.Equals(expected, actual));
    }

    [Fact]
    public void ShouldExpandNumeratorAndDenominator()
    {
        TensorType actual = ExpandAllTransformation.Expand(TensorFactory.Parse("(c+d)/(a+b)**2"));
        TensorType expected = TensorFactory.Parse("c/(a**2+2*a*b+b**2)+d/(a**2+2*a*b+b**2)");
        Assert.True(TensorUtils.Equals(expected, actual));
    }

    [Fact]
    public void ShouldExpandAllBrackets()
    {
        TensorType actual = ExpandAllTransformation.Expand(
            TensorFactory.Parse("((a+b)*(c+a)-a)*f_mn*(f^mn+r^mn)-((a-b)*(c-a)+a)*r_ab*(f^ab+r^ab)"));
        ExpandTest.AssertAllBracketsExpanded(actual);
        TensorType expected = TensorFactory.Parse("(2*c*b+2*Power[a, 2]+-2*a)*r_{ab}*f^{ab}+(-1*b*a+c*b+-1*c*a+Power[a, 2]+-1*a)*r^{ab}*r_{ab}+(b*a+c*b+c*a+Power[a, 2]+-1*a)*f^{mn}*f_{mn}");
        Assert.True(TensorUtils.Equals(actual, expected));
    }

    [Fact]
    public void ShouldExpandComplexFraction()
    {
        TensorType actual = ExpandAllTransformation.Expand(
            TensorFactory.Parse("1/((a + b)*(c + a)) + ((a + b)**2/(v +i)**2)*(1/((a + b)*(c + a)) + (a + c)**2/(v + i)**2)"));
        TensorType expected = TensorFactory.Parse("b**2*(i**2+2*v*i+v**2)**(-2)*c**2+2*(i**2+2*v*i+v**2)**(-2)*a**3*b+2*(i**2+2*v*i+v**2)**(-2)*a**3*c+(i**2+2*v*i+v**2)**(-1)*a**2*(c*a+a**2+b*c+b*a)**(-1)+2*b**2*(i**2+2*v*i+v**2)**(-2)*c*a+2*(i**2+2*v*i+v**2)**(-2)*b*a*c**2+(i**2+2*v*i+v**2)**(-2)*a**4+(i**2+2*v*i+v**2)**(-2)*a**2*c**2+(c*a+a**2+b*c+b*a)**(-1)+2*(i**2+2*v*i+v**2)**(-1)*b*(c*a+a**2+b*c+b*a)**(-1)*a+b**2*(i**2+2*v*i+v**2)**(-2)*a**2+b**2*(i**2+2*v*i+v**2)**(-1)*(c*a+a**2+b*c+b*a)**(-1)+4*(i**2+2*v*i+v**2)**(-2)*a**2*b*c");
        Assert.True(TensorUtils.Equals(expected, actual));
    }

    [Fact]
    public void ShouldExpandWithMetricElimination()
    {
        TensorType tensor = TensorFactory.Parse("Sin[R_abcd*R^abcd]");
        tensor = TensorFactory.ParseExpression("R_abcd = 1/3*(g_ac*g_bd - g_bc*g_ad)").Transform(tensor);
        tensor = ExpandAllTransformation.Expand(
            tensor,
            EliminateMetricsTransformation.Instance,
            TensorFactory.ParseExpression("d_i^i = 4"));
        Assert.True(TensorUtils.Equals(tensor, TensorFactory.Parse("Sin[8/3]")));
    }

    [Fact]
    public void ShouldExpandWithMetricEliminationAndScale()
    {
        TensorType tensor = TensorFactory.Parse("Sin[1/la**2*R_abcd*R^abcd]");
        tensor = TensorFactory.ParseExpression("R_abcd = 1/3*(g_ac*g_bd - g_bc*g_ad)").Transform(tensor);
        tensor = ExpandAllTransformation.Expand(
            tensor,
            EliminateMetricsTransformation.Instance,
            TensorFactory.ParseExpression("d_i^i = 4"));
        Assert.True(TensorUtils.Equals(tensor, TensorFactory.Parse("Sin[1/la**2*8/3]")));
    }
}
