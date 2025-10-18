using NRedberry.Core.Tensors;
using NRedberry.Core.Transformations.Symmetrization;
using NRedberry.Core.Utils;

namespace NRedberry.Core.IndexMapping;

/// <summary>
/// Skeleton port of cc.redberry.core.indexmapping.Mapping.
/// </summary>
public sealed class Mapping : ITransformation
{
    private readonly int[] fromNames = null!;
    private readonly int[] toData = null!;
    private readonly bool sign;

    public static Mapping IdentityMapping => throw new NotImplementedException();

    public Mapping(int[] from, int[] to)
    {
        throw new NotImplementedException();
    }

    public Mapping(int[] from, int[] to, bool sign)
    {
        throw new NotImplementedException();
    }

    internal Mapping(IIndexMappingBuffer buffer)
    {
        throw new NotImplementedException();
    }

    private Mapping(int[] fromNames, int[] toData, bool sign, bool _)
    {
        throw new NotImplementedException();
    }

    public Tensor Transform(Tensor tensor)
    {
        throw new NotImplementedException();
    }

    public bool IsEmpty()
    {
        throw new NotImplementedException();
    }

    public bool IsIdentity()
    {
        throw new NotImplementedException();
    }

    public bool GetSign()
    {
        throw new NotImplementedException();
    }

    public Mapping AddSign(bool sign)
    {
        throw new NotImplementedException();
    }

    public int Size()
    {
        throw new NotImplementedException();
    }

    public IntArray GetFromNames()
    {
        throw new NotImplementedException();
    }

    public IntArray GetToData()
    {
        throw new NotImplementedException();
    }

    public override bool Equals(object? obj)
    {
        throw new NotImplementedException();
    }

    public override int GetHashCode()
    {
        throw new NotImplementedException();
    }

    public override string ToString()
    {
        throw new NotImplementedException();
    }

    public static Mapping ValueOf(string value)
    {
        throw new NotImplementedException();
    }
}
