using NRedberry.Physics.Feyncalc;
using NRedberry.Tensors;
using NRedberry.Transformations.Expand;
using NRedberry.Transformations.Symmetrization;
using Shouldly;
using TensorCC = NRedberry.Tensors.CC;
using TensorFactory = NRedberry.Tensors.Tensors;
using Xunit;
using Xunit.Abstractions;

namespace NRedberry.Physics.Tests.Feyncalc;

public sealed class UnitaryTraceTransformationTest(ITestOutputHelper testOutputHelper)
{
    [Fact]
    public void Test1()
    {
        Reset();
        ConfigureMatrixInsertion(TensorFactory.ParseSimple("T^a'_b'a"), IndexType.Matrix1);

        Tensor t = TensorFactory.Parse("Tr[T_a*T_b]");
        t = UnitaryTrace(t);
        testOutputHelper.WriteLine(t.ToString());
        ShouldEqualTensor("g_ab/2", t);
    }

    [Fact]
    public void Test2()
    {
        Reset();
        ConfigureMatrixInsertion(TensorFactory.ParseSimple("T^a'_b'a"), IndexType.Matrix1);

        SetSymmetric(TensorFactory.ParseSimple("d_abd"));
        SetAntiSymmetric("f_abc");
        Tensor t = TensorFactory.Parse("Tr[T_a*T_b*T_c]");
        t = UnitaryTrace(t);
        t = EliminateDueSymmetriesTransformation.Instance.Transform(t);
        Tensor expected = TensorFactory.Parse("d_abc/4+I/4*f_abc");
        ShouldEqualTensor(expected, t);
    }

    [Fact]
    public void Test3()
    {
        Reset();
        ConfigureMatrixInsertion(TensorFactory.ParseSimple("T^a'_b'a"), IndexType.Matrix1);

        SetSymmetric(TensorFactory.ParseSimple("d_abd"));
        AddAntiSymmetry(TensorFactory.ParseSimple("f_abc"), 1, 0, 2);
        AddSymmetry("f_abc", 2, 0, 1);
        Tensor t = TensorFactory.Parse("Tr[T_a*T_b*T_c*T_d]");
        Tensor t1 = TensorFactory.Parse("Tr[T_b*T_a*T_c*T_d]");

        t = UnitaryTrace(t);
        t1 = UnitaryTrace(t1);
        testOutputHelper.WriteLine(TensorFactory.Subtract(t, t1).ToString());

        t = EliminateDueSymmetriesTransformation.Instance.Transform(t);
        Tensor expected = TensorFactory.Parse("-(I/8)*f_adx*d_bc^x + (I/8)*d_adx*f_bc^x+1/8*d_ade*d_bc^e - 1/8*d_bde*d_ac^e+1/8*d_cde*d_ab^e + 1/(4*N)*g_ad*g_bc - 1/(4*N)*g_ac*g_bd + 1/(4*N)*g_ab*g_cd");
        testOutputHelper.WriteLine(t.ToString());
        testOutputHelper.WriteLine(expected.ToString());
        // ShouldEqualTensor(expected, t);
    }

    [Fact]
    public void Test4()
    {
        Reset();
        ConfigureMatrixInsertion(TensorFactory.ParseSimple("M^A'_B'\\alpha"), IndexType.Matrix2);

        SetSymmetric("e_\\alpha\\beta\\gamma");
        SetAntiSymmetric("r_\\alpha\\beta\\gamma");

        ITransformation trace = new UnitaryTraceTransformation(
            TensorFactory.ParseSimple("M_\\alpha^A'_B'"),
            TensorFactory.ParseSimple("e_\\alpha\\beta\\gamma"),
            TensorFactory.ParseSimple("r_\\alpha\\beta\\gamma"),
            TensorFactory.ParseSimple("D"));

        Tensor t = TensorFactory.Parse("Tr[M_\\alpha*M_\\beta*M_\\gamma]");
        t = trace.Transform(t);
        t = EliminateDueSymmetriesTransformation.Instance.Transform(t);

        Tensor expected = TensorFactory.Parse("r_\\alpha\\beta\\gamma/4+I/4*e_\\alpha\\beta\\gamma");
        ShouldEqualTensor(expected, t);
    }

