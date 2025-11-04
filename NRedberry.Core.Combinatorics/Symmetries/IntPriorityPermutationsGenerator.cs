namespace NRedberry.Core.Combinatorics.Symmetries;

public sealed class IntPriorityPermutationsGenerator : IIntCombinatorialPort
{
    private readonly IntPermutationsGenerator generator;
    private readonly List<PermutationPriorityTuple> tuples = [];
    private readonly HashSet<PermutationPriorityTuple> set = [];
    private int[] last = [];
    private int lastTuplePointer;

    public IntPriorityPermutationsGenerator(int dimension)
    {
        generator = new IntPermutationsGenerator(dimension);
    }

    public IntPriorityPermutationsGenerator(int[] initialPermutation)
    {
        generator = new IntPermutationsGenerator(initialPermutation);
    }

    public int[] Take()
    {
        if (lastTuplePointer == tuples.Count)
        {
            int[] next;
            do
            {
                if (!generator.MoveNext())
                {
                    return [];
                }

                next = generator.Current;
            }
            while (set.Contains(new PermutationPriorityTuple(next)));

            last = next;
            return next;
        }

        return tuples[lastTuplePointer++].Permutation;
    }

    public void Nice()
    {
        if (last.Length == 0)
        {
            var index = lastTuplePointer - 1;
            var nPriority = ++tuples[index].Priority;
            var position = index;
            while (--position >= 0 && tuples[position].Priority < nPriority)
            {
            }

            ++position;
            Swap(position, index);
            return;
        }

        var tuple = new PermutationPriorityTuple((int[])last.Clone());
        set.Add(tuple);
        tuples.Add(tuple);
        ++lastTuplePointer;
    }

    public void Reset()
    {
        generator.Reset();
        lastTuplePointer = 0;
        last = [];
    }

    public int[] GetReference() => tuples[lastTuplePointer - 1].Permutation;

    private void Swap(int i, int j) => (tuples[i], tuples[j]) = (tuples[j], tuples[i]);
}
