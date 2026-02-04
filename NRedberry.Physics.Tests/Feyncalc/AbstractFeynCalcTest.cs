using NRedberry.Contexts;
using NRedberry;
using NRedberry.Indices;
using NRedberry.Numbers;
using NRedberry.Physics.Feyncalc;
using NRedberry.Tensors;
using NRedberry.Transformations;
using NRedberry.Transformations.Symmetrization;
using TensorCC = NRedberry.Tensors.CC;
using TensorFactory = NRedberry.Tensors.Tensors;

namespace NRedberry.Physics.Tests.Feyncalc;

public abstract class AbstractFeynCalcTest
{
    protected DiracOrderTransformation? dOrder;
    protected DiracTraceTransformation? dTrace;
    protected DiracSimplify1? dSimplify1;
    protected DiracSimplify0? dSimplify0;
    protected ITransformation? dSimplify;
    protected ITransformation? dSimplify0Chain;
    protected ITransformation? dSimplify1Chain;
    protected Expression? delDummy;
    protected Expression? traceOfOne;
    protected Expression? deltaTrace;
    protected ITransformation? simplifyG5;
    protected ITransformation? simplifyLeviCivita;
    protected SchoutenIdentities4? schouten4;
    protected DiracOptions? diracOptions;

    public virtual void SetUp()
    {
        SetUp(DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());
    }

    public virtual void SetUp(long seed)
    {
        TensorCC.Reset();
        TensorCC.ResetTensorNames(unchecked((int)seed));
        TensorCC.SetDefaultOutputFormat(OutputFormat.SimpleRedberry);
        TensorCC.SetParserAllowsSameVariance(true);

        // TODO: GeneralIndicesInsertion is not yet ported; parser rules are skipped.
        TensorFactory.Parse("GS[p_a] := p_a*G^a");
        TensorFactory.Parse("Pair[p_a, q_a] := p_a*q^a");
        // TODO: setAntiSymmetric("e_abcd") not yet ported.

        diracOptions = new DiracOptions();
        dOrder = new DiracOrderTransformation(diracOptions);
        dTrace = new DiracTraceTransformation(diracOptions);
        deltaTrace = TensorFactory.ParseExpression("d^a_a = 4");
        simplifyG5 = new SimplifyGamma5Transformation(diracOptions);

        dSimplify0 = new DiracSimplify0(diracOptions);
        dSimplify1 = new DiracSimplify1(diracOptions);

        dSimplify0Chain = new TransformationCollection(simplifyG5, dSimplify0);
        dSimplify1Chain = new TransformationCollection(simplifyG5, dSimplify1);
        dSimplify = new DiracSimplifyTransformation(diracOptions);

        delDummy = TensorFactory.ParseExpression("Dummy = 1");
        traceOfOne = TensorFactory.ParseExpression("d^a'_a' = 4");

        simplifyLeviCivita = new LeviCivitaSimplifyTransformation(TensorFactory.ParseSimple("e_abcd"), true);
        schouten4 = new SchoutenIdentities4(TensorFactory.ParseSimple("e_abcd"));
    }

    protected void TestFeynCalcData(ITransformation transformation, string resourceFile)
    {
        TestFeynCalcData(transformation, resourceFile, false, false);
    }

    protected void TestFeynCalcData(ITransformation transformation, string resourceFile, bool doTrace, bool addGamma5)
    {
        TestFeynCalcData(transformation, resourceFile, doTrace, addGamma5, true);
    }

    protected void TestFeynCalcData(
        ITransformation transformation,
        string resourceFile,
        bool doTrace,
        bool addGamma5,
        bool insertDummy)
    {
        throw new NotSupportedException("FeynCalc resource parsing is not yet ported.");
    }

    protected void AssertEquals(string a, Tensor b)
    {
        AssertEquals(TensorFactory.Parse(a), b);
    }

    protected void AssertEquals(Tensor a, Tensor b)
    {
        AssertEquals(a, b, new IdentityTransformation());
    }

    protected void AssertEquals(Tensor a, Tensor b, ITransformation addon)
    {
        if (a.Indices.GetFree().Size(IndexType.Matrix1) != 0 && b.Indices.GetFree().Size(IndexType.Matrix1) == 0)
        {
            AssertEquals(b, a, addon);
            return;
        }

        if (a.Indices.GetFree().Size(IndexType.Matrix1) == 0 && b.Indices.GetFree().Size(IndexType.Matrix1) != 0)
        {
            Indices.Indices free = b.Indices.GetFree().GetOfType(IndexType.Matrix1);
            Tensor metric = CreateMetricOrKronecker(free);
            a = ExpandAndEliminateTransformation.ExpandAndEliminate(
                TensorFactory.MultiplyAndRenameConflictingDummies(metric, a));
        }

        if (dOrder is null || traceOfOne is null)
        {
            throw new InvalidOperationException("SetUp() must be called before assertions.");
        }

        a = ExpandAndEliminateTransformation.ExpandAndEliminate(dOrder.Transform(a));
        b = ExpandAndEliminateTransformation.ExpandAndEliminate(dOrder.Transform(b));
        a = traceOfOne.Transform(a);
        b = traceOfOne.Transform(b);
        a = addon.Transform(a);
        b = addon.Transform(b);
        if (!TensorUtils.Equals(a, b))
        {
            throw new InvalidOperationException("Tensor comparison failed.");
        }
    }

    private static Tensor CreateMetricOrKronecker(Indices.Indices indices)
    {
        if (indices.Size() == 0)
        {
            return Complex.One;
        }

        if (indices.Size() == 2)
        {
            return Context.Get().CreateMetricOrKronecker(indices[0], indices[1]);
        }

        if (indices.Size() % 2 != 0)
        {
            throw new ArgumentException("Expected even number of indices.");
        }

        List<Tensor> factors = new(indices.Size() / 2);
        for (int i = 0; i < indices.Size(); i += 2)
        {
            factors.Add(Context.Get().CreateMetricOrKronecker(indices[i], indices[i + 1]));
        }

        return TensorFactory.Multiply(factors.ToArray());
    }
}
