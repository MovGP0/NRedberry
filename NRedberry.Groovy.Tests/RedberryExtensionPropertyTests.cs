using NRedberry.Core.Combinatorics;
using NRedberry.Groups;
using Shouldly;
using Xunit;
using NumberComplex = NRedberry.Numbers.Complex;
using TensorApi = NRedberry.Tensors.Tensors;

namespace NRedberry.Groovy.Tests;

public sealed class RedberryExtensionPropertyTests
{
    [Fact]
    public void ShouldParseTensorFromStringViaExtensionProperty()
    {
        "a+b".t.ToString().ShouldBe(TensorApi.Parse("a+b").ToString());
    }

    [Fact]
    public void ShouldConvertNumbersViaExtensionProperty()
    {
        7.t.ShouldBeOfType<NumberComplex>();
        7.t.ToString().ShouldBe(new NumberComplex(7).ToString());
    }

    [Fact]
    public void ShouldCreatePermutationFromOneLineNotationViaExtensionProperty()
    {
        Permutation permutation = new[] { 1, 0 }.p;

        permutation.ShouldBe(Permutations.CreatePermutation(1, 0));
    }

    [Fact]
    public void ShouldCreateAntisymmetricPermutationFromNegativeOneLineNotationViaExtensionProperty()
    {
        Permutation permutation = new[] { -1, 0 }.p;

        permutation.ShouldBe(Permutations.CreatePermutation(true, 1, 0));
    }

    [Fact]
    public void ShouldCreatePermutationFromCycleNotationViaExtensionProperty()
    {
        Permutation permutation = new[]
        {
            new[] { 1, 0 },
            new[] { 2, 3 },
        }.p;

        permutation.ShouldBe(Permutations.CreatePermutation([[1, 0], [2, 3]]));
    }
}
