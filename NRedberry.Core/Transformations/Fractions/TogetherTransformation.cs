using NRedberry.Tensors;
using NRedberry.Transformations.Symmetrization;

namespace NRedberry.Transformations.Fractions;

/// <summary>
/// Skeleton port of cc.redberry.core.transformations.fractions.TogetherTransformation.
/// </summary>
public sealed class TogetherTransformation : ITransformation, TransformationToStringAble
{
    public static TogetherTransformation Default => throw new NotImplementedException();
    public static TogetherTransformation Factorized => throw new NotImplementedException();

    private readonly ITransformation factor;

    public TogetherTransformation(ITransformation factor)
    {
        throw new NotImplementedException();
    }

    public TogetherTransformation(TogetherOptions options)
    {
        throw new NotImplementedException();
    }

    public static Tensor Together(Tensor tensor)
    {
        throw new NotImplementedException();
    }

    public static Tensor Together(Tensor tensor, ITransformation factor)
    {
        throw new NotImplementedException();
    }

    public static Tensor Together(Tensor tensor, bool doFactor)
    {
        throw new NotImplementedException();
    }

    public Tensor Transform(Tensor tensor)
    {
        throw new NotImplementedException();
    }

    public string ToString(OutputFormat outputFormat)
    {
        throw new NotImplementedException();
    }

    public override string ToString()
    {
        throw new NotImplementedException();
    }

    private Tensor TogetherSum(Tensor tensor)
    {
        throw new NotImplementedException();
    }

    private SplitStruct SplitFraction(Tensor tensor)
    {
        throw new NotImplementedException();
    }

    private static bool CheckPower(Tensor tensor)
    {
        throw new NotImplementedException();
    }

    private sealed class SplitStruct
    {
        public IReadOnlyDictionary<Tensor, Tensor> Denominators { get; }
        public Tensor Numerator { get; }

        public SplitStruct(IReadOnlyDictionary<Tensor, Tensor> denominators, Tensor numerator)
        {
            throw new NotImplementedException();
        }
    }

    public sealed class TogetherOptions
    {
        public ITransformation? Factor { get; set; }

        public TogetherOptions()
        {
            throw new NotImplementedException();
        }
    }
}