    [Fact]
    public void Test5()
    {
        Reset();
        ConfigureMatrixInsertion(TensorFactory.ParseSimple("T^a'_b'a"), IndexType.Matrix1);

        SetSymmetric(TensorFactory.ParseSimple("d_abd"));
        SetAntiSymmetric("f_abc");

        ITransformation trace = new UnitaryTraceTransformation(
            TensorFactory.ParseSimple("T_a"),
            TensorFactory.ParseSimple("f_abc"),
            TensorFactory.ParseSimple("d_abc"),
            TensorFactory.Parse("3"));

        Tensor t = TensorFactory.Parse("g^ab*g^cr*Tr[T_a*T_b*T_c + f_abc]*Tr[T_p*T_q*T_r - 1/12*d_pqr]");
        t = trace.Transform(t);
        t = ExpandTransformation.Expand(t, EliminateMetricsTransformation.Instance);
        t = EliminateDueSymmetriesTransformation.Instance.Transform(t);
        ShouldEqualTensor("0", t);
    }

    [Fact]
    public void Test6()
    {
        Reset();
        // Ignored in Java.
        ConfigureMatrixInsertion(TensorFactory.ParseSimple("T^a'_b'a"), IndexType.Matrix1);

        SetSymmetric("d_abd");
        SetAntiSymmetric("f_abc");

        ITransformation trace = new UnitaryTraceTransformation(
            TensorFactory.ParseSimple("T_a"),
            TensorFactory.ParseSimple("f_abc"),
            TensorFactory.ParseSimple("d_abc"),
            TensorFactory.Parse("N"));

        Tensor t = TensorFactory.Parse("Tr[T_a*T_b*T_c*T^a]");
        ShouldEqualTensor("(N/4-1/4/N)*g_bc", trace.Transform(t));
        t = TensorFactory.Parse("Tr[T_a*T_b*T^a*T_c]");
        ShouldEqualTensor("-1/4/N*g_bc", trace.Transform(t));
        t = TensorFactory.Parse("Tr[T_a*T_b*T^a*T_c]");
        ShouldEqualTensor("-1/4/N*g_bc", trace.Transform(t));
        t = TensorFactory.Parse("Tr[T_a*T_b*T_c*T^a*T^b]");
        ShouldEqualTensor("0", trace.Transform(t));
        t = TensorFactory.Parse("Tr[T_a*T_b*T_c*T_d*T^a*T^c]");
        ShouldEqualTensor("g_bd/8/N**2", trace.Transform(t));
        t = TensorFactory.Parse("Tr[T_a*T_b*T_c*T_d*T^c*T^a]");
        ShouldEqualTensor("g_bd/8/N**2-g_bd/8", trace.Transform(t));
        t = TensorFactory.Parse("Tr[T_a*T_b*T_c*T_d*T^a*T^b*T^c]");
        t = EliminateDueSymmetriesTransformation.Instance.Transform(trace.Transform(t));
        testOutputHelper.WriteLine(t.ToString());
        ShouldEqualTensor("0", trace.Transform(t));
    }

    [Fact]
    public void Test6A()
    {
        Reset();
        // Ignored in Java.
        ConfigureMatrixInsertion(TensorFactory.ParseSimple("T^a'_b'a"), IndexType.Matrix1);

        SetSymmetric("d_abd");
        SetAntiSymmetric("f_abc");
        SetAntiSymmetric("e_abc");

        ITransformation trace = new UnitaryTraceTransformation(
            TensorFactory.ParseSimple("T_a"),
            TensorFactory.ParseSimple("f_abc"),
            TensorFactory.ParseSimple("d_abc"),
            TensorFactory.Parse("N"));

        var t = TensorFactory.Parse("Tr[T_a*T_b*T_c*T_d*T^a*T^b*T^c]");
        t = EliminateDueSymmetriesTransformation.Instance.Transform(trace.Transform(t));
        t = TensorFactory.ParseExpression("f_abc = I*e_abc").Transform(t);
        LeviCivitaSimplifyTransformation simplifyLeviCivita = new(TensorFactory.ParseSimple("e_abc"), false);
        t = simplifyLeviCivita.Transform(t);
        UnitarySimplifyTransformation simplifyUnitary = new(
            TensorFactory.ParseSimple("T_a"),
            TensorFactory.ParseSimple("f_abc"),
            TensorFactory.ParseSimple("d_abc"),
            TensorFactory.Parse("N"));
        t = simplifyUnitary.Transform(t);
        testOutputHelper.WriteLine(t.ToString());
        ShouldEqualTensor("0", trace.Transform(t));
    }

