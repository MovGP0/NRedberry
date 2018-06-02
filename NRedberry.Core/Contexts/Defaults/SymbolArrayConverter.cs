using System;
using System.Linq;
using NRedberry.Core.Tensors;

namespace NRedberry.Core.Contexts.Defaults
{
    public abstract class SymbolArrayConverter : IIndexSymbolConverter
    {
        private string[] Symbols { get; }
        private string[] UTF { get; }

        protected SymbolArrayConverter(string[] symbols, string[] utf)
        {
            Symbols = symbols;
            UTF = utf;
            if (symbols.Length != utf.Length)
                throw new ApplicationException();
        }

        public bool ApplicableToSymbol(string symbol)
        {
            return Symbols.Any(s => s.Equals(symbol));
        }

        public int GetCode(string symbol)
        {
            for (var i = 0; i < Symbols.Length; ++i)
            {
                if (Symbols[i].Equals(symbol))
                {
                    return i;
                }
            }

            throw new IndexConverterException();
        }

        public string GetSymbol(int code, OutputFormat mode)
        {
            try
            {
                switch (mode)
                {
                    case OutputFormat.UTF8:
                        return UTF[code];
                    case OutputFormat.RedberryConsole:
                        return "\\" + Symbols[code];
                    default:
                        return Symbols[code];
                }
            }
            catch (ArgumentOutOfRangeException e)
            {
                throw new IndexConverterException(e.Message, e);
            }
        }

        public int MaxNumberOfSymbols => Symbols.Length - 1;
        public abstract byte Type { get; }
    }
}