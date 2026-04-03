using NRedberry.Contexts.Defaults;
using NRedberry.Core.Exceptions;
using Xunit;

namespace NRedberry.Core.Contexts.Tests;

public sealed class IndexSymbolConverterTests
{
    [Fact(DisplayName = "Should validate applicable symbols")]
    public void ShouldValidateApplicableSymbols()
    {
        LatinLowerCaseConverter converter = LatinLowerCaseConverter.Instance;

        converter.ApplicableToSymbol("a").ShouldBeTrue();
        converter.ApplicableToSymbol("z").ShouldBeTrue();
        converter.ApplicableToSymbol("A").ShouldBeFalse();
        converter.ApplicableToSymbol("ab").ShouldBeFalse();
    }

    [Fact(DisplayName = "Should convert symbols to codes")]
    public void ShouldConvertSymbolsToCodes()
    {
        LatinLowerCaseConverter converter = LatinLowerCaseConverter.Instance;

        converter.GetCode("a").ShouldBe(0);
        converter.GetCode("z").ShouldBe(25);
    }

    [Fact(DisplayName = "Should convert codes to symbols")]
    public void ShouldConvertCodesToSymbols()
    {
        LatinLowerCaseConverter converter = LatinLowerCaseConverter.Instance;

        converter.GetSymbol(0, OutputFormat.Redberry).ShouldBe("a");
        converter.GetSymbol(25, OutputFormat.Redberry).ShouldBe("z");
    }

    [Fact(DisplayName = "Should throw when code is out of range")]
    public void ShouldThrowWhenCodeIsOutOfRange()
    {
        LatinLowerCaseConverter converter = LatinLowerCaseConverter.Instance;

        Should.Throw<IndexConverterException>(() => converter.GetSymbol(26, OutputFormat.Redberry));
    }

    [Fact(DisplayName = "Should expose type and max symbols")]
    public void ShouldExposeTypeAndMaxSymbols()
    {
        LatinLowerCaseConverter converter = LatinLowerCaseConverter.Instance;

        converter.Type.ShouldBe((byte)0);
        converter.MaxNumberOfSymbols.ShouldBe(25);
    }
}
