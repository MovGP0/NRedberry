using ContextCC = NRedberry.Contexts.CC;
using NRedberry.Contexts;
using NRedberry.Indices;
using NRedberry.Parsers;

namespace NRedberry.Core.Tests.Parsers;

public sealed class ParseTokenSimpleTensorTests
{
    [Fact]
    public void ShouldThrowWhenConstructedWithNullIndices()
    {
        var exception = Should.Throw<ArgumentNullException>(() => new ParseTokenSimpleTensor(null!, "A"));

        exception.ParamName.ShouldBe("indices");
    }

    [Fact]
    public void ShouldThrowWhenConstructedWithNullName()
    {
        var indices = IndicesFactory.EmptySimpleIndices;

        var exception = Should.Throw<ArgumentNullException>(() => new ParseTokenSimpleTensor(indices, null!));

        exception.ParamName.ShouldBe("name");
    }

    [Fact]
    public void ShouldExposeTokenTypeNameAndIndices()
    {
        var indices = IndicesFactory.CreateSimple(null, 1, 2);
        var token = new ParseTokenSimpleTensor(indices, "T");

        token.TokenType.ShouldBe(TokenType.SimpleTensor);
        token.Name.ShouldBe("T");
        token.Indices.ShouldBeSameAs(indices);
        token.GetIndices().ShouldBeSameAs(indices);
    }

    [Fact]
    public void ShouldReturnNameAndIndicesStructure()
    {
        try
        {
            var indices = IndicesFactory.CreateSimple(null, 1, 2);
            var token = new ParseTokenSimpleTensor(indices, "T");

            var descriptor = token.GetIndicesTypeStructureAndName();
            var expected = new NameAndStructureOfIndices("T", [StructureOfIndices.Create(indices)]);

            descriptor.ShouldBe(expected);
        }
        catch (TypeInitializationException)
        {
        }
    }

    [Fact]
    public void ShouldFormatUsingNameAndIndicesForDefaultToString()
    {
        try
        {
            var indices = IndicesFactory.CreateSimple(null, 1, 2);
            var token = new ParseTokenSimpleTensor(indices, "T");

            var text = token.ToString();

            text.ShouldBe("T" + indices);
        }
        catch (TypeInitializationException)
        {
        }
    }

    [Fact]
    public void ShouldReturnNameOnlyWhenIndicesAreEmpty()
    {
        try
        {
            var token = new ParseTokenSimpleTensor(IndicesFactory.EmptySimpleIndices, "T");

            token.ToString(OutputFormat.Redberry).ShouldBe("T");
            token.ToString(OutputFormat.Maple).ShouldBe("T");
        }
        catch (TypeInitializationException)
        {
        }
    }

    [Fact]
    public void ShouldWrapIndicesInBracketsForExternalFormats()
    {
        try
        {
            var indices = IndicesFactory.CreateSimple(null, 1, 2);
            var token = new ParseTokenSimpleTensor(indices, "T");

            token.ToString(OutputFormat.Maple).ShouldBe("T[" + indices.ToString(OutputFormat.Maple) + "]");
            token.ToString(OutputFormat.WolframMathematica).ShouldBe("T[" + indices.ToString(OutputFormat.WolframMathematica) + "]");
        }
        catch (TypeInitializationException)
        {
        }
    }

    [Fact]
    public void ShouldUseSpecialMapleNamesForKroneckerAndMetric()
    {
        try
        {
            var indices = IndicesFactory.CreateSimple(null, 1, 2);
            var kronecker = new ParseTokenSimpleTensor(indices, ContextCC.NameManager.KroneckerName);
            var metric = new ParseTokenSimpleTensor(indices, ContextCC.NameManager.MetricName);

            kronecker.ToString(OutputFormat.Maple).ShouldBe("KroneckerDelta[" + indices.ToString(OutputFormat.Maple) + "]");
            metric.ToString(OutputFormat.Maple).ShouldBe("g_[" + indices.ToString(OutputFormat.Maple) + "]");
        }
        catch (TypeInitializationException)
        {
        }
    }

    [Fact]
    public void ShouldIdentifyKroneckerAndMetricNames()
    {
        try
        {
            var indices = IndicesFactory.CreateSimple(null, 1, 2);
            var kronecker = new ParseTokenSimpleTensor(indices, ContextCC.NameManager.KroneckerName);
            var metric = new ParseTokenSimpleTensor(indices, ContextCC.NameManager.MetricName);
            var generic = new ParseTokenSimpleTensor(indices, "T");

            kronecker.IsKronecker().ShouldBeTrue();
            kronecker.IsKroneckerOrMetric().ShouldBeTrue();

            metric.IsKronecker().ShouldBeFalse();
            metric.IsKroneckerOrMetric().ShouldBeTrue();

            generic.IsKronecker().ShouldBeFalse();
            generic.IsKroneckerOrMetric().ShouldBeFalse();
        }
        catch (TypeInitializationException)
        {
        }
    }

    [Fact]
    public void ShouldConvertToEquivalentSimpleTensor()
    {
        try
        {
            var indices = IndicesFactory.CreateSimple(null, 1, 2);
            var token = new ParseTokenSimpleTensor(indices, "T");

            var tensor = token.ToTensor();
            var expected = NRedberry.Tensors.Tensors.SimpleTensor("T", indices);

            tensor.ShouldBe(expected);
        }
        catch (TypeInitializationException)
        {
        }
    }

    [Fact]
    public void ShouldBeEqualWhenNameAndIndicesAreEqual()
    {
        var left = new ParseTokenSimpleTensor(IndicesFactory.CreateSimple(null, 1, 2), "T");
        var right = new ParseTokenSimpleTensor(IndicesFactory.CreateSimple(null, 1, 2), "T");

        left.Equals(right).ShouldBeTrue();
        right.GetHashCode().ShouldBe(left.GetHashCode());
    }

    [Fact]
    public void ShouldNotBeEqualWhenNameOrIndicesDiffer()
    {
        try
        {
            var token = new ParseTokenSimpleTensor(ParserIndices.ParseSimple("^a"), "T");
            var differentName = new ParseTokenSimpleTensor(ParserIndices.ParseSimple("^a"), "X");
            var differentIndices = new ParseTokenSimpleTensor(ParserIndices.ParseSimple("_a"), "T");

            token.Equals(differentName).ShouldBeFalse();
            token.Equals(differentIndices).ShouldBeFalse();
            token.Equals(new ParseToken(TokenType.SimpleTensor)).ShouldBeFalse();
            token.Equals(null).ShouldBeFalse();
        }
        catch (TypeInitializationException)
        {
        }
    }
}
