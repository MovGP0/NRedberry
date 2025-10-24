using System.Collections;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Arith;

/// <summary>
/// List of big primes. Provides an Iterator for generating prime numbers.
/// Similar to ALDES/SAC2 SACPOL.PRIME list.
/// </summary>
/// <remarks>
/// Original Java file: cc.redberry.core.transformations.factor.jasfactor.edu.jas.arith.PrimeList
/// See Knuth vol 2, page 390, for list of known primes. See also ALDES/SAC2 SACPOL.PRIME
/// </remarks>
public sealed class PrimeList : IEnumerable<BigInteger>
{
    public enum Range
    {
        Small,
        Low,
        Medium,
        Large,
        Mersenne
    }

    private static volatile List<BigInteger>? SMALL_LIST = null;
    private static volatile List<BigInteger>? LOW_LIST = null;
    private static volatile List<BigInteger>? MEDIUM_LIST = null;
    private static volatile List<BigInteger>? LARGE_LIST = null;
    private static volatile List<BigInteger>? MERSENNE_LIST = null;

    private List<BigInteger>? val = null;
    private BigInteger last;

    public PrimeList() : this(Range.Medium)
    {
    }

    public PrimeList(Range r)
    {
        throw new NotImplementedException();
    }

    private void AddSmall()
    {
        throw new NotImplementedException();
    }

    private void AddLow()
    {
        throw new NotImplementedException();
    }

    private void AddMedium()
    {
        throw new NotImplementedException();
    }

    private void AddLarge()
    {
        throw new NotImplementedException();
    }

    private void AddMersenne()
    {
        throw new NotImplementedException();
    }

    public static BigInteger GetLongPrime(int n, int m)
    {
        throw new NotImplementedException();
    }

    public static BigInteger GetMersennePrime(int n)
    {
        throw new NotImplementedException();
    }

    public override string ToString()
    {
        throw new NotImplementedException();
    }

    public int Size()
    {
        throw new NotImplementedException();
    }

    public BigInteger Get(int i)
    {
        throw new NotImplementedException();
    }

    public IEnumerator<BigInteger> GetEnumerator()
    {
        throw new NotImplementedException();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
