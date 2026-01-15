using BigInteger = System.Numerics.BigInteger;
using NRedberry.Indices;
using NRedberry.Numbers;
using NRedberry.Parsers;

namespace NRedberry.Tensors;

public static class Tensors
{
    public static Tensor Pow(Tensor argument, int power)
    {
        return Pow(argument, new Complex(power));
    }

    public static Tensor Pow(Tensor argument, BigInteger power)
    {
        return Pow(argument, new Complex(power));
    }

    public static Tensor Pow(Tensor argument, Tensor power)
    {
        PowerBuilder builder = new();
        builder.Put(argument);
        builder.Put(power);
        return builder.Build();
    }

    public static Tensor Multiply(params Tensor[] factors)
    {
        return ProductFactory.Factory.Create(factors);
    }

    public static Tensor Multiply(IEnumerable<Tensor> factors)
    {
        ArgumentNullException.ThrowIfNull(factors);
        return Multiply(factors.ToArray());
    }

    public static Tensor MultiplyAndRenameConflictingDummies(params Tensor[] factors)
    {
        return ProductFactory.Factory.Create(ResolveDummy(factors));
    }

    public static Tensor MultiplyAndRenameConflictingDummies(IEnumerable<Tensor> factors)
    {
        ArgumentNullException.ThrowIfNull(factors);
        return MultiplyAndRenameConflictingDummies(factors.ToArray());
    }

    public static Tensor[] ResolveDummy(params Tensor[] factors)
    {
        ArgumentNullException.ThrowIfNull(factors);

        var result = new Tensor[factors.Length];
        HashSet<int> forbidden = [];
        List<Tensor> toResolve = [];
        for (int i = factors.Length - 1; i >= 0; --i)
        {
            var factor = factors[i];
            if (factor is MultiTensor || factor.Indices.GetFree().Size() == 0)
            {
                toResolve.Add(factor);
                forbidden.UnionWith(IndicesUtils.GetIndicesNames(factor.Indices.GetFree()));
            }
            else
            {
                forbidden.UnionWith(TensorUtils.GetAllIndicesNamesT(factor));
                result[i] = factor;
            }
        }

        int toResolvePosition = toResolve.Count;
        for (int i = factors.Length - 1; i >= 0; --i)
        {
            if (result[i] is null)
            {
                var factor = toResolve[--toResolvePosition];
                var newFactor = ApplyIndexMapping.RenameDummy(factor, forbidden.ToArray());
                forbidden.UnionWith(TensorUtils.GetAllIndicesNamesT(newFactor));
                result[i] = newFactor;
            }
        }

        return result;
    }

    public static void ResolveAllDummies(Tensor[] factors)
    {
        ArgumentNullException.ThrowIfNull(factors);

        HashSet<int> forbidden = [];
        for (int i = factors.Length - 1; i >= 0; --i)
        {
            forbidden.UnionWith(TensorUtils.GetAllIndicesNamesT(factors[i]));
        }

        for (int i = factors.Length - 1; i >= 0; --i)
        {
            factors[i] = ApplyIndexMapping.RenameDummy(factors[i], forbidden.ToArray());
            forbidden.UnionWith(TensorUtils.GetAllIndicesNamesT(factors[i]));
        }
    }

    public static Tensor Divide(Tensor a, Tensor b)
    {
        return Multiply(a, Reciprocal(b));
    }

    public static Tensor DivideAndRenameConflictingDummies(Tensor a, Tensor b)
    {
        return MultiplyAndRenameConflictingDummies(a, Reciprocal(b));
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
        ArgumentNullException.ThrowIfNull(tensors);
        return Sum(tensors.ToArray());
    }

    public static Tensor Subtract(Tensor a, Tensor b)
    {
        return Sum(a, Negate(b));
    }

    public static Tensor Negate(Tensor tensor)
    {
        ArgumentNullException.ThrowIfNull(tensor);
        if (tensor is Complex complex)
        {
            return complex.Negate();
        }

        return Multiply(Complex.MinusOne, tensor);
    }

    public static Tensor Reciprocal(Tensor tensor)
    {
        return Pow(tensor, Complex.MinusOne);
    }

    public static SimpleTensor SimpleTensor(string name, SimpleIndices indices)
    {
        return Tensor.SimpleTensor(name, indices);
    }

    public static SimpleTensor SimpleTensor(int name, SimpleIndices indices)
    {
        return Tensor.SimpleTensor(name, indices);
    }

    public static Expression Expression(Tensor left, Tensor right)
    {
        return (Expression)ExpressionFactory.Instance.Create(left, right);
    }

    public static Tensor Parse(string expression)
    {
        return CC.Current.ParseManager.Parse(expression);
    }

    public static Tensor Parse(string expression, params IParseTokenTransformer[] preprocessors)
    {
        return CC.Current.ParseManager.Parse(expression, preprocessors);
    }

    public static Tensor Parse(Tensor expression)
    {
        ArgumentNullException.ThrowIfNull(expression);
        return expression;
    }

    public static Tensor[] Parse(params string[] expressions)
    {
        ArgumentNullException.ThrowIfNull(expressions);
        Tensor[] result = new Tensor[expressions.Length];
        for (int i = 0; i < expressions.Length; ++i)
        {
            result[i] = Parse(expressions[i]);
        }

        return result;
    }

    public static SimpleTensor ParseSimple(string expression)
    {
        var t = Parse(expression);
        if (t is SimpleTensor st)
        {
            return st;
        }

        throw new ArgumentException("Input tensor is not SimpleTensor.");
    }

    public static Expression ParseExpression(string expression)
    {
        var t = Parse(expression);
        if (t is Expression expr)
        {
            return expr;
        }

        throw new ArgumentException("Input tensor is not Expression.");
    }
}
