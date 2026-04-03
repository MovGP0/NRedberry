using System.Runtime.CompilerServices;
using NRedberry.Contexts;
using NRedberry.Parsers;
using RedberryParser = NRedberry.Parsers.Parser;

namespace NRedberry.Core.Tests.Parsers;

public sealed class ContextSettingsExtensionsTests
{
    [Fact]
    public void ShouldReturnDefaultParserForContextSettingsInstance()
    {
        var settings = CreateContextSettingsWithoutConstructor();

        var parser = settings.Parser();

        parser.ShouldBeSameAs(RedberryParser.Default);
    }

    [Fact]
    public void ShouldReturnDefaultParserEvenWhenSettingsParserIsCustomized()
    {
        var settings = CreateContextSettingsWithoutConstructor();
        settings.Parser = new RedberryParser();

        var parser = settings.Parser();

        parser.ShouldBeSameAs(RedberryParser.Default);
        parser.ShouldNotBeSameAs(settings.Parser);
    }

    [Fact]
    public void ShouldReturnDefaultParserWhenContextSettingsIsNull()
    {
        ContextSettings? settings = null;

        var parser = ContextSettingsExtensions.Parser(settings!);

        parser.ShouldBeSameAs(RedberryParser.Default);
    }

    private static ContextSettings CreateContextSettingsWithoutConstructor()
    {
        return (ContextSettings)RuntimeHelpers.GetUninitializedObject(typeof(ContextSettings));
    }
}
