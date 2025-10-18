using NRedberry.Core.Combinatorics;

namespace NRedberry.Core.Groups;

/// <summary>
/// Algorithms for constructing, modifying, and manipulating base and strong generating sets (BSGS) of permutation groups.
/// </summary>
public static class AlgorithmsBase
{
    /// <summary>
    /// Represents the result of the Strip method.
    /// </summary>
    public class StripContainer
    {
        public int TerminationLevel { get; }
        public Permutation Remainder { get; }

        public StripContainer(int terminationLevel, Permutation remainder)
        {
            TerminationLevel = terminationLevel;
            Remainder = remainder;
        }
    }

    /// <summary>
    /// Determines whether a permutation belongs to the group defined by the given BSGS.
    /// </summary>
    /// <param name="bsgs">The base and strong generating set.</param>
    /// <param name="permutation">The permutation to test.</param>
    /// <returns>True if the permutation belongs to the group; otherwise, false.</returns>
    public static bool MembershipTest(List<BSGSElement> bsgs, Permutation permutation)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Creates a raw BSGS candidate represented as a list.
    /// </summary>
    /// <param name="generators">The group generators.</param>
    /// <returns>A list representing the raw BSGS candidate.</returns>
    public static List<BSGSCandidateElement> CreateRawBSGSCandidate(params Permutation[] generators)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Applies the Schreier-Sims algorithm to complete the specified BSGS candidate.
    /// </summary>
    /// <param name="bsgsCandidate">The BSGS candidate to process.</param>
    public static void SchreierSimsAlgorithm(List<BSGSCandidateElement> bsgsCandidate)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Changes the base of the specified BSGS to a new base using transpositions.
    /// </summary>
    /// <param name="bsgs">The BSGS to modify.</param>
    /// <param name="newBase">The new base.</param>
    public static void RebaseWithTranspositions(List<BSGSCandidateElement> bsgs, int[] newBase)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Creates a BSGS for a symmetric group of the specified degree.
    /// </summary>
    /// <param name="degree">The degree of the group.</param>
    /// <returns>A BSGS representing the symmetric group.</returns>
    public static List<BSGSElement> CreateSymmetricGroupBSGS(int degree)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Calculates the order of a permutation group represented by the specified BSGS.
    /// </summary>
    /// <param name="bsgs">The base and strong generating set.</param>
    /// <returns>The order of the permutation group.</returns>
    public static long CalculateOrder(List<BSGSElement> bsgs)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Removes redundant generators from the specified BSGS candidate.
    /// </summary>
    /// <param name="bsgsCandidate">The BSGS candidate to process.</param>
    public static void RemoveRedundantGenerators(List<BSGSCandidateElement> bsgsCandidate)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Returns the base of the specified BSGS as an array.
    /// </summary>
    /// <param name="bsgs">The BSGS to extract the base from.</param>
    /// <returns>An array representing the base.</returns>
    public static int[] GetBaseAsArray(List<BSGSElement> bsgs)
    {
        throw new NotImplementedException();
    }
}