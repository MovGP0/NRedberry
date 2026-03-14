using NRedberry;
using NRedberry.Contexts;
using NRedberry.Indices;
using NRedberry.Parsers.Preprocessor;
using NRedberry.Tensors;
using NRedberry.Transformations;
using NRedberry.Transformations.Substitutions;
using NRedberry.Transformations.Symmetrization;
using TensorApi = NRedberry.Tensors.Tensors;
using TensorCC = NRedberry.Tensors.CC;

namespace NRedberry.Physics.Feyncalc;

/// <summary>
/// Port of cc.redberry.physics.feyncalc.AbstractTransformationWithGammas.
/// </summary>
public abstract class AbstractTransformationWithGammas : TransformationToStringAble
{
    protected const string GammaMatrixStringName = "G";
    protected const string Gamma5StringName = "G5";
    protected const string LeviCivitaStringName = "eps";

    private SubstitutionTransformation? _deltaTraces;

    protected AbstractTransformationWithGammas(DiracOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);

        CheckNotation(options.GammaMatrix, options.Gamma5, options.LeviCivita);
        if (!options.Created)
        {
            options.TriggerCreate();
        }

        GammaName = options.GammaMatrix.Name;
        Gamma5Name = options.Gamma5 is null ? int.MinValue : options.Gamma5.Name;

        IndexType[] types = TraceUtils.ExtractTypesFromMatrix(options.GammaMatrix);
        MetricType = types[0];
        MatrixType = types[1];

        TokenTransformer = CreateTokenTransformer(
            MetricType,
            MatrixType,
            options.GammaMatrix,
            options.Gamma5,
            options.LeviCivita);

        ExpandAndEliminate = options.ExpandAndEliminate;

        string traceOfOneExpression = "d^a'_a'=" + options.TraceOfOne.ToString(OutputFormat.Redberry);
        TraceOfOne = (Expression)TensorApi.Parse(traceOfOneExpression, TokenTransformer);

