using System.Text;
using NRedberry.Concurrent;
using NRedberry.Core.Combinatorics;
using NRedberry.Core.Utils;
using NRedberry.Groups;
using NRedberry.IndexMapping;
using NRedberry.Indices;
using NRedberry.Numbers;
using NRedberry.Tensors.Functions;
using NRedberry.Tensors.Iterators;

namespace NRedberry.Tensors;

public static class TensorUtils
{
    public static string Info(Tensor expr)
    {
        var sb = new StringBuilder();
        sb
            .Append("// [")
            .Append(expr.GetType().Name)
            .Append(",\n//  ")
            .Append("size = ")
            .Append(expr.Size)
            .Append(",\n//  ")
            .Append("symbolic = ")
            .Append(IsSymbolic(expr))
            .Append(",\n//  ")
            .Append("freeIndices = ")
            .Append(expr.Indices.GetFree())
            .Append(",\n//  ")
            .Append("indices = ")
            .Append(expr.Indices)
            .Append(",\n//  ")
            .Append("symbolsCount = ")
            .Append(SymbolsCount(expr))
            .Append(",\n//  ")
            .Append("symbolsAppear = ")
            .Append(GetAllDiffSimpleTensors(expr))
            .Append("\n//]");
        return sb.ToString();
    }

    public static long SymbolsCount(Tensor expr)
    {
        long counter = 0;
        SymbolsCount(expr, ref counter);
        return counter;
    }

    private static void SymbolsCount(Tensor expr, ref long counter)
    {
        if (expr is SimpleTensor)
        {
            counter++;
        }

        foreach (Tensor t in expr)
        {
            SymbolsCount(t, ref counter);
        }
    }

    public static bool HaveIndicesIntersections(Tensor u, Tensor v)
    {
        return IndicesUtils.HaveIntersections(u.Indices, v.Indices);
    }

    public static bool IsZeroOrIndeterminate(Tensor tensor)
    {
        return tensor is Complex complex && NumberUtils.IsZeroOrIndeterminate(complex);
    }

    public static bool IsIndeterminate(Tensor tensor)
    {
        return tensor is Complex complex && NumberUtils.IsIndeterminate(complex);
    }

    public static bool IsInteger(Tensor tensor)
    {
        return tensor is Complex complex && complex.IsInteger();
    }

    public static bool IsNaturalNumber(Tensor tensor)
    {
        return tensor is Complex complex && complex.IsNatural();
    }

    public static bool IsNumeric(Tensor tensor)
    {
        return tensor is Complex complex && complex.IsNumeric();
    }

    public static bool IsNegativeNaturalNumber(Tensor tensor)
    {
        return tensor is Complex complex && complex.IsNegativeInteger();
    }

    public static bool IsPositiveNaturalNumber(Tensor tensor)
    {
        return tensor is Complex complex && complex.IsPositiveNatural();
    }

    public static bool IsRealPositiveNumber(Tensor tensor)
    {
        if (tensor is Complex complex)
        {
            return complex.IsReal() && complex.GetReal().SigNum() > 0;
        }

        return false;
    }

    public static bool IsRealNegativeNumber(Tensor tensor)
    {
        if (tensor is Complex complex)
        {
            return complex.IsReal() && complex.GetReal().SigNum() < 0;
        }

        return false;
    }

    public static bool IsIndexless(params Tensor[] tensors)
    {
        foreach (Tensor t in tensors)
        {
            if (!IsIndexlessInternal(t))
            {
                return false;
            }
        }

        return true;
    }

    private static bool IsIndexlessInternal(Tensor tensor)
    {
        return tensor.Indices.Size() == 0;
    }

    public static bool IsScalar(params Tensor[] tensors)
    {
        foreach (Tensor t in tensors)
        {
            if (!IsScalarInternal(t))
            {
                return false;
            }
        }

        return true;
    }

    private static bool IsScalarInternal(Tensor tensor)
    {
        return tensor.Indices.GetFree().Size() == 0;
    }

    public static bool IsSymbol(Tensor t)
    {
        return t.GetType() == typeof(SimpleTensor) && t.Indices.Size() == 0;
    }

