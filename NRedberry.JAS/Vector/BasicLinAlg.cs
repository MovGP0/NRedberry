using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Structure;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Vector;

/// <summary>
/// Basic linear algebra methods. Implements Basic linear algebra computations and tests.
/// Note: will use wrong method dispatch in JRE when used with GenSolvablePolynomial.
/// </summary>
/// <typeparam name="C">coefficient type</typeparam>
/// <remarks>
/// Original Java file: cc.redberry.core.transformations.factor.jasfactor.edu.jas.vector.BasicLinAlg
/// </remarks>
public class BasicLinAlg<C> where C : RingElem<C>
{
    /// <summary>
    /// Addition of vectors of ring elements.
    /// </summary>
    /// <param name="a">a ring element list</param>
    /// <param name="b">a ring element list</param>
    /// <returns>a+b, the vector sum of a and b.</returns>
    public List<C>? VectorAdd(List<C>? a, List<C>? b)
    {
        if (a == null)
        {
            return b;
        }

        if (b == null)
        {
            return a;
        }

        List<C> result = new(a.Count);
        using IEnumerator<C> ai = a.GetEnumerator();
        using IEnumerator<C> bi = b.GetEnumerator();
        while (ai.MoveNext() && bi.MoveNext())
        {
            C sum = ai.Current.Sum(bi.Current);
            result.Add(sum);
        }

        // Note: original implementation ignores trailing elements if lists differ in length.
        return result;
    }

    /// <summary>
    /// Test vector of zero ring elements.
    /// </summary>
    /// <param name="a">a ring element list</param>
    /// <returns>true, if all polynomial in a are zero, else false.</returns>
    public bool IsZero(List<C?>? a)
    {
        if (a is null)
        {
            return true;
        }

        return a.OfType<C>().All(element => element.IsZero());
    }
}
