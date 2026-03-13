using NRedberry.Core.Utils;
using Xunit;

namespace NRedberry.Core.Tests.Utils;

public sealed class SoftReferenceWrapperTests
{
    [Fact]
    public void ShouldWrapAndResetReferent()
    {
        object value = new();
        SoftReferenceWrapper<object> wrapper = new(value);

        Assert.Same(value, wrapper.GetReferent());

        object replacement = new();
        wrapper.ResetReferent(replacement);

        Assert.Same(replacement, wrapper.GetReferent());
    }

    [Fact]
    public void ShouldAcceptDirectWeakReferenceReset()
    {
        object value = new();
        WeakReference<object> reference = new(value);
        SoftReferenceWrapper<object> wrapper = new();

        wrapper.ResetReference(reference);

        Assert.Same(reference, wrapper.GetReference());
        Assert.Same(value, wrapper.GetReferent());
    }
}