    public static bool IsSymbolOrNumber(Tensor t)
    {
        return t is Complex || IsSymbol(t);
    }

    public static bool IsSymbolic(Tensor t)
    {
        if (t.Indices.Size() != 0)
        {
            return false;
        }

        foreach (Tensor c in t)
        {
            if (!IsSymbolic(c))
            {
                return false;
            }
        }

        return true;
    }

    public static bool IsSymbolic(params Tensor[] tensors)
    {
        foreach (Tensor t in tensors)
        {
            if (!IsSymbolic(t))
            {
                return false;
            }
        }

        return true;
    }

    public static bool IsOne(Tensor tensor)
    {
        return tensor is Complex complex && complex.IsOne();
    }

    public static bool IsZero(Tensor tensor)
    {
        return tensor is Complex complex && complex.IsZero();
    }

    public static bool IsImageOne(Tensor tensor)
    {
        return tensor is Complex && tensor.Equals(Complex.ImaginaryOne);
    }

    public static bool IsMinusOne(Tensor tensor)
    {
        return tensor is Complex && tensor.Equals(Complex.MinusOne);
    }

    public static bool IsIntegerOdd(Tensor tensor)
    {
        return tensor is Complex complex && NumberUtils.IsIntegerOdd(complex);
    }

    public static bool IsIntegerEven(Tensor tensor)
    {
        return tensor is Complex complex && NumberUtils.IsIntegerEven(complex);
    }

    public static bool IsPositiveIntegerPower(Tensor t)
    {
        return t is Power && IsPositiveNaturalNumber(t[1]);
    }

    public static bool IsPositiveIntegerPowerOfSimpleTensor(Tensor t)
    {
        return IsPositiveIntegerPower(t) && t[0] is SimpleTensor;
    }

    public static bool IsPositiveIntegerPowerOfProduct(Tensor t)
    {
        return IsPositiveIntegerPower(t) && t[0] is Product;
    }

    public static bool IsNegativeIntegerPower(Tensor t)
    {
        return t is Power && IsNegativeNaturalNumber(t[1]);
    }

    public static bool PassOutDummies(Tensor tensor)
    {
        return GetAllDummyIndicesT(tensor).Count != 0;
    }

    public static bool ContainsSimpleTensors(Tensor tensor, HashSet<int> setOfNames)
    {
        var iterator = new FromChildToParentIterator(tensor);
        Tensor? current;
        while ((current = iterator.Next()) is not null)
        {
            if (current is SimpleTensor simpleTensor && setOfNames.Contains(simpleTensor.Name))
            {
                return true;
            }
        }

        return false;
    }

    public static bool HasImaginaryPart(Tensor t)
    {
        if (t is Complex complex)
        {
            return !complex.Imaginary.IsZero();
        }

        foreach (Tensor f in t)
        {
            if (HasImaginaryPart(f))
            {
                return true;
            }
        }

        return false;
    }

    public static bool EqualsExactly(Tensor[] u, Tensor[] v)
    {
        if (u.Length != v.Length)
        {
            return false;
        }

        for (int i = 0; i < u.Length; ++i)
        {
            if (!EqualsExactly(u[i], v[i]))
            {
                return false;
            }
        }

        return true;
    }

    public static bool EqualsExactly(Tensor u, string v)
    {
        return EqualsExactly(u, Tensors.Parse(v));
    }

