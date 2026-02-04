using System;
using NRedberry.Numbers;
using NRedberry.Tensors;
using NRedberry.Transformations.Expand;
using NRedberry.Transformations.Factor;
using TensorCC = NRedberry.Tensors.CC;
using TensorFactory = NRedberry.Tensors.Tensors;
using TensorType = NRedberry.Tensors.Tensor;
using Xunit;

namespace NRedberry.Core.Tests.Transformations.Factor;

public sealed class FactorTransformationTest
{
    [Fact]
    public void Test1()
    {
        for (int i = 0; i < 20; ++i)
        {
            TensorCC.ResetTensorNames();
            TensorType tensor = TensorFactory.Parse("2304*m**2*N*m**8 - 1152*s*N*m**8 + 288*m**6*N*s**2 - 1536*m**8*N*t + 480*m**6*N*s*t - 48*m**4*N*s**2*t + 352*m**6*N*t**2 - 56*m**4*N*s*t**2 + 2*m**2*N*s**2*t**2 - 32*m**4*N*t**3 + 2*m**2*N*s*t**3 + m**2*N*t**4");
            Assert.True(TensorUtils.Equals(tensor, ExpandTransformation.Expand(FactorTransformation.Factor(tensor))));
        }
    }

    [Fact]
    public void Test2()
    {
        for (int i = 0; i < 20; ++i)
        {
            TensorCC.ResetTensorNames();
            TensorType tensor = TensorFactory.Parse("2304*m**2*N*m**8 - 1152*s*N*m**8 + 288*m**6*N*s**2 - 1536*m**8*N*t + 480*m**6*N*s*t - 48*m**4*N*s**2*t + 352*m**6*N*t**2 - 56*m**4*N*s*t**2 + 2*m**2*N*s**2*t**2 - 32*m**4*N*t**3 + 2*m**2*N*s*t**3 + m**2*N*t**4 + 1");
            Assert.True(TensorUtils.Equals(tensor, ExpandTransformation.Expand(FactorTransformation.Factor(tensor))));
        }
    }

    [Fact]
    public void Test3()
    {
        TensorType tensor = TensorFactory.Parse("-(1/4)*e**4*m**2*s**3+(3/8)*e**4*m**4*s**2+(1/8)*s*e**4*m**4*t+(1/16)*e**4*t**2*m**4+(1/16)*e**4*m**8-(1/4)*e**4*m**2*t*s**2+(1/16)*e**4*t**2*s**2+(1/16)*e**4*s**4+(1/8)*e**4*t*s**3-(1/4)*s*e**4*m**6");
        TensorType expected = TensorFactory.Parse("(1/16)*e**4*(t**2*s**2-4*m**2*s**3+6*m**4*s**2+2*t*s**3-4*s*m**6-4*t*m**2*s**2+2*s*t*m**4+m**8+m**4*t**2+s**4)");
        Assert.True(TensorUtils.Equals(FactorTransformation.Factor(tensor), expected));
    }

    [Fact]
    public void Test4()
    {
        TensorType tensor = TensorFactory.Parse("(x + y + z + 56*x + i)**10");
        TensorType normalized = TensorFactory.Parse("362033331456891249*((1/57)*z+(1/57)*y+(1/57)*i+x)**10");
        TensorType expanded = ExpandTransformation.Expand(tensor);
        TensorType factored = FactorTransformation.Factor(expanded);
        Assert.True(TensorUtils.Equals(factored, tensor) || TensorUtils.Equals(factored, normalized));
    }

