using NRedberry.Concurrent;
using NRedberry.Core.Combinatorics;
using NRedberry.Tensors;

namespace NRedberry.Transformations.Substitutions;

/// <summary>
/// Port of cc.redberry.core.transformations.substitutions.IndexlessBijectionsPort.
/// </summary>
public sealed class IndexlessBijectionsPort : IOutputPort<int[]>
{
    private readonly IntDistinctTuplesPort? _combinationsPort;
    private readonly bool _finished;

    public IndexlessBijectionsPort(Tensor[] from, Tensor[] to)
    {
        ArgumentNullException.ThrowIfNull(from);
        ArgumentNullException.ThrowIfNull(to);

        if (from.Length > to.Length)
        {
            _finished = true;
            return;
        }

        int[][] hashReflections = new int[from.Length][];
        for (int i = 0; i < from.Length; ++i)
        {
            List<int> reflections = [];
            int hash = from[i].GetHashCode();

            int j = 0;
            for (; j < to.Length; ++j)
            {
                if (to[j].GetHashCode() >= hash)
                {
                    break;
                }
            }

            if (j == to.Length || to[j].GetHashCode() > hash)
            {
                _finished = true;
                return;
            }

            for (; j < to.Length; ++j)
            {
                if (to[j].GetHashCode() != hash)
                {
                    break;
                }

                reflections.Add(j);
            }

            hashReflections[i] = [.. reflections];
        }

        _combinationsPort = new IntDistinctTuplesPort(hashReflections);
    }

    public int[] Take()
    {
        if (_finished)
        {
            return null;
        }

        return _combinationsPort!.Take();
    }
}