    public static bool EqualsExactly(Tensor u, Tensor v)
    {
        if (ReferenceEquals(u, v))
        {
            return true;
        }

        if (u.GetType() != v.GetType())
        {
            return false;
        }

        if (u is Complex)
        {
            return u.Equals(v);
        }

        if (u.GetHashCode() != v.GetHashCode())
        {
            return false;
        }

        if (u.GetType() == typeof(SimpleTensor))
        {
            return u.Indices.Equals(v.Indices);
        }

        if (u.Size != v.Size)
        {
            return false;
        }

        if (u is MultiTensor)
        {
            int size = u.Size;
            int[] hashArray = new int[size];
            for (int i = 0; i < size; ++i)
            {
                if ((hashArray[i] = u[i].GetHashCode()) != v[i].GetHashCode())
                {
                    return false;
                }
            }

            int begin = 0;
            for (int i = 1; i <= size; ++i)
            {
                if (i == size || hashArray[i] != hashArray[i - 1])
                {
                    if (i - 1 != begin)
                    {
                        int stretchLength = i - begin;
                        bool[] usedPos = new bool[stretchLength];
                        for (int n = begin; n < i; ++n)
                        {
                            bool matched = false;
                            for (int j = begin; j < i; ++j)
                            {
                                if (!usedPos[j - begin] && EqualsExactly(u[n], v[j]))
                                {
                                    usedPos[j - begin] = true;
                                    matched = true;
                                    break;
                                }
                            }

                            if (!matched)
                            {
                                return false;
                            }
                        }

                        return true;
                    }

                    if (!EqualsExactly(u[i - 1], v[i - 1]))
                    {
                        return false;
                    }

                    begin = i;
                }
            }
        }

        if (u.GetType() == typeof(TensorField))
        {
            var su = (SimpleTensor)u;
            var sv = (SimpleTensor)v;
            if (su.Name != sv.Name || !u.Indices.Equals(v.Indices))
            {
                return false;
            }
        }

        int totalSize = u.Size;
        for (int i = 0; i < totalSize; ++i)
        {
            if (!EqualsExactly(u[i], v[i]))
            {
                return false;
            }
        }

        return true;
    }

    public static HashSet<int> GetAllDummyIndicesT(Tensor tensor)
    {
        return GetAllDummyIndicesT(false, tensor);
    }

    public static HashSet<int> GetAllDummyIndicesIncludingScalarFunctionsT(Tensor tensor)
    {
        return GetAllDummyIndicesT(true, tensor);
    }

    private static HashSet<int> GetAllDummyIndicesT(bool includeScalarFunctions, Tensor tensor)
    {
        var set = new HashSet<int>();
        AppendAllIndicesNamesT(tensor, set, includeScalarFunctions);
        int[] free = IndicesUtils.GetIndicesNames(tensor.Indices.GetFree());
        foreach (int index in free)
        {
            set.Remove(index);
        }

        return set;
    }

    public static HashSet<int> GetAllIndicesNamesT(IEnumerable<Tensor> tensors)
    {
        var set = new HashSet<int>();
        foreach (Tensor tensor in tensors)
        {
            AppendAllIndicesNamesT(tensor, set, false);
        }

        return set;
    }

    public static HashSet<int> GetAllIndicesNamesT(params Tensor[] tensors)
    {
        var set = new HashSet<int>();
        foreach (Tensor tensor in tensors)
        {
            AppendAllIndicesNamesT(tensor, set, false);
        }

        return set;
    }

    public static void AppendAllIndicesNamesT(Tensor tensor, HashSet<int> set)
    {
        AppendAllIndicesNamesT(tensor, set, false);
    }

    public static void AppendAllIndicesNamesIncludingScalarFunctionsT(Tensor tensor, HashSet<int> set)
    {
        AppendAllIndicesNamesT(tensor, set, true);
    }

    private static void AppendAllIndicesNamesT(Tensor tensor, HashSet<int> set, bool includeScalarFunctions)
    {
        if (tensor is SimpleTensor)
        {
            Indices.Indices ind = tensor.Indices;
            set.EnsureCapacity(set.Count + ind.Size());
            int size = ind.Size();
            for (int i = 0; i < size; ++i)
            {
                set.Add(IndicesUtils.GetNameWithType(ind[i]));
            }
        }
        else if (tensor is Power)
        {
            AppendAllIndicesNamesT(tensor[0], set, includeScalarFunctions);
        }
        else if (tensor is ScalarFunction && !includeScalarFunctions)
        {
            return;
        }
        else
        {
            for (int i = tensor.Size - 1; i >= 0; --i)
            {
                Tensor t = tensor[i];
                AppendAllIndicesNamesT(t, set, includeScalarFunctions);
            }
        }
    }

    public static bool Equals(Tensor u, Tensor v)
    {
        return IndexMappings.Equals(u, v);
    }

    public static bool? Compare1(Tensor u, Tensor v)
    {
        return IndexMappings.Compare1(u, v);
    }

    public static void AssertIndicesConsistency(Tensor t)
    {
        AssertIndicesConsistency(t, new HashSet<int>());
    }

