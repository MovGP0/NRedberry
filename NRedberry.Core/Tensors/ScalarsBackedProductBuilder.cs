using NRedberry.Indices;
using NRedberry.Numbers;

namespace NRedberry.Tensors;

/*
 * Original: ./core/src/main/java/cc/redberry/core/tensor/ScalarsBackedProductBuilder.java
 */

public sealed class ScalarsBackedProductBuilder : TensorBuilder
{
    private readonly PowersContainer _powers;
    private readonly Dictionary<int, Component> _indexToComponent;
    private readonly HashSet<Component> _components;
    private Complex _factor = Complex.One;

    public ScalarsBackedProductBuilder()
    {
        _powers = new PowersContainer();
        _indexToComponent = new Dictionary<int, Component>();
        _components = new HashSet<Component>();
    }

    public ScalarsBackedProductBuilder(int capacity)
        : this(capacity, capacity, capacity)
    {
    }

    public ScalarsBackedProductBuilder(int powersCapacity, int componentsCapacity, int indicesCapacity)
    {
        _powers = new PowersContainer(powersCapacity);
        _indexToComponent = new Dictionary<int, Component>(indicesCapacity);
        _components = new HashSet<Component>(componentsCapacity);
    }

    private ScalarsBackedProductBuilder(
        PowersContainer powers,
        Dictionary<int, Component> indexToComponent,
        HashSet<Component> components,
        Complex factor)
    {
        _powers = powers;
        _indexToComponent = indexToComponent;
        _components = components;
        _factor = factor;
    }

    public void Put(Tensor tensor)
    {
        ArgumentNullException.ThrowIfNull(tensor);

        if (tensor is Complex complexTensor)
        {
            _factor = _factor.Multiply(complexTensor);
            return;
        }

        if (NumberUtils.IsZeroOrIndeterminate(_factor))
        {
            return;
        }

        if (tensor is Product product)
        {
            _factor = _factor.Multiply(product.Factor);
            if (NumberUtils.IsZeroOrIndeterminate(_factor))
            {
                return;
            }

            foreach (Tensor t in product.IndexlessData)
            {
                _powers.Put(t);
            }

            foreach (Tensor t in product.Content.Scalars)
            {
                _powers.Put(t);
            }

            Tensor? nonScalar = product.Content.NonScalar;
            if (nonScalar is null)
            {
                return;
            }

            if (nonScalar is Product)
            {
                foreach (Tensor t in nonScalar)
                {
                    PutNonScalar(t);
                }
            }
            else
            {
                PutNonScalar(nonScalar);
            }

            return;
        }

        if (TensorUtils.IsScalar(tensor))
        {
            _powers.Put(tensor);
        }
        else
        {
            PutNonScalar(tensor);
        }
    }

    public Tensor Build()
    {
        if (NumberUtils.IsZeroOrIndeterminate(_factor))
        {
            return _factor;
        }

        if (_powers.Sign)
        {
            _factor = _factor.Negate();
        }

        var indexless = new List<Tensor>(_powers.Count);
        var data = new List<Tensor>();

        foreach (Tensor power in _powers)
        {
            if (power is Complex complexPower)
            {
                _factor = _factor.Multiply(complexPower);
                if (NumberUtils.IsZeroOrIndeterminate(_factor))
                {
                    return _factor;
                }
            }
            else if (TensorUtils.IsIndexless(power))
            {
                indexless.Add(power);
            }
            else if (power is Product product)
            {
                foreach (Tensor t in product)
                {
                    data.Add(t);
                }
            }
            else
            {
                data.Add(power);
            }
        }

        foreach (Component component in _components)
        {
            data.AddRange(component.Elements);
        }

        if (indexless.Count == 0 && data.Count == 0)
        {
            return _factor;
        }

        if (_factor.IsOne())
        {
            if (indexless.Count == 1 && data.Count == 0)
            {
                return indexless[0];
            }

            if (indexless.Count == 0 && data.Count == 1)
            {
                return data[0];
            }
        }

        if (_factor.IsMinusOne())
        {
            Sum? sum = null;
            if (indexless.Count == 1 && data.Count == 0 && indexless[0] is Sum indexlessSum)
            {
                sum = indexlessSum;
            }

            if (indexless.Count == 0 && data.Count == 1 && data[0] is Sum dataSum)
            {
                sum = dataSum;
            }

            if (sum is not null)
            {
                Tensor[] sumData = (Tensor[])sum.Data.Clone();
                for (int i = sumData.Length - 1; i >= 0; --i)
                {
                    sumData[i] = Tensors.Negate(sumData[i]);
                }

                return new Sum(sum.Indices, sumData, sum.GetHashCode());
            }
        }

        return new Product(
            new IndicesBuilder().Append(data).Indices,
            _factor,
            indexless.ToArray(),
            data.ToArray());
    }

