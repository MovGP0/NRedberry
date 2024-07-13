﻿using NRedberry.Core.Numbers;

namespace NRedberry.Core.Tensors.Functions;

public class ArcCot : ScalarFunction
{
    public ArcCot(Tensor argument) : base(argument) { }

    public override Tensor Derivative()
    {
        return Tensors.Sum(Complex.One, Argument.Pow(Complex.Two)).Pow(Complex.MinusOne).Multiply(Complex.MinusOne);
    }

    protected override string FunctionName() => "ArcCot";

    public int GetHashCode() => 2311 * Argument.GetHashCode();

    public override TensorBuilder GetBuilder() => new ScalarFunctionBuilder(ArcCotFactory.Factory);

    public override TensorFactory GetFactory() => ArcCotFactory.Factory;
}