    private static void AssertIndicesConsistency(Tensor t, HashSet<int> indices)
    {
        if (t is SimpleTensor)
        {
            Indices.Indices ind = t.Indices;
            for (int i = ind.Size() - 1; i >= 0; --i)
            {
                if (indices.Contains(ind[i]))
                {
                    throw new InconsistentIndicesException(ind[i]);
                }

                indices.Add(ind[i]);
            }
        }

        if (t is Product)
        {
            for (int i = t.Size - 1; i >= 0; --i)
            {
                AssertIndicesConsistency(t[i], indices);
            }
        }

        if (t is Sum)
        {
            var sumIndices = new HashSet<int>();
            for (int i = t.Size - 1; i >= 0; --i)
            {
                var temp = new HashSet<int>(indices);
                AssertIndicesConsistency(t[i], temp);
                AppendAllIndicesT(t[i], sumIndices);
            }

            indices.UnionWith(sumIndices);
        }

        if (t is Expression)
        {
            foreach (Tensor c in t)
            {
                AssertIndicesConsistency(c, new HashSet<int>(indices));
            }
        }
    }

    private static void AppendAllIndicesT(Tensor tensor, HashSet<int> set)
    {
        if (tensor is SimpleTensor)
        {
            Indices.Indices ind = tensor.Indices;
            int size = ind.Size();
            for (int i = 0; i < size; ++i)
            {
                set.Add(ind[i]);
            }
        }
        else if (tensor is Power)
        {
            AppendAllIndicesT(tensor[0], set);
        }
        else
        {
            for (int i = tensor.Size - 1; i >= 0; --i)
            {
                Tensor t = tensor[i];
                if (t is ScalarFunction)
                {
                    continue;
                }

                AppendAllIndicesT(t, set);
            }
        }
    }

    public static bool IsZeroDueToSymmetry(Tensor t)
    {
        return IndexMappings.IsZeroDueToSymmetry(t);
    }

    private static Permutation GetSymmetryFromMapping1(int[] sortedIndicesNames, int[] sortPermutation, Mapping mapping)
    {
        int dimension = sortedIndicesNames.Length;

        int[] fromIndices = mapping.GetFromNames().ToArray();
        int[] toIndices = mapping.GetToData().ToArray();

        int[] permutation = new int[dimension];
        Array.Fill(permutation, -1);

        for (int i = 0; i < dimension; ++i)
        {
            int fromIndex = sortedIndicesNames[i];
            int positionInFrom = ArraysUtils.BinarySearch1(fromIndices, fromIndex);
            if (positionInFrom < 0)
            {
                continue;
            }

            int positionInIndices = Array.BinarySearch(
                sortedIndicesNames,
                IndicesUtils.GetNameWithType(toIndices[positionInFrom]));

            if (positionInIndices < 0)
            {
                return Permutations.CreateIdentityPermutation(dimension);
            }

            permutation[sortPermutation[i]] = sortPermutation[positionInIndices];
        }

        for (int i = 0; i < dimension; ++i)
        {
            if (permutation[i] == -1)
            {
                permutation[i] = i;
            }
        }

        return Permutations.CreatePermutation(mapping.GetSign(), permutation);
    }

    public static Permutation GetSymmetryFromMapping(int[] indices, Mapping mapping)
    {
        int[] sortedIndicesNames = IndicesUtils.GetIndicesNames(indices);
        int[] sortPermutation = ArraysUtils.QuickSortP(sortedIndicesNames);
        return GetSymmetryFromMapping1(sortedIndicesNames, sortPermutation, mapping);
    }

    public static List<Permutation> GetSymmetriesFromMappings(int[] indices, IOutputPort<IIndexMappingBuffer> mappingsPort)
    {
        var symmetries = new List<Permutation>();
        int[] sortedIndicesNames = IndicesUtils.GetIndicesNames(indices);
        int[] sortPermutation = ArraysUtils.QuickSortP(sortedIndicesNames);
        while (true)
        {
            IIndexMappingBuffer? buffer = mappingsPort.Take();
            if (buffer is null)
            {
                break;
            }

            symmetries.Add(GetSymmetryFromMapping1(sortedIndicesNames, sortPermutation, new Mapping(buffer)));
        }

        return symmetries;
    }

