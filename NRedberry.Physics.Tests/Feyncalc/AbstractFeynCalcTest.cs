using NRedberry.Contexts;
using NRedberry.Numbers;
using NRedberry.Physics.Feyncalc;
using NRedberry.Tensors;
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

        // GeneralIndicesInsertion and parser alias definitions are still unported.
        // Keep the shared test setup limited to declarations that the current parser can execute.

        diracOptions = new DiracOptions
        {
            GammaMatrix = TensorFactory.ParseSimple("G^a'_b'a"),
            Gamma5 = TensorFactory.ParseSimple("G5^a'_b'"),
            LeviCivita = TensorFactory.ParseSimple("e_abcd")
        };
        dOrder = new DiracOrderTransformation(diracOptions);
        dTrace = new DiracTraceTransformation(diracOptions);
        deltaTrace = TensorFactory.ParseExpression("d^a_a = 4");
        simplifyG5 = new SimplifyGamma5Transformation(diracOptions);

        dSimplify0 = TryCreate(() => new DiracSimplify0(diracOptions));
        dSimplify1 = TryCreate(() => new DiracSimplify1(diracOptions));

        dSimplify0Chain = dSimplify0 is null ? null : new TransformationCollection(simplifyG5, dSimplify0);
        dSimplify1Chain = dSimplify1 is null ? null : new TransformationCollection(simplifyG5, dSimplify1);
        dSimplify = TryCreate(() => new DiracSimplifyTransformation(diracOptions));

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
        ArgumentNullException.ThrowIfNull(transformation);
        ArgumentException.ThrowIfNullOrWhiteSpace(resourceFile);

        if (dTrace is null || delDummy is null || traceOfOne is null)
        {
            throw new InvalidOperationException("SetUp() must be called before replaying FeynCalc resources.");
        }

        using StreamReader reader = File.OpenText(ResolveFeynCalcResourcePath(resourceFile));

        var totalTests = 0;
        ITransformation addon = doTrace ? dTrace : new IdentityTransformation();
        while (true)
        {
            string? initial = reader.ReadLine();
            if (initial is null)
            {
                break;
            }

            string? answer = reader.ReadLine();
            if (answer is null)
            {
                throw new InvalidDataException($"Resource '{resourceFile}' contains an unmatched test input line.");
            }

            totalTests++;
            string preparedInitial = PrepareResourceExpression(initial, doTrace, addGamma5, false);
            string preparedAnswer = PrepareResourceExpression(answer, doTrace, addGamma5, insertDummy);

            Tensor test = TensorFactory.Parse(preparedInitial);
            Tensor expected = TensorFactory.Parse(preparedAnswer);
            expected = TryTransform(delDummy, expected);
            expected = ExpandAndEliminateTransformation.ExpandAndEliminate(expected);
            expected = TryTransform(traceOfOne, expected);

            Tensor result = transformation.Transform(test);
            try
            {
                ShouldMatchTensor(expected, result, addon);
            }
            catch (Exception exception)
            {
                throw new InvalidOperationException(
                    string.Concat(
                        $"FeynCalc resource replay failed for '{resourceFile}' after {totalTests} case(s).",
                        Environment.NewLine,
                        $"Original: {preparedInitial}",
                        Environment.NewLine,
                        $"Answer: {preparedAnswer}",
                        Environment.NewLine,
                        $"Result: {result}"),
                    exception);
            }
        }
    }

    protected void ShouldMatchTensor(string a, Tensor b)
    {
        ShouldMatchTensor(TensorFactory.Parse(a), b);
    }

    protected void ShouldMatchTensor(Tensor a, Tensor b)
    {
        ShouldMatchTensor(a, b, new IdentityTransformation());
    }

    protected void ShouldMatchTensor(Tensor a, Tensor b, ITransformation addon)
    {
        if (a.Indices.GetFree().Size(IndexType.Matrix1) != 0 && b.Indices.GetFree().Size(IndexType.Matrix1) == 0)
        {
            ShouldMatchTensor(b, a, addon);
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
        a = TryTransform(traceOfOne, a);
        b = TryTransform(traceOfOne, b);
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

    private static string PrepareResourceExpression(
        string expression,
        bool doTrace,
        bool addGamma5,
        bool insertDummy)
    {
        string prepared = expression;
        if (insertDummy)
        {
            prepared = $"({prepared})*Dummy";
        }

        if (addGamma5)
        {
            prepared = $"({prepared})*G5";
        }

        if (doTrace)
        {
            prepared = $"Tr[{prepared}]";
        }

        return prepared;
    }

    private static string ResolveFeynCalcResourcePath(string resourceFile)
    {
        DirectoryInfo? current = new(AppContext.BaseDirectory);
        while (current is not null)
        {
            string candidate = Path.Combine(
                current.FullName,
                "redberry",
                "physics",
                "src",
                "test",
                "resources",
                "cc",
                "redberry",
                "physics",
                "feyncalc",
                resourceFile);
            if (File.Exists(candidate))
            {
                return candidate;
            }

            current = current.Parent;
        }

        throw new FileNotFoundException($"Unable to locate FeynCalc test resource '{resourceFile}'.");
    }

    protected static string[] ReadFeynCalcResourceLines(string resourceFile)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(resourceFile);
        return File.ReadAllLines(ResolveFeynCalcResourcePath(resourceFile));
    }

    private static T? TryCreate<T>(Func<T> factory)
        where T : class
    {
        try
        {
            return factory();
        }
        catch (NotImplementedException)
        {
            return null;
        }
    }

    private static Tensor TryTransform(ITransformation transformation, Tensor tensor)
    {
        try
        {
            return transformation.Transform(tensor);
        }
        catch (NotImplementedException)
        {
            return tensor;
        }
    }
}
