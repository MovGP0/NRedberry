using NRedberry.Physics.Feyncalc;
using NRedberry.Tensors;
using Shouldly;
using TensorFactory = NRedberry.Tensors.Tensors;
using Xunit;

namespace NRedberry.Physics.Tests.Feyncalc;

public sealed class SpinorsSimplifyTransformationTest : AbstractFeynCalcTest
{
    [Fact]
    public void Test1()
    {
        SetUp();

        SpinorsSimplifyTransformation sp = new(
            new SpinorsSimplifyOptions("u", "v", "cu", "cv", "p_a", "m"));

        var t = TensorFactory.Parse("cu*G_a*p^a");
        ShouldMatchTensor("m*cu", sp.Transform(t));

        t = TensorFactory.Parse("cu*G_b*G_a*p^a");
        ShouldMatchTensor("2*cu*p_{b}-m*cu*G_{b}", sp.Transform(t));

        t = TensorFactory.Parse("2*t_s*cu*G_a*p^a");
        ShouldMatchTensor("2*t_s*m*cu", sp.Transform(t));

        t = TensorFactory.Parse("G_a*p^a*u");
        ShouldMatchTensor("m*u", sp.Transform(t));

        t = TensorFactory.Parse("G_b*G_a*p^a*u");
        ShouldMatchTensor("G_b*m*u", sp.Transform(t));

        t = TensorFactory.Parse("G_a*G_b*p^a*u");
        ShouldMatchTensor("2*p_b*u-G_b*m*u", sp.Transform(t));

        t = TensorFactory.Parse("G_a*G_b*p^a*v");
        ShouldMatchTensor("2*p_b*v+G_b*m*v", sp.Transform(t));

        t = TensorFactory.Parse("k^b*G_a*G_b*p^a*u");
        ShouldMatchTensor("2*k^b*p_b*u-k^b*G_b*m*u", sp.Transform(t));

        t = TensorFactory.Parse("p^a*G_a*G_b*G_c*u");
        ShouldMatchTensor("m*G_b*G_c*u+2*G_c*u*p_b-2*G_b*u*p_c", sp.Transform(t));

        t = TensorFactory.Parse("p^a*G_a*G_b*G_c*v");
        ShouldMatchTensor("-m*G_b*G_c*v+2*G_c*v*p_b-2*G_b*v*p_c", sp.Transform(t));

        t = TensorFactory.Parse("cu*p^a*G_a*G_b*G_c*v");
        ShouldMatchTensor("m*cu*G_b*G_c*v", sp.Transform(t));
    }

    [Fact]
    public void Test1A()
    {
        SetUp();

        SpinorsSimplifyTransformation sp = new(
            new SpinorsSimplifyOptions(null, "v", null, null, "p_a", "m"));

        Tensor t = TensorFactory.Parse("cu*p^a*G_a*G_b*v");
        ShouldMatchTensor("2*cu*v*p_{b}+m*cu*G_{b}*v", sp.Transform(t));
    }

    [Fact]
    public void Test2()
    {
        SetUp();

        SpinorsSimplifyOptions options = new("u[p_a]", "v[p_a]", "cu[p_a]", "cv[p_a]", "p_a", "m")
        {
            DoDiracSimplify = true
        };
        SpinorsSimplifyTransformation sp = new(options);

        var t = TensorFactory.Parse("cu[p_a]*G_a*p^a");
        ShouldMatchTensor("m*cu[p_a]", sp.Transform(t));

        t = TensorFactory.Parse("cu[k_a]*G_a*p^a");
        ShouldKeepSameReference(t, sp.Transform(t));

        t = TensorFactory.Parse("cu[p_a]*G_a*p^a*G_b*p^b*u[p_a]");
        ShouldMatchTensor("m**2*cu[p_{a}]*u[p_{a}]", sp.Transform(t));

        t = TensorFactory.Parse("cu[p1_a]*G_a*p^a*G_b*p^b*u[p1_a]");
        ShouldKeepSameReference(t, sp.Transform(t));

        t = TensorFactory.Parse("2*p_i*p_j*Tr[G_p*G^q]*cu[p_a]*G_c*G_a*p^a*G_b*p^b*G^c*u[p_a]");
        ShouldMatchTensor("2*p_i*p_j*Tr[G_p*G^q]*4*m**2*cu[p_{a}]*u[p_{a}]", sp.Transform(t));

        t = TensorFactory.Parse("2*p_i*p_j*Tr[G^i*G^j]*cu[p_a]*G_c*G_a*p^a*G_b*p^b*G^c*u[p_a]");
        ShouldMatchTensor("2*m**2*4*4*m**2*cu[p_{a}]*u[p_{a}]", sp.Transform(t));
    }