    public static List<Permutation> FindIndicesSymmetries(int[] indices, Tensor tensor)
    {
        return GetSymmetriesFromMappings(indices, IndexMappings.CreatePortOfBuffers(tensor, tensor));
    }

    public static List<Permutation> FindIndicesSymmetries(SimpleIndices indices, Tensor tensor)
    {
        return GetSymmetriesFromMappings(indices.AllIndices.ToArray(), IndexMappings.CreatePortOfBuffers(tensor, tensor));
    }

    public static List<Permutation> GetIndicesSymmetriesForIndicesWithSameStates(int[] indices, Tensor tensor)
    {
        List<Permutation> total = FindIndicesSymmetries(indices, tensor);
        var symmetries = new List<Permutation>();
        foreach (Permutation s in total)
        {
            bool ok = true;
            for (int i = 0; i < indices.Length; ++i)
            {
                if (IndicesUtils.GetRawStateInt(indices[i]) != IndicesUtils.GetRawStateInt(indices[s.NewIndexOf(i)]))
                {
                    ok = false;
                    break;
                }
            }

            if (ok)
            {
                symmetries.Add(s);
            }
        }

        return symmetries;
    }

    public static HashSet<SimpleTensor> GetAllSymbols(params Tensor[] tensors)
    {
        var set = new HashSet<SimpleTensor>();
        foreach (Tensor tensor in tensors)
        {
            AddSymbols(tensor, set);
        }

        return set;
    }

    private static void AddSymbols(Tensor tensor, HashSet<SimpleTensor> set)
    {
        if (IsSymbol(tensor))
        {
            set.Add((SimpleTensor)tensor);
        }
        else
        {
            foreach (Tensor t in tensor)
            {
                AddSymbols(t, set);
            }
        }
    }

    public static HashSet<SimpleTensor> GetAllSymbolsAndSymbolicFields(params Tensor[] tensors)
    {
        var set = new HashSet<SimpleTensor>();
        foreach (Tensor tensor in tensors)
        {
            AddSymbols(tensor, set);
        }

        return set;
    }

    private static void AddSymbolsAndSymbolicFields(Tensor tensor, HashSet<SimpleTensor> set)
    {
        if (tensor is SimpleTensor && tensor.Indices.Size() == 0)
        {
            bool contentSymbolic = true;
            foreach (Tensor t in tensor)
            {
                if (!IsSymbolic(t))
                {
                    contentSymbolic = false;
                    break;
                }
            }

            if (contentSymbolic)
            {
                set.Add((SimpleTensor)tensor);
            }
        }
        else
        {
            foreach (Tensor t in tensor)
            {
                AddSymbolsAndSymbolicFields(t, set);
            }
        }
    }

    public static ICollection<SimpleTensor> GetAllDiffSimpleTensors(params Tensor[] tensors)
    {
        var names = new Dictionary<int, SimpleTensor>();
        foreach (Tensor tensor in tensors)
        {
            AddAllDiffSimpleTensors(tensor, names);
        }

        return names.Values;
    }

    private static void AddAllDiffSimpleTensors(Tensor tensor, Dictionary<int, SimpleTensor> names)
    {
        if (tensor is SimpleTensor simpleTensor)
        {
            names[simpleTensor.Name] = simpleTensor;
        }
        else
        {
            foreach (Tensor t in tensor)
            {
                AddAllDiffSimpleTensors(t, names);
            }
        }
    }

    public static HashSet<int> GetAllNamesOfSymbols(params Tensor[] tensors)
    {
        var set = new HashSet<int>();
        foreach (Tensor tensor in tensors)
        {
            AddSymbolsNames(tensor, set);
        }

        return set;
    }

    private static void AddSymbolsNames(Tensor tensor, HashSet<int> set)
    {
        if (IsSymbol(tensor))
        {
            set.Add(((SimpleTensor)tensor).Name);
        }
        else
        {
            foreach (Tensor t in tensor)
            {
                AddSymbolsNames(t, set);
            }
        }
    }

