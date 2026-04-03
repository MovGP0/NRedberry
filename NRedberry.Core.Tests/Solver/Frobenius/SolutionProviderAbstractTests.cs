using System.Reflection;
using NRedberry.Solver.Frobenius;

namespace NRedberry.Core.Tests.Solver.Frobenius;

public sealed class SolutionProviderAbstractTests
{
    [Fact]
    public void ShouldThrowWhenProviderIsNull()
    {
        ConstructorInfo? constructor = SingleProviderType.GetConstructor([SolutionProviderType, typeof(int), typeof(int[])]);

        TargetInvocationException exception = Should.Throw<TargetInvocationException>(() => constructor!.Invoke([null!, 0, new[] { 1 }]));
        ArgumentNullException innerException = exception.InnerException.ShouldBeOfType<ArgumentNullException>();
        innerException.ParamName.ShouldBe("provider");
    }

    [Fact]
    public void ShouldThrowWhenCoefficientsAreNull()
    {
        object dummyProvider = CreateDummyProvider([1], [2]);
        ConstructorInfo? constructor = SingleProviderType.GetConstructor([SolutionProviderType, typeof(int), typeof(int[])]);

        TargetInvocationException exception = Should.Throw<TargetInvocationException>(() => constructor!.Invoke([dummyProvider, 0, null!]));
        ArgumentNullException innerException = exception.InnerException.ShouldBeOfType<ArgumentNullException>();
        innerException.ParamName.ShouldBe("coefficients");
    }

    [Fact]
    public void ShouldTickFalseWhenUnderlyingProviderHasNoMoreSolutions()
    {
        object provider = CreateSingleProvider([1], [3], 0, [1]);

        Tick(provider).ShouldBeTrue();
        Take(provider).ShouldBe([1]);
        Tick(provider).ShouldBeFalse();
    }

    [Fact]
    public void ShouldComputeCurrentRemaindersFromCounter()
    {
        object provider = CreateSingleProvider([3, 4], [5, 8], 1, [2, 3]);

        Tick(provider).ShouldBeTrue();
        Take(provider).ShouldBe([3, 4]);
        CurrentRemainders(provider).ShouldBe([5, 8]);
        Take(provider).ShouldBe([3, 5]);
        CurrentRemainders(provider).ShouldBe([3, 5]);
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
