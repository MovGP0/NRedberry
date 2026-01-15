using NRedberry.Indices;

namespace NRedberry.Tensors.Functions;

public abstract class ScalarFunction : Tensor
{
    protected readonly Tensor Argument;

    protected ScalarFunction(Tensor argument)
    {
        if (!TensorUtils.IsScalar(argument))
            throw new TensorException("Non scalar argument " + argument + " in scalar function");
        Argument = argument;
    }

    public override Indices.Indices Indices => IndicesFactory.EmptyIndices;

    protected abstract string FunctionName();

    public abstract Tensor Derivative();

    public override Tensor this[int i]
    {
        get
        {
            if (i != 0)
                throw new IndexOutOfRangeException();
            return Argument;
        }
    }

    public override int Size => 1;

    public override string ToString(OutputFormat mode)
    {
        string stringSymbol = FunctionName();
        if (mode.Is(OutputFormat.UTF8))
        {
            return $"{stringSymbol}({Argument.ToString(OutputFormat.UTF8)})";
        }

        if (mode.Is(OutputFormat.LaTeX))
        {
            return $"\\{stringSymbol.ToLower()}({Argument.ToString(OutputFormat.UTF8)})";
        }

        if (mode.Is(OutputFormat.Redberry) || mode.Is(OutputFormat.SimpleRedberry) || mode.Is(OutputFormat.WolframMathematica))
        {
            return $"{char.ToUpper(stringSymbol[0])}{stringSymbol.Substring(1)}[{Argument.ToString(mode)}]";
        }

        return $"{stringSymbol}({Argument.ToString(mode)})";
    }
}