        string deltaTraceExpression = "d^a_a=" + options.Dimension.ToString(OutputFormat.Redberry);
        DeltaTrace = (Expression)TensorApi.Parse(deltaTraceExpression, TokenTransformer);
    }

    public override string ToString()
    {
        return ToString(TensorCC.GetDefaultOutputFormat());
    }

    public string ToString(OutputFormat outputFormat)
    {
        _ = outputFormat;
        return GetType().Name;
    }

    protected ITransformation ExpandAndEliminate { get; }

    protected Expression TraceOfOne { get; }

    protected Expression DeltaTrace { get; }

    protected SubstitutionTransformation DeltaTraces => _deltaTraces ??= new SubstitutionTransformation(DeltaTrace);

    protected ChangeIndicesTypesAndTensorNames TokenTransformer { get; }

    protected int GammaName { get; }

    protected int Gamma5Name { get; }

    protected IndexType MetricType { get; }

    protected IndexType MatrixType { get; }

    public Tensor Transform(Tensor t)
    {
        throw new NotImplementedException();
    }

    protected bool ContainsGammaOr5Matrices(Tensor tensor)
    {
        ArgumentNullException.ThrowIfNull(tensor);

        if (tensor.GetType() == typeof(SimpleTensor))
        {
            int hash = tensor.GetHashCode();
            return hash == GammaName || hash == Gamma5Name;
        }

        foreach (Tensor child in tensor)
        {
            if (ContainsGammaOr5Matrices(child))
            {
                return true;
            }
        }

        return false;
    }

    protected bool ContainsGammaMatrices(Tensor tensor)
    {
        ArgumentNullException.ThrowIfNull(tensor);

        if (tensor.GetType() == typeof(SimpleTensor))
        {
            return tensor.GetHashCode() == GammaName;
        }

        foreach (Tensor child in tensor)
        {
            if (ContainsGammaMatrices(child))
            {
                return true;
            }
        }

        return false;
    }

    protected bool IsGammaOrGamma5(Tensor tensor)
    {
        ArgumentNullException.ThrowIfNull(tensor);

        int hash = tensor.GetHashCode();
        return (hash == GammaName || hash == Gamma5Name) && tensor.GetType() == typeof(SimpleTensor);
    }

    protected bool IsGamma(Tensor tensor)
    {
        ArgumentNullException.ThrowIfNull(tensor);

        return tensor.GetHashCode() == GammaName && tensor.GetType() == typeof(SimpleTensor);
    }

    protected bool IsGamma5(Tensor tensor)
    {
        ArgumentNullException.ThrowIfNull(tensor);

        return tensor.GetHashCode() == Gamma5Name && tensor.GetType() == typeof(SimpleTensor);
    }

    protected SimpleTensor SetUpperMatrixIndex(SimpleTensor gamma, int matrixUpper)
    {
        ArgumentNullException.ThrowIfNull(gamma);

        int matrixLower = gamma.Indices.GetOfType(MatrixType).LowerIndices[0];
        return SetMatrixIndices(gamma, matrixUpper, matrixLower);
    }

    protected SimpleTensor SetLowerMatrixIndex(SimpleTensor gamma, int matrixLower)
    {
        ArgumentNullException.ThrowIfNull(gamma);

        int matrixUpper = gamma.Indices.GetOfType(MatrixType).UpperIndices[0];
        return SetMatrixIndices(gamma, matrixUpper, matrixLower);
    }

    protected void SwapAdj(Tensor[] gammas, int j)
    {
        ArgumentNullException.ThrowIfNull(gammas);

        Tensor tensor = gammas[j];
        gammas[j] = SetMatrixIndices((SimpleTensor)gammas[j + 1], gammas[j].Indices.GetOfType(MatrixType));
        gammas[j + 1] = SetMatrixIndices((SimpleTensor)tensor, gammas[j + 1].Indices.GetOfType(MatrixType));
    }

    protected Tensor[] CutAdj(Tensor[] original, int i)
    {
        ArgumentNullException.ThrowIfNull(original);

        if (original.Length < 2)
        {
            return original;
        }

        Tensor[] result = new Tensor[original.Length - 2];
        Array.Copy(original, 0, result, 0, i);
        Array.Copy(original, i + 2, result, i, original.Length - i - 2);

        if (result.Length == 0)
        {
            return result;
        }

        int matrixUpper;
        int matrixLower;
        if (i == 0)
        {
            i = 1;
            matrixUpper = original[0].Indices.GetOfType(MatrixType).UpperIndices[0];
            matrixLower = result[i - 1].Indices.GetOfType(MatrixType).LowerIndices[0];
        }
        else if (i == original.Length - 2)
        {
            matrixUpper = result[i - 1].Indices.GetOfType(MatrixType).UpperIndices[0];
            matrixLower = original[original.Length - 1].Indices.GetOfType(MatrixType).LowerIndices[0];
        }
        else
        {
            matrixUpper = result[i - 1].Indices.GetOfType(MatrixType).UpperIndices[0];
            matrixLower = result[i].Indices.GetOfType(MatrixType).UpperIndices[0];
        }

        result[i - 1] = SetMatrixIndices((SimpleTensor)result[i - 1], matrixUpper, matrixLower);
        return result;
    }

    protected Tensor[] CreateLine(int length)
    {
        Tensor[] gammas = new Tensor[length];
        int matrixIndex = 0;

        for (int i = 0; i < length; ++i)
        {
            gammas[i] = TensorApi.SimpleTensor(
                GammaName,
                IndicesFactory.CreateSimple(
                    null,
                    IndicesUtils.CreateIndex(matrixIndex, MatrixType, true),
                    IndicesUtils.CreateIndex(++matrixIndex, MatrixType, false),
                    IndicesUtils.SetType(MetricType, i)));
        }

        return gammas;
    }

    protected Tensor[] Del(Tensor[] arr, int i)
    {
        ArgumentNullException.ThrowIfNull(arr);

        Tensor tensor = arr[i];
        Tensor[] result = new Tensor[arr.Length - 1];
        Array.Copy(arr, 0, result, 0, i);
        Array.Copy(arr, i + 1, result, i, arr.Length - i - 1);

        if (result.Length == 0)
        {
            return result;
        }

        if (i == 0)
        {
            result[0] = SetUpperMatrixIndex(
                (SimpleTensor)result[0],
                tensor.Indices.GetOfType(MatrixType).UpperIndices[0]);
        }
        else if (i == result.Length)
        {
            result[result.Length - 1] = SetLowerMatrixIndex(
                (SimpleTensor)result[result.Length - 1],
                tensor.Indices.GetOfType(MatrixType).LowerIndices[0]);
        }
        else
        {
            result[i] = SetUpperMatrixIndex(
                (SimpleTensor)result[i],
                tensor.Indices.GetOfType(MatrixType).UpperIndices[0]);
        }

        return result;
    }

    protected int Del(List<Tensor> arr, int i)
    {
        ArgumentNullException.ThrowIfNull(arr);

        Tensor tensor = arr[i];
        arr.RemoveAt(i);

        if (arr.Count == 0)
        {
            return tensor.Indices.GetOfType(MatrixType).LowerIndices[0];
        }

        if (i == 0)
        {
            arr[0] = SetUpperMatrixIndex(
                (SimpleTensor)arr[0],
                tensor.Indices.GetOfType(MatrixType).UpperIndices[0]);
            return tensor.Indices.GetOfType(MatrixType).LowerIndices[0];
        }

        if (i == arr.Count)
        {
            arr[arr.Count - 1] = SetLowerMatrixIndex(
                (SimpleTensor)arr[arr.Count - 1],
                tensor.Indices.GetOfType(MatrixType).LowerIndices[0]);
            return tensor.Indices.GetOfType(MatrixType).UpperIndices[0];
        }

        arr[i] = SetUpperMatrixIndex(
            (SimpleTensor)arr[i],
            tensor.Indices.GetOfType(MatrixType).UpperIndices[0]);
        return tensor.Indices.GetOfType(MatrixType).LowerIndices[0];
    }

    protected static SimpleTensor SetMatrixIndices(SimpleTensor gamma, Indices.Indices matrixIndices)
    {
        ArgumentNullException.ThrowIfNull(gamma);
        ArgumentNullException.ThrowIfNull(matrixIndices);

        return SetMatrixIndices(gamma, matrixIndices.UpperIndices[0], matrixIndices.LowerIndices[0]);
    }

    protected static SimpleTensor SetMatrixIndices(SimpleTensor gamma, int matrixUpper, int matrixLower)
    {
        ArgumentNullException.ThrowIfNull(gamma);

        int[] indices = gamma.Indices.AllIndices.ToArray();
        for (int i = indices.Length - 1; i >= 0; --i)
        {
            byte type = IndicesUtils.GetType(indices[i]);
            if (!TensorCC.IsMetric(type))
            {
                indices[i] = IndicesUtils.GetState(indices[i])
                    ? IndicesUtils.CreateIndex(matrixUpper, type, true)
                    : IndicesUtils.CreateIndex(matrixLower, type, false);
            }
        }

        return TensorApi.SimpleTensor(gamma.Name, IndicesFactory.CreateSimple(null, indices));
    }

    protected static SimpleTensor SetMetricIndex(SimpleTensor gamma, int metricIndex)
    {
        ArgumentNullException.ThrowIfNull(gamma);

        int[] indices = gamma.Indices.AllIndices.ToArray();
        for (int i = indices.Length - 1; i >= 0; --i)
        {
            if (TensorCC.IsMetric(IndicesUtils.GetType(indices[i])))
            {
                indices[i] = metricIndex;
            }
        }

        return TensorApi.SimpleTensor(gamma.Name, IndicesFactory.CreateSimple(null, indices));
    }

    protected static Tensor DefaultTraceOfOne()
    {
        return DefaultDimension();
    }

    protected static Tensor DefaultDimension()
    {
        return TensorApi.Parse("4");
    }

    protected static void CheckNotation(SimpleTensor gammaMatrix)
    {
        ArgumentNullException.ThrowIfNull(gammaMatrix);

        IndexType[] types = TraceUtils.ExtractTypesFromMatrix(gammaMatrix);
        IndexType metricType = types[0];
        IndexType matrixType = types[1];

        if (gammaMatrix.Indices.Size() != 3
            || gammaMatrix.Indices.Size(metricType) != 1
            || gammaMatrix.Indices.Size(matrixType) != 2)
        {
            throw new ArgumentException("Not a gamma: " + gammaMatrix, nameof(gammaMatrix));
        }
    }

    protected static void CheckNotation(
        SimpleTensor gammaMatrix,
        SimpleTensor? gamma5Matrix,
        SimpleTensor? leviCivita)
    {
        ArgumentNullException.ThrowIfNull(gammaMatrix);

        IndexType[] types = TraceUtils.ExtractTypesFromMatrix(gammaMatrix);
        IndexType metricType = types[0];
        IndexType matrixType = types[1];

        if (gammaMatrix.Indices.Size() != 3
            || gammaMatrix.Indices.Size(metricType) != 1
            || gammaMatrix.Indices.Size(matrixType) != 2)
        {
            throw new ArgumentException("Not a gamma: " + gammaMatrix, nameof(gammaMatrix));
        }

        if (gamma5Matrix is not null
            && (gamma5Matrix.Indices.Size() != 2
                || gamma5Matrix.Indices.Size(matrixType) != 2))
        {
            throw new ArgumentException("Not a gamma5: " + gamma5Matrix, nameof(gamma5Matrix));
        }

        if (leviCivita is not null
            && (leviCivita.Indices.Size() != 4
                || leviCivita.Indices.Size(metricType) != 4))
        {
            throw new ArgumentException("Not a Levi-Civita: " + leviCivita, nameof(leviCivita));
        }
    }

    private static ChangeIndicesTypesAndTensorNames CreateTokenTransformer(
        IndexType metricType,
        IndexType matrixType,
        SimpleTensor gammaMatrix,
        SimpleTensor? gamma5,
        SimpleTensor? leviCivita)
    {
        return new ChangeIndicesTypesAndTensorNames(
            new GammaTypesAndNamesTransformer(metricType, matrixType, gammaMatrix, gamma5, leviCivita));
    }
}

file sealed record GammaTypesAndNamesTransformer(
    IndexType MetricType,
    IndexType MatrixType,
    SimpleTensor GammaMatrix,
    SimpleTensor? Gamma5,
    SimpleTensor? LeviCivita)
    : TypesAndNamesTransformer
{
    public int NewIndex(int oldIndex, NameAndStructureOfIndices descriptor)
    {
        _ = descriptor;
        return oldIndex;
    }

    public IndexType NewType(IndexType oldType, NameAndStructureOfIndices descriptor)
    {
        _ = descriptor;

        return oldType switch
        {
            IndexType.LatinLower => MetricType,
            IndexType.Matrix1 => MatrixType,
            _ => oldType
        };
    }

    public string NewName(string oldName, NameAndStructureOfIndices descriptor)
    {
        return oldName switch
        {
            "G" => GammaMatrix.GetStringName(),
            "G5" => Gamma5?.GetStringName()
                ?? throw new ArgumentException("Gamma5 is not specified."),
            "eps" => LeviCivita?.GetStringName()
                ?? throw new ArgumentException("Levi-Civita is not specified."),
            _ => descriptor.Name
        };
    }
}
