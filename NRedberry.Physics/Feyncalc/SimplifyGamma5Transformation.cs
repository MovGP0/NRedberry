using NRedberry.Contexts;
using NRedberry.Core.Utils;
using NRedberry.Graphs;
using NRedberry.Indices;
using NRedberry.Tensors;
using NRedberry.Transformations;
using TensorApi = NRedberry.Tensors.Tensors;

namespace NRedberry.Physics.Feyncalc;

/// <summary>
/// Skeleton port of cc.redberry.physics.feyncalc.SimplifyGamma5Transformation.
/// </summary>
public sealed class SimplifyGamma5Transformation : AbstractFeynCalcTransformation
{
    public SimplifyGamma5Transformation(DiracOptions options)
        : base(PrepareOptions(options), Transformation.Identity)
    {
    }

    public string ToString(OutputFormat outputFormat)
    {
        return "SimplifyGamma5";
    }

    protected override Tensor? TransformLine(ProductOfGammas productOfGammas, List<int> modifiedElements)
    {
        ArgumentNullException.ThrowIfNull(productOfGammas);
        ArgumentNullException.ThrowIfNull(modifiedElements);

        if (productOfGammas.Length == 1)
        {
            return null;
        }

        if (productOfGammas.G5Positions.Count == 0)
        {
            return null;
        }

        if (productOfGammas.G5Positions.Count == 1)
        {
            if (productOfGammas.GraphType == GraphType.Cycle)
            {
                return null;
            }

            if (productOfGammas.G5Positions[0] == productOfGammas.Length - 1)
            {
                return null;
            }
        }

        if (productOfGammas.G5Positions.Count == productOfGammas.Length)
        {
            if (productOfGammas.Length % 2 == 0)
            {
                if (productOfGammas.GraphType == GraphType.Cycle)
                {
                    return TraceOfOne[1];
                }

                Indices.Indices freeIndices = productOfGammas.GetIndices().GetFree();
                return Context.Get().CreateMetricOrKronecker(freeIndices[0], freeIndices[1]);
            }

            SimpleTensor gamma5 = (SimpleTensor)productOfGammas.ProductContent[productOfGammas.GPositions[0]];
            return SetMatrixIndices(gamma5, productOfGammas.GetIndices().GetFree());
        }

        return SimplifyProduct(productOfGammas.ToList());
    }

    private Tensor SimplifyProduct(IList<Tensor> gammas)
    {
        ArgumentNullException.ThrowIfNull(gammas);

        List<Tensor> gammaList = gammas as List<Tensor> ?? [.. gammas];
        int upper = gammaList[0].Indices.GetOfType(MatrixType).UpperIndices[0];
        int lower = gammaList[gammaList.Count - 1].Indices.GetOfType(MatrixType).LowerIndices[0];
        int initialSize = gammaList.Count;
        bool sign = false;
        int dummy = -1;
        for (int i = gammaList.Count - 1; i >= 0; --i)
        {
            if (IsGamma5(gammaList[i]))
            {
                sign ^= ((gammaList.Count - i) % 2) == 0;
                dummy = Del(gammaList, i);
            }
        }

        if ((initialSize - gammaList.Count) % 2 == 1)
        {
            if (gammaList.Count == 0)
            {
                gammaList.Add(TensorApi.SimpleTensor(
                    Gamma5Name,
                    IndicesFactory.CreateSimple(null, upper, lower)));
            }
            else
            {
                Tensor last = gammaList[gammaList.Count - 1];
                gammaList[gammaList.Count - 1] = SetLowerMatrixIndex((SimpleTensor)last, dummy);
                gammaList.Add(TensorApi.SimpleTensor(
                    Gamma5Name,
                    IndicesFactory.CreateSimple(
                        null,
                        IndicesUtils.Raise(dummy),
                        last.Indices.GetOfType(MatrixType).LowerIndices[0])));
            }
        }

        Tensor result = TensorApi.Multiply(gammaList);
        if (sign)
        {
            result = TensorApi.Negate(result);
        }

        return result;
    }

    private static DiracOptions PrepareOptions(DiracOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);

        DiracOptions prepared = options.Clone();
        prepared.ExpandAndEliminate = Transformation.Identity;
        return prepared;
    }
}
