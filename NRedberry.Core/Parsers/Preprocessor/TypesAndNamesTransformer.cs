using NRedberry.Contexts;
using NRedberry.Core.Utils;

namespace NRedberry.Parsers.Preprocessor;

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
            ArgumentNullException.ThrowIfNull(from);
            ArgumentNullException.ThrowIfNull(to);

            var from0 = (int[])from.Clone();
            var to0 = (int[])to.Clone();
            ArraysUtils.QuickSort(from0, to0);

            return new Transformer(
                (oldType, _) => oldType,
                (oldIndex, _) =>
                {
                    int index = Array.BinarySearch(from0, oldIndex);
                    return index >= 0 ? to0[index] : oldIndex;
                },
                (oldName, _) => oldName);
        }

        public static TypesAndNamesTransformer ChangeType(IndexType oldType, IndexType newType)
        {
            return new Transformer(
                (old, _) => old == oldType ? newType : old,
                (oldIndex, _) => oldIndex,
                (oldName, _) => oldName);
        }

        public static TypesAndNamesTransformer ChangeName(string[] from, string[] to)
        {
            ArgumentNullException.ThrowIfNull(from);
            ArgumentNullException.ThrowIfNull(to);

            var from0 = (string[])from.Clone();
            var to0 = (string[])to.Clone();
            ArraysUtils.QuickSort(from0, to0);

            return new Transformer(
                (oldType, _) => oldType,
                (oldIndex, _) => oldIndex,
                (_, descriptor) =>
                {
                    int index = Array.BinarySearch(from0, descriptor.Name);
                    return index >= 0 ? to0[index] : descriptor.Name;
                });
        }

        public static TypesAndNamesTransformer And(params TypesAndNamesTransformer[] transformers)
        {
            ArgumentNullException.ThrowIfNull(transformers);

            return new Transformer(
                (oldType, descriptor) =>
                {
                    var current = oldType;
                    foreach (var transformer in transformers)
                    {
                        current = transformer.NewType(current, descriptor);
                    }

                    return current;
                },
                (oldIndex, descriptor) =>
                {
                    var current = oldIndex;
                    foreach (var transformer in transformers)
                    {
                        current = transformer.NewIndex(current, descriptor);
                    }

                    return current;
                },
                (oldName, descriptor) =>
                {
                    var current = oldName;
                    foreach (var transformer in transformers)
                    {
                        current = transformer.NewName(current, descriptor);
                    }

                    return current;
                });
        }

        private sealed record Transformer(
            Func<IndexType, NameAndStructureOfIndices, IndexType> TypeTransformer,
            Func<int, NameAndStructureOfIndices, int> IndexTransformer,
            Func<string, NameAndStructureOfIndices, string> NameTransformer)
            : TypesAndNamesTransformer
        {
            public IndexType NewType(IndexType oldType, NameAndStructureOfIndices descriptor)
            {
                return TypeTransformer(oldType, descriptor);
            }

            public int NewIndex(int oldIndex, NameAndStructureOfIndices descriptor)
            {
                return IndexTransformer(oldIndex, descriptor);
            }

            public string NewName(string oldName, NameAndStructureOfIndices descriptor)
            {
                return NameTransformer(oldName, descriptor);
            }
        }
    }
}
