using System.Text;
using TensorCC = NRedberry.Tensors.CC;
using NRedberry.Contexts;
using NRedberry.Indices;
using NRedberry.Tensors;

namespace NRedberry.Transformations.Symmetrization;

/// <summary>
/// Eliminates contractions with metrics and Kronecker deltas.
/// </summary>
public sealed class EliminateMetricsTransformation : TransformationToStringAble
{
    public static EliminateMetricsTransformation Instance { get; } = new();

    private EliminateMetricsTransformation()
    {
    }

    public static Tensor Eliminate(Tensor tensor)
    {
        return Instance.Transform(tensor);
    }

    public Tensor Transform(Tensor tensor)
    {
        return Transform(tensor, DummyMetricsChain.Instance);
    }

    public string ToString(OutputFormat outputFormat)
    {
        return "EliminateMetrics";
    }

    public override string ToString()
    {
        return ToString(TensorCC.GetDefaultOutputFormat());
    }

    private Tensor Transform(Tensor tensor, IMetricsChain chain)
    {
        if (tensor is SimpleTensor simpleTensor)
        {
            SimpleTensor applied = chain.Apply(simpleTensor);
            if (applied is TensorField field)
            {
                bool updated = false;
                TensorBuilder builder = field.GetBuilder();
                for (int i = 0, size = field.Size; i < size; ++i)
                {
                    Tensor current = field[i];
                    Tensor transformed = Transform(current);
                    if (!ReferenceEquals(current, transformed))
                    {
                        updated = true;
                    }

                    builder.Put(transformed);
                }

                if (updated)
                {
                    return builder.Build();
                }
            }

            return applied;
        }

        if (tensor is Product)
        {
            MetricsChainImpl tempContainer = new(chain);
            List<Tensor> nonMetrics = new();
            bool applied = false;

            for (int i = tensor.Size - 1; i >= 0; --i)
            {
                Tensor current = tensor[i];
                if (current is SimpleTensor simple && Context.Get().IsKroneckerOrMetric(simple))
                {
                    applied |= tempContainer.Add(new MetricWrapper(current));
                }
                else
                {
                    nonMetrics.Add(current);
                }
            }

            for (int i = nonMetrics.Count - 1; i >= 0; --i)
            {
                Tensor temp = nonMetrics[i];
                Tensor current = Transform(temp, tempContainer);
                if (!ReferenceEquals(current, temp))
                {
                    applied = true;
                    nonMetrics[i] = current;
                }
            }

            if (!applied)
            {
                return tensor;
            }

            TensorBuilder builder = new ScalarsBackedProductBuilder();
            foreach (Tensor nonMetric in nonMetrics)
            {
                builder.Put(nonMetric);
            }

            foreach (MetricWrapper metricWrapper in tempContainer.Container)
            {
                builder.Put(metricWrapper.Metric);
            }

            return builder.Build();
        }

        if (tensor is Sum || tensor is Expression)
        {
            Tensor[] data = new Tensor[tensor.Size];
            bool applied = false;

            for (int i = tensor.Size - 1; i >= 0; --i)
            {
                Tensor oldTensor = tensor[i];
                Tensor newTensor = i == 0 ? Transform(oldTensor, chain) : Transform(oldTensor, chain.Clone());
                data[i] = newTensor;
                if (!ReferenceEquals(oldTensor, newTensor))
                {
                    applied = true;
                }
            }

            if (!applied)
            {
                return tensor;
            }

            TensorBuilder builder = tensor.GetBuilder();
            foreach (Tensor term in data)
            {
                builder.Put(term);
            }

            return builder.Build();
        }

        Tensor[] rebuild = new Tensor[tensor.Size];
        bool modified = false;
        for (int i = tensor.Size - 1; i >= 0; --i)
        {
            Tensor temp = Transform(tensor[i]);
            if (!ReferenceEquals(temp, tensor[i]))
            {
                modified = true;
            }

            rebuild[i] = temp;
        }

        if (!modified)
        {
            return tensor;
        }

        TensorFactory? factory = tensor.GetFactory();
        return factory is null ? tensor : factory.Create(rebuild);
    }
}

