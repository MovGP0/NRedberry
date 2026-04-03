using NRedberry.Contexts;
using NRedberry.Indices;
using NRedberry.Numbers;
using NRedberry.Tensors;
using ContextCC = NRedberry.Contexts.CC;

namespace NRedberry.Physics.Feyncalc;

/// <summary>
/// Port of cc.redberry.physics.feyncalc.TraceUtils.
/// </summary>
public static class TraceUtils
{
    public const string UnitaryMatrixName = "T";
    public const string StructureConstantName = "F";
    public const string SymmetricConstantName = "D";
    public const string DimensionName = "N";

    public static IndexType[] ExtractTypesFromMatrix(SimpleTensor matrix)
    {
        ArgumentNullException.ThrowIfNull(matrix);

        if (matrix.Indices.Size() != 3)
        {
            throw new ArgumentException($"Not a matrix: {matrix}.");
        }

        NameDescriptor descriptor = ContextCC.GetNameDescriptor(matrix.Name);
        StructureOfIndices typeStructure = descriptor.GetStructureOfIndices();
        byte metricType = byte.MaxValue;
        byte matrixType = byte.MaxValue;

        for (byte type = 0; type < IndexTypeMethods.TypesCount; ++type)
        {
            int typeCount = typeStructure.TypeCount(type);
            if (typeCount == 0)
            {
                continue;
            }

            if (typeCount == 2)
            {
                if (matrixType != byte.MaxValue || ContextCC.IsMetric(type))
                {
                    throw new ArgumentException($"Not a matrix: {matrix}.");
                }

                matrixType = type;
                continue;
            }

            if (typeCount == 1)
            {
                if (metricType != byte.MaxValue || !ContextCC.IsMetric(type))
                {
                    throw new ArgumentException($"Not a matrix: {matrix}.");
                }

                metricType = type;
                continue;
            }

            throw new ArgumentException($"Not a matrix: {matrix}.");
        }

        if (metricType == byte.MaxValue || matrixType == byte.MaxValue)
        {
            throw new ArgumentException($"Not a matrix: {matrix}.");
        }

        return [IndexTypeMethods.GetType(metricType), IndexTypeMethods.GetType(matrixType)];
    }

    public static void CheckUnitaryInput(
        SimpleTensor unitaryMatrix,
        SimpleTensor structureConstant,
        SimpleTensor symmetricConstant,
        Tensor dimension)
    {
        ArgumentNullException.ThrowIfNull(unitaryMatrix);
        ArgumentNullException.ThrowIfNull(structureConstant);
        ArgumentNullException.ThrowIfNull(symmetricConstant);
        ArgumentNullException.ThrowIfNull(dimension);

        if (dimension is Complex && !TensorUtils.IsNaturalNumber(dimension))
        {
            throw new ArgumentException("Non natural degree.");
        }

        if (unitaryMatrix.Indices.Size() != 3)
        {
            throw new ArgumentException($"Not a unitary matrix: {unitaryMatrix}");
        }

        IndexType metricType = ExtractTypesFromMatrix(unitaryMatrix)[0];
        if (!TensorUtils.IsScalar(dimension))
        {
            throw new ArgumentException("Non scalar degree.");
        }

        if (structureConstant.Name == symmetricConstant.Name)
        {
            throw new ArgumentException("Structure and symmetric constants have same names.");
        }

        foreach (SimpleTensor tensor in new[] { structureConstant, symmetricConstant })
        {
            if (tensor.Indices.Size() != 3)
            {
                throw new ArgumentException($"Illegal input for SU(N) constants: {tensor}");
            }

            for (int i = 0; i < 3; ++i)
            {
                if (IndicesUtils.GetTypeEnum(tensor.Indices[i]) != metricType)
                {
                    throw new ArgumentException(
                        $"Different indices metric types: {unitaryMatrix} and {tensor}");
                }
            }
        }
    }
}
