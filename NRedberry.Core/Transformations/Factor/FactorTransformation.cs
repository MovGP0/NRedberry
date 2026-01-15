using System.Text;
using BigInteger = System.Numerics.BigInteger;
using NRedberry.Numbers;
using NRedberry.Tensors;
using NRedberry.Tensors.Functions;
using NRedberry.Tensors.Iterators;
using NRedberry.Transformations.Fractions;
using NRedberry.Transformations.Options;
using NRedberry.Transformations.Symmetrization;
using NRedberry.Utils;

namespace NRedberry.Transformations.Factor;

/// <summary>
/// Skeleton port of cc.redberry.core.transformations.factor.FactorTransformation.
/// </summary>
public sealed class FactorTransformation : ITransformation, TransformationToStringAble
{
    /// <summary>
    /// Singleton instance.
    /// </summary>
    public static FactorTransformation Instance { get; } = new(true, JasFactor.Engine);

    private readonly bool factorScalars;

    public ITransformation FactorizationEngine { get; }

    /// <summary>
    /// Specifies whether scalar but not symbolic (i.e. scalar indexed expressions) should be factorized
    /// on a par with symbolic (i.e. without any indices) expressions.
    /// </summary>
    public FactorTransformation(bool factorScalars, ITransformation factorizationEngine)
    {
        ArgumentNullException.ThrowIfNull(factorizationEngine);

        this.factorScalars = factorScalars;
        FactorizationEngine = factorizationEngine;
    }

    [Creator]
    public FactorTransformation([Options] FactorOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);

