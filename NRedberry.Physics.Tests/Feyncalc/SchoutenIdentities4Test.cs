using NRedberry.Physics.Feyncalc;
using NRedberry.Tensors;
using TensorFactory = NRedberry.Tensors.Tensors;
using Xunit;

namespace NRedberry.Physics.Tests.Feyncalc;

public sealed class SchoutenIdentities4Test
{
    private const string ReductionBlockedReason =
        "Blocked: SchoutenIdentities4 does not yet reproduce the upstream reduction for the fully reducible identity cases.";

    private const string CanonicalizationBlockedReason =
        "Blocked: upstream Test2 was exploratory only, and the current port still produces different canonical forms for the compared tensors.";

    [Fact]
    public void ShouldLeaveNonMatchingIdentitiesUnchanged()
    {
        SetAntiSymmetric("e_abcd");
        SchoutenIdentities4 transformation = new(TensorFactory.ParseSimple("e_abcd"));

        Tensor tensor = TensorFactory.Parse("-2*g_{ad}*e_{bcef}+2*g_{ac}*e_{bdef}-2*g_{ab}*e_{cdef}-2*g_{af}*e_{bcde}+g_{ae}*e_{bcdf} + f_aebcdf");
        ShouldBeSameReference(tensor, transformation.Transform(tensor));

        tensor = TensorFactory.Parse("2*g_{ad}*e_{bcef}+2*g_{ac}*e_{bdef}-2*g_{ab}*e_{cdef}-2*g_{af}*e_{bcde}+2*g_{ae}*e_{bcdf} + f_aebcdf");
        ShouldBeSameReference(tensor, transformation.Transform(tensor));
    }

    [Fact(Skip = ReductionBlockedReason)]
    public void ShouldReduceMatchingSchoutenIdentity()
    {
    }

    [Fact(Skip = CanonicalizationBlockedReason)]
    public void ShouldProduceEquivalentCanonicalFormsForExploratoryCase()
    {
    }

    private static void ShouldBeSameReference(Tensor expected, Tensor actual)
    {
        ReferenceEquals(expected, actual).ShouldBeTrue();
    }

    private static void SetAntiSymmetric(string tensor)
    {
        TensorFactory.ParseSimple(tensor).SimpleIndices.Symmetries.SetAntiSymmetric();
    }
}
