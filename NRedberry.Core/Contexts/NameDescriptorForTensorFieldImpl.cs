using System;
using System.Collections.Generic;
using System.Linq;
using NRedberry.Core.Indices;

namespace NRedberry.Contexts;

public sealed class NameDescriptorForTensorFieldImpl: NameDescriptorForTensorField
{
    readonly Dictionary<DerivativeDescriptor, NameDescriptorForTensorFieldDerivative> derivatives = new();
    readonly NameAndStructureOfIndices[] keys;

    public NameDescriptorForTensorFieldImpl(
        string name,
        StructureOfIndices[] indexTypeStructures,
        int id,
        bool isDiracDelta)
        : base(indexTypeStructures, id, new int[indexTypeStructures.Length - 1], name, isDiracDelta)
    {
        keys = [new NameAndStructureOfIndices(name, indexTypeStructures)];
    }

    public override NameAndStructureOfIndices[] GetKeys()
    {
        return keys;
    }

    public override string GetName(SimpleIndices? indices, OutputFormat format)
    {
        return Name;
    }

    public override bool IsDerivative()
    {
        return false;
    }

    public override NameDescriptorForTensorField GetParent()
    {
        return this;
    }

    private readonly object lockObject = new();

    public override NameDescriptorForTensorField GetDerivative(params int[] orders)
    {
        if (orders.Length != IndexTypeStructures.Length - 1)
            throw new ArgumentException();

        bool isZeroOrder = true;
        foreach (int o in orders)
        {
            if (o < 0)
                throw new ArgumentException("Negative derivative order.");

            if (o != 0)
                isZeroOrder = false;
        }

        if (isZeroOrder) return this;

        var derivativeDescriptor = new DerivativeDescriptor(orders);
        if (!derivatives.TryGetValue(derivativeDescriptor, out var nd))
        {
            lock (lockObject)
            {
                if (!derivatives.TryGetValue(derivativeDescriptor, out nd))
                {
                    nd = NameManager.CreateDescriptorForFieldDerivative(this, orders);
                    derivatives.Add(derivativeDescriptor, nd);
                }
            }
        }

        return nd;
    }

    private sealed class DerivativeDescriptor(int[] orders)
    {
        public readonly int[] Orders = orders;

        public override bool Equals(object? obj)
        {
            return obj is DerivativeDescriptor that
                && Orders.SequenceEqual(that.Orders);
        }

        public override int GetHashCode()
        {
            return Orders.Aggregate(17, (current, item) => current * 23 + item);
        }
    }
}