internal interface IMetricsChain
{
    bool MergeWith(MetricWrapper metricWrapper);

    bool Add(MetricWrapper metricWrapper);

    SimpleTensor Apply(SimpleTensor tensor);

    IMetricsChain Clone();

    bool Equals(IMetricsChain other);
}

internal sealed class MetricsChainImpl : IMetricsChain
{
    private readonly IMetricsChain _parent;
    private readonly List<MetricWrapper> _container;

    public MetricsChainImpl(IMetricsChain parent)
    {
        _parent = parent ?? throw new ArgumentNullException(nameof(parent));
        _container = new List<MetricWrapper>();
    }

    private MetricsChainImpl(List<MetricWrapper> container, IMetricsChain parent)
    {
        _container = container ?? throw new ArgumentNullException(nameof(container));
        _parent = parent ?? throw new ArgumentNullException(nameof(parent));
    }

    public IReadOnlyList<MetricWrapper> Container => _container;

    public bool MergeWith(MetricWrapper metricWrapper)
    {
        bool updated = false;
        for (int i = _container.Count - 1; i >= 0; --i)
        {
            MetricWrapper current = _container[i];
            if (metricWrapper.Apply(current))
            {
                _container.RemoveAt(i);
                updated = true;
            }
        }

        return _parent.MergeWith(metricWrapper) || updated;
    }

    public bool Add(MetricWrapper metricWrapper)
    {
        bool merged = MergeWith(metricWrapper);
        _container.Add(metricWrapper);
        return merged;
    }

    public SimpleTensor Apply(SimpleTensor tensor)
    {
        SimpleTensor oldValue = tensor;
        for (int i = 0; i < _container.Count; ++i)
        {
            MetricWrapper current = _container[i];
            SimpleTensor newValue = current.Apply(oldValue);
            if (!ReferenceEquals(newValue, oldValue))
            {
                _container.RemoveAt(i);
                --i;
                oldValue = newValue;
            }
        }

        return _parent.Apply(oldValue);
    }

    public IMetricsChain Clone()
    {
        List<MetricWrapper> newList = new(_container.Count);
        foreach (MetricWrapper metricWrapper in _container)
        {
            newList.Add(metricWrapper.Clone());
        }

        return new MetricsChainImpl(newList, _parent.Clone());
    }

    public bool Equals(IMetricsChain other)
    {
        if (other is DummyMetricsChain)
        {
            return false;
        }

        if (other is not MetricsChainImpl otherChain)
        {
            return false;
        }

        if (_container.Count != otherChain._container.Count)
        {
            return false;
        }

        _container.Sort();
        otherChain._container.Sort();

        for (int i = 0; i < _container.Count; ++i)
        {
            if (!_container[i].Equals(otherChain._container[i]))
            {
                return false;
            }
        }

        return _parent.Equals(otherChain._parent);
    }

    public override string ToString()
    {
        StringBuilder builder = new();
        foreach (MetricWrapper metricWrapper in _container)
        {
            builder.Append(metricWrapper).Append(';');
        }

        return builder.ToString();
    }
}

internal sealed class DummyMetricsChain : IMetricsChain
{
    public static DummyMetricsChain Instance { get; } = new();

    private DummyMetricsChain()
    {
    }

    public bool MergeWith(MetricWrapper metricWrapper)
    {
        return false;
    }

    public bool Add(MetricWrapper metricWrapper)
    {
        throw new InvalidOperationException();
    }

    public SimpleTensor Apply(SimpleTensor tensor)
    {
        return tensor;
    }

    public IMetricsChain Clone()
    {
        return Instance;
    }

    public bool Equals(IMetricsChain other)
    {
        return other is DummyMetricsChain;
    }

