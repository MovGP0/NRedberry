using NRedberry.Tensors;
using NRedberry.Tensors.Iterators;
using NRedberry.Transformations;
using NRedberry.Transformations.Expand;
using NRedberry.Transformations.Fractions;
using NRedberry.Transformations.Symmetrization;
using TensorCC = NRedberry.Tensors.CC;
using TensorFactory = NRedberry.Tensors.Tensors;
using TensorType = NRedberry.Tensors.Tensor;
using Xunit;

namespace NRedberry.Core.Tests.Transformations.Expand;

public sealed class ExpandTest
{
    [Fact]
    public void Test0()
    {
        TensorType tensor = TensorFactory.Parse("a*c");
        TensorType actual = ExpandTransformation.Expand(tensor);
        TensorType expected = TensorFactory.Parse("a*c");
        TensorUtils.EqualsExactly(actual, expected).ShouldBeTrue();
    }

    [Fact]
    public void Test1()
    {
        TensorType tensor = TensorFactory.Parse("(a+b)*c+a*c");
        TensorType actual = ExpandTransformation.Expand(tensor);
        TensorType expected = TensorFactory.Parse("2*a*c+b*c");
        TensorUtils.EqualsExactly(actual, expected).ShouldBeTrue();
    }

    [Fact]
    public void Test2()
    {
        TensorType tensor = TensorFactory.Parse("(a+b)*c-a*c");
        TensorType actual = ExpandTransformation.Expand(tensor);
        TensorType expected = TensorFactory.Parse("b*c");
        TensorUtils.EqualsExactly(actual, expected).ShouldBeTrue();
    }

    [Fact]
    public void Test3()
    {
        TensorType tensor = TensorFactory.Parse("(a*p_i+b*p_i)*c-a*c*p_i");
        TensorType actual = ExpandTransformation.Expand(tensor);
        TensorType expected = TensorFactory.Parse("b*c*p_i");
        TensorUtils.EqualsExactly(actual, expected).ShouldBeTrue();
    }

    [Fact]
    public void Test4()
    {
        TensorType tensor = TensorFactory.Parse("(a*p_i+b*p_i)*c-a*c*k_i");
        TensorType actual = ExpandTransformation.Expand(tensor);
        TensorType expected = TensorFactory.Parse("(a*c+c*b)*p_i-a*c*k_i");
        TensorUtils.EqualsExactly(actual, expected).ShouldBeTrue();
    }

    [Fact]
    public void Test5()
    {
        TensorType actual = TensorFactory.Parse("c*(a*(c+n)+b)");
        actual = ExpandTransformation.Expand(actual);
        TensorType expected = TensorFactory.Parse("c*a*c+c*a*n+c*b");
        TensorUtils.EqualsExactly(actual, expected).ShouldBeTrue();
    }

    [Fact]
    public void Test6()
    {
        TensorType actual = TensorFactory.Parse("a*(c+b)");
        actual = ExpandTransformation.Expand(actual);
        TensorType expected = TensorFactory.Parse("a*c+a*b");
        TensorUtils.EqualsExactly(actual, expected).ShouldBeTrue();
    }

    [Fact]
    public void Test7()
    {
        TensorType actual = TensorFactory.Parse("Power[a+b,2]");
        actual = ExpandTransformation.Expand(actual);
        TensorType expected = TensorFactory.Parse("a*a+b*b+2*a*b");
        TensorUtils.EqualsExactly(actual, expected).ShouldBeTrue();
    }

    [Fact]
    public void Test8()
    {
        TensorType actual = TensorFactory.Parse("Power[a+b,30]");
        actual = ExpandTransformation.Expand(actual);
        _ = actual;
    }

    [Fact]
    public void Test10()
    {
        for (int i = 2; i < 30; ++i)
        {
            TensorType actual = TensorFactory.Pow(TensorFactory.Parse("a+b"), i);
            actual = ExpandTransformation.Expand(actual);
            actual.Size == i + 1.ShouldBeTrue();
        }
    }

