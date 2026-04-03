using NRedberry.Contexts.Defaults;
using NRedberry.Indices;
using Xunit;

namespace NRedberry.Core.Contexts.Tests.Defaults;

public sealed class IndexWithStrokeConverterTests
{
    [Fact(DisplayName = "Should handle stroked symbols")]
    public void ShouldHandleStrokedSymbols()
    {
        // Arrange
        var converter = new IndexWithStrokeConverter(LatinLowerCaseConverter.Instance, 2);

        // Assert
        converter.ApplicableToSymbol("a''").ShouldBeTrue();
        converter.ApplicableToSymbol("a'").ShouldBeFalse();
        converter.ApplicableToSymbol("A''").ShouldBeFalse();

        converter.GetCode("a''").ShouldBe(0);
        converter.GetSymbol(0, OutputFormat.Redberry).ShouldBe("a''");
        ((int)converter.Type).ShouldBe(IndexTypeMethods.AlphabetsCount);
    }
}
