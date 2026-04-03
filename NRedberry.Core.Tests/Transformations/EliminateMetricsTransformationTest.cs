using NRedberry.Core.Tests.Extensions;
using NRedberry.IndexGeneration;
using NRedberry.Indices;
using NRedberry.Tensors;
using NRedberry.Transformations.Symmetrization;
using TensorCC = NRedberry.Tensors.CC;
using ContextType = NRedberry.Contexts.Context;
using TensorFactory = NRedberry.Tensors.Tensors;
using TensorType = NRedberry.Tensors.Tensor;

namespace NRedberry.Core.Tests.Transformations;

public sealed class EliminateMetricsTransformationTest
{
    [Theory]
    [InlineData("g_mn*A^mn", "A^n_n", false)]
    [InlineData("d^n_m*A^m_n", "A^n_n", false)]
    [InlineData("d_m^n*A^m_n", "A^n_n", false)]
    [InlineData("d_m^n*d^m_n", "d^n_n", false)]
    [InlineData("g_mn*g^mn", "d^n_n", false)]
    [InlineData("g_\\mu\\nu*g^\\mu\\nu", "d^\\mu_\\mu", false)]
    [InlineData("2*a*g_mn*g^mn", "2*a*d^n_n", false)]
    [InlineData("B^ma*g_mn*A^nb", "B^ma*A_m^b", false)]
    [InlineData("B^ma*d_m^n*A_n^b", "B^ma*A_m^b", false)]
    [InlineData("g^mx*g_xa", "d^m_a", false)]
    [InlineData("d^m_x*g^xa", "g^ma", false)]
    [InlineData("d^m_x*d^x_a", "d^m_a", false)]
    [InlineData("g_mn*g^mn*g_ab*g^ab", "d_b^b**2", false)]
    [InlineData("g_mn*g^ma*g_ab*g^bn", "d_m^m", false)]
    [InlineData("F^n*F^m*F^ab", "F^n*F^m*F^ab", false)]
    [InlineData("g^mc*g_am", "d_a^c", false)]
    [InlineData("g_ab*F^ab", "F^a_a", false)]
    [InlineData("g_ab*g^bc*(d_c^f*F_f+g_cd*g^de*X_e+g_cj*d^j_k*(X^k+X_l*g^lk))", "F_{a}+X_{a}+X_{a}+X_{a}", false)]
    [InlineData("g^ab*g^gd*(p_g*g_ba+p_a*g_bg)", "p^{d}*d^{b}_{b}+p^{d}", false)]
    [InlineData("g_mn*g_ab*(F^n*F^m*F^ab+F^n*F^m*F^ab)", "F^n*F_n*F^a_a+F^r*F_r*F_x^x", false)]
    [InlineData("(F^n*F^m*F^ab+F^n*F^m*F^ab)*X_b", "(F^n*F^m*F^ab+F^n*F^m*F^ab)*X_b", false)]
    [InlineData("g_mn*(F^m_b+g_ab*(F^am+g_xy*F^xyam))", "F_{nb}+F_{bn}+F_{y}^{y}_{bn}", false)]
    [InlineData("g^nb*A_nb+g^nb*g_mn*(F^m_b+g_ab*(F^am+g_xy*F^xyam))", "A_n^n+F_n^n+F_n^n+F^{x}_{x}_n^n", false)]
    [InlineData("A_mn+g_mn*h", "A_mn+g_mn*h", false)]
    [InlineData("g^mc*(A_mn+g_mn*h)", "A^c_n+d^c_n*h", false)]
    [InlineData("g^ab*(g_mn*F_zxab^m+g^cd*g_mn*F_zxab^m*K_cd+g_zx*g_ab*X_n)", "F_zx^b_bn+F_zx^b_bn*K^d_d+X_n*g_zx*d^b_b", false)]
    [InlineData("X_a+g_ab*(X^b+g^bc*(X_c+d_c^f*F_f+g_cd*g^de*X_e+g_cj*d^j_k*(X^k+X_l*g^lk)))", "X_{a}+X_{a}+X_{a}+F_{a}+X_{a}+X_{a}+X_{a}", false)]
    [InlineData("g^mn*g_mn", "d^n_n", false)]
    [InlineData("g^ma*g_mn*g_ab*g^bc*d_c^n", "d^n_n", false)]
    [InlineData("d^c_a*d^a_b*d_o^b*g^ox", "g^cx", false)]
    [InlineData("d^c_o*g^ox", "g^cx", false)]
    [InlineData("p^n*d^a_d*g^db", "p^n*g^ab", false)]
    [InlineData("g^ab*(g_am*F_b+d_m^x*Y_xab+X_abm*g_pq*g^pq)", "F_m+Y_m^b_b+X^b_bm*d_q^q", false)]
    [InlineData("d^m_n*d^a_b*(F^nb+d^A_B*(M^B_A*X^n*X^b+M^Bnb_A))", "F^{ma}+M^{A}_{A}*X^{m}*X^{a}+M^{Ama}_{A}", false)]
    [InlineData("g_{\\alpha \\beta}*(F^{\\alpha}+g^{\\gamma \\alpha}*U_{\\gamma})", "F_{\\beta}+U_{\\beta}", false)]
    [InlineData("g^{\\alpha \\beta}*(F_{\\alpha}+g_{\\gamma \\alpha}*U^{\\gamma})", "F^{\\beta}+U^{\\beta}", false)]
    [InlineData("g^{\\alpha \\beta}*g_{\\beta \\alpha}", "d^{\\alpha}_{\\alpha}", false)]
    [InlineData("g^{mn}*F_m[x]", "F^n[x]", false)]
    [InlineData("F[g_mn*A^m]", "F[A_n]", false)]
    [InlineData("d_{f}^{c}*g~3_{bn}^{f}_{m}[x_{f}]", "g~3_{bn}^{c}_{m}[x_{f}]", false)]
    [InlineData("g_mn*F^n*k", "F_m*k", true)]
    [InlineData("g_mn*F^n", "F_m", true)]
    [InlineData("g^mn*g^ab*g^gd*(p_g*g_ba+p_a*g_bg)*(p_m*g_dn+p_n*g_dm)", "2*(p_{g}+d_{b}^{b}*p_{g})*p^{g}", true)]
    [InlineData("A_mn+g_ma*B^a_n", "A_mn+B_mn", true)]
    [InlineData("g^ad*(g_ab*X^b+X_a)", "X^{d}+X^{d}", true)]
    [InlineData("g_mn*g^na*g_ab", "g_mb", false)]
    [InlineData("g^na*g_mn*g_ab", "g_mb", false)]
    [InlineData("g^na*g_ab*g_mn", "g_mb", false)]
    [InlineData("g_ab*g^na*g_mn", "g_mb", false)]
    [InlineData("Sin[g^am*(X_a+g_ab*(X^b+g^bc*(X_c+d_c^f*F_f+g_cd*g^de*X_e+g_cj*d^j_k*(X^k+X_l*g^lk))))*J_m]", "Sin[(6*X^{m}+F^{m})*J_{m}]", true)]
    [InlineData("Sin[g^ac*(X_a+g_ab*(X^b+g^bc*(X_c+d_c^f*F_f+g_cd*g^de*X_e+g_cj*d^j_k*(X^k+X_l*g^lk))))*J_c]", "Sin[(6*X^{c}+F^{c})*J_{c}]", true)]
    public void ShouldContractExpression(string input, string expected, bool exact)
    {
        if (exact)
        {
            input.ShouldContractEqualsExactly(expected);
            return;
        }

        input.ShouldContractEquals(expected);
    }

