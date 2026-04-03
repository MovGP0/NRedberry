using NRedberry.Core.Combinatorics.Symmetries;
using Xunit;

namespace NRedberry.Core.Combinatorics.Tests.Symmetries;

public sealed class EmptySymmetriesTests
{
    [Fact]
    public void ShouldBeEmptyForZeroDimension()
    {
        EmptySymmetries zero = new(0);

        zero.IsEmpty.ShouldBeTrue();
    }

    [Fact]
    public void ShouldBeEmptyForOneDimension()
    {
        EmptySymmetries one = new(1);

        one.IsEmpty.ShouldBeTrue();
    }

    [Fact]
    public void ShouldRejectDimensionsGreaterThanOne()
    {
        Should.Throw<ArgumentException>(() => new EmptySymmetries(2));
    }
}