    [Fact]
    public void Test5()
    {
        TensorType tensor = TensorFactory.Parse("(x - y + z)**2*(a+b)**3");
        TensorType expanded = ExpandTransformation.Expand(tensor);
        Assert.True(TensorUtils.Equals(FactorTransformation.Factor(expanded), tensor));

        tensor = TensorFactory.Parse("(x - y + a)**2*(a+b)**3");
        expanded = ExpandTransformation.Expand(tensor);
        Assert.True(TensorUtils.Equals(FactorTransformation.Factor(expanded), tensor));

        tensor = TensorFactory.Parse("(x - y + a)**2*(a+b)**3*(x + b)");
        expanded = ExpandTransformation.Expand(tensor);
        Assert.True(TensorUtils.Equals(FactorTransformation.Factor(expanded), tensor));

        tensor = TensorFactory.Parse("(x - y - a)**2*(a - b)**3*(x - b)");
        expanded = ExpandTransformation.Expand(tensor);
        Assert.True(TensorUtils.Equals(FactorTransformation.Factor(expanded), tensor));

        tensor = TensorFactory.Parse("(x - y - a)**2*(a - b)**3*(x - b)**2");
        expanded = ExpandTransformation.Expand(tensor);
        Assert.True(TensorUtils.Equals(FactorTransformation.Factor(expanded), tensor));

        tensor = TensorFactory.Parse("(x - y - a)**2*(a - b)**3*(x - b)**2*(p + q)");
        expanded = ExpandTransformation.Expand(tensor);
        Assert.True(TensorUtils.Equals(FactorTransformation.Factor(expanded), tensor));

        tensor = TensorFactory.Parse("(x**12 - y**2 - a)**2*(a - b**3)**3*(x**5 - b**9)**2*(p + q)");
        expanded = ExpandTransformation.Expand(tensor);
        Assert.True(TensorUtils.Equals(FactorTransformation.Factor(expanded), tensor));
    }

    [Fact(Skip = "Ignored in original test suite.")]
    public void Test6r()
    {
    }

    [Fact(Skip = "Ignored in original test suite.")]
    public void Test6ra()
    {
    }

    [Fact]
    public void Test7()
    {
        TensorType tensor = ExpandTransformation.Expand(TensorFactory.Parse("2*((1/2)*m*t**4-4*m**3*t**3+8*m**5*t**2)"));
        _ = FactorTransformation.Factor(tensor);
        tensor = ExpandTransformation.Expand(TensorFactory.Parse("((1/2)*m*t**4-4*m**3*t**3+8*m**5*t**2)"));
        _ = FactorTransformation.Factor(tensor);
    }

    [Fact(Skip = "FactorOut API is not exposed in the C# port yet.")]
    public void TestFactorOut8()
    {
    }

    [Fact(Skip = "FactorOut API is not exposed in the C# port yet.")]
    public void TestFactorOut9()
    {
    }

    [Fact(Skip = "FactorOut API is not exposed in the C# port yet.")]
    public void TestFactorOut10()
    {
    }

    [Fact(Skip = "FactorOut API is not exposed in the C# port yet.")]
    public void TestFactorOut11()
    {
    }

    [Fact(Skip = "FactorOut API is not exposed in the C# port yet.")]
    public void TestFactorOut12()
    {
    }

    [Fact(Skip = "FactorOut API is not exposed in the C# port yet.")]
    public void TestFactorOut13()
    {
    }

    [Fact(Skip = "FactorOut API is not exposed in the C# port yet.")]
    public void TestFactorOut14()
    {
    }

    [Fact(Skip = "FactorOut API is not exposed in the C# port yet.")]
    public void TestFactorOut15()
    {
    }

    [Fact(Skip = "FactorOut API is not exposed in the C# port yet.")]
    public void TestFactorOut16()
    {
    }

    [Fact(Skip = "FactorOut API is not exposed in the C# port yet.")]
    public void TestFactorOut17()
    {
    }

    [Fact]
    public void Test18()
    {
        TensorType tensor = TensorFactory.Parse("(a+a*b)**(-2) + a");
        Assert.True(TensorUtils.Equals(FactorTransformation.Factor(tensor), TensorFactory.Parse("(1 + a**3 + 2*a**3*b + a**3*b**2)/(a**2*(1 + b)**2)")));
    }

