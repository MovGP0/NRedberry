using NRedberry;
using NRedberry.Core.Utils;
using NRedberry.Graphs;
using NRedberry.Indices;
using NRedberry.Numbers;
using NRedberry.Tensors;

namespace NRedberry.Physics.Feyncalc;

/// <summary>
/// Skeleton port of cc.redberry.physics.feyncalc.ProductOfGammas.
/// </summary>
public sealed class ProductOfGammas
{
    public int Offset { get; }

    public int Length { get; }

    public ProductContent ProductContent { get; }

    public List<int> GPositions { get; }

    public List<int> G5Positions { get; }

    public GraphType GraphType { get; }

    public ProductOfGammas(int offset, ProductContent productContent, List<int> gPositions, List<int> g5Positions, GraphType graphType)
    {
        ArgumentNullException.ThrowIfNull(productContent);
        ArgumentNullException.ThrowIfNull(gPositions);
        ArgumentNullException.ThrowIfNull(g5Positions);

        Offset = offset;
        ProductContent = productContent;
        GPositions = gPositions;
        G5Positions = g5Positions;
        GraphType = graphType;
        Length = gPositions.Count;
    }

    public Tensor[] ToArray()
    {
        Tensor[] array = new Tensor[GPositions.Count];
        for (int i = 0; i < GPositions.Count; ++i)
        {
            array[i] = ProductContent[GPositions[i]];
        }

        return array;
    }

    public Tensor ToProduct()
    {
        ScalarsBackedProductBuilder builder = new();
        for (int i = 0; i < GPositions.Count; ++i)
        {
            builder.Put(ProductContent[GPositions[i]]);
        }

        return builder.Build();
    }

    public IList<Tensor> ToList()
    {
        List<Tensor> result = new(GPositions.Count);
        for (int i = 0; i < GPositions.Count; ++i)
        {
            result.Add(ProductContent[GPositions[i]]);
        }

        return result;
    }

    public Indices.Indices GetIndices()
    {
        IndicesBuilder indices = new();
        for (int i = 0; i < Length; ++i)
        {
            indices.Append(ProductContent[GPositions[i]]);
        }

        return indices.Indices;
    }

    public sealed class It
    {
        private static IIndicator<GraphType> DefaultFilter { get; } = new ExcludeGraphIndicator();

        private Product Product { get; }

        private ProductContent Content { get; }

        private IndexType MatrixType { get; }

        private PrimitiveSubgraph[] Partition { get; }

        private int GammaName { get; }

        private int Gamma5Name { get; }

        private IIndicator<GraphType> Filter { get; }

        private int _partitionIndex;

        private int _subgraphPosition;

        public It(int gammaName, int gamma5Name, Product product, IndexType matrixType, IIndicator<GraphType>? filter)
        {
            ArgumentNullException.ThrowIfNull(product);

            GammaName = gammaName;
            Gamma5Name = gamma5Name;
            Product = product;
            Content = product.Content;
            MatrixType = matrixType;
            Partition = PrimitiveSubgraphPartition.CalculatePartition(Content, matrixType);
            Filter = filter ?? DefaultFilter;
        }

        public ProductOfGammas? Take()
        {
            if (_partitionIndex == Partition.Length)
            {
                return null;
            }

            for (; _partitionIndex < Partition.Length; ++_partitionIndex)
            {
                if (Filter.Is(Partition[_partitionIndex].GraphType))
                {
                    break;
                }
            }

            if (_partitionIndex == Partition.Length)
            {
                return null;
            }

            PrimitiveSubgraph currentSubgraph = Partition[_partitionIndex];
            List<int> gPositions = [];
            List<int> g5Positions = [];

            for (;; ++_subgraphPosition)
            {
                if (_subgraphPosition == currentSubgraph.Size)
                {
                    ++_partitionIndex;
                    _subgraphPosition = 0;
                    break;
                }

                int position = currentSubgraph.GetPosition(_subgraphPosition);
                Tensor tensor = Content[position];
                if (IsGamma(tensor))
                {
                    gPositions.Add(position);
                }
                else if (IsGamma5(tensor))
                {
                    g5Positions.Add(gPositions.Count);
                    gPositions.Add(position);
                }
                else if (gPositions.Count != 0 || g5Positions.Count != 0)
                {
                    break;
                }
            }

            if (gPositions.Count == 0 && g5Positions.Count == 0)
            {
                return Take();
            }

            if (currentSubgraph.GraphType == GraphType.Cycle
                && g5Positions.Count == 1
                && gPositions.Count == currentSubgraph.Size
                && g5Positions[0] != currentSubgraph.Size - 1)
            {
                int g5Position = g5Positions[0];
                int size = currentSubgraph.Size;
                List<int> reordered = new(size);
                for (int i = g5Position + 1; i < size; ++i)
                {
                    reordered.Add(currentSubgraph.GetPosition(i));
                }

                for (int i = 0; i <= g5Position; ++i)
                {
                    reordered.Add(currentSubgraph.GetPosition(i));
                }

                gPositions = reordered;
                g5Positions[0] = size - 1;
            }

            GraphType graphType = GraphType.Line;
            if (currentSubgraph.GraphType == GraphType.Cycle
                && gPositions.Count == currentSubgraph.Size)
            {
                graphType = GraphType.Cycle;
            }

            if (!Filter.Is(graphType))
            {
                return Take();
            }

            return new ProductOfGammas(
                GetIndexlessOffset(Product),
                Content,
                gPositions,
                g5Positions,
                graphType);
        }

        private bool IsGamma(Tensor tensor)
        {
            return tensor.GetHashCode() == GammaName && tensor.GetType() == typeof(SimpleTensor);
        }

        private bool IsGamma5(Tensor tensor)
        {
            return tensor.GetHashCode() == Gamma5Name && tensor.GetType() == typeof(SimpleTensor);
        }

        private static int GetIndexlessOffset(Product product)
        {
            return product.IndexlessData.Length + (product.Factor == Complex.One ? 0 : 1);
        }
    }
}

internal sealed class ExcludeGraphIndicator : IIndicator<GraphType>
{
    public bool Is(GraphType @object)
    {
        return @object != GraphType.Graph;
    }
}
