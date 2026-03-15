using NRedberry.Physics.Feyncalc;
using NRedberry.Tensors;
using NRedberry.Transformations.Expand;
using NRedberry.Transformations.Symmetrization;
using Shouldly;
using TensorFactory = NRedberry.Tensors.Tensors;
using Xunit;

namespace NRedberry.Physics.Tests.Feyncalc;

public sealed class PassarinoVeltmanTest
{
    [Fact]
    public void Test1()
    {
        Expression subs = PassarinoVeltman.GenerateSubstitution(
            1,
            TensorFactory.ParseSimple("q_a"),
            [TensorFactory.ParseSimple("k1_a")]);

        subs.ToString().ShouldBe("q_{a} = k1_{c}*k1^{c}**(-1)*q_{b}*k1^{b}*k1_{a}");
    }

    [Fact(Skip = "Blocked by unported SubstitutionTransformation(Expression) used by simplification rules.")]
    public void Test2()
    {
        TransformationCollection simpl = new(
            TensorFactory.ParseExpression("k1_a*k1^a = 0"),
            TensorFactory.ParseExpression("k2_a*k2^a = 0"),
            TensorFactory.ParseExpression("k1_a*k2^a = s"));

        Expression subs = PassarinoVeltman.GenerateSubstitution(
            1,
            TensorFactory.ParseSimple("q_a"),
            [TensorFactory.ParseSimple("k1_a"), TensorFactory.ParseSimple("k2_a")],
            simpl);

        ShouldEqualTensor("q_a = k1_a*(q^b*k2_b)/s + k2_a*(q^b*k1_b)/s", subs);
    }

    [Fact(Skip = "Blocked by unported SubstitutionTransformation(Expression) used by simplification rules.")]
    public void Test3()
    {
        TransformationCollection simpl = new(
            TensorFactory.ParseExpression("k1_a*k1^a = m1"),
            TensorFactory.ParseExpression("k2_a*k2^a = m2"),
            TensorFactory.ParseExpression("k1_a*k2^a = s"));

        Expression subs = PassarinoVeltman.GenerateSubstitution(
            1,
            TensorFactory.ParseSimple("q_a"),
            [TensorFactory.ParseSimple("k1_a"), TensorFactory.ParseSimple("k2_a")],
            simpl);

        Tensor expected = TensorFactory.Parse(
            "k1_a*((q^b*k2_b)*s - (q^b*k1_b)*m2)/(s**2 - m1*m2) + k2_a*((q^b*k1_b)*s - (q^b*k2_b)*m1)/(s**2 - m1*m2)");

        Tensor expandedExpected = ExpandTransformation.Expand(expected);
        Tensor expandedActual = ExpandTransformation.Expand(subs[1]);
        TensorUtils.Equals(expandedExpected, expandedActual).ShouldBeTrue(
            "Expanded substitution should match the expected result.");
    }

    [Fact(Skip = "Blocked by unported SubstitutionTransformation(Expression) used by simplification rules.")]
    public void Test4()
    {
        Tensor[][] input =
        [
            [TensorFactory.Parse("k1_i"), TensorFactory.Parse("0")],
            [TensorFactory.Parse("k2_i"), TensorFactory.Parse("0")],
            [TensorFactory.Parse("k3_i"), TensorFactory.Parse("m")],
            [TensorFactory.Parse("k4_i"), TensorFactory.Parse("m")]
        ];
        TransformationCollection simpl = new(FeynCalcUtils.SetMandelstam(input));

        Expression subs = PassarinoVeltman.GenerateSubstitution(
            4,
            TensorFactory.ParseSimple("q_a"),
            [],
            simpl);
    }

    private static void ShouldEqualTensor(string expected, Tensor actual)
    {
        TensorUtils.Equals(TensorFactory.Parse(expected), actual).ShouldBeTrue("Tensor comparison failed.");
    }
}
