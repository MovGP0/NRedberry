using NRedberry.Indices;

namespace NRedberry.Contexts;

public sealed class NameDescriptorForTensorFieldImpl(
    string name,
    StructureOfIndices[] indexTypeStructures,
    int id,
    bool isDiracDelta)
    : NameDescriptorForTensorField(indexTypeStructures, id, new int[indexTypeStructures.Length - 1], name, isDiracDelta)
{
    readonly Dictionary<DerivativeDescriptor, NameDescriptorForTensorFieldDerivative> derivatives = new();
    readonly NameAndStructureOfIndices[] keys = [new NameAndStructureOfIndices(name, indexTypeStructures)];
    private readonly object lockObject = new();

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

    public override NameDescriptorForTensorField GetDerivative(params int[] orders)
    {
        ArgumentNullException.ThrowIfNull(orders);

        if (orders.Length != IndexTypeStructures.Length - 1)
        {
            throw new ArgumentException();
        }

        bool isZeroOrder = true;
        foreach (int o in orders)
        {
            if (o < 0)
            {
                throw new ArgumentException("Negative derivative order.");
            }

            if (o != 0)
            {
                isZeroOrder = false;
            }
        }

        if (isZeroOrder)
        {
            return this;
        }

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

        return nd!;
    }
}
