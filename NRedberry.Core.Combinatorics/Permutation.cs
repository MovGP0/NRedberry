using System.Collections.Immutable;
using System.Numerics;

namespace NRedberry.Core.Combinatorics;

/// <summary>
/// Interface describing a single permutation.
/// See <c>Permutations.CreatePermutation(bool, int[])</c> and
/// <c>Permutations.CreatePermutation(bool, int[][])</c> for how to construct permutations.
/// </summary>
/// <remarks>
/// This is a C# transcription of the Java interface
/// <c>cc.redberry.core.groups.permutations.Permutation</c>.
/// The semantics of the methods are kept as close as possible to the original:
/// one-line notation, disjoint cycles, composition order
/// (“this * other” means apply <c>this</c> first, then <c>other</c>), antisymmetry flag,
/// and the various helpers around conjugation, commutators, and powers.
/// </remarks>
public interface Permutation : IComparable<Permutation>
{
    /// <summary>
    /// Returns array that represents this permutation in one-line notation.
    /// </summary>
    /// <returns>Array that represents this permutation in one-line notation.</returns>
    int[] OneLine();

    /// <summary>
    /// Returns immutable array that represents this permutation in one-line notation.
    /// </summary>
    /// <returns>Immutable array that represents this permutation in one-line notation.</returns>
    ImmutableArray<int> OneLineImmutable();

    /// <summary>
    /// Returns an array of disjoint cycles that represent this permutation.
    /// </summary>
    /// <returns>Array of disjoint cycles that represent this permutation.</returns>
    int[][] Cycles();

    /// <summary>
    /// Returns image of specified point under the action of this permutation.
    /// </summary>
    /// <param name="i">Point.</param>
    /// <returns>Image of specified point under the action of this permutation.</returns>
    int NewIndexOf(int i);

    /// <summary>
    /// Returns image of specified point under the action of this permutation.
    /// This method is the same as <see cref="NewIndexOf(int)"/>.
    /// </summary>
    /// <param name="i">Point.</param>
    /// <returns>Image of specified point under the action of this permutation.</returns>
    int ImageOf(int i);

    /// <summary>
    /// Returns image of specified set of points under the action of this permutation.
    /// </summary>
    /// <param name="set">Set of points.</param>
    /// <returns>Image of the specified set under this permutation.</returns>
    int[] ImageOf(int[] set);

    /// <summary>
    /// Permutes array and returns the result.
    /// </summary>
    /// <param name="array">Array to permute.</param>
    /// <returns>Permuted array.</returns>
    int[] Permute(int[] array);

    /// <summary>
    /// Permutes array and returns the result.
    /// </summary>
    /// <param name="array">Array to permute.</param>
    /// <returns>Permuted array.</returns>
    char[] Permute(char[] array);

    /// <summary>
    /// Permutes array and returns the result.
    /// </summary>
    /// <typeparam name="T">Element type.</typeparam>
    /// <param name="array">Array to permute.</param>
    /// <returns>Permuted array.</returns>
    T[] Permute<T>(T[] array);

    /// <summary>
    /// Permutes list and returns the result.
    /// </summary>
    /// <typeparam name="T">Element type.</typeparam>
    /// <param name="list">List to permute.</param>
    /// <returns>Permuted list.</returns>
    List<T> Permute<T>(List<T> list);

    /// <summary>
    /// Returns conjugation of specified element by this permutation, i.e. <c>this⁻¹ * p * this</c>.
    /// </summary>
    /// <param name="p">Permutation to conjugate.</param>
    /// <returns>Conjugation of specified element by this permutation.</returns>
    /// <exception cref="Exception">
    /// Should be thrown if the result of composition is inconsistent symmetry
    /// (antisymmetry with odd parity of permutation).
    /// </exception>
    Permutation Conjugate(Permutation p);

    /// <summary>
    /// Returns commutator of this and specified permutation, i.e. <c>this⁻¹ * p⁻¹ * this * p</c>.
    /// </summary>
    /// <param name="p">Permutation.</param>
    /// <returns>Commutator of this and specified permutation.</returns>
    /// <exception cref="Exception">
    /// Should be thrown if the result of composition is inconsistent symmetry
    /// (antisymmetry with odd parity of permutation).
    /// </exception>
    Permutation Commutator(Permutation p);

    /// <summary>
    /// Returns image of specified point under the action of inverse of this permutation.
    /// </summary>
    /// <param name="i">Point.</param>
    /// <returns>Image of specified point under the action of inverse of this permutation.</returns>
    int NewIndexOfUnderInverse(int i);

    /// <summary>
    /// Returns <c>true</c> if this permutation is antisymmetry and <c>false</c> otherwise.
    /// </summary>
    /// <returns><c>true</c> if this permutation is antisymmetry; otherwise <c>false</c>.</returns>
    bool Antisymmetry();

    /// <summary>
    /// If this is antisymmetry, then converts this permutation to symmetry.
    /// </summary>
    /// <returns>Same permutation with <c>false</c> antisymmetry.</returns>
    Permutation ToSymmetry();

    /// <summary>
    /// Changes sign (symmetry to antisymmetry and vice versa) of this permutation.
    /// </summary>
    /// <returns>Same permutation with changed sign.</returns>
    Permutation Negate();

