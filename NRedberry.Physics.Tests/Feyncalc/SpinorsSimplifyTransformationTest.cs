using System;
using NRedberry.Physics.Feyncalc;
using NRedberry.Tensors;
using TensorFactory = NRedberry.Tensors.Tensors;

namespace NRedberry.Physics.Tests.Feyncalc;

public sealed class SpinorsSimplifyTransformationTest : AbstractFeynCalcTest
{
    public void Test1()
    {
        SetUp();
        // TODO: GeneralIndicesInsertion is not yet ported; insertion rules skipped.

        SpinorsSimplifyTransformation sp = new(
            new SpinorsSimplifyOptions("u", "v", "cu", "cv", "p_a", "m"));

        Tensor t;
        t = TensorFactory.Parse("cu*G_a*p^a");
        AssertEquals("m*cu", sp.Transform(t));

        t = TensorFactory.Parse("cu*G_b*G_a*p^a");
        AssertEquals("2*cu*p_{b}-m*cu*G_{b}", sp.Transform(t));

        t = TensorFactory.Parse("2*t_s*cu*G_a*p^a");
        AssertEquals("2*t_s*m*cu", sp.Transform(t));

        t = TensorFactory.Parse("G_a*p^a*u");
        AssertEquals("m*u", sp.Transform(t));

        t = TensorFactory.Parse("G_b*G_a*p^a*u");
        AssertEquals("G_b*m*u", sp.Transform(t));

        t = TensorFactory.Parse("G_a*G_b*p^a*u");
        AssertEquals("2*p_b*u-G_b*m*u", sp.Transform(t));

        t = TensorFactory.Parse("G_a*G_b*p^a*v");
        AssertEquals("2*p_b*v+G_b*m*v", sp.Transform(t));

        t = TensorFactory.Parse("k^b*G_a*G_b*p^a*u");
        AssertEquals("2*k^b*p_b*u-k^b*G_b*m*u", sp.Transform(t));

        t = TensorFactory.Parse("p^a*G_a*G_b*G_c*u");
        AssertEquals("m*G_b*G_c*u+2*G_c*u*p_b-2*G_b*u*p_c", sp.Transform(t));

        t = TensorFactory.Parse("p^a*G_a*G_b*G_c*v");
        AssertEquals("-m*G_b*G_c*v+2*G_c*v*p_b-2*G_b*v*p_c", sp.Transform(t));

        t = TensorFactory.Parse("cu*p^a*G_a*G_b*G_c*v");
        AssertEquals("m*cu*G_b*G_c*v", sp.Transform(t));
    }

    public void Test1A()
    {
        SetUp();
        // TODO: GeneralIndicesInsertion is not yet ported; insertion rules skipped.

        SpinorsSimplifyTransformation sp = new(
            new SpinorsSimplifyOptions(null, "v", null, null, "p_a", "m"));

        Tensor t = TensorFactory.Parse("cu*p^a*G_a*G_b*v");
        AssertEquals("2*cu*v*p_{b}+m*cu*G_{b}*v", sp.Transform(t));
    }

    public void Test2()
    {
        SetUp();
        // TODO: GeneralIndicesInsertion is not yet ported; insertion rules skipped.

        SpinorsSimplifyOptions options = new("u[p_a]", "v[p_a]", "cu[p_a]", "cv[p_a]", "p_a", "m")
        {
            DoDiracSimplify = true
        };
        SpinorsSimplifyTransformation sp = new(options);

        Tensor t;
        t = TensorFactory.Parse("cu[p_a]*G_a*p^a");
        AssertEquals("m*cu[p_a]", sp.Transform(t));

        t = TensorFactory.Parse("cu[k_a]*G_a*p^a");
        AssertSameReference(t, sp.Transform(t));

        t = TensorFactory.Parse("cu[p_a]*G_a*p^a*G_b*p^b*u[p_a]");
        AssertEquals("m**2*cu[p_{a}]*u[p_{a}]", sp.Transform(t));

        t = TensorFactory.Parse("cu[p1_a]*G_a*p^a*G_b*p^b*u[p1_a]");
        AssertSameReference(t, sp.Transform(t));

        t = TensorFactory.Parse("2*p_i*p_j*Tr[G_p*G^q]*cu[p_a]*G_c*G_a*p^a*G_b*p^b*G^c*u[p_a]");
        AssertEquals("2*p_i*p_j*Tr[G_p*G^q]*4*m**2*cu[p_{a}]*u[p_{a}]", sp.Transform(t));

        t = TensorFactory.Parse("2*p_i*p_j*Tr[G^i*G^j]*cu[p_a]*G_c*G_a*p^a*G_b*p^b*G^c*u[p_a]");
        AssertEquals("2*m**2*4*4*m**2*cu[p_{a}]*u[p_{a}]", sp.Transform(t));
    }

