using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;
using Shouldly;
using Xunit;

namespace NRedberry.JAS.Tests;

public sealed class ExpVectorIntegerTests
{
    [Fact]
    public void ShouldExtendContractAndAbsValues()
    {
        ExpVectorInteger vector = new([1L, -2L, 3L]);

        vector.Extend(1, 0, 4).GetVal().ShouldBe([4L, 1L, -2L, 3L]);
        vector.Contract(1, 2).GetVal().ShouldBe([-2L, 3L]);
        vector.Abs().GetVal().ShouldBe([1L, 2L, 3L]);
    }
}
