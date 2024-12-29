namespace NRedberry.Contexts.Defaults;

public class GreekLaTeXUpperCaseConverter : SymbolArrayConverter
{
    private static readonly string[] Symbols = new string[11];
    private static readonly string[] UTF = new string[11];

    static GreekLaTeXUpperCaseConverter()
    {
        Symbols[0] = "\\Gamma";
        Symbols[1] = "\\Delta";
        Symbols[2] = "\\Theta";
        Symbols[3] = "\\Lambda";
        Symbols[4] = "\\Xi";
        Symbols[5] = "\\Pi";
        Symbols[6] = "\\Sigma";
        Symbols[7] = "\\Upsilon";
        Symbols[8] = "\\Phi";
        Symbols[9] = "\\Psi";
        Symbols[10] = "\\Omega";

        UTF[0] = ((char)0x0393).ToString();
        UTF[1] = ((char)0x0394).ToString();
        UTF[2] = ((char)0x0398).ToString();
        UTF[3] = ((char)0x039B).ToString();
        UTF[4] = ((char)0x039E).ToString();
        UTF[5] = ((char)0x03A0).ToString();
        UTF[6] = ((char)0x03A3).ToString();
        UTF[7] = ((char)0x03A5).ToString();
        UTF[8] = ((char)0x03A6).ToString();
        UTF[9] = ((char)0x03A8).ToString();
        UTF[10] = ((char)0x03A9).ToString();
    }

    [Pure]
    public static GreekLaTeXUpperCaseConverter Instance { get; } = new();

    private GreekLaTeXUpperCaseConverter() : base(Symbols, UTF)
    {
    }

    [Pure]
    public override byte Type => 3;
}