    public void Test3()
    {
        SetUp();
        // TODO: GeneralIndicesInsertion is not yet ported; insertion rules skipped.

        SpinorsSimplifyTransformation sp2 = new(
            new SpinorsSimplifyOptions(null, null, "cu[p1_a[charm]]", null, "p1_a[charm]", "mc"));

        Tensor t = TensorFactory.Parse("p1^{a}[charm]*p1^{e}[charm]*v^{b'A'}[p2_{m}[charm]]*G_{a}^{e'}_{b'}*G_{b}^{a'}_{e'}*cu_{a'A'}[p1_{m}[charm]]*k2^{g}*e^{b}_{kge}*k1^{k}");
        sp2.Transform(t);
    }

    public void Test4()
    {
        SetUp();
        // TODO: GeneralIndicesInsertion is not yet ported; insertion rules skipped.

        SpinorsSimplifyTransformation sp = new(
            new SpinorsSimplifyOptions("u", "v", "cu", "cv", "p_a", "m"));

        Tensor t;
        t = TensorFactory.Parse("cu*v");
        AssertEquals("0", sp.Transform(t));

        t = TensorFactory.Parse("cu*G_a*p^a*v");
        AssertEquals("0", sp.Transform(t));

        t = TensorFactory.Parse("cu*G_a*p^a*G_b*p^b*v");
        AssertEquals("0", sp.Transform(t));
    }

    public void Test5()
    {
        SetUp();
        // TODO: GeneralIndicesInsertion is not yet ported; insertion rules skipped.

        SpinorsSimplifyTransformation sp = new(
            new SpinorsSimplifyOptions("u", "v", "cu", "cv", "p_a", "m"));

        Tensor t;
        t = TensorFactory.Parse("G_a*p^a*G5*u");
        AssertEquals("-G5*m*u", sp.Transform(t));

        t = TensorFactory.Parse("G_a*p^a*G5*G5*u");
        AssertEquals("m*u", sp.Transform(t));

        t = TensorFactory.Parse("cu*G_a*p^a*G5");
        AssertEquals("m*cu*G5", sp.Transform(t));

        t = TensorFactory.Parse("cu*G5*G_a*p^a*G5");
        AssertEquals("-m*cu", sp.Transform(t));

        t = TensorFactory.Parse("G_{c}^{a'}_{d'}*G_{k}^{d'}_{g'}*G_{l}^{g'}_{e'}*v^{f'}*G5^{e'}_{f'}*cu_{a'}*eps^{c}_{a}[h[bottom]]*k2^{k}*k2^{a}*k1^{l}");
        Console.WriteLine(t);

        Console.WriteLine(TensorFactory.Parse("G_{a}^{f'}_{b'}*G^{me'}_{f'}*G^{da'}_{e'}*cu_{a'}*v^{b'}*k2^{a}*k2^{k}*p1^{e}*e^{b}_{nke}*k1_{m}*k1^{n}*eps_{bd}"));
    }

    public void Test6()
    {
        SetUp();
        // TODO: GeneralIndicesInsertion is not yet ported; insertion rules skipped.

        SpinorsSimplifyTransformation sp = new(
            new SpinorsSimplifyOptions(null, null, null, "cv[x_a]", "p_a", "0"));

        Tensor t = TensorFactory.Parse("cv[x_a]*G_{a}*u*p^{a}");
        Console.WriteLine(sp.Transform(t));
    }

    private static void AssertSameReference(Tensor expected, Tensor actual)
    {
        if (!ReferenceEquals(expected, actual))
        {
            throw new InvalidOperationException("Expected transformation to return the original instance.");
        }
    }
}
