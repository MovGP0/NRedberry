using NRedberry.Core.Combinatorics;
using NRedberry.Core.Utils;

namespace NRedberry.Groups;

internal sealed class IntersectionIndicator(List<BSGSCandidateElement> larger) : IIndicator<Permutation>
{
    public bool Is(Permutation @object)
    {
        return AlgorithmsBase.MembershipTest(larger, @object);
    }
}
