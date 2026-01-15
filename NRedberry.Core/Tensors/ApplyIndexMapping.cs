using System.Collections.Immutable;
using NRedberry.Core.Utils;
using NRedberry.IndexGeneration;
using NRedberry.IndexMapping;
using NRedberry.Indices;
using NRedberry.Numbers;
using NRedberry.Tensors.Functions;
using NRedberry.Transformations.Symmetrization;

namespace NRedberry.Tensors;

/// <summary>
/// Renames dummy indices of tensor prohibiting some dummy index to be equal to one of the specified forbidden indices.
/// </summary>
public sealed class ApplyIndexMapping
{
    public static Tensor RenameDummy(Tensor tensor, int[] forbiddenNames, int[] allowedDummiesNames)
    {
        if (forbiddenNames.Length == 0)
        {
            return tensor;
        }

        if (tensor is Complex or ScalarFunction)
        {
            return tensor;
        }

        HashSet<int> allIndicesNames = TensorUtils.GetAllDummyIndicesT(tensor);
        if (allIndicesNames.Count == 0)
        {
            return tensor;
        }

        allIndicesNames.EnsureCapacity(forbiddenNames.Length);

        List<int>? fromL = null;
        foreach (int forbidden in forbiddenNames)
        {
            if (!allIndicesNames.Add(forbidden))
            {
                fromL ??= [];
                fromL.Add(forbidden);
            }
        }

        if (fromL == null)
        {
            return tensor;
        }

        int[] freeIndices = IndicesUtils.GetIndicesNames(tensor.Indices.GetFree());
        for (int i = 0; i < freeIndices.Length; ++i)
        {
            allIndicesNames.Add(freeIndices[i]);
        }

        int[] from = fromL.ToArray();
        int[] to = new int[fromL.Count];
        Array.Sort(from);
        Dictionary<byte, Queue<int>> allowedGenerators = BuildAllowedIndexQueues(allowedDummiesNames);
        for (int i = from.Length - 1; i >= 0; --i)
        {
            to[i] = GenerateFromAllowed(allowedGenerators, IndicesUtils.GetType(from[i]));
        }

        return ApplyInternal(tensor, new IndexMapper(from, to), false);
    }

    public static Tensor RenameDummy(Tensor tensor, int[] forbiddenNames, HashSet<int> added)
    {
        if (forbiddenNames.Length == 0)
        {
            return tensor;
        }

        if (tensor is Complex or ScalarFunction)
        {
            return tensor;
        }

        HashSet<int> allIndicesNames = TensorUtils.GetAllDummyIndicesT(tensor);
        if (allIndicesNames.Count == 0)
        {
            return tensor;
        }

        allIndicesNames.EnsureCapacity(forbiddenNames.Length);

        List<int>? fromL = null;
        foreach (int forbidden in forbiddenNames)
        {
            if (!allIndicesNames.Add(forbidden))
            {
                fromL ??= [];
                fromL.Add(forbidden);
            }
        }

        if (fromL == null)
        {
            return tensor;
        }

        int[] freeIndices = IndicesUtils.GetIndicesNames(tensor.Indices.GetFree());
        for (int i = 0; i < freeIndices.Length; ++i)
        {
            allIndicesNames.Add(freeIndices[i]);
        }

        IndexGenerator generator = new(allIndicesNames.ToArray());
        int[] from = fromL.ToArray();
        int[] to = new int[fromL.Count];
        Array.Sort(from);
        added.EnsureCapacity(from.Length);
        for (int i = from.Length - 1; i >= 0; --i)
        {
            int generated = generator.Generate(IndicesUtils.GetType(from[i]));
            to[i] = generated;
            added.Add(generated);
        }

        return ApplyInternal(tensor, new IndexMapper(from, to), false);
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

        HashSet<int> allIndicesNames = TensorUtils.GetAllDummyIndicesT(tensor);
        if (allIndicesNames.Count == 0)
        {
            return tensor;
        }

        allIndicesNames.EnsureCapacity(forbiddenNames.Length);

        List<int>? fromL = null;
        foreach (int forbidden in forbiddenNames)
        {
            if (!allIndicesNames.Add(forbidden))
            {
                fromL ??= [];
                fromL.Add(forbidden);
            }
        }

        if (fromL == null)
        {
            return tensor;
        }

        int[] freeIndices = IndicesUtils.GetIndicesNames(tensor.Indices.GetFree());
        for (int i = 0; i < freeIndices.Length; ++i)
        {
            allIndicesNames.Add(freeIndices[i]);
        }

        IndexGenerator generator = new(allIndicesNames.ToArray());
        int[] from = fromL.ToArray();
        int[] to = new int[fromL.Count];
        Array.Sort(from);
        for (int i = from.Length - 1; i >= 0; --i)
        {
            to[i] = generator.Generate(IndicesUtils.GetType(from[i]));
        }

        return ApplyInternal(tensor, new IndexMapper(from, to), false);
    }

