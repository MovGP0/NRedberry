using System.Reflection;
using NRedberry.Solver.Frobenius;

namespace NRedberry.Core.Tests.Solver.Frobenius;

public sealed class FinalSolutionProviderTests
{
    [Fact]
    public void ShouldReturnNullWhenTakeCalledWithoutTick()
    {
        object dummyProvider = CreateDummySolutionProvider([1, 2], [6, 0]);
        object finalProvider = CreateFinalSolutionProvider(dummyProvider, 0, [3, 0]);

        int[]? solution = Take(finalProvider);

        solution.ShouldBeNull();
    }

    [Fact]
    public void ShouldReturnAugmentedSolutionWhenRemaindersMatchSingleCounter()
    {
        object dummyProvider = CreateDummySolutionProvider([2, 3, 4], [6, 0, 10]);
        object finalProvider = CreateFinalSolutionProvider(dummyProvider, 1, [3, 0, 5]);

        Tick(finalProvider).ShouldBeTrue();

        int[]? solution = Take(finalProvider);
        int[]? secondTake = Take(finalProvider);

        solution.ShouldBe(new[] { 2, 5, 4 });
        secondTake.ShouldBeNull();
    }

    [Fact]
    public void ShouldReturnNullWhenFirstNonZeroCoefficientDoesNotDivideRemainder()
    {
        object dummyProvider = CreateDummySolutionProvider([7, 8, 9], [0, 10, 6]);
        object finalProvider = CreateFinalSolutionProvider(dummyProvider, 2, [0, 4, 2]);

        Tick(finalProvider).ShouldBeTrue();

        int[]? solution = Take(finalProvider);

        solution.ShouldBeNull();
    }

    [Fact]
    public void ShouldReturnNullWhenZeroCoefficientHasNonZeroRemainder()
    {
        object dummyProvider = CreateDummySolutionProvider([4, 5, 6], [6, 1, 12]);
        object finalProvider = CreateFinalSolutionProvider(dummyProvider, 0, [2, 0, 4]);

        Tick(finalProvider).ShouldBeTrue();

        int[]? solution = Take(finalProvider);

        solution.ShouldBeNull();
    }

    [Fact]
    public void ShouldReturnNullWhenCountersDoNotMatchAcrossCoefficients()
    {
        object dummyProvider = CreateDummySolutionProvider([1, 1], [6, 12]);
        object finalProvider = CreateFinalSolutionProvider(dummyProvider, 1, [2, 3]);

        Tick(finalProvider).ShouldBeTrue();

        int[]? solution = Take(finalProvider);

        solution.ShouldBeNull();
    }

    private static object CreateDummySolutionProvider(int[] solution, int[] remainders)
    {
        Type dummyProviderType = GetFrobeniusType("DummySolutionProvider");
        ConstructorInfo? constructor = dummyProviderType.GetConstructor([typeof(int[]), typeof(int[])]);
        return constructor!.Invoke([solution, remainders]);
    }

    private static object CreateFinalSolutionProvider(object provider, int position, int[] coefficients)
    {
        Type finalProviderType = GetFrobeniusType("FinalSolutionProvider");
        ConstructorInfo? constructor = finalProviderType.GetConstructors(BindingFlags.Instance | BindingFlags.Public)[0];
        return constructor.Invoke([provider, (object)position, coefficients]);
    }

    private static bool Tick(object provider)
    {
        MethodInfo tick = provider.GetType().GetMethod("Tick", BindingFlags.Instance | BindingFlags.Public)!;
        return (bool)tick.Invoke(provider, [])!;
    }

    private static int[]? Take(object provider)
    {
        MethodInfo take = provider.GetType().GetMethod("Take", BindingFlags.Instance | BindingFlags.Public)!;
        return (int[]?)take.Invoke(provider, []);
    }

    private static Type GetFrobeniusType(string typeName)
    {
        Assembly assembly = typeof(FrobeniusSolver).Assembly;
        return assembly.GetType($"NRedberry.Solver.Frobenius.{typeName}", throwOnError: true)!;
    }
}
