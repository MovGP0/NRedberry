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

        first.ShouldBe("x2");
        second.ShouldBe("x3");
    }

    [Fact]
    public void ShouldThrowWhenPrefixIsNull()
    {
        ArgumentNullException exception = Should.Throw<ArgumentNullException>(() => new LocalSymbolsProvider(TensorApi.Parse("a"), null!));

        exception.ParamName.ShouldBe("prefix");
    }
}
