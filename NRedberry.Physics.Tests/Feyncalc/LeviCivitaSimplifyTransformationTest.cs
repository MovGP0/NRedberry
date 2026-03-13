using System;
using NRedberry.Indices;
using NRedberry.Numbers;
using NRedberry.Physics.Feyncalc;
using NRedberry.Tensors;
using NRedberry.Transformations;
using NRedberry.Transformations.Expand;
using NRedberry.Transformations.Symmetrization;
using TensorCC = NRedberry.Tensors.CC;
using TensorFactory = NRedberry.Tensors.Tensors;
using Xunit;

namespace NRedberry.Physics.Tests.Feyncalc;

public sealed class LeviCivitaSimplifyTransformationTest
{
    [Fact]
    public void ShouldThrowUntilLeviCivitaSimplifyTransformationIsPorted()
    {
        Assert.Throws<NotImplementedException>(() => new LeviCivitaSimplifyTransformation(
            TensorFactory.ParseSimple("e_abcd"),
            true));
    }

    public void Test1()
    {
        Reset();
        SimpleTensor eps = TensorFactory.ParseSimple("e_abcd");
        Tensor t;

        t = TensorFactory.Parse("e_abcd*k^a*k^b");
        AssertEquals(Complex.Zero, SimplifyLeviCivita(t, eps));

        t = TensorFactory.Parse("e_abcd*k^ac*k^be");
        Console.WriteLine(SimplifyLeviCivita(t, eps));
        AssertEquals(t, SimplifyLeviCivita(t, eps));

        t = TensorFactory.Parse("e_abed*k^ac*k^b_c");
        Console.WriteLine(SimplifyLeviCivita(t, eps));
        AssertEquals(Complex.Zero, SimplifyLeviCivita(t, eps));

        t = TensorFactory.Parse("e_abed*g^ed");
        Console.WriteLine(SimplifyLeviCivita(t, eps));
        AssertEquals(Complex.Zero, SimplifyLeviCivita(t, eps));

        t = TensorFactory.Parse("e_abed*e^abpq*g^ed");
        Console.WriteLine(SimplifyLeviCivita(t, eps));
        AssertEquals(Complex.Zero, SimplifyLeviCivita(t, eps));

        t = TensorFactory.Parse("e_abed*e^abpq*(g^ek*g^dl+g^el*g^dk)");
        AssertEquals(Complex.Zero, SimplifyLeviCivita(t, eps));
    }

    public void Test2()
    {
        Reset();
        SimpleTensor eps = TensorFactory.ParseSimple("e_ab");
        Tensor t;

        t = TensorFactory.Parse("e_ed*e^pq");
        AssertEquals("d^{p}_{d}*d^{q}_{e}-d^{q}_{d}*d^{p}_{e}", SimplifyLeviCivita(t, eps));

        t = TensorFactory.Parse("e_ed*e^eq");
        AssertEquals("-d_{d}^{q}", SimplifyLeviCivita(t, eps));
    }

    public void Test3()
    {
        Reset();
        SimpleTensor eps = TensorFactory.ParseSimple("e_abc");
        Tensor t;

        t = TensorFactory.Parse("e_abc*e^abd");
        AssertEquals("2*d^d_c", SimplifyLeviCivita(t, eps));
        t = TensorFactory.Parse("e_abc*e^abc");
        AssertEquals("6", SimplifyLeviCivita(t, eps));
    }

    public void Test4()
    {
        Reset();
        SimpleTensor eps = TensorFactory.ParseSimple("e_abcf");
        AddAntiSymmetry("e_abcd", 1, 0, 2, 3);
        AddAntiSymmetry("e_abcd", 1, 2, 3, 0);
        Tensor t;
        t = TensorFactory.Parse("e_abcx");
        AssertEquals(t, SimplifyLeviCivita(t, eps));

        t = TensorFactory.Parse("e_abcx*e^abcy");
        AssertEquals("-6*d^y_x", SimplifyLeviCivita(t, eps));
        t = TensorFactory.Parse("e_abcx*e^acby");
        AssertEquals("6*d^y_x", SimplifyLeviCivita(t, eps));
        t = TensorFactory.Parse("e_abcx*e^acby");
        AssertEquals("6*d^y_x", SimplifyLeviCivita(t, eps));
        t = TensorFactory.Parse("e_abcd*e^abcd");
        AssertEquals("-24", SimplifyLeviCivita(t, eps));
        t = TensorFactory.Parse("e_abce*e^pqrs*e_rs^ce");
        AssertEquals("-4*e_{ab}^{pq}", SimplifyLeviCivita(t, eps));
        t = TensorFactory.Parse("-4*I*e^{dh}_{b}^{f}*e_{g}^{b}_{ah}*e_{cdef}");
        AssertEquals("16*I*e_aceg", SimplifyLeviCivita(t, eps));
        t = TensorFactory.Parse("(4*I)*e^{h}_{d}^{fb}*e_{abch}*e_{e}^{d}_{gf}");
        AssertEquals("16*I*e_aceg", SimplifyLeviCivita(t, eps));

        t = TensorFactory.Parse("(4*I)*e^{h}_{d}^{fb}*e_{abch}*e_{e}^{d}_{gf}+g_mn*e^mn_ac*g_eg");
        AssertEquals("16*I*e_aceg", SimplifyLeviCivita(t, eps));
    }

    public void Test4A()
    {
        Reset();
        SimpleTensor eps = TensorFactory.ParseSimple("e_abcf");
        AddAntiSymmetry("e_abcd", 1, 0, 2, 3);
        AddAntiSymmetry("e_abcd", 1, 2, 3, 0);
        Tensor t;

        t = TensorFactory.Parse("-4*I*e^{dh}_{b}^{f}*e_{g}^{b}_{ah}*e_{cdef}");
        AssertEquals("16*I*e_aceg", SimplifyLeviCivita(t, eps));
    }

    public void Test5()
    {
        Reset();
        SetAntiSymmetric("e_abcd");
        SimpleTensor eps = TensorFactory.ParseSimple("e_abcf");
        Tensor t = TensorFactory.Parse("e_{abcd} * e_{mnkl} * e_{pqrs} *k1^{a}*k1^{m}*k1^{p} *k2^{b}*k2^{n}*k2^{q} *k3^{c}*k3^{k}*k3^{r}");
        t = SimplifyLeviCivita(t, eps);
        t = TensorFactory.ParseExpression("k1_m*k1^m = 0").Transform(t);
        t = TensorFactory.ParseExpression("k2_m*k2^m = 0").Transform(t);
        t = TensorFactory.ParseExpression("k3_m*k3^m = 0").Transform(t);

        t = TensorFactory.ParseExpression("k1_m*k2^m = s/2").Transform(t);
        t = TensorFactory.ParseExpression("k1_m*k3^m = -t/2").Transform(t);
        t = TensorFactory.ParseExpression("k2_m*k3^m = -u/2").Transform(t);

        t = SimplifyLeviCivita(t, eps);
        AssertEquals(10, t.Size);
    }

    public void Test6()
    {
        Reset();
        SetAntiSymmetric("e_abcd");
        SimpleTensor eps = TensorFactory.ParseSimple("e_abcf");
        Tensor t = TensorFactory.Parse("e_abcd*e^b_n^a_m*e^m_e^n_f");
        t = SimplifyLeviCivita(t, eps);
        AssertEquals("-4*e_{cdef}", t);
    }

    public void Test7()
    {
        Reset();
        SetAntiSymmetric("e_abcd");
        SimpleTensor eps = TensorFactory.ParseSimple("e_abcf");
        Tensor t = TensorFactory.Parse("e_abcd*e_k^c_mn*e^dam_s*e^n_x^bk");
        t = SimplifyLeviCivita(t, eps);
        AssertEquals("12*g_sx", t);
    }

    public void Test8()
    {
        Reset();
        SetAntiSymmetric("e_abcd");
        SimpleTensor eps = TensorFactory.ParseSimple("e_abcf");
        Tensor t = TensorFactory.Parse("e_abcd*e_k^c_mn*e^dam_s*e^n_x^bk");
        t = SimplifyLeviCivita(t, eps);
        AssertEquals("12*g_sx", t);
    }

    public void Test9()
    {
        Reset();
        // TODO: RandomTensor is not yet ported; re-enable once available.
    }

    private static Tensor SimplifyLeviCivita(Tensor tensor, SimpleTensor eps)
    {
        return new LeviCivitaSimplifyTransformation(eps, true).Transform(tensor);
    }

    private static void AddAntiSymmetry(string tensor, params int[] permutation)
    {
        SimpleTensor simple = TensorFactory.ParseSimple(tensor);
        simple.SimpleIndices.Symmetries.AddAntiSymmetry(permutation);
    }

    private static void SetAntiSymmetric(string tensor)
    {
        SimpleTensor simple = TensorFactory.ParseSimple(tensor);
        simple.SimpleIndices.Symmetries.SetAntiSymmetric();
    }

    private static void Reset()
    {
        TensorCC.Reset();
    }

    private static void AssertEquals(string expected, Tensor actual)
    {
        AssertEquals(TensorFactory.Parse(expected), actual);
    }

    private static void AssertEquals(Tensor expected, Tensor actual)
    {
        if (!TensorUtils.Equals(expected, actual))
        {
            throw new InvalidOperationException("Tensor comparison failed.");
        }
    }

    private static void AssertEquals(int expected, int actual)
    {
        if (expected != actual)
        {
            throw new InvalidOperationException($"Expected {expected} but got {actual}.");
        }
    }
}
