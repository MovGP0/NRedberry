using NRedberry.Core.Graphs;
using NRedberry.Core.Indices;
using NRedberry.Core.Tensors;
using NRedberry.Core.Utils;

namespace NRedberry.Physics.Feyncalc;

/// <summary>
/// Skeleton port of cc.redberry.physics.feyncalc.ProductOfGammas.
/// </summary>
public sealed class ProductOfGammas
{
    public int Offset => throw new NotImplementedException();
    public int Length => throw new NotImplementedException();
    public ProductContent ProductContent => throw new NotImplementedException();
    public List<int> GPositions => throw new NotImplementedException();
    public List<int> G5Positions => throw new NotImplementedException();
    public GraphType GraphType => throw new NotImplementedException();

    public ProductOfGammas(int offset, ProductContent productContent, List<int> gPositions, List<int> g5Positions, GraphType graphType)
    {
        throw new NotImplementedException();
    }

    public Tensor[] ToArray()
    {
        throw new NotImplementedException();
    }

    public Tensor ToProduct()
    {
        throw new NotImplementedException();
    }

    public IList<Tensor> ToList()
    {
        throw new NotImplementedException();
    }

    public Indices GetIndices()
    {
        throw new NotImplementedException();
    }

    public sealed class It
    {
        public It(int gammaName, int gamma5Name, Product product, IndexType matrixType, IIndicator<GraphType>? filter)
        {
            throw new NotImplementedException();
        }

        public ProductOfGammas? Take()
        {
            throw new NotImplementedException();
        }
    }
}