    [Fact]
    public void Test11()
    {
        for (int i = 0; i < 100; ++i)
        {
            TensorCC.ResetTensorNames();
            TensorType actual = TensorFactory.Parse("Power[a_i^i+b_i^i,2]");
            actual = ExpandTransformation.Expand(actual);
            TensorType expected = TensorFactory.Parse("2*b_{i}^{i}*a_{a}^{a}+a_{i}^{i}*a_{a}^{a}+b_{i}^{i}*b_{a}^{a}");
            TensorUtils.Equals(actual, expected).ShouldBeTrue();
        }
    }

    [Fact]
    public void Test11a()
    {
        TensorType actual = TensorFactory.Parse("(a_i^i+b_i^i)**2");
        actual = ExpandTransformation.Expand(actual);
        _ = actual;
    }

    [Fact]
    public void Test12()
    {
        TensorType actual = TensorFactory.Parse("f_mn*(f^mn+r^mn)-r_ab*f^ab");
        actual = ExpandTransformation.Expand(actual);
        TensorType expected = TensorFactory.Parse("f_{mn}*f^{mn}");
        TensorUtils.EqualsExactly(actual, expected).ShouldBeTrue();
    }

    [Fact]
    public void Test13()
    {
        TensorType actual = TensorFactory.Parse("((a+b)*(c+a)-b*a)*f_mn*(f^mn+r^mn)");
        actual = ExpandTransformation.Expand(actual);
        TensorType expected = TensorFactory.Parse("(a*a+b*c+a*c)*f_mn*f^mn+(a*a+b*c+a*c)*f_mn*r^mn");
        TensorUtils.EqualsExactly(actual, expected).ShouldBeTrue();
    }

    [Fact]
    public void Test14()
    {
        TensorType actual = TensorFactory.Parse("(a+b)*f_mn*(f^mn+r^mn)-(a+b*(c+d))*r_ab*(f^ab+r^ab)");
        actual = ExpandTransformation.Expand(actual);
        AssertAllBracketsExpanded(actual);
    }

    [Fact]
    public void Test15()
    {
        for (int i = 0; i < 100; ++i)
        {
            TensorCC.ResetTensorNames();
            TensorType actual = TensorFactory.Parse("(a+b)*(a*f_m+b*g_m)*(b*f^m+a*g^m)");
            actual = ExpandTransformation.Expand(actual);
            TensorType expected = TensorFactory.Parse("(Power[a, 2]*b+a*Power[b, 2])*g_{m}*g^{m}+(Power[a, 3]+Power[a, 2]*b+a*Power[b, 2]+Power[b, 3])*f^{m}*g_{m}+(Power[a, 2]*b+a*Power[b, 2])*f_{m}*f^{m}");
            TensorUtils.Equals(actual, expected).ShouldBeTrue();
        }
    }

    [Fact]
    public void Test16()
    {
        TensorType actual = TensorFactory.Parse("((a+b)*(c+a)-a)*f_mn*(f^mn+r^mn)-((a-b)*(c-a)+a)*r_ab*(f^ab+r^ab)");
        actual = ExpandTransformation.Expand(actual);
        TensorType expected = TensorFactory.Parse("(Power[a, 2]+c*b+-1*a+c*a+b*a)*f^{mn}*f_{mn}+(-2*a+2*Power[a, 2]+2*c*b)*r^{mn}*f_{mn}+(Power[a, 2]+c*b+-1*a+-1*c*a+-1*b*a)*r^{ab}*r_{ab}");
        TensorUtils.Equals(actual, expected).ShouldBeTrue();
    }

    [Fact]
    public void Test17()
    {
        TensorType actual = ExpandTransformation.Expand(TensorFactory.Parse("((a+b)*(c+a)-a)*f_mn*(f^mn+r^mn)-((a-b)*(c-a)+a)*r_ab*(f^ab+r^ab)"));
        AssertAllBracketsExpanded(actual);
        TensorType expected = TensorFactory.Parse("(2*c*b+2*Power[a, 2]+-2*a)*r_{ab}*f^{ab}+(-1*b*a+c*b+-1*c*a+Power[a, 2]+-1*a)*r^{ab}*r_{ab}+(b*a+c*b+c*a+Power[a, 2]+-1*a)*f^{mn}*f_{mn}");
        TensorUtils.Equals(actual, expected).ShouldBeTrue();
    }

