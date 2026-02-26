using NRedberry.Contexts;
using Shouldly;
using Xunit;

namespace NRedberry.Core.Contexts.Tests;

public sealed class CCTests
{
    [Fact(DisplayName = "Should expose current context")]
    public void ShouldExposeCurrentContext()
    {
        Context current = CC.Current;

        current.ShouldBeSameAs(Context.Get());
    }

    [Fact(DisplayName = "Should delegate default output format")]
    public void ShouldDelegateDefaultOutputFormat()
    {
        OutputFormat original = CC.DefaultOutputFormat;
        try
        {
            CC.DefaultOutputFormat = OutputFormat.Redberry;

            CC.DefaultOutputFormat.ShouldBe(OutputFormat.Redberry);
            Context.Get().DefaultOutputFormat.ShouldBe(OutputFormat.Redberry);
        }
        finally
        {
            CC.DefaultOutputFormat = original;
        }
    }

    [Fact(DisplayName = "Should expose managers")]
    public void ShouldExposeManagers()
    {
        CC.NameManager.ShouldNotBeNull();
        CC.IndexConverterManager.ShouldNotBeNull();
    }

    [Fact(DisplayName = "Should delegate IsMetric")]
    public void ShouldDelegateIsMetric()
    {
        const byte type = 0;

        CC.IsMetric(type).ShouldBe(Context.Get().IsMetric(type));
    }

    [Fact(DisplayName = "Should allow tensor name resets")]
    public void ShouldAllowTensorNameResets()
    {
        CC.ResetTensorNames();
        CC.ResetTensorNames(123);
    }
}