    [Fact]
    public void ShouldContractProduct11()
    {
        for (int i = 0; i < 100; ++i)
        {
            TensorCC.ResetTensorNames();
            TensorType actual = Contract(TensorFactory.Parse("g_bg*(p^g*g^ba*p_a+p_a*g^ab*g^gd*p_d)"));
            TensorType expected = TensorFactory.Parse("p^{a}*p_{a}+p^{d}*p_{d}");
            TensorUtils.Equals(actual, expected).ShouldBeTrue();
        }
    }

    [Fact]
    public void ShouldLeaveSumWithoutContractableMetrics()
    {
        TensorType tensor = TensorFactory.Parse("g^ed*(g_a*X+X_a)");
        TensorType result = Contract(tensor);
        result.ShouldBeSameAs(tensor);
    }

    [Fact(Skip = "Ignored in original test suite.")]
    public void ShouldSkipPerformanceTest1()
    {
    }

    [Fact(Skip = "Ignored in original test suite.")]
    public void ShouldSkipPerformance2()
    {
    }

    [Fact]
    public void ShouldHandlePerformance3()
    {
        TensorType tensor;
        for (int i = 0; i < 100; ++i)
        {
            tensor = GenerateNotContractedMetricSequence(1000);
            Contract(tensor).ShouldBeSameAs(tensor);
        }
    }

    [Fact(Skip = "Ignored in original test suite.")]
    public void ShouldSkipAbstractScalarFunction4()
    {
    }

    [Fact]
    public void ShouldContractWithDeltaReplacement()
    {
        TensorType tensor = Contract("1/48*(16+2*g^{\\mu \\nu }*g_{\\mu \\nu })");
        Expression d = TensorFactory.ParseExpression("d_\\mu^\\mu=4");
        tensor = d.Transform(tensor);
        TensorUtils.Equals(tensor, TensorFactory.Parse("1/2")).ShouldBeTrue();
    }

    private static TensorType GenerateContractedMetricSequence(int length)
    {
        TensorBuilder builder = new ScalarsBackedProductBuilder();
        IndexGenerator generator = new();
        const byte type = 0;
        int a = generator.Generate(type);
        int b = generator.Generate(type);
        for (int i = 0; i < length; ++i)
        {
            builder.Put(ContextType.Get().CreateMetric(a, b));
            a = IndicesUtils.InverseIndexState(b);
            b = IndicesUtils.SetRawState(IndicesUtils.GetRawStateInt(a), generator.Generate(type));
        }

        return builder.Build();
    }

    private static TensorType GenerateNotContractedMetricSequence(int length)
    {
        TensorBuilder builder = new ScalarsBackedProductBuilder();
        IndexGenerator generator = new();
        const byte type = 0;
        for (int i = 0; i < length; ++i)
        {
            builder.Put(ContextType.Get().CreateMetric(generator.Generate(type), generator.Generate(type)));
        }

        return builder.Build();
    }

    private static TensorType Contract(string tensor)
    {
        return Contract(TensorFactory.Parse(tensor));
    }

    private static TensorType Contract(TensorType tensor)
    {
        return EliminateMetricsTransformation.Eliminate(tensor);
    }
}