    [Fact]
    public void Test3()
    {
        SetUp();

        SpinorsSimplifyTransformation sp2 = new(
            new SpinorsSimplifyOptions(null, null, "cu[p1_a[charm]]", null, "p1_a[charm]", "mc"));

        Tensor t = TensorFactory.Parse("p1^{a}[charm]*p1^{e}[charm]*v^{b'A'}[p2_{m}[charm]]*G_{a}^{e'}_{b'}*G_{b}^{a'}_{e'}*cu_{a'A'}[p1_{m}[charm]]*k2^{g}*e^{b}_{kge}*k1^{k}");
        sp2.Transform(t).ShouldNotBeNull();
    }

    [Fact]
    public void Test4()
    {
        SetUp();

        SpinorsSimplifyTransformation sp = new(
            new SpinorsSimplifyOptions("u", "v", "cu", "cv", "p_a", "m"));

        var t = TensorFactory.Parse("cu*v");
        ShouldMatchTensor("0", sp.Transform(t));

        t = TensorFactory.Parse("cu*G_a*p^a*v");
        ShouldMatchTensor("0", sp.Transform(t));

        t = TensorFactory.Parse("cu*G_a*p^a*G_b*p^b*v");
        ShouldMatchTensor("0", sp.Transform(t));
    }

    [Fact]
    public void Test5()
    {
        SetUp();

        SpinorsSimplifyTransformation sp = new(
            new SpinorsSimplifyOptions("u", "v", "cu", "cv", "p_a", "m"));

        var t = TensorFactory.Parse("G_a*p^a*G5*u");
        ShouldMatchTensor("-G5*m*u", sp.Transform(t));

        t = TensorFactory.Parse("G_a*p^a*G5*G5*u");
        ShouldMatchTensor("m*u", sp.Transform(t));

        t = TensorFactory.Parse("cu*G_a*p^a*G5");
        ShouldMatchTensor("m*cu*G5", sp.Transform(t));

        t = TensorFactory.Parse("cu*G5*G_a*p^a*G5");
        ShouldMatchTensor("-m*cu", sp.Transform(t));

        t = TensorFactory.Parse("G_{c}^{a'}_{d'}*G_{k}^{d'}_{g'}*G_{l}^{g'}_{e'}*v^{f'}*G5^{e'}_{f'}*cu_{a'}*eps^{c}_{a}[h[bottom]]*k2^{k}*k2^{a}*k1^{l}");
        sp.Transform(t).ShouldNotBeNull();
    }

    [Fact]
    public void Test6()
    {
        SetUp();

        SpinorsSimplifyTransformation sp = new(
            new SpinorsSimplifyOptions(null, null, null, "cv[x_a]", "p_a", "0"));

        Tensor t = TensorFactory.Parse("cv[x_a]*G_{a}*u*p^{a}");
        sp.Transform(t).ShouldNotBeNull();
    }

    private static void ShouldKeepSameReference(Tensor expected, Tensor actual)
    {
        ReferenceEquals(expected, actual).ShouldBeTrue();
    }
}
