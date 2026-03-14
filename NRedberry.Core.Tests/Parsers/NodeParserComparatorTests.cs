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

        Assert.Same(first, second);
    }

    [Fact]
    public void ShouldReturnZeroWhenComparingSameReference()
    {
        var comparator = GetComparator();
        var parser = new StubTokenParser(5);

        var result = comparator.Compare(parser, parser);

        Assert.Equal(0, result);
    }

    [Fact]
    public void ShouldTreatNullAsLowerPrecedenceThanParser()
    {
        var comparator = GetComparator();
        var parser = new StubTokenParser(5);

        var nullLeftResult = comparator.Compare(null, parser);
        var nullRightResult = comparator.Compare(parser, null);

        Assert.Equal(1, nullLeftResult);
        Assert.Equal(-1, nullRightResult);
    }

    [Fact]
    public void ShouldOrderByPriorityDescending()
    {
        var comparator = GetComparator();
        var higherPriority = new StubTokenParser(10);
        var lowerPriority = new StubTokenParser(1);

        var result = comparator.Compare(higherPriority, lowerPriority);

        Assert.True(result < 0);
    }

    [Fact]
    public void ShouldReturnZeroForEqualPriorities()
    {
        var comparator = GetComparator();
        var left = new StubTokenParser(7);
        var right = new StubTokenParser(7);

        var result = comparator.Compare(left, right);

        Assert.Equal(0, result);
    }

    private static IComparer<ITokenParser> GetComparator()
    {
        var comparatorType = typeof(ITokenParser).Assembly.GetType("NRedberry.Parsers.NodeParserComparator");
        Assert.NotNull(comparatorType);

        var instanceProperty = comparatorType!.GetProperty("Instance", BindingFlags.Public | BindingFlags.Static);
        Assert.NotNull(instanceProperty);

        var instance = instanceProperty!.GetValue(null);
        Assert.NotNull(instance);

        return Assert.IsAssignableFrom<IComparer<ITokenParser>>(instance);
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
