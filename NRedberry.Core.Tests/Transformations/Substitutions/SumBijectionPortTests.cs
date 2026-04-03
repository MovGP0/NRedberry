using NRedberry.Transformations.Substitutions;
using TensorCC = NRedberry.Tensors.CC;
using TensorApi = NRedberry.Tensors.Tensors;
using TensorType = NRedberry.Tensors.Tensor;

namespace NRedberry.Core.Tests.Transformations.Substitutions;

public sealed class SumBijectionPortTests
{
    [Fact]
    public void ShouldReturnNullWhenFromHasMoreSummandsThanTarget()
    {
        SumBijectionPort port = new(TensorApi.Parse("a+b+c"), TensorApi.Parse("a+b"));

        port.Take().ShouldBeNull();
    }

    [Fact]
    public void ShouldReturnNullWhenTargetHasNoMatchingHashStretch()
    {
        SumBijectionPort port = new(TensorApi.Parse("c+d"), TensorApi.Parse("f_cd+V_cd"));

        port.Take().ShouldBeNull();
    }

    [Fact]
    public void ShouldFindDistinctBijectionForJavaCase()
    {
        TensorCC.ResetTensorNames(unchecked((int)2634486062579664417L));

        TensorType target = TensorApi.Parse("f_i + R_ijk*F^kj + R_ijk*F^jk - R_kij*F^jk");
        TensorType from = TensorApi.Parse("f_i + R_ijk*F^kj - R_kij*F^jk");

        SumBijectionPort port = new(from, target);
        BijectionContainer result = port.Take();

        result.ShouldNotBeNull();
        result.Bijection.Length.ShouldBe(3);
        result.Bijection.Distinct().Count().ShouldBe(3);
        result.Bijection.ShouldAllBe(index => index >= 0 && index < target.Size);
    }

    [Fact]
    public void ShouldFindAtLeastOneBijectionForSimpleSum()
    {
        SumBijectionPort port = new(TensorApi.Parse("a+b"), TensorApi.Parse("a+b+c"));

        BijectionContainer result = port.Take();

        result.ShouldNotBeNull();
        result.Bijection.Length.ShouldBe(2);
        result.Bijection.Distinct().Count().ShouldBe(2);
        result.Bijection.ShouldAllBe(index => index >= 0 && index < 3);
    }

    [Fact]
    public void ShouldHandleAntisymmetricSummands()
    {
        TensorCC.Reset();
        TensorCC.SetParserAllowsSameVariance(true);
        TensorApi.ParseSimple("b_nm").SimpleIndices.Symmetries.AddAntiSymmetry(IndexType.LatinLower, 1, 0);

        SumBijectionPort port = new(
            TensorApi.Parse("a_mn+b_mn"),
            TensorApi.Parse("a_mn-b_nm+c_mn"));

        BijectionContainer result = port.Take();

        result.ShouldNotBeNull();
        result.Bijection.Length.ShouldBe(2);
    }
}
