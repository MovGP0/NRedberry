using NRedberry.Tensors.Iterators;
using TensorApi = NRedberry.Tensors.Tensors;
using Xunit;

namespace NRedberry.Core.Tests.Tensors.Iterators;

public sealed class ITreeIteratorTests
{
    [Fact]
    public void ShouldCreateDirectionalIteratorsFromFactory()
    {
        ITreeIterator fromChildToParent = ITreeIterator.Factory.Create(TensorApi.Parse("a+b"), true, TraverseGuide.All);
        ITreeIterator fromParentToChild = ITreeIterator.Factory.Create(TensorApi.Parse("a+b"), false, TraverseGuide.All);

        Assert.IsType<FromChildToParentIterator>(fromChildToParent);
        Assert.IsType<FromParentToChildIterator>(fromParentToChild);
    }

    [Fact]
    public void ShouldValidateFactoryArguments()
    {
        Assert.Throws<ArgumentNullException>(() => ITreeIterator.Factory.Create(null!, true, TraverseGuide.All));
        Assert.Throws<ArgumentNullException>(() => ITreeIterator.Factory.Create(TensorApi.Parse("a"), true, null!));
    }
}