    internal static void AssertAllBracketsExpanded(TensorType tensor)
    {
        TreeTraverseIterator<NoPayload> iterator = new(tensor);
        TraverseState? state;
        while ((state = iterator.Next()) is not null)
        {
            if (state != TraverseState.Leaving)
            {
                continue;
            }

            TensorType current = iterator.Current();
            if (current is Power && current[0] is Sum)
            {
                !TensorUtils.IsNaturalNumber(current[1]).ShouldBeTrue();
            }
            else if (current is Product)
            {
                int indexlessSumCount = 0;
                int indexedSumCount = 0;
                foreach (TensorType term in current)
                {
                    if (term is Sum)
                    {
                        if (term.Indices.Size() == 0)
                        {
                            ++indexlessSumCount;
                        }
                        else
                        {
                            ++indexedSumCount;
                        }
                    }
                }

                indexedSumCount == 0 && indexlessSumCount < 2.ShouldBeTrue();
            }
        }
    }

    private sealed class NoPayload : Payload<NoPayload>
    {
        public TensorType OnLeaving(StackPosition<NoPayload> stackPosition)
        {
            return null!;
        }
    }

    [Fact]
    public void Test19()
    {
        TensorType tensor = TensorFactory.Parse("T_ij^ij*N_as^sa*K^fd_df");
        TensorType result = ExpandTransformation.Expand(tensor);
        ReferenceEquals(tensor, result).ShouldBeTrue();
    }

    [Fact]
    public void Test20()
    {
        TensorType tensor = TensorFactory.Parse("(a+b)*T_ij^ij*N_as^sa*K^fd_df+a*b*F_m^m");
        TensorType result = ExpandTransformation.Expand(tensor);
        ReferenceEquals(tensor, result).ShouldBeTrue();
    }

    [Fact]
    public void Test21()
    {
        TensorType tensor = TensorFactory.Parse("(1/2*(a+b)*f_mn+g_mn)*((a+b)*(a+b)*3*g_ij+(a+b)*h_ij)");
        AssertAllBracketsExpanded(ExpandTransformation.Expand(tensor));
    }

    [Fact]
    public void Test21a()
    {
        TensorType tensor = TensorFactory.Parse("2*(a+b)");
        TensorUtils.Equals(ExpandTransformation.Expand(tensor), TensorFactory.Parse("2*a+2*b")).ShouldBeTrue();
    }

    [Fact]
    public void Test22()
    {
        TensorType tensor = TensorFactory.Parse("((a+b)*f_mn+g_mn)*((a+b)*g_ij+h_ij)");
        AssertAllBracketsExpanded(ExpandTransformation.Expand(tensor));
    }

    [Fact(Skip = "Large regression test skipped for now.")]
    public void Test23()
    {
    }

    [Fact(Skip = "Large regression test skipped for now.")]
    public void Test24()
    {
    }

    [Fact]
    public void Test25()
    {
        for (int i = 0; i < 100; ++i)
        {
            TensorCC.ResetTensorNames();
            TensorType tensor = TensorFactory.Parse("(b+a)*(c+a)+(a+b)*(c+b)");
            TensorUtils.Equals(
                TensorFactory.Parse("2*c*a+2*b*a+a**2+2*c*b+b**2"),
                ExpandPort.ExpandUsingPort(tensor)).ShouldBeTrue();
        }
    }

    [Fact]
    public void Test26()
    {
        for (int i = 0; i < 100; ++i)
        {
            TensorCC.ResetTensorNames();
            TensorType tensor = TensorFactory.Parse("((a+b)*(c+a)-a)*f_mn*(f^mn+r^mn)-((a-b)*(c-a)+a)*r_ab*(f^ab+r^ab)");
            TensorType expected = ExpandTransformation.Expand(tensor);
            TensorType actual = ExpandPort.ExpandUsingPort(tensor);
            TensorUtils.Equals(expected, actual).ShouldBeTrue();
        }
    }

