using System;
using System.Collections.Generic;
using System.Linq;
using NRedberry.Core.Indices;
using NRedberry.Core.Numbers;
using NRedberry.Core.Tensors.Functions;

namespace NRedberry.Core.Tensors;

/// <summary>
/// Renames dummy indices of tensor prohibiting some dummy index to be equal to one of the specified forbidden indices.
/// </summary>
public sealed class ApplyIndexMapping
{
    public static Tensor RenameDummy(Tensor tensor, int[] forbiddenNames, HashSet<int> added)
    {
        throw new NotImplementedException();
    }

    public static Tensor RenameDummy(Tensor tensor, params int[] forbiddenNames)
    {
        if (forbiddenNames.Length == 0)
        {
            return tensor;
        }

        if (tensor is Complex or ScalarFunction)
        {
            return tensor;
        }

        var allIndicesNames = TensorUtils.GetAllDummyIndicesT(tensor);
        if (!allIndicesNames.Any())
        {
            return tensor;
        }

        allIndicesNames.EnsureCapacity(forbiddenNames.Length);

        IList<int> fromL = new List<int>();
        foreach (var forbidden in forbiddenNames)
        {
            if (!allIndicesNames.Add(forbidden))
            {
                fromL.Add(forbidden);
            }
        }

        if (allIndicesNames.Count == 0)
        {
            return tensor;
        }

        foreach (var index in tensor.Indices.GetFree())
        {
            allIndicesNames.Add(index);
        }

        var generator = new IndexGenerator(allIndicesNames.ToArray());

        int[] from = fromL.ToArray(), to = new int[fromL.Count];
        Array.Sort(from);
        int i;
        for (i = from.Length - 1; i >= 0; --i)
            to[i] = generator.Generate(IndicesUtils.GetType(from[i]));

        return applyIndexMapping(tensor, new IndexMapper(from, to), false);
    }

    private static Tensor applyIndexMapping(Tensor tensor, IndexMapper indexMapper, bool contractIndices)
    {
        throw new NotImplementedException();
    }
}