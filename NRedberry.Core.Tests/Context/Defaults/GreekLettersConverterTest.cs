using NRedberry.Contexts.Defaults;

namespace NRedberry.Core.Tests.Context.Defaults;

public sealed class GreekLettersConverterTest
{
    [Fact]
    public void ShouldConvertUppercaseGreekLetters()
    {
        string[] symbols =
        [
            "\\Gamma",
            "\\Delta",
            "\\Theta",
            "\\Lambda",
            "\\Xi",
            "\\Pi",
            "\\Sigma",
            "\\Upsilon",
            "\\Phi",
            "\\Psi",
            "\\Omega",
        ];
        string[] utf =
        [
            "\u0393",
            "\u0394",
            "\u0398",
            "\u039B",
            "\u039E",
            "\u03A0",
            "\u03A3",
            "\u03A5",
            "\u03A6",
            "\u03A8",
            "\u03A9",
        ];

        GreekLaTeXUpperCaseConverter converter = GreekLaTeXUpperCaseConverter.Instance;

        for (int i = 0; i < symbols.Length; ++i)
        {
            converter.ApplicableToSymbol(symbols[i]).ShouldBeTrue();
            converter.GetCode(symbols[i]).ShouldBe(i);
            converter.GetSymbol(i, OutputFormat.LaTeX).ShouldBe(symbols[i]);
            converter.GetSymbol(i, OutputFormat.UTF8).ShouldBe(utf[i]);
        }
    }

    [Fact]
    public void ShouldConvertLowercaseGreekLetters()
    {
        string[] symbols =
        [
            "\\alpha",
            "\\beta",
            "\\gamma",
            "\\delta",
            "\\epsilon",
            "\\zeta",
            "\\eta",
            "\\theta",
            "\\iota",
            "\\kappa",
            "\\lambda",
            "\\mu",
            "\\nu",
            "\\xi",
            "\\pi",
            "\\rho",
            "\\sigma",
            "\\tau",
            "\\upsilon",
            "\\phi",
            "\\chi",
            "\\psi",
            "\\omega",
        ];
        string[] utf =
        [
            "\u03B1",
            "\u03B2",
            "\u03B3",
            "\u03B4",
            "\u03B5",
            "\u03B6",
            "\u03B7",
            "\u03B8",
            "\u03B9",
            "\u03BA",
            "\u03BB",
            "\u03BC",
            "\u03BD",
            "\u03BE",
            "\u03C0",
            "\u03C1",
            "\u03C3",
            "\u03C4",
            "\u03C5",
            "\u03C6",
            "\u03C7",
            "\u03C8",
            "\u03C9",
        ];

        GreekLaTeXLowerCaseConverter converter = GreekLaTeXLowerCaseConverter.Instance;

        for (int i = 0; i < symbols.Length; ++i)
        {
            converter.ApplicableToSymbol(symbols[i]).ShouldBeTrue();
            converter.GetCode(symbols[i]).ShouldBe(i);
            converter.GetSymbol(i, OutputFormat.LaTeX).ShouldBe(symbols[i]);
            converter.GetSymbol(i, OutputFormat.UTF8).ShouldBe(utf[i]);
        }
    }
}
