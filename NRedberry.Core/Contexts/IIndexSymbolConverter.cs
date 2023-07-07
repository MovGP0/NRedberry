using NRedberry.Core.Tensors;

namespace NRedberry.Core.Contexts;

public interface IIndexSymbolConverter
{
    bool ApplicableToSymbol(string symbol);

    string GetSymbol(long code, OutputFormat outputFormat);

    long GetCode(string symbol);

    long MaxNumberOfSymbols();

    byte GetType_();
}