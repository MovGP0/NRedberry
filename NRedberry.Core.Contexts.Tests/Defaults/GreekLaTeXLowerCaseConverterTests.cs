using NRedberry.Contexts.Defaults;
using NRedberry.Core.Exceptions;
using Xunit;

namespace NRedberry.Core.Contexts.Tests.Defaults;

public sealed class GreekLaTeXLowerCaseConverterTests
{
    [Fact(DisplayName = "Should map Greek lower-case symbols")]
    public void ShouldMapGreekLowerCaseSymbols()
    {
        // Arrange
        var converter = GreekLaTeXLowerCaseConverter.Instance;

        // Assert
        ((int)converter.Type).ShouldBe(2);
        converter.ApplicableToSymbol("\\alpha").ShouldBeTrue();
        converter.ApplicableToSymbol("alpha").ShouldBeFalse();

        converter.GetCode("\\alpha").ShouldBe(0);
        converter.GetSymbol(0, OutputFormat.UTF8).ShouldBe("α");
        converter.GetSymbol(0, OutputFormat.LaTeX).ShouldBe("\\alpha");
        converter.GetSymbol(0, OutputFormat.Redberry).ShouldBe("\\\\alpha");
    }

    [Fact(DisplayName = "Should throw for invalid symbol codes")]
    public void ShouldThrowForInvalidSymbolCodes()
    {
        // Arrange
        var converter = GreekLaTeXLowerCaseConverter.Instance;

        // Act + Assert
        Should.Throw<IndexConverterException>(() => converter.GetSymbol(100, OutputFormat.Redberry));
    }
}
