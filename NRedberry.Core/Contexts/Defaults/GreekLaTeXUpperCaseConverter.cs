using System.Diagnostics.Contracts;

namespace NRedberry.Contexts.Defaults;

public class GreekLaTeXUpperCaseConverter : SymbolArrayConverter
{
    private static readonly string[] symbols = new string[11];
    private static readonly string[] utf = new string[11];

    static GreekLaTeXUpperCaseConverter()
    {
        symbols[0] = "\\Gamma";
        symbols[1] = "\\Delta";
        symbols[2] = "\\Theta";
        symbols[3] = "\\Lambda";
        symbols[4] = "\\Xi";
        symbols[5] = "\\Pi";
        symbols[6] = "\\Sigma";
        symbols[7] = "\\Upsilon";
        symbols[8] = "\\Phi";
        symbols[9] = "\\Psi";
        symbols[10] = "\\Omega";

        utf[0] = ((char)0x0393).ToString();
        utf[1] = ((char)0x0394).ToString();
        utf[2] = ((char)0x0398).ToString();
        utf[3] = ((char)0x039B).ToString();
        utf[4] = ((char)0x039E).ToString();
        utf[5] = ((char)0x03A0).ToString();
        utf[6] = ((char)0x03A3).ToString();
        utf[7] = ((char)0x03A5).ToString();
        utf[8] = ((char)0x03A6).ToString();
        utf[9] = ((char)0x03A8).ToString();
        utf[10] = ((char)0x03A9).ToString();
    }

    [Pure]
    public static GreekLaTeXUpperCaseConverter Instance { get; } = new();

    private GreekLaTeXUpperCaseConverter() : base(symbols, utf)
    {
    }

    [Pure]
    public override byte GetType_() => 3;
}