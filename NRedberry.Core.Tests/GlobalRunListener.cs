using Xunit.Sdk;

namespace NRedberry.Core.Tests;

/// <summary>
/// Skeleton port of cc.redberry.core.GlobalRunListener.
/// </summary>
public sealed class GlobalRunListener
{
    public GlobalRunListener()
    {
        throw SkipException.ForSkip("Pending port from Java.");
    }

    public void TestStarted(object description)
    {
        throw SkipException.ForSkip("Pending port from Java.");
    }

    public void TestFailure(object failure)
    {
        throw SkipException.ForSkip("Pending port from Java.");
    }
}
