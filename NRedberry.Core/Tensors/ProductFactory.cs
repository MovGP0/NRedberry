using NRedberry.Indices;
using NRedberry.Numbers;
using NRedberry.Transformations.Symmetrization;

namespace NRedberry.Tensors;

public sealed class ProductFactory : TensorFactory
{
    public static readonly ProductFactory Factory = new();

    private ProductFactory()
    {
    }

    public Tensor Create(params Tensor[] tensors)
    {
        if (tensors.Length == 0)
        {
            return Complex.One;
        }

        if (tensors.Length == 1)
        {
            return tensors[0];
        }

        Complex factor = Complex.One;

        var indexlessContainer = new IndexlessWrapper();
        var dataContainer = new DataWrapper();
        foreach (Tensor current in tensors)
        {
            if (current is Complex complex)
            {
                factor = factor.Multiply(complex);
            }
            else if (current is Product p)
            {
                indexlessContainer.Add(p.IndexlessData);
                dataContainer.Add(p.Data, p.Content, p.Indices);
                factor = factor.Multiply(p.Factor);
            }
            else if (current.Indices.Size() == 0)
            {
                indexlessContainer.Add(current);
            }
            else
            {
                dataContainer.Add(current);
            }

            if (factor.IsNaN())
            {
                return factor;
            }
        }

        if (NumberUtils.IsZeroOrIndeterminate(factor))
        {
            return factor;
        }

        if (factor.IsNumeric())
        {
            List<Tensor> newTensors = [];
            factor = Complex.One;
            foreach (Tensor currentTensor in tensors)
            {
                Tensor current = ToNumericITransformation.ToNumeric(currentTensor);
                if (current is Complex complex)
                {
                    factor = factor.Multiply(complex);
                }
                else
                {
                    newTensors.Add(current);
                }
            }

            if (newTensors.Count == 0)
            {
                return factor;
            }

            indexlessContainer = new IndexlessWrapper();
            dataContainer = new DataWrapper();
            foreach (Tensor current in newTensors)
            {
                if (current is Product p)
                {
                    indexlessContainer.Add(p.IndexlessData);
                    dataContainer.Add(p.Data, p.Content, p.Indices);
                    factor = factor.Multiply(p.Factor);
                }
                else if (current.Indices.Size() == 0)
                {
                    indexlessContainer.Add(current);
                }
                else
                {
                    dataContainer.Add(current);
                }
            }
        }

        //Processing data with indices
        int i;
        ProductContent? content;
        Indices.Indices indices;
        Tensor[] data = dataContainer.List.ToArray();
        if (dataContainer.Count == 1)
        {
            content = dataContainer.Content;
            indices = dataContainer.Indices;
            if (indices == null)
            {
                System.Diagnostics.Debug.Assert(dataContainer.List.Count == 1);
                indices = IndicesFactory.Create(dataContainer.List[0].Indices);
            }
        }
        else
        {
            content = null;
            Array.Sort(data);
            var builder = new IndicesBuilder();
            for (i = dataContainer.List.Count - 1; i >= 0; --i)
            {
                builder.Append(dataContainer.List[i]);
            }

            indices = builder.Indices;
        }

        //Processing indexless data
        Tensor[] indexless;
        if (indexlessContainer.Count == 0)
        {
            indexless = Array.Empty<Tensor>();
        }
        else if (indexlessContainer.Count == 1)
        {
            indexless = indexlessContainer.List.ToArray();
        }
        else
        {
            var powersContainer = new PowersContainer(indexlessContainer.List.Count);
            List<Tensor> indexlessArray = [];
            Tensor? tensor = null;
            for (i = indexlessContainer.List.Count - 1; i >= 0; --i)
            {
                tensor = indexlessContainer.List[i];
                if (TensorUtils.IsSymbolic(tensor))
                {
                    powersContainer.Put(tensor);
                }
                else
                {
                    indexlessArray.Add(tensor);
                }
            }

            foreach (Tensor t in powersContainer)
            {
                if (t is Product p)
                {
                    factor = factor.Multiply(p.Factor);
                    indexlessArray.EnsureCapacity(t.Size);
                    foreach (Tensor multiplier in p.IndexlessData)
                    {
                        indexlessArray.Add(multiplier);
                    }
                }
                else if (t is Complex complex)
                {
                    factor = factor.Multiply(complex);
                    if (NumberUtils.IsZeroOrIndeterminate(factor))
                    {
                        return factor;
                    }
                }
                else
                {
                    indexlessArray.Add(t);
                }
            }

            if (powersContainer.Sign)
            {
                factor = factor.Negate();
            }

            indexless = indexlessArray.ToArray();
            Array.Sort(indexless);
        }

        //Constructing result
        if (data.Length == 0 && indexless.Length == 0)
        {
            return factor;
        }

        if (factor.IsOne())
        {
            if (data.Length == 1 && indexless.Length == 0)
            {
                return data[0];
            }

            if (data.Length == 0 && indexless.Length == 1)
            {
                return indexless[0];
            }
        }

        if (factor.IsMinusOne())
        {
            Sum? sum = null;
            if (indexless.Length == 1 && data.Length == 0 && indexless[0] is Sum indexlessSum)
            {
                //case (-1)*(a+b) -> -a-b
                sum = indexlessSum;
            }

            if (indexless.Length == 0 && data.Length == 1 && data[0] is Sum dataSum)
            {
                //case (-1)*(a_i+b_i) -> -a_i-b_i
                sum = dataSum;
            }

            if (sum != null)
            {
                Tensor[] sumData = (Tensor[])sum.Data.Clone();
                for (i = sumData.Length - 1; i >= 0; --i)
                {
                    sumData[i] = Tensors.Negate(sumData[i]);
                }

                return new Sum(sum.Indices, sumData, sum.GetHashCode());
            }
        }

        return new Product(factor, indexless, data, content, indices);
    }

    public Tensor Create(Tensor tensor)
    {
        throw new NotImplementedException();
    }

    private class ListWrapper
    {
        public readonly List<Tensor> List = [];
        public int Count;

        public void Add(Tensor tensor)
        {
            List.Add(tensor);
            ++Count;
        }
    }

    private sealed class IndexlessWrapper : ListWrapper
    {
        public void Add(Tensor[] tensors)
        {
            if (tensors.Length != 0)
            {
                List.AddRange(tensors);
                ++Count;
            }
        }
    }

    private sealed class DataWrapper : ListWrapper
    {
        public ProductContent? Content { get; private set; }
        public Indices.Indices? Indices { get; private set; }

        public void Add(Tensor[] tensors, ProductContent content, Indices.Indices indices)
        {
            if (tensors.Length != 0)
            {
                List.AddRange(tensors);
                Content = content;
                Indices = indices;
                ++Count;
            }
        }
    }
}
