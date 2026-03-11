using NRedberry.Solver.Frobenius;
using Xunit;

namespace NRedberry.Core.Tests.Solver.Frobenius;

public sealed class SolutionProviderTests
{
    [Fact]
    public void ShouldBeInterface()
    {
        Assert.True(SolutionProviderType.IsInterface);
    }

    [Fact]
    public void ShouldDeclareTickMethodReturningBoolean()
    {
        var tick = SolutionProviderType.GetMethod("Tick");

        Assert.NotNull(tick);
        Assert.Equal(typeof(bool), tick!.ReturnType);
    }

    [Fact]
    public void ShouldDeclareCurrentRemaindersMethodReturningIntegerArray()
    {
        var currentRemainders = SolutionProviderType.GetMethod("CurrentRemainders");

        Assert.NotNull(currentRemainders);
        Assert.Equal(typeof(int[]), currentRemainders!.ReturnType);
    }

    [Fact]
    public void ShouldInheritFromOutputPortOfIntegerArray()
    {
        var baseInterface = typeof(NRedberry.Concurrent.IOutputPort<int[]>);

        Assert.Contains(baseInterface, SolutionProviderType.GetInterfaces());
    }

    private static Type SolutionProviderType => GetFrobeniusType("SolutionProvider");

    private static Type GetFrobeniusType(string name)
    {
        return typeof(FrobeniusSolver).Assembly.GetType($"NRedberry.Solver.Frobenius.{name}", true)!;
    }
}
