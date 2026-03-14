using NRedberry.Tensors;
using TensorFactory = NRedberry.Tensors.Tensors;

namespace NRedberry.Physics.Feyncalc;

/// <summary>
/// Port of cc.redberry.physics.feyncalc.SpinorsSimplifyOptions.
/// </summary>
public sealed class SpinorsSimplifyOptions : DiracOptions
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SpinorsSimplifyOptions"/> class.
    /// </summary>
    public SpinorsSimplifyOptions()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SpinorsSimplifyOptions"/> class from string representations.
    /// </summary>
    public SpinorsSimplifyOptions(string? u, string? v, string? uBar, string? vBar, string momentum, string mass)
    {
        ArgumentNullException.ThrowIfNull(momentum);
        ArgumentNullException.ThrowIfNull(mass);

        U = u is null ? null : TensorFactory.ParseSimple(u);
        V = v is null ? null : TensorFactory.ParseSimple(v);
        UBar = uBar is null ? null : TensorFactory.ParseSimple(uBar);
        VBar = vBar is null ? null : TensorFactory.ParseSimple(vBar);
        Momentum = TensorFactory.ParseSimple(momentum);
        Mass = TensorFactory.Parse(mass);
    }

    public SimpleTensor? U { get; set; }

    public SimpleTensor? V { get; set; }

    public SimpleTensor? UBar { get; set; }

    public SimpleTensor? VBar { get; set; }

    public SimpleTensor? Momentum { get; set; }

    public Tensor? Mass { get; set; }

    public bool DoDiracSimplify { get; set; }
}