    public static Tensor OptimizeDummies(Tensor t)
    {
        if (t is SimpleTensor or ScalarFunction)
        {
            return t;
        }

        if (t is Sum or Expression)
        {
            Tensor[] oldData = t is Sum sum ? sum.Data : t.ToArray();
            Tensor[]? newData = null;
            DummiesContainer[] dummies = new DummiesContainer[t.Size];
            Dictionary<byte, MaxType> maxTypeCounts = new();
            for (int i = oldData.Length - 1; i >= 0; --i)
            {
                Tensor c = OptimizeDummies(oldData[i]);
                if (c != oldData[i])
                {
                    newData ??= (Tensor[])oldData.Clone();
                    newData[i] = c;
                }

                dummies[i] = new DummiesContainer(TensorUtils.GetAllDummyIndicesT(c));
                dummies[i].Update(i, maxTypeCounts);
            }

            int totalDummiesCount = 0;
            foreach (MaxType type in maxTypeCounts.Values)
            {
                totalDummiesCount += type.Count;
            }

            int[] totalDummies = new int[totalDummiesCount];
            int p = 0;
            foreach ((byte type, MaxType maxType) in maxTypeCounts)
            {
                dummies[maxType.Pointer].Write(type, p, maxType.Count, totalDummies);
                p += maxType.Count;
            }

            MasterDummiesContainer masterDummies = new(totalDummies);
            int[] from = new int[totalDummiesCount];
            int[] to = new int[totalDummiesCount];
            for (int i = oldData.Length - 1; i >= 0; --i)
            {
                if (dummies[i].Dummies.Length == 0)
                {
                    continue;
                }

                int count = 0;
                int typeStartInFrom = 0;
                byte previousType = IndicesUtils.GetType(dummies[i].Dummies[0]);
                int start = masterDummies.TypeStart(previousType, 0);
                int current = start;

                for (int j = 0; j < dummies[i].Dummies.Length; ++j)
                {
                    int index = dummies[i].Dummies[j];
                    byte type = IndicesUtils.GetType(index);
                    if (previousType != type)
                    {
                        for (int k = count - 1; k >= typeStartInFrom; --k)
                        {
                            to[k] = masterDummies.NextAndRemove(start);
                        }

                        previousType = type;
                        start = masterDummies.TypeStart(type, start);
                        typeStartInFrom = count;
                    }

                    if ((current = masterDummies.Remove(index, current)) < 0)
                    {
                        current = BinarySearchAbs(current);
                        from[count++] = index;
                    }
                }

                for (int k = count - 1; k >= typeStartInFrom; --k)
                {
                    to[k] = masterDummies.NextAndRemove(start);
                }

                masterDummies.Reset();

                if (count == 0)
                {
                    continue;
                }

                newData ??= (Tensor[])oldData.Clone();

                int[] fromSlice = new int[count];
                int[] toSlice = new int[count];
                Array.Copy(from, fromSlice, count);
                Array.Copy(to, toSlice, count);
                newData[i] = ApplyInternal(newData[i], new IndexMapper(fromSlice, toSlice), false);
            }

            if (newData == null)
            {
                return t;
            }

            return t is Sum
                ? new Sum(newData, t.Indices)
                : new Expression(t.Indices, newData[0], newData[1]);
        }

        return UnsafeMappingApply(t, new TensorTransformation(OptimizeDummies));
    }