    [Fact]
    public void Test27()
    {
        for (int i = 0; i < 100; ++i)
        {
            TensorCC.ResetTensorNames();
            TensorType tensor = TensorFactory.Parse("((a+b)*(c+a)-a)*f_mn*(f^mn+r^mn)-((a-b)*(c-a)+a)*r_ab*(f^ab+r^ab)*(f_a^a+r_b^b)**5");
            TensorType expected = ExpandTransformation.Expand(tensor);
            TensorType actual = ExpandPort.ExpandUsingPort(tensor);
            AssertAllBracketsExpanded(expected);
            AssertAllBracketsExpanded(actual);
            TensorUtils.Equals(expected, actual).ShouldBeTrue();
        }
    }

    [Fact]
    public void Test28()
    {
        for (int i = 0; i < 100; ++i)
        {
            TensorCC.ResetTensorNames();
            TensorType tensor = TensorFactory.Parse("(a_i^i+b_i^i)**5");
            TensorType expected = ExpandTransformation.Expand(tensor);
            TensorUtils.AssertIndicesConsistency(expected);
        }
    }

    [Fact]
    public void Test29()
    {
        for (int i = 0; i < 100; ++i)
        {
            TensorCC.ResetTensorNames();
            TensorType tensor = TensorFactory.Parse("(a+b)*(a_b^b+b_a^a)**5");
            TensorType expected = ExpandTransformation.Expand(tensor);
            TensorType actual = ExpandPort.ExpandUsingPort(tensor);
            TensorUtils.Equals(expected, actual).ShouldBeTrue();
        }
    }

    [Fact(Skip = "Large regression test skipped for now.")]
    public void Test30()
    {
    }

    [Fact]
    public void Test31()
    {
        TensorType tensor = TensorFactory.Parse("(p_{a}*k_{b}+(k^{d}*k_{d}-m**2)**2*k_{a}*k_{b})");
        tensor = ExpandTransformation.Expand(tensor);
        TensorUtils.AssertIndicesConsistency(tensor);
    }

    [Fact]
    public void Test32()
    {
        TensorType tensor = ExpandTransformation.Expand(TensorFactory.Parse("(a*b+(c*d-m**2)**2*a*b)"));
        AssertAllBracketsExpanded(tensor);
    }

    [Fact]
    public void Test33()
    {
        TensorCC.ResetTensorNames(unchecked((int)-1920349242311093308L));
        _ = TensorFactory.Parse("k_a*k_b/(k_a*k^a-m**2)+p_a*k_b/(k_a*k^a-m**2)**3");
        TensorType tensor = TensorFactory.Parse("(-m**2+k_{d}*k^{d})**2*k_{a}*k_{b}+p_{a}*k_{b}");
        tensor = ExpandTransformation.Expand(tensor);
        TensorUtils.AssertIndicesConsistency(tensor);
    }

    [Fact(Skip = "TogetherTransformation depends on CollectScalarFactors which is not yet ported.")]
    public void Test34()
    {
    }

    [Fact]
    public void Test35()
    {
        TensorType t1 = ExpandTransformation.Expand(TensorFactory.Parse("a_a^a*F_mn+(b_m^m-a_m^m)*F_mn"));
        TensorType t2 = ExpandPort.ExpandUsingPort(t1);
        TensorUtils.Equals(t1, t2).ShouldBeTrue();
    }

    [Fact]
    public void Test36()
    {
        TensorType t1 = ExpandTransformation.Expand(TensorFactory.Parse("(A_abcd+B_abcd)*(A^ab + F^ab*(A_e^e+B_e^e)**2)"));
        _ = t1;
    }

    [Fact]
    public void Test37()
    {
        TensorType tensor = TensorFactory.Parse("Sin[(a+b)*(c+d)]");
        ReferenceEquals(tensor, ExpandTransformation.Expand(tensor)).ShouldBeTrue();
    }

