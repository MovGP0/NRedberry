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
            TensorUtils.Equals(tensor, ExpandTransformation.Expand(FactorTransformation.Factor(tensor))).ShouldBeTrue();
        }
    }

    [Fact]
    public void Test2()
    {
        for (int i = 0; i < 20; ++i)
        {
            TensorCC.ResetTensorNames();
            TensorType tensor = TensorFactory.Parse("2304*m**2*N*m**8 - 1152*s*N*m**8 + 288*m**6*N*s**2 - 1536*m**8*N*t + 480*m**6*N*s*t - 48*m**4*N*s**2*t + 352*m**6*N*t**2 - 56*m**4*N*s*t**2 + 2*m**2*N*s**2*t**2 - 32*m**4*N*t**3 + 2*m**2*N*s*t**3 + m**2*N*t**4 + 1");
            TensorUtils.Equals(tensor, ExpandTransformation.Expand(FactorTransformation.Factor(tensor))).ShouldBeTrue();
        }
    }

    [Fact]
    public void Test3()
    {
        TensorType tensor = TensorFactory.Parse("-(1/4)*e**4*m**2*s**3+(3/8)*e**4*m**4*s**2+(1/8)*s*e**4*m**4*t+(1/16)*e**4*t**2*m**4+(1/16)*e**4*m**8-(1/4)*e**4*m**2*t*s**2+(1/16)*e**4*t**2*s**2+(1/16)*e**4*s**4+(1/8)*e**4*t*s**3-(1/4)*s*e**4*m**6");
        TensorType expected = TensorFactory.Parse("(1/16)*e**4*(t**2*s**2-4*m**2*s**3+6*m**4*s**2+2*t*s**3-4*s*m**6-4*t*m**2*s**2+2*s*t*m**4+m**8+m**4*t**2+s**4)");
        TensorUtils.Equals(FactorTransformation.Factor(tensor), expected).ShouldBeTrue();
    }

    [Fact]
    public void Test4()
    {
        TensorType tensor = TensorFactory.Parse("(x + y + z + 56*x + i)**10");
        TensorType normalized = TensorFactory.Parse("362033331456891249*((1/57)*z+(1/57)*y+(1/57)*i+x)**10");
        TensorType expanded = ExpandTransformation.Expand(tensor);
        TensorType factored = FactorTransformation.Factor(expanded);
        (TensorUtils.Equals(factored, tensor) || TensorUtils.Equals(factored, normalized)).ShouldBeTrue();
    }

    [Fact]
    public void Test5()
    {
        TensorType tensor = TensorFactory.Parse("(x - y + z)**2*(a+b)**3");
        TensorType expanded = ExpandTransformation.Expand(tensor);
        TensorUtils.Equals(FactorTransformation.Factor(expanded), tensor).ShouldBeTrue();

        tensor = TensorFactory.Parse("(x - y + a)**2*(a+b)**3");
        expanded = ExpandTransformation.Expand(tensor);
        TensorUtils.Equals(FactorTransformation.Factor(expanded), tensor).ShouldBeTrue();

        tensor = TensorFactory.Parse("(x - y + a)**2*(a+b)**3*(x + b)");
        expanded = ExpandTransformation.Expand(tensor);
        TensorUtils.Equals(FactorTransformation.Factor(expanded), tensor).ShouldBeTrue();

        tensor = TensorFactory.Parse("(x - y - a)**2*(a - b)**3*(x - b)");
        expanded = ExpandTransformation.Expand(tensor);
        TensorUtils.Equals(FactorTransformation.Factor(expanded), tensor).ShouldBeTrue();

        tensor = TensorFactory.Parse("(x - y - a)**2*(a - b)**3*(x - b)**2");
        expanded = ExpandTransformation.Expand(tensor);
        TensorUtils.Equals(FactorTransformation.Factor(expanded), tensor).ShouldBeTrue();

        tensor = TensorFactory.Parse("(x - y - a)**2*(a - b)**3*(x - b)**2*(p + q)");
        expanded = ExpandTransformation.Expand(tensor);
        TensorUtils.Equals(FactorTransformation.Factor(expanded), tensor).ShouldBeTrue();

        tensor = TensorFactory.Parse("(x**12 - y**2 - a)**2*(a - b**3)**3*(x**5 - b**9)**2*(p + q)");
        expanded = ExpandTransformation.Expand(tensor);
        TensorUtils.Equals(FactorTransformation.Factor(expanded), tensor).ShouldBeTrue();
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
        TensorUtils.Equals(FactorTransformation.Factor(tensor), TensorFactory.Parse("(1 + a**3 + 2*a**3*b + a**3*b**2)/(a**2*(1 + b)**2)")).ShouldBeTrue();
    }

    [Fact]
    public void Test17()
    {
        TensorType tensor = TensorFactory.Parse("(a+a*b)**(-1) + 1/a");
        TensorUtils.Equals(FactorTransformation.Factor(tensor), TensorFactory.Parse("(2+b)/(a*(1 + b))")).ShouldBeTrue();
    }

    [Fact(Skip = "Ignored in original test suite (broken JAS).")]
    public void Test19()
    {
    }

    [Fact]
    public void Test19a()
    {
        TensorType tensor = TensorFactory.Parse("a+b*a+(a**2+2*a*b+b**2)/(a+b)*F^i_i+(a**2+2*a*b+b**2)*(a+b)**(-1)*H^i_i");
        TensorUtils.Equals(
            FactorTransformation.Factor(tensor, false),
            TensorFactory.Parse("(a+b)*H^{i}_{i}+(a+b)*F^{i}_{i}+a*(1+b)"))
            .ShouldBeTrue();

        tensor = TensorFactory.Parse("(a**2+2*a*b+b**2)/(a+b)*F^i_i+(a**2+2*a*b+b**2)*(a+b)**(-1)*H^i_i");
        TensorUtils.Equals(
            FactorTransformation.Factor(tensor, false),
            TensorFactory.Parse("(a+b)*H^{i}_{i}+(a+b)*F^{i}_{i}"))
            .ShouldBeTrue();
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
        TensorUtils.Equals(
            FactorTransformation.Factor(TensorFactory.Parse("2*I*a + 4*I*b")),
            TensorFactory.Parse("2*I*(a + 2*b)"))
            .ShouldBeTrue();

        TensorUtils.Equals(
            FactorTransformation.Factor(TensorFactory.Parse("2*I*a + 4*(I*b + I*c)")),
            TensorFactory.Parse("2*I*(a + 2*b + 2*c)"))
            .ShouldBeTrue();

        TensorUtils.Equals(
            FactorTransformation.Factor(TensorFactory.Parse("2*I*a + 4*(-I*b + I*c)")),
            TensorFactory.Parse("2*I*(a - 2*b + 2*c)"))
            .ShouldBeTrue();

        TensorUtils.Equals(
            FactorTransformation.Factor(TensorFactory.Parse("2*I*(I*a + I*d) + 4*I*(-I*b + I*c)")),
            TensorFactory.Parse("2*(-a -d + 2*b - 2*c)"))
            .ShouldBeTrue();
    }

    [Fact]
    public void Test22()
    {
        TensorType tensor = TensorFactory.Parse("I*(a+b) - I*a - I*b");
        TensorUtils.Equals(FactorTransformation.Factor(tensor), Complex.Zero).ShouldBeTrue();
    }

    [Fact]
    public void Test23()
    {
        TensorType tensor = TensorFactory.Parse("I*(a+b) - I*a - I*b + c*I");
        TensorUtils.Equals(FactorTransformation.Factor(tensor), TensorFactory.Parse("c*I")).ShouldBeTrue();
    }

    [Fact]
    public void Test24()
    {
        TensorType tensor = TensorFactory.Parse("a**(-1)*b-(b*rc3+a)**(-1)*b-(b*rc3+a)**(-1)*b**2*a**(-1)*rc3");
        TensorUtils.Equals(FactorTransformation.Factor(tensor), TensorFactory.Parse("0")).ShouldBeTrue();
    }

    [Fact]
    public void Test25()
    {
        TensorType tensor = TensorFactory.Parse("I*a + f");
        TensorUtils.Equals(FactorTransformation.Factor(tensor), TensorFactory.Parse("I*a + f")).ShouldBeTrue();
    }
}