    private static Tensor RenameDummyWithSign(Tensor tensor, int[] forbidden, bool sign)
    {
        Tensor result = RenameDummy(tensor, forbidden);
        return sign ? Tensors.Negate(result) : result;
    }

    public static Tensor ApplyIndexMappingAutomatically(Tensor tensor, Mapping mapping)
    {
        return ApplyIndexMappingAutomatically(tensor, mapping, []);
    }

    public static Tensor ApplyIndexMappingAutomatically(Tensor tensor, Mapping mapping, int[] forbidden)
    {
        if (mapping.IsEmpty() || tensor.Indices.GetFree().Size() == 0)
        {
            return RenameDummyWithSign(tensor, forbidden, mapping.GetSign());
        }

        int[] freeIndices = IndicesUtils.GetIndicesNames(tensor.Indices.GetFree());
        Array.Sort(freeIndices);

        int[] from = mapping.GetFromNames().ToArray();
        int[] to = mapping.GetToData().ToArray();

        int pointer = 0;
        int oldFromLength = from.Length;
        for (int i = 0; i < oldFromLength; ++i)
        {
            if (Arrays.BinarySearch(freeIndices, from[i]) >= 0)
            {
                from[pointer] = from[i];
                to[pointer] = to[i];
                ++pointer;
            }
        }

        if (pointer == 0)
        {
            return RenameDummyWithSign(tensor, forbidden, mapping.GetSign());
        }

        int newFromLength = pointer;

        ArraysUtils.QuickSort(from, 0, pointer, to);
        List<int> list = [];
        for (int i = 0; i < freeIndices.Length; ++i)
        {
            if (Arrays.BinarySearch(from, 0, pointer, freeIndices[i]) < 0)
            {
                if (newFromLength < oldFromLength)
                {
                    from[newFromLength] = freeIndices[i];
                    to[newFromLength] = freeIndices[i];
                }
                else
                {
                    list.Add(freeIndices[i]);
                }

                ++newFromLength;
            }
        }

        if (newFromLength < oldFromLength)
        {
            int[] newFrom = new int[newFromLength];
            int[] newTo = new int[newFromLength];
            Array.Copy(from, newFrom, newFromLength);
            Array.Copy(to, newTo, newFromLength);
            from = newFrom;
            to = newTo;
        }
        else if (newFromLength > oldFromLength)
        {
            int[] toAdd = list.ToArray();
            int[] newFrom = new int[newFromLength];
            int[] newTo = new int[newFromLength];
            Array.Copy(from, newFrom, oldFromLength);
            Array.Copy(to, newTo, oldFromLength);
            Array.Copy(toAdd, 0, newFrom, oldFromLength, toAdd.Length);
            Array.Copy(toAdd, 0, newTo, oldFromLength, toAdd.Length);
            from = newFrom;
            to = newTo;
        }

        return Apply(tensor, new Mapping(from, to, mapping.GetSign()), forbidden);
    }

    public static Tensor Apply(Tensor tensor, Mapping mapping)
    {
        return Apply(tensor, mapping, []);
    }

    public static Tensor Apply(Tensor tensor, Mapping mapping, int[] forbidden)
    {
        if (TensorUtils.IsZeroOrIndeterminate(tensor))
        {
            return tensor;
        }

        if (mapping.IsEmpty())
        {
            if (tensor.Indices.GetFree().Size() != 0)
            {
                throw new ArgumentException("From length does not match free indices size.");
            }

            return RenameDummyWithSign(tensor, forbidden, mapping.GetSign());
        }

        int[] freeIndicesNames = IndicesUtils.GetIndicesNames(tensor.Indices.GetFree());
        Array.Sort(freeIndicesNames);
        if (!EqualsToArray(mapping.GetFromNames(), freeIndicesNames))
        {
            string fromIndices;
            try
            {
                fromIndices = IndicesUtils.ToString(mapping.GetFromNames().ToArray());
            }
            catch (Exception)
            {
                fromIndices = "error";
            }

            throw new ArgumentException(
                $"From indices names ({fromIndices}) does not match free indices names of tensor ({IndicesUtils.ToString(freeIndicesNames)}).");
        }

        Tensor result = ApplyInternal(tensor, mapping, forbidden);
        return mapping.GetSign() ? Tensors.Negate(result) : result;
    }

