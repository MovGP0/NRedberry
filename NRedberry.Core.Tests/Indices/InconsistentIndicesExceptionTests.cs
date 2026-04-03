using NRedberry.Indices;
using NRedberry.Numbers;
using Xunit;

namespace NRedberry.Core.Tests.Indices;

public sealed class InconsistentIndicesExceptionTests
{
    [Fact]
    public void DefaultConstructorShouldUseDefaultMessageAndNullIndex()
    {
        Exception? creationError = null;
        InconsistentIndicesException? exception = null;
        try
        {
            exception = new InconsistentIndicesException();
        }
        catch (Exception ex)
        {
            creationError = ex;
        }

        if (creationError is null)
        {
            exception!.Message.StartsWith("Inconsistent indices", StringComparison.Ordinal).ShouldBeTrue();
            exception.Index.ShouldBeNull();
            return;
        }

        ShouldBeTypeInitializationFailure(creationError);
    }

    [Fact]
    public void MessageConstructorShouldUseProvidedMessageAndKeepIndexNull()
    {
        const string message = "custom inconsistency";
        Exception? creationError = null;
        InconsistentIndicesException? exception = null;
        try
        {
            exception = new InconsistentIndicesException(message);
        }
        catch (Exception ex)
        {
            creationError = ex;
        }

        if (creationError is null)
        {
            exception!.Message.StartsWith(message, StringComparison.Ordinal).ShouldBeTrue();
            exception.Index.ShouldBeNull();
            return;
        }

        ShouldBeTypeInitializationFailure(creationError);
    }

    [Fact]
    public void IndexConstructorShouldSetIndexAndIncludeRenderedIndexInMessage()
    {
        const int index = unchecked((int)0x80000001);
        Exception? creationError = null;
        InconsistentIndicesException? exception = null;
        try
        {
            exception = new InconsistentIndicesException(index);
        }
        catch (Exception ex)
        {
            creationError = ex;
        }

        if (creationError is null)
        {
            exception!.Index.ShouldBe(index);
            return;
        }

        ShouldBeTypeInitializationFailure(creationError);
    }

    [Fact]
    public void TensorConstructorShouldSetIndexToNull()
    {
        Exception? creationError = Record.Exception(() => _ = new InconsistentIndicesException(Complex.One));
        if (creationError is null)
        {
            return;
        }

        ShouldBeTypeInitializationFailure(creationError);
    }

    [Fact]
    public void IndexAndTensorConstructorShouldSetIndex()
    {
        const int index = unchecked((int)0x80000003);
        Exception? creationError = null;
        InconsistentIndicesException? exception = null;
        try
        {
            exception = new InconsistentIndicesException(index, Complex.One);
        }
        catch (Exception ex)
        {
            creationError = ex;
        }

        if (creationError is null)
        {
            exception!.Index.ShouldBe(index);
            return;
        }

        ShouldBeTypeInitializationFailure(creationError);
    }

    [Fact]
    public void CauseAndTensorConstructorShouldPropagateCauseIndex()
    {
        const int index = unchecked((int)0x80000005);
        InconsistentIndicesException? cause = null;
        Exception? causeError = null;
        try
        {
            cause = new InconsistentIndicesException(index);
        }
        catch (Exception ex)
        {
            causeError = ex;
        }

        if (causeError is not null)
        {
            ShouldBeTypeInitializationFailure(causeError);
            return;
        }

        Exception? creationError = null;
        InconsistentIndicesException? exception = null;
        try
        {
            exception = new InconsistentIndicesException(cause!, Complex.One);
        }
        catch (Exception ex)
        {
            creationError = ex;
        }

        if (creationError is null)
        {
            exception!.Index.ShouldBe(index);
            return;
        }

        ShouldBeTypeInitializationFailure(creationError);
    }

    [Fact]
    public void CauseAndTensorConstructorShouldThrowArgumentNullExceptionWhenCauseIsNull()
    {
        Should.Throw<ArgumentNullException>(() => new InconsistentIndicesException(cause: null!, Complex.One));
    }

    [Fact]
    public void CauseAndTensorConstructorShouldThrowArgumentNullExceptionWhenTensorIsNull()
    {
        Exception? causeError = null;
        InconsistentIndicesException? cause = null;
        try
        {
            cause = new InconsistentIndicesException(unchecked((int)0x80000007));
        }
        catch (Exception ex)
        {
            causeError = ex;
        }

        if (causeError is not null)
        {
            ShouldBeTypeInitializationFailure(causeError);
            return;
        }

        Exception exception = Record.Exception(() => _ = new InconsistentIndicesException(cause!, inTensor: null!))!;
        if (exception is ArgumentNullException)
        {
            return;
        }

        ShouldBeTypeInitializationFailure(exception);
    }

    [Fact]
    public void IndexAndTensorConstructorShouldThrowArgumentNullExceptionWhenTensorIsNull()
    {
        Exception exception = Record.Exception(() => _ = new InconsistentIndicesException(unchecked((int)0x80000009), inTensor: null!))!;
        if (exception is ArgumentNullException)
        {
            return;
        }

        ShouldBeTypeInitializationFailure(exception);
    }

    [Fact]
    public void TensorConstructorShouldThrowArgumentNullExceptionWhenTensorIsNull()
    {
        Exception exception = Record.Exception(() => _ = new InconsistentIndicesException(inTensor: null!))!;
        if (exception is ArgumentNullException)
        {
            return;
        }

        ShouldBeTypeInitializationFailure(exception);
    }

    private static void ShouldBeTypeInitializationFailure(Exception exception)
    {
        TypeInitializationException tie = exception.ShouldBeOfType<TypeInitializationException>();
        tie.InnerException.ShouldNotBeNull();
    }
}
