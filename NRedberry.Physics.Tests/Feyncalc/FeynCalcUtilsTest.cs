using NRedberry.Physics.Feyncalc;
using NRedberry.Tensors;
using Shouldly;
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
            () => ShouldEqualExpression("k1_i*k1^i", "m1**2", actual[0]),
            () => ShouldEqualExpression("k2_i*k2^i", "m2**2", actual[1]),
            () => ShouldEqualExpression("k3_i*k3^i", "m3**2", actual[2]),
            () => ShouldEqualExpression("k4_i*k4^i", "m4**2", actual[3]),
            () => ShouldEqualExpression("2*k1_i*k2^i", "s-m1**2-m2**2", actual[4]),
            () => ShouldEqualExpression("2*k3_i*k4^i", "s-m3**2-m4**2", actual[5]),
            () => ShouldEqualExpression("-2*k1_i*k3^i", "t-m1**2-m3**2", actual[6]),
            () => ShouldEqualExpression("-2*k2_i*k4^i", "t-m2**2-m4**2", actual[7]),
            () => ShouldEqualExpression("-2*k1_i*k4^i", "u-m1**2-m4**2", actual[8]),
            () => ShouldEqualExpression("-2*k2_i*k3^i", "u-m2**2-m3**2", actual[9]));
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
        ShouldEqualExpression("2*k3_i*k4^i", "s-m3**2-m4**2", ma[5]);
        ShouldEqualExpression("-2*k2_i*k3^i", "u-m2**2-m3**2", ma[9]);
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
            () => ShouldEqualExpression("k1_i*k1^i", "m1**2", ma[0]),
            () => ShouldEqualExpression("k5_i*k5^i", "m5**2", ma[4]),
            () => ShouldEqualExpression("k1_i*k2^i", "(1/2)*(-m1**2-m2**2+s)", ma[5]),
            () => ShouldEqualExpression("k3^i*k1_i", "(1/2)*(m1**2+m3**2-t1)", ma[6]),
            () => ShouldEqualExpression("k4^i*k1_i", "(1/2)*(m1**2-t2+m4**2)", ma[7]),
            () => ShouldEqualExpression("k5^i*k1_i", "(1/2)*(-m1**2-m2**2-m3**2+s+t2-m4**2+t1)", ma[8]),
            () => ShouldEqualExpression("k3^i*k2_i", "(1/2)*(-u1+m2**2+m3**2)", ma[9]),
            () => ShouldEqualExpression("k4^i*k2_i", "(1/2)*(-u2+m2**2+m4**2)", ma[10]),
            () => ShouldEqualExpression("k5^i*k2_i", "(1/2)*(u1-m1**2+u2-m2**2-m3**2+s-m4**2)", ma[11]),
            () => ShouldEqualExpression("k3_i*k4^i", "(1/2)*(-u1-u2+2*m1**2+m3**2-s-t2+m4**2+2*m2**2-t1+m5**2)", ma[12]),
            () => ShouldEqualExpression("k5^i*k3_i", "(1/2)*(-m1**2+u2-m2**2-m3**2+s+t2-m4**2-m5**2)", ma[13]),
            () => ShouldEqualExpression("k5^i*k4_i", "(1/2)*(u1-m1**2-m2**2-m3**2+s-m4**2+t1-m5**2)", ma[14]));
    }

    private static void ShouldEqualExpression(string expectedLeft, string expectedRight, Expression actual)
    {
        TensorUtils.Equals(TensorFactory.Parse(expectedLeft), actual[0]).ShouldBeTrue("Left side comparison failed.");
        TensorUtils.Equals(TensorFactory.Parse(expectedRight), actual[1]).ShouldBeTrue("Right side comparison failed.");
    }
}
