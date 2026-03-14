using System.Collections.Generic;
using System.Reflection;
using NRedberry.Parsers;
using RedberryParser = NRedberry.Parsers.Parser;
using Xunit;

namespace NRedberry.Core.Tests.Parsers;

public sealed class NodeParserComparatorTests
{
    [Fact]
    public void ShouldExposeSingletonInstance()
    {
        var first = GetComparator();
        var second = GetComparator();

        second.ShouldBeSameAs(first);
    }

    [Fact]
    public void ShouldReturnZeroWhenComparingSameReference()
    {
        var comparator = GetComparator();
        var parser = new StubTokenParser(5);

        var result = comparator.Compare(parser, parser);

        result.ShouldBe(0);
    }

    [Fact]
    public void ShouldTreatNullAsLowerPrecedenceThanParser()
    {
        var comparator = GetComparator();
        var parser = new StubTokenParser(5);

        var nullLeftResult = comparator.Compare(null, parser);
        var nullRightResult = comparator.Compare(parser, null);

        nullLeftResult.ShouldBe(1);
        nullRightResult.ShouldBe(-1);
    }

    [Fact]
    public void ShouldOrderByPriorityDescending()
    {
        var comparator = GetComparator();
        var higherPriority = new StubTokenParser(10);
        var lowerPriority = new StubTokenParser(1);

        var result = comparator.Compare(higherPriority, lowerPriority);

        result < 0.ShouldBeTrue();
    }

    [Fact]
    public void ShouldReturnZeroForEqualPriorities()
    {
        var comparator = GetComparator();
        var left = new StubTokenParser(7);
        var right = new StubTokenParser(7);

        var result = comparator.Compare(left, right);

        result.ShouldBe(0);
    }

    private static IComparer<ITokenParser> GetComparator()
    {
        var comparatorType = typeof(ITokenParser).Assembly.GetType("NRedberry.Parsers.NodeParserComparator");
        comparatorType.ShouldNotBeNull();

        var instanceProperty = comparatorType!.GetProperty("Instance", BindingFlags.Public | BindingFlags.Static);
        instanceProperty.ShouldNotBeNull();

        var instance = instanceProperty!.GetValue(null);
        instance.ShouldNotBeNull();

        return instance.ShouldBeAssignableTo<IComparer<ITokenParser>>();
    }
}

file sealed class StubTokenParser(int priority) : ITokenParser
{
    public int Priority { get; } = priority;

    public ParseToken? ParseToken(string expression, RedberryParser parser)
    {
        return null;
    }
}
