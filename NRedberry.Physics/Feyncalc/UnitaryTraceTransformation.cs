using System.Text.RegularExpressions;
using NRedberry;
using NRedberry.Contexts;
using NRedberry.Graphs;
using NRedberry.Indices;
using NRedberry.Numbers;
using NRedberry.Parsers;
using NRedberry.Parsers.Preprocessor;
using NRedberry.Tensors;
using NRedberry.Tensors.Iterators;
using NRedberry.Transformations;
using NRedberry.Transformations.Symmetrization;
using Complex = NRedberry.Numbers.Complex;
using TensorApi = NRedberry.Tensors.Tensors;
using TensorCC = NRedberry.Tensors.CC;

namespace NRedberry.Physics.Feyncalc;

/// <summary>
/// Port of cc.redberry.physics.feyncalc.UnitaryTraceTransformation.
/// </summary>
public sealed class UnitaryTraceTransformation : TransformationToStringAble
{
    private static readonly ParseToken PairProductToken;

    private static readonly ParseToken SingleTraceToken;

    private static Regex DimensionPattern { get; } = new(@"\bN\b", RegexOptions.Compiled);

    private ITransformation UnitarySimplifications { get; }

    private Expression PairProduct { get; }

    private Expression SingleTrace { get; }

    private int UnitaryMatrix { get; }

    private IndexType MatrixType { get; }

    static UnitaryTraceTransformation()
    {
        var parser = TensorCC.Current.ParseManager.Parser;
        PairProductToken = parser.Parse("T_a^a'_c'*T_b^c'_b' = 1/(2*N)*g_ab*d^a'_b' + I/2*F_abc*T^ca'_b' + 1/2*D_abc*T^ca'_b'");
        SingleTraceToken = parser.Parse("T_a^a'_a' = 0");
    }

    public UnitaryTraceTransformation(UnitarySimplifyOptions options)
        : this(options.UnitaryMatrix, options.StructureConstant, options.SymmetricConstant, options.Dimension)
    {
    }

    public UnitaryTraceTransformation(SimpleTensor unitaryMatrix, SimpleTensor structureConstant, SimpleTensor symmetricConstant, Tensor dimension)
    {
        TraceUtils.CheckUnitaryInput(unitaryMatrix, structureConstant, symmetricConstant, dimension);

        UnitaryMatrix = unitaryMatrix.Name;
        IndexType[] types = TraceUtils.ExtractTypesFromMatrix(unitaryMatrix);
        MatrixType = types[1];

        var tokenTransformer = new ChangeIndicesTypesAndTensorNames(
            new UnitaryTypesAndNamesTransformer(
                types[0],
                types[1],
                unitaryMatrix,
                structureConstant,
                symmetricConstant,
                dimension));

        var pairProduct = (Expression)tokenTransformer.Transform(PairProductToken).ToTensor();
        PairProduct = InlineNumericDimension(pairProduct, dimension);
        SingleTrace = (Expression)tokenTransformer.Transform(SingleTraceToken).ToTensor();
        UnitarySimplifications = new UnitarySimplifyTransformation(unitaryMatrix, structureConstant, symmetricConstant, dimension);
    }

