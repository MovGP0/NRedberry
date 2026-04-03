using System.Reflection;
using NRedberry.Solver.Frobenius;

namespace NRedberry.Core.Tests.Solver.Frobenius;

public sealed class SingleSolutionProviderTests
{
    [Fact]
    public void ShouldReturnNullWhenTakeCalledWithoutTick()
    {
        object provider = CreateSingleProvider([4], [5], 0, [2]);

        int[]? solution = Take(provider);

        solution.ShouldBeNull();
    }

    [Fact]
    public void ShouldEnumerateSolutionsUntilRemainderBecomesNegative()
    {
        object provider = CreateSingleProvider([7], [5], 0, [2]);

        Tick(provider).ShouldBeTrue();
        Take(provider).ShouldBe([7]);
        Take(provider).ShouldBe([8]);
        Take(provider).ShouldBe([9]);
        Take(provider).ShouldBeNull();
        Tick(provider).ShouldBeFalse();
    }

    [Fact]
    public void ShouldResetCurrentRemaindersAfterFailedTake()
    {
        object provider = CreateSingleProvider([3, 4], [5, 8], 1, [2, 3]);

        Tick(provider).ShouldBeTrue();
        Take(provider).ShouldBe([3, 4]);
        CurrentRemainders(provider).ShouldBe([5, 8]);
        Take(provider).ShouldBe([3, 5]);
        CurrentRemainders(provider).ShouldBe([3, 5]);
        Take(provider).ShouldBe([3, 6]);
        CurrentRemainders(provider).ShouldBe([1, 2]);
        Take(provider).ShouldBeNull();
    }

    private static object CreateSingleProvider(int[] baseSolution, int[] remainders, int position, int[] coefficients)
    {
        object dummyProvider = CreateDummyProvider(baseSolution, remainders);
        ConstructorInfo? constructor = SingleProviderType.GetConstructor([SolutionProviderType, typeof(int), typeof(int[])]);
        return constructor!.Invoke([dummyProvider, position, coefficients]);
    }

    private static object CreateDummyProvider(int[] solution, int[] remainders)
    {
        ConstructorInfo? constructor = DummyProviderType.GetConstructor([typeof(int[]), typeof(int[])]);
        return constructor!.Invoke([solution, remainders]);
    }

    private static bool Tick(object provider)
    {
        return (bool)provider.GetType().GetMethod("Tick", BindingFlags.Instance | BindingFlags.Public)!.Invoke(provider, null)!;
    }

    private static int[]? Take(object provider)
    {
        return (int[]?)provider.GetType().GetMethod("Take", BindingFlags.Instance | BindingFlags.Public)!.Invoke(provider, null);
    }

    private static int[] CurrentRemainders(object provider)
    {
        return (int[])provider.GetType().GetMethod("CurrentRemainders", BindingFlags.Instance | BindingFlags.Public)!.Invoke(provider, null)!;
    }

    private static Type SingleProviderType => GetFrobeniusType("SingleSolutionProvider");

    private static Type DummyProviderType => GetFrobeniusType("DummySolutionProvider");

    private static Type SolutionProviderType => GetFrobeniusType("SolutionProvider");

    private static Type GetFrobeniusType(string name)
    {
        return typeof(FrobeniusSolver).Assembly.GetType($"NRedberry.Solver.Frobenius.{name}", true)!;
    }
}
