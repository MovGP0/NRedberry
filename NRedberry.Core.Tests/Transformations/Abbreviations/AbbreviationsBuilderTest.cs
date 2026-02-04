using System.IO;
using NRedberry.Tensors;
using NRedberry.Transformations;
using NRedberry.Transformations.Abbreviations;
using NRedberry.Transformations.Expand;
using NRedberry.Transformations.Substitutions;
using TensorFactory = NRedberry.Tensors.Tensors;
using TensorType = NRedberry.Tensors.Tensor;
using Xunit;

namespace NRedberry.Core.Tests.Transformations.Abbreviations;

public sealed class AbbreviationsBuilderTest
{
    [Fact]
    public void ShouldBuildAbbreviationsForNestedSums()
    {
        AbbreviationsBuilder abbrs = new();
        TensorType tensor = TensorFactory.Parse("(c*(a+b) + f)*k_a*k^a*f_q + 2*(c*(-a-b) - f)*k_a*k^a*t_q");
        AssertCorrectAbbreviations(abbrs, tensor);

        _ = abbrs.Transform(tensor);
    }

    [Fact]
    public void ShouldAbbreviateTopLevelWhenEnabled()
    {
        AbbreviationsBuilder abbrs = new()
        {
            AbbreviateTopLevel = true
        };

        TensorType tensor = TensorFactory.Parse("a*(c+d) + b*(c+d)");
        AssertCorrectAbbreviations(abbrs, tensor);

        _ = abbrs.Transform(tensor);
    }

    [Fact]
    public void ShouldKeepTopLevelWhenDisabled()
    {
        AbbreviationsBuilder abbrs = new()
        {
            AbbreviateTopLevel = false
        };

        TensorType tensor = TensorFactory.Parse("(a+b)*k_a*p^a + (a+b)*f_a*t^a");
        AssertCorrectAbbreviations(abbrs, tensor);

        _ = abbrs.Transform(tensor);
        _ = abbrs.GetAbbreviations();
    }

    [Fact]
    public void ShouldSerializeAndDeserializeAbbreviations()
    {
        AbbreviationsBuilder abbrs = new()
        {
            AbbreviateTopLevel = true
        };

        _ = abbrs.Transform(TensorFactory.Parse("(a+b)*k_a*p^a + (a+b)*f_a*t^a"));

        string tempDirectory = Path.Combine(".temp");
        Directory.CreateDirectory(tempDirectory);
        string filePath = Path.Combine(tempDirectory, "abbreviations.json");

        try
        {
            AbbreviationsBuilder.WriteToFile(abbrs, filePath);
            AbbreviationsBuilder deserialized = AbbreviationsBuilder.ReadFromFile(filePath);

            Assert.Equal(abbrs.AbbreviateTopLevel, deserialized.AbbreviateTopLevel);
            HashSet<Abbreviation> expected = new(abbrs.GetAbbreviations());
            HashSet<Abbreviation> actual = new(deserialized.GetAbbreviations());
            Assert.Equal(expected, actual);
        }
        finally
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            if (Directory.Exists(tempDirectory) && Directory.GetFiles(tempDirectory).Length == 0)
            {
                Directory.Delete(tempDirectory);
            }
        }
    }

    private static void AssertCorrectAbbreviations(AbbreviationsBuilder abbrs, TensorType tensor)
    {
        TensorType transformed = abbrs.Transform(tensor);
        SubstitutionTransformation substitutions = abbrs.AbbreviationReplacements();
        TensorType expandedOriginal = ExpandTransformation.Expand(tensor);
        TensorType expandedApplied = ExpandTransformation.Expand(
            Transformation.ApplyUntilUnchanged(transformed, substitutions));
        Assert.True(TensorUtils.Equals(expandedOriginal, expandedApplied));
    }
}
