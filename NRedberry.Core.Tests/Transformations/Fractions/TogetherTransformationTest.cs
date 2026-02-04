using NRedberry.Contexts;
using NRedberry.Tensors;
using NRedberry.Transformations.Expand;
using NRedberry.Transformations.Fractions;
using TensorCC = NRedberry.Tensors.CC;
using TensorFactory = NRedberry.Tensors.Tensors;
using TensorType = NRedberry.Tensors.Tensor;
using Xunit;

namespace NRedberry.Core.Tests.Transformations.Fractions;

public sealed class TogetherTransformationTest
{
    [Fact]
    public void ShouldTogetherProduct1()
    {
        TensorType actual = TensorFactory.Parse("(a+b)*(a+b)");
        actual = TogetherTransformation.Together(actual);
        TensorType expected = TensorFactory.Parse("(a+b)**2");

        Assert.True(TensorUtils.EqualsExactly(actual, expected));
    }

    [Fact]
    public void ShouldTogetherProduct2()
    {
        TensorType actual = TensorFactory.Parse("(a_m^m+b_n^n)*(a_m^m+b_a^a)");
        actual = TogetherTransformation.Together(actual);
        TensorType expected = TensorFactory.Parse("(a_i^i+b_i^i)**2");

        Assert.True(TensorUtils.Equals(actual, expected));
    }

    [Fact]
    public void ShouldTogetherProduct3()
    {
        TensorType actual = TensorFactory.Parse("k_i*k^i*k_j*k^j");
        actual = TogetherTransformation.Together(actual);
        TensorType expected = TensorFactory.Parse("(k_i*k^i)**2");

        Assert.True(TensorUtils.Equals(actual, expected));
    }

    [Fact]
    public void ShouldTogetherSimpleFractions()
    {
        TensorType actual = TensorFactory.Parse("1/a+1/b");
        actual = TogetherTransformation.Together(actual);
        TensorType expected = TensorFactory.Parse("(a+b)/(a*b)");

        Assert.True(TensorUtils.Equals(actual, expected));
    }

    [Fact]
    public void ShouldTogetherSimplePowers()
    {
        TensorType actual = TensorFactory.Parse("1/a**2+1/a");
        actual = TogetherTransformation.Together(actual);
        TensorType expected = TensorFactory.Parse("(a+1)/(a**2)");

        Assert.True(TensorUtils.Equals(actual, expected));
    }

    [Fact(Skip = "Large expression port pending; original Java test is very heavy.")]
    public void ShouldTogetherLargeExpression()
    {
        // TODO: Port test3 from Java once the large expression is validated in C#.
    }

    [Fact]
    public void ShouldReduceFractionExpression()
    {
        TensorType actual = TensorFactory.Parse("(a*b+b*c+c*a)/(1/a+1/b+1/c)");
        actual = TogetherTransformation.Together(actual);
        TensorType expected = TensorFactory.Parse("a*b*c");

        Assert.True(TensorUtils.Equals(actual, expected));
    }

    [Fact(Skip = "Large expression port pending; original Java test is very heavy.")]
    public void ShouldTogetherVeryLargeExpression()
    {
        // TODO: Port test5 from Java once the large expression is validated in C#.
    }

    [Fact(Skip = "Large expression port pending; original Java test is very heavy.")]
    public void ShouldTogetherEvenLargerExpression()
    {
        // TODO: Port test6 from Java once the large expression is validated in C#.
    }

    [Fact]
    public void ShouldTogetherLinearTerms()
    {
        TensorType tensor = TensorFactory.Parse("k_i/(k_i*k^i-m**2)+p_i/(k_i*k^i-m**2)");
        TensorType expected = TensorFactory.Parse("(p_i+k_i)/(k_i*k^i-m**2)");

        Assert.True(TensorUtils.Equals(expected, TogetherTransformation.Together(tensor)));
    }

    [Fact]
    public void ShouldTogetherCommonDenominator()
    {
        TensorType tensor = TensorFactory.Parse("k_i*k_j/(k_i*k^i-m**2)+p_i*k_j/(k_i*k^i-m**2)");
        TensorType expected = TensorFactory.Parse("(p_i*k_j+k_i*k_j)/(k_i*k^i-m**2)");

        Assert.True(TensorUtils.Equals(expected, TogetherTransformation.Together(tensor)));
    }

