using NRedberry.Indices;
using Xunit;

namespace NRedberry.Core.Tests.Context;

public sealed class IndexConverterManagerTest
{
    [Fact]
    public void ShouldRejectDuplicateConverters()
    {
        IIndexSymbolConverter[] converters =
        [
            IndexType.LatinLower.GetSymbolConverter(),
            IndexType.Matrix1.GetSymbolConverter(),
            IndexType.LatinLower.GetSymbolConverter(),
            IndexType.LatinUpper.GetSymbolConverter(),
        ];

        Assert.Throws<ArgumentException>(() => new NRedberry.Contexts.IndexConverterManager(converters));
    }
}
