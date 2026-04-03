using NRedberry.Core.Combinatorics.Extensions;
using Xunit;

namespace NRedberry.Core.Combinatorics.Tests.Extensions;

public sealed class EnumeratorExtensionsTests
{
    [Fact]
    public void ShouldWrapSingleElementAsEnumerator()
    {
        IEnumerator<int> enumerator = 7.GetEnumerator();

        enumerator.MoveNext().ShouldBeTrue();
        enumerator.Current.ShouldBe(7);
        enumerator.MoveNext().ShouldBeFalse();
    }
}
