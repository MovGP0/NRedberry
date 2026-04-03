using NRedberry.Core.Combinatorics;
using NRedberry.Groups;

namespace NRedberry.Core.Tests.Groups;

public sealed class BacktrackSearchPayloadTests
{
    [Fact(DisplayName = "Should set word reference on payload")]
    public void ShouldSetWordReferenceOnPayload()
    {
        // Arrange
        var payload = new CountingPayload();
        Permutation[] word = [NRedberry.Groups.Permutations.GetIdentityPermutation()];

        // Act
        payload.SetWordReference(word);

        // Assert
        payload.WordLength.ShouldBe(1);
    }

    [Fact(DisplayName = "Should delegate test function in default payload")]
    public void ShouldDelegateTestFunctionInDefaultPayload()
    {
        // Arrange
        var tester = new CountingTestFunction();
        var payload = BacktrackSearchPayload.CreateDefaultPayload(tester);

        // Act
        bool result = payload.Test(NRedberry.Groups.Permutations.GetIdentityPermutation(), 0);

        // Assert
        result.ShouldBeTrue();
        tester.Count.ShouldBe(1);
    }

    private sealed class CountingPayload : BacktrackSearchPayload
    {
        public int WordLength => WordReference.Length;

        public override void BeforeLevelIncrement(int level)
        {
        }

        public override void AfterLevelIncrement(int level)
        {
        }

        public override bool Test(Permutation permutation, int level)
        {
            return true;
        }
    }

    private sealed class CountingTestFunction : IBacktrackSearchTestFunction
    {
        public int Count { get; private set; }

        public bool Test(Permutation permutation, int level)
        {
            Count++;
            return true;
        }
    }
}
