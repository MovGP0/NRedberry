using NRedberry.Contexts;
using NRedberry.IndexGeneration;
using NRedberry.Indices;
using NRedberry.Tensors;
using NRedberry.Transformations.Expand;
using NRedberry.Transformations.Symmetrization;
using TensorCC = NRedberry.Tensors.CC;
using ContextType = NRedberry.Contexts.Context;
using TensorFactory = NRedberry.Tensors.Tensors;
using TensorType = NRedberry.Tensors.Tensor;
using Xunit;

namespace NRedberry.Core.Tests.Transformations;

public sealed class EliminateMetricsTransformationTest
{
    private static TensorType Contract(string tensor)
    {
        return Contract(TensorFactory.Parse(tensor));
    }

    private static TensorType Contract(TensorType tensor)
    {
        return EliminateMetricsTransformation.Eliminate(tensor);
    }

    [Fact]
    public void ShouldEliminateMetricWithSelfContraction()
    {
        TensorType actual = Contract("g_mn*A^mn");
        TensorType expected = TensorFactory.Parse("A^n_n");
        Assert.True(TensorUtils.Equals(actual, expected));
    }

    [Fact]
    public void ShouldEliminateKroneckerContraction()
    {
        TensorType actual = Contract("d^n_m*A^m_n");
        TensorType expected = TensorFactory.Parse("A^n_n");
        Assert.True(TensorUtils.Equals(actual, expected));
    }

    [Fact]
    public void ShouldEliminateKroneckerContractionWithSwappedIndices()
    {
        TensorType actual = Contract("d_m^n*A^m_n");
        TensorType expected = TensorFactory.Parse("A^n_n");
        Assert.True(TensorUtils.Equals(actual, expected));
    }

    [Fact]
    public void ShouldContractKroneckerProducts()
    {
        TensorType actual = Contract("d_m^n*d^m_n");
        TensorType expected = TensorFactory.Parse("d^n_n");
        Assert.True(TensorUtils.Equals(actual, expected));
    }

    [Fact]
    public void ShouldContractMetricProducts()
    {
        TensorType actual = Contract("g_mn*g^mn");
        TensorType expected = TensorFactory.Parse("d^n_n");
        Assert.True(TensorUtils.Equals(actual, expected));
    }

    [Fact]
    public void ShouldContractMetricProductsWithGreekIndices()
    {
        TensorType actual = Contract("g_\\mu\\nu*g^\\mu\\nu");
        TensorType expected = TensorFactory.Parse("d^\\mu_\\mu");
        Assert.True(TensorUtils.Equals(actual, expected));
    }

    [Fact]
    public void ShouldPreserveScalarFactors()
    {
        TensorType actual = Contract("2*a*g_mn*g^mn");
        TensorType expected = TensorFactory.Parse("2*a*d^n_n");
        Assert.True(TensorUtils.Equals(actual, expected));
    }

    [Fact]
    public void ShouldContractMixedIndices()
    {
        TensorType actual = Contract("B^ma*g_mn*A^nb");
        TensorType expected = TensorFactory.Parse("B^ma*A_m^b");
        Assert.True(TensorUtils.Equals(actual, expected));
    }

    [Fact]
    public void ShouldContractKroneckerInProduct()
    {
        TensorType actual = Contract("B^ma*d_m^n*A_n^b");
        TensorType expected = TensorFactory.Parse("B^ma*A_m^b");
        Assert.True(TensorUtils.Equals(actual, expected));
    }

    [Fact]
    public void ShouldContractMetricToKronecker()
    {
        TensorType actual = Contract("g^mx*g_xa");
        TensorType expected = TensorFactory.Parse("d^m_a");
        Assert.True(TensorUtils.Equals(actual, expected));
    }

    [Fact]
    public void ShouldContractKroneckerToMetric()
    {
        TensorType actual = Contract("d^m_x*g^xa");
        TensorType expected = TensorFactory.Parse("g^ma");
        Assert.True(TensorUtils.Equals(actual, expected));
    }

