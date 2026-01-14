using NRedberry.Core.Combinatorics;

namespace NRedberry.Groups;

internal sealed class DefaultBacktrackSearchPayload(IBacktrackSearchTestFunction test) : BacktrackSearchPayload
{
    private readonly IBacktrackSearchTestFunction test = test ?? throw new ArgumentNullException(nameof(test));

    public override void BeforeLevelIncrement(int level)
    {
    }

    public override void AfterLevelIncrement(int level)
    {
    }

    public override bool Test(Permutation permutation, int level)
    {
        return test.Test(permutation, level);
    }
}
