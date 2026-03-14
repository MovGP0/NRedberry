using NRedberry.Core.Utils;
using NRedberry.Tensors;
using TensorFactory = NRedberry.Tensors.Tensors;

namespace NRedberry.Transformations.Powerexpand;

public static class PowerExpandUtils
{
    public static bool PowerUnfoldApplicable(Tensor tensor, IIndicator<Tensor> indicator)
    {
        ArgumentNullException.ThrowIfNull(tensor);
        ArgumentNullException.ThrowIfNull(indicator);

        return tensor is Power power
            && (power[0].Indices.Size() != 0
                || (power[0] is Product && !TensorUtils.IsInteger(power[1])))
            && PowerExpandApplicable1(power, indicator);
    }

    public static bool PowerExpandApplicable(Tensor tensor, IIndicator<Tensor> indicator)
    {
        ArgumentNullException.ThrowIfNull(tensor);
        ArgumentNullException.ThrowIfNull(indicator);

        return tensor is Power power
            && power[0] is Product
            && !TensorUtils.IsInteger(power[1])
            && PowerExpandApplicable1(power, indicator);
    }

    public static Tensor[] PowerExpandToArray(Power power)
    {
        ArgumentNullException.ThrowIfNull(power);
        return PowerExpandToArray(power, new TrueIndicator<Tensor>());
    }

    public static Tensor[] PowerExpandToArray(Power power, params SimpleTensor[] variables)
    {
        ArgumentNullException.ThrowIfNull(power);
        ArgumentNullException.ThrowIfNull(variables);

        return PowerExpandToArray(power, VarsToIndicator(variables));
    }

    public static Tensor[] PowerExpandToArray(Power power, IIndicator<Tensor> indicator)
    {
        ArgumentNullException.ThrowIfNull(power);
        ArgumentNullException.ThrowIfNull(indicator);

        if (power[0] is not Product)
        {
            throw new ArgumentException("Base should be product of tensors.", nameof(power));
        }

        return PowerExpandToArray1(power, indicator);
    }

    public static Tensor[] PowerExpandToArray(Tensor tensor, IIndicator<Tensor> indicator)
    {
        ArgumentNullException.ThrowIfNull(tensor);
        ArgumentNullException.ThrowIfNull(indicator);

        if (tensor is not Power power)
        {
            throw new ArgumentException("Tensor should be a power.", nameof(tensor));
        }

        return PowerExpandToArray(power, indicator);
    }

    public static Tensor[] PowerExpandToArray1(Tensor tensor, IIndicator<Tensor> indicator)
    {
        ArgumentNullException.ThrowIfNull(tensor);
        ArgumentNullException.ThrowIfNull(indicator);

        if (tensor is not Power power)
        {
            throw new ArgumentException("Tensor should be a power.", nameof(tensor));
        }

        Product product = power[0] as Product
            ?? throw new ArgumentException("Base should be product of tensors.", nameof(tensor));

        Tensor[] scalars = product.GetAllScalars();
        List<Tensor> factorOut = new(scalars.Length);
        List<Tensor> leave = new(scalars.Length);

        Tensor exponent = power[1];
        for (int i = 0; i < scalars.Length; ++i)
        {
            if (indicator.Is(scalars[i]))
            {
                factorOut.Add(TensorFactory.Pow(scalars[i], exponent));
            }
            else
            {
                leave.Add(scalars[i]);
            }
        }

        if (leave.Count != 0)
        {
            factorOut.Add(TensorFactory.Pow(TensorFactory.Multiply(leave), exponent));
        }

        return factorOut.ToArray();
    }

    public static IIndicator<Tensor> VarsToIndicator(SimpleTensor[] variables)
    {
        ArgumentNullException.ThrowIfNull(variables);

        int[] names = new int[variables.Length];
        for (int i = 0; i < variables.Length; ++i)
        {
            ArgumentNullException.ThrowIfNull(variables[i]);
            names[i] = variables[i].Name;
        }

        Array.Sort(names);
        return new VariablesIndicator(names);
    }

    public static Tensor[] PowerExpandIntoChainToArray(Power power, int[] forbiddenIndices, IIndicator<Tensor> indicator)
    {
        ArgumentNullException.ThrowIfNull(power);
        ArgumentNullException.ThrowIfNull(forbiddenIndices);
        ArgumentNullException.ThrowIfNull(indicator);

        if (power[0] is not Product)
        {
            throw new ArgumentException("Base should be product of tensors.", nameof(power));
        }

        return PowerExpandIntoChainToArray1(power, forbiddenIndices, indicator);
    }

    private static bool PowerExpandApplicable1(Tensor power, IIndicator<Tensor> indicator)
    {
        foreach (Tensor tensor in power[0])
        {
            if (indicator.Is(tensor))
            {
                return true;
            }
        }

        return indicator.Is(power);
    }

    private static Tensor[] PowerExpandIntoChainToArray1(Tensor power, int[] forbiddenIndices, IIndicator<Tensor> indicator)
    {
        if (!TensorUtils.IsPositiveNaturalNumber(power[1]))
        {
            return PowerExpandToArray1(power, indicator);
        }

        int exponent = ((NRedberry.Numbers.Complex)power[1]).IntValue();
        Tensor[] scalars = power[0] is Product product
            ? product.GetAllScalars()
            : [power[0]];

        List<Tensor> factorOut = new(scalars.Length);
        List<Tensor> leave = new(scalars.Length);
        HashSet<int> allForbidden = new(forbiddenIndices);

        for (int i = 0; i < scalars.Length; ++i)
        {
            if (!indicator.Is(scalars[i]))
            {
                leave.Add(scalars[i]);
                continue;
            }

            if (scalars[i] is SimpleTensor simpleTensor && simpleTensor.Indices.Size() == 0)
            {
                factorOut.Add(TensorFactory.Pow(simpleTensor, exponent));
                continue;
            }

            for (int j = 0; j < exponent; ++j)
            {
                Tensor temp = ApplyIndexMapping.RenameDummy(scalars[i], allForbidden.ToArray());
                allForbidden.UnionWith(TensorUtils.GetAllIndicesNamesT(temp));
                factorOut.Add(temp);
            }
        }

        if (leave.Count != 0)
        {
            factorOut.Add(TensorFactory.Pow(TensorFactory.Multiply(leave), exponent));
        }

        return factorOut.ToArray();
    }

    private sealed record class VariablesIndicator(int[] Names) : IIndicator<Tensor>
    {
        public bool Is(Tensor @object)
        {
            int toCheck;
            if (@object is SimpleTensor simpleTensor)
            {
                toCheck = simpleTensor.Name;
            }
            else if (@object is Power power)
            {
                if (power[0] is not SimpleTensor powerBase)
                {
                    return false;
                }

                toCheck = powerBase.Name;
            }
            else
            {
                return false;
            }

            return Array.BinarySearch(Names, toCheck) >= 0;
        }
    }
}
