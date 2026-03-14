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

        builder.MaxSumSize.ShouldBe(AbbreviationsBuilder.DefaultAbbrSize);
        builder.AbbrPrefix.ShouldBe(AbbreviationsBuilder.DefaultAbbrPrefix);
        builder.AbbreviateScalars.ShouldBeTrue();
        builder.AbbreviateScalarsSeparately.ShouldBeFalse();
        builder.AbbreviateTopLevel.ShouldBeFalse();
        builder.Locked.ShouldBeFalse();
        builder.GetAbbreviations().ShouldBeEmpty();
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

        result.ShouldBeSameAs(tensor);
        builder.GetAbbreviations().ShouldBeEmpty();
    }
}
