using NRedberry.Physics.Feyncalc;
using NRedberry.Tensors;
using TensorFactory = NRedberry.Tensors.Tensors;
using Xunit;

namespace NRedberry.Physics.Tests.Feyncalc;

public sealed class FeynCalcUtilsTest
{
    [Fact]
    public void ShouldCreateMandelstamSubstitutions()
    {
        Tensor[][] input =
        [
            [TensorFactory.Parse("k1_i"), TensorFactory.Parse("m1")],
            [TensorFactory.Parse("k2_i"), TensorFactory.Parse("m2")],
            [TensorFactory.Parse("k3_i"), TensorFactory.Parse("m3")],
            [TensorFactory.Parse("k4_i"), TensorFactory.Parse("m4")]
        ];

        Expression[] actual = FeynCalcUtils.SetMandelstam(input);

        actual.Length.ShouldBe(10);
        actual.ShouldSatisfyAllConditions(
            () => ShouldEqualExpression("k1_{i}*k1^{i} = m1**2", actual[0]),
            () => ShouldEqualExpression("k2_{i}*k2^{i} = m2**2", actual[1]),
            () => ShouldEqualExpression("k3_{i}*k3^{i} = m3**2", actual[2]),
            () => ShouldEqualExpression("k4_{i}*k4^{i} = m4**2", actual[3]),
            () => ShouldEqualExpression("2*k1_{i}*k2^{i} = s-m1**2-m2**2", actual[4]),
            () => ShouldEqualExpression("2*k3_{i}*k4^{i} = -m3**2+s-m4**2", actual[5]),
            () => ShouldEqualExpression("-2*k3^{i}*k1_{i} = -m3**2+t-m1**2", actual[6]),
            () => ShouldEqualExpression("-2*k4^{i}*k2_{i} = t-m4**2-m2**2", actual[7]),
            () => ShouldEqualExpression("-2*k4^{i}*k1_{i} = -m1**2-m4**2+u", actual[8]),
            () => ShouldEqualExpression("-2*k3^{i}*k2_{i} = -m3**2+u-m2**2", actual[9]));
    }

    [Fact]
    public void TestSetMandelstam1()
    {
        Tensor[][] input =
        [
            [TensorFactory.Parse("k1_i"), TensorFactory.Parse("m1")],
            [TensorFactory.Parse("k2_i"), TensorFactory.Parse("m2")],
            [TensorFactory.Parse("k3_i"), TensorFactory.Parse("m3")],
            [TensorFactory.Parse("k4_i"), TensorFactory.Parse("m4")]
        ];

        Expression[] ma = FeynCalcUtils.SetMandelstam(input);
        ma.Length.ShouldBe(10);
        ShouldEqualExpression("2*k4^{i}*k3_{i} = -m3**2+s-m4**2", ma[5]);
        ShouldEqualExpression("-2*k2_{i}*k3^{i} = -m3**2+u-m2**2", ma[9]);
    }

    [Fact]
    public void TestSetMandelstam5()
    {
        Tensor[][] input =
        [
            [TensorFactory.Parse("k1_i"), TensorFactory.Parse("m1")],
            [TensorFactory.Parse("k2_i"), TensorFactory.Parse("m2")],
            [TensorFactory.Parse("k3_i"), TensorFactory.Parse("m3")],
            [TensorFactory.Parse("k4_i"), TensorFactory.Parse("m4")],
            [TensorFactory.Parse("k5_i"), TensorFactory.Parse("m5")]
        ];

        Expression[] ma = FeynCalcUtils.SetMandelstam5(input);
        ma.Length.ShouldBe(15);
        ma.ShouldSatisfyAllConditions(
            () => ShouldEqualExpression("k1_{i}*k1^{i} = m1**2", ma[0]),
            () => ShouldEqualExpression("k5_{i}*k5^{i} = m5**2", ma[4]),
            () => ShouldEqualExpression("k1_{i}*k2^{i} = (1/2)*(s-m1**2-m2**2)", ma[5]),
            () => ShouldEqualExpression("k3_{i}*k1^{i} = (1/2)*(m3**2-t1+m1**2)", ma[6]),
            () => ShouldEqualExpression("k1^{i}*k4_{i} = (1/2)*(m1**2-t2+m4**2)", ma[7]),
            () => ShouldEqualExpression("k1^{i}*k5_{i} = (1/2)*(-m3**2+s+t1-m1**2+t2-m4**2-m2**2)", ma[8]),
            () => ShouldEqualExpression("k3_{i}*k2^{i} = (1/2)*(m3**2-u1+m2**2)", ma[9]),
            () => ShouldEqualExpression("k2^{i}*k4_{i} = (1/2)*(m4**2-u2+m2**2)", ma[10]),
            () => ShouldEqualExpression("k2^{i}*k5_{i} = (1/2)*(-m3**2+s+u1-m1**2-m4**2+u2-m2**2)", ma[11]),
            () => ShouldEqualExpression("k3_{i}*k4^{i} = (1/2)*(m3**2+2*m1**2+2*m2**2-s-t1+m5**2-u1-t2+m4**2-u2)", ma[12]),
            () => ShouldEqualExpression("k3_{i}*k5^{i} = (1/2)*(-m3**2+s-m5**2-m1**2+t2-m4**2+u2-m2**2)", ma[13]),
            () => ShouldEqualExpression("k4^{i}*k5_{i} = (1/2)*(-m3**2+s+t1-m5**2+u1-m1**2-m4**2-m2**2)", ma[14]));
    }

    private static void ShouldEqualExpression(string expected, Expression actual)
    {
        actual.ToString().ShouldBe(expected);
    }
}
