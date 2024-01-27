using System;
using NRedberry.Core.Combinatorics;

namespace NRedberry.Core.Groups;

public class PermutationOneLineByte : Permutation
{
    private readonly byte[] permutation;
    private readonly byte internalDegree; // MAX_VALUE = 127 => max permutation length = 126
    private readonly bool isIdentity;
    private readonly bool antisymmetry;

    /// <summary>
    /// Creates a permutation with antisymmetry property from a given array in one-line notation and a boolean value of antisymmetry.
    /// </summary>
    /// <param name="antisymmetry">Antisymmetry (true for antisymmetry, false for symmetry).</param>
    /// <param name="permutation">Permutation in one-line notation.</param>
    /// <exception cref="ArgumentException">Thrown if the permutation is inconsistent with one-line notation.</exception>
    /// <exception cref="ArgumentException">Thrown if antisymmetry is true and permutation order is odd.</exception>
    public PermutationOneLineByte(bool antisymmetry, params byte[] permutation)
    {
        if (!Permutations.TestPermutationCorrectness(permutation, antisymmetry))
            throw new ArgumentException("Inconsistent permutation: " + string.Join(", ", permutation));
        this.permutation = (byte[])permutation.Clone();
        this.antisymmetry = antisymmetry;
        this.isIdentity = Permutations.IsIdentity(permutation);
        this.internalDegree = Permutations.InternalDegree(permutation);
    }

    //!no check for one-line notation => unsafe constructor
    internal PermutationOneLineByte(bool isIdentity, bool antisymmetry, byte internalDegree, byte[] permutation)
    {
        this.isIdentity = isIdentity;
        this.permutation = permutation;
        this.antisymmetry = antisymmetry;
        this.internalDegree = internalDegree;
        if (antisymmetry && Permutations.OrderOfPermutationIsOdd(permutation))
            throw new InconsistentGeneratorsException();
    }

    //!!no any checks, used only to create inverse or identity permutation
    internal PermutationOneLineByte(bool isIdentity, bool antisymmetry, byte internalDegree, byte[] permutation, bool identity)
    {
        if (!identity)
            throw new ArgumentException("This constructor is only for creating an identity permutation.");
        this.permutation = permutation;
        this.antisymmetry = antisymmetry;
        this.isIdentity = isIdentity;
        this.internalDegree = internalDegree;
    }

    // TODO: implement the rest of the methods
}