using NRedberry.Contexts;

namespace NRedberry.Core.Parsers;

public static class ContextSettingsExtensions
{
    public static Parser Parser(this ContextSettings _) => Parsers.Parser.Default;
}
