namespace NRedberry;

public interface IIndexSymbolConverter
{
    bool ApplicableToSymbol(string symbol);

    string GetSymbol(long code, OutputFormat outputFormat);

    int GetCode(string symbol);

    int MaxNumberOfSymbols();

    byte GetType_();
}