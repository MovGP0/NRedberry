using System;

namespace Shouldly;

public static class StringComparisonShouldExtensions
{
    public static void ShouldContain(
        this string? actual,
        string expected,
        StringComparison comparison,
        string? customMessage = null)
    {
        actual.ShouldNotBeNull(customMessage);
        bool contains = actual.Contains(expected, comparison);
        contains.ShouldBeTrue(customMessage ?? BuildContainMessage(actual, expected, comparison));
    }

    public static void ShouldBeEqual(
        this string? actual,
        string? expected,
        StringComparison comparison,
        string? customMessage = null)
    {
        if (actual is null || expected is null)
        {
            actual.ShouldBe(expected, customMessage);
            return;
        }

        bool equals = string.Equals(actual, expected, comparison);
        equals.ShouldBeTrue(customMessage ?? BuildEqualityMessage(actual, expected, comparison));
    }

    private static string BuildContainMessage(string actual, string expected, StringComparison comparison)
    {
        return $"'{actual}' should contain '{expected}' using {comparison}.";
    }

    private static string BuildEqualityMessage(string actual, string expected, StringComparison comparison)
    {
        return $"'{actual}' should equal '{expected}' using {comparison}.";
    }
}