    private static Tensor ApplyInternal(Tensor tensor, Mapping mapping, int[] forbidden)
    {
        int mappingSize = mapping.Size();
        int[] allForbidden = new int[mappingSize + forbidden.Length];
        int[] toData = mapping.GetToData().ToArray();
        int[] fromNames = mapping.GetFromNames().ToArray();
        Array.Copy(toData, 0, allForbidden, 0, mappingSize);
        Array.Copy(forbidden, 0, allForbidden, mappingSize, forbidden.Length);
        for (int i = allForbidden.Length - 1; i >= 0; --i)
        {
            allForbidden[i] = IndicesUtils.GetNameWithType(allForbidden[i]);
        }

        List<int> fromL = new(mappingSize);
        List<int> toL = new(mappingSize);
        fromL.AddRange(fromNames);
        toL.AddRange(toData);

        Array.Sort(allForbidden);

        int[] dummyIndices = TensorUtils.GetAllDummyIndicesT(tensor).ToArray();
        int[] forbiddenGeneratorIndices = new int[allForbidden.Length + dummyIndices.Length];
        Array.Copy(allForbidden, 0, forbiddenGeneratorIndices, 0, allForbidden.Length);
        Array.Copy(dummyIndices, 0, forbiddenGeneratorIndices, allForbidden.Length, dummyIndices.Length);

        IndexGenerator generator = new(forbiddenGeneratorIndices);
        foreach (int index in dummyIndices)
        {
            if (Arrays.BinarySearch(allForbidden, index) >= 0)
            {
                fromL.Add(index);
                toL.Add(generator.Generate(IndicesUtils.GetType(index)));
            }
        }

        int[] from = fromL.ToArray();
        int[] to = toL.ToArray();
        ArraysUtils.QuickSort(from, to);

        return ApplyInternal(tensor, new IndexMapper(from, to));
    }

    private static Tensor ApplyInternal(Tensor tensor, IndexMapper indexMapper)
    {
        if (tensor is SimpleTensor)
        {
            return ApplyInternal(tensor, indexMapper, false);
        }

        if (tensor is Complex or ScalarFunction)
        {
            return tensor;
        }

        return ApplyInternal(
            tensor,
            indexMapper,
            indexMapper.Contract(IndicesUtils.GetIndicesNames(tensor.Indices.GetFree())));
    }

    public static Tensor ApplyIndexMappingAndRenameAllDummies(Tensor tensor, Mapping mapping, int[] allowedDummies)
    {
        if (TensorUtils.IsZero(tensor))
        {
            return tensor;
        }

        int[] freeIndicesNames = IndicesUtils.GetIndicesNames(tensor.Indices.GetFree());
        Array.Sort(freeIndicesNames);
        if (!EqualsToArray(mapping.GetFromNames(), freeIndicesNames))
        {
            throw new ArgumentException(
                $"From indices names does not match free indices names of tensor. Tensor: {tensor} mapping: {mapping}");
        }

        int[] dummies = TensorUtils.GetAllDummyIndicesT(tensor).ToArray();
        int[] from = new int[mapping.Size() + dummies.Length];
        int[] to = new int[mapping.Size() + dummies.Length];
        int[] fromNames = mapping.GetFromNames().ToArray();
        int[] toData = mapping.GetToData().ToArray();
        Array.Copy(fromNames, 0, from, 0, mapping.Size());
        Array.Copy(toData, 0, to, 0, mapping.Size());
        Array.Copy(dummies, 0, from, mapping.Size(), dummies.Length);

        Dictionary<byte, Queue<int>> allowedGenerators = BuildAllowedIndexQueues(allowedDummies);
        for (int i = mapping.Size() + dummies.Length - 1, mappingSize = mapping.Size(); i >= mappingSize; --i)
        {
            to[i] = GenerateFromAllowed(allowedGenerators, IndicesUtils.GetType(from[i]));
        }

        ArraysUtils.QuickSort(from, to);
        tensor = ApplyInternal(tensor, new IndexMapper(from, to));
        if (mapping.GetSign())
        {
            tensor = Tensors.Negate(tensor);
        }

        return tensor;
    }

