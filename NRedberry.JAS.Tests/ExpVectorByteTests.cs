using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;
using Xunit;

namespace NRedberry.JAS.Tests;

public sealed class ExpVectorByteTests
{
    [Fact]
    public void ShouldStoreAndReverseByteBackedValues()
    {
        ExpVectorByte vector = new([1L, 2L, 3L]);

        vector.GetVal().ShouldBe([1L, 2L, 3L]);
        vector.Reverse().GetVal().ShouldBe([3L, 2L, 1L]);
        vector.MaxDeg().ShouldBe(3);
    }
}
