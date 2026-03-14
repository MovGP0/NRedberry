using NRedberry.Physics.Feyncalc;
using NRedberry.Tensors;
using Shouldly;
using TensorFactory = NRedberry.Tensors.Tensors;
using Xunit;
using Xunit.Abstractions;

namespace NRedberry.Physics.Tests.Feyncalc;

public sealed class SchoutenIdentities4Test
{
    private readonly ITestOutputHelper testOutputHelper;

    public SchoutenIdentities4Test(ITestOutputHelper testOutputHelper)
    {
        this.testOutputHelper = testOutputHelper;
    }

    [Fact]
    public void ShouldThrowUntilSchoutenIdentities4IsPorted()
    {
        Should.Throw<NotImplementedException>(() => new SchoutenIdentities4(TensorFactory.ParseSimple("e_abcd")));
    }

    [Fact]
    public void Test1()
    {
        // TODO: setAntiSymmetric is not yet ported.
        SchoutenIdentities4 tr = new(TensorFactory.ParseSimple("e_abcd"));

        var t = TensorFactory.Parse("-2*g_{ad}*e_{bcef}+2*g_{ac}*e_{bdef}-2*g_{ab}*e_{cdef}-2*g_{af}*e_{bcde}+2*g_{ae}*e_{bcdf} + f_aebcdf");
        ShouldEqualTensor("f_aebcdf", tr.Transform(t));

        t = TensorFactory.Parse("-2*g_{ad}*e_{bcef}+2*g_{ac}*e_{bdef}-2*g_{ab}*e_{cdef}-2*g_{af}*e_{bcde}+g_{ae}*e_{bcdf} + f_aebcdf");
        ShouldBeSameReference(t, tr.Transform(t));

        t = TensorFactory.Parse("2*g_{ad}*e_{bcef}+2*g_{ac}*e_{bdef}-2*g_{ab}*e_{cdef}-2*g_{af}*e_{bcde}+2*g_{ae}*e_{bcdf} + f_aebcdf");
        ShouldBeSameReference(t, tr.Transform(t));

        t = TensorFactory.Parse("2*g_{ad}*e_{bcef}-2*g_{ac}*e_{bdef}+2*g_{ab}*e_{cdef}+2*g_{af}*e_{bcde}+2*g_{ae}*e_{cbdf} + f_aebcdf");
        ShouldEqualTensor("f_aebcdf", tr.Transform(t));
    }

    [Fact]
    public void Test2()
    {
        // TODO: setAntiSymmetric is not yet ported.
        SchoutenIdentities4 tr = new(TensorFactory.ParseSimple("e_abcd"));
        Tensor a = TensorFactory.Parse("(-4*I)*g_{cf}*e_{abde}+(-4*I)*g_{af}*e_{bcde}+(-4*I)*g_{ab}*e_{cdef}+(-4*I)*g_{be}*e_{acdf}+(4*I)*g_{bf}*e_{acde}+(4*I)*g_{ac}*e_{bdef}+(4*I)*g_{ce}*e_{abdf}+(-4*I)*g_{ef}*e_{abcd}+(4*I)*g_{ae}*e_{bcdf}+(-4*I)*g_{bc}*e_{adef}");
        Tensor b = TensorFactory.Parse("(-4*I)*g_{fd}*e_{ecba}+(4*I)*g_{cf}*e_{deba}+(-4*I)*g_{bd}*e_{efca}+(4*I)*g_{be}*e_{dfca}+(4*I)*g_{bf}*e_{ceda}+(-4*I)*g_{cd}*e_{bfea}+(-4*I)*g_{ce}*e_{dfba}+(-4*I)*g_{ef}*e_{bdca}+(4*I)*g_{bc}*e_{defa}+(4*I)*g_{ed}*e_{bfca}");
        Tensor c = TensorFactory.Subtract(a, b);
        c = tr.Transform(c);
        testOutputHelper.WriteLine(c.ToString());
        testOutputHelper.WriteLine(a.ToString());
        testOutputHelper.WriteLine(b.ToString());
        a = tr.Transform(a);
        testOutputHelper.WriteLine(a.ToString());
        b = tr.Transform(b);
        testOutputHelper.WriteLine(b.ToString());
        testOutputHelper.WriteLine(TensorUtils.Equals(a, b).ToString());
    }

    private static void ShouldEqualTensor(string expected, Tensor actual)
    {
        TensorUtils.Equals(TensorFactory.Parse(expected), actual).ShouldBeTrue();
    }

    private static void ShouldBeSameReference(Tensor expected, Tensor actual)
    {
        ReferenceEquals(expected, actual).ShouldBeTrue();
    }
}