    private static Tensor ApplyInternal(Tensor tensor, IndexMapper indexMapper, bool contractIndices)
    {
        if (tensor is SimpleTensor)
        {
            SimpleTensor simpleTensor = (SimpleTensor)tensor;
            SimpleIndices oldIndices = simpleTensor.SimpleIndices;
            SimpleIndices newIndices = (SimpleIndices)oldIndices.ApplyIndexMapping(indexMapper);
            if (oldIndices == newIndices)
            {
                return tensor;
            }

            if (tensor is TensorField field)
            {
                return new TensorField(field.Name, newIndices, field.Arguments, field.ArgumentIndices);
            }

            return new SimpleTensor(simpleTensor.Name, newIndices);
        }

        if (tensor is Complex or ScalarFunction)
        {
            return tensor;
        }

        if (tensor is Expression)
        {
            bool contract = indexMapper.Contract(IndicesUtils.GetIndicesNames(tensor.Indices));
            Tensor newLhs = ApplyInternal(tensor[0], indexMapper, contract);
            Tensor newRhs = ApplyInternal(tensor[1], indexMapper, contract);
            if (newLhs != tensor[0] || newRhs != tensor[1])
            {
                return new Expression(tensor.Indices, newLhs, newRhs);
            }

            return tensor;
        }

        if (tensor is Power)
        {
            Tensor oldBase = tensor[0];
            Tensor newBase = ApplyInternal(oldBase, indexMapper, false);
            if (oldBase == newBase)
            {
                return tensor;
            }

            return new Power(newBase, tensor[1]);
        }

        if (contractIndices)
        {
            return ApplyToEachChild(tensor, tt => ApplyInternal(tt, indexMapper));
        }

        if (tensor is Product product)
        {
            Tensor[] indexless = product.IndexlessData;
            Tensor[]? newIndexless = null;
            Tensor[] data = product.Data;
            Tensor[]? newData = null;

            for (int i = indexless.Length - 1; i >= 0; --i)
            {
                Tensor oldTensor = indexless[i];
                Tensor newTensor = ApplyInternal(oldTensor, indexMapper, false);
                if (oldTensor != newTensor)
                {
                    newIndexless ??= (Tensor[])indexless.Clone();
                    newIndexless[i] = newTensor;
                }
            }

            for (int i = data.Length - 1; i >= 0; --i)
            {
                Tensor oldTensor = data[i];
                Tensor newTensor = ApplyInternal(oldTensor, indexMapper, false);
                if (oldTensor != newTensor)
                {
                    newData ??= (Tensor[])data.Clone();
                    newData[i] = newTensor;
                }
            }

            if (newIndexless == null && newData == null)
            {
                return tensor;
            }

            newIndexless ??= indexless;

            if (newData == null)
            {
                return new Product(product.Indices, product.Factor, newIndexless, data);
            }

            IndicesBuilder indicesBuilder = new();
            indicesBuilder.Append(newData);
            return new Product(indicesBuilder.Indices, product.Factor, newIndexless, newData);
        }

        if (tensor is Sum sum)
        {
            Tensor[] data = sum.Data;
            Tensor[]? newData = null;
            for (int i = data.Length - 1; i >= 0; --i)
            {
                Tensor oldTensor = data[i];
                Tensor newTensor = ApplyInternal(oldTensor, indexMapper, false);
                if (oldTensor != newTensor)
                {
                    newData ??= (Tensor[])data.Clone();
                    newData[i] = newTensor;
                }
            }

            if (newData == null)
            {
                return tensor;
            }

            return new Sum(newData, IndicesFactory.Create(newData[0].Indices.GetFree()));
        }

        throw new InvalidOperationException();
    }