    /// <summary>
    /// Returns the result of <c>this * other</c>.
    /// Applying the resulting permutation is equivalent to applying <paramref name="other"/> after <c>this</c>.
    /// </summary>
    /// <param name="other">Other permutation.</param>
    /// <returns>The result of <c>this * other</c>.</returns>
    /// <exception cref="Exception">
    /// Should be thrown if the result of composition is inconsistent symmetry
    /// (antisymmetry with odd parity of permutation).
    /// </exception>
    Permutation Composition(Permutation other);

    /// <summary>
    /// Returns the result of <c>this * a * b</c>.
    /// Applying the resulting permutation is equivalent to applying <c>b</c> after <c>a</c> after <c>this</c>.
    /// </summary>
    /// <param name="a">First permutation.</param>
    /// <param name="b">Second permutation.</param>
    /// <returns>The result of <c>this * a * b</c>.</returns>
    /// <exception cref="Exception">
    /// Should be thrown if the result of composition is inconsistent symmetry
    /// (antisymmetry with odd permutation parity).
    /// </exception>
    Permutation Composition(Permutation a, Permutation b);

    /// <summary>
    /// Returns the result of <c>this * a * b * c</c>.
    /// Applying the resulting permutation is equivalent to applying <c>c</c> after <c>b</c> after <c>a</c> after <c>this</c>.
    /// </summary>
    /// <param name="a">First permutation.</param>
    /// <param name="b">Second permutation.</param>
    /// <param name="c">Third permutation.</param>
    /// <returns>The result of <c>this * a * b * c</c>.</returns>
    /// <exception cref="Exception">
    /// Should be thrown if the result of composition is inconsistent symmetry
    /// (antisymmetry with odd permutation parity).
    /// </exception>
    Permutation Composition(Permutation a, Permutation b, Permutation c);

    /// <summary>
    /// Returns the result of <c>this * other⁻¹</c>.
    /// Applying the resulting permutation is equivalent to applying <c>other⁻¹</c> after <c>this</c>.
    /// </summary>
    /// <param name="other">Other permutation.</param>
    /// <returns>The result of <c>this * other⁻¹</c>.</returns>
    /// <exception cref="Exception">
    /// Should be thrown if the result of composition is inconsistent symmetry
    /// (antisymmetry with odd permutation parity).
    /// </exception>
    Permutation CompositionWithInverse(Permutation other);

    /// <summary>
    /// Returns the inverse permutation of this.
    /// </summary>
    /// <returns>The inverse permutation of this.</returns>
    Permutation Inverse();

    /// <summary>
    /// Returns <c>true</c> if this represents identity permutation.
    /// </summary>
    /// <returns><c>true</c> if this is identity permutation.</returns>
    bool IsIdentity { get; }

    /// <summary>
    /// Returns the identity permutation with the degree of this permutation.
    /// </summary>
    /// <returns>Identity permutation with the degree of this permutation.</returns>
    Permutation Identity { get; }

    /// <summary>
    /// Calculates and returns the order of this permutation.
    /// </summary>
    /// <returns>Order of this permutation.</returns>
    BigInteger Order { get; }

    /// <summary>
    /// Returns <c>true</c> if order of this permutation is odd and <c>false</c> otherwise.
    /// </summary>
    /// <returns><c>true</c> if order of this permutation is odd; otherwise <c>false</c>.</returns>
    bool OrderIsOdd { get; }

    /// <summary>
    /// Returns a largest moved point plus one.
    /// </summary>
    /// <returns>Largest moved point plus one.</returns>
    int Degree { get; }

    /// <summary>
    /// Returns length of the underlying array (at low-level).
    /// </summary>
    /// <returns>Length of the underlying array (at low-level).</returns>
    int Length { get; }

    /// <summary>
    /// Returns this raised to the specified exponent.
    /// </summary>
    /// <param name="exponent">Exponent.</param>
    /// <returns>This permutation raised to the specified exponent.</returns>
    Permutation Pow(int exponent);

    /// <summary>
    /// Returns parity of this permutation.
    /// </summary>
    /// <returns>Parity of this permutation.</returns>
    int Parity { get; }

    /// <summary>
    /// Inserts identity action on the set [0, 1, ..., size - 1];
    /// as result the degree of resulting permutation will be <c>size + degree(this)</c>.
    /// </summary>
    /// <param name="size">Size of the set.</param>
    /// <returns>Permutation moved to the right by the specified size.</returns>
    Permutation MoveRight(int size);

    /// <summary>
    /// Returns lengths of cycles in disjoint cycle notation.
    /// </summary>
    /// <returns>Lengths of cycles in disjoint cycle notation.</returns>
    int[] LengthsOfCycles { get; }

    /// <summary>
    /// Returns a string representation of this permutation in one-line notation.
    /// </summary>
    /// <returns>A string representation of this permutation in one-line notation.</returns>
    string ToStringOneLine();

    /// <summary>
    /// Returns a string representation of this permutation in disjoint cycles notation.
    /// </summary>
    /// <returns>A string representation of this permutation in disjoint cycles notation.</returns>
    string ToStringCycles();
}
