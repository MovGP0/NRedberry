using System.Reflection;
using NRedberry.Solver.Frobenius;
using Xunit;

namespace NRedberry.Core.Tests.Solver.Frobenius;

public sealed class SingleSolutionProviderTests
{
    [Fact]
    public void ShouldReturnNullWhenTakeCalledWithoutTick()
    {
        object provider = CreateSingleProvider([4], [5], 0, [2]);

        int[]? solution = Take(provider);

        Assert.Null(solution);
    }

    [Fact]
    public void ShouldEnumerateSolutionsUntilRemainderBecomesNegative()
    {
        object provider = CreateSingleProvider([7], [5], 0, [2]);

        Assert.True(Tick(provider));
        Assert.Equal([7], Take(provider));
        Assert.Equal([8], Take(provider));
        Assert.Equal([9], Take(provider));
        Assert.Null(Take(provider));
        Assert.False(Tick(provider));
    }

    [Fact]
    public void ShouldResetCurrentRemaindersAfterFailedTake()
    {
        object provider = CreateSingleProvider([3, 4], [5, 8], 1, [2, 3]);

        Assert.True(Tick(provider));
        Assert.Equal([3, 4], Take(provider));
        Assert.Equal([5, 8], CurrentRemainders(provider));
        Assert.Equal([3, 5], Take(provider));
        Assert.Equal([3, 5], CurrentRemainders(provider));
        Assert.Equal([3, 6], Take(provider));
        Assert.Equal([1, 2], CurrentRemainders(provider));
        Assert.Null(Take(provider));
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
