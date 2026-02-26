using NRedberry.Contexts.Defaults;
using NRedberry.Core.Exceptions;
using Shouldly;
using Xunit;

namespace NRedberry.Core.Contexts.Tests.Defaults;

public sealed class GreekLaTeXUpperCaseConverterTests
{
    [Fact(DisplayName = "Should map Greek upper-case symbols")]
    public void ShouldMapGreekUpperCaseSymbols()
    {
        // Arrange
        var converter = GreekLaTeXUpperCaseConverter.Instance;

        // Assert
        ((int)converter.Type).ShouldBe(3);
        converter.ApplicableToSymbol("\\Gamma").ShouldBeTrue();
        converter.ApplicableToSymbol("Gamma").ShouldBeFalse();

        converter.GetCode("\\Gamma").ShouldBe(0);
        converter.GetSymbol(0, OutputFormat.UTF8).ShouldBe("Γ");
        converter.GetSymbol(0, OutputFormat.LaTeX).ShouldBe("\\Gamma");
        converter.GetSymbol(0, OutputFormat.Redberry).ShouldBe("\\\\Gamma");
    }

    [Fact(DisplayName = "Should throw for invalid symbol codes")]
    public void ShouldThrowForInvalidSymbolCodes()
    {
        // Arrange
        var converter = GreekLaTeXUpperCaseConverter.Instance;

        // Act + Assert
        Should.Throw<IndexConverterException>(() => converter.GetSymbol(100, OutputFormat.Redberry));
    }
}
