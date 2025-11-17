using NRedberry.Contexts;

namespace NRedberry.Core.Tensors;

public static class Tensors
{
    public static SimpleTensor ParseSimple(string expression)
    {
        var t = Parse(expression);
        if (t is SimpleTensor st)
        {
            return st;
        }

        throw new ArgumentException("Input tensor is not SimpleTensor.");
    }

    public static Tensor Parse(string expression)
    {
        return CC.Current.ParseManager.Parse(expression);
    }

    /// <summary>
    /// Returns the result of summation of several tensors.
    /// </summary>
    /// <param name="tensors">Array of summands</param>
    /// <returns>Result of summation</returns>
    /// <exception cref="TensorException">Thrown if tensors have different free indices</exception>
    public static Tensor Sum(params Tensor[] tensors)
    {
        return SumFactory.Factory.Create(tensors);
    }

    /// <summary>
    /// Returns the result of summation of several tensors.
    /// </summary>
    /// <param name="tensors">Collection of summands</param>
    /// <returns>Result of summation</returns>
    /// <exception cref="TensorException">Thrown if tensors have different free indices</exception>
    public static Tensor Sum(IEnumerable<Tensor> tensors)
    {
        return Sum(tensors.ToArray());
    }

    public static Tensor Negate(Tensor tensor)
    {
        throw new NotImplementedException();
    }
}
