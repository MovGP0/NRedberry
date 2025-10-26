namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Arith;

/// <summary>
/// Combinatoric algorithms. Similar to ALDES/SAC2 SACCOMB module.
/// </summary>
/// <remarks>
/// Original Java file: cc.redberry.core.transformations.factor.jasfactor.edu.jas.arith.Combinatoric
/// </remarks>
public class Combinatoric
{
    /// <summary>
    /// Factorial.
    /// </summary>
    /// <param name="n">integer</param>
    /// <returns>n!, with 0! = 1.</returns>
    public static BigInteger Factorial(long n)
    {
        if (n <= 1)
        {
            return BigInteger.One;
        }
        BigInteger f = BigInteger.One;
        if (n >= int.MaxValue)
        {
            throw new NotSupportedException(n + " >= int.MaxValue = " + int.MaxValue);
        }
        for (int i = 2; i <= n; i++)
        {
            f = f.Multiply(new BigInteger(i));
        }
        return f;
    }
}
