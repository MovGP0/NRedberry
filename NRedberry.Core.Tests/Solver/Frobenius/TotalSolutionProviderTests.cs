using System.Reflection;
using NRedberry.Solver.Frobenius;
using Xunit;

namespace NRedberry.Core.Tests.Solver.Frobenius;

public sealed class TotalSolutionProviderTests
{
    [Fact]
    public void ShouldThrowWhenProvidersAreNull()
    {
        ConstructorInfo? constructor = TotalProviderType.GetConstructor([SolutionProviderArrayType]);

        TargetInvocationException exception = Assert.Throws<TargetInvocationException>(() => constructor!.Invoke([null!]));
        ArgumentNullException innerException = Assert.IsType<ArgumentNullException>(exception.InnerException);
        Assert.Equal("providers", innerException.ParamName);
    }

    [Fact]
    public void ShouldEnumerateAllSolutionsAcrossProviders()
    {
        object dummyProvider = CreateDummyProvider([0, 0], [2]);
        object firstProvider = CreateSingleProvider(dummyProvider, 0, [1]);
        object finalProvider = CreateFinalProvider(firstProvider, 1, [1]);
        object totalProvider = CreateTotalProvider([firstProvider, finalProvider]);

        Assert.Equal([0, 2], Take(totalProvider));
        Assert.Equal([1, 1], Take(totalProvider));
        Assert.Equal([2, 0], Take(totalProvider));
        Assert.Null(Take(totalProvider));
    }

    [Fact]
    public void ShouldReturnNullWhenNoSolutionsExist()
    {
        object dummyProvider = CreateDummyProvider([0], [1]);
        object finalProvider = CreateFinalProvider(dummyProvider, 0, [2]);
        object totalProvider = CreateTotalProvider([finalProvider]);

        Assert.Null(Take(totalProvider));
    }

    private static object CreateDummyProvider(int[] solution, int[] remainders)
    {
        ConstructorInfo? constructor = DummyProviderType.GetConstructor([typeof(int[]), typeof(int[])]);
        return constructor!.Invoke([solution, remainders]);
    }

    private static object CreateSingleProvider(object provider, int position, int[] coefficients)
    {
        ConstructorInfo? constructor = SingleProviderType.GetConstructor([SolutionProviderType, typeof(int), typeof(int[])]);
        return constructor!.Invoke([provider, position, coefficients]);
    }

    private static object CreateFinalProvider(object provider, int position, int[] coefficients)
    {
        ConstructorInfo? constructor = FinalProviderType.GetConstructor([SolutionProviderType, typeof(int), typeof(int[])]);
        return constructor!.Invoke([provider, position, coefficients]);
    }

    private static object CreateTotalProvider(object[] providers)
    {
        ConstructorInfo? constructor = TotalProviderType.GetConstructor([SolutionProviderArrayType]);
        Array providerArray = Array.CreateInstance(SolutionProviderType, providers.Length);
        for (int i = 0; i < providers.Length; i++)
        {
            providerArray.SetValue(providers[i], i);
        }

        return constructor!.Invoke([providerArray]);
    }

    private static int[]? Take(object provider)
    {
        return (int[]?)provider.GetType().GetMethod("Take", BindingFlags.Instance | BindingFlags.Public)!.Invoke(provider, null);
    }

    private static Type DummyProviderType => GetFrobeniusType("DummySolutionProvider");

    private static Type SingleProviderType => GetFrobeniusType("SingleSolutionProvider");

    private static Type FinalProviderType => GetFrobeniusType("FinalSolutionProvider");

    private static Type TotalProviderType => GetFrobeniusType("TotalSolutionProvider");

    private static Type SolutionProviderType => GetFrobeniusType("SolutionProvider");

    private static Type SolutionProviderArrayType => SolutionProviderType.MakeArrayType();

    private static Type GetFrobeniusType(string name)
    {
        return typeof(FrobeniusSolver).Assembly.GetType($"NRedberry.Solver.Frobenius.{name}", true)!;
    }
}
