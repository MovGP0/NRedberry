using NRedberry.Contexts.Defaults;
using NRedberry.Core.Exceptions;
using Xunit;

namespace NRedberry.Core.Contexts.Tests.Defaults;

public sealed class LatinUpperCaseConverterTests
{
    [Fact(DisplayName = "Should map latin upper-case symbols")]
    public void ShouldMapLatinUpperCaseSymbols()
    {
        // Arrange
        var converter = LatinUpperCaseConverter.Instance;

        // Assert
        ((int)converter.Type).ShouldBe(1);
        converter.MaxNumberOfSymbols.ShouldBe(25);
        converter.ApplicableToSymbol("A").ShouldBeTrue();
        converter.ApplicableToSymbol("a").ShouldBeFalse();
        converter.ApplicableToSymbol("AA").ShouldBeFalse();

        converter.GetCode("A").ShouldBe(0);
        converter.GetCode("Z").ShouldBe(25);
        converter.GetSymbol(0, OutputFormat.Redberry).ShouldBe("A");
    }

    [Fact(DisplayName = "Should throw when code is out of range")]
    public void ShouldThrowWhenCodeIsOutOfRange()
    {
        // Arrange
        var converter = LatinUpperCaseConverter.Instance;

        // Act + Assert
        Should.Throw<IndexConverterException>(() => converter.GetSymbol(26, OutputFormat.Redberry));
    }
}
