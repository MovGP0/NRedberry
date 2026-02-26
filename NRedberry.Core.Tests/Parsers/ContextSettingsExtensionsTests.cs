using System.Runtime.CompilerServices;
using NRedberry.Contexts;
using NRedberry.Parsers;
using RedberryParser = NRedberry.Parsers.Parser;
using Xunit;

namespace NRedberry.Core.Tests.Parsers;

public sealed class ContextSettingsExtensionsTests
{
    [Fact]
    public void ShouldReturnDefaultParserForContextSettingsInstance()
    {
        var settings = CreateContextSettingsWithoutConstructor();

        var parser = settings.Parser();

        Assert.Same(RedberryParser.Default, parser);
    }

    [Fact]
    public void ShouldReturnDefaultParserEvenWhenSettingsParserIsCustomized()
    {
        var settings = CreateContextSettingsWithoutConstructor();
        settings.Parser = new RedberryParser();

        var parser = settings.Parser();

        Assert.Same(RedberryParser.Default, parser);
        Assert.NotSame(settings.Parser, parser);
    }

    [Fact]
    public void ShouldReturnDefaultParserWhenContextSettingsIsNull()
    {
        ContextSettings? settings = null;

        var parser = ContextSettingsExtensions.Parser(settings!);

        Assert.Same(RedberryParser.Default, parser);
    }

    private static ContextSettings CreateContextSettingsWithoutConstructor()
    {
        return (ContextSettings)RuntimeHelpers.GetUninitializedObject(typeof(ContextSettings));
    }
}