    public static int TreeDepth(Tensor tensor)
    {
        if (tensor.GetType() == typeof(SimpleTensor) || tensor is Complex)
        {
            return 0;
        }

        int depth = 1;
        foreach (Tensor t in tensor)
        {
            int temp = TreeDepth(t) + 1;
            if (temp > depth)
            {
                depth = temp;
            }
        }

        return depth;
    }

    public static Tensor Det(Tensor[][] matrix)
    {
        CheckMatrix(matrix);
        return Det1(matrix);
    }

    public static Tensor[][] Inverse(Tensor[][] matrix)
    {
        CheckMatrix(matrix);
        if (matrix.Length == 1)
        {
            return [[Tensors.Reciprocal(matrix[0][0])]];
        }

        Tensor det = Det(matrix);
        int length = matrix.Length;
        Tensor[][] inverse = new Tensor[length][];
        for (int i = 0; i < length; ++i)
        {
            inverse[i] = new Tensor[length];
        }

        for (int i = 0; i < length; ++i)
        {
            for (int j = 0; j < length; ++j)
            {
                inverse[j][i] = Tensors.DivideAndRenameConflictingDummies(Det(DeleteFromMatrix(matrix, i, j)), det);
                if ((i + j) % 2 != 0)
                {
                    inverse[j][i] = Tensors.Negate(inverse[j][i]);
                }
            }
        }

        return inverse;
    }

    private static void CheckMatrix(Tensor[][] tensors)
    {
        if (tensors.Length == 0)
        {
            throw new InvalidOperationException("Empty matrix.");
        }

        int cc = tensors.Length;
        foreach (Tensor[] tt in tensors)
        {
            if (tt.Length != cc)
            {
                throw new ArgumentException("Non square matrix");
            }
        }
    }

    private static Tensor Det1(Tensor[][] matrix)
    {
        if (matrix.Length == 1)
        {
            return matrix[0][0];
        }

        var sum = new SumBuilder();
        for (int i = 0; i < matrix.Length; ++i)
        {
            Tensor temp = Tensors.MultiplyAndRenameConflictingDummies(matrix[0][i], Det(DeleteFromMatrix(matrix, 0, i)));
            if (i % 2 == 1)
            {
                temp = Tensors.Negate(temp);
            }

            sum.Put(temp);
        }

        return sum.Build();
    }

    private static Tensor[][] DeleteFromMatrix(Tensor[][] matrix, int row, int column)
    {
        if (matrix.Length == 1)
        {
            return Array.Empty<Tensor[]>();
        }

        Tensor[][] newMatrix = new Tensor[matrix.Length - 1][];
        int cRow = 0;
        for (int i = 0; i < matrix.Length; ++i)
        {
            if (i == row)
            {
                continue;
            }

            newMatrix[cRow] = new Tensor[matrix.Length - 1];
            int cColumn = 0;
            for (int j = 0; j < matrix.Length; ++j)
            {
                if (j == column)
                {
                    continue;
                }

                newMatrix[cRow][cColumn++] = matrix[i][j];
            }

            ++cRow;
        }

        return newMatrix;
    }

    public static bool ContainsFractions(Tensor tensor)
    {
        if (tensor is SimpleTensor)
        {
            return false;
        }

        if (tensor is Power)
        {
            return IsNegativeNaturalNumber(tensor[1]);
        }

        foreach (Tensor t in tensor)
        {
            if (ContainsFractions(t))
            {
                return true;
            }
        }

        return false;
    }

    public static HashSet<int> GetSimpleTensorsNames(Tensor t)
    {
        return AddSimpleTensorsNames(t, new HashSet<int>());
    }

    private static HashSet<int> AddSimpleTensorsNames(Tensor t, HashSet<int> names)
    {
        if (t is TensorField field)
        {
            names.Add(field.GetNameDescriptor().GetParent().Id);
        }

        if (t is SimpleTensor simpleTensor)
        {
            names.Add(simpleTensor.Name);
        }

        foreach (Tensor tt in t)
        {
            AddSimpleTensorsNames(tt, names);
        }

        return names;
    }

    public static bool ShareSimpleTensors(Tensor a, Tensor b)
    {
        return TestContainsNames(b, GetSimpleTensorsNames(a));
    }

