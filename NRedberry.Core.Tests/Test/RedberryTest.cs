using System.Reflection;
using System.Runtime.CompilerServices;
using Xunit;
using Xunit.Sdk;

namespace NRedberry.Core.Tests.Test;

public abstract class RedberryTest
{
    protected void AssumeTestIsEnabled([CallerMemberName] string? methodName = null)
    {
        if (methodName is null)
        {
            return;
        }

        MethodInfo? method = GetType().GetMethod(methodName);
        if (method is null)
        {
            return;
        }

        if (method.IsDefined(typeof(LongTestAttribute), inherit: true) && !TestUtils.DoLongTests())
        {
            throw SkipException.ForSkip("Long tests are disabled.");
        }

        if (method.IsDefined(typeof(PerformanceTestAttribute), inherit: true) && !TestUtils.DoPerformanceTests())
        {
            throw SkipException.ForSkip("Performance tests are disabled.");
        }
    }
}

public sealed class RedberryTestTests
{
    [Fact]
    public void ShouldNotSkipUnmarkedMethods()
    {
        RedberryTestProbe probe = new();

        probe.PlainMethod();
    }

    [Fact]
    public void ShouldSkipLongMarkedMethodsWhenDisabled()
    {
        using EnvironmentVariableScope _ = new("longTest", null);
        RedberryTestProbe probe = new();

        Assert.Throws<SkipException>(() => probe.LongMarkedMethod());
    }

    [Fact]
    public void ShouldAllowLongMarkedMethodsWhenEnabled()
    {
        using EnvironmentVariableScope _ = new("longTest", "true");
        RedberryTestProbe probe = new();

        probe.LongMarkedMethod();
    }

    [Fact]
    public void ShouldSkipPerformanceMarkedMethodsWhenDisabled()
    {
        using EnvironmentVariableScope _ = new("testPerformance", null);
        RedberryTestProbe probe = new();

        Assert.Throws<SkipException>(() => probe.PerformanceMarkedMethod());
    }

    [Fact]
    public void ShouldAllowPerformanceMarkedMethodsWhenEnabled()
    {
        using EnvironmentVariableScope _ = new("testPerformance", "true");
        RedberryTestProbe probe = new();

        probe.PerformanceMarkedMethod();
    }
}

internal sealed class RedberryTestProbe : RedberryTest
{
    public void PlainMethod()
    {
        AssumeTestIsEnabled();
    }

    [LongTest]
    public void LongMarkedMethod()
    {
        AssumeTestIsEnabled();
    }

    [PerformanceTest]
    public void PerformanceMarkedMethod()
    {
        AssumeTestIsEnabled();
    }
}
