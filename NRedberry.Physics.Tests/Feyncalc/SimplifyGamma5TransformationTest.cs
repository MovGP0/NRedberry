using System;
using NRedberry.Tensors;
using NRedberry.Transformations.Symmetrization;
using TensorFactory = NRedberry.Tensors.Tensors;

namespace NRedberry.Physics.Tests.Feyncalc;

public sealed class SimplifyGamma5TransformationTest : AbstractFeynCalcTest
{
    public void Test1()
    {
        SetUp();
        ITransformation simplify = RequireSimplifyG5();
        Tensor t;

        t = TensorFactory.Parse("G_a*G5");
        AssertSameReference(t, simplify.Transform(t));

        t = TensorFactory.Parse("G5*G_a");
        AssertEquals("-G_a*G5", simplify.Transform(t));

        t = TensorFactory.Parse("G5*G_a*G5");
        AssertEquals("-G_a", simplify.Transform(t));

        t = TensorFactory.Parse("G5*G5*G5");
        AssertEquals("G5", simplify.Transform(t));

        t = TensorFactory.Parse("G5*G_a*G_b*G5");
        AssertEquals("G_a*G_b", simplify.Transform(t));

        t = TensorFactory.Parse("G5*G_a*G5*G_b*G5");
        AssertEquals("-G_a*G_b*G5", simplify.Transform(t));

        t = TensorFactory.Parse("G5*G5*G_a*G5*G_b*G5");
        AssertEquals("-G_a*G_b", simplify.Transform(t));

        t = TensorFactory.Parse("G_a*G5*G_b*G5*G5*G5");
        AssertEquals("-G_a*G_b", simplify.Transform(t));
    }

    public void Test2()
    {
        SetUp();
        ITransformation simplify = RequireSimplifyG5();
        Tensor t;

        t = TensorFactory.Parse("G5*AMATRIX*G_a");
        AssertEquals("G5*AMATRIX*G_a", simplify.Transform(t));

        t = TensorFactory.Parse("G5*G_a*G5*AMATRIX*G5");
        AssertEquals("-G_a*AMATRIX*G5", simplify.Transform(t));

        t = TensorFactory.Parse("G5*G5*G5*AMATRIX*G5*G_d*G_c");
        AssertEquals("G5*AMATRIX*G_d*G_c*G5", simplify.Transform(t));

        t = TensorFactory.Parse("Tr[G5*G_a*G_b*G5]");
        AssertEquals("Tr[G_a*G_b]", simplify.Transform(t));

        t = TensorFactory.Parse("2*k^a*p^c*q^d*G5*G_a*G5*AMATRIX*G5*G_d*G_c");
        AssertEquals("-2*k^a*p^c*q^d*G_a*AMATRIX*G_d*G_c*G5", simplify.Transform(t));
    }

    public void Test3()
    {
        SetUp(12332);
        ITransformation simplify = RequireSimplifyG5();
        // TODO: GeneralIndicesInsertion is not yet ported; insertion rules skipped.

        Tensor t;
        t = TensorFactory.Parse("G5*G_a");
        t = ApplyUntilUnchanged(t, 1000, simplify);
        AssertSameReference(t, simplify.Transform(t));

        t = TensorFactory.Parse("Tr[G5*G_a*G_b]");
        t = ApplyUntilUnchanged(t, 1000, simplify);
        AssertSameReference(t, simplify.Transform(t));

        t = TensorFactory.Parse("Tr[G5*G_a*G_b] + Tr[G_a*G_b]");
        t = ApplyUntilUnchanged(t, 1000, simplify);
        AssertSameReference(t, simplify.Transform(t));

        t = TensorFactory.Parse("Tr[G5*G_a*G5*G_b*G5] + Tr[G_a*G_b]");
        t = ApplyUntilUnchanged(t, 1000, simplify);
        AssertSameReference(t, simplify.Transform(t));

        t = TensorFactory.Parse("cu[p1_{m}[charm]]*G^{d}*G5*v[p2_{m}[charm]]");
        t = ApplyUntilUnchanged(t, 1000, simplify);
        AssertSameReference(t, simplify.Transform(t));
    }