    [Fact]
    public void ShouldContractNestedKronecker()
    {
        TensorType actual = Contract("d^m_x*d^x_a");
        TensorType expected = TensorFactory.Parse("d^m_a");
        Assert.True(TensorUtils.Equals(actual, expected));
    }

    [Fact]
    public void ShouldContractMetricChain()
    {
        TensorType expected = TensorFactory.Parse("g_mb");
        TensorType actual = Contract("g_mn*g^na*g_ab");
        Assert.True(TensorUtils.Equals(actual, expected));

        actual = Contract("g^na*g_mn*g_ab");
        Assert.True(TensorUtils.Equals(actual, expected));

        actual = Contract("g^na*g_ab*g_mn");
        Assert.True(TensorUtils.Equals(actual, expected));

        actual = Contract("g_ab*g^na*g_mn");
        Assert.True(TensorUtils.Equals(actual, expected));
    }

    [Fact]
    public void ShouldContractMetricProductsToKronecker()
    {
        TensorType actual = Contract("g_mn*g^mn*g_ab*g^ab");
        TensorType expected = TensorFactory.Parse("d_m^m*d_a^a");
        Assert.True(TensorUtils.Equals(actual, expected));
    }

    [Fact]
    public void ShouldContractMixedMetricProducts()
    {
        TensorType actual = Contract("g_mn*g^ma*g_ab*g^bn");
        TensorType expected = TensorFactory.Parse("d_m^m");
        Assert.True(TensorUtils.Equals(actual, expected));
    }

    [Fact]
    public void ShouldContractProduct1()
    {
        TensorType actual = Contract(TensorFactory.Parse("g_mn*F^n*k"));
        TensorType expected = TensorFactory.Parse("F_m*k");
        Assert.True(TensorUtils.EqualsExactly(actual, expected));
    }

    [Fact]
    public void ShouldContractProduct2()
    {
        TensorType actual = Contract(TensorFactory.Parse("g_mn*F^n"));
        TensorType expected = TensorFactory.Parse("F_m");
        Assert.True(TensorUtils.EqualsExactly(actual, expected));
    }

    [Fact]
    public void ShouldContractProduct3()
    {
        TensorType actual = Contract(TensorFactory.Parse("g_mn*g_ab*F^n*F^m*F^ab"));
        TensorType expected = TensorFactory.Parse("F^n*F_n*F^a_a");
        Assert.True(TensorUtils.Equals(actual, expected));
    }

    [Fact]
    public void ShouldLeaveProductWithoutMetrics()
    {
        TensorType actual = Contract(TensorFactory.Parse("F^n*F^m*F^ab"));
        TensorType expected = TensorFactory.Parse("F^n*F^m*F^ab");
        Assert.True(TensorUtils.Equals(actual, expected));
    }

    [Fact]
    public void ShouldContractProduct6()
    {
        TensorType actual = Contract(TensorFactory.Parse("g^mc*g_am"));
        TensorType expected = TensorFactory.Parse("d_a^c");
        Assert.True(TensorUtils.Equals(actual, expected));
    }

    [Fact]
    public void ShouldContractProduct7()
    {
        TensorType actual = Contract(TensorFactory.Parse("g_ab*F^ab"));
        TensorType expected = TensorFactory.Parse("F^a_a");
        Assert.True(TensorUtils.Equals(actual, expected));
    }

    [Fact]
    public void ShouldContractProduct8()
    {
        TensorType actual = Contract(TensorFactory.Parse("g_ab*g^bc*(d_c^f*F_f+g_cd*g^de*X_e+g_cj*d^j_k*(X^k+X_l*g^lk))"));
        TensorType expected = TensorFactory.Parse("F_{a}+X_{a}+X_{a}+X_{a}");
        Assert.True(TensorUtils.Equals(actual, expected));
    }

