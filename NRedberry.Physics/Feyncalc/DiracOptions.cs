using NRedberry.Numbers;
using NRedberry.Tensors;
using NRedberry.Transformations;
using NRedberry.Transformations.Symmetrization;
using TensorFactory = NRedberry.Tensors.Tensors;

namespace NRedberry.Physics.Feyncalc;

/// <summary>
/// Port of cc.redberry.physics.feyncalc.DiracOptions.
/// </summary>
public class DiracOptions : ICloneable
{
    private static readonly SimpleTensor DefaultGammaMatrix = TensorFactory.ParseSimple("G_a");
    private static readonly SimpleTensor DefaultGamma5 = TensorFactory.ParseSimple("G5");
    private static readonly SimpleTensor DefaultLeviCivita = TensorFactory.ParseSimple("e_abcd");

    private Tensor? _traceOfOne;
    private ITransformation? _expandAndEliminate;

    /// <summary>
    /// Initializes a new instance of the <see cref="DiracOptions"/> class.
    /// </summary>
    public DiracOptions()
    {
        GammaMatrix = DefaultGammaMatrix;
        Gamma5 = DefaultGamma5;
        LeviCivita = DefaultLeviCivita;
        Dimension = Complex.Four;
        Simplifications = Transformation.Identity;
        MinkowskiSpace = true;
        Cache = true;
    }

    /// <summary>
    /// Gets or sets the gamma matrix tensor.
    /// </summary>
    public SimpleTensor GammaMatrix { get; set; }

    /// <summary>
    /// Gets or sets the gamma-five tensor.
    /// </summary>
    public SimpleTensor? Gamma5 { get; set; }

    /// <summary>
    /// Gets or sets the Levi-Civita tensor.
    /// </summary>
    public SimpleTensor LeviCivita { get; set; }

    /// <summary>
    /// Gets or sets the spacetime dimension tensor.
    /// </summary>
    public Tensor Dimension { get; set; }

    /// <summary>
    /// Gets or sets the trace-of-one tensor.
    /// </summary>
    public Tensor TraceOfOne
    {
        get => _traceOfOne ?? GuessTraceOfOne(Dimension);
        set
        {
            ArgumentNullException.ThrowIfNull(value);
            _traceOfOne = value;
        }
    }

    /// <summary>
    /// Gets or sets additional simplification transformations.
    /// </summary>
    public ITransformation Simplifications { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether Minkowski space is used.
    /// </summary>
    public bool MinkowskiSpace { get; set; }

    /// <summary>
    /// Gets or sets the Levi-Civita simplifying transformation.
    /// </summary>
    public ITransformation? SimplifyLeviCivita { get; set; }

    /// <summary>
    /// Gets or sets the expand-and-eliminate transformation.
    /// </summary>
    public ITransformation ExpandAndEliminate
    {
        get => _expandAndEliminate ?? new ExpandAndEliminateTransformation(Simplifications);
        set
        {
            ArgumentNullException.ThrowIfNull(value);
            _expandAndEliminate = value;
        }
    }

    /// <summary>
    /// Gets or sets a value indicating whether caching is enabled.
    /// </summary>
    public bool Cache { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the options have been created.
    /// </summary>
    public bool Created { get; set; }

    /// <summary>
    /// Creates a shallow copy of the options.
    /// </summary>
    /// <returns>The cloned instance.</returns>
    public virtual DiracOptions Clone()
    {
        return (DiracOptions)MemberwiseClone();
    }

    object ICloneable.Clone()
    {
        return Clone();
    }

    /// <summary>
    /// Creates a copy with the specified expand transformation.
    /// </summary>
    /// <param name="expand">The expand transformation.</param>
    /// <returns>A cloned options instance.</returns>
    protected DiracOptions SetExpand(ITransformation? expand)
    {
        DiracOptions options = Clone();
        options._expandAndEliminate = expand;
        return options;
    }

    /// <summary>
    /// Triggers creation of derived option state.
    /// </summary>
    public void TriggerCreate()
    {
        Created = true;
        _traceOfOne ??= GuessTraceOfOne(Dimension);
        _expandAndEliminate ??= new ExpandAndEliminateTransformation(Simplifications);
        SimplifyLeviCivita ??= Transformation.Identity;
    }

    /// <summary>
    /// Guesses the trace-of-one tensor for the provided dimension.
    /// </summary>
    /// <param name="dimension">Dimension tensor.</param>
    /// <returns>The guessed tensor.</returns>
    protected static Tensor GuessTraceOfOne(Tensor dimension)
    {
        ArgumentNullException.ThrowIfNull(dimension);

        if (TensorUtils.IsIntegerOdd(dimension))
        {
            Tensor exponent = TensorFactory.Divide(
                TensorFactory.Subtract(dimension, Complex.One),
                Complex.Two);
            return TensorFactory.Pow(Complex.Two, exponent);
        }

        return TensorFactory.Pow(Complex.Two, TensorFactory.Divide(dimension, Complex.Two));
    }
}
