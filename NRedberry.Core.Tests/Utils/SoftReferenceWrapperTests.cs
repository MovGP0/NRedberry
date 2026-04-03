using NRedberry.Core.Utils;

namespace NRedberry.Core.Tests.Utils;

public sealed class SoftReferenceWrapperTests
{
    [Fact]
    public void ShouldWrapAndResetReferent()
    {
        object value = new();
        SoftReferenceWrapper<object> wrapper = new(value);

        wrapper.GetReferent().ShouldBeSameAs(value);

        object replacement = new();
        wrapper.ResetReferent(replacement);

        wrapper.GetReferent().ShouldBeSameAs(replacement);
    }

    [Fact]
    public void ShouldAcceptDirectWeakReferenceReset()
    {
        object value = new();
        WeakReference<object> reference = new(value);
        SoftReferenceWrapper<object> wrapper = new();

        wrapper.ResetReference(reference);

        wrapper.GetReference().ShouldBeSameAs(reference);
        wrapper.GetReferent().ShouldBeSameAs(value);
    }
}
