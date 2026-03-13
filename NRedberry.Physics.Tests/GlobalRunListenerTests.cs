using Xunit;

namespace NRedberry.Physics.Tests;

public sealed class GlobalRunListenerTests
{
    [Fact]
    public void ShouldResetTensorNamesWithoutThrowing()
    {
        GlobalRunListener listener = new();
        listener.TestStarted("physics");
    }
}
