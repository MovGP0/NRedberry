using NRedberry.Core.Utils;
using NRedberry.Tensors;
using NRedberry.Transformations.Symmetrization;

namespace NRedberry.Physics.Feyncalc;

/// <summary>
/// Skeleton port of cc.redberry.physics.feyncalc.SpinorsSimplifyTransformation.
/// </summary>
public sealed class SpinorsSimplifyTransformation : AbstractTransformationWithGammas
{
    private readonly SimpleTensor? u;
    private readonly SimpleTensor? v;
    private readonly SimpleTensor? uBar;
    private readonly SimpleTensor? vBar;
    private readonly SimpleTensor momentum = null!;
    private readonly SimpleTensor mass = null!;
    private readonly ITransformation uSubs = null!;
    private readonly ITransformation vSubs = null!;
    private readonly ITransformation uBarSubs = null!;
    private readonly ITransformation vBarSubs = null!;
    private readonly ITransformation p2 = null!;
    private readonly ITransformation simplifyG5 = null!;
    private readonly ITransformation ortogonality = null!;
    private readonly ITransformation diracSimplify = null!;

    public SpinorsSimplifyTransformation(SpinorsSimplifyOptions options)
        : base(options)
    {
        throw new NotImplementedException();
    }

    public string ToString(OutputFormat outputFormat)
    {
        throw new NotImplementedException();
    }

    private void CheckSpinorNotation(SimpleTensor? spinor, bool bar)
    {
        throw new NotImplementedException();
    }

    private ITransformation CreateSubs(SimpleTensor? spinor, bool negate)
    {
        throw new NotImplementedException();
    }

    private ITransformation CreateBarSubs(SimpleTensor? spinor, bool negate)
    {
        throw new NotImplementedException();
    }

    private ITransformation CreateP2Subs()
    {
        throw new NotImplementedException();
    }

    private Expression[]? CreateOrtIdentities(SimpleTensor? left, SimpleTensor? right)
    {
        throw new NotImplementedException();
    }

    private Tensor Move(Tensor product, int index, bool left)
    {
        throw new NotImplementedException();
    }

    private Tensor Move0(Tensor product, int index, bool left)
    {
        throw new NotImplementedException();
    }

    private Tensor ToLeft0(Tensor[] gammas, int index)
    {
        throw new NotImplementedException();
    }

    private Tensor ToRight0(Tensor[] gammas, int index)
    {
        throw new NotImplementedException();
    }

    private Tensor[] CreateLine(int length, List<int> gamma5Positions)
    {
        throw new NotImplementedException();
    }

    private int WithMomentum(int gamma, ProductContent productContent, StructureOfContractions structure)
    {
        throw new NotImplementedException();
    }

    private SpinorType? IsSpinor(Tensor tensor)
    {
        throw new NotImplementedException();
    }

    private enum SpinorType
    {
        U,
        V,
        UBar,
        VBar
    }
}
