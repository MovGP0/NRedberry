using NRedberry.Transformations.Abbreviations;
using TensorApi = NRedberry.Tensors.Tensors;
using Xunit;

namespace NRedberry.Core.Tests.Transformations.Abbreviations;

public sealed class AbbreviationsBuilderTests
{
    [Fact]
    public void ShouldExposeExpectedDefaults()
    {
        AbbreviationsBuilder builder = new();

        Assert.Equal(AbbreviationsBuilder.DefaultAbbrSize, builder.MaxSumSize);
        Assert.Equal(AbbreviationsBuilder.DefaultAbbrPrefix, builder.AbbrPrefix);
        Assert.True(builder.AbbreviateScalars);
        Assert.False(builder.AbbreviateScalarsSeparately);
        Assert.False(builder.AbbreviateTopLevel);
        Assert.False(builder.Locked);
        Assert.Empty(builder.GetAbbreviations());
    }

    [Fact]
    public void ShouldRespectLockedModeForNewAbbreviations()
    {
        AbbreviationsBuilder builder = new()
        {
            Locked = true,
            AbbreviateTopLevel = true
        };

        NRedberry.Tensors.Tensor tensor = TensorApi.Parse("a+b");
        NRedberry.Tensors.Tensor result = builder.Transform(tensor);

        Assert.Same(tensor, result);
        Assert.Empty(builder.GetAbbreviations());
    }
}