    [Fact]
    public void Test17()
    {
        TensorType tensor = TensorFactory.Parse("(a+a*b)**(-1) + 1/a");
        Assert.True(TensorUtils.Equals(FactorTransformation.Factor(tensor), TensorFactory.Parse("(2+b)/(a*(1 + b))")));
    }

    [Fact(Skip = "Ignored in original test suite (broken JAS).")]
    public void Test19()
    {
    }

    [Fact]
    public void Test19a()
    {
        TensorType tensor = TensorFactory.Parse("a+b*a+(a**2+2*a*b+b**2)/(a+b)*F^i_i+(a**2+2*a*b+b**2)*(a+b)**(-1)*H^i_i");
        Assert.True(TensorUtils.Equals(
            FactorTransformation.Factor(tensor, false),
            TensorFactory.Parse("(a+b)*H^{i}_{i}+(a+b)*F^{i}_{i}+a*(1+b)")));

        tensor = TensorFactory.Parse("(a**2+2*a*b+b**2)/(a+b)*F^i_i+(a**2+2*a*b+b**2)*(a+b)**(-1)*H^i_i");
        Assert.True(TensorUtils.Equals(
            FactorTransformation.Factor(tensor, false),
            TensorFactory.Parse("(a+b)*H^{i}_{i}+(a+b)*F^{i}_{i}")));
    }

    [Fact(Skip = "Ignored in original test suite (broken JAS).")]
    public void Test19b()
    {
    }

    [Fact(Skip = "Ignored in original test suite.")]
    public void Test20()
    {
    }

    [Fact(Skip = "Ignored in original test suite.")]
    public void Test15Ignored()
    {
    }

    [Fact]
    public void Test21()
    {
        Assert.True(TensorUtils.Equals(
            FactorTransformation.Factor(TensorFactory.Parse("2*I*a + 4*I*b")),
            TensorFactory.Parse("2*I*(a + 2*b)")));

        Assert.True(TensorUtils.Equals(
            FactorTransformation.Factor(TensorFactory.Parse("2*I*a + 4*(I*b + I*c)")),
            TensorFactory.Parse("2*I*(a + 2*b + 2*c)")));

        Assert.True(TensorUtils.Equals(
            FactorTransformation.Factor(TensorFactory.Parse("2*I*a + 4*(-I*b + I*c)")),
            TensorFactory.Parse("2*I*(a - 2*b + 2*c)")));

        Assert.True(TensorUtils.Equals(
            FactorTransformation.Factor(TensorFactory.Parse("2*I*(I*a + I*d) + 4*I*(-I*b + I*c)")),
            TensorFactory.Parse("2*(-a -d + 2*b - 2*c)")));
    }

    [Fact]
    public void Test22()
    {
        TensorType tensor = TensorFactory.Parse("I*(a+b) - I*a - I*b");
        Assert.True(TensorUtils.Equals(FactorTransformation.Factor(tensor), Complex.Zero));
    }

    [Fact]
    public void Test23()
    {
        TensorType tensor = TensorFactory.Parse("I*(a+b) - I*a - I*b + c*I");
        Assert.True(TensorUtils.Equals(FactorTransformation.Factor(tensor), TensorFactory.Parse("c*I")));
    }

    [Fact]
    public void Test24()
    {
        TensorType tensor = TensorFactory.Parse("a**(-1)*b-(b*rc3+a)**(-1)*b-(b*rc3+a)**(-1)*b**2*a**(-1)*rc3");
        Assert.True(TensorUtils.Equals(FactorTransformation.Factor(tensor), TensorFactory.Parse("0")));
    }

    [Fact]
    public void Test25()
    {
        TensorType tensor = TensorFactory.Parse("I*a + f");
        Assert.True(TensorUtils.Equals(FactorTransformation.Factor(tensor), TensorFactory.Parse("I*a + f")));
    }
}