    [Fact]
    public void ShouldPreserveIndicesAfterExpansion()
    {
        TensorCC.ResetTensorNames(unchecked((int)-1920349242311093308L));
        TensorType tensor = TensorFactory.Parse("k_a*k_b/(k_i*k^i-m**2)+k_a*k_b/(k_a*k^a-m**2)+p_a*k_b/(k_a*k^a-m**2)**3");
        tensor = TogetherTransformation.Together(tensor);
        tensor = ExpandTransformation.Expand(tensor);

        TensorUtils.AssertIndicesConsistency(tensor);
        TensorCC.SetDefaultOutputFormat(OutputFormat.LaTeX);
    }

    [Fact]
    public void ShouldTogetherMixedProducts()
    {
        TensorType tensor = TensorFactory.Parse("1/(a*b)+1/c");
        tensor = TogetherTransformation.Together(tensor);

        Assert.True(TensorUtils.Equals(tensor, TensorFactory.Parse("(a*b+c)/(a*b*c)")));
    }

    [Fact]
    public void ShouldTogetherFractionalPowers()
    {
        TensorType tensor = TensorFactory.Parse("1/a**(1/2)+1/c**(1/2)");
        tensor = TogetherTransformation.Together(tensor);

        Assert.True(TensorUtils.Equals(tensor, TensorFactory.Parse("c**(-1/2)*(c**(1/2)+a**(1/2))*a**(-1/2)")));
    }

    [Fact]
    public void ShouldTogetherWithTensorFactor()
    {
        TensorType tensor = TensorFactory.Parse("(1/a**(1/2)+1/c**(1/2))*f_mn");
        tensor = TogetherTransformation.Together(tensor);

        Assert.True(TensorUtils.Equals(tensor, TensorFactory.Parse("c**(-1/2)*(c**(1/2)+a**(1/2))*a**(-1/2)*f_mn")));
    }

    [Fact]
    public void ShouldTogetherWithAdditionalSummand()
    {
        TensorType tensor = TensorFactory.Parse("(1/a**(1/2)+1/c**(1/2))*f_mn + f_mn");
        tensor = TogetherTransformation.Together(tensor);

        Assert.True(TensorUtils.Equals(tensor, TensorFactory.Parse("c**(-1/2)*(c**(1/2)+a**(1/2)+a**(1/2)*c**(1/2))*a**(-1/2)*f_mn")));
    }

    [Fact]
    public void ShouldKeepIndicesConsistent()
    {
        TensorType tensor = TensorFactory.Parse("(T_a^a + B_b^b)/(f_m^m + y_b^b) + (T_b^b + B_m^m)/(g_a^a + x)");
        tensor = TogetherTransformation.Together(tensor);

        TensorUtils.AssertIndicesConsistency(tensor);
    }

    [Fact]
    public void ShouldRemainStableAcrossContexts()
    {
        for (int i = 0; i < 50; ++i)
        {
            TensorCC.ResetTensorNames();
            TensorType tensor = TensorFactory.Parse("(1 + (g_a^a + 2)*f_b^b)/f_a^a + 2*(x_a^a + y_a^a)");
            tensor = TogetherTransformation.Together(tensor);

            TensorUtils.AssertIndicesConsistency(tensor);
        }
    }

    [Fact]
    public void ShouldExpandAllAfterTogether()
    {
        TensorType tensor = TensorFactory.Parse("A_c^c/(A_a^a + A_a^b*B^a_b)**2 + A_a^a/(B_c^c + A_c^b*B^c_b)/F_d^d**2 + (C_a^a + F_d^d)/(B_c^c + A_c^b*B^c_b)**3");
        tensor = TogetherTransformation.Together(tensor);
        TensorUtils.AssertIndicesConsistency(tensor);
        tensor = ExpandAllTransformation.Expand(tensor);

        TensorUtils.AssertIndicesConsistency(tensor);
    }

    [Fact(Skip = "Large expression port pending; original Java test is very heavy.")]
    public void ShouldKeepIndicesConsistentForHugeExpression()
    {
        // TODO: Port test17 from Java once the large expression is validated in C#.
    }
}
