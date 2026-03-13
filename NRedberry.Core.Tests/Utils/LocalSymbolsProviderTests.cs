using NRedberry.Utils;
using TensorApi = NRedberry.Tensors.Tensors;
using Xunit;

namespace NRedberry.Core.Tests.Utils;

public sealed class LocalSymbolsProviderTests
{
    [Fact]
    public void ShouldSkipForbiddenNamesWithMatchingPrefix()
    {
        LocalSymbolsProvider provider = new(TensorApi.Parse("x0+x1"), "x");

        string first = provider.Take().ToString(OutputFormat.Redberry);
        string second = provider.Take().ToString(OutputFormat.Redberry);

        Assert.Equal("x2", first);
        Assert.Equal("x3", second);
    }

    [Fact]
    public void ShouldThrowWhenPrefixIsNull()
    {
        ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => new LocalSymbolsProvider(TensorApi.Parse("a"), null!));

        Assert.Equal("prefix", exception.ParamName);
    }
}
