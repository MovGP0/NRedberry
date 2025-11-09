using NRedberry.Core.Parsers;
using NRedberry.Core.Tensors;
using NRedberry.Core.Utils;

namespace NRedberry.Physics.Feyncalc;

/// <summary>
/// Skeleton port of cc.redberry.physics.feyncalc.DiracSimplify1.
/// </summary>
public sealed class DiracSimplify1 : AbstractFeynCalcTransformation
{
    private readonly Expression[] _substitutionsCache;
    private readonly Dictionary<int, Expression> _generatedSubstitutions = new();

    private static readonly Parser Parser;
    private static readonly ParseToken S1;
    private static readonly ParseToken S2;
    private static readonly ParseToken S3;
    private static readonly ParseToken S4;

    static DiracSimplify1()
    {
        Parser = null!;
        S1 = null!;
        S2 = null!;
        S3 = null!;
        S4 = null!;
    }

    public DiracSimplify1(DiracOptions options)
        : base(options, null)
    {
        throw new NotImplementedException();
    }

    public string ToString(OutputFormat outputFormat)
    {
        throw new NotImplementedException();
    }

    protected override Tensor? TransformLine(ProductOfGammas productOfGammas, List<int> modifiedElements)
    {
        throw new NotImplementedException();
    }
}
