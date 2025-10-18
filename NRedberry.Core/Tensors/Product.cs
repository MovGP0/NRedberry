using NRedberry.Core.Indices;
using NRedberry.Core.Utils;
using Complex = NRedberry.Core.Numbers.Complex;

namespace NRedberry.Core.Tensors;

public sealed class Product : MultiTensor
{
    ///<summary>
    /// Numerical factor.
    ///</summary>
    public Complex Factor { get; }

    ///<summary>
    /// Elements with zero size of indices (without indices).
    ///</summary>
    public Tensor[] IndexlessData { get; }

    ///<summary>
    /// Elements with indices.
    ///</summary>
    public Tensor[] Data { get; }

    ///<summary>
    /// Reference to cached ProductContent object.
    ///</summary>
    private WeakReference<ProductContent> ContentReference { get; }

    ///<summary>
    /// Hash code of this product.
    ///</summary>
    private int hash;

    public Product(Indices.Indices indices, Complex factor, Tensor[] indexless, Tensor[] data) : base(indices)
    {
        Factor = GetDefaultReference(factor);
        IndexlessData = indexless;
        Data = data;

        Array.Sort(data);
        Array.Sort(indexless);

        ContentReference = new WeakReference<ProductContent>(ProductContent.EmptyInstance);
        CalculateContent();
        hash = CalculateHash();
    }

    public Product(Complex factor, Tensor[] indexlessData, Tensor[] data, ProductContent? content, Indices.Indices indices) : base(indices)
    {
        Factor = GetDefaultReference(factor);
        IndexlessData = indexlessData;
        Data = data;
        ContentReference = new WeakReference<ProductContent>(ProductContent.EmptyInstance);
        if (content == null)
        {
            CalculateContent();
        }
        else
        {
            ContentReference.TryGetTarget(out content);
        }
        hash = CalculateHash();
    }

    //very unsafe
    public Product(Indices.Indices indices, Complex factor, Tensor[] indexlessData, Tensor[] data, WeakReference<ProductContent> contentReference, int hash) : base(indices)
    {
        Factor = factor;
        IndexlessData = indexlessData;
        Data = data;
        ContentReference = contentReference;
        this.hash = hash;
    }

    //very unsafe
    public Product(Indices.Indices indices, Complex factor, Tensor[] indexlessData, Tensor[] data, WeakReference<ProductContent> contentReference) : base(indices)
    {
        Factor = factor;
        IndexlessData = indexlessData;
        Data = data;
        ContentReference = contentReference;
        hash = CalculateHash();
    }

    private static Complex GetDefaultReference(Complex factor)
    {
        return factor.IsOne() ? Complex.One : factor.IsMinusOne() ? Complex.MinusOne : factor;
    }

    private int CalculateHash()
    {
        int result;
        if (Factor == Complex.One || Factor == Complex.MinusOne)
        {
            result = 0;
        }
        else
        {
            result = Factor.GetHashCode();
        }

        foreach (Tensor t in IndexlessData)
        {
            result = result * 31 + t.GetHashCode();
        }

        foreach (Tensor t in Data)
        {
            result = result * 17 + t.GetHashCode();
        }

        if (Factor == Complex.MinusOne && Size == 2)
        {
            return result;
        }

        return result - 79 * Content.StructureOfContractionsHashed.GetHashCode();
    }

    public override int GetHashCode() => hash;

    public override Tensor this[int i] => throw new NotImplementedException();

    public override int Size { get; }
    public override string ToString(OutputFormat outputFormat)
    {
        throw new NotImplementedException();
    }

    public override TensorBuilder GetBuilder()
    {
        throw new NotImplementedException();
    }

    public override TensorFactory? GetFactory() => null;

    protected override Tensor Remove1(int[] positions)
    {
        throw new NotImplementedException();
    }

    public override Tensor Remove(int position)
    {
        throw new NotImplementedException();
    }

    protected override Complex GetNeutral()
    {
        throw new NotImplementedException();
    }

    protected override Tensor Select1(int[] positions)
    {
        throw new NotImplementedException();
    }

    internal Tensor[] GetAllScalars()
    {
        throw new NotImplementedException();
    }

    public ProductContent Content
    {
        get
        {
            var success = ContentReference.TryGetTarget(out var content);
            if (!success)
            {
                content = CalculateContent();
                ContentReference.SetTarget(content);
            }
            return content;
        }
    }

    private ProductContent CalculateContent()
    {
        throw new NotImplementedException();
    }

    private static int Hc(Tensor t, int[] inds)
    {
        Indices.Indices ind = t.Indices.GetFree();
        long h = 31;
        int ii;
        for (int i = ind.Size() - 1; i >= 0; --i)
        {
            ii = IndicesUtils.GetNameWithType(ind[i]);
            if ((ii = Array.BinarySearch(inds, ii)) >= 0)
                h ^= HashFunctions.JenkinWang32shift(ii);
        }
        return (int)h;
    }

    private class ScaffoldWrapper : IComparable<ScaffoldWrapper>
    {
        public readonly int[] Inds;
        public readonly Tensor T;
        public readonly TensorContraction Tc;
        public readonly long HashWithIndices;

        public ScaffoldWrapper(int[] inds, Tensor t, TensorContraction tc)
        {
            Inds = inds;
            T = t;
            Tc = tc;
            HashWithIndices = Hc(t, inds);
        }

        public int CompareTo(ScaffoldWrapper o)
        {
            int r = Tc.CompareTo(o.Tc);
            if (r != 0)
                return r;
            return HashWithIndices.CompareTo(o.HashWithIndices);
        }
    }
}