    private static bool TestContainsNames(Tensor t, HashSet<int> names)
    {
        if (t is TensorField field)
        {
            if (names.Contains(field.GetNameDescriptor().GetParent().Id))
            {
                return true;
            }
        }
        else if (t is SimpleTensor simpleTensor)
        {
            return names.Contains(simpleTensor.Name);
        }

        foreach (Tensor tt in t)
        {
            if (TestContainsNames(tt, names))
            {
                return true;
            }
        }

        return false;
    }

    public static Expression[] GenerateReplacementsOfScalars(Tensor tensor)
    {
        return GenerateReplacementsOfScalars(tensor, CC.GetParametersGenerator());
    }

    public static Expression[] GenerateReplacementsOfScalars(Tensor tensor, IOutputPort<SimpleTensor> generatedCoefficients)
    {
        var scalars = new HashSet<Tensor>();
        var iterator = new FromChildToParentIterator(tensor);
        Tensor? c;
        while ((c = iterator.Next()) is not null)
        {
            if (c is Product product)
            {
                foreach (Tensor scalar in product.Content.Scalars)
                {
                    scalars.Add(scalar);
                }
            }
        }

        var replacements = new Expression[scalars.Count];
        int i = -1;
        foreach (Tensor scalar in scalars)
        {
            replacements[++i] = Tensors.Expression(scalar, generatedCoefficients.Take());
        }

        return replacements;
    }

    public static int Count(Tensor expression, params Tensor[] patterns)
    {
        return Count(expression, 1, patterns.ToList(), false);
    }

    public static int Count(Tensor expression, List<Tensor> patterns)
    {
        return Count(expression, 1, patterns, false);
    }

    public static int Count(Tensor expression, int level, List<Tensor> patterns, bool sumPowers)
    {
        if (level == 0)
        {
            return 0;
        }

        if (level < 0)
        {
            throw new ArgumentException();
        }

        int count = 0;
        if (level == 1)
        {
            foreach (Tensor el in expression)
            {
                foreach (Tensor p in patterns)
                {
                    int c = Match0(el, p, sumPowers);
                    count += c;
                    if (c > 0)
                    {
                        break;
                    }
                }
            }
        }
        else
        {
            foreach (Tensor el in expression)
            {
                count += Count(el, level - 1, patterns, sumPowers);
            }
        }

        return count;
    }

    public static int Exponent(Tensor expression, params Tensor[] pattern)
    {
        return Exponent(expression, pattern.ToList());
    }

    public static int Exponent(Tensor expression, List<Tensor> pattern)
    {
        if (expression is SimpleTensor)
        {
            return Match1(expression, pattern);
        }

        if (IsPositiveIntegerPower(expression))
        {
            return ((Complex)expression[1]).IntValue() * Exponent(expression[0], pattern);
        }

        if (expression is Product)
        {
            int exponent = 0;
            foreach (Tensor tensor in expression)
            {
                exponent += Exponent(tensor, pattern);
            }

            return exponent;
        }

        if (expression is Sum)
        {
            int exponent = 0;
            foreach (Tensor tensor in expression)
            {
                exponent = Math.Max(exponent, Exponent(tensor, pattern));
            }

            return exponent;
        }

        return 0;
    }

    private static int Match0(Tensor el, Tensor patt, bool sumPowers)
    {
        if (sumPowers && IsPositiveIntegerPower(el))
        {
            return ((Complex)el[1]).IntValue() * Match0(el[0], patt, false);
        }

        if (IndexMappings.AnyMappingExists(patt, el))
        {
            return 1;
        }

        if (patt is TensorField pattField
            && el is TensorField elField
            && !pattField.IsDerivative())
        {
            return elField.GetParentField().Name == pattField.Name ? 1 : 0;
        }

        return 0;
    }

    private static int Match1(Tensor el, List<Tensor> patterns)
    {
        foreach (Tensor patt in patterns)
        {
            if (IndexMappings.AnyMappingExists(patt, el))
            {
                return 1;
            }

            if (patt is TensorField pattField
                && el is TensorField elField
                && !pattField.IsDerivative()
                && elField.GetParentField().Name == pattField.Name)
            {
                return 1;
            }
        }

        return 0;
    }
}
