using NRedberry.Contexts;

namespace NRedberry.Parsers;

public static class ContextSettingsExtensions
{
    public static Parser Parser(this ContextSettings _) => Parsers.Parser.Default;
}
