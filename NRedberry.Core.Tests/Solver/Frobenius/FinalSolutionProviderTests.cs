using System.Reflection;
using NRedberry.Solver.Frobenius;
using Xunit;

namespace NRedberry.Core.Tests.Solver.Frobenius;

public sealed class FinalSolutionProviderTests
{
    [Fact]
    public void ShouldReturnNullWhenTakeCalledWithoutTick()
    {
        object dummyProvider = CreateDummySolutionProvider(new[] { 1, 2 }, new[] { 6, 0 });
        object finalProvider = CreateFinalSolutionProvider(dummyProvider, 0, new[] { 3, 0 });

        int[]? solution = Take(finalProvider);

        Assert.Null(solution);
    }

    [Fact]
    public void ShouldReturnAugmentedSolutionWhenRemaindersMatchSingleCounter()
    {
        object dummyProvider = CreateDummySolutionProvider(new[] { 2, 3, 4 }, new[] { 6, 0, 10 });
        object finalProvider = CreateFinalSolutionProvider(dummyProvider, 1, new[] { 3, 0, 5 });

        Assert.True(Tick(finalProvider));

        int[]? solution = Take(finalProvider);
        int[]? secondTake = Take(finalProvider);

        Assert.Equal(new[] { 2, 5, 4 }, solution);
        Assert.Null(secondTake);
    }

    [Fact]
    public void ShouldReturnNullWhenFirstNonZeroCoefficientDoesNotDivideRemainder()
    {
        object dummyProvider = CreateDummySolutionProvider(new[] { 7, 8, 9 }, new[] { 0, 10, 6 });
        object finalProvider = CreateFinalSolutionProvider(dummyProvider, 2, new[] { 0, 4, 2 });

        Assert.True(Tick(finalProvider));

        int[]? solution = Take(finalProvider);

        Assert.Null(solution);
    }

    [Fact]
    public void ShouldReturnNullWhenZeroCoefficientHasNonZeroRemainder()
    {
        object dummyProvider = CreateDummySolutionProvider(new[] { 4, 5, 6 }, new[] { 6, 1, 12 });
        object finalProvider = CreateFinalSolutionProvider(dummyProvider, 0, new[] { 2, 0, 4 });

        Assert.True(Tick(finalProvider));

        int[]? solution = Take(finalProvider);

        Assert.Null(solution);
    }

    [Fact]
    public void ShouldReturnNullWhenCountersDoNotMatchAcrossCoefficients()
    {
        object dummyProvider = CreateDummySolutionProvider(new[] { 1, 1 }, new[] { 6, 12 });
        object finalProvider = CreateFinalSolutionProvider(dummyProvider, 1, new[] { 2, 3 });

        Assert.True(Tick(finalProvider));

        int[]? solution = Take(finalProvider);

        Assert.Null(solution);
    }

    private static object CreateDummySolutionProvider(int[] solution, int[] remainders)
    {
        Type dummyProviderType = GetFrobeniusType("DummySolutionProvider");
        ConstructorInfo? constructor = dummyProviderType.GetConstructor(new[] { typeof(int[]), typeof(int[]) });
        return constructor!.Invoke(new object[] { solution, remainders });
    }

    private static object CreateFinalSolutionProvider(object provider, int position, int[] coefficients)
    {
        Type finalProviderType = GetFrobeniusType("FinalSolutionProvider");
        ConstructorInfo? constructor = finalProviderType.GetConstructors(BindingFlags.Instance | BindingFlags.Public)[0];
        return constructor.Invoke(new[] { provider, (object)position, coefficients });
    }

    private static bool Tick(object provider)
    {
        MethodInfo tick = provider.GetType().GetMethod("Tick", BindingFlags.Instance | BindingFlags.Public)!;
        return (bool)tick.Invoke(provider, Array.Empty<object>())!;
    }

    private static int[]? Take(object provider)
    {
        MethodInfo take = provider.GetType().GetMethod("Take", BindingFlags.Instance | BindingFlags.Public)!;
        return (int[]?)take.Invoke(provider, Array.Empty<object>());
    }

    private static Type GetFrobeniusType(string typeName)
    {
        Assembly assembly = typeof(FrobeniusSolver).Assembly;
        return assembly.GetType($"NRedberry.Solver.Frobenius.{typeName}", throwOnError: true)!;
    }
}
