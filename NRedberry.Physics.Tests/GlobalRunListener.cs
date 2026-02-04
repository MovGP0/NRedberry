using NRedberry.Tensors;

namespace NRedberry.Physics.Tests;

public sealed class GlobalRunListener
{
    public void TestStarted(string? description = null)
    {
        CC.ResetTensorNames();
    }
}