    [Fact]
    public void Test38()
    {
        TensorType tensor = TensorFactory.Parse("1/(a+b)**2 + Sin[(a+b)*(c+d)]");
        ReferenceEquals(tensor, ExpandTransformation.Expand(tensor)).ShouldBeTrue();
    }

    [Fact]
    public void Test39()
    {
        TensorType tensor = TensorFactory.Parse("(a+b)*g_mn");
        ReferenceEquals(tensor, ExpandTransformation.Expand(tensor)).ShouldBeTrue();

        tensor = TensorFactory.Parse("(a+b)*g_mn*f^ab");
        ReferenceEquals(tensor, ExpandTransformation.Expand(tensor)).ShouldBeTrue();

        tensor = TensorFactory.Parse("(a+b*c)*g_mn*f^ab");
        ReferenceEquals(tensor, ExpandTransformation.Expand(tensor)).ShouldBeTrue();

        tensor = TensorFactory.Parse("a+b*c");
        ReferenceEquals(tensor, ExpandTransformation.Expand(tensor)).ShouldBeTrue();

        tensor = TensorFactory.Parse("a_i^i+b*c");
        ReferenceEquals(tensor, ExpandTransformation.Expand(tensor)).ShouldBeTrue();

        tensor = TensorFactory.Parse("a_ij+b*c_ij");
        ReferenceEquals(tensor, ExpandTransformation.Expand(tensor)).ShouldBeTrue();

        tensor = TensorFactory.Parse("a_ij+(b+a)*c_ij");
        ReferenceEquals(tensor, ExpandTransformation.Expand(tensor)).ShouldBeTrue();

        tensor = TensorFactory.Parse("(c+d)*a_ij+(b+a)*c_ij");
        ReferenceEquals(tensor, ExpandTransformation.Expand(tensor)).ShouldBeTrue();

        tensor = TensorFactory.Parse("(c+d_f^f)*a_ij+(b+a)*c_ij");
        !ReferenceEquals(tensor, ExpandTransformation.Expand(tensor)).ShouldBeTrue();
    }

    [Fact]
    public void Test40()
    {
        TensorType tensor = TensorFactory.Parse("((a+b)*f_mn+(c+d*(a+b))*l_mn)*(a+b)");
        TensorType expanded = ExpandTransformation.Expand(tensor);
        TensorUtils.Equals(expanded, TensorFactory.Parse("(a**2+2*a*b+b**2)*f_{mn}+(c*a+2*a*d*b+c*b+a**2*d+b**2*d)*l_{mn}")).ShouldBeTrue();
        AssertAllBracketsExpanded(ExpandTransformation.Expand(tensor));
    }

    [Fact]
    public void TestExpandPortWithPower1()
    {
        TensorType tensor = TensorFactory.Parse("1/(x+y)**2 + a*(c + d)");
        TensorType expected = TensorFactory.Parse("1/(x+y)**2 + a*c + a*d");
        TensorType actual = ExpandPort.ExpandUsingPort(tensor);
        TensorUtils.Equals(expected, actual).ShouldBeTrue();
    }

    [Fact]
    public void Test41()
    {
        TensorType tensor = TensorFactory.Parse("a*d + b*c + f");
        ReferenceEquals(tensor, ExpandTransformation.Expand(tensor)).ShouldBeTrue();
    }

    [Fact]
    public void Test42()
    {
        TensorType tensor = TensorFactory.Parse("(A_m*A^m)**2");
        ReferenceEquals(tensor, ExpandTransformation.Expand(tensor)).ShouldBeTrue();
        ReferenceEquals(tensor, ExpandPort.ExpandUsingPort(tensor)).ShouldBeTrue();
    }

    [Fact(Skip = "Depends on CollectScalarFactorsTransformation which is not yet ported.")]
    public void Test44()
    {
    }

    [Fact(Skip = "Performance test skipped.")]
    public void Test45Performance()
    {
    }

    [Fact(Skip = "RandomTensor is not yet ported.")]
    public void Test46()
    {
    }