        factorScalars = options.FactorScalars;
        FactorizationEngine = options.FactorizationEngine ?? throw new ArgumentNullException(nameof(options));
    }

    public Tensor Transform(Tensor tensor)
    {
        ArgumentNullException.ThrowIfNull(tensor);

        if (factorScalars)
        {
            Expression[] replacementsOfScalars = TensorUtils.GenerateReplacementsOfScalars(
                tensor,
                new LocalSymbolsProvider(tensor, "sclr"));
            foreach (Expression e in replacementsOfScalars)
            {
                tensor = e.Transform(tensor);
            }

            tensor = FactorSymbolicTerms(tensor);
            foreach (Expression e in replacementsOfScalars)
            {
                tensor = e.Transpose().Transform(tensor);
            }

            return tensor;
        }

        return FactorSymbolicTerms(tensor);
    }

    private Tensor FactorSymbolicTerms(Tensor tensor)
    {
        var iterator = new FromParentToChildIterator(tensor);
        Tensor? current;
        while ((current = iterator.Next()) is not null)
        {
            if (current is not Sum sum)
            {
                continue;
            }

            Tensor remainder = current;
            var symbolicPositions = new List<int>();
            for (int i = current.Size - 1; i >= 0; --i)
            {
                Tensor temp = current[i];
                if (IsSymbolic(temp))
                {
                    symbolicPositions.Add(i);
                    if (remainder is Sum remainderSum)
                    {
                        remainder = remainderSum.Remove(i);
                    }
                    else
                    {
                        remainder = Complex.Zero;
                    }
                }
            }

            Tensor symbolicPart = sum.Select(symbolicPositions.ToArray());
            symbolicPart = FactorSymbolicTerm(symbolicPart);
            if (remainder is Sum remainderSum1)
            {
                var sb = new SumBuilder(remainderSum1.Size);
                foreach (Tensor tt in remainderSum1)
                {
                    sb.Put(FactorSymbolicTerms(tt));
                }

                remainder = sb.Build();
            }
            else
            {
                remainder = FactorSymbolicTerms(remainder);
            }

            iterator.Set(Tensors.Tensors.Sum(symbolicPart, remainder));
        }

        return iterator.Result();
    }

    private Tensor FactorSymbolicTerm(Tensor sum)
    {
        Tensor? current;
        if (FactorizationEngine is JasFactor)
        {
            ITreeIterator iterator = new FromChildToParentIterator(sum);
            while ((current = iterator.Next()) is not null)
            {
                if (current is Sum)
                {
                    iterator.Set(FactorOut(current));
                }
            }

            sum = iterator.Result();

            iterator = new FromParentToChildIterator(sum);
            while ((current = iterator.Next()) is not null)
            {
                if (current is not Sum)
                {
                    continue;
                }

                if (NeedTogether(current))
                {
                    current = TogetherTransformation.Together(current, this);
                    if (current is Product)
                    {
                        TensorBuilder? pb = null;
                        for (int i = current.Size - 1; i >= 0; --i)
                        {
                            if (current[i] is Sum)
                            {
                                if (pb is null)
                                {
                                    pb = current.GetBuilder();
                                    for (int j = current.Size - 1; j > i; --j)
                                    {
                                        pb.Put(current[j]);
                                    }
                                }

                                pb.Put(FactorSum1(current[i]));
                            }
                            else if (pb is not null)
                            {
                                pb.Put(current[i]);
                            }
                        }

                        iterator.Set(pb is null ? current : pb.Build());
                    }
                    else
                    {
                        iterator.Set(current);
                    }
                }
                else
                {
                    iterator.Set(FactorSum1(current));
                }
            }

            return iterator.Result();
        }

        return FactorizationEngine.Transform(sum);
    }

    private Tensor FactorSum1(Tensor sum)
    {
        Tensor[] parts = ReIm(sum);
        if (!TensorUtils.IsZero(parts[0]))
        {
            Tensor im = parts[0];
            if (im is Sum imSum)
            {
                im = FastTensors.MultiplySumElementsOnFactor(imSum, Complex.ImaginaryOne);
            }
            else
            {
                im = Tensors.Tensors.Multiply(im, Complex.ImaginaryOne);
            }

            im = FactorizationEngine.Transform(im);
            im = Tensors.Tensors.Multiply(im, Complex.ImaginaryMinusOne);
            parts[0] = im;
        }

        if (!TensorUtils.IsZero(parts[1]))
        {
            parts[1] = FactorizationEngine.Transform(parts[1]);
        }

        return Tensors.Tensors.Sum(parts[0], parts[1]);
    }

    /// <summary>
    /// Factors scalar parts of tensor over the integers.
    /// </summary>
    public static Tensor Factor(Tensor tensor, bool factorScalars, ITransformation factorizationEngine)
    {
        return new FactorTransformation(factorScalars, factorizationEngine).Transform(tensor);
    }

    /// <summary>
    /// Factors scalar parts of tensor over the integers.
    /// </summary>
    public static Tensor Factor(Tensor tensor, bool factorScalars)
    {
        return Factor(tensor, factorScalars, JasFactor.Engine);
    }

    /// <summary>
    /// Factors scalar parts of tensor over the integers.
    /// </summary>
    public static Tensor Factor(Tensor tensor)
    {
        return Factor(tensor, true, JasFactor.Engine);
    }

    private static Tensor[] ReIm(Tensor sum)
    {
        var im = new List<int>(sum.Size);
        for (int i = sum.Size - 1; i >= 0; --i)
        {
            Tensor term = sum[i];
            if (term is Complex complex && !complex.Imaginary.IsZero())
            {
                im.Add(i);
            }
            else if (term is Product product && !product.Factor.Imaginary.IsZero())
            {
                im.Add(i);
            }
        }

        Tensor[] parts = new Tensor[2];
        int[] positions = im.ToArray();
        parts[0] = ((Sum)sum).Select(positions);
        parts[1] = ((Sum)sum).Remove(positions);
        return parts;
    }

    private static bool NeedTogether(Tensor t)
    {
        if (t is Power)
        {
            if (NeedTogether(t[0]))
            {
                return true;
            }

            return ((Complex)t[1]).Real.SigNum() < 0;
        }

        if (t is SimpleTensor)
        {
            return false;
        }

        foreach (Tensor tt in t)
        {
            if (NeedTogether(tt))
            {
                return true;
            }
        }

        return false;
    }

    private bool IsSymbolic(Tensor t)
    {
        // in case of Wolfram etc., just check that there are no indices
        if (FactorizationEngine is not JasFactor)
        {
            return TensorUtils.IsSymbolic(t);
        }

        return IsSymbolicPoly(t);
    }

    private static bool IsSymbolicPoly(Tensor t)
    {
        if (t.Indices.Size() != 0 || t is ScalarFunction)
        {
            return false;
        }

        if (t is SimpleTensor)
        {
            return t.Size == 0;
        }

        if (t is Power)
        {
            if (!IsSymbolicPoly(t[0]))
            {
                return false;
            }

            if (!TensorUtils.IsInteger(t[1]))
            {
                return false;
            }

            var exponent = (Complex)t[1];
            return exponent.IsReal() && !exponent.IsNumeric();
        }

        foreach (Tensor tt in t)
        {
            if (!IsSymbolicPoly(tt))
            {
                return false;
            }
        }

        return true;
    }

    private Tensor FactorOut(Tensor tensor)
    {
        var iterator = new FromChildToParentIterator(tensor);
        Tensor? current;
        while ((current = iterator.Next()) is not null)
        {
            if (current is Sum)
            {
                iterator.Set(FactorOut1(current));
            }
        }

        return iterator.Result();
    }

    private static bool IsProductOfSums(Tensor tensor)
    {
        if (tensor is Sum)
        {
            return false;
        }

        if (tensor is Product)
        {
            foreach (Tensor t in tensor)
            {
                if (IsIntegerPowerOfSum(t))
                {
                    return true;
                }
            }
        }

        return IsIntegerPowerOfSum(tensor);
    }

    private static bool IsIntegerPowerOfSum(Tensor tensor)
    {
        if (tensor is Sum)
        {
            return true;
        }

        return tensor is Power && tensor[0] is Sum && TensorUtils.IsInteger(tensor[1]);
    }

    private Tensor FactorOut1(Tensor tensor)
    {
        // S0: factor out imaginary numbers: I*a + I*b
        bool? factorOutImaginaryOne = null;
        bool containsImaginaryOne;
        foreach (Tensor t in tensor)
        {
            if (t is Product product)
            {
                containsImaginaryOne = product.Factor.IsImaginary();
            }
            else if (t is Complex complex)
            {
                containsImaginaryOne = complex.IsImaginary();
            }
            else
            {
                containsImaginaryOne = false;
            }

            if (factorOutImaginaryOne is null)
            {
                factorOutImaginaryOne = containsImaginaryOne;
            }
            else if (factorOutImaginaryOne != containsImaginaryOne)
            {
                factorOutImaginaryOne = false;
            }
        }

        if (factorOutImaginaryOne == true)
        {
            tensor = FastTensors.MultiplySumElementsOnFactor((Sum)tensor, Complex.ImaginaryMinusOne);
        }

        if (tensor is not Sum)
        {
            if (factorOutImaginaryOne == true)
            {
                tensor = Tensors.Tensors.Multiply(Complex.ImaginaryOne, tensor);
            }

            return FactorOut(tensor);
        }

        // S1: (a+b)*c + a*d + b*d -> (a+b)*c + (a+b)*d
        Tensor temp = tensor;
        int j = temp.Size;
        var nonProductOfSumsPositions = new List<int>();
        for (int i = 0; i < j; ++i)
        {
            if (!IsProductOfSums(temp[i]))
            {
                nonProductOfSumsPositions.Add(i);
            }
        }

        // S2: find product of sums with minimal number of multipliers
        FactorTerm[] terms;
        var pivotPosition = new FactorInt();
        if (nonProductOfSumsPositions.Count == 0 || nonProductOfSumsPositions.Count == temp.Size)
        {
            terms = SumToSplitArray((Sum)temp, pivotPosition);
        }
        else
        {
            var sb = new SumBuilder();
            for (int i = nonProductOfSumsPositions.Count - 1; i >= 0; --i)
            {
                sb.Put(((Sum)temp)[nonProductOfSumsPositions[i]]);
                temp = ((Sum)temp).Remove(nonProductOfSumsPositions[i]);
            }

            Tensor withoutSumsTerm = FactorSymbolicTerms(sb.Build());
            if (IsProductOfSums(withoutSumsTerm))
            {
                temp = Tensors.Tensors.Sum(temp, withoutSumsTerm);
                if (temp is not Sum)
                {
                    // (a+b)*c - a*c - b*c = (a+b)*c - (a+b)*c = 0
                    return temp;
                }

                terms = SumToSplitArray((Sum)temp, pivotPosition);
            }
            else
            {
                if (temp is not Sum)
                {
                    terms = [TensorToTerm(temp), TensorToTerm(withoutSumsTerm)];
                }
                else
                {
                    terms = new FactorTerm[temp.Size + 1];
                    Array.Copy(SumToSplitArray((Sum)temp, pivotPosition), 0, terms, 0, temp.Size);
                    terms[temp.Size] = TensorToTerm(withoutSumsTerm);
                }

                pivotPosition.Value = terms[pivotPosition.Value].Factors.Length > terms[^1].Factors.Length
                    ? terms.Length - 1
                    : pivotPosition.Value;
            }
        }

        temp = MergeTerms(terms, pivotPosition.Value, tensor);
        if (factorOutImaginaryOne == true)
        {
            temp = Tensors.Tensors.Multiply(Complex.ImaginaryOne, temp);
        }

        return temp;
    }

    private static Tensor MergeTerms(FactorTerm[] terms, int pivotPosition, Tensor tensor)
    {
        // S3: initialize reference in pivot factors
        FactorTerm pivot = terms[pivotPosition];
        var baseFactors = new List<FactorTermNode>(pivot.Factors.Length);
        foreach (FactorTermNode node in pivot.Factors)
        {
            FactorTermNode baseNode = node.Clone();
            baseNode.MinExponent = new FactorBigInt();
            baseFactors.Add(baseNode);
        }

        // S4: merge all terms
        BigInteger baseExponent;
        BigInteger tempExponent;
        List<FactorTermNode>? tempList;
        bool? sign = null;
        for (int i = terms.Length - 1; i >= 0; --i)
        {
            if (baseFactors.Count == 0)
            {
                return tensor;
            }

            for (int j = baseFactors.Count - 1; j >= 0; --j)
            {
                FactorTermNode baseNode = baseFactors[j];
                if (!terms[i].Map.TryGetValue(baseNode.Tensor.GetHashCode(), out tempList))
                {
                    baseFactors.RemoveAt(j);
                    continue;
                }

                foreach (FactorTermNode nn in tempList)
                {
                    sign = TensorUtils.Compare1(baseNode.Tensor, nn.Tensor);
                    if (sign is null)
                    {
                        continue;
                    }

                    baseExponent = baseNode.Exponent;
                    tempExponent = nn.Exponent;

                    if (baseExponent.Sign != tempExponent.Sign)
                    {
                        baseFactors.RemoveAt(j);
                        continue;
                    }

                    if (sign == true)
                    {
                        nn.DiffSigns = true;
                    }

                    nn.MinExponent = baseNode.MinExponent;

                    if (baseExponent.Sign > 0 && baseExponent.CompareTo(tempExponent) > 0)
                    {
                        baseNode.Exponent = tempExponent;
                    }
                    else if (baseExponent.Sign < 0 && baseExponent.CompareTo(tempExponent) < 0)
                    {
                        baseNode.Exponent = tempExponent;
                    }

                    break;
                }

                if (sign is null)
                {
                    baseFactors.RemoveAt(j);
                }
            }
        }

        if (baseFactors.Count == 0)
        {
            return tensor;
        }

        var pb = new ScalarsBackedProductBuilder(baseFactors.Count);
        foreach (FactorTermNode node in baseFactors)
        {
            pb.Put(node.ToTensor());
            node.MinExponent!.Value = node.Exponent;
        }

        var sb = new SumBuilder(tensor.Size);
        foreach (FactorTerm term in terms)
        {
            sb.Put(NodesToProduct(term.Factors));
        }

        return Tensors.Tensors.Multiply(pb.Build(), sb.Build());
    }

    private static FactorTerm[] SumToSplitArray(Sum sum, FactorInt pivotPosition)
    {
        var terms = new FactorTerm[sum.Size];
        int pivotSumsCount = int.MaxValue;
        int pivotPosition1 = -1;
        for (int i = sum.Size - 1; i >= 0; --i)
        {
            terms[i] = TensorToTerm(sum[i]);
            if (terms[i].Factors.Length < pivotSumsCount)
            {
                pivotSumsCount = terms[i].Factors.Length;
                pivotPosition1 = i;
            }
        }

        pivotPosition.Value = pivotPosition1;
        return terms;
    }

    private static FactorTerm TensorToTerm(Tensor tensor)
    {
        if (tensor is Product)
        {
            var factors = new FactorTermNode[tensor.Size];
            int i = -1;
            foreach (Tensor t in tensor)
            {
                factors[++i] = CreateNode(t);
            }

            return new FactorTerm(factors);
        }

        return new FactorTerm([CreateNode(tensor)]);
    }

    private static FactorTermNode CreateNode(Tensor tensor)
    {
        if (tensor is Power && TensorUtils.IsInteger(tensor[1]))
        {
            return new FactorTermNode(
                tensor[0],
                ((Rational)((Complex)tensor[1]).Real).Numerator);
        }

        return new FactorTermNode(tensor, BigInteger.One);
    }

    private static Tensor NodesToProduct(FactorTermNode[] nodes)
    {
        var tensors = new Tensor[nodes.Length];
        for (int i = nodes.Length - 1; i >= 0; --i)
        {
            tensors[i] = nodes[i].ToTensor();
        }

        return Tensors.Tensors.Multiply(tensors);
    }

    public string ToString(OutputFormat outputFormat)
    {
        return "Factor";
    }

    public override string ToString()
    {
        return ToString(CC.GetDefaultOutputFormat());
    }
}

