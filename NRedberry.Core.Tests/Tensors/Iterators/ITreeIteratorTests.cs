using NRedberry.Tensors.Iterators;
using TensorApi = NRedberry.Tensors.Tensors;

namespace NRedberry.Core.Tests.Tensors.Iterators;

public sealed class ITreeIteratorTests
{
    [Fact]
    public void ShouldCreateDirectionalIteratorsFromFactory()
    {
        ITreeIterator fromChildToParent = ITreeIterator.Factory.Create(TensorApi.Parse("a+b"), true, TraverseGuide.All);
        ITreeIterator fromParentToChild = ITreeIterator.Factory.Create(TensorApi.Parse("a+b"), false, TraverseGuide.All);

        fromChildToParent.ShouldBeOfType<FromChildToParentIterator>();
        fromParentToChild.ShouldBeOfType<FromParentToChildIterator>();
    }

    [Fact]
    public void ShouldValidateFactoryArguments()
    {
        Should.Throw<ArgumentNullException>(() => ITreeIterator.Factory.Create(null!, true, TraverseGuide.All));
        Should.Throw<ArgumentNullException>(() => ITreeIterator.Factory.Create(TensorApi.Parse("a"), true, null!));
    }
}
