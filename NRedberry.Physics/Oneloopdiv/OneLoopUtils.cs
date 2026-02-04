using NRedberry;
using NRedberry.Tensors;
using TensorFactory = NRedberry.Tensors.Tensors;

namespace NRedberry.Physics.Oneloopdiv;

/// <summary>
/// Skeleton port of cc.redberry.physics.oneloopdiv.OneLoopUtils.
/// </summary>
public static class OneLoopUtils
{
    private static readonly Expression[] AntiDeSitterBackgroundDefinitions =
    [
        TensorFactory.ParseExpression("R_{lmab} = (1/3)*(g_{lb}*g_{ma}-g_{la}*g_{mb})*La"),
        TensorFactory.ParseExpression("R_{lm} = -g_{lm}*La")
    ];

    public static Expression[] AntiDeSitterBackground()
    {
        return (Expression[])AntiDeSitterBackgroundDefinitions.Clone();
    }

    public static void SetUpRiemannSymmetries()
    {
        AddSymmetry("R_lm", IndexType.LatinLower, false, 1, 0);
        AddSymmetry("R_lmab", IndexType.LatinLower, true, 0, 1, 3, 2);
        AddSymmetry("R_lmab", IndexType.LatinLower, false, 2, 3, 0, 1);
    }

    private static void AddSymmetry(string tensor, IndexType type, bool sign, params int[] permutation)
    {
        var simple = TensorFactory.ParseSimple(tensor);
        simple.SimpleIndices.Symmetries.Add(type, sign, permutation);
    }
}
