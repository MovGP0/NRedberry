using NRedberry.Contexts.Defaults;
using NRedberry.Core.Exceptions;
using Shouldly;
using Xunit;

namespace NRedberry.Core.Contexts.Tests.Defaults;

public sealed class LatinLowerCaseConverterTests
{
    [Fact(DisplayName = "Should map latin lower-case symbols")]
    public void ShouldMapLatinLowerCaseSymbols()
    {
        // Arrange
        var converter = LatinLowerCaseConverter.Instance;

        // Assert
        ((int)converter.Type).ShouldBe(0);
        converter.MaxNumberOfSymbols.ShouldBe(25);
        converter.ApplicableToSymbol("a").ShouldBeTrue();
        converter.ApplicableToSymbol("A").ShouldBeFalse();
        converter.ApplicableToSymbol("aa").ShouldBeFalse();

        converter.GetCode("a").ShouldBe(0);
        converter.GetCode("z").ShouldBe(25);
        converter.GetSymbol(0, OutputFormat.Redberry).ShouldBe("a");
    }

    [Fact(DisplayName = "Should throw when code is out of range")]
    public void ShouldThrowWhenCodeIsOutOfRange()
    {
        // Arrange
        var converter = LatinLowerCaseConverter.Instance;

        // Act + Assert
        Should.Throw<IndexConverterException>(() => converter.GetSymbol(26, OutputFormat.Redberry));
    }
}