    [Fact]
    public void ShouldContractProduct9()
    {
        TensorType actual = Contract(TensorFactory.Parse("g^mn*g^ab*g^gd*(p_g*g_ba+p_a*g_bg)*(p_m*g_dn+p_n*g_dm)"));
        TensorType expected = TensorFactory.Parse("(p^{d}*d^{b}_{b}+p^{d})*(p_{d}+p_{d})");
        Assert.True(TensorUtils.Equals(actual, expected));
    }

    [Fact]
    public void ShouldContractProduct10()
    {
        TensorType actual = Contract(TensorFactory.Parse("g^ab*g^gd*(p_g*g_ba+p_a*g_bg)"));
        TensorType expected = TensorFactory.Parse("p^{d}*d^{b}_{b}+p^{d}");
        Assert.True(TensorUtils.Equals(actual, expected));
    }

    [Fact]
    public void ShouldContractProduct11()
    {
        for (int i = 0; i < 100; ++i)
        {
            TensorCC.ResetTensorNames();
            TensorType actual = Contract(TensorFactory.Parse("g_bg*(p^g*g^ba*p_a+p_a*g^ab*g^gd*p_d)"));
            TensorType expected = TensorFactory.Parse("p^{a}*p_{a}+p^{d}*p_{d}");
            Assert.True(TensorUtils.Equals(actual, expected));
        }
    }

    [Fact]
    public void ShouldContractSum1()
    {
        TensorType actual = Contract(TensorFactory.Parse("g_mn*g_ab*(F^n*F^m*F^ab+F^n*F^m*F^ab)"));
        TensorType expected = TensorFactory.Parse("F^n*F_n*F^a_a+F^r*F_r*F_x^x");
        Assert.True(TensorUtils.Equals(actual, expected));
    }

    [Fact]
    public void ShouldContractSum2()
    {
        TensorType actual = Contract(TensorFactory.Parse("(F^n*F^m*F^ab+F^n*F^m*F^ab)*X_b"));
        TensorType expected = TensorFactory.Parse("(F^n*F^m*F^ab+F^n*F^m*F^ab)*X_b");
        Assert.True(TensorUtils.Equals(actual, expected));
    }

    [Fact]
    public void ShouldContractSum3()
    {
        TensorType actual = Contract(TensorFactory.Parse("g_mn*(F^m_b+g_ab*(F^am+g_xy*F^xyam))"));
        TensorType expected = TensorFactory.Parse("F_{nb}+F_{bn}+F_{y}^{y}_{bn}");
        Assert.True(TensorUtils.Equals(actual, expected));
    }

    [Fact]
    public void ShouldContractSum4()
    {
        TensorType actual = Contract(TensorFactory.Parse("g^nb*A_nb+g^nb*g_mn*(F^m_b+g_ab*(F^am+g_xy*F^xyam))"));
        TensorType expected = TensorFactory.Parse("A_n^n+F_n^n+F_n^n+F^{x}_{x}_n^n");
        Assert.True(TensorUtils.Equals(actual, expected));
    }

    [Fact]
    public void ShouldKeepUncontractableSum()
    {
        TensorType actual = Contract(TensorFactory.Parse("A_mn+g_mn*h"));
        TensorType expected = TensorFactory.Parse("A_mn+g_mn*h");
        Assert.True(TensorUtils.Equals(actual, expected));
    }

    [Fact]
    public void ShouldContractSum6()
    {
        TensorType actual = Contract(TensorFactory.Parse("g^mc*(A_mn+g_mn*h)"));
        TensorType expected = TensorFactory.Parse("A^c_n+d^c_n*h");
        Assert.True(TensorUtils.Equals(actual, expected));
    }

    [Fact]
    public void ShouldContractSum7()
    {
        TensorType actual = Contract(TensorFactory.Parse("g^ab*(g_mn*F_zxab^m+g^cd*g_mn*F_zxab^m*K_cd+g_zx*g_ab*X_n)"));
        TensorType expected = TensorFactory.Parse("F_zx^b_bn+F_zx^b_bn*K^d_d+X_n*g_zx*d^b_b");
        Assert.True(TensorUtils.Equals(actual, expected));
    }

