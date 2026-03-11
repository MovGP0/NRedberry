using System.Reflection;
using NRedberry.Solver.Frobenius;
using Xunit;

namespace NRedberry.Core.Tests.Solver.Frobenius;

public sealed class SolutionProviderAbstractTests
{
    [Fact]
    public void ShouldThrowWhenProviderIsNull()
    {
        ConstructorInfo? constructor = SingleProviderType.GetConstructor([SolutionProviderType, typeof(int), typeof(int[])]);

        TargetInvocationException exception = Assert.Throws<TargetInvocationException>(() => constructor!.Invoke([null!, 0, new[] { 1 }]));
        ArgumentNullException innerException = Assert.IsType<ArgumentNullException>(exception.InnerException);
        Assert.Equal("provider", innerException.ParamName);
    }

    [Fact]
    public void ShouldThrowWhenCoefficientsAreNull()
    {
        object dummyProvider = CreateDummyProvider([1], [2]);
        ConstructorInfo? constructor = SingleProviderType.GetConstructor([SolutionProviderType, typeof(int), typeof(int[])]);

        TargetInvocationException exception = Assert.Throws<TargetInvocationException>(() => constructor!.Invoke([dummyProvider, 0, null!]));
        ArgumentNullException innerException = Assert.IsType<ArgumentNullException>(exception.InnerException);
        Assert.Equal("coefficients", innerException.ParamName);
    }

    [Fact]
    public void ShouldTickFalseWhenUnderlyingProviderHasNoMoreSolutions()
    {
        object provider = CreateSingleProvider([1], [3], 0, [1]);

        Assert.True(Tick(provider));
        Assert.Equal([1], Take(provider));
        Assert.False(Tick(provider));
    }

    [Fact]
    public void ShouldComputeCurrentRemaindersFromCounter()
    {
        object provider = CreateSingleProvider([3, 4], [5, 8], 1, [2, 3]);

        Assert.True(Tick(provider));
        Assert.Equal([3, 4], Take(provider));
        Assert.Equal([5, 8], CurrentRemainders(provider));
        Assert.Equal([3, 5], Take(provider));
        Assert.Equal([3, 5], CurrentRemainders(provider));
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