    public Tensor Transform(Tensor tensor)
    {
        ArgumentNullException.ThrowIfNull(tensor);

        var iterator = new FromChildToParentIterator(tensor);
        Tensor? current;
        while ((current = iterator.Next()) != null)
        {
            if (current is SimpleTensor simpleTensor)
            {
                if (simpleTensor.Name == UnitaryMatrix
                    && simpleTensor.Indices.GetOfType(MatrixType).GetFree().Size() == 0)
                {
                    iterator.Set(Complex.Zero);
                }
            }
            else if (current is Product product)
            {
                ProductContent productContent = product.Content;
                PrimitiveSubgraph[] subgraphs = PrimitiveSubgraphPartition.CalculatePartition(productContent, MatrixType);
                if (subgraphs.Length == 0)
                {
                    continue;
                }

                List<int> positionsOfMatrices = [];
                TensorBuilder calculatedTraces = new ScalarsBackedProductBuilder();
                int sizeOfIndexless = GetIndexlessOffset(product);

                foreach (PrimitiveSubgraph subgraph in subgraphs)
                {
                    if (subgraph.GraphType != GraphType.Cycle)
                    {
                        continue;
                    }

                    int[] partition = subgraph.Partition;
                    bool allUnitaryMatrices = true;
                    for (int i = partition.Length - 1; i >= 0; --i)
                    {
                        partition[i] += sizeOfIndexless;
                        if (!IsUnitaryMatrixOrOne(product[partition[i]], UnitaryMatrix))
                        {
                            allUnitaryMatrices = false;
                            break;
                        }
                    }

                    if (!allUnitaryMatrices)
                    {
                        continue;
                    }

                    calculatedTraces.Put(TraceOfProduct(product.Select(partition)));
                    positionsOfMatrices.AddRange(partition);
                }

                Tensor updated = product.Remove([.. positionsOfMatrices]);
                updated = TensorApi.Multiply(updated, calculatedTraces.Build());
                iterator.Set(UnitarySimplifications.Transform(updated));
            }
        }

        return UnitarySimplifications.Transform(iterator.Result());
    }

    public string ToString(OutputFormat outputFormat)
    {
        _ = outputFormat;
        return "UnitaryTrace";
    }

    public override string ToString()
    {
        return ToString(TensorCC.GetDefaultOutputFormat());
    }

    private Tensor TraceOfProduct(Tensor tensor)
    {
        Tensor oldTensor = tensor;
        while (true)
        {
            Tensor newTensor = oldTensor;
            newTensor = UnitarySimplifications.Transform(newTensor);
            newTensor = SingleTrace.Transform(newTensor);
            newTensor = PairProduct.Transform(newTensor);
            newTensor = ExpandAndEliminateTransformation.ExpandAndEliminate(newTensor);
            if (ReferenceEquals(newTensor, oldTensor))
            {
                return newTensor;
            }

            oldTensor = newTensor;
        }
    }

    private static bool IsUnitaryMatrixOrOne(Tensor tensor, int unitaryMatrix)
    {
        if (tensor is SimpleTensor simpleTensor)
        {
            int name = simpleTensor.Name;
            return name == unitaryMatrix || TensorCC.NameManager.IsKroneckerOrMetric(name);
        }

        return false;
    }

    private static void CheckUnitaryInput(SimpleTensor unitaryMatrix, SimpleTensor structureConstant, SimpleTensor symmetricConstant, Tensor dimension)
    {
        TraceUtils.CheckUnitaryInput(unitaryMatrix, structureConstant, symmetricConstant, dimension);
    }

    private static Expression InlineNumericDimension(Expression expression, Tensor dimension)
    {
        if (dimension is not Complex)
        {
            return expression;
        }

        string expressionText = expression.ToString(OutputFormat.Redberry);
        string dimensionText = dimension.ToString(OutputFormat.Redberry);
        string substituted = DimensionPattern.Replace(expressionText, dimensionText);
        return TensorApi.ParseExpression(substituted);
    }

    private static int GetIndexlessOffset(Product product)
    {
        return product.IndexlessData.Length + (product.Factor == Complex.One ? 0 : 1);
    }
}

file sealed record UnitaryTypesAndNamesTransformer(
    IndexType MetricType,
    IndexType MatrixType,
    SimpleTensor UnitaryMatrix,
    SimpleTensor StructureConstant,
    SimpleTensor SymmetricConstant,
    Tensor Dimension)
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
            TraceUtils.UnitaryMatrixName => UnitaryMatrix.GetStringName(),
            TraceUtils.StructureConstantName => StructureConstant.GetStringName(),
            TraceUtils.SymmetricConstantName => SymmetricConstant.GetStringName(),
            TraceUtils.DimensionName when Dimension is not Complex => Dimension.ToString(OutputFormat.Redberry),
            _ => descriptor.Name
        };
    }
}