    private static Tensor UnsafeMappingApply(Tensor tensor, ITransformation mapping)
    {
        if (tensor is SimpleTensor)
        {
            Tensor newTensor = mapping.Transform(tensor);
            return newTensor != tensor ? newTensor : tensor;
        }

        if (tensor is Complex or ScalarFunction)
        {
            return tensor;
        }

        if (tensor is Expression)
        {
            Tensor newLhs = mapping.Transform(tensor[0]);
            Tensor newRhs = mapping.Transform(tensor[1]);
            if (newLhs == tensor[0] && newRhs == tensor[1])
            {
                return tensor;
            }

            return new Expression(tensor.Indices, newLhs, newRhs);
        }

        if (tensor is Power)
        {
            Tensor oldBase = tensor[0];
            Tensor newBase = mapping.Transform(oldBase);
            if (oldBase == newBase)
            {
                return tensor;
            }

            return new Power(newBase, tensor[1]);
        }

        if (tensor is Product product)
        {
            Tensor[] indexless = product.IndexlessData;
            Tensor[]? newIndexless = null;
            Tensor[] data = product.Data;
            Tensor[]? newData = null;

            for (int i = indexless.Length - 1; i >= 0; --i)
            {
                Tensor oldTensor = indexless[i];
                Tensor newTensor = mapping.Transform(oldTensor);
                if (oldTensor != newTensor)
                {
                    newIndexless ??= (Tensor[])indexless.Clone();
                    newIndexless[i] = newTensor;
                }
            }

            for (int i = data.Length - 1; i >= 0; --i)
            {
                Tensor oldTensor = data[i];
                Tensor newTensor = mapping.Transform(oldTensor);
                if (oldTensor != newTensor)
                {
                    newData ??= (Tensor[])data.Clone();
                    newData[i] = newTensor;
                }
            }

            if (newIndexless == null && newData == null)
            {
                return tensor;
            }

            newIndexless ??= indexless;

            if (newData == null)
            {
                return new Product(product.Indices, product.Factor, newIndexless, data);
            }

            IndicesBuilder indicesBuilder = new();
            indicesBuilder.Append(newData);
            return new Product(indicesBuilder.Indices, product.Factor, newIndexless, newData);
        }

        if (tensor is Sum sum)
        {
            Tensor[] data = sum.Data;
            Tensor[]? newData = null;
            for (int i = data.Length - 1; i >= 0; --i)
            {
                Tensor oldTensor = data[i];
                Tensor newTensor = mapping.Transform(oldTensor);
                if (oldTensor != newTensor)
                {
                    newData ??= (Tensor[])data.Clone();
                    newData[i] = newTensor;
                }
            }

            if (newData == null)
            {
                return tensor;
            }

            return new Sum(newData, IndicesFactory.Create(newData[0].Indices.GetFree()));
        }

        throw new InvalidOperationException();
    }

