using NRedberry.Contexts;
using NRedberry.Tensors.Functions;
using RedberryContext = NRedberry.Contexts.Context;

namespace NRedberry.Core.Tests.Tensors;

public sealed class ContextManagerTests
{
    [Fact]
    public void ShouldInitializeAndReplaceCurrentContext()
    {
        RedberryContext original = ContextManager.CurrentContext;

        try
        {
            RedberryContext created = ContextManager.InitializeNew();
            ContextManager.GetCurrentContext().ShouldBeSameAs(created);

            ContextSettings settings = new(OutputFormat.LaTeX, "delta", "eta");
            RedberryContext configured = ContextManager.InitializeNew(settings);

            ContextManager.CurrentContext.ShouldBeSameAs(configured);
            configured.DefaultOutputFormat.ShouldBe(OutputFormat.LaTeX);
            configured.KroneckerName.ShouldBe("delta");
            configured.MetricName.ShouldBe("eta");

            ContextManager.SetCurrentContext(original);
            ContextManager.CurrentContext.ShouldBeSameAs(original);
        }
        finally
        {
            ContextManager.SetCurrentContext(original);
        }
    }
}
