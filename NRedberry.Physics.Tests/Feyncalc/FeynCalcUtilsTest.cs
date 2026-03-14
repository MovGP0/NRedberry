using NRedberry.Physics.Feyncalc;
using NRedberry.Tensors;
using TensorFactory = NRedberry.Tensors.Tensors;
using Xunit;

namespace NRedberry.Physics.Tests.Feyncalc;

public sealed class FeynCalcUtilsTest
{
    [Fact]
    public void ShouldThrowSetMandelstamUntilPorted()
    {
        Tensor[][] input =
        [
            [TensorFactory.Parse("k1_i"), TensorFactory.Parse("m1")],
            [TensorFactory.Parse("k2_i"), TensorFactory.Parse("m2")],
            [TensorFactory.Parse("k3_i"), TensorFactory.Parse("m3")],
            [TensorFactory.Parse("k4_i"), TensorFactory.Parse("m4")]
        ];

        Assert.Throws<NotImplementedException>(() => FeynCalcUtils.SetMandelstam(input));
    }

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

        foreach (Expression expression in ma)
        {
            Console.WriteLine(expression);
        }
    }

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

        foreach (Expression expression in ma)
        {
            Console.WriteLine(expression);
        }
    }
}
