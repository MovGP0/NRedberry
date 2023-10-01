namespace NRedberry.Apache.Commons.Math;

/// <summary>
/// Interface representing a field.
/// Classes implementing this interface will often be singletons.
/// </summary>
/// <typeparam name="T">The type of the field elements.</typeparam>
public interface IField<T>
{
    /// <summary>
    /// Gets the additive identity of the field.
    /// The additive identity is the element e0 of the field such that
    /// for all elements a of the field, the equalities a + e0 = e0 + a = a hold.
    /// </summary>
    /// <returns>The additive identity of the field.</returns>
    T Zero { get; }
    
    /// <summary>
    /// Gets the multiplicative identity of the field.
    /// The multiplicative identity is the element e1 of the field such that
    /// for all elements a of the field, the equalities a * e1 = e1 * a = a hold.
    /// </summary>
    /// <returns>The multiplicative identity of the field.</returns>
    T One { get; }

    /// <summary>
    /// Returns the runtime class of the FieldElement.
    /// </summary>
    /// <returns>The <see cref="Type"/> object that represents the runtime class of this object.</returns>
    TC GetRuntimeClass<TC>() where TC : IFieldElement<T>;
}