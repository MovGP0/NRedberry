using NRedberry.Tensors;
using NRedberry.Transformations;
using NRedberry.Transformations.Symmetrization;
using TensorFactory = NRedberry.Tensors.Tensors;

namespace NRedberry.Physics.Feyncalc;

/// <summary>
/// Skeleton port of cc.redberry.physics.feyncalc.LeviCivitaSimplifyOptions.
/// </summary>
public sealed class LeviCivitaSimplifyOptions()
{
    private static readonly SimpleTensor DefaultLeviCivita = TensorFactory.ParseSimple("e_abcd");
    private static readonly Tensor DefaultDimension = TensorFactory.Parse("N");

    public LeviCivitaSimplifyOptions(bool minkowskiSpace)
        : this()
    {
        MinkowskiSpace = minkowskiSpace;
    }

    public SimpleTensor LeviCivita { get; set; } = DefaultLeviCivita;

    public bool MinkowskiSpace { get; set; } = true;

    public ITransformation Simplifications { get; set; } = Transformation.Identity;

    public ITransformation OverallSimplifications { get; set; } = Transformation.Identity;

    public Tensor Dimension { get; set; } = DefaultDimension;
}
