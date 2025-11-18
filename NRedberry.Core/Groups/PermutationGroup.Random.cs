using NRedberry.Core.Combinatorics;

namespace NRedberry.Core.Groups;

public sealed partial class PermutationGroup
{
    public List<Permutation> RandomSource()
    {
        if (_randomSource == null)
        {
            var randomSource = new List<Permutation>(_generators);
            RandomPermutation.Randomness(
                randomSource,
                RandomPermutation.DefaultRandomnessExtendToSize,
                RandomPermutation.DefaultNumberOfRandomRefinements,
                Random.Shared);
            _randomSource = randomSource;
        }

        return _randomSource;
    }

    public Permutation RandomElement()
    {
        return RandomElement(Random.Shared);
    }

    public Permutation RandomElement(Random generator)
    {
        List<BSGSElement> bsgs = GetBSGS();
        BSGSElement element = bsgs[0];
        Permutation result = element.GetTransversalOf(element.GetOrbitPoint(generator.Next(element.OrbitSize)));
        for (int i = 1; i < bsgs.Count; ++i)
        {
            element = bsgs[i];
            result = result.Composition(element.GetTransversalOf(element.GetOrbitPoint(generator.Next(element.OrbitSize))));
        }

        return result;
    }
}
