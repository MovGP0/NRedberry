using NRedberry.Contexts;
using NRedberry.Tensors.Functions;
using RedberryContext = NRedberry.Contexts.Context;
using Xunit;

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
            Assert.Same(created, ContextManager.GetCurrentContext());

            ContextSettings settings = new(OutputFormat.LaTeX, "delta", "eta");
            RedberryContext configured = ContextManager.InitializeNew(settings);

            Assert.Same(configured, ContextManager.CurrentContext);
            Assert.Equal(OutputFormat.LaTeX, configured.DefaultOutputFormat);
            Assert.Equal("delta", configured.KroneckerName);
            Assert.Equal("eta", configured.MetricName);

            ContextManager.SetCurrentContext(original);
            Assert.Same(original, ContextManager.CurrentContext);
        }
        finally
        {
            ContextManager.SetCurrentContext(original);
        }
    }
}
