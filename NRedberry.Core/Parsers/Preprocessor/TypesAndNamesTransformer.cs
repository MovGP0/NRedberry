using System;
using NRedberry;
using NRedberry.Contexts;

namespace NRedberry.Core.Parsers.Preprocessor;

/*
 * Original: ./core/src/main/java/cc/redberry/core/parser/preprocessor/TypesAndNamesTransformer.java
 */

public interface TypesAndNamesTransformer
{
    IndexType NewType(IndexType oldType, NameAndStructureOfIndices descriptor);

    int NewIndex(int oldIndex, NameAndStructureOfIndices descriptor);

    string NewName(string oldName, NameAndStructureOfIndices descriptor);

    public static class Utils
    {
        public static TypesAndNamesTransformer SetIndices(int[] from, int[] to)
        {
            throw new NotImplementedException();
        }

        public static TypesAndNamesTransformer ChangeType(IndexType oldType, IndexType newType)
        {
            throw new NotImplementedException();
        }
    }
}