    [Fact]
    public void ShouldContractSum8()
    {
        TensorType actual = Contract(TensorFactory.Parse("X_a+g_ab*(X^b+g^bc*(X_c+d_c^f*F_f+g_cd*g^de*X_e+g_cj*d^j_k*(X^k+X_l*g^lk)))"));
        TensorType expected = TensorFactory.Parse("X_{a}+X_{a}+X_{a}+F_{a}+X_{a}+X_{a}+X_{a}");
        Assert.True(TensorUtils.Equals(actual, expected));
    }

    [Fact]
    public void ShouldContractSum9()
    {
        TensorType actual = Contract(TensorFactory.Parse("A_mn+g_ma*B^a_n"));
        TensorType expected = TensorFactory.Parse("A_mn+B_mn");
        Assert.True(TensorUtils.EqualsExactly(actual, expected));
    }

    [Fact]
    public void ShouldContractSum10()
    {
        TensorType actual = Contract(TensorFactory.Parse("g^ad*(g_ab*X^b+X_a)"));
        TensorType expected = TensorFactory.Parse("X^{d}+X^{d}");
        Assert.True(TensorUtils.EqualsExactly(actual, expected));
    }

    [Fact]
    public void ShouldLeaveSumWithoutContractableMetrics()
    {
        TensorType tensor = TensorFactory.Parse("g^ed*(g_a*X+X_a)");
        TensorType result = Contract(tensor);
        Assert.True(ReferenceEquals(tensor, result));
    }

    [Fact]
    public void ShouldContractMetricKronecker1()
    {
        TensorType actual = Contract("g^mn*g_mn");
        TensorType expected = TensorFactory.Parse("d^n_n");
        Assert.True(TensorUtils.Equals(actual, expected));
    }

    [Fact]
    public void ShouldContractMetricKronecker2()
    {
        TensorType actual = Contract("g^ma*g_mn*g_ab*g^bc*d_c^n");
        TensorType expected = TensorFactory.Parse("d^n_n");
        Assert.True(TensorUtils.Equals(actual, expected));
    }

    [Fact]
    public void ShouldContractMetricKronecker3()
    {
        TensorType actual = Contract("d^c_a*d^a_b*d_o^b*g^ox");
        TensorType expected = TensorFactory.Parse("g^cx");
        Assert.True(TensorUtils.Equals(actual, expected));
    }

    [Fact]
    public void ShouldContractMetricKronecker4()
    {
        TensorType actual = Contract("d^c_o*g^ox");
        TensorType expected = TensorFactory.Parse("g^cx");
        Assert.True(TensorUtils.Equals(actual, expected));
    }

    [Fact]
    public void ShouldContractMetricKronecker5()
    {
        TensorType actual = Contract("p^n*d^a_d*g^db");
        TensorType expected = TensorFactory.Parse("p^n*g^ab");
        Assert.True(TensorUtils.Equals(actual, expected));
    }

    [Fact]
    public void ShouldContractMetricKronecker6()
    {
        TensorType actual = Contract("g^ab*(g_am*F_b+d_m^x*Y_xab+X_abm*g_pq*g^pq)");
        TensorType expected = TensorFactory.Parse("F_m+Y_m^b_b+X^b_bm*d_q^q");
        Assert.True(TensorUtils.Equals(actual, expected));
    }

    [Fact]
    public void ShouldContractKroneckerChain()
    {
        TensorType actual = Contract("d^m_n*d^a_b*(F^nb+d^A_B*(M^B_A*X^n*X^b+M^Bnb_A))");
        TensorType expected = TensorFactory.Parse("F^{ma}+M^{A}_{A}*X^{m}*X^{a}+M^{Ama}_{A}");
        Assert.True(TensorUtils.Equals(actual, expected));
    }

