using Shouldly;
using Xunit;

namespace NRedberry.Core.Combinatorics.Tests;

public sealed class CombinatorialContractsTests
{
    [Fact]
    public void ShouldExposeGeneratorEnumeratorContract()
    {
        IIntCombinatorialGenerator generator = new IntCombinationPermutationGenerator(3, 2);

        generator.GetEnumerator().ShouldBeSameAs(generator);
        generator.MoveNext().ShouldBeTrue();
        generator.Current.ShouldBe([0, 1]);
    }

    [Fact]
    public void ShouldExposePortTakeAndResetContract()
    {
        IIntCombinatorialPort port = new IntCombinationPermutationGenerator(3, 2);

        int[]? firstReference = port.Take();
        int[]? secondReference = port.Take();
        port.Reset();
        int[]? resetFirst = port.Take();

        firstReference.ShouldNotBeNull();
        secondReference.ShouldNotBeNull();

        int[] first = (int[])firstReference.Clone();
        resetFirst.ShouldNotBeNull();

        first.ShouldBe([0, 1]);
        resetFirst.ShouldBe([0, 1]);
        port.GetReference().ShouldBeSameAs(secondReference);
        port.GetReference().ShouldBeSameAs(resetFirst);
    }
}

public sealed class InconsistentGeneratorsExceptionTests
{
    [Fact]
    public void ShouldExposeStandardExceptionConstructors()
    {
        InvalidOperationException inner = new("inner");
        InconsistentGeneratorsException withoutMessage = new();
        InconsistentGeneratorsException withMessage = new("broken");
        InconsistentGeneratorsException withInner = new("outer", inner);

        withoutMessage.Message.ShouldContain(nameof(InconsistentGeneratorsException));
        withMessage.Message.ShouldBe("broken");
        withInner.InnerException.ShouldBeSameAs(inner);
        withInner.Message.ShouldBe("outer");
    }
}
