using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Ps;

/// <summary>
/// Abstract class for generating functions for coefficients of power series.
/// Was an interface, now this class handles the caching itself.
/// </summary>
/// <typeparam name="C">ring element type</typeparam>
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
    /// Get cached coefficient or generate coefficient.
    /// </summary>
    /// <param name="index">of requested coefficient</param>
    /// <returns>coefficient at index.</returns>
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
