using NRedberry;
using NRedberry.Contexts;
using NRedberry.Core.Exceptions;
using Shouldly;
using Xunit;

namespace NRedberry.Core.Contexts.Tests;

public sealed class IndexConverterManagerTests
{
    [Fact(DisplayName = "Should get symbol from matching converter")]
    public void ShouldGetSymbolFromMatchingConverter()
    {
        var converter = new TestConverter(2, "x");
        IndexConverterManager manager = new([converter]);
        long code = ((long)converter.Type << 24) | 12;

        string symbol = manager.GetSymbol(code, OutputFormat.Redberry);

        symbol.ShouldBe("x12");
    }

    [Fact(DisplayName = "Should get code from matching converter")]
    public void ShouldGetCodeFromMatchingConverter()
    {
        var converter = new TestConverter(3, "y");
        IndexConverterManager manager = new([converter]);

        int code = manager.GetCode("y7");

        code.ShouldBe(((converter.Type & 0x7F) << 24) | 7);
    }

    [Fact(DisplayName = "Should throw on duplicate converter types")]
    public void ShouldThrowOnDuplicateConverterTypes()
    {
        var first = new TestConverter(1, "a");
        var second = new TestConverter(1, "b");

        Should.Throw<ArgumentException>(() => new IndexConverterManager([first, second]));
    }

    [Fact(DisplayName = "Should throw when no converter applies")]
    public void ShouldThrowWhenNoConverterApplies()
    {
        var converter = new TestConverter(5, "z");
        IndexConverterManager manager = new([converter]);

        Should.Throw<ArgumentException>(() => manager.GetCode("other1"));
    }

    [Fact(DisplayName = "Should wrap IndexConverterException in GetSymbol")]
    public void ShouldWrapIndexConverterExceptionInGetSymbol()
    {
        var converter = new ThrowingConverter(6);
        IndexConverterManager manager = new([converter]);
        long code = ((long)converter.Type << 24) | 1;

        Should.Throw<ArgumentException>(() => manager.GetSymbol(code, OutputFormat.Redberry));
    }

    private sealed class TestConverter(byte type, string prefix) : IIndexSymbolConverter
    {
        public bool ApplicableToSymbol(string symbol)
        {
            return symbol.StartsWith(prefix, StringComparison.Ordinal);
        }

        public string GetSymbol(int code, OutputFormat outputFormat)
        {
            return $"{prefix}{code}";
        }

        public int GetCode(string symbol)
        {
            string suffix = symbol[prefix.Length..];
            return int.Parse(suffix);
        }

        public int MaxNumberOfSymbols => 1000;

        public byte Type { get; } = type;
    }

    private sealed class ThrowingConverter(byte type) : IIndexSymbolConverter
    {
        public bool ApplicableToSymbol(string symbol)
        {
            return true;
        }

        public string GetSymbol(int code, OutputFormat outputFormat)
        {
            throw new IndexConverterException("boom");
        }

        public int GetCode(string symbol)
        {
            throw new IndexConverterException("boom");
        }

        public int MaxNumberOfSymbols => 0;

        public byte Type { get; } = type;
    }
}
