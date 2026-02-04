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
        return IsPropertyEnabled("performanceTests")
            || IsPropertyEnabled("longTest");
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
    public void ShouldReportLongTestMode()
    {
        if (TestUtils.DoLongTests())
        {
            Console.WriteLine("Long tests.");
        }
        else
        {
            Console.WriteLine("Short tests.");
        }
    }
}
