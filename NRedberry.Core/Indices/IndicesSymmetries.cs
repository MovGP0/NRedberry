using System;
using System.Collections;
using System.Collections.Generic;
using NRedberry.Core.Combinatorics;

namespace NRedberry.Core.Indices;

/// <summary>
/// Representation of permutational symmetries of indices of simple tensors.
/// </summary>
public class IndicesSymmetries : IEnumerable<Symmetry>
{
    public IEnumerator<Symmetry> GetEnumerator()
    {
        throw new NotImplementedException();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public static IndicesSymmetries Create(StructureOfIndices indexTypeStructure)
    {
        throw new NotImplementedException();
    }
}