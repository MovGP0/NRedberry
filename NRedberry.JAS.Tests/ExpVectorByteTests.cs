using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;
using Xunit;

namespace NRedberry.JAS.Tests;

public sealed class ExpVectorByteTests
{
    [Fact]
    public void ShouldStoreAndReverseByteBackedValues()
    {
        ExpVectorByte vector = new([1L, 2L, 3L]);

        Assert.Equal([1L, 2L, 3L], vector.GetVal());
        Assert.Equal([3L, 2L, 1L], vector.Reverse().GetVal());
        Assert.Equal(3, vector.MaxDeg());
    }
}
