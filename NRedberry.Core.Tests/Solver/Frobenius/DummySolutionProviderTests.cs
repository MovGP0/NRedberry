using System.Reflection;
using NRedberry.Solver.Frobenius;
using Xunit;

namespace NRedberry.Core.Tests.Solver.Frobenius;

public sealed class DummySolutionProviderTests
{
    [Fact]
    public void ShouldThrowWhenSolutionIsNull()
    {
        var exception = Should.Throw<TargetInvocationException>(() => CreateProvider(null!, [1, 2]));

        ArgumentNullException innerException = exception.InnerException.ShouldBeOfType<ArgumentNullException>();
        innerException.ParamName.ShouldBe("solution");
    }

    [Fact]
    public void ShouldThrowWhenCurrentRemainderIsNull()
    {
        var exception = Should.Throw<TargetInvocationException>(() => CreateProvider([1, 2], null!));

        ArgumentNullException innerException = exception.InnerException.ShouldBeOfType<ArgumentNullException>();
        innerException.ParamName.ShouldBe("currentRemainder");
    }

    [Fact]
    public void ShouldReturnSolutionOnlyOnceAndUpdateTick()
    {
        int[] solution = [3, 5];
        object provider = CreateProvider(solution, [10]);

        Tick(provider).ShouldBeTrue();
        Take(provider).ShouldBeSameAs(solution);
        Tick(provider).ShouldBeFalse();
        Take(provider).ShouldBeNull();
    }

    [Fact]
    public void ShouldReturnCurrentRemaindersReference()
    {
        int[] remainders = [7, 11];
        object provider = CreateProvider([1], remainders);

        CurrentRemainders(provider).ShouldBeSameAs(remainders);
    }

    private static object CreateProvider(int[] solution, int[] currentRemainder)
    {
        return Activator.CreateInstance(ProviderType, solution, currentRemainder)!;
    }

    private static bool Tick(object provider)
    {
        return (bool)ProviderType.GetMethod("Tick")!.Invoke(provider, null)!;
    }

    private static int[]? Take(object provider)
    {
        return (int[]?)ProviderType.GetMethod("Take")!.Invoke(provider, null);
    }

    private static int[] CurrentRemainders(object provider)
    {
        return (int[])ProviderType.GetMethod("CurrentRemainders")!.Invoke(provider, null)!;
    }

    private static Type ProviderType
    {
        get
        {
            return typeof(FrobeniusSolver).Assembly.GetType("NRedberry.Solver.Frobenius.DummySolutionProvider", true)!;
        }
    }
}
