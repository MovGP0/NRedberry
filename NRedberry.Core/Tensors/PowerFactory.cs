using System;
using System.Linq;
using NRedberry.Core.Numbers;

namespace NRedberry.Core.Tensors;

public sealed class PowerFactory : ITensorFactory
{
    public static PowerFactory Factory = new();

    private PowerFactory()
    {
    }

    public Tensor Create(params Tensor[] tensors)
    {
        CheckWithException(tensors);
        return Power(tensors[0], tensors[1]);
    }

    public static Tensor Power(Tensor argument, Tensor power)
    {
        //TODO improve Complex^Complex
        if (argument is Complex a && power is Complex p)
        {
            var result = Exponentiation.ExponentiateIfPossible(a, p);
            if (result != null) return result;
        }

        if (TensorUtils.IsOne(power)) return argument;
        if (TensorUtils.IsZero(power) || TensorUtils.IsOne(argument)) return Complex.One;
        if (TensorUtils.IsZero(argument)) return Complex.Zero;

        if (argument is Product)
        {
            if (TensorUtils.IsInteger(power)
                //case (2*x)**(y)           //todo replace with isPositiveNumerical(argument.get(0))
                || (argument.Size == 2 && TensorUtils.IsRealPositiveNumber(argument[0])))
            {
                Tensor[] scalars = ((Product)argument).GetAllScalars();
                if (scalars.Length > 1)
                {
                    ITensorBuilder pb = argument.GetBuilder();//creating product builder
                    foreach (Tensor t in scalars)
                        pb.Put(t.Pow(power));//TODO refactor for performance
                    return pb.Build();
                }
            }
        }

        if (argument is Power)
        {
            return argument[0].Pow(argument[1].Multiply(power));
        }

        return new Power(argument, power);
    }

    private static void CheckWithException(Tensor[] tensors)
    {
        if (tensors.Length != 2) throw new ArgumentException("Wrong number of arguments.");
        if (!TensorUtils.IsScalar(tensors)) throw new ArgumentException("Non scalar power parametres.");

        if (tensors.Any(t => t == null))
        {
            throw new NullReferenceException();
        }
    }
}