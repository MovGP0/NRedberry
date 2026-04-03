using NRedberry.Tensors;
using NRedberry.Transformations.Expand;
using NRedberry.Transformations.Fractions;
using TensorCC = NRedberry.Tensors.CC;
using TensorFactory = NRedberry.Tensors.Tensors;
using TensorType = NRedberry.Tensors.Tensor;

namespace NRedberry.Core.Tests.Transformations.Fractions;

public sealed class TogetherTransformationTest
{
    [Fact]
    public void ShouldTogetherProduct1()
    {
        TensorType actual = TensorFactory.Parse("(a+b)*(a+b)");
        actual = TogetherTransformation.Together(actual);
        TensorType expected = TensorFactory.Parse("(a+b)**2");

        TensorUtils.EqualsExactly(actual, expected).ShouldBeTrue();
    }

    [Fact]
    public void ShouldTogetherProduct2()
    {
        TensorType actual = TensorFactory.Parse("(a_m^m+b_n^n)*(a_m^m+b_a^a)");
        actual = TogetherTransformation.Together(actual);
        TensorType expected = TensorFactory.Parse("(a_i^i+b_i^i)**2");

        TensorUtils.Equals(actual, expected).ShouldBeTrue();
    }

    [Fact]
    public void ShouldTogetherProduct3()
    {
        TensorType actual = TensorFactory.Parse("k_i*k^i*k_j*k^j");
        actual = TogetherTransformation.Together(actual);
        TensorType expected = TensorFactory.Parse("(k_i*k^i)**2");

        TensorUtils.Equals(actual, expected).ShouldBeTrue();
    }

    [Fact]
    public void ShouldTogetherSimpleFractions()
    {
        TensorType actual = TensorFactory.Parse("1/a+1/b");
        actual = TogetherTransformation.Together(actual);
        TensorType expected = TensorFactory.Parse("(a+b)/(a*b)");

        TensorUtils.Equals(actual, expected).ShouldBeTrue();
    }

    [Fact]
    public void ShouldTogetherSimplePowers()
    {
        TensorType actual = TensorFactory.Parse("1/a**2+1/a");
        actual = TogetherTransformation.Together(actual);
        TensorType expected = TensorFactory.Parse("(a+1)/(a**2)");

        TensorUtils.Equals(actual, expected).ShouldBeTrue();
    }

    [Fact(Skip = "Large expression port pending; original Java test is very heavy.")]
    public void ShouldTogetherLargeExpression()
    {
    }

    [Fact]
    public void ShouldReduceFractionExpression()
    {
        TensorType actual = TensorFactory.Parse("(a*b+b*c+c*a)/(1/a+1/b+1/c)");
        actual = TogetherTransformation.Together(actual);
        TensorType expected = TensorFactory.Parse("a*b*c");

        TensorUtils.Equals(actual, expected).ShouldBeTrue();
    }

    [Fact(Skip = "Large expression port pending; original Java test is very heavy.")]
    public void ShouldTogetherVeryLargeExpression()
    {
    }

    [Fact(Skip = "Large expression port pending; original Java test is very heavy.")]
    public void ShouldTogetherEvenLargerExpression()
    {
    }

    [Fact]
    public void ShouldTogetherLinearTerms()
    {
        TensorType tensor = TensorFactory.Parse("k_i/(k_i*k^i-m**2)+p_i/(k_i*k^i-m**2)");
        TensorType expected = TensorFactory.Parse("(p_i+k_i)/(k_i*k^i-m**2)");

        TensorUtils.Equals(expected, TogetherTransformation.Together(tensor)).ShouldBeTrue();
    }

    [Fact]
    public void ShouldTogetherCommonDenominator()
    {
        TensorType tensor = TensorFactory.Parse("k_i*k_j/(k_i*k^i-m**2)+p_i*k_j/(k_i*k^i-m**2)");
        TensorType expected = TensorFactory.Parse("(p_i*k_j+k_i*k_j)/(k_i*k^i-m**2)");

        TensorUtils.Equals(expected, TogetherTransformation.Together(tensor)).ShouldBeTrue();
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

        TensorUtils.Equals(tensor, TensorFactory.Parse("(a*b+c)/(a*b*c)")).ShouldBeTrue();
    }