    [Fact]
    public void Test47()
    {
        Expression e1 = TensorFactory.ParseExpression("p_a*p^a = x");
        Expression e2 = TensorFactory.ParseExpression("k_a*k^a = x");

        TensorType tensor = TensorFactory.Parse("(p_a*p^a + k_r*k^r)**2");
        TensorUtils.Equals(ExpandTransformation.Expand(tensor, e1, e2), TensorFactory.Parse("4*x**2")).ShouldBeTrue();

        tensor = TensorFactory.Parse("(p_a*p^a + k_r*k^r)**3");
        TensorUtils.Equals(ExpandTransformation.Expand(tensor, e1, e2), TensorFactory.Parse("8*x**3")).ShouldBeTrue();

        tensor = TensorFactory.Parse("(p_a*p^a + k_r*k^r)**4");
        TensorUtils.Equals(ExpandTransformation.Expand(tensor, e1, e2), TensorFactory.Parse("16*x**4")).ShouldBeTrue();
    }

    [Fact]
    public void Test48()
    {
        TensorType tensor = TensorFactory.Parse("((a-b)*f_{m}*f^{m}+a+b)*(a+b)*f_{a}*f^{a}");
        TensorType expected = TensorFactory.Parse("(-b**2+a**2)*f_{a}*f^{a}*f_{m}*f^{m}+(a**2+2*a*b+b**2)*f_{a}*f^{a}");
        TensorUtils.Equals(expected, ExpandTransformation.Expand(tensor)).ShouldBeTrue();
    }

    [Fact]
    public void Test49()
    {
        TensorType tensor = TensorFactory.Parse("(2*(c+a)-164*a)*(f_{a}+t_{a})*f^{a}");
        ITransformation[] subs =
        [
            TensorFactory.ParseExpression("f_a*f^a = a"),
            TensorFactory.ParseExpression("f_a*t^a = b"),
            TensorFactory.ParseExpression("t_a*t^a = c")
        ];
        TensorType expected = TensorFactory.Parse("-162*a*b+2*c*a+2*c*b-162*a**2");
        TensorType actual = ExpandTransformation.Expand(tensor, subs);
        TensorUtils.Equals(expected, actual).ShouldBeTrue();
    }

    [Fact]
    public void Test50()
    {
        TensorType tensor = TensorFactory.Parse("(b+a)*(c+a)+(a+b)*(c+b)");
        TensorUtils.Equals(tensor, ExpandPort.ExpandUsingPort(tensor, false)).ShouldBeTrue();
    }

    [Fact]
    public void Test51()
    {
        TensorType tensor = TensorFactory.Parse("((a+b)*(f_a + r_a) + (a + c)*t_a)*(c+r)*k^a");
        TensorType expected = TensorFactory.Parse("(c+r)*(a+b)*f_a*k^a + (c+r)*(a+b)*r_a*k^a + (c+r)*(a+c)*t_a*k^a");
        TensorUtils.Equals(expected, ExpandPort.ExpandUsingPort(tensor, false)).ShouldBeTrue();
    }

    [Fact]
    public void Test52()
    {
        for (int i = 0; i < 33; ++i)
        {
            TensorCC.Reset();
            TensorType tensor = TensorFactory.Parse("((a+b)*(c+d)*(f_a + (k+i)*t_a) + (a + c)*t_a)*(c+r)*((a+b)*f^a + (c+d)*t^a)");
            TensorType expected = TensorFactory.Parse("((c+d)**2*(c+r)*(a+b)+(a+c)*(c+r)*(a+b)+(c+d)*(c+r)*(i+k)*(a+b)**2)*t_{a}*f^{a}+(c+d)*(c+r)*(a+b)**2*f_{a}*f^{a}+((c+d)**2*(c+r)*(i+k)*(a+b)+(a+c)*(c+d)*(c+r))*t_{a}*t^{a}");
            TensorUtils.Equals(expected, ExpandPort.ExpandUsingPort(tensor, false)).ShouldBeTrue();
        }
    }
}
