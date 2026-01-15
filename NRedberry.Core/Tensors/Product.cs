using System.Text;
using NRedberry.Core.Utils;
using NRedberry.Graphs;
using NRedberry.Indices;
using NRedberry.Numbers;
using Complex = NRedberry.Numbers.Complex;

namespace NRedberry.Tensors;

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

    public Product(Indices.Indices indices, Complex factor, Tensor[] indexless, Tensor[] data)
        : base(indices)
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

    public Product(Complex factor, Tensor[] indexlessData, Tensor[] data, ProductContent? content, Indices.Indices indices)
        : base(indices)
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
            ContentReference.SetTarget(content);
        }

        hash = CalculateHash();
    }

    //very unsafe
    public Product(Indices.Indices indices, Complex factor, Tensor[] indexlessData, Tensor[] data, WeakReference<ProductContent> contentReference, int hash)
        : base(indices)
    {
        Factor = factor;
        IndexlessData = indexlessData;
        Data = data;
        ContentReference = contentReference;
        this.hash = hash;
    }

    //very unsafe
    public Product(Indices.Indices indices, Complex factor, Tensor[] indexlessData, Tensor[] data, WeakReference<ProductContent> contentReference)
        : base(indices)
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

    public override Tensor this[int i]
    {
        get
        {
            if (Factor != Complex.One)
            {
                --i;
            }

            if (i == -1)
            {
                return Factor;
            }

            if (i < IndexlessData.Length)
            {
                return IndexlessData[i];
            }

            return Data[i - IndexlessData.Length];
        }
    }

    public override int Size
    {
        get
        {
            int size = Data.Length + IndexlessData.Length;
            if (Factor == Complex.One)
            {
                return size;
            }

            return size + 1;
        }
    }

    public override string ToString(OutputFormat outputFormat)
    {
        var sb = new StringBuilder();
        char operatorChar = outputFormat.Is(OutputFormat.LaTeX) ? ' ' : '*';

        if (Factor.IsReal() && Factor.GetReal().SigNum() < 0)
        {
            sb.Append('-');
            Complex f = Factor.Abs();
            if (!f.IsOne())
            {
                sb.Append(f.ToStringWith<Product>(outputFormat)).Append(operatorChar);
            }
        }
        else if (Factor != Complex.One)
        {
            sb.Append(Factor.ToStringWith<Product>(outputFormat)).Append(operatorChar);
        }

        int size = Factor == Complex.One ? Size : Size - 1;
        for (int i = 0; i < IndexlessData.Length; ++i)
        {
            sb.Append(IndexlessData[i].ToStringWith<Product>(outputFormat));
            if (i == size - 1)
            {
                return sb.ToString();
            }

            sb.Append(operatorChar);
        }

        if (outputFormat.PrintMatrixIndices || IndicesUtils.NonMetricTypes(Indices).Count == 0)
        {
            return PrintData(sb, outputFormat, operatorChar);
        }

        return PrintMatrices(sb, outputFormat, operatorChar);
    }

    public override TensorBuilder GetBuilder()
    {
        return new ScalarsBackedProductBuilder();
    }

    public override TensorFactory? GetFactory() => ProductFactory.Factory;

    protected override Tensor Remove1(int[] positions)
    {
        Complex newFactor = Factor;
        if (Factor != Complex.One)
        {
            if (positions[0] == 0)
            {
                newFactor = Complex.One;
                positions = positions.Length == 1 ? [] : positions[1..];
            }

            for (int i = positions.Length - 1; i >= 0; --i)
            {
                --positions[i];
            }
        }

        int dataFrom = Array.BinarySearch(positions, IndexlessData.Length - 1);
        if (dataFrom < 0)
        {
            dataFrom = ~dataFrom - 1;
        }

        int[] indexlessPositions = positions[..(dataFrom + 1)];
        int[] dataPositions = positions[(dataFrom + 1)..];
        for (int i = 0; i < dataPositions.Length; ++i)
        {
            dataPositions[i] -= IndexlessData.Length;
        }

        Tensor[] newIndexless = ArraysUtils.Remove(IndexlessData, indexlessPositions);
        Tensor[] newData = ArraysUtils.Remove(Data, dataPositions);

        return CreateProduct(new IndicesBuilder().Append(newData).Indices, newFactor, newIndexless, newData);
    }

    public override Tensor Remove(int position)
    {
        return SetComplex(position, Complex.One);
    }

    protected override Complex GetNeutral()
    {
        return Complex.One;
    }

    protected override Tensor Select1(int[] positions)
    {
        int add = Factor == Complex.One ? 0 : 1;
        Complex newFactor = Complex.One;
        List<Tensor> newIndexless = [];
        List<Tensor> newData = [];
        foreach (int position in positions)
        {
            int adjusted = position - add;
            if (adjusted == -1)
            {
                newFactor = Factor;
            }
            else if (adjusted < IndexlessData.Length)
            {
                newIndexless.Add(IndexlessData[adjusted]);
            }
            else
            {
                newData.Add(Data[adjusted - IndexlessData.Length]);
            }
        }

        return new Product(
            new IndicesBuilder().Append(newData).Indices,
            newFactor,
            newIndexless.ToArray(),
            newData.ToArray());
    }

    internal Tensor[] GetAllScalars()
    {
        Tensor[] scalars = Content.Scalars;
        if (Factor == Complex.One)
        {
            Tensor[] allScalars = new Tensor[IndexlessData.Length + scalars.Length];
            Array.Copy(IndexlessData, 0, allScalars, 0, IndexlessData.Length);
            Array.Copy(scalars, 0, allScalars, IndexlessData.Length, scalars.Length);
            return allScalars;
        }

        Tensor[] allScalarsWithFactor = new Tensor[1 + IndexlessData.Length + scalars.Length];
        allScalarsWithFactor[0] = Factor;
        Array.Copy(IndexlessData, 0, allScalarsWithFactor, 1, IndexlessData.Length);
        Array.Copy(scalars, 0, allScalarsWithFactor, IndexlessData.Length + 1, scalars.Length);
        return allScalarsWithFactor;
    }

    private static Tensor CreateProduct(Indices.Indices indices, Complex factor, Tensor[] indexless, Tensor[] data)
    {
        if (indexless.Length == 0 && data.Length == 0)
        {
            return factor;
        }

        if (factor == Complex.One)
        {
            if (indexless.Length == 0 && data.Length == 1)
            {
                return data[0];
            }

            if (indexless.Length == 1 && data.Length == 0)
            {
                return indexless[0];
            }
        }

        return new Product(indices, factor, indexless, data);
    }

    private Tensor SetComplex(int i, Complex complex)
    {
        if (NumberUtils.IsZeroOrIndeterminate(complex))
        {
            return complex;
        }

        if (Factor != Complex.One)
        {
            if (i == 0)
            {
                if (complex.IsOne())
                {
                    if (Data.Length == 1 && IndexlessData.Length == 0)
                    {
                        return Data[0];
                    }

                    if (Data.Length == 0 && IndexlessData.Length == 1)
                    {
                        return IndexlessData[0];
                    }
                }

                complex = GetDefaultReference(complex);
                return new Product(Indices, complex, IndexlessData, Data, ContentReference);
            }

            complex = complex.Multiply(Factor);
            complex = GetDefaultReference(complex);
            --i;
        }

        if (complex.IsOne())
        {
            if (Data.Length == 2 && IndexlessData.Length == 0)
            {
                return Data[1 - i];
            }

            if (Data.Length == 0 && IndexlessData.Length == 2)
            {
                return IndexlessData[1 - i];
            }

            if (Data.Length == 1 && IndexlessData.Length == 1)
            {
                return i == 0 ? Data[0] : IndexlessData[0];
            }
        }

        if ((Data.Length == 1 && IndexlessData.Length == 0) || (Data.Length == 0 && IndexlessData.Length == 1))
        {
            return complex;
        }

        if (i < IndexlessData.Length)
        {
            Tensor[] newIndexless = ArraysUtils.Remove(IndexlessData, i);
            return new Product(Indices, complex, newIndexless, Data, ContentReference);
        }

        Tensor[] newData = ArraysUtils.Remove(Data, i - IndexlessData.Length);
        return new Product(new IndicesBuilder().Append(newData).Indices, complex, IndexlessData, newData);
    }

    private string PrintData(StringBuilder sb, OutputFormat outputFormat, char operatorChar)
    {
        for (int i = 0; ; ++i)
        {
            sb.Append(Data[i].ToStringWith<Product>(outputFormat));
            if (i == Data.Length - 1)
            {
                break;
            }

            sb.Append(operatorChar);
        }

        RemoveLastOperatorChar(sb, operatorChar);
        return sb.ToString();
    }

    private string PrintMatrices(StringBuilder sb, OutputFormat outputFormat, char operatorChar)
    {
        return PrintData(sb, outputFormat, operatorChar);
    }

    private static void RemoveLastOperatorChar(StringBuilder sb, char operatorChar)
    {
        if (sb.Length > 0 && sb[^1] == operatorChar)
        {
            sb.Remove(sb.Length - 1, 1);
        }
    }

    private short[] CalculateStretchIndices()
    {
        short[] stretchIndex = new short[Data.Length];
        short index = 0;
        int oldHash = Data[0].GetHashCode();
        for (int i = 1; i < Data.Length; ++i)
        {
            if (oldHash == Data[i].GetHashCode())
            {
                stretchIndex[i] = index;
            }
            else
            {
                stretchIndex[i] = ++index;
                oldHash = Data[i].GetHashCode();
            }
        }

        return stretchIndex;
    }

    private static long PackToLong(int tensorIndex, short stretchIndex, short id)
    {
        return ((long)tensorIndex << 32)
            | (0xFFFF0000L & ((long)stretchIndex << 16))
            | (0xFFFFL & id);
    }

    private static int[] InfoToTensorIndices(long[] info)
    {
        int[] result = new int[info.Length];
        for (int i = 0; i < info.Length; ++i)
        {
            result[i] = ((int)(info[i] >> 32)) + 1;
        }

        return result;
    }

    private const long DummyTensorInfo = -65536;

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
        if (Data.Length == 0)
        {
            ContentReference.SetTarget(ProductContent.EmptyInstance);
            return ProductContent.EmptyInstance;
        }

        Indices.Indices freeIndices = Indices.GetFree();
        int differentIndicesCount = (Indices.Size() + freeIndices.Size()) / 2;

        int[] upperIndices = new int[differentIndicesCount];
        int[] lowerIndices = new int[differentIndicesCount];
        long[] upperInfo = new long[differentIndicesCount];
        long[] lowerInfo = new long[differentIndicesCount];

        int[][] indices = [lowerIndices, upperIndices];
        long[][] info = [lowerInfo, upperInfo];

        int[] pointer = new int[2];
        short[] stretchIndices = CalculateStretchIndices();

        TensorContraction[] contractions = new TensorContraction[Data.Length];
        TensorContraction freeContraction = new(-1, new long[freeIndices.Size()]);

        int state;
        int index;

        for (int i = 0; i < freeIndices.Size(); ++i)
        {
            index = freeIndices[i];
            state = 1 - IndicesUtils.GetStateInt(index);
            info[state][pointer[state]] = DummyTensorInfo;
            indices[state][pointer[state]++] = IndicesUtils.GetNameWithType(index);
        }

        for (int tensorIndex = 0; tensorIndex < Data.Length; ++tensorIndex)
        {
            Indices.Indices tensorIndices = Data[tensorIndex].Indices;
            short[] diffIds = tensorIndices.GetDiffIds();
            for (int i = 0; i < tensorIndices.Size(); ++i)
            {
                index = tensorIndices[i];
                state = IndicesUtils.GetStateInt(index);
                info[state][pointer[state]] = PackToLong(tensorIndex, stretchIndices[tensorIndex], diffIds[i]);
                indices[state][pointer[state]++] = IndicesUtils.GetNameWithType(index);
            }

            contractions[tensorIndex] = new TensorContraction(stretchIndices[tensorIndex], new long[tensorIndices.Size()]);
        }

        ArraysUtils.QuickSort(indices[0], info[0]);
        ArraysUtils.QuickSort(indices[1], info[1]);

        int[] components = GraphUtils.CalculateConnectedComponents(
            InfoToTensorIndices(upperInfo),
            InfoToTensorIndices(lowerInfo),
            Data.Length + 1);

        int componentCount = components[components.Length - 1];
        int[] componentSizes = new int[componentCount];
        for (int i = 1; i < components.Length - 1; ++i)
        {
            ++componentSizes[components[i]];
        }

        Tensor[][] datas = new Tensor[componentCount][];
        for (int i = 0; i < componentCount; ++i)
        {
            datas[i] = new Tensor[componentSizes[i]];
        }

        Array.Fill(componentSizes, 0);
        for (int i = 1; i < Data.Length + 1; ++i)
        {
            datas[components[i]][componentSizes[components[i]]++] = Data[i - 1];
        }

        Tensor? nonScalar = null;
        if (componentCount == 1)
        {
            if (Data.Length == 1)
            {
                nonScalar = Data[0];
            }
            else
            {
                nonScalar = new Product(Indices, Complex.One, [], Data, ContentReference, 0);
            }
        }
        else if (datas[0].Length > 0)
        {
            nonScalar = TensorExtensions.Multiply(datas[0]);
        }

        Tensor[] scalars = new Tensor[componentCount - 1];
        if (nonScalar is null && componentCount == 2 && Factor == Complex.One && IndexlessData.Length == 0)
        {
            scalars[0] = this;
        }
        else
        {
            for (int i = 1; i < componentCount; ++i)
            {
                scalars[i - 1] = TensorExtensions.Multiply(datas[i]);
            }

            Array.Sort(scalars);
        }

        System.Diagnostics.Debug.Assert(indices[0].SequenceEqual(indices[1]));

        int[] pointers = new int[Data.Length];
        int freePointer = 0;
        for (int i = 0; i < differentIndicesCount; ++i)
        {
            int tensorIndex = (int)(info[0][i] >> 32);
            long contraction = (0x0000FFFF00000000L & (info[0][i] << 32)) | (0xFFFFFFFFL & info[1][i]);
            if (tensorIndex == -1)
            {
                freeContraction.IndexContractions[freePointer++] = contraction;
            }
            else
            {
                contractions[tensorIndex].IndexContractions[pointers[tensorIndex]++] = contraction;
            }

            tensorIndex = (int)(info[1][i] >> 32);
            contraction = (0x0000FFFF00000000L & (info[1][i] << 32)) | (0xFFFFFFFFL & info[0][i]);
            if (tensorIndex == -1)
            {
                freeContraction.IndexContractions[freePointer++] = contraction;
            }
            else
            {
                contractions[tensorIndex].IndexContractions[pointers[tensorIndex]++] = contraction;
            }
        }

        foreach (TensorContraction contraction in contractions)
        {
            contraction.SortContractions();
        }

        freeContraction.SortContractions();

        int[] inds = IndicesUtils.GetIndicesNames(Indices.GetFree());
        Array.Sort(inds);
        ScaffoldWrapper[] wrappers = new ScaffoldWrapper[contractions.Length];
        for (int i = 0; i < contractions.Length; ++i)
        {
            wrappers[i] = new ScaffoldWrapper(inds, components[i + 1], Data[i], contractions[i]);
        }

        ArraysUtils.QuickSort(wrappers, Data);
        for (int i = 0; i < contractions.Length; ++i)
        {
            contractions[i] = wrappers[i].Tc;
        }

        StructureOfContractionsHashed structureOfContractionsHashed = new(freeContraction, contractions);
        StructureOfContractions structureOfContractions = StructureOfContractions.EmptyFullContractionsStructure;
        ProductContent content = new(structureOfContractionsHashed, structureOfContractions, scalars, nonScalar, stretchIndices, Data);
        ContentReference.SetTarget(content);

        if (componentCount == 1 && nonScalar is Product product)
        {
            product.hash = product.CalculateHash();
        }

        return content;
    }

    private static int Hc(Tensor t, int[] inds)
    {
        Indices.Indices ind = t.Indices.GetFree();
        int h = 31;
        for (int i = ind.Size() - 1; i >= 0; --i)
        {
            int index = IndicesUtils.GetNameWithType(ind[i]);
            int position = Array.BinarySearch(inds, index);
            if (position >= 0)
            {
                h ^= HashFunctions.JenkinWang32shift(position);
            }
        }

        return h;
    }

    private class ScaffoldWrapper(int[] inds, int component, Tensor t, TensorContraction tc) : IComparable<ScaffoldWrapper>
    {
        public readonly int[] Inds = inds;
        public readonly int Component = component;
        public readonly Tensor T = t;
        public readonly TensorContraction Tc = tc;
        public readonly int HashWithIndices = Hc(t, inds);

        public int CompareTo(ScaffoldWrapper o)
        {
            int r = Tc.CompareTo(o.Tc);
            if (r != 0)
            {
                return r;
            }

            r = HashWithIndices.CompareTo(o.HashWithIndices);
            if (r != 0)
            {
                return r;
            }

            return Component.CompareTo(o.Component);
        }
    }
}
