using NRedberry.Indices;
using NRedberry.Numbers;
using NRedberry.Physics.Feyncalc;
using NRedberry.Tensors;
using NRedberry.Transformations.Expand;
using NRedberry.Transformations.Symmetrization;
using Shouldly;
using TensorCC = NRedberry.Tensors.CC;
using TensorFactory = NRedberry.Tensors.Tensors;
using RandomTensorGenerator = NRedberry.Tensors.Random.RandomTensor;
using Xunit;
using Xunit.Abstractions;

namespace NRedberry.Physics.Tests.Feyncalc;

public sealed class LeviCivitaSimplifyTransformationTest(ITestOutputHelper testOutputHelper)
{
    [Fact]
    public void ShouldThrowUntilLeviCivitaSimplifyTransformationIsPorted()
    {
        Should.Throw<NotImplementedException>(() => new LeviCivitaSimplifyTransformation(
            TensorFactory.ParseSimple("e_abcd"),
            true));
    }

    [Fact]
    public void Test1()
    {
        Reset();
        SimpleTensor eps = TensorFactory.ParseSimple("e_abcd");

        var t = TensorFactory.Parse("e_abcd*k^a*k^b");
        ShouldMatchTensor(Complex.Zero, SimplifyLeviCivita(t, eps));

        t = TensorFactory.Parse("e_abcd*k^ac*k^be");
        testOutputHelper.WriteLine(SimplifyLeviCivita(t, eps).ToString());
        ShouldMatchTensor(t, SimplifyLeviCivita(t, eps));

        t = TensorFactory.Parse("e_abed*k^ac*k^b_c");
        testOutputHelper.WriteLine(SimplifyLeviCivita(t, eps).ToString());
        ShouldMatchTensor(Complex.Zero, SimplifyLeviCivita(t, eps));

        t = TensorFactory.Parse("e_abed*g^ed");
        testOutputHelper.WriteLine(SimplifyLeviCivita(t, eps).ToString());
        ShouldMatchTensor(Complex.Zero, SimplifyLeviCivita(t, eps));

        t = TensorFactory.Parse("e_abed*e^abpq*g^ed");
        testOutputHelper.WriteLine(SimplifyLeviCivita(t, eps).ToString());
        ShouldMatchTensor(Complex.Zero, SimplifyLeviCivita(t, eps));

        t = TensorFactory.Parse("e_abed*e^abpq*(g^ek*g^dl+g^el*g^dk)");
        ShouldMatchTensor(Complex.Zero, SimplifyLeviCivita(t, eps));
    }

    [Fact]
    public void Test2()
    {
        Reset();
        SimpleTensor eps = TensorFactory.ParseSimple("e_ab");

        var t = TensorFactory.Parse("e_ed*e^pq");
        ShouldMatchTensor("d^{p}_{d}*d^{q}_{e}-d^{q}_{d}*d^{p}_{e}", SimplifyLeviCivita(t, eps));

        t = TensorFactory.Parse("e_ed*e^eq");
        ShouldMatchTensor("-d_{d}^{q}", SimplifyLeviCivita(t, eps));
    }

    [Fact]
    public void Test3()
    {
        Reset();
        SimpleTensor eps = TensorFactory.ParseSimple("e_abc");

        var t = TensorFactory.Parse("e_abc*e^abd");
        ShouldMatchTensor("2*d^d_c", SimplifyLeviCivita(t, eps));
        t = TensorFactory.Parse("e_abc*e^abc");
        ShouldMatchTensor("6", SimplifyLeviCivita(t, eps));
    }

    [Fact]
    public void Test4()
    {
        Reset();
        SimpleTensor eps = TensorFactory.ParseSimple("e_abcf");
        AddAntiSymmetry("e_abcd", 1, 0, 2, 3);
        AddAntiSymmetry("e_abcd", 1, 2, 3, 0);
        var t = TensorFactory.Parse("e_abcx");
        ShouldMatchTensor(t, SimplifyLeviCivita(t, eps));

        t = TensorFactory.Parse("e_abcx*e^abcy");
        ShouldMatchTensor("-6*d^y_x", SimplifyLeviCivita(t, eps));
        t = TensorFactory.Parse("e_abcx*e^acby");
        ShouldMatchTensor("6*d^y_x", SimplifyLeviCivita(t, eps));
        t = TensorFactory.Parse("e_abcx*e^acby");
        ShouldMatchTensor("6*d^y_x", SimplifyLeviCivita(t, eps));
        t = TensorFactory.Parse("e_abcd*e^abcd");
        ShouldMatchTensor("-24", SimplifyLeviCivita(t, eps));
        t = TensorFactory.Parse("e_abce*e^pqrs*e_rs^ce");
        ShouldMatchTensor("-4*e_{ab}^{pq}", SimplifyLeviCivita(t, eps));
        t = TensorFactory.Parse("-4*I*e^{dh}_{b}^{f}*e_{g}^{b}_{ah}*e_{cdef}");
        ShouldMatchTensor("16*I*e_aceg", SimplifyLeviCivita(t, eps));
        t = TensorFactory.Parse("(4*I)*e^{h}_{d}^{fb}*e_{abch}*e_{e}^{d}_{gf}");
        ShouldMatchTensor("16*I*e_aceg", SimplifyLeviCivita(t, eps));

        t = TensorFactory.Parse("(4*I)*e^{h}_{d}^{fb}*e_{abch}*e_{e}^{d}_{gf}+g_mn*e^mn_ac*g_eg");
        ShouldMatchTensor("16*I*e_aceg", SimplifyLeviCivita(t, eps));
    }

