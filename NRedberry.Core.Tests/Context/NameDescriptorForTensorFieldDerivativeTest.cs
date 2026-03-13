using NRedberry.Contexts;
using Xunit;

namespace NRedberry.Core.Tests.Context;

public sealed class NameDescriptorForTensorFieldDerivativeTest
{
    [Fact]
    public void ShouldConvertPermutation()
    {
        int[] permutation = [3, 2, 1, 0];
        int[] mapping = [1, 2, 4, 3];
        int[] expected = [0, 3, 4, 1, 2, 5, 6, 7, 8, 9];

        Assert.Equal(expected, NameDescriptorForTensorFieldDerivative.ConvertPermutation(permutation, mapping, 10));

        int[] reverseMapping = [-1, 0, 1, 3, 2, -1, -1, -1, -1, -1];
        Assert.Equal(permutation, NameDescriptorForTensorFieldDerivative.ConvertPermutation(expected, reverseMapping, 4));
    }
}
