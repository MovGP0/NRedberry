using System.Collections;
using NRedberry.Numbers;

namespace NRedberry.Tensors;

/*
 * Original: ./core/src/main/java/cc/redberry/core/tensor/PowersContainer.java
 */

public sealed class PowersContainer : IEnumerable<Tensor>
{
    private bool _sign;
    private readonly System.Collections.Generic.Dictionary<int, System.Collections.Generic.List<PowerNode>> _powers;

    public PowersContainer()
    {
        _powers = new System.Collections.Generic.Dictionary<int, System.Collections.Generic.List<PowerNode>>();
    }

    public PowersContainer(int initialCapacity)
    {
        _powers = new System.Collections.Generic.Dictionary<int, System.Collections.Generic.List<PowerNode>>(initialCapacity);
    }

    public bool Sign => _sign;

    public bool IsEmpty()
    {
        return _powers.Count == 0;
    }

    public int Count => _powers.Count;

    public void Put(Tensor tensor)
    {
        ArgumentNullException.ThrowIfNull(tensor);

        Tensor baseTensor;
        Tensor exponent;
        if (tensor is Power)
        {
            baseTensor = tensor[0];
            exponent = tensor[1];
        }
        else
        {
            baseTensor = tensor;
            exponent = Complex.One;
        }

        int hash = baseTensor.GetHashCode();
        if (!_powers.TryGetValue(hash, out var nodes))
        {
            nodes = new System.Collections.Generic.List<PowerNode>();
            _powers[hash] = nodes;
        }

        foreach (PowerNode node in nodes)
        {
            bool? compare = TensorUtils.Compare1(node.Base, baseTensor);
            if (compare is null)
            {
                continue;
            }

            if (compare == false)
            {
                node.PutExponent(exponent);
                return;
            }

            if (TensorUtils.IsInteger(exponent))
            {
                node.PutExponent(exponent);
                if (TensorUtils.IsIntegerOdd(exponent))
                {
                    _sign = !_sign;
                }

                return;
            }

            if (node.Exponent is null || node.Exponent is Complex)
            {
                Complex exponent1 = node.Exponent is null ? Complex.One : (Complex)node.Exponent;
                if (exponent1.IsInteger())
                {
                    node.Base = baseTensor;
                    node.PutExponent(exponent);
                    if (NumberUtils.IsIntegerOdd(exponent1))
                    {
                        _sign = !_sign;
                    }

                    return;
                }
            }
        }

        nodes.Add(new PowerNode(baseTensor, exponent));
    }

    public void Merge(PowersContainer container)
    {
        ArgumentNullException.ThrowIfNull(container);

        if (container._sign)
        {
            _sign = !_sign;
        }

        foreach (Tensor tensor in container)
        {
            Put(tensor);
        }
    }

    public IEnumerator<Tensor> GetEnumerator()
    {
        foreach (var list in _powers.Values)
        {
            foreach (PowerNode node in list)
            {
                yield return node.Build();
            }
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    private sealed record class PowerNode
    {
        public Tensor Base { get; set; }
        public Tensor? Exponent { get; private set; }

        public PowerNode(Tensor baseTensor, Tensor exponent)
        {
            Base = baseTensor;
            Exponent = TensorUtils.IsOne(exponent) ? null : exponent;
        }

        public void PutExponent(Tensor exponent)
        {
            if (Exponent is null)
            {
                Exponent = Complex.One;
            }

            Exponent = Tensors.Sum(Exponent, exponent);
        }

        public Tensor Build()
        {
            return Exponent is null ? Base : Tensors.Pow(Base, Exponent);
        }
    }
}
