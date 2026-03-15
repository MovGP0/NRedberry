using NRedberry.Indices;
using NRedberry.IndexMapping;
using NRedberry.Numbers;
using NRedberry.Tensors;

namespace NRedberry.Transformations.Substitutions;

/// <summary>
/// Port of cc.redberry.core.transformations.substitutions.PrimitiveProductSubstitution.
/// </summary>
public sealed class PrimitiveProductSubstitution : PrimitiveSubstitution
{
    private readonly Complex _fromFactor;
    private readonly Tensor[] _fromIndexless;
    private readonly Tensor[] _fromData;
    private readonly ProductContent _fromContent;

    public PrimitiveProductSubstitution(Tensor from, Tensor to)
        : base(from, to)
    {
        Product product = (Product)From;
        _fromFactor = product.Factor;
        _fromIndexless = product.IndexlessData;
        _fromContent = product.Content;
        _fromData = _fromContent.GetDataCopy();
    }

    protected override Tensor NewToCore(Tensor currentNode, SubstitutionIterator iterator)
    {
        Product product = (Product)currentNode;
        Complex factor = product.Factor;
        PContent content = new(product.IndexlessData, GetDataSubProduct(product));

        ForbiddenContainer forbidden = new();
        SubsResult? substitutionResult = AtomicSubstitute(content, forbidden, iterator);
        if (substitutionResult is null)
        {
            return currentNode;
        }

        List<Tensor> newTos = [];
        while (substitutionResult is not null)
        {
            factor = factor.Divide(_fromFactor);
            newTos.Add(substitutionResult.NewTo);
            content = substitutionResult.Remainder;
            substitutionResult = AtomicSubstitute(content, forbidden, iterator);
        }

        Tensor[] result = new Tensor[newTos.Count + content.Indexless.Length + 2];
        Array.Copy(newTos.ToArray(), 0, result, 0, newTos.Count);
        Array.Copy(content.Indexless, 0, result, newTos.Count, content.Indexless.Length);
        result[^2] = content.Data;
        result[^1] = factor;
        return NRedberry.Tensors.Tensors.Multiply(result);
    }

