using NRedberry.Core.Tensors;
using NRedberry.Core.Utils;

namespace NRedberry.Physics.Feyncalc;

/// <summary>
/// Skeleton port of cc.redberry.physics.feyncalc.DiracOrderTransformation.
/// </summary>
public sealed class DiracOrderTransformation : AbstractFeynCalcTransformation
{
    private readonly Dictionary<IntArray, Cached> _cache = new();

    public DiracOrderTransformation(DiracOptions options)
        : base(options, null)
    {
        throw new NotImplementedException();
    }

    protected override Tensor? TransformLine(ProductOfGammas productOfGammas, IntArrayList modifiedElements)
    {
        throw new NotImplementedException();
    }

    public string ToString(OutputFormat outputFormat)
    {
        throw new NotImplementedException();
    }

    private Tensor? GetContraction(int gamma, ProductContent productContent, StructureOfContractions structure)
    {
        throw new NotImplementedException();
    }

    private Tensor[] CreateArray(int[] permutation)
    {
        throw new NotImplementedException();
    }

    private Tensor? OrderArray(Tensor[] gammas)
    {
        throw new NotImplementedException();
    }

    private Tensor OrderArray0(Tensor[] gammas)
    {
        throw new NotImplementedException();
    }

    private sealed class Cached
    {
        public Cached(Tensor[] originalArray, Tensor ordered)
        {
            throw new NotImplementedException();
        }

        public Tensor[] OriginalArray => throw new NotImplementedException();

        public Tensor Ordered => throw new NotImplementedException();

        public int[] GetOriginalIndices(IndexType metricType)
        {
            throw new NotImplementedException();
        }
    }

    private sealed class Gamma : IComparable<Gamma>
    {
        public Gamma(Tensor gamma, int index, Tensor? contraction)
        {
            throw new NotImplementedException();
        }

        public Tensor GammaTensor => throw new NotImplementedException();

        public int Index => throw new NotImplementedException();

        public Tensor? Contraction => throw new NotImplementedException();

        public bool Contracted()
        {
            throw new NotImplementedException();
        }

        public int CompareTo(Gamma? other)
        {
            throw new NotImplementedException();
        }
    }
}
