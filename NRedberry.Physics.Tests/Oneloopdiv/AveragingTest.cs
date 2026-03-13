using System;
using NRedberry;
using NRedberry.Numbers;
using NRedberry.Physics.Oneloopdiv;
using NRedberry.Tensors;
using NRedberry.Transformations.Expand;
using NRedberry.Transformations.Symmetrization;
using TensorCC = NRedberry.Tensors.CC;
using TensorFactory = NRedberry.Tensors.Tensors;
using Xunit;

namespace NRedberry.Physics.Tests.Oneloopdiv;

public sealed class AveragingTest
{
    [Fact]
    public void ShouldLeaveScalarUntouched()
    {
        Averaging averaging = new(TensorFactory.ParseSimple("n_\\mu"));
        Tensor transformed = averaging.Transform(Complex.One);
        Assert.Same(Complex.One, transformed);
    }

    public void Test4_0()
    {
        for (int i = 0; i < 100; ++i)
        {
            TensorCC.ResetTensorNames();
            Tensor t = TensorFactory.Parse("n^\\mu*n_\\mu*n_\\alpha*n^\\alpha*n_\\nu*n^\\nu");
            Expression d = (Expression)TensorFactory.Parse("d_\\mu^\\mu=4");
            t = new Averaging(TensorFactory.ParseSimple("n_\\mu")).Transform(t);
            t = ExpandTransformation.Expand(t, EliminateMetricsTransformation.Instance, d);
            t = EliminateMetricsTransformation.Eliminate(t);
            t = d.Transform(t);
            if (!TensorUtils.IsOne(t))
            {
                Console.WriteLine(t);
            }

            AssertTrue(TensorUtils.IsOne(t));
        }
    }

    public void Test4()
    {
        Tensor t = TensorFactory.Parse("n^\\mu*n_\\mu*n_\\alpha*n^\\alpha*n_\\nu*n^\\nu*n_\\lambda*n^\\lambda*n_\\rho*n^\\rho");
        Expression d = (Expression)TensorFactory.Parse("d_\\mu^\\mu=4");
        t = new Averaging(TensorFactory.ParseSimple("n_\\mu")).Transform(t);
        t = ExpandTransformation.Expand(t, EliminateMetricsTransformation.Instance);
        t = EliminateMetricsTransformation.Eliminate(t);
        t = d.Transform(t);
        AssertTrue(TensorUtils.IsOne(t));
    }

    public void Test5()
    {
        AddSymmetry("F_{\\mu\\nu\\alpha\\beta}", IndexType.GreekLower, true, 1, 0, 2, 3);
        Tensor ff = TensorFactory.Parse("FF=(-1/6)*F^{\\nu \\beta \\epsilon }_{\\zeta }*F_{\\nu \\beta }^{\\zeta }_{\\epsilon }+n^{\\mu }*F^{\\alpha }_{\\nu }^{\\epsilon }_{\\lambda }*n^{\\nu }*F_{\\alpha \\mu }^{\\lambda }_{\\epsilon }+(-8/3)*n^{\\mu }*F_{\\beta \\nu }^{\\epsilon }_{\\lambda }*n^{\\alpha }*n^{\\beta }*n^{\\nu }*F_{\\alpha \\mu }^{\\lambda }_{\\epsilon }");
        ff = new Averaging(TensorFactory.ParseSimple("n_\\mu")).Transform(ff);
        ff = ExpandTransformation.Expand(ff);
        ff = EliminateMetricsTransformation.Instance.Transform(ff);
        ff = ((Expression)TensorFactory.Parse("F_{\\mu}^\\mu_\\alpha\\beta=0")).Transform(ff);

        Console.WriteLine(ff);
    }

    public void Test6()
    {
        Tensor t = TensorFactory.Parse("a*n_\\mu*n_\\nu");
        t = new Averaging(TensorFactory.ParseSimple("n_\\mu")).Transform(t);
        Tensor expected = TensorFactory.Parse("1/4*a*g_\\mu\\nu");
        AssertTrue(TensorUtils.Equals(t, expected));
    }

    public void Test7()
    {
        Tensor t = TensorFactory.Parse("a*n_\\mu*n_\\nu+g_{\\mu\\nu}*n_\\alpha*n^\\alpha+n_\\mu*n_\\nu*n_\\alpha*g^\\alpha");
        Expression d = TensorFactory.ParseExpression("d_\\mu^\\mu =4");
        t = new Averaging(TensorFactory.ParseSimple("n_\\mu")).Transform(t);
        t = ExpandTransformation.Expand(t, EliminateMetricsTransformation.Instance, d);
        t = EliminateMetricsTransformation.Eliminate(t);
        t = d.Transform(t);
        Tensor expected = TensorFactory.Parse("(1/4*a+1)*g_\\mu\\nu");
        AssertTrue(TensorUtils.Equals(t, expected));
    }

    public void Test8()
    {
        Tensor t = TensorFactory.Parse("1");
        t = new Averaging(TensorFactory.ParseSimple("n_\\mu")).Transform(t);
        Tensor expected = TensorFactory.Parse("1");
        AssertTrue(TensorUtils.Equals(t, expected));
    }

    public void Test9()
    {
        Tensor t = TensorFactory.Parse("n_\\mu*n_\\nu*n^\\alpha*n^\\beta");
        t = new Averaging(TensorFactory.ParseSimple("n_\\mu")).Transform(t);
        AssertTrue(TensorUtils.Equals(t, TensorFactory.Parse("1/24*(d^{\\alpha }_{\\nu }*d^{\\beta }_{\\mu }+d^{\\alpha }_{\\mu }*d^{\\beta }_{\\nu }+g^{\\alpha \\beta }*g_{\\mu \\nu })")));
    }

    public void Test10()
    {
        Tensor t = TensorFactory.Parse("n_\\mu*n_\\nu*n^\\alpha*n^\\beta*n^\\gamma*n^\\lambda*n^\\sigma*n^\\rho*n^\\theta*n^\\zeta");
        _ = new Averaging(TensorFactory.ParseSimple("n_\\mu")).Transform(t);
    }

    public void Test11()
    {
        // Ignored in Java.
    }

    public void Test12()
    {
        // Ignored in Java.
    }

    public void Test13()
    {
        // Ignored in Java.
    }

    public void Test14()
    {
        // Ignored in Java.
    }

    public void Test15()
    {
        // Ignored in Java.
    }

    private static void AddSymmetry(string tensor, IndexType type, bool sign, params int[] permutation)
    {
        var simple = TensorFactory.ParseSimple(tensor);
        simple.SimpleIndices.Symmetries.Add(type, sign, permutation);
    }

    private static void AssertTrue(bool condition)
    {
        if (!condition)
        {
            throw new InvalidOperationException("Assertion failed.");
        }
    }
}
