﻿using NRedberry.Core.Numbers;

namespace NRedberry.Core.Tensors.Functions;

public class ArcCos : ScalarFunction
{
    public ArcCos(Tensor argument) : base(argument) { }

    public override Tensor Derivative()
    {
        return Tensors.Sum(Complex.One, Argument.Pow(Complex.Two)).Pow(Complex.MinusOneHalf).Multiply(Complex.MinusOne);
    }

    protected override string FunctionName() => "ArcCos";

    public override int GetHashCode() => 92841 * Argument.GetHashCode();

    public override TensorBuilder GetBuilder() => new ScalarFunctionBuilder(ArcCosFactory.Factory);

    public override TensorFactory GetFactory() => ArcCosFactory.Factory;
}