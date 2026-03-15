using NRedberry.Physics.Feyncalc;
using NRedberry.Tensors;
using Shouldly;
using Xunit;

namespace NRedberry.Physics.Tests.Feyncalc;

public sealed class AbstractFeynCalcTestTests
{
    [Fact]
    public void ShouldInitializeSharedFeynCalcState()
    {
        AbstractFeynCalcTestProbe probe = new();

        probe.SetUp(1);

        probe.DiracOrder.ShouldNotBeNull();
        probe.DiracTrace.ShouldNotBeNull();
        probe.TraceOfOne.ShouldNotBeNull();
        probe.DeltaTrace.ShouldNotBeNull();
    }

    [Fact]
    public void ShouldReadDiracOrderResourceCases()
    {
        AbstractFeynCalcTestProbe probe = new();

        string[] lines = probe.ReadResourceLines("DiracOrder_abcd");

        lines[0].ShouldBe("G_a*G_b*G_c*G_d");
        lines[1].ShouldBe("G_a*G_b*G_c*G_d");
        lines.Length.ShouldBeGreaterThan(2);
    }
}

public sealed class AbstractFeynCalcTestProbe : AbstractFeynCalcTest
{
    public DiracOrderTransformation? DiracOrder => dOrder;

    public DiracTraceTransformation? DiracTrace => dTrace;

    public DiracSimplify1? DiracSimplify1 => dSimplify1;

    public DiracSimplify0? DiracSimplify0 => dSimplify0;

    public Expression? TraceOfOne => traceOfOne;

    public Expression? DeltaTrace => deltaTrace;

    public string[] ReadResourceLines(string resourceFile)
    {
        return ReadFeynCalcResourceLines(resourceFile);
    }
}
