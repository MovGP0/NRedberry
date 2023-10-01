namespace NRedberry.Apache.Commons.Math;

/// <summary>
/// Defines the methods that elements of a mathematical field must implement.
/// </summary>
/// <typeparam name="T">The type of the field element.</typeparam>
public interface IFieldElement<T>
{
    /// <summary>
    /// Adds the given element to this element.
    /// </summary>
    /// <param name="a">The element to add.</param>
    /// <returns>A new element representing the sum of this element and <paramref name="a"/>.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="a"/> is null.</exception>
    T Add(T a);

    /// <summary>
    /// Subtracts the given element from this element.
    /// </summary>
    /// <param name="a">The element to subtract.</param>
    /// <returns>A new element representing the difference between this element and <paramref name="a"/>.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="a"/> is null.</exception>
    T Subtract(T a);

    /// <summary>
    /// Returns the additive inverse of this element.
    /// </summary>
    /// <returns>The additive inverse of this element.</returns>
    T Negate();

    /// <summary>
    /// Multiplies this element by an integer.
    /// </summary>
    /// <param name="n">The integer to multiply by.</param>
    /// <returns>A new element representing this element multiplied by <paramref name="n"/>.</returns>
    T Multiply(int n);

    /// <summary>
    /// Multiplies this element by another element.
    /// </summary>
    /// <param name="a">The element to multiply by.</param>
    /// <returns>A new element representing the product of this element and <paramref name="a"/>.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="a"/> is null.</exception>
    T Multiply(T a);

    /// <summary>
    /// Divides this element by another element.
    /// </summary>
    /// <param name="a">The element to divide by.</param>
    /// <returns>A new element representing the quotient of this element and <paramref name="a"/>.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="a"/> is null.</exception>
    /// <exception cref="DivideByZeroException">Thrown when <paramref name="a"/> is zero.</exception>
    T Divide(T a);

    /// <summary>
    /// Returns the multiplicative inverse of this element.
    /// </summary>
    /// <returns>The multiplicative inverse of this element.</returns>
    /// <exception cref="DivideByZeroException">Thrown when this element is zero.</exception>
    T Reciprocal();

    /// <summary>
    /// Gets the field to which this element belongs.
    /// </summary>
    /// <returns>The field to which this element belongs.</returns>
    IField<T> Field { get; }
}