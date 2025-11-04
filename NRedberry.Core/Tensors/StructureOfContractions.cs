using System.Text;

namespace NRedberry.Core.Tensors;

public sealed class StructureOfContractions
{
    public static readonly StructureOfContractions EmptyFullContractionsStructure = new([]);
    public readonly int[][] contractions;
    public readonly int[] components;
    public readonly int componentCount;

    public StructureOfContractions(int[][] contractions)
    {
        this.contractions = contractions;
        components = new int[contractions.Length];
        Array.Fill(components, -1);
        int componentCounter = -1;
        for (int i = 0; i < contractions.Length; i++)
        {
            if (components[i] == -1)
            {
                components[i] = ++componentCounter;
                FillComponents(components, componentCounter, i);
            }
        }

        componentCount = componentCounter + 1;
    }

    private void FillComponents(int[] components, int component, int position)
    {
        foreach (long l in contractions[position])
        {
            int to = ToPosition(l);
            if (to != -1 && components[to] == -1)
            {
                components[to] = component;
                FillComponents(components, component, to);
            }
        }
    }

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < contractions.Length; i++)
        {
            foreach (long l in contractions[i])
            {
                sb.Append(i).Append("_").Append(FromIPosition(l)).Append(" -> ").Append(ToPosition(l)).Append("_").Append(ToIDiffId(l));
                sb.Append("\n");
            }
        }

        return sb.ToString();
    }

    public static int ToPosition(long contraction)
    {
        return (int)(contraction >> 32);
    }

    public static short ToIDiffId(long contraction)
    {
        return (short)(0xFFFF & (contraction >> 16));
    }

    public static int FromIPosition(long contraction)
    {
        return (int)(0xFFFF & contraction);
    }
}
