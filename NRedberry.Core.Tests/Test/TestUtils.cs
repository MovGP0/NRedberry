using Xunit;

namespace NRedberry.Core.Tests.Test;

public static class TestUtils
{
    public static bool DoLongTests()
    {
        return IsPropertyEnabled("longTests")
            || IsPropertyEnabled("longTest");
    }

    public static bool DoPerformanceTests()
    {
        return IsPropertyEnabled("testPerformance")
            || IsPropertyEnabled("performanceTest")
            || IsPropertyEnabled("performanceTests");
    }

    public static bool SkipGapTests()
    {
        return IsPropertyEnabled("noGAP")
            || IsPropertyEnabled("noGap");
    }

    public static int Its(int shortTest, int longTest)
    {
        return DoLongTests() ? longTest : shortTest;
    }

    public static long Its(long shortTest, long longTest)
    {
        return DoLongTests() ? longTest : shortTest;
    }

    private static bool IsPropertyEnabled(string propertyName)
    {
        string? value = Environment.GetEnvironmentVariable(propertyName);
        return value is "" || string.Equals(value, "true", StringComparison.Ordinal);
    }
}

public sealed class TestUtilsTests
{
    [Fact]
    public void ShouldEnableLongTestsFromEnvironment()
    {
        using EnvironmentVariableScope _ = new("longTest", "true");

        TestUtils.DoLongTests().ShouldBeTrue();
        TestUtils.Its(1, 2).ShouldBe(2);
        TestUtils.Its(3L, 4L).ShouldBe(4L);
    }

    [Fact]
    public void ShouldEnablePerformanceTestsFromEnvironment()
    {
        using EnvironmentVariableScope _ = new("testPerformance", "true");

        TestUtils.DoPerformanceTests().ShouldBeTrue();
    }

    [Fact]
    public void ShouldEnableGapSkipFromEnvironment()
    {
        using EnvironmentVariableScope _ = new("noGap", "true");

        TestUtils.SkipGapTests().ShouldBeTrue();
    }
}

internal sealed class EnvironmentVariableScope : IDisposable
{
    private readonly string _name;
    private readonly string? _originalValue;

    public EnvironmentVariableScope(string name, string? value)
    {
        _name = name;
        _originalValue = Environment.GetEnvironmentVariable(name);
        Environment.SetEnvironmentVariable(name, value);
    }

    public void Dispose()
    {
        Environment.SetEnvironmentVariable(_name, _originalValue);
    }
}
