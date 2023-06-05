using System.Linq;
using NRedberry.Core.Numbers;

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

    public static bool IsOne(Tensor tensor)
    {
        return tensor is Complex c && c.IsOne();
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
        if (tensor is Complex complex) {
            return complex.IsReal() && complex.GetReal().SigNum() > 0;
        }
        return false;
    }

    public static bool IsSymbol(Tensor tensor)
    {
        throw new System.NotImplementedException();
    }
}