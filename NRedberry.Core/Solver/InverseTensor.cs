using NRedberry.IndexMapping;
using NRedberry.Numbers;
using NRedberry.TensorGenerators;
using NRedberry.Tensors;
using NRedberry.Transformations.Expand;
using NRedberry.Transformations.Symmetrization;
using TensorCC = NRedberry.Tensors.CC;
using TensorOps = NRedberry.Tensors.Tensors;

namespace NRedberry.Solver;

/*
 * Original: ./core/src/main/java/cc/redberry/core/solver/InverseTensor.java
 */

public sealed class InverseTensor
{
    private readonly Expression[] _equations;
    private readonly SimpleTensor[] _unknownCoefficients;
    private readonly Expression _generalInverse;

    public InverseTensor(Expression toInverse, Expression equation, Tensor[] samples)
    {
        ArgumentNullException.ThrowIfNull(toInverse);
        ArgumentNullException.ThrowIfNull(equation);
        ArgumentNullException.ThrowIfNull(samples);

        (_equations, _unknownCoefficients, _generalInverse) = Build(toInverse, equation, samples, false, Array.Empty<ITransformation>());
    }

    public InverseTensor(Expression toInverse, Expression equation, Tensor[] samples, bool symmetricForm, ITransformation[] transformations)
    {
        ArgumentNullException.ThrowIfNull(toInverse);
        ArgumentNullException.ThrowIfNull(equation);
        ArgumentNullException.ThrowIfNull(samples);
        ArgumentNullException.ThrowIfNull(transformations);

        (_equations, _unknownCoefficients, _generalInverse) = Build(toInverse, equation, samples, symmetricForm, transformations);
    }

    public Expression[] GetEquations()
    {
        return (Expression[])_equations.Clone();
    }

    public Expression GetGeneralInverseForm()
    {
        return _generalInverse;
    }

    public SimpleTensor[] GetUnknownCoefficients()
    {
        return (SimpleTensor[])_unknownCoefficients.Clone();
    }

    public ReducedSystem ToReducedSystem()
    {
        return new ReducedSystem(_equations, _unknownCoefficients, new[] { _generalInverse });
    }

    public static Expression FindInverseWithMaple(Expression toInverse, Expression equation, Tensor[] samples, bool symmetricForm, ITransformation[] transformations, string mapleBinDir, string path)
    {
        return FindInverseWithMaple(toInverse, equation, samples, symmetricForm, false, transformations, mapleBinDir, path);
    }

    public static Expression FindInverseWithMaple(Expression toInverse, Expression equation, Tensor[] samples, bool symmetricForm, bool keepFreeParameters, ITransformation[] transformations, string mapleBinDir, string path)
    {
        ReducedSystem reducedSystem = new InverseTensor(toInverse, equation, samples, symmetricForm, transformations).ToReducedSystem();
        return ExternalSolver.SolveSystemWithExternalProgram(
            ExternalSolver.MapleScriptCreator.Instance,
            reducedSystem,
            keepFreeParameters,
            mapleBinDir,
            path)[0][0];
    }

    public static Expression FindInverseWithMathematica(Expression toInverse, Expression equation, Tensor[] samples, bool symmetricForm, ITransformation[] transformations, string mathematicaBinDir, string path)
    {
        return FindInverseWithMathematica(toInverse, equation, samples, symmetricForm, false, transformations, mathematicaBinDir, path);
    }

    public static Expression FindInverseWithMathematica(Expression toInverse, Expression equation, Tensor[] samples, bool symmetricForm, bool keepFreeParameters, ITransformation[] transformations, string mathematicaBinDir, string path)
    {
        ReducedSystem reducedSystem = new InverseTensor(toInverse, equation, samples, symmetricForm, transformations).ToReducedSystem();
        return ExternalSolver.SolveSystemWithExternalProgram(
            ExternalSolver.MathematicaScriptCreator.Instance,
            reducedSystem,
            keepFreeParameters,
            mathematicaBinDir,
            path)[0][0];
    }

    private static string NewCoefficientName(params Tensor[] tensors)
    {
        HashSet<SimpleTensor> simpleTensors = TensorUtils.GetAllSymbols(tensors);
        List<char> forbidden = [];
        foreach (SimpleTensor tensor in simpleTensors)
        {
            string name = TensorCC.GetNameDescriptor(tensor.Name).GetName(tensor.SimpleIndices, OutputFormat.Redberry);
            if (name.Length > 1 && int.TryParse(name[1..], out _))
            {
                forbidden.Add(name[0]);
            }
        }

        forbidden.Sort();
        char candidate = 'a';
        foreach (char forbiddenName in forbidden)
        {
            if (candidate != forbiddenName)
            {
                break;
            }

            candidate++;
        }

        return candidate.ToString();
    }

    private static (Expression[] Equations, SimpleTensor[] UnknownCoefficients, Expression GeneralInverse) Build(
        Expression toInverse,
        Expression equation,
        Tensor[] samples,
        bool symmetricForm,
        ITransformation[] transformations)
    {
        if (equation[0] is not Product leftEq)
        {
            throw new ArgumentException("Equation l.h.s. is not a product of tensors.", nameof(equation));
        }

        SimpleTensor? inverseLhs = null;
        foreach (Tensor tensor in leftEq)
        {
            if (!IndexMappings.MappingExists(tensor, toInverse[0]))
            {
                inverseLhs = (SimpleTensor)tensor;
                break;
            }
        }

        if (inverseLhs is null)
        {
            throw new ArgumentException("Failed to identify inverse tensor on the equation left-hand side.", nameof(equation));
        }

        GeneratedTensor generatedTensor = TensorGenerator.GenerateStructure(inverseLhs.SimpleIndices, samples, symmetricForm, true, true);
        SimpleTensor[] unknownCoefficients = generatedTensor.Coefficients;
        Expression generalInverse = TensorOps.Expression(inverseLhs, generatedTensor.Tensor);

        Tensor temp = equation;
        temp = toInverse.Transform(temp);
        temp = generalInverse.Transform(temp);

        var allTransformations = new ITransformation[transformations.Length + 1];
        allTransformations[0] = EliminateMetricsTransformation.Instance;
        Array.Copy(transformations, 0, allTransformations, 1, transformations.Length);

        temp = ExpandTransformation.Expand(temp, allTransformations);
        foreach (ITransformation transformation in allTransformations)
        {
            temp = transformation.Transform(temp);
        }

        temp = CollectNonScalarsITransformation.CollectNonScalars(temp);
        equation = (Expression)temp;

        List<Split> rightSplit = [];
        if (equation[1] is Sum)
        {
            foreach (Tensor summand in equation[1])
            {
                rightSplit.Add(Split.SplitScalars(summand));
            }
        }
        else
        {
            rightSplit.Add(Split.SplitScalars(equation[1]));
        }

        List<Expression> equationsList = [];
        foreach (Tensor summand in equation[0])
        {
            Split current = Split.SplitScalars(summand);
            bool matched = false;
            foreach (Split split in rightSplit)
            {
                if (TensorUtils.Equals(current.Factor, split.Factor))
                {
                    equationsList.Add(TensorOps.Expression(current.Summand, split.Summand));
                    matched = true;
                    break;
                }
            }

            if (!matched)
            {
                equationsList.Add(TensorOps.Expression(current.Summand, Complex.Zero));
            }
        }

        return (equationsList.ToArray(), unknownCoefficients, generalInverse);
    }
}
