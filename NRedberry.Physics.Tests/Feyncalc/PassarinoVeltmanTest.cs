using System;
using NRedberry.Physics.Feyncalc;
using NRedberry.Tensors;
using NRedberry.Transformations.Expand;
using NRedberry.Transformations.Symmetrization;
using TensorFactory = NRedberry.Tensors.Tensors;

namespace NRedberry.Physics.Tests.Feyncalc;

public sealed class PassarinoVeltmanTest
{
    public void Test1()
    {
        Expression subs = PassarinoVeltman.GenerateSubstitution(
            1,
            TensorFactory.ParseSimple("q_a"),
            new[] { TensorFactory.ParseSimple("k1_a") });

        AssertEquals("q_a = q_b*k1^b * k1_a/(k1_c*k1^c)", subs);
    }

    public void Test2()
    {
        TransformationCollection simpl = new(
            TensorFactory.ParseExpression("k1_a*k1^a = 0"),
            TensorFactory.ParseExpression("k2_a*k2^a = 0"),
            TensorFactory.ParseExpression("k1_a*k2^a = s"));

        Expression subs = PassarinoVeltman.GenerateSubstitution(
            1,
            TensorFactory.ParseSimple("q_a"),
            new[] { TensorFactory.ParseSimple("k1_a"), TensorFactory.ParseSimple("k2_a") },
            simpl);

        AssertEquals("q_a = k1_a*(q^b*k2_b)/s + k2_a*(q^b*k1_b)/s", subs);
    }

    public void Test3()
    {
        TransformationCollection simpl = new(
            TensorFactory.ParseExpression("k1_a*k1^a = m1"),
            TensorFactory.ParseExpression("k2_a*k2^a = m2"),
            TensorFactory.ParseExpression("k1_a*k2^a = s"));

        Expression subs = PassarinoVeltman.GenerateSubstitution(
            1,
            TensorFactory.ParseSimple("q_a"),
            new[] { TensorFactory.ParseSimple("k1_a"), TensorFactory.ParseSimple("k2_a") },
            simpl);

        Tensor expected = TensorFactory.Parse(
            "k1_a*((q^b*k2_b)*s - (q^b*k1_b)*m2)/(s**2 - m1*m2) + k2_a*((q^b*k1_b)*s - (q^b*k2_b)*m1)/(s**2 - m1*m2)");

        Tensor expandedExpected = ExpandTransformation.Expand(expected);
        Tensor expandedActual = ExpandTransformation.Expand(subs[1]);
        if (!TensorUtils.Equals(expandedExpected, expandedActual))
        {
            throw new InvalidOperationException("Expanded substitution does not match expected result.");
        }
    }

    public void Test4()
    {
        Tensor[][] input =
        {
            new[] { TensorFactory.Parse("k1_i"), TensorFactory.Parse("0") },
            new[] { TensorFactory.Parse("k2_i"), TensorFactory.Parse("0") },
            new[] { TensorFactory.Parse("k3_i"), TensorFactory.Parse("m") },
            new[] { TensorFactory.Parse("k4_i"), TensorFactory.Parse("m") }
        };
        TransformationCollection simpl = new(FeynCalcUtils.SetMandelstam(input));

        Expression subs = PassarinoVeltman.GenerateSubstitution(
            4,
            TensorFactory.ParseSimple("q_a"),
            Array.Empty<SimpleTensor>(),
            simpl);

        Tensor tensor = subs[1];
        Console.WriteLine(ExpandTransformation.Expand(tensor).Size);
    }

    private static void AssertEquals(string expected, Tensor actual)
    {
        if (!TensorUtils.Equals(TensorFactory.Parse(expected), actual))
        {
            throw new InvalidOperationException("Tensor comparison failed.");
        }
    }
}
