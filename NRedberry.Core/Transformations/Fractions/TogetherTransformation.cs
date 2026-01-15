using NRedberry.Numbers;
using NRedberry.Tensors;
using NRedberry.Transformations.Factor;
using NRedberry.Transformations.Substitutions;
using NRedberry.Transformations.Symmetrization;

namespace NRedberry.Transformations.Fractions;

/// <summary>
/// Skeleton port of cc.redberry.core.transformations.fractions.TogetherTransformation.
/// </summary>
public sealed class TogetherTransformation : ITransformation, TransformationToStringAble
{
    public static TogetherTransformation Default { get; } = new(new IdentityTransformation());
    public static TogetherTransformation Factorized { get; } = new(FactorTransformation.Instance);

    private readonly ITransformation factor;

    public TogetherTransformation(ITransformation factor)
    {
        ArgumentNullException.ThrowIfNull(factor);
        this.factor = factor;
    }

    public TogetherTransformation(TogetherOptions options)
        : this(options?.Factor ?? new IdentityTransformation())
    {
    }

    public static Tensor Together(Tensor tensor)
    {
        return Default.Transform(tensor);
    }

    public static Tensor Together(Tensor tensor, ITransformation factor)
    {
        return new TogetherTransformation(factor).Transform(tensor);
    }

    public static Tensor Together(Tensor tensor, bool doFactor)
    {
        return doFactor ? Factorized.Transform(tensor) : Together(tensor);
    }

    public Tensor Transform(Tensor tensor)
    {
        ArgumentNullException.ThrowIfNull(tensor);

        var iterator = new SubstitutionIterator(tensor);
        Tensor? current;
        while ((current = iterator.Next()) is not null)
        {
            if (current is Sum)
            {
                iterator.SafeSet(TogetherSum(current));
            }

            if (current is Product product)
            {
                iterator.SafeSet(CollectScalarFactorsITransformation.CollectScalarFactorsInProduct(product));
            }
        }

        return factor.Transform(iterator.Result());
    }

    public string ToString(OutputFormat outputFormat)
    {
        return "Together";
    }

    public override string ToString()
    {
        return ToString(CC.GetDefaultOutputFormat());
    }

    private Tensor TogetherSum(Tensor t)
    {
        bool performTogether = false;
        foreach (Tensor s in t)
        {
            if (s is Product)
            {
                foreach (Tensor p in s)
                {
                    if (CheckPower(p))
                    {
                        performTogether = true;
                        break;
                    }
                }
            }
            else if (CheckPower(s))
            {
                performTogether = true;
                break;
            }
        }

        if (!performTogether)
        {
            return t;
        }

        TogetherSplitStruct baseStruct = SplitFraction(t[0]);
        var numeratorTerms = new List<Tensor>[t.Size];
        numeratorTerms[0] = new List<Tensor> { baseStruct.Numerator };
        Tensor sTerm;
        Tensor power;
        Complex exponent;
        Complex? exponentDelta;
        for (int i = 1; i < t.Size; ++i)
        {
            sTerm = t[i];
            TogetherSplitStruct temp = SplitFraction(sTerm);

            var newNumeratorTerm = new List<Tensor> { temp.Numerator };
            numeratorTerms[i] = newNumeratorTerm;

            foreach (KeyValuePair<Tensor, Complex> baseEntry in baseStruct.Denominators)
            {
                if (!temp.Denominators.TryGetValue(baseEntry.Key, out exponent))
                {
                    power = Tensors.Tensors.Pow(baseEntry.Key, baseEntry.Value);
                    newNumeratorTerm.Add(power);
                }
                else if ((exponentDelta = baseEntry.Value.Subtract(exponent)).Real.SigNum() > 0)
                {
                    power = Tensors.Tensors.Pow(baseEntry.Key, exponentDelta);
                    newNumeratorTerm.Add(power);
                }
            }

            foreach (KeyValuePair<Tensor, Complex> tempEntry in temp.Denominators)
            {
                if (!baseStruct.Denominators.TryGetValue(tempEntry.Key, out exponent))
                {
                    power = Tensors.Tensors.Pow(tempEntry.Key, tempEntry.Value);
                    for (int j = 0; j < i; ++j)
                    {
                        numeratorTerms[j].Add(power);
                    }

                    baseStruct.Denominators[tempEntry.Key] = tempEntry.Value;
                }
                else if ((exponentDelta = tempEntry.Value.Subtract(exponent)).Real.SigNum() > 0)
                {
                    power = Tensors.Tensors.Pow(tempEntry.Key, exponentDelta);
                    for (int j = 0; j < i; ++j)
                    {
                        numeratorTerms[j].Add(power);
                    }

                    baseStruct.Denominators[tempEntry.Key] = tempEntry.Value;
                }
            }
        }

        var numeratorSumBuilder = new SumBuilder();
        foreach (List<Tensor> term in numeratorTerms)
        {
            numeratorSumBuilder.Put(
                CollectScalarFactorsITransformation.CollectScalarFactors(
                    Tensors.Tensors.MultiplyAndRenameConflictingDummies(term.ToArray())));
        }

        Tensor[] resultProduct = new Tensor[1 + baseStruct.Denominators.Count];
        resultProduct[0] = numeratorSumBuilder.Build();
        int index = 0;
        foreach (KeyValuePair<Tensor, Complex> baseEntry in baseStruct.Denominators)
        {
            resultProduct[++index] = Tensors.Tensors.Pow(baseEntry.Key, baseEntry.Value.Negate());
        }

        return Tensors.Tensors.MultiplyAndRenameConflictingDummies(resultProduct);
    }

    private TogetherSplitStruct SplitFraction(Tensor tensor)
    {
        tensor = factor.Transform(tensor);

        var map = new Dictionary<Tensor, Complex>();
        if (CheckPower(tensor))
        {
            map[tensor[0]] = ((Complex)tensor[1]).Negate();
            return new TogetherSplitStruct(map, Complex.One);
        }

        if (tensor is Product)
        {
            Tensor product = tensor;
            Tensor? temp = null;
            for (int i = tensor.Size - 1; i >= 0; --i)
            {
                Tensor m = tensor[i];
                if (CheckPower(m))
                {
                    map[m[0]] = ((Complex)m[1]).Negate();
                    if (product is Product productValue)
                    {
                        product = productValue.Remove(i);
                        temp = product;
                    }
                    else
                    {
                        temp = Complex.One;
                    }
                }
            }

            temp ??= tensor;
            return new TogetherSplitStruct(map, temp);
        }

        return new TogetherSplitStruct(map, tensor);
    }

    private static bool CheckPower(Tensor power)
    {
        return power is Power && TensorUtils.IsRealNegativeNumber(power[1]);
    }

    public sealed class TogetherOptions
    {
        public ITransformation? Factor { get; set; }
    }
}

internal sealed class TogetherSplitStruct
{
    public Dictionary<Tensor, Complex> Denominators { get; }
    public Tensor Numerator { get; }

    public TogetherSplitStruct(Dictionary<Tensor, Complex> denominators, Tensor numerator)
    {
        Denominators = denominators;
        Numerator = numerator;
    }
}