    public static Tensor RenameIndicesOfFieldsArguments(Tensor tensor, HashSet<int> forbidden)
    {
        if (tensor is TensorField field)
        {
            Tensor[]? args = null;
            SimpleIndices[]? argsIndices = null;

            int[] _forbidden = forbidden.ToArray();
            for (int i = field.Arguments.Length - 1; i >= 0; --i)
            {
                Tensor arg = field.Arguments[i];
                int[] from = TensorUtils.GetAllIndicesNamesT(arg).ToArray();
                IndexGenerator ig = new(ArraysUtils.AddAll(_forbidden, from));
                Array.Sort(from);
                int[] to = new int[from.Length];
                for (int j = from.Length - 1; j >= 0; --j)
                {
                    if (forbidden.Contains(from[j]))
                    {
                        to[j] = ig.Generate(IndicesUtils.GetType(from[j]));
                    }
                    else
                    {
                        to[j] = from[j];
                    }

                    forbidden.Add(to[j]);
                }

                IndexMapper mapping = new(from, to);
                arg = ApplyInternal(arg, mapping);
                if (arg != field.Arguments[i])
                {
                    if (args == null)
                    {
                        args = (Tensor[])field.Arguments.Clone();
                        argsIndices = (SimpleIndices[])field.ArgumentIndices.Clone();
                    }

                    args[i] = arg;
                    argsIndices![i] = (SimpleIndices)field.ArgumentIndices[i].ApplyIndexMapping(mapping);
                }
            }

            if (args == null)
            {
                return tensor;
            }

            return new TensorField(field.Name, field.SimpleIndices, args, argsIndices!);
        }

        if (tensor is Product product)
        {
            Tensor[]? data = null;
            Tensor[]? indexless = null;
            for (int i = product.Data.Length - 1; i >= 0; --i)
            {
                Tensor temp = RenameIndicesOfFieldsArguments(product.Data[i], forbidden);
                if (temp != product.Data[i])
                {
                    data ??= (Tensor[])product.Data.Clone();
                    data[i] = temp;
                }
            }

            for (int i = product.IndexlessData.Length - 1; i >= 0; --i)
            {
                Tensor temp = RenameIndicesOfFieldsArguments(product.IndexlessData[i], forbidden);
                if (temp != product.IndexlessData[i])
                {
                    indexless ??= (Tensor[])product.IndexlessData.Clone();
                    indexless[i] = temp;
                }
            }

            if (data == null && indexless == null)
            {
                return tensor;
            }

            data ??= product.Data;
            indexless ??= product.IndexlessData;

            return new Product(product.Indices, product.Factor, indexless, data);
        }

        if (tensor is Sum sum)
        {
            Tensor[]? data = null;
            for (int i = sum.Size - 1; i >= 0; --i)
            {
                Tensor temp = RenameIndicesOfFieldsArguments(sum.Data[i], forbidden);
                if (temp != sum.Data[i])
                {
                    data ??= (Tensor[])sum.Data.Clone();
                    data[i] = temp;
                }
            }

            if (data == null)
            {
                return tensor;
            }

            return new Sum(data, sum.Indices);
        }

        if (tensor is Complex or SimpleTensor)
        {
            return tensor;
        }

        if (tensor is Power)
        {
            Tensor a = RenameIndicesOfFieldsArguments(tensor[0], forbidden);
            Tensor b = RenameIndicesOfFieldsArguments(tensor[1], forbidden);
            if (a == tensor[0] && b == tensor[1])
            {
                return tensor;
            }

            return new Power(a, b);
        }

        if (tensor is Expression)
        {
            Tensor a = RenameIndicesOfFieldsArguments(tensor[0], forbidden);
            Tensor b = RenameIndicesOfFieldsArguments(tensor[1], forbidden);
            if (a == tensor[0] && b == tensor[1])
            {
                return tensor;
            }

            return new Expression(tensor.Indices, a, b);
        }

        if (tensor is ScalarFunction)
        {
            Tensor arg = RenameIndicesOfFieldsArguments(tensor[0], forbidden);
            if (arg == tensor[0])
            {
                return tensor;
            }

            TensorFactory? factory = tensor.GetFactory();
            if (factory == null)
            {
                throw new InvalidOperationException();
            }

            return factory.Create(arg);
        }

        throw new InvalidOperationException();
    }

    private static void CheckConsistent(Tensor tensor, int[] from)
    {
        int[] freeIndices = tensor.Indices.GetFree().AllIndices.ToArray();
        Array.Sort(freeIndices);
        if (!freeIndices.SequenceEqual(from))
        {
            throw new ArgumentException("From indices are not equal to free indices of tensor.");
        }
    }

    public static Tensor InvertIndices(Tensor tensor)
    {
        var indices = tensor.Indices.GetFree();
        if (indices.Size() == 0)
        {
            return tensor;
        }

        return Apply(tensor, new Mapping(indices.AllIndices.ToArray(), indices.GetInverted().AllIndices.ToArray()));
    }

    private static Tensor ApplyToEachChild(Tensor tensor, Func<Tensor, Tensor> transformation)
    {
        TensorBuilder? builder = null;
        for (int i = 0, size = tensor.Size; i < size; ++i)
        {
            Tensor oldChild = tensor[i];
            Tensor newChild = transformation(oldChild);
            if (builder != null || newChild != oldChild)
            {
                if (builder == null)
                {
                    builder = tensor.GetBuilder();
                    for (int j = 0; j < i; ++j)
                    {
                        builder.Put(tensor[j]);
                    }
                }

                builder.Put(newChild);
            }
        }

        return builder == null ? tensor : builder.Build();
    }

    private static bool EqualsToArray(ImmutableArray<int> array, int[] other)
    {
        if (array.Length != other.Length)
        {
            return false;
        }

        for (int i = 0; i < array.Length; ++i)
        {
            if (array[i] != other[i])
            {
                return false;
            }
        }

        return true;
    }

