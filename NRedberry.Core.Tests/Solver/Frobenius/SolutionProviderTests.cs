using NRedberry.Solver.Frobenius;
using Xunit;

namespace NRedberry.Core.Tests.Solver.Frobenius;

public sealed class SolutionProviderTests
{
    [Fact]
    public void ShouldBeInterface()
    {
        SolutionProviderType.IsInterface.ShouldBeTrue();
    }

    [Fact]
    public void ShouldDeclareTickMethodReturningBoolean()
    {
        var tick = SolutionProviderType.GetMethod("Tick");

        tick.ShouldNotBeNull();
        tick!.ReturnType.ShouldBe(typeof(bool));
    }

    [Fact]
    public void ShouldDeclareCurrentRemaindersMethodReturningIntegerArray()
    {
        var currentRemainders = SolutionProviderType.GetMethod("CurrentRemainders");

        currentRemainders.ShouldNotBeNull();
        currentRemainders!.ReturnType.ShouldBe(typeof(int[]));
    }

    [Fact]
    public void ShouldInheritFromOutputPortOfIntegerArray()
    {
        var baseInterface = typeof(NRedberry.Concurrent.IOutputPort<int[]>);

        SolutionProviderType.GetInterfaces().ShouldContain(baseInterface);
    }

    private static Type SolutionProviderType => GetFrobeniusType("SolutionProvider");

    private static Type GetFrobeniusType(string name)
    {
        return typeof(FrobeniusSolver).Assembly.GetType($"NRedberry.Solver.Frobenius.{name}", true)!;
    }
}
