using NRedberry;
using NRedberry.Indices;
using NRedberry.Tensors;

namespace NRedberry.Utils;

/// <summary>
/// Skeleton port of cc.redberry.core.utils.MatrixUtils.
/// </summary>
public static class MatrixUtils
{
    public static bool IsGeneralizedMatrix(SimpleTensor tensor, IndexType type, int upper, int lower)
    {
        ArgumentNullException.ThrowIfNull(tensor);
        if (CC.IsMetric(type))
        {
            throw new ArgumentException("Matrices can not be of metric type.", nameof(type));
        }

        SimpleIndices indices = (SimpleIndices)tensor.Indices.GetOfType(type);
        int i = 0;
        for (; i < upper; ++i)
        {
            if (!IndicesUtils.GetState(indices[i]))
            {
                return false;
            }
        }

        upper += lower;
        for (; i < upper; ++i)
        {
            if (IndicesUtils.GetState(indices[i]))
            {
                return false;
            }
        }

        return true;
    }

    public static bool IsMatrix(SimpleTensor tensor, IndexType type)
    {
        return IsGeneralizedMatrix(tensor, type, 1, 1);
    }

    public static bool IsVector(SimpleTensor tensor, IndexType type)
    {
        return IsGeneralizedMatrix(tensor, type, 1, 0);
    }

    public static bool IsCovector(SimpleTensor tensor, IndexType type)
    {
        return IsGeneralizedMatrix(tensor, type, 0, 1);
    }
}
