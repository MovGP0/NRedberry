using NRedberry.Contexts;
using NRedberry.Indices;
using NRedberry.Numbers;
using NRedberry.TensorGenerators;
using NRedberry.Tensors;
using NRedberry.Transformations.Expand;
using NRedberry.Transformations.Symmetrization;
using IndexType = NRedberry.IndexType;
using TensorCC = NRedberry.Tensors.CC;
using TensorOps = NRedberry.Tensors.Tensors;

namespace NRedberry.Solver;

/*
 * Original: ./core/src/main/java/cc/redberry/core/solver/ReduceEngine.java
 */

public static class ReduceEngine
{
    private const int IterationLimit = 10000;

    public static ReducedSystem ReduceToSymbolicSystem(Expression[] equations, SimpleTensor[] vars, ITransformation[] rules)
    {
        return ReduceToSymbolicSystem(equations, vars, rules, new bool[vars.Length]);
    }

    public static ReducedSystem ReduceToSymbolicSystem(Expression[] equations, SimpleTensor[] vars, ITransformation[] rules, bool[] symmetricForm)
    {
        ArgumentNullException.ThrowIfNull(equations);
        ArgumentNullException.ThrowIfNull(vars);
        ArgumentNullException.ThrowIfNull(rules);
        ArgumentNullException.ThrowIfNull(symmetricForm);

        if (symmetricForm.Length != vars.Length)
        {
            throw new ArgumentException("Length of symmetricForm must match vars length.", nameof(symmetricForm));
        }

        Tensor[] zeroReduced = new Tensor[equations.Length];
        for (int i = equations.Length - 1; i >= 0; --i)
        {
            zeroReduced[i] = TensorOps.Subtract(equations[i][0], equations[i][1]);
            zeroReduced[i] = ExpandTransformation.Expand(zeroReduced[i], EliminateMetricsTransformation.Instance);
            zeroReduced[i] = EliminateMetricsTransformation.Eliminate(zeroReduced[i]);
        }

        HashSet<int> varsNames = new(vars.Length);
        foreach (SimpleTensor var in vars)
        {
            varsNames.Add(var.Name);
        }

        Tensor[] samples = GetSamples(zeroReduced, varsNames);
        if (samples.Length == 0)
        {
            for (int i = 0; i < vars.Length; ++i)
            {
                if (vars[i].Indices.Size() != 0)
                {
                    return null!;
                }
            }
        }

        var generalSolutions = new Expression[vars.Length];
        List<SimpleTensor> unknownCoefficients = [];

        for (int i = 0; i < generalSolutions.Length; ++i)
        {
            if (vars[i].Indices.Size() == 0)
            {
                SimpleTensor nVar = TensorCC.GenerateNewSymbol();
                unknownCoefficients.Add(nVar);
                generalSolutions[i] = TensorOps.Expression(vars[i], nVar);
            }
            else
            {
                GeneratedTensor generatedTensor = TensorGenerator.GenerateStructure(vars[i].SimpleIndices, samples, symmetricForm[i], true, true);
                unknownCoefficients.AddRange(generatedTensor.Coefficients);
                generalSolutions[i] = TensorOps.Expression(vars[i], generatedTensor.Tensor);
            }
        }

        List<ITransformation> allRules = new(rules);
        allRules.Insert(0, EliminateMetricsTransformation.Instance);
        ITransformation simplification = new TransformationCollection(allRules);

        List<Expression> reducedSystem = [];
        foreach (Tensor original in zeroReduced)
        {
            Tensor equation = original;
            int count = IterationLimit;
            do
            {
                foreach (Expression solution in generalSolutions)
                {
                    equation = solution.Transform(equation);
                }

                equation = ExpandTransformation.Expand(equation, simplification);
                equation = simplification.Transform(equation);
                equation = CollectNonScalarsITransformation.CollectNonScalars(equation);
                if (!TensorUtils.ContainsSimpleTensors(equation, varsNames))
                {
                    break;
                }
            }
            while (count-- > 0);

            if (count <= 0)
            {
                throw new InvalidOperationException("Maximum number of iterations exceeded: the system cannot be reduced after 10 000 iterations.");
            }

            if (equation.Indices.Size() == 0)
            {
                reducedSystem.Add(TensorOps.Expression(equation, Complex.Zero));
            }
            else if (equation is Sum sum)
            {
                foreach (Tensor t in sum)
                {
                    reducedSystem.Add(TensorOps.Expression(Split.SplitScalars(t).Summand, Complex.Zero));
                }
            }
            else
            {
                reducedSystem.Add(TensorOps.Expression(Split.SplitScalars(equation).Summand, Complex.Zero));
            }
        }

        return new ReducedSystem(
            reducedSystem.ToArray(),
            unknownCoefficients.ToArray(),
            generalSolutions);
    }

    private static Tensor[] GetSamples(Tensor[] zeroReduced, HashSet<int> vars)
    {
        ICollection<SimpleTensor> content = TensorUtils.GetAllDiffSimpleTensors(zeroReduced);
        List<Tensor> samples = new(content.Count + 1);
        HashSet<IndexType> usedTypes = [];

        foreach (SimpleTensor st in content)
        {
            if (vars.Contains(st.Name))
            {
                continue;
            }

            if (st.Indices.Size() == 0)
            {
                continue;
            }

            if (Context.Get().IsKroneckerOrMetric(st))
            {
                usedTypes.Add(IndicesUtils.GetTypeEnum(st.Indices[0]));
                continue;
            }

            SimpleIndices si = st.SimpleIndices;
            for (int i = si.Size() - 1; i >= 0; --i)
            {
                usedTypes.Add(IndicesUtils.GetTypeEnum(si[i]));
            }

            SimpleTensor renamed = TensorOps.SimpleTensor(
                st.Name,
                IndicesFactory.CreateSimple(st.GetNameDescriptor().GetSymmetries(), si));
            samples.AddRange(TensorGeneratorUtils.AllStatesCombinations(renamed));
        }

        foreach (IndexType type in usedTypes)
        {
            byte btype = type.GetType_();
            samples.Add(Context.Get().CreateKronecker(
                IndicesUtils.SetType(btype, 0),
                IndicesUtils.UpperRawStateInt | IndicesUtils.SetType(btype, 1)));

            if (TensorCC.IsMetric(btype))
            {
                samples.Add(Context.Get().CreateMetric(
                    IndicesUtils.SetType(btype, 0),
                    IndicesUtils.SetType(btype, 1)));
                samples.Add(Context.Get().CreateMetric(
                    IndicesUtils.UpperRawStateInt | IndicesUtils.SetType(btype, 0),
                    IndicesUtils.UpperRawStateInt | IndicesUtils.SetType(btype, 1)));
            }
        }

        return samples.ToArray();
    }
}