    [Fact]
    public void Test4A()
    {
        Reset();
        SimpleTensor eps = TensorFactory.ParseSimple("e_abcf");
        AddAntiSymmetry("e_abcd", 1, 0, 2, 3);
        AddAntiSymmetry("e_abcd", 1, 2, 3, 0);

        var t = TensorFactory.Parse("-4*I*e^{dh}_{b}^{f}*e_{g}^{b}_{ah}*e_{cdef}");
        ShouldMatchTensor("16*I*e_aceg", SimplifyLeviCivita(t, eps));
    }

    [Fact]
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
        ShouldHaveValue(10, t.Size);
    }

    [Fact]
    public void Test6()
    {
        Reset();
        SetAntiSymmetric("e_abcd");
        SimpleTensor eps = TensorFactory.ParseSimple("e_abcf");
        Tensor t = TensorFactory.Parse("e_abcd*e^b_n^a_m*e^m_e^n_f");
        t = SimplifyLeviCivita(t, eps);
        ShouldMatchTensor("-4*e_{cdef}", t);
    }

    [Fact]
    public void Test7()
    {
        Reset();
        SetAntiSymmetric("e_abcd");
        SimpleTensor eps = TensorFactory.ParseSimple("e_abcf");
        Tensor t = TensorFactory.Parse("e_abcd*e_k^c_mn*e^dam_s*e^n_x^bk");
        t = SimplifyLeviCivita(t, eps);
        ShouldMatchTensor("12*g_sx", t);
    }

    [Fact]
    public void Test8()
    {
        Reset();
        SetAntiSymmetric("e_abcd");
        SimpleTensor eps = TensorFactory.ParseSimple("e_abcf");
        Tensor t = TensorFactory.Parse("e_abcd*e_k^c_mn*e^dam_s*e^n_x^bk");
        t = SimplifyLeviCivita(t, eps);
        ShouldMatchTensor("12*g_sx", t);
    }

    [Fact(Skip = "LeviCivitaSimplifyTransformation is not yet ported.")]
    public void Test9()
    {
        Reset();
        SetAntiSymmetric("e_abcd");
        SetAntiSymmetric("D_ac");
        SetAntiSymmetric("B_abc");
        SetAntiSymmetric("A_abc");

        RandomTensorGenerator random = new(false);
        random.AddToNamespace(
            TensorFactory.Parse("F_a"),
            TensorFactory.Parse("A_ab"),
            TensorFactory.Parse("B_abc"),
            TensorFactory.Parse("D_ac"),
            TensorFactory.Parse("g_ac"),
            TensorFactory.Parse("e_abcd"));

        Tensor t1 = random.NextSum(20, 8, IndicesFactory.EmptyIndices);
        Tensor t2 = random.NextSum(20, 8, IndicesFactory.EmptyIndices);
        ITransformation transformation = new TransformationCollection(
            EliminateMetricsTransformation.Instance,
            TensorFactory.ParseExpression("A_ab*B^bac = T^c"),
            TensorFactory.ParseExpression("A_ab*A^ba = xx"),
            TensorFactory.ParseExpression("D_ab*D^ba = yy"),
            EliminateDueSymmetriesTransformation.Instance,
            new LeviCivitaSimplifyTransformation(TensorFactory.ParseSimple("e_abcd"), true),
            ExpandAndEliminateTransformation.Instance,
            TensorFactory.ParseExpression("A_ab*B^bac = T^c"),
            TensorFactory.ParseExpression("A_ab*A^ba = xx"),
            TensorFactory.ParseExpression("D_ab*D^ba = yy"));

        _ = new ExpandTransformation(transformation)
            .Transform(Tensor.MultiplyAndRenameConflictingDummies([t1, t2]));
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

    private static void ShouldMatchTensor(string expected, Tensor actual)
    {
        ShouldMatchTensor(TensorFactory.Parse(expected), actual);
    }

    private static void ShouldMatchTensor(Tensor expected, Tensor actual)
    {
        TensorUtils.Equals(expected, actual).ShouldBeTrue();
    }

    private static void ShouldHaveValue(int expected, int actual)
    {
        actual.ShouldBe(expected);
    }
}
