using NRedberry.Core.Tensors;

namespace NRedberry.Physics.Oneloopdiv;

/// <summary>
/// Skeleton port of cc.redberry.physics.oneloopdiv.OneLoopCounterterms.
/// </summary>
public sealed class OneLoopCounterterms
{
    private static readonly string Flat_ = string.Empty;
    private static readonly string WR_ = string.Empty;
    private static readonly string SR_ = string.Empty;
    private static readonly string SSR_ = string.Empty;
    private static readonly string FF_ = string.Empty;
    private static readonly string FR_ = string.Empty;
    private static readonly string RR_ = string.Empty;
    private static readonly string DELTA_1_ = string.Empty;
    private static readonly string DELTA_2_ = string.Empty;
    private static readonly string DELTA_3_ = string.Empty;
    private static readonly string DELTA_4_ = string.Empty;
    private static readonly string ACTION_ = string.Empty;

    private readonly Expression _flat = null!;
    private readonly Expression _wr = null!;
    private readonly Expression _sr = null!;
    private readonly Expression _ssr = null!;
    private readonly Expression _ff = null!;
    private readonly Expression _fr = null!;
    private readonly Expression _rr = null!;
    private readonly Expression _delta1 = null!;
    private readonly Expression _delta2 = null!;
    private readonly Expression _delta3 = null!;
    private readonly Expression _delta4 = null!;
    private readonly Expression _action = null!;

    private OneLoopCounterterms(
        Expression flat,
        Expression wr,
        Expression sr,
        Expression ssr,
        Expression ff,
        Expression fr,
        Expression rr,
        Expression delta1,
        Expression delta2,
        Expression delta3,
        Expression delta4,
        Expression action)
    {
        throw new NotImplementedException();
    }

    public Expression Flat => throw new NotImplementedException();

    public Expression Wr => throw new NotImplementedException();

    public Expression Sr => throw new NotImplementedException();

    public Expression Ssr => throw new NotImplementedException();

    public Expression Ff => throw new NotImplementedException();

    public Expression Fr => throw new NotImplementedException();

    public Expression Rr => throw new NotImplementedException();

    public Expression Counterterms => throw new NotImplementedException();

    public Expression Delta1 => throw new NotImplementedException();

    public Expression Delta2 => throw new NotImplementedException();

    public Expression Delta3 => throw new NotImplementedException();

    public Expression Delta4 => throw new NotImplementedException();

    public static OneLoopCounterterms CalculateOneLoopCounterterms(OneLoopInput input)
    {
        throw new NotImplementedException();
    }
}