public sealed class FactorOptions
{
    public bool FactorScalars { get; set; } = true;
    public ITransformation FactorizationEngine { get; set; } = JasFactor.Engine;
}

internal sealed class FactorTerm
{
    public FactorTermNode[] Factors { get; }
    public Dictionary<int, List<FactorTermNode>> Map { get; }

    public FactorTerm(FactorTermNode[] factors)
    {
        Factors = factors;
        Map = new Dictionary<int, List<FactorTermNode>>(factors.Length);
        foreach (FactorTermNode t in factors)
        {
            int hash = t.Tensor.GetHashCode();
            if (!Map.TryGetValue(hash, out List<FactorTermNode>? list))
            {
                list = new List<FactorTermNode>();
                Map.Add(hash, list);
            }

            list.Add(t);
        }
    }

    public override string ToString()
    {
        var sb = new StringBuilder();
        for (int i = 0; ; ++i)
        {
            sb.Append('(').Append(Factors[i]).Append(')');
            if (i == Factors.Length - 1)
            {
                return sb.ToString();
            }

            sb.Append("*");
        }
    }
}

internal sealed class FactorTermNode
{
    public Tensor Tensor { get; }
    public BigInteger Exponent { get; set; }
    public FactorBigInt? MinExponent { get; set; }
    public bool DiffSigns { get; set; }

    public FactorTermNode(Tensor tensor, BigInteger exponent)
    {
        Tensor = tensor;
        Exponent = exponent;
    }

    public Tensor ToTensor()
    {
        BigInteger exponent = Exponent;
        if (MinExponent?.Value != null)
        {
            exponent -= MinExponent.Value.Value;
            if (DiffSigns && !MinExponent.Value.Value.IsEven)
            {
                return Tensors.Tensors.Negate(Tensors.Tensors.Pow(Tensor, new Complex(exponent)));
            }
        }

        return Tensors.Tensors.Pow(Tensor, new Complex(exponent));
    }

    public override bool Equals(object? obj)
    {
        return obj is FactorTermNode other && TensorUtils.Equals(other.Tensor, Tensor);
    }

    public override int GetHashCode()
    {
        return Tensor.GetHashCode();
    }

    public override string ToString()
    {
        return Tensor + " -> " + Exponent;
    }

    public FactorTermNode Clone()
    {
        return new FactorTermNode(Tensor, Exponent);
    }
}

internal sealed class FactorBigInt
{
    public BigInteger? Value { get; set; }
}

internal sealed class FactorInt
{
    public int Value { get; set; }
}
