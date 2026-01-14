using System.Diagnostics;
using System.Text;
using NRedberry.Core.Combinatorics;
using NRedberry.Groups;
using NRedberry.Indices;

namespace NRedberry.Contexts;

internal sealed class NameDescriptorForTensorFieldDerivative : NameDescriptorForTensorField
{
    public NameDescriptorForTensorFieldImpl Parent { get; }

    public NameDescriptorForTensorFieldDerivative(int id, int[] orders, NameDescriptorForTensorFieldImpl parent)
        : base(
            GenerateStructures(ValidateParent(parent), ValidateOrders(orders)),
            id,
            orders,
            GenerateName(orders, ValidateParent(parent)),
            ValidateParent(parent).IsDiracDelta)
    {
        Parent = parent;
        InitializeSymmetries();
    }

    public override NameDescriptorForTensorField GetParent()
    {
        return Parent;
    }

    public override NameAndStructureOfIndices[] GetKeys()
    {
        return [];
    }

    public override string GetName(SimpleIndices? indices, OutputFormat format)
    {
        if (format.Is(OutputFormat.WolframMathematica))
        {
            var spl = Name.Split('~');
            return $"Derivative{spl[1].Replace("(", "[").Replace(")", "]")}[{spl[0]}]";
        }

        if (format.Is(OutputFormat.Maple))
        {
            var sb = new StringBuilder();
            sb.Append("D[");
            for (var j = 0; j < Orders.Length; ++j)
            {
                for (var i = 0; i < Orders[j]; ++i)
                {
                    sb.Append(j + 1).Append(",");
                }
            }

            sb.Remove(sb.Length - 1, 1);
            sb.Append("](").Append(Name.Split('~')[0]).Append(")");
            return sb.ToString();
        }

        return Name;
    }

    public override bool IsDerivative()
    {
        return true;
    }

    public override NameDescriptorForTensorField GetDerivative(params int[] orders)
    {
        if (orders.Length != IndexTypeStructures.Length - 1)
        {
            throw new ArgumentException();
        }

        var resOrder = Orders.ToArray();
        for (var i = orders.Length - 1; i >= 0; --i)
        {
            resOrder[i] += orders[i];
        }

        return Parent.GetDerivative(resOrder);
    }

    private void InitializeSymmetries()
    {
        var baseStructure = StructuresOfIndices[0];

        StructureOfIndices[] partition = new StructureOfIndices[1 + Orders.Sum()];
        partition[0] = Parent.StructuresOfIndices[0];
        int i;
        int j;
        int k = 0;
        for (i = 0; i < Orders.Length; ++i)
        {
            for (j = 0; j < Orders[i]; ++j)
            {
                partition[++k] = StructuresOfIndices[i + 1].GetInverted();
            }
        }

        int[][] mapping = baseStructure.GetPartitionMappings(partition);

        // Adding field symmetries
        foreach (Permutation p in Parent.GetSymmetries().Generators)
        {
            Symmetries.AddSymmetry(ConvertPermutation(p, mapping[0], baseStructure.Size));
        }

        // Adding block symmetries of derivatives
        var aggregator = new List<int>();
        j = 1;
        int[] cycle;
        for (i = 0; i < Orders.Length; ++i)
        {
            if (StructuresOfIndices[i + 1].Size != 0 && Orders[i] >= 2)
            {
                // Adding symmetries for indices from each slot
                cycle = Permutations.CreateBlockCycle(StructuresOfIndices[i + 1].Size, 2);
                aggregator.AddRange(mapping[j]);
                aggregator.AddRange(mapping[j + 1]);
                Symmetries.AddSymmetry(
                    Permutations.CreatePermutation(ConvertPermutation(cycle, aggregator.ToArray(), baseStructure.Size)));

                if (Orders[i] >= 3)
                {
                    for (k = 2; k < Orders[i]; ++k)
                    {
                        aggregator.AddRange(mapping[j + k]);
                    }

                    cycle = Permutations.CreateBlockCycle(StructuresOfIndices[i + 1].Size, Orders[i]);
                    Symmetries.AddSymmetry(
                        Permutations.CreatePermutation(ConvertPermutation(cycle, aggregator.ToArray(), baseStructure.Size)));
                }

                aggregator.Clear();
            }

            j += Orders[i];
        }
    }

    private static Permutation ConvertPermutation(Permutation permutation, int[] mapping, int newDimension)
    {
        ArgumentNullException.ThrowIfNull(permutation);
        return Permutations.CreatePermutation(permutation.IsAntisymmetry, ConvertPermutation(permutation.OneLine(), mapping, newDimension));
    }

    private static int[] ConvertPermutation(int[] permutation, int[] mapping, int newDimension)
    {
        ArgumentNullException.ThrowIfNull(permutation);
        ArgumentNullException.ThrowIfNull(mapping);
        Debug.Assert(permutation.Length == mapping.Length, "Permutation and mapping arrays must be of the same length.");

        var result = new int[newDimension];
        for (var i = 0; i < newDimension; ++i)
        {
            result[i] = i;
        }

        for (var i = permutation.Length - 1; i >= 0; --i)
        {
            if (mapping[i] == -1)
            {
                continue;
            }

            var k = mapping[permutation[i]];
            Debug.Assert(k != -1, "Mapping at the permuted index must not be -1.");
            result[mapping[i]] = k;
        }

        return result;
    }

    private static string GenerateName(int[] orders, NameDescriptorForTensorFieldImpl parent)
    {
        var sb = new StringBuilder();
        sb.Append(parent.Name).Append('~').Append('(');
        for (var i = 0; i < orders.Length; ++i)
        {
            sb.Append(orders[i]);
            if (i < orders.Length - 1)
            {
                sb.Append(',');
            }
        }

        sb.Append(')');
        return sb.ToString();
    }

    private static StructureOfIndices[] GenerateStructures(NameDescriptorForTensorFieldImpl parent, int[] orders)
    {
        var structureOfIndices = (StructureOfIndices[])parent.IndexTypeStructures.Clone();
        for (var i = 0; i < orders.Length; ++i)
        {
            for (var j = 0; j < orders[i]; ++j)
            {
                structureOfIndices[0] = structureOfIndices[0].Append(structureOfIndices[i + 1].GetInverted());
            }
        }

        return structureOfIndices;
    }

    private static int[] ValidateOrders(int[] orders)
    {
        ArgumentNullException.ThrowIfNull(orders);
        return orders;
    }

    private static NameDescriptorForTensorFieldImpl ValidateParent(NameDescriptorForTensorFieldImpl parent)
    {
        ArgumentNullException.ThrowIfNull(parent);
        return parent;
    }
}
