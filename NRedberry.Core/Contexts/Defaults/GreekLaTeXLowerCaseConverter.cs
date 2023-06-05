namespace NRedberry.Core.Contexts.Defaults;

public sealed class GreekLaTeXLowerCaseConverter : SymbolArrayConverter
{
    private static string[] Symbols { get; } = new string[23];
    private static string[] UTF { get; } = new string[23];

    static GreekLaTeXLowerCaseConverter()
    {
        Symbols[0] = "\\alpha";
        Symbols[1] = "\\beta";
        Symbols[2] = "\\gamma";
        Symbols[3] = "\\delta";
        Symbols[4] = "\\epsilon";
        Symbols[5] = "\\zeta";
        Symbols[6] = "\\eta";
        Symbols[7] = "\\theta";
        Symbols[8] = "\\iota";
        Symbols[9] = "\\kappa";
        Symbols[10] = "\\lambda";
        Symbols[11] = "\\mu";
        Symbols[12] = "\\nu";
        Symbols[13] = "\\xi";
        //symbols[14] = "o";//\\omicron
        Symbols[14] = "\\pi";
        Symbols[15] = "\\rho";
        Symbols[16] = "\\sigma";
        //symbols[17]= "final sigma??"
        Symbols[17] = "\\tau";
        Symbols[18] = "\\upsilon";
        Symbols[19] = "\\phi";
        Symbols[20] = "\\chi";
        Symbols[21] = "\\psi";
        Symbols[22] = "\\omega";

        for (var i = 0; i < 23; ++i)
        {
            char greekLetter;
            if (i >= 16)
                greekLetter = (char)((char)0x03b1 + i + 2);
            else if (i >= 14)
                greekLetter = (char)((char)0x03b1 + i + 1);
            else
                greekLetter = (char)((char)0x03b1 + i);
            UTF[i] = char.ToString(greekLetter);
        }
    }

    private GreekLaTeXLowerCaseConverter() : base(Symbols, UTF)
    {
    }

    public static GreekLaTeXLowerCaseConverter Instance = new GreekLaTeXLowerCaseConverter();

    public override byte Type => 2;
}