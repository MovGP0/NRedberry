namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;

public static partial class PolyUtil
{
    private static int PopCount(System.Numerics.BigInteger value)
    {
        value = System.Numerics.BigInteger.Abs(value);
        int count = 0;
        while (value > System.Numerics.BigInteger.Zero)
        {
            if (!value.IsEven)
            {
                count++;
            }

            value >>= 1;
        }

        return count;
    }
}
