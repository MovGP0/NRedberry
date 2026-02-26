using NRedberry.Contexts;
using Shouldly;
using Xunit;

namespace NRedberry.Core.Contexts.Tests;

public sealed class IContextListenerTests
{
    [Fact(DisplayName = "Should receive context events")]
    public void ShouldReceiveContextEvents()
    {
        var listener = new TestContextListener();

        listener.OnNext(new ContextEvent());
        listener.OnCompleted();

        listener.ReceivedEvents.ShouldBe(1);
        listener.Completed.ShouldBeTrue();
        listener.Error.ShouldBeNull();
    }

    [Fact(DisplayName = "Should capture errors")]
    public void ShouldCaptureErrors()
    {
        var listener = new TestContextListener();
        var exception = new InvalidOperationException("boom");

        listener.OnError(exception);

        listener.Error.ShouldBe(exception);
    }

    private sealed class TestContextListener : IContextListener
    {
        public int ReceivedEvents { get; private set; }

        public bool Completed { get; private set; }

        public Exception? Error { get; private set; }

        public void OnCompleted()
        {
            Completed = true;
        }

        public void OnError(Exception error)
        {
            Error = error;
        }

        public void OnNext(ContextEvent value)
        {
            ReceivedEvents++;
        }
    }
}