    private static Dictionary<byte, Queue<int>> BuildAllowedIndexQueues(int[] allowedIndices)
    {
        Dictionary<byte, Queue<int>> generators = new();
        if (allowedIndices.Length == 0)
        {
            return generators;
        }

        int[] data = (int[])allowedIndices.Clone();
        Array.Sort(data);
        byte type = IndicesUtils.GetType(data[0]);
        data[0] = IndicesUtils.GetNameWithoutType(data[0]);
        int prevIndex = 0;
        for (int i = 1; i < data.Length; ++i)
        {
            byte currentType = IndicesUtils.GetType(data[i]);
            if (currentType != type)
            {
                generators[type] = new Queue<int>(data[prevIndex..i]);
                prevIndex = i;
                type = currentType;
            }

            data[i] = IndicesUtils.GetNameWithoutType(data[i]);
        }

        generators[type] = new Queue<int>(data[prevIndex..]);
        return generators;
    }

    private static int GenerateFromAllowed(Dictionary<byte, Queue<int>> generators, byte type)
    {
        if (!generators.TryGetValue(type, out Queue<int>? generator) || generator.Count == 0)
        {
            throw new IndexOutOfRangeException(
                "No allowed indices with specified type: " + IndexTypeMethods.GetType(type));
        }

        return IndicesUtils.SetType(type, generator.Dequeue());
    }

    private static int BinarySearchAbs(int i)
    {
        return i < 0 ? ~i : i;
    }

    private sealed record class MaxType
    {
        public MaxType(int count, int pointer)
        {
            Count = count;
            Pointer = pointer;
        }

        public int Count { get; set; }
        public int Pointer { get; set; }
    }

    private sealed record class MasterDummiesContainer
    {
        private readonly int[] totalDummies;
        private readonly bool[] removed;

        public MasterDummiesContainer(int[] totalDummies)
        {
            this.totalDummies = totalDummies;
            Array.Sort(this.totalDummies);
            removed = new bool[totalDummies.Length];
        }

        public void Reset()
        {
            Array.Fill(removed, false);
        }

        public int Remove(int index, int start)
        {
            int result = Arrays.BinarySearch(totalDummies, start, totalDummies.Length, index);
            if (result >= 0)
            {
                removed[result] = true;
            }

            return result;
        }

        public int TypeStart(byte type, int from)
        {
            return BinarySearchAbs(Arrays.BinarySearch(totalDummies, from, totalDummies.Length, type << 24));
        }

        public int NextAndRemove(int start)
        {
            for (int i = start; i < removed.Length; ++i)
            {
                if (!removed[i])
                {
                    removed[i] = true;
                    return totalDummies[i];
                }
            }

            throw new InvalidOperationException("No available dummy indices.");
        }
    }

    private sealed record class DummiesContainer
    {
        public DummiesContainer(HashSet<int> dummiesT)
        {
            Dummies = dummiesT.ToArray();
            Array.Sort(Dummies);
        }

        public int[] Dummies { get; }

        public void Update(int pointer, Dictionary<byte, MaxType> maxTypeValues)
        {
            int previousPointer = 0;
            while (previousPointer < Dummies.Length)
            {
                byte type = IndicesUtils.GetType(Dummies[previousPointer]);
                int current = BinarySearchAbs(
                    Arrays.BinarySearch(Dummies, previousPointer, Dummies.Length, (type + 1) << 24));
                int count = current - previousPointer;
                if (!maxTypeValues.TryGetValue(type, out MaxType? typeInfo))
                {
                    maxTypeValues[type] = new MaxType(count, pointer);
                }
                else if (typeInfo.Count < count)
                {
                    typeInfo.Count = count;
                    typeInfo.Pointer = pointer;
                }

                previousPointer = current;
            }
        }

        public void Write(byte type, int start, int count, int[] dest)
        {
            int startPointer = BinarySearchAbs(Arrays.BinarySearch(Dummies, 0, Dummies.Length, type << 24));
            Array.Copy(Dummies, startPointer, dest, start, count);
        }
    }

    private sealed record class TensorTransformation(Func<Tensor, Tensor> TransformFunc) : ITransformation
    {
        public Tensor Transform(Tensor t)
        {
            return TransformFunc(t);
        }
    }
}
