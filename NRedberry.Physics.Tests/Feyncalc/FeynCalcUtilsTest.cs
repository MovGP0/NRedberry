using System;
using NRedberry.Physics.Feyncalc;
using NRedberry.Tensors;
using TensorFactory = NRedberry.Tensors.Tensors;

namespace NRedberry.Physics.Tests.Feyncalc;

public sealed class FeynCalcUtilsTest
{
    public void TestSetMandelstam1()
    {
        Tensor[][] input =
        {
            new[] { TensorFactory.Parse("k1_i"), TensorFactory.Parse("m1") },
            new[] { TensorFactory.Parse("k2_i"), TensorFactory.Parse("m2") },
            new[] { TensorFactory.Parse("k3_i"), TensorFactory.Parse("m3") },
            new[] { TensorFactory.Parse("k4_i"), TensorFactory.Parse("m4") }
        };

        Expression[] ma = FeynCalcUtils.SetMandelstam(input);

        foreach (Expression expression in ma)
        {
            Console.WriteLine(expression);
        }
    }

    public void TestSetMandelstam5()
    {
        Tensor[][] input =
        {
            new[] { TensorFactory.Parse("k1_i"), TensorFactory.Parse("m1") },
            new[] { TensorFactory.Parse("k2_i"), TensorFactory.Parse("m2") },
            new[] { TensorFactory.Parse("k3_i"), TensorFactory.Parse("m3") },
            new[] { TensorFactory.Parse("k4_i"), TensorFactory.Parse("m4") },
            new[] { TensorFactory.Parse("k5_i"), TensorFactory.Parse("m5") }
        };

        Expression[] ma = FeynCalcUtils.SetMandelstam5(input);

        foreach (Expression expression in ma)
        {
            Console.WriteLine(expression);
        }
    }
}
