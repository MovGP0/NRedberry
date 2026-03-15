using NRedberry;
using NRedberry.Contexts;
using NRedberry.IndexMapping;
using NRedberry.Indices;
using NRedberry.Numbers;
using NRedberry.TensorGenerators;
using NRedberry.Tensors;
using NRedberry.Transformations;
using NRedberry.Transformations.Collect;
using NRedberry.Transformations.Symmetrization;
using TensorFactory = NRedberry.Tensors.Tensors;

namespace NRedberry.Physics.Feyncalc;

public static class PassarinoVeltman
{
    public static Expression GenerateSubstitution(int order, SimpleTensor loopMomentum, SimpleTensor[] externalMomentums)
    {
        return GenerateSubstitution(order, loopMomentum, externalMomentums, Transformation.Identity);
    }

    public static Expression GenerateSubstitution(int order, SimpleTensor loopMomentum, SimpleTensor[] externalMomentums, ITransformation simplifications)
    {
        ArgumentNullException.ThrowIfNull(loopMomentum);
        ArgumentNullException.ThrowIfNull(externalMomentums);
        ArgumentNullException.ThrowIfNull(simplifications);

        if (order == 0)
        {
            throw new ArgumentException("Zero order", nameof(order));
        }

        Check(loopMomentum, externalMomentums);

        byte indexType = IndicesUtils.GetType(loopMomentum.Indices[0]);
        int[] indicesArray = new int[order];
        for (int i = 0; i < order; i++)
        {
            indicesArray[i] = IndicesUtils.SetType(indexType, i);
        }

        SimpleIndices indices = IndicesFactory.CreateSimple(null, indicesArray);
        indices = IndicesFactory.CreateSimple(
            IndicesSymmetries.Create(indices.StructureOfIndices),
            indicesArray);

        Tensor loopProduct = Complex.One;
        for (int i = 0; i < indices.Size(); ++i)
        {
            Tensor currentLoopMomentum = ApplyIndexMapping.Apply(
                loopMomentum,
                new Mapping([loopMomentum.Indices[0]], [indicesArray[i]]));
            loopProduct = TensorFactory.Multiply(loopProduct, currentLoopMomentum);
        }

        Tensor[] samples = new Tensor[externalMomentums.Length + 1];
        Array.Copy(externalMomentums, samples, externalMomentums.Length);
        samples[^1] = Context.Get().CreateMetric(
            IndicesUtils.SetType(indexType, 0),
            IndicesUtils.SetType(indexType, 1));

        GeneratedTensor generatedTensor = TensorGenerator.GenerateStructure(indices, samples, true, true, true);
        SimpleTensor[] parameters = generatedTensor.Coefficients;
        Tensor expression = generatedTensor.Tensor;

        expression = new CollectTransformation(parameters).Transform(expression);
        Tensor[] coefficients = CoefficientsList(expression, parameters);

        Tensor[][] matrix = new Tensor[parameters.Length][];
        Tensor[] rhs = new Tensor[parameters.Length];
        for (int i = 0; i < parameters.Length; ++i)
        {
            matrix[i] = new Tensor[parameters.Length];
        }

        for (int j = 0; j < parameters.Length; ++j)
        {
            Tensor inverted = ApplyIndexMapping.InvertIndices(coefficients[j]);
            for (int i = 0; i < parameters.Length; ++i)
            {
                matrix[i][j] = ExpandAndEliminateTransformation.ExpandAndEliminate(
                    TensorFactory.MultiplyAndRenameConflictingDummies(coefficients[i], inverted),
                    simplifications);
                matrix[i][j] = simplifications.Transform(matrix[i][j]);
            }

            rhs[j] = ExpandAndEliminateTransformation.ExpandAndEliminate(
                TensorFactory.MultiplyAndRenameConflictingDummies(loopProduct, inverted));
        }

        Tensor[][] inverse = TensorUtils.Inverse(matrix);
        SumBuilder solution = new();
        for (int j = 0; j < parameters.Length; ++j)
        {
            SumBuilder partialSolution = new();
            for (int i = 0; i < parameters.Length; i++)
            {
                partialSolution.Put(TensorFactory.MultiplyAndRenameConflictingDummies(inverse[i][j], rhs[i]));
            }

            solution.Put(TensorFactory.MultiplyAndRenameConflictingDummies(partialSolution.Build(), coefficients[j]));
        }

        return TensorFactory.Expression(loopProduct, solution.Build());
    }

    private static void Check(SimpleTensor loopMomentum, SimpleTensor[] externalMomentums)
    {
        Check(null, loopMomentum);
        IndexType type = IndicesUtils.GetTypeEnum(loopMomentum.Indices[0]);
        foreach (SimpleTensor externalMomentum in externalMomentums)
        {
            Check(type, externalMomentum);
        }
    }

    private static void Check(IndexType? type, SimpleTensor momentum)
    {
        ArgumentNullException.ThrowIfNull(momentum);

        if (momentum.Indices.Size() != 1)
        {
            throw new ArgumentException($"Not a momentum: {momentum}", nameof(momentum));
        }

        if (type is not null && type != IndicesUtils.GetTypeEnum(momentum.Indices[0]))
        {
            throw new ArgumentException($"Not a momentum: {momentum} wrong index type", nameof(momentum));
        }
    }

    private static Tensor[] CoefficientsList(Tensor tensor, SimpleTensor[] coefficients)
    {
        ArgumentNullException.ThrowIfNull(tensor);
        ArgumentNullException.ThrowIfNull(coefficients);

        bool hasSymbolic = false;
        foreach (SimpleTensor coefficient in coefficients)
        {
            if (coefficient.Indices.Size() == 0)
            {
                hasSymbolic = true;
                break;
            }
        }

        tensor = new CollectTransformation(coefficients, hasSymbolic).Transform(tensor);

        Tensor[] result = new Tensor[coefficients.Length];
        Array.Fill(result, Complex.Zero);

        if (tensor is Product)
        {
            Monomial? monomial = GetFromProduct(tensor, coefficients);
            if (monomial is not null)
            {
                result[monomial.Index] = monomial.Coefficient;
            }
        }
        else
        {
            foreach (Tensor term in tensor)
            {
                Monomial? monomial = GetFromProduct(term, coefficients);
                if (monomial is not null)
                {
                    result[monomial.Index] = monomial.Coefficient;
                }
            }
        }

        return result;
    }

    private static Monomial? GetFromProduct(Tensor tensor, SimpleTensor[] coefficients)
    {
        ArgumentNullException.ThrowIfNull(tensor);
        ArgumentNullException.ThrowIfNull(coefficients);

        if (tensor is not Product product)
        {
            return null;
        }

        for (int j = 0; j < tensor.Size; j++)
        {
            if (tensor[j] is not SimpleTensor)
            {
                continue;
            }

            for (int i = 0; i < coefficients.Length; i++)
            {
                if (tensor[j].Equals(coefficients[i]))
                {
                    return new Monomial(product.Remove(j), i);
                }
            }
        }

        return null;
    }

    private sealed record Monomial(Tensor Coefficient, int Index);
}
