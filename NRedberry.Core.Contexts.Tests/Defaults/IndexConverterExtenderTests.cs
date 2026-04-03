using NRedberry.Contexts.Defaults;
using NRedberry.Core.Exceptions;
using Xunit;

namespace NRedberry.Core.Contexts.Tests.Defaults;

public sealed class IndexConverterExtenderTests
{
    [Fact(DisplayName = "Should validate and parse subscripted symbols")]
    public void ShouldValidateAndParseSubscriptedSymbols()
    {
        // Arrange
        var converter = new IndexConverterExtender(LatinLowerCaseConverter.Instance);

        // Assert
        converter.ApplicableToSymbol("a").ShouldBeTrue();
        converter.ApplicableToSymbol("a_2").ShouldBeTrue();
        converter.ApplicableToSymbol("a_{2}").ShouldBeTrue();
        converter.ApplicableToSymbol("a_").ShouldBeFalse();
        converter.ApplicableToSymbol("a_{").ShouldBeFalse();

        converter.GetCode("a").ShouldBe(0);
        converter.GetCode("a_2").ShouldBe(52);
        converter.GetCode("a_{2}").ShouldBe(52);
    }

    [Fact(DisplayName = "Should format symbols for output formats")]
    public void ShouldFormatSymbolsForOutputFormats()
    {
        // Arrange
        var converter = new IndexConverterExtender(LatinLowerCaseConverter.Instance);

        // Act
        string redberry = converter.GetSymbol(52, OutputFormat.Redberry);
        string mathematica = converter.GetSymbol(52, OutputFormat.WolframMathematica);
        string maple = converter.GetSymbol(52, OutputFormat.Maple);

        // Assert
        redberry.ShouldBe("a_{2}");
        mathematica.ShouldBe("Subscript[a, 2]");
        maple.ShouldBe("a2");
        converter.MaxNumberOfSymbols.ShouldBe(259);
    }

    [Fact(DisplayName = "Should throw for invalid symbols")]
    public void ShouldThrowForInvalidSymbols()
    {
        // Arrange
        var converter = new IndexConverterExtender(LatinLowerCaseConverter.Instance);

        // Act + Assert
        Should.Throw<IndexConverterException>(() => converter.GetCode(string.Empty));
        Should.Throw<IndexConverterException>(() => converter.GetCode("a_"));
    }
}