    public override string ToString()
    {
        return "RootMetricKroneckerContainer";
    }
}

internal sealed class MetricWrapper : IComparable<MetricWrapper>, IEquatable<MetricWrapper>
{
    private readonly int[] _indices = new int[2];

    public MetricWrapper(Tensor tensor)
    {
        _indices[0] = tensor.Indices[0];
        _indices[1] = tensor.Indices[1];
        Array.Sort(_indices);
        Metric = tensor;
    }

    private MetricWrapper(int index1, int index2, Tensor metric)
    {
        _indices[0] = index1;
        _indices[1] = index2;
        Metric = metric;
    }

    public Tensor Metric { get; private set; }

    public int CompareTo(MetricWrapper? other)
    {
        if (other is null)
        {
            return 1;
        }

        int result = _indices[0].CompareTo(other._indices[0]);
        return result != 0 ? result : _indices[1].CompareTo(other._indices[1]);
    }

    public SimpleTensor Apply(SimpleTensor tensor)
    {
        SimpleIndices oldIndices = tensor.SimpleIndices;
        int from = -1;
        int to = -1;

        for (int i = 0; i < oldIndices.Size(); ++i)
        {
            int oldIndex = oldIndices[i];
            for (int j = 0; j < 2; ++j)
            {
                if ((oldIndex ^ _indices[j]) == int.MinValue)
                {
                    from = oldIndex;
                    to = _indices[1 - j];
                    goto Found;
                }
            }
        }

        Found:
        MetricIndexMapping mapping = new(from, to);
        SimpleIndices newIndices = (SimpleIndices)oldIndices.ApplyIndexMapping(mapping);
        if (ReferenceEquals(oldIndices, newIndices))
        {
            return tensor;
        }

        if (tensor.GetType() == typeof(SimpleTensor))
        {
            return Tensors.Tensors.SimpleTensor(tensor.Name, newIndices);
        }

        TensorField field = (TensorField)tensor;
        return TensorField.Create(field.Name, newIndices, field.ArgumentIndices, field.Arguments);
    }

    public bool Apply(MetricWrapper other)
    {
        for (int i = 0; i < 2; ++i)
        {
            for (int j = 0; j < 2; ++j)
            {
                if ((_indices[i] ^ other._indices[j]) == int.MinValue)
                {
                    Metric = Context.Get().CreateMetricOrKronecker(_indices[1 - i], other._indices[1 - j]);
                    _indices[i] = other._indices[1 - j];
                    Array.Sort(_indices);
                    return true;
                }
            }
        }

        return false;
    }

    public MetricWrapper Clone()
    {
        Tensor metric = Context.Get().CreateMetricOrKronecker(_indices[0], _indices[1]);
        return new MetricWrapper(_indices[0], _indices[1], metric);
    }

    public bool Equals(MetricWrapper? other)
    {
        if (other is null)
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return _indices[0] == other._indices[0] && _indices[1] == other._indices[1];
    }

    public override bool Equals(object? obj)
    {
        return obj is MetricWrapper other && Equals(other);
    }

    public override int GetHashCode()
    {
        HashCode hashCode = new();
        hashCode.Add(_indices[0]);
        hashCode.Add(_indices[1]);
        return hashCode.ToHashCode();
    }

    public static bool operator ==(MetricWrapper? left, MetricWrapper? right)
    {
        if (ReferenceEquals(left, right))
        {
            return true;
        }

        if (left is null || right is null)
        {
            return false;
        }

        return left.Equals(right);
    }

    public static bool operator !=(MetricWrapper? left, MetricWrapper? right)
    {
        return !(left == right);
    }

    public override string ToString()
    {
        return Metric.ToString();
    }
}

internal sealed class MetricIndexMapping : IIndexMapping
{
    private readonly int _from;
    private readonly int _to;

    public MetricIndexMapping(int from, int to)
    {
        _from = from;
        _to = to;
    }

    public int Map(int from)
    {
        return from == _from ? _to : from;
    }
}