    public void Test4()
    {
        SetUp();
        ITransformation simplify = RequireSimplifyG5();

        Tensor t = TensorFactory.Parse("Tr[G5*G5*G5*G5*G5]");
        if (!TensorUtils.Equals(TensorFactory.Parse("0"), simplify.Transform(t)))
        {
            throw new InvalidOperationException("Tensor comparison failed.");
        }
    }

    public void Test3A()
    {
        for (int i = 0; i < 10; ++i)
        {
            SetUp();
            ITransformation simplify = RequireSimplifyG5();
            // TODO: GeneralIndicesInsertion is not yet ported; insertion rules skipped.

            Tensor t = TensorFactory.Parse("-378*cu_{a'A'}[p1_{m}[charm]]*T^{CA'}_{B'}*k1^{e}*k1^{k}*f_{BAC}*k2^{a}*k2_{b}*k2^{h}*G_{a}^{a'}_{b'}*e^{b}_{khe}*v^{b'B'}[p2_{m}[charm]]-294*cu_{a'B'}[p1_{m}[charm]]*k1^{e}*k1_{m}*k1^{k}*k1^{a}*k2^{h}*G^{da'}_{e'}*e^{b}_{khe}*e_{b}^{m}_{ad}*g_{BA}*v^{b'B'}[p2_{m}[charm]]*G5^{e'}_{b'}-1344*cu_{a'A'}[p1_{m}[charm]]*T^{CA'}_{B'}*k1^{a}*k1^{k}*k1^{e}*k2^{h}*p1_{m}[charm]*G^{da'}_{e'}*e^{b}_{khe}*e_{a}^{m}_{bd}*v^{b'B'}[p2_{m}[charm]]*d_{BAC}*G5^{e'}_{b'}+(-3654*I)*cu_{a'A'}[p1_{m}[charm]]*T^{CA'}_{B'}*k1^{e}*k1^{k}*k2^{h}*p1_{b}[charm]*e^{b}_{khe}*v^{a'B'}[p2_{m}[charm]]*d_{BAC}+(-126*I)*cu_{a'A'}[p1_{m}[charm]]*T^{CA'}_{B'}*k1^{k}*f_{BAC}*k2^{e}*k2_{m}*k2^{h}*p2^{a}[charm]*G^{da'}_{e'}*e^{b}_{khe}*e_{b}^{m}_{ad}*v^{b'B'}[p2_{m}[charm]]*G5^{e'}_{b'}+(-126*I)*cu_{a'A'}[p1_{m}[charm]]*T^{CA'}_{B'}*k1^{k}*k1^{e}*k2^{a}*k2^{h}*k2_{b}*G_{a}^{a'}_{b'}*e^{b}_{khe}*v^{b'B'}[p2_{m}[charm]]*d_{BAC}-84*cu_{a'B'}[p1_{m}[charm]]*k1^{k}*k2^{e}*k2^{h}*k2_{m}*p1^{a}[charm]*G^{da'}_{e'}*e^{b}_{khe}*e_{b}^{m}_{ad}*g_{BA}*v^{b'B'}[p2_{m}[charm]]*G5^{e'}_{b'}-126*cu_{a'A'}[p1_{m}[charm]]*T^{CA'}_{B'}*k1^{k}*f_{BAC}*k2^{a}*k2^{h}*p1_{b}[charm]*p2^{e}[charm]*G_{a}^{a'}_{b'}*e^{b}_{khe}*v^{b'B'}[p2_{m}[charm]]+((-42*I)*(-4+m[charm]**2)-42*I+(21*I)*(-5+m[charm]**2)+(-84*I)*m[charm]**2+(-42*I)*(2*m[charm]**2+4*m[bottom]**2-20)+(91*I)*(-3+m[charm]**2)+(-56*I)*(-2+m[charm]**2))*cu_{a'B'}[p1_{m}[charm]]*k1^{k}*k2^{h}*p2^{e}[charm]*G_{b}^{a'}_{b'}*e^{b}_{khe}*g_{BA}*v^{b'B'}[p2_{m}[charm]]-84*cu_{a'B'}[p1_{m}[charm]]*k1^{k}*k2^{h}*p1^{e}[charm]*p2^{a}[charm]*p2_{m}[charm]*G^{da'}_{e'}*e^{b}_{khe}*e_{b}^{m}_{ad}*g_{BA}*v^{b'B'}[p2_{m}[charm]]*G5^{e'}_{b'}-42*cu_{a'B'}[p1_{m}[charm]]*k1^{e}*k1^{k}*k2^{h}*k2_{m}*p2^{a}[charm]*G^{da'}_{e'}*e^{b}_{khe}*e_{b}^{m}_{ad}*g_{BA}*v^{b'B'}[p2_{m}[charm]]*G5^{e'}_{b'}+882*cu_{a'A'}[p1_{m}[charm]]*T^{CA'}_{B'}*k1^{a}*k1^{k}*k1_{m}*k2^{h}*p1^{e}[charm]*G^{da'}_{e'}*e_{b}^{m}_{ad}*e^{b}_{khe}*v^{b'B'}[p2_{m}[charm]]*d_{BAC}*G5^{e'}_{b'}+(-252*I)*cu_{a'A'}[p1_{m}[charm]]*T^{CA'}_{B'}*k1^{k}*f_{BAC}*k2^{h}*p1^{e}[charm]*p2^{a}[charm]*p2_{m}[charm]*G^{da'}_{e'}*e^{b}_{khe}*e_{b}^{m}_{ad}*v^{b'B'}[p2_{m}[charm]]*G5^{e'}_{b'}+(630*I)*cu_{b'A'}[p1_{m}[charm]]*T^{CA'}_{B'}*k1^{k}*k2^{h}*k2^{e}*p2_{b}[charm]*e^{b}_{khe}*v^{b'B'}[p2_{m}[charm]]*d_{ABC}+126*cu_{a'A'}[p1_{m}[charm]]*T^{CA'}_{B'}*k1^{k}*f_{BAC}*k2^{a}*k2^{h}*p2^{e}[charm]*G_{a}^{e'}_{b'}*G_{b}^{a'}_{e'}*e^{b}_{khe}*v^{b'B'}[p2_{m}[charm]]+252*cu_{a'A'}[p1_{m}[charm]]*T^{CA'}_{B'}*k1^{k}*k2^{h}*k2^{e}*p2_{m}[charm]*p2^{a}[charm]*G^{da'}_{e'}*e^{b}_{khe}*e_{b}^{m}_{ad}*v^{b'B'}[p2_{m}[charm]]*d_{BAC}*G5^{e'}_{b'}");
            ApplyUntilUnchanged(t, 1000, simplify);
        }
    }

    private static Tensor ApplyUntilUnchanged(Tensor tensor, int maxIterations, ITransformation transformation)
    {
        Tensor current = tensor;
        for (int i = 0; i < maxIterations; ++i)
        {
            Tensor next = transformation.Transform(current);
            if (TensorUtils.Equals(current, next))
            {
                return next;
            }

            current = next;
        }

        return current;
    }

    private ITransformation RequireSimplifyG5()
    {
        if (simplifyG5 is null)
        {
            throw new InvalidOperationException("SetUp() must be called before using simplifyG5.");
        }

        return simplifyG5;
    }

    private static void AssertSameReference(Tensor expected, Tensor actual)
    {
        if (!ReferenceEquals(expected, actual))
        {
            throw new InvalidOperationException("Expected transformation to return the original instance.");
        }
    }
}
