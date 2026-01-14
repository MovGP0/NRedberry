using NRedberry.Core.Combinatorics;

namespace NRedberry.Groups;

internal sealed class IntersectionPayload(
    List<BSGSCandidateElement> larger,
    int[] baseArray,
    Permutation[] intersectionWord)
    : BacktrackSearchPayload
{
    public override void BeforeLevelIncrement(int level)
    {
        int image = WordReference[level].NewIndexOf(baseArray[level]);
        if (level == 0)
        {
            intersectionWord[level] = larger[level].GetTransversalOf(image);
            return;
        }

        intersectionWord[level] = larger[level]
            .GetTransversalOf(intersectionWord[level - 1].NewIndexOfUnderInverse(image))
            .Composition(intersectionWord[level - 1]);
    }

    public override void AfterLevelIncrement(int level)
    {
    }

    public override bool Test(Permutation permutation, int level)
    {
        if (level == 0)
        {
            return larger[level].BelongsToOrbit(WordReference[level].NewIndexOf(baseArray[level]));
        }

        int image = WordReference[level].NewIndexOf(baseArray[level]);
        int prevImage = intersectionWord[level - 1].NewIndexOfUnderInverse(image);
        return larger[level].BelongsToOrbit(prevImage);
    }
}
