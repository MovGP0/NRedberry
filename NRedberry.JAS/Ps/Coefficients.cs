using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Ps;

/// <summary>
/// Abstract base that mirrors the original {@code Coefficients} interface and now handles
/// coefficient-generation caching internally while remaining {@cSerializable}.
/// </summary>
/// <typeparam name="C">ring element type.</typeparam>
/// <remarks>
/// Original Java file: cc.redberry.core.transformations.factor.jasfactor.edu.jas.ps.Coefficients
/// </remarks>
public abstract class Coefficients<C> where C : RingElem<C>
{
    /// <summary>
    /// Cache for already computed coefficients.
    /// </summary>
    public readonly Dictionary<int, C> CoeffCache;

    /// <summary>
    /// Public no arguments constructor.
    /// </summary>
    protected Coefficients()
    {
        CoeffCache = new Dictionary<int, C>();
    }

    /// <summary>
    /// Public constructor with pre-filled cache.
    /// </summary>
    /// <param name="cache">pre-filled coefficient cache</param>
    protected Coefficients(Dictionary<int, C> cache)
    {
        CoeffCache = cache;
    }

    /// <summary>
    /// Looks up a coefficient in the cache or delegates to <see cref="Generate(int)"/> when missing.
    /// </summary>
    /// <param name="index">Index of the requested coefficient.</param>
    /// <returns>Coefficient at <paramref name="index"/>.</returns>
    public C Get(int index)
    {
        if (CoeffCache == null)
        {
            return Generate(index);
        }

        if (CoeffCache.TryGetValue(index, out C? value))
        {
            return value;
        }

        value = Generate(index);
        CoeffCache[index] = value;
        return value;
    }

    /// <summary>
    /// Generate coefficient.
    /// </summary>
    /// <param name="index">of requested coefficient</param>
    /// <returns>coefficient at index.</returns>
    protected abstract C Generate(int index);
}
