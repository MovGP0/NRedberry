using NRedberry.Indices;

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

        Should.Throw<ArgumentException>(() => new NRedberry.Contexts.IndexConverterManager(converters));
    }
}
