using NRedberry.Tensors;
using NRedberry.Transformations.Symmetrization;

namespace NRedberry.Physics.Feyncalc;

/// <summary>
/// Skeleton port of cc.redberry.physics.feyncalc.DiracOptions.
/// </summary>
public class DiracOptions : ICloneable
{
    /// <summary>
    /// Gets or sets the gamma matrix tensor.
    /// </summary>
    public SimpleTensor GammaMatrix
    {
        get => throw new NotImplementedException();
        set => throw new NotImplementedException();
    }

    /// <summary>
    /// Gets or sets the gamma-five tensor.
    /// </summary>
    public SimpleTensor? Gamma5
    {
        get => throw new NotImplementedException();
        set => throw new NotImplementedException();
    }

    /// <summary>
    /// Gets or sets the Levi-Civita tensor.
    /// </summary>
    public SimpleTensor LeviCivita
    {
        get => throw new NotImplementedException();
        set => throw new NotImplementedException();
    }

    /// <summary>
    /// Gets or sets the spacetime dimension tensor.
    /// </summary>
    public Tensor Dimension
    {
        get => throw new NotImplementedException();
        set => throw new NotImplementedException();
    }

    /// <summary>
    /// Gets or sets the trace-of-one tensor.
    /// </summary>
    public Tensor TraceOfOne
    {
        get => throw new NotImplementedException();
        set => throw new NotImplementedException();
    }

    /// <summary>
    /// Gets or sets additional simplification transformations.
    /// </summary>
    public ITransformation Simplifications
    {
        get => throw new NotImplementedException();
        set => throw new NotImplementedException();
    }

    /// <summary>
    /// Gets or sets a value indicating whether Minkowski space is used.
    /// </summary>
    public bool MinkowskiSpace
    {
        get => throw new NotImplementedException();
        set => throw new NotImplementedException();
    }

    /// <summary>
    /// Gets or sets the Levi-Civita simplifying transformation.
    /// </summary>
    public ITransformation? SimplifyLeviCivita
    {
        get => throw new NotImplementedException();
        set => throw new NotImplementedException();
    }

    /// <summary>
    /// Gets or sets the expand-and-eliminate transformation.
    /// </summary>
    public ITransformation ExpandAndEliminate
    {
        get => throw new NotImplementedException();
        set => throw new NotImplementedException();
    }

    /// <summary>
    /// Gets or sets a value indicating whether caching is enabled.
    /// </summary>
    public bool Cache
    {
        get => throw new NotImplementedException();
        set => throw new NotImplementedException();
    }

    /// <summary>
    /// Gets or sets a value indicating whether the options have been created.
    /// </summary>
    public bool Created
    {
        get => throw new NotImplementedException();
        set => throw new NotImplementedException();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DiracOptions"/> class.
    /// </summary>
    public DiracOptions()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Creates a shallow copy of the options.
    /// </summary>
    /// <returns>The cloned instance.</returns>
    public object Clone()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Creates a copy with the specified expand transformation.
    /// </summary>
    /// <param name="expand">The expand transformation.</param>
    /// <returns>A cloned options instance.</returns>
    protected DiracOptions SetExpand(ITransformation? expand)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Triggers creation of derived option state.
    /// </summary>
    public void TriggerCreate()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Guesses the trace-of-one tensor for the provided dimension.
    /// </summary>
    /// <param name="dimension">Dimension tensor.</param>
    /// <returns>The guessed tensor.</returns>
    protected static Tensor GuessTraceOfOne(Tensor dimension)
    {
        throw new NotImplementedException();
    }
}
