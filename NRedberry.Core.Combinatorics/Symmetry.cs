namespace NRedberry.Core.Combinatorics;

public class Symmetry : Permutation
{
    private bool sign { get; }

    // Constructor with permutation array and sign
    public Symmetry(int[] permutation, bool sign) : base(permutation)
    {
        this.sign = sign;
    }

    // Constructor with dimension
    public Symmetry(int dimension) : base(dimension)
    {
        sign = false;
    }

    public Symmetry(int[] permutation, bool sign, bool notClone) : base(permutation, notClone)
    {
        this.sign = sign;
    }

    public new Symmetry GetOne()
    {
        return new Symmetry(_permutation.Length);
    }

    public bool IsAntiSymmetry()
    {
        return sign;
    }

    public new Symmetry Composition(Permutation element)
    {
        var s = (Symmetry) element;
        return new Symmetry(CompositionArray(element), sign ^ s.sign, true);
    }

    public new Symmetry Inverse()
    {
        return new Symmetry(CalculateInverse(), sign);
    }

    public override bool Equals(object? obj)
    {
        if (obj == null) return false;
        if (GetType() != obj.GetType()) return false;
        var other = (Symmetry) obj;
        if (sign != other.sign) return false;
        return _permutation.SequenceEqual(other._permutation);
    }

    public override int GetHashCode()
    {
        return base.GetHashCode() + (sign ? 1 : 0);
    }

    public override string ToString()
    {
        return base.ToString() + "(" + (sign ? "-" : "+") + ")";
    }
}