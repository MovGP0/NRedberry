using NRedberry.Core.Indices;
using NRedberry.Core.Numbers;
using NRedberry.Core.Tensors.Functions;

namespace NRedberry.Core.Tensors;

public static class TensorUtils
{
    public static bool IsScalar(params Tensor[] tensors)
    {
        return tensors.All(IsScalar);
    }

    private static bool IsScalar(Tensor tensor)
    {
        return tensor.Indices.GetFree().Size() == 0;
    }

    public static bool IsZero(Tensor tensor)
    {
        return tensor is Complex c && c.IsZero();
    }

    public static bool IsInteger(Tensor tensor)
    {
        return tensor is Complex c && c.IsInteger();
    }

    public static bool IsRealPositiveNumber(Tensor tensor)
    {
        if (tensor is Complex complex)
        {
            return complex.IsReal() && complex.GetReal().SigNum() > 0;
        }

        return false;
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

    public static bool IsNaturalNumber(Tensor tensor)
    {
        return tensor is Complex complex && complex.IsNatural();
    }

    public static bool IsNumeric(Tensor tensor)
    {
        return tensor is Complex complex && complex.IsNumeric();
    }

    public static bool IsNegativeIntegerNumber(Tensor tensor)
    {
        return tensor is Complex complex && complex.IsNegativeInteger();
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
        return tensors.All(t => t.Indices.Size() == 0);
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
        // ... other checks and loops here...
        throw new NotImplementedException("not implemented");
    }

    public static bool IsSymbolic(params Tensor[] tensors)
    {
        return tensors.All(t => IsSymbolic(t));
    }

    public static bool IsOne(Tensor tensor)
    {
        return tensor is Complex c && c.IsOne();
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
        return t is Power && IsNaturalNumber(t[1]);
    }

    public static bool IsNegativeIntegerPower(Tensor t)
    {
        return t is Power && IsNegativeIntegerNumber(t[1]);
    }

    public static HashSet<int> GetAllDummyIndicesT(Tensor tensor)
    {
        throw new NotImplementedException();
    }

    public static HashSet<int> GetAllIndicesNamesT(params Tensor[] tensors)
    {
        HashSet<int> set = [];
        foreach (var tensor in tensors)
        {
            AppendAllIndicesNamesT(tensor, set);
        }

        return set;
    }

    public static void AppendAllIndicesNamesT(Tensor tensor, HashSet<int> set)
    {
        if (tensor is SimpleTensor)
        {
            var ind = tensor.Indices;
            set.EnsureCapacity(ind.Size());
            int size = ind.Size();
            for (int i = 0; i < size; ++i)
            {
                set.Add(IndicesUtils.GetNameWithType(ind[i]));
            }
        }
        else if (tensor is Power)
        {
            AppendAllIndicesNamesT(tensor[0], set);
        }
        else if (tensor is ScalarFunction)
        {
            // return
        }
        else
        {
            for (int i = tensor.Size - 1; i >= 0; --i)
            {
                var t = tensor[i];
                AppendAllIndicesNamesT(t, set);
            }
        }
    }
}
