using System.Reflection;
using System.Runtime.CompilerServices;
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
