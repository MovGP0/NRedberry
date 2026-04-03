using NRedberry.Core.Combinatorics;
using NRedberry.Groups;
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
        int[] array = [1, 0];
        Permutation permutation = array.p;

        permutation.ShouldBe(Permutations.CreatePermutation(1, 0));
    }

    [Fact]
    public void ShouldCreateAntisymmetricPermutationFromNegativeOneLineNotationViaExtensionProperty()
    {
        int[] array = [-1, 0];
        Permutation permutation = array.p;

        permutation.ShouldBe(Permutations.CreatePermutation(true, 1, 0));
    }

    [Fact]
    public void ShouldCreatePermutationFromCycleNotationViaExtensionProperty()
    {
        int[][] array = [[1, 0], [2, 3]];
        Permutation permutation = array.p;

        permutation.ShouldBe(Permutations.CreatePermutation([[1, 0], [2, 3]]));
    }
}