    [Fact]
    public void ShouldContractGreekIndices1()
    {
        TensorType actual = Contract("g_{\\alpha \\beta}*(F^{\\alpha}+g^{\\gamma \\alpha}*U_{\\gamma})");
        TensorType expected = TensorFactory.Parse("F_{\\beta}+U_{\\beta}");
        Assert.True(TensorUtils.Equals(actual, expected));
    }

    [Fact]
    public void ShouldContractGreekIndices2()
    {
        TensorType actual = Contract("g^{\\alpha \\beta}*(F_{\\alpha}+g_{\\gamma \\alpha}*U^{\\gamma})");
        TensorType expected = TensorFactory.Parse("F^{\\beta}+U^{\\beta}");
        Assert.True(TensorUtils.Equals(actual, expected));
    }

    [Fact]
    public void ShouldContractGreekIndices3()
    {
        TensorType actual = Contract("g^{\\alpha \\beta}*g_{\\beta \\alpha}");
        TensorType expected = TensorFactory.Parse("d^{\\alpha}_{\\alpha}");
        Assert.True(TensorUtils.Equals(actual, expected));
    }

    [Fact]
    public void ShouldContractField1()
    {
        TensorType actual = Contract("g^{mn}*F_m[x]");
        TensorType expected = TensorFactory.Parse("F^n[x]");
        Assert.True(TensorUtils.Equals(actual, expected));
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
            Assert.True(ReferenceEquals(tensor, Contract(tensor)));
        }
    }

    [Fact]
    public void ShouldContractAbstractScalarFunction2()
    {
        TensorType actual = Contract("Sin[g^am*(X_a+g_ab*(X^b+g^bc*(X_c+d_c^f*F_f+g_cd*g^de*X_e+g_cj*d^j_k*(X^k+X_l*g^lk))))*J_m]");
        TensorType expected = TensorFactory.Parse("Sin[(X_{a}+X_{a}+X_{a}+F_{a}+X_{a}+X_{a}+X_{a})*J^a]");
        Assert.True(TensorUtils.Equals(actual, expected));
    }

    [Fact]
    public void ShouldContractAbstractScalarFunction3()
    {
        TensorType actual = Contract("Sin[g^ac*(X_a+g_ab*(X^b+g^bc*(X_c+d_c^f*F_f+g_cd*g^de*X_e+g_cj*d^j_k*(X^k+X_l*g^lk))))*J_c]");
        TensorType expected = TensorFactory.Parse("Sin[(X_{a}+X_{a}+X_{a}+F_{a}+X_{a}+X_{a}+X_{a})*J^a]");
        Assert.True(TensorUtils.Equals(actual, expected));
    }

    [Fact(Skip = "Ignored in original test suite.")]
    public void ShouldSkipAbstractScalarFunction4()
    {
    }

    [Fact]
    public void ShouldContractFieldArg()
    {
        TensorType actual = Contract("F[g_mn*A^m]");
        TensorType expected = TensorFactory.Parse("F[A_n]");
        Assert.True(TensorUtils.Equals(actual, expected));
    }

    [Fact]
    public void ShouldContractWithDeltaReplacement()
    {
        TensorType tensor = Contract("1/48*(16+2*g^{\\mu \\nu }*g_{\\mu \\nu })");
        Expression d = TensorFactory.ParseExpression("d_\\mu^\\mu=4");
        tensor = d.Transform(tensor);
        Assert.True(TensorUtils.Equals(tensor, TensorFactory.Parse("1/2")));
    }

    [Fact]
    public void ShouldContractFieldDerivative()
    {
        TensorType tensor = TensorFactory.Parse("d_{f}^{c}*g~3_{bn}^{f}_{m}[x_{f}]");
        TensorType expected = TensorFactory.Parse("g~3_{bn}^{c}_{m}[x_{f}]");
        Assert.True(TensorUtils.Equals(Contract(tensor), expected));
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
}