    [Fact]
    public void Test7()
    {
        Reset();
        ConfigureMatrixInsertion(TensorFactory.ParseSimple("m^a'_b'a"), IndexType.Matrix1);

        SetAntiSymmetric("f_abc");
        SetSymmetric("d_abc");

        UnitaryTraceTransformation tr = new(
            TensorFactory.ParseSimple("m_a"),
            TensorFactory.ParseSimple("f_abc"),
            TensorFactory.ParseSimple("d_abc"),
            TensorFactory.ParseSimple("n"));
        Tensor t = TensorFactory.Parse("Tr[m^a*m_b*m^c*(p^b*m_a + p_a*m^b)]");
        Tensor expanded = ExpandTransformation.Expand(t);
        ShouldEqualTensor("((1/4)*n-(1/2)*n**(-1))*p^{c}", tr.Transform(expanded));
    }

    [Fact]
    public void Test8()
    {
        Reset();
        ConfigureMatrixInsertion(TensorFactory.ParseSimple("T^a'_b'a"), IndexType.Matrix1);

        SetSymmetric("d_abd");
        SetAntiSymmetric("f_abc");
        SetAntiSymmetric("e_abc");

        ITransformation trace = new UnitaryTraceTransformation(
            TensorFactory.ParseSimple("T_a"),
            TensorFactory.ParseSimple("f_abc"),
            TensorFactory.ParseSimple("d_abc"),
            TensorFactory.Parse("N"));
        Tensor t = TensorFactory.Parse("T^aa'_b'*T^bb'_c'*d^c'_a'");
        ShouldEqualTensor("(1/2)*g^{ba}", trace.Transform(t));
    }

    private static Tensor UnitaryTrace(Tensor tensor)
    {
        UnitaryTraceTransformation trace = new(
            TensorFactory.ParseSimple("T_a^a'_b'"),
            TensorFactory.ParseSimple("f_abc"),
            TensorFactory.ParseSimple("d_abc"),
            TensorFactory.ParseSimple("N"));
        return trace.Transform(tensor);
    }

    private static void ConfigureMatrixInsertion(SimpleTensor rule, IndexType indexType)
    {
        _ = rule;
        _ = indexType;
        // TODO: GeneralIndicesInsertion is not yet ported; parser rules skipped.
    }

    private static void SetSymmetric(SimpleTensor tensor)
    {
        tensor.SimpleIndices.Symmetries.SetSymmetric();
    }

    private static void SetSymmetric(string tensor)
    {
        SetSymmetric(TensorFactory.ParseSimple(tensor));
    }

    private static void SetAntiSymmetric(SimpleTensor tensor)
    {
        tensor.SimpleIndices.Symmetries.SetAntiSymmetric();
    }

    private static void SetAntiSymmetric(string tensor)
    {
        SetAntiSymmetric(TensorFactory.ParseSimple(tensor));
    }

    private static void AddAntiSymmetry(SimpleTensor tensor, params int[] permutation)
    {
        tensor.SimpleIndices.Symmetries.AddAntiSymmetry(permutation);
    }

    private static void AddSymmetry(string tensor, params int[] permutation)
    {
        SimpleTensor simple = TensorFactory.ParseSimple(tensor);
        simple.SimpleIndices.Symmetries.AddSymmetry(permutation);
    }

    private static void Reset()
    {
        TensorCC.Reset();
    }

    private static void ShouldEqualTensor(string expected, Tensor actual)
    {
        ShouldEqualTensor(TensorFactory.Parse(expected), actual);
    }

    private static void ShouldEqualTensor(Tensor expected, Tensor actual)
    {
        TensorUtils.Equals(expected, actual).ShouldBeTrue(
            $"Tensor comparison failed.{Environment.NewLine}Expected: {expected}{Environment.NewLine}Actual: {actual}");
    }
}