    public TensorBuilder Clone()
    {
        var newComponents = new HashSet<Component>(_components.Count);
        var newIndexToComponent = new Dictionary<int, Component>(_indexToComponent.Count);
        foreach (Component current in _components)
        {
            Component component = current.Copy();
            newComponents.Add(component);
            foreach (int index in component.FreeIndices)
            {
                newIndexToComponent[index] = component;
            }
        }

        var powers = new PowersContainer(_powers.Count);
        powers.Merge(_powers);
        return new ScalarsBackedProductBuilder(powers, newIndexToComponent, newComponents, _factor);
    }

    private void PutNonScalar(Tensor tensor)
    {
        Indices.Indices freeIndices = tensor.Indices.GetFree();
        var freeSet = new HashSet<int>(freeIndices.AllIndices);

        var toMerge = new HashSet<Component>();
        if (_indexToComponent.Count != 0)
        {
            foreach (int currentIndex in freeSet.ToArray())
            {
                System.Diagnostics.Debug.Assert(!_indexToComponent.ContainsKey(currentIndex));

                int index = IndicesUtils.InverseIndexState(currentIndex);
                if (_indexToComponent.Remove(index, out Component? component))
                {
                    freeSet.Remove(currentIndex);
                    component.FreeIndices.Remove(index);
                    toMerge.Add(component);
                }
            }
        }

        if (toMerge.Count == 0)
        {
            var component = new Component(tensor, freeSet);
            foreach (int index in freeSet)
            {
                _indexToComponent[index] = component;
            }

            _components.Add(component);
            return;
        }

        using var iterator = toMerge.GetEnumerator();
        iterator.MoveNext();
        Component root = iterator.Current;
        root.Elements.Add(tensor);
        root.FreeIndices.UnionWith(freeSet);

        foreach (int index in freeSet)
        {
            _indexToComponent[index] = root;
        }

        while (iterator.MoveNext())
        {
            Component temp = iterator.Current;
            foreach (int index in temp.FreeIndices)
            {
                _indexToComponent[index] = root;
            }

            root.FreeIndices.UnionWith(temp.FreeIndices);
            root.Elements.AddRange(temp.Elements);
            _components.Remove(temp);
        }

        if (root.FreeIndices.Count == 0)
        {
            _components.Remove(root);
            _powers.Put(new Product(
                new IndicesBuilder().Append(root.Elements).Indices,
                Complex.One,
                [],
                root.Elements.ToArray()));
        }
    }

    private sealed record class Component
    {
        public List<Tensor> Elements { get; }

        public HashSet<int> FreeIndices { get; }

        public Component(Tensor tensor, HashSet<int> freeIndices)
        {
            Elements = [tensor];
            FreeIndices = freeIndices;
        }

        private Component(List<Tensor> elements, HashSet<int> freeIndices)
        {
            Elements = elements;
            FreeIndices = freeIndices;
        }

        public Component Copy()
        {
            return new Component(new List<Tensor>(Elements), new HashSet<int>(FreeIndices));
        }
    }
}
