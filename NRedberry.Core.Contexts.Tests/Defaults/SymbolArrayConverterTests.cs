using NRedberry.Contexts.Defaults;
using NRedberry.Core.Exceptions;
using Shouldly;
using Xunit;

namespace NRedberry.Core.Contexts.Tests.Defaults;

public sealed class SymbolArrayConverterTests
{
    [Fact(DisplayName = "Should map symbols and codes")]
    public void ShouldMapSymbolsAndCodes()
    {
        // Arrange
        var converter = new TestSymbolArrayConverter(["a", "b"], ["α", "β"]);

        // Assert
        converter.ApplicableToSymbol("a").ShouldBeTrue();
        converter.ApplicableToSymbol("c").ShouldBeFalse();
        converter.GetCode("b").ShouldBe(1);
        converter.GetSymbol(1, OutputFormat.UTF8).ShouldBe("β");
        converter.GetSymbol(1, OutputFormat.LaTeX).ShouldBe("b");
        converter.GetSymbol(1, OutputFormat.Redberry).ShouldBe("\\b");
        converter.MaxNumberOfSymbols.ShouldBe(1);
    }

    [Fact(DisplayName = "Should throw for invalid symbol codes")]
    public void ShouldThrowForInvalidSymbolCodes()
    {
        // Arrange
        var converter = new TestSymbolArrayConverter(["a"], ["α"]);

        // Act + Assert
        Should.Throw<IndexConverterException>(() => converter.GetSymbol(2, OutputFormat.Redberry));
    }

    [Fact(DisplayName = "Should throw when symbols and UTF arrays mismatch")]
    public void ShouldThrowWhenSymbolsAndUtfArraysMismatch()
    {
        // Act + Assert
        Should.Throw<ApplicationException>(() => _ = new TestSymbolArrayConverter(["a"], ["α", "β"]));
    }

    private sealed class TestSymbolArrayConverter : SymbolArrayConverter
    {
        public TestSymbolArrayConverter(string[] symbols, string[] utf)
            : base(symbols, utf)
        {
        }

        public override byte Type => 0;
    }
}