    private SubsResult? AtomicSubstitute(PContent content, ForbiddenContainer forbidden, SubstitutionIterator iterator)
    {
        Mapping? mapping = null;
        int[]? indexlessBijection;
        int[] dataBijection;

        IndexlessBijectionsPort indexlessPort = new(_fromIndexless, content.Indexless);
        while ((indexlessBijection = indexlessPort.Take()) is not null)
        {
            Tensor[] currentIndexless = Extract(content.Indexless, indexlessBijection);
            MappingsPort indexlessMappings = IndexMappings.CreateBijectiveProductPort(_fromIndexless, currentIndexless);
            mapping = indexlessMappings.Take();
            if (mapping is not null)
            {
                break;
            }
        }

        if (mapping is null || indexlessBijection is null)
        {
            return null;
        }

        bool sign = mapping.GetSign();
        mapping = null;

        Tensor[] currentData;
        if (content.Data is Product dataProduct)
        {
            ProductContent currentContent = dataProduct.Content;
            currentData = currentContent.GetDataCopy();
            ProductsBijectionsPort dataPort = new(_fromContent, currentContent);
            int[]? candidate;
            while ((candidate = dataPort.Take()) is not null)
            {
                Tensor[] matchedData = Extract(currentData, candidate);
                MappingsPort dataMappings = IndexMappings.CreateBijectiveProductPort(_fromData, matchedData);
                mapping = dataMappings.Take();
                if (mapping is not null)
                {
                    dataBijection = candidate;
                    goto MappingResolved;
                }
            }

            return null;
        }

        if (TensorUtils.IsOne(content.Data))
        {
            if (_fromContent.Size != 0)
            {
                return null;
            }

            dataBijection = [];
            currentData = [];
            mapping = Mapping.IdentityMapping;
            goto MappingResolved;
        }

        if (_fromContent.Size != 1)
        {
            return null;
        }

        dataBijection = [0];
        currentData = [content.Data];
        mapping = IndexMappings.GetFirst(_fromContent[0], content.Data);
        if (mapping is null)
        {
            return null;
        }

MappingResolved:
        mapping = mapping.AddSign(sign);
        Array.Sort(indexlessBijection);
        Array.Sort(dataBijection);

        Tensor[] indexlessRemainder = new Tensor[content.Indexless.Length - _fromIndexless.Length];
        ScalarsBackedProductBuilder dataRemainder = new();

        int pivot = 0;
        int j = 0;
        for (int i = 0; i < content.Indexless.Length; ++i)
        {
            if (pivot < indexlessBijection.Length && i == indexlessBijection[pivot])
            {
                ++pivot;
            }
            else
            {
                indexlessRemainder[j++] = content.Indexless[i];
            }
        }

        pivot = 0;
        for (int i = 0; i < currentData.Length; ++i)
        {
            if (pivot < dataBijection.Length && i == dataBijection[pivot])
            {
                ++pivot;
            }
            else
            {
                dataRemainder.Put(currentData[i]);
            }
        }

        Tensor dataRemainderTensor = dataRemainder.Build();
        PContent remainder = new(indexlessRemainder, dataRemainderTensor);

        Tensor newTo;
        if (ToIsSymbolic)
        {
            newTo = mapping.GetSign() ? NRedberry.Tensors.Tensors.Negate(To) : To;
        }
        else if (PossiblyAddsDummies)
        {
            forbidden.Forbidden ??= new HashSet<int>(iterator.GetForbidden());

            HashSet<int> remainderIndices = new(forbidden.Forbidden);
            remainderIndices.UnionWith(TensorUtils.GetAllIndicesNamesT(indexlessRemainder));
            remainderIndices.UnionWith(TensorUtils.GetAllIndicesNamesT(dataRemainderTensor));
            newTo = ApplyIndexMapping.Apply(To, mapping, remainderIndices.ToArray());
            forbidden.Forbidden.UnionWith(TensorUtils.GetAllIndicesNamesT(newTo));
        }
        else
        {
            HashSet<int> allowed = [];
            for (int i = 0; i < indexlessBijection.Length; ++i)
            {
                allowed.UnionWith(TensorUtils.GetAllDummyIndicesT(content.Indexless[indexlessBijection[i]]));
            }

            List<Tensor> selectedData = [];
            for (int i = 0; i < dataBijection.Length; ++i)
            {
                Tensor selected = currentData[dataBijection[i]];
                allowed.UnionWith(TensorUtils.GetAllDummyIndicesT(selected));
                selectedData.Add(selected);
            }

            if (selectedData.Count != 0)
            {
                allowed.UnionWith(TensorUtils.GetAllDummyIndicesT(NRedberry.Tensors.Tensors.Multiply(selectedData.ToArray())));
            }

            foreach (int index in IndicesUtils.GetIndicesNames(mapping.GetToData()))
            {
                allowed.Remove(index);
            }

            newTo = ApplyIndexMapping.ApplyIndexMappingAndRenameAllDummies(To, mapping, allowed.ToArray());
        }

        return new SubsResult(newTo, remainder);
    }

    private static Tensor[] Extract(Tensor[] source, int[] positions)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(positions);

        Tensor[] result = new Tensor[positions.Length];
        for (int i = 0; i < positions.Length; ++i)
        {
            result[i] = source[positions[i]];
        }

        return result;
    }

    private sealed class ForbiddenContainer
    {
        public HashSet<int>? Forbidden { get; set; }
    }

    private sealed class SubsResult
    {
        public SubsResult(Tensor newTo, PContent remainder)
        {
            NewTo = newTo ?? throw new ArgumentNullException(nameof(newTo));
            Remainder = remainder ?? throw new ArgumentNullException(nameof(remainder));
        }

        public Tensor NewTo { get; }

        public PContent Remainder { get; }
    }

    private sealed class PContent
    {
        public PContent(Tensor[] indexless, Tensor data)
        {
            Indexless = indexless ?? throw new ArgumentNullException(nameof(indexless));
            Data = data ?? throw new ArgumentNullException(nameof(data));
        }

        public Tensor[] Indexless { get; }

        public Tensor Data { get; }
    }

    private static Tensor GetDataSubProduct(Product product)
    {
        ArgumentNullException.ThrowIfNull(product);

        if (product.Data.Length == 0)
        {
            return Complex.One;
        }

        if (product.Data.Length == 1)
        {
            return product.Data[0];
        }

        return NRedberry.Tensors.Tensors.Multiply(product.Data);
    }
}
