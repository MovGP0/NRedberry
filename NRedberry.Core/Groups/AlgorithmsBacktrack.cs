using System;
using System.Collections.Generic;
using NRedberry.Core.Combinatorics;
using NRedberry.Core.Utils;

namespace NRedberry.Core.Groups;

/// <summary>
/// Algorithms using backtrack search in permutation groups, including subgroup search, stabilizers, coset representatives, intersections, and more.
/// </summary>
public static class AlgorithmsBacktrack
{
    /// <summary>
    /// Searches for a subgroup within a specified group.
    /// </summary>
    /// <param name="group">The base and strong generating set of the group.</param>
    /// <param name="subgroup">The initial subgroup to extend.</param>
    /// <param name="testFunction">The test function to apply at each level of the search tree.</param>
    /// <param name="property">The property of subgroup elements to satisfy.</param>
    public static void SubgroupSearch(
        List<BSGSElement> group,
        List<BSGSCandidateElement> subgroup,
        IBacktrackSearchTestFunction testFunction,
        IIndicator<Permutation> property)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Searches for a subgroup within a specified group with payload functionality.
    /// </summary>
    /// <param name="group">The base and strong generating set of the group.</param>
    /// <param name="subgroup">The initial subgroup to extend.</param>
    /// <param name="payload">The payload test function and event listener.</param>
    /// <param name="property">The property of subgroup elements to satisfy.</param>
    public static void SubgroupSearchWithPayload(
        List<BSGSElement> group,
        List<BSGSCandidateElement> subgroup,
        BacktrackSearchPayload payload,
        IIndicator<Permutation> property)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Finds left coset representatives of a subgroup within a group.
    /// </summary>
    /// <param name="group">The group.</param>
    /// <param name="subgroup">The subgroup of the group.</param>
    /// <returns>An array of left coset representatives.</returns>
    public static Permutation[] LeftCosetRepresentatives(List<BSGSElement> group, List<BSGSElement> subgroup)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Finds a coset representative for a given group element.
    /// </summary>
    /// <param name="element">The group element.</param>
    /// <param name="group">The group.</param>
    /// <param name="subgroup">The subgroup.</param>
    /// <returns>The coset representative of the element.</returns>
    public static Permutation LeftTransversalOf(Permutation element, List<BSGSElement> group, List<BSGSElement> subgroup)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Calculates the intersection of two subgroups.
    /// </summary>
    /// <param name="group1">The first group.</param>
    /// <param name="group2">The second group.</param>
    /// <param name="intersection">The resulting intersection of the two groups.</param>
    public static void Intersection(List<BSGSElement> group1, List<BSGSElement> group2, List<BSGSCandidateElement> intersection)
    {
        throw new NotImplementedException();
    }
}