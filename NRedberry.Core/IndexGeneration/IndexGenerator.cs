using NRedberry.Indices;

namespace NRedberry.IndexGeneration;

public sealed class IndexGenerator : ICloneable, ICloneable<IndexGenerator>
{
    private readonly Dictionary<byte, IntGenerator> _generators = new();

    /// <summary>
    /// Creates with generator without engaged data.
    /// </summary>
    public IndexGenerator()
    {
    }

    public IndexGenerator(Indices.Indices indices)
        : this(indices.AllIndices.ToArray())
    {
    }

    public IndexGenerator(Dictionary<byte, IntGenerator> generators)
    {
        ArgumentNullException.ThrowIfNull(generators);
        _generators = generators;
    }

    public IndexGenerator(params int[] indices)
    {
        if (indices.Length == 0)
        {
            return;
        }

        for (int i = 0; i < indices.Length; ++i)
        {
            indices[i] = IndicesUtils.GetNameWithType(indices[i]);
        }

        Array.Sort(indices);
        byte type = IndicesUtils.GetType(indices[0]);
        indices[0] = IndicesUtils.GetNameWithoutType(indices[0]);
        int prevIndex = 0;
        for (int i = 1; i < indices.Length; ++i)
        {
            if (IndicesUtils.GetType(indices[i]) != type)
            {
                _generators[type] = new IntGenerator(indices[prevIndex..i]);
                prevIndex = i;
                type = IndicesUtils.GetType(indices[i]);
            }

            indices[i] = IndicesUtils.GetNameWithoutType(indices[i]);
        }

        _generators[type] = new IntGenerator(indices[prevIndex..]);
    }

    public bool Contains(int index)
    {
        byte type = IndicesUtils.GetType(index);
        if (!_generators.TryGetValue(type, out IntGenerator? intGen))
        {
            return false;
        }

        return intGen.Contains(IndicesUtils.GetNameWithoutType(index));
    }

    public void MergeFrom(IndexGenerator other)
    {
        ArgumentNullException.ThrowIfNull(other);
        foreach ((byte type, IntGenerator otherGenerator) in other._generators)
        {
            if (!_generators.TryGetValue(type, out IntGenerator? thisGenerator))
            {
                _generators[type] = otherGenerator.Clone();
            }
            else
            {
                thisGenerator.MergeFrom(otherGenerator);
            }
        }
    }

    public int Generate(IndexType type)
    {
        return Generate(type.GetType_());
    }

    public int Generate(byte type)
    {
        if (!_generators.TryGetValue(type, out IntGenerator? intGenerator))
        {
            intGenerator = new IntGenerator();
            _generators[type] = intGenerator;
        }

        return IndicesUtils.SetType(type, intGenerator.GetNext());
    }

    public IndexGenerator Clone()
    {
        Dictionary<byte, IntGenerator> newMap = new(_generators.Count);
        foreach ((byte type, IntGenerator generator) in _generators)
        {
            newMap[type] = generator.Clone();
        }

        return new IndexGenerator(newMap);
    }

    object ICloneable.Clone() => Clone();
}