    [Fact]
    public void ShouldTogetherFractionalPowers()
    {
        TensorType tensor = TensorFactory.Parse("1/a**(1/2)+1/c**(1/2)");
        tensor = TogetherTransformation.Together(tensor);

        TensorUtils.Equals(tensor, TensorFactory.Parse("c**(-1/2)*(c**(1/2)+a**(1/2))*a**(-1/2)")).ShouldBeTrue();
    }

    [Fact]
    public void ShouldTogetherWithTensorFactor()
    {
        TensorType tensor = TensorFactory.Parse("(1/a**(1/2)+1/c**(1/2))*f_mn");
        tensor = TogetherTransformation.Together(tensor);

        TensorUtils.Equals(tensor, TensorFactory.Parse("c**(-1/2)*(c**(1/2)+a**(1/2))*a**(-1/2)*f_mn")).ShouldBeTrue();
    }

    [Fact]
    public void ShouldTogetherWithAdditionalSummand()
    {
        TensorType tensor = TensorFactory.Parse("(1/a**(1/2)+1/c**(1/2))*f_mn + f_mn");
        tensor = TogetherTransformation.Together(tensor);

        TensorUtils.Equals(tensor, TensorFactory.Parse("c**(-1/2)*(c**(1/2)+a**(1/2)+a**(1/2)*c**(1/2))*a**(-1/2)*f_mn")).ShouldBeTrue();
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

    [Fact]
    public void ShouldKeepIndicesConsistentForHugeExpression()
    {
        TensorType tensor = TensorFactory.Parse("(8*I)*m[fl]**2*g**2*(k1_{c}*k1^{c}*k1_{d}*k1^{d}+k1_{c}*k1_{d}*p^{c}[fl]*p^{d}[fl]-2*k1_{d}*k1^{d}*k1_{c}*p^{c}[fl])**(-1)*k1^{i}*k1_{a}*(-(1/4)*m[fl]**(-2)*p_{b}[fl]*p_{i}[fl]+g_{ib})*g_{BA}+(8*I)*m[fl]**2*g**2*(k1_{c}*k1^{c}*k1_{d}*k1^{d}+k1_{c}*k1_{d}*p^{c}[fl]*p^{d}[fl]-2*k1_{d}*k1^{d}*k1_{c}*p^{c}[fl])**(-1)*k1^{i}*k1_{b}*(-(1/4)*m[fl]**(-2)*p_{a}[fl]*p_{i}[fl]+g_{ia})*g_{BA}+(I)*g**2*(k1_{c}*k1^{c}*k1_{e}*k1^{e}+k1_{c}*k1_{e}*p^{c}[fl]*p^{e}[fl]-2*k1_{e}*k1^{e}*k1_{c}*p^{c}[fl])**(-1)*k1_{a}*(-(1/4)*m[fl]**(-2)*p_{d}[fl]*p_{i}[fl]+g_{id})*g_{BA}*p^{d}[fl]*p^{i}[fl]*p_{b}[fl]+(I)*g**2*(k1_{c}*k1^{c}*k1_{e}*k1^{e}+k1_{c}*k1_{e}*p^{c}[fl]*p^{e}[fl]-2*k1_{e}*k1^{e}*k1_{c}*p^{c}[fl])**(-1)*k1_{b}*(-(1/4)*m[fl]**(-2)*p_{d}[fl]*p_{i}[fl]+g_{id})*g_{BA}*p^{d}[fl]*p^{i}[fl]*p_{a}[fl]+(4*m[fl]**2*((1/2*I)*g**2*(-k1_{c}*p^{c}[fl]+k1_{c}*k1^{c})**(-1)+(1/2*I)*(-k2^{c}*p_{c}[fl]+k2_{c}*k2^{c})**(-1)*g**2)+(-2*I)*m[fl]**2*g**2*(-k1_{c}*p^{c}[fl]+k1_{c}*k1^{c})**(-1)+(-2*I)*(-k2^{c}*p_{c}[fl]+k2_{c}*k2^{c})**(-1)*m[fl]**2*g**2)*(-(1/4)*m[fl]**(-2)*p_{i}[fl]*p^{i}[fl]+d_{i}^{i})*g_{ab}*g_{BA}+((I)*(-k2^{c}*p_{c}[fl]+k2_{c}*k2^{c})**(-1)*g**2+(3*I)*(k2_{c}*k2^{c}*k2_{d}*k2^{d}-2*k2_{d}*k2^{d}*k2^{c}*p_{c}[fl]+k2^{c}*k2^{d}*p_{c}[fl]*p_{d}[fl])**(-1)*m[fl]**2*g**2+(-I)*m[fl]**2*g**2*(k1_{c}*k1^{c}*k1_{d}*k1^{d}+k1_{c}*k1_{d}*p^{c}[fl]*p^{d}[fl]-2*k1_{d}*k1^{d}*k1_{c}*p^{c}[fl])**(-1)+4*m[fl]**2*((1/4*I)*(k2_{c}*k2^{c}*k2_{d}*k2^{d}-2*k2_{d}*k2^{d}*k2^{c}*p_{c}[fl]+k2^{c}*k2^{d}*p_{c}[fl]*p_{d}[fl])**(-1)*g**2+(1/4*I)*g**2*(k1_{c}*k1^{c}*k1_{d}*k1^{d}+k1_{c}*k1_{d}*p^{c}[fl]*p^{d}[fl]-2*k1_{d}*k1^{d}*k1_{c}*p^{c}[fl])**(-1))+(I)*g**2*(-k1_{c}*p^{c}[fl]+k1_{c}*k1^{c})**(-1))*(-(1/4)*m[fl]**(-2)*p_{a}[fl]*p_{i}[fl]+g_{ia})*g_{BA}*p^{i}[fl]*p_{b}[fl]+(2*I)*g**2*(k1_{c}*k1^{c}*k1_{e}*k1^{e}+k1_{c}*k1_{e}*p^{c}[fl]*p^{e}[fl]-2*k1_{e}*k1^{e}*k1_{c}*p^{c}[fl])**(-1)*k1^{i}*k1^{f}*g_{ab}*(-(1/4)*m[fl]**(-2)*p_{d}[fl]*p_{i}[fl]+g_{id})*g_{BA}*p_{f}[fl]*p^{d}[fl]+(4*I)*m[fl]**2*g**2*(k1_{c}*k1^{c}*k1_{e}*k1^{e}+k1_{c}*k1_{e}*p^{c}[fl]*p^{e}[fl]-2*k1_{e}*k1^{e}*k1_{c}*p^{c}[fl])**(-1)*k1^{d}*g_{ab}*(-(1/4)*m[fl]**(-2)*p_{d}[fl]*p_{i}[fl]+g_{id})*g_{BA}*p^{i}[fl]+(-2*I)*(k2_{c}*k2^{c}*k2_{e}*k2^{e}-2*k2_{e}*k2^{e}*k2^{c}*p_{c}[fl]+k2^{c}*k2^{e}*p_{c}[fl]*p_{e}[fl])**(-1)*g**2*(-(1/4)*m[fl]**(-2)*p_{d}[fl]*p_{i}[fl]+g_{id})*g_{BA}*k2^{i}*k2_{b}*p^{d}[fl]*p_{a}[fl]+(-2*I)*(k2_{c}*k2^{c}*k2_{e}*k2^{e}-2*k2_{e}*k2^{e}*k2^{c}*p_{c}[fl]+k2^{c}*k2^{e}*p_{c}[fl]*p_{e}[fl])**(-1)*g**2*(-(1/4)*m[fl]**(-2)*p_{d}[fl]*p_{i}[fl]+g_{id})*g_{BA}*k2^{i}*k2_{a}*p^{d}[fl]*p_{b}[fl]+(-2*I)*g**2*(k1_{c}*k1^{c}*k1_{e}*k1^{e}+k1_{c}*k1_{e}*p^{c}[fl]*p^{e}[fl]-2*k1_{e}*k1^{e}*k1_{c}*p^{c}[fl])**(-1)*k1^{i}*k1_{a}*(-(1/4)*m[fl]**(-2)*p_{d}[fl]*p_{i}[fl]+g_{id})*g_{BA}*p^{d}[fl]*p_{b}[fl]+(-2*I)*g**2*(k1_{c}*k1^{c}*k1_{e}*k1^{e}+k1_{c}*k1_{e}*p^{c}[fl]*p^{e}[fl]-2*k1_{e}*k1^{e}*k1_{c}*p^{c}[fl])**(-1)*k1^{i}*k1_{b}*(-(1/4)*m[fl]**(-2)*p_{d}[fl]*p_{i}[fl]+g_{id})*g_{BA}*p^{d}[fl]*p_{a}[fl]+(I)*(k2_{c}*k2^{c}*k2_{e}*k2^{e}-2*k2_{e}*k2^{e}*k2^{c}*p_{c}[fl]+k2^{c}*k2^{e}*p_{c}[fl]*p_{e}[fl])**(-1)*g**2*(-(1/4)*m[fl]**(-2)*p_{d}[fl]*p_{i}[fl]+g_{id})*g_{BA}*k2_{b}*p^{d}[fl]*p^{i}[fl]*p_{a}[fl]+(I)*(k2_{c}*k2^{c}*k2_{e}*k2^{e}-2*k2_{e}*k2^{e}*k2^{c}*p_{c}[fl]+k2^{c}*k2^{e}*p_{c}[fl]*p_{e}[fl])**(-1)*g**2*(-(1/4)*m[fl]**(-2)*p_{d}[fl]*p_{i}[fl]+g_{id})*g_{BA}*k2_{a}*p^{d}[fl]*p^{i}[fl]*p_{b}[fl]+(2*I)*(k2_{c}*k2^{c}*k2_{e}*k2^{e}-2*k2_{e}*k2^{e}*k2^{c}*p_{c}[fl]+k2^{c}*k2^{e}*p_{c}[fl]*p_{e}[fl])**(-1)*g**2*(-(1/4)*m[fl]**(-2)*p_{d}[fl]*p_{i}[fl]+g_{id})*g_{BA}*k2^{i}*p^{d}[fl]*p_{a}[fl]*p_{b}[fl]+((I)*(-k2^{c}*p_{c}[fl]+k2_{c}*k2^{c})**(-1)*g**2+(-I)*(k2_{c}*k2^{c}*k2_{e}*k2^{e}-2*k2_{e}*k2^{e}*k2^{c}*p_{c}[fl]+k2^{c}*k2^{e}*p_{c}[fl]*p_{e}[fl])**(-1)*m[fl]**2*g**2+(-I)*m[fl]**2*g**2*(k1_{c}*k1^{c}*k1_{e}*k1^{e}+k1_{c}*k1_{e}*p^{c}[fl]*p^{e}[fl]-2*k1_{e}*k1^{e}*k1_{c}*p^{c}[fl])**(-1)+4*m[fl]**2*((1/4*I)*(k2_{c}*k2^{c}*k2_{e}*k2^{e}-2*k2_{e}*k2^{e}*k2^{c}*p_{c}[fl]+k2^{c}*k2^{e}*p_{c}[fl]*p_{e}[fl])**(-1)*g**2+(1/4*I)*g**2*(k1_{c}*k1^{c}*k1_{e}*k1^{e}+k1_{c}*k1_{e}*p^{c}[fl]*p^{e}[fl]-2*k1_{e}*k1^{e}*k1_{c}*p^{c}[fl])**(-1))+(-I)*g**2*(-k1_{c}*p^{c}[fl]+k1_{c}*k1^{c})**(-1))*g_{ab}*(-(1/4)*m[fl]**(-2)*p_{d}[fl]*p_{i}[fl]+g_{id})*g_{BA}*p^{d}[fl]*p^{i}[fl]+(2*I)*(k2_{c}*k2^{c}*k2_{e}*k2^{e}-2*k2_{e}*k2^{e}*k2^{c}*p_{c}[fl]+k2^{c}*k2^{e}*p_{c}[fl]*p_{e}[fl])**(-1)*g**2*g_{ab}*(-(1/4)*m[fl]**(-2)*p_{d}[fl]*p_{i}[fl]+g_{id})*g_{BA}*k2^{i}*k2_{m}*p^{d}[fl]*p^{m}[fl]+(8*I)*(k2_{c}*k2^{c}*k2_{d}*k2^{d}-2*k2_{d}*k2^{d}*k2^{c}*p_{c}[fl]+k2^{c}*k2^{d}*p_{c}[fl]*p_{d}[fl])**(-1)*m[fl]**2*g**2*(-(1/4)*m[fl]**(-2)*p_{a}[fl]*p_{i}[fl]+g_{ia})*g_{BA}*k2^{i}*k2_{b}+(8*I)*(k2_{c}*k2^{c}*k2_{d}*k2^{d}-2*k2_{d}*k2^{d}*k2^{c}*p_{c}[fl]+k2^{c}*k2^{d}*p_{c}[fl]*p_{d}[fl])**(-1)*m[fl]**2*g**2*(-(1/4)*m[fl]**(-2)*p_{b}[fl]*p_{i}[fl]+g_{ib})*g_{BA}*k2^{i}*k2_{a}+(4*I)*(k2_{c}*k2^{c}*k2_{e}*k2^{e}-2*k2_{e}*k2^{e}*k2^{c}*p_{c}[fl]+k2^{c}*k2^{e}*p_{c}[fl]*p_{e}[fl])**(-1)*m[fl]**2*g**2*g_{ab}*(-(1/4)*m[fl]**(-2)*p_{d}[fl]*p_{i}[fl]+g_{id})*g_{BA}*k2^{d}*p^{i}[fl]+(-8*I)*(k2_{c}*k2^{c}*k2_{e}*k2^{e}-2*k2_{e}*k2^{e}*k2^{c}*p_{c}[fl]+k2^{c}*k2^{e}*p_{c}[fl]*p_{e}[fl])**(-1)*m[fl]**2*g**2*g_{ab}*(-(1/4)*m[fl]**(-2)*p_{d}[fl]*p_{i}[fl]+g_{id})*g_{BA}*k2^{d}*k2^{i}+(-8*I)*(k2_{c}*k2^{c}*k2_{d}*k2^{d}-2*k2_{d}*k2^{d}*k2^{c}*p_{c}[fl]+k2^{c}*k2^{d}*p_{c}[fl]*p_{d}[fl])**(-1)*m[fl]**2*g**2*(-(1/4)*m[fl]**(-2)*p_{a}[fl]*p_{i}[fl]+g_{ia})*g_{BA}*k2^{i}*p_{b}[fl]+(-8*I)*m[fl]**2*g**2*(k1_{c}*k1^{c}*k1_{e}*k1^{e}+k1_{c}*k1_{e}*p^{c}[fl]*p^{e}[fl]-2*k1_{e}*k1^{e}*k1_{c}*p^{c}[fl])**(-1)*k1^{d}*k1^{i}*g_{ab}*(-(1/4)*m[fl]**(-2)*p_{d}[fl]*p_{i}[fl]+g_{id})*g_{BA}+(-8*I)*m[fl]**2*g**2*(k1_{c}*k1^{c}*k1_{d}*k1^{d}+k1_{c}*k1_{d}*p^{c}[fl]*p^{d}[fl]-2*k1_{d}*k1^{d}*k1_{c}*p^{c}[fl])**(-1)*k1^{i}*(-(1/4)*m[fl]**(-2)*p_{b}[fl]*p_{i}[fl]+g_{ib})*g_{BA}*p_{a}[fl]+((-I)*g**2*(k1_{c}*k1^{c}*k1_{e}*k1^{e}+k1_{c}*k1_{e}*p^{c}[fl]*p^{e}[fl]-2*k1_{e}*k1^{e}*k1_{c}*p^{c}[fl])**(-1)+(-I)*(k2_{c}*k2^{c}*k2_{e}*k2^{e}-2*k2_{e}*k2^{e}*k2^{c}*p_{c}[fl]+k2^{c}*k2^{e}*p_{c}[fl]*p_{e}[fl])**(-1)*g**2)*(-(1/4)*m[fl]**(-2)*p_{d}[fl]*p_{i}[fl]+g_{id})*g_{BA}*p^{d}[fl]*p^{i}[fl]*p_{a}[fl]*p_{b}[fl]+(-I)*g**2*(k1_{c}*k1^{c}*k1_{e}*k1^{e}+k1_{c}*k1_{e}*p^{c}[fl]*p^{e}[fl]-2*k1_{e}*k1^{e}*k1_{c}*p^{c}[fl])**(-1)*k1^{f}*g_{ab}*(-(1/4)*m[fl]**(-2)*p_{d}[fl]*p_{i}[fl]+g_{id})*g_{BA}*p_{f}[fl]*p^{d}[fl]*p^{i}[fl]+(-4*I)*(k2_{c}*k2^{c}*k2_{d}*k2^{d}-2*k2_{d}*k2^{d}*k2^{c}*p_{c}[fl]+k2^{c}*k2^{d}*p_{c}[fl]*p_{d}[fl])**(-1)*m[fl]**2*g**2*(-(1/4)*m[fl]**(-2)*p_{a}[fl]*p_{i}[fl]+g_{ia})*g_{BA}*k2_{b}*p^{i}[fl]+(-4*I)*(k2_{c}*k2^{c}*k2_{d}*k2^{d}-2*k2_{d}*k2^{d}*k2^{c}*p_{c}[fl]+k2^{c}*k2^{d}*p_{c}[fl]*p_{d}[fl])**(-1)*m[fl]**2*g**2*(-(1/4)*m[fl]**(-2)*p_{b}[fl]*p_{i}[fl]+g_{ib})*g_{BA}*k2_{a}*p^{i}[fl]+(2*I)*g**2*(k1_{c}*k1^{c}*k1_{e}*k1^{e}+k1_{c}*k1_{e}*p^{c}[fl]*p^{e}[fl]-2*k1_{e}*k1^{e}*k1_{c}*p^{c}[fl])**(-1)*k1^{i}*(-(1/4)*m[fl]**(-2)*p_{d}[fl]*p_{i}[fl]+g_{id})*g_{BA}*p^{d}[fl]*p_{a}[fl]*p_{b}[fl]+(4*m[fl]**2*((1/2*I)*(-k2^{c}*p_{c}[fl]+k2_{c}*k2^{c})**(-1)*g**2+(-3/2*I)*g**2*(-k1_{c}*p^{c}[fl]+k1_{c}*k1^{c})**(-1))+(-4*I)*m[fl]**2*g**2*(-k1_{c}*p^{c}[fl]+k1_{c}*k1^{c})**(-1)+(-4*I)*(-k2^{c}*p_{c}[fl]+k2_{c}*k2^{c})**(-1)*m[fl]**2*g**2+4*((1/2*I)*g**2*(-k1_{c}*p^{c}[fl]+k1_{c}*k1^{c})**(-1)+(-3/2*I)*(-k2^{c}*p_{c}[fl]+k2_{c}*k2^{c})**(-1)*g**2)*m[fl]**2)*(-(1/4)*m[fl]**(-2)*p_{a}[fl]*p_{b}[fl]+g_{ba})*g_{BA}");
        TensorUtils.AssertIndicesConsistency(tensor);
        tensor = TogetherTransformation.Together(tensor);
        TensorUtils.AssertIndicesConsistency(tensor);
    }
}
