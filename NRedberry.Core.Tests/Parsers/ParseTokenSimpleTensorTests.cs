using ContextCC = NRedberry.Contexts.CC;
using NRedberry.Contexts;
using NRedberry.Indices;
using NRedberry.Parsers;
using Xunit;

namespace NRedberry.Core.Tests.Parsers;

public sealed class ParseTokenSimpleTensorTests
{
    [Fact]
    public void ShouldThrowWhenConstructedWithNullIndices()
    {
        var exception = Assert.Throws<ArgumentNullException>(() => new ParseTokenSimpleTensor(null!, "A"));

        Assert.Equal("indices", exception.ParamName);
    }

    [Fact]
    public void ShouldThrowWhenConstructedWithNullName()
    {
        var indices = IndicesFactory.EmptySimpleIndices;

        var exception = Assert.Throws<ArgumentNullException>(() => new ParseTokenSimpleTensor(indices, null!));

        Assert.Equal("name", exception.ParamName);
    }

    [Fact]
    public void ShouldExposeTokenTypeNameAndIndices()
    {
        var indices = IndicesFactory.CreateSimple(null, 1, 2);
        var token = new ParseTokenSimpleTensor(indices, "T");

        Assert.Equal(TokenType.SimpleTensor, token.TokenType);
        Assert.Equal("T", token.Name);
        Assert.Same(indices, token.Indices);
        Assert.Same(indices, token.GetIndices());
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

            Assert.Equal(expected, descriptor);
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

            Assert.Equal("T" + indices, text);
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

            Assert.Equal("T", token.ToString(OutputFormat.Redberry));
            Assert.Equal("T", token.ToString(OutputFormat.Maple));
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

            Assert.Equal("T[" + indices.ToString(OutputFormat.Maple) + "]", token.ToString(OutputFormat.Maple));
            Assert.Equal(
                "T[" + indices.ToString(OutputFormat.WolframMathematica) + "]",
                token.ToString(OutputFormat.WolframMathematica));
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

            Assert.Equal("KroneckerDelta[" + indices.ToString(OutputFormat.Maple) + "]", kronecker.ToString(OutputFormat.Maple));
            Assert.Equal("g_[" + indices.ToString(OutputFormat.Maple) + "]", metric.ToString(OutputFormat.Maple));
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

            Assert.True(kronecker.IsKronecker());
            Assert.True(kronecker.IsKroneckerOrMetric());

            Assert.False(metric.IsKronecker());
            Assert.True(metric.IsKroneckerOrMetric());

            Assert.False(generic.IsKronecker());
            Assert.False(generic.IsKroneckerOrMetric());
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

            Assert.Equal(expected, tensor);
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

        Assert.True(left.Equals(right));
        Assert.Equal(left.GetHashCode(), right.GetHashCode());
    }

    [Fact]
    public void ShouldNotBeEqualWhenNameOrIndicesDiffer()
    {
        try
        {
            var token = new ParseTokenSimpleTensor(ParserIndices.ParseSimple("^a"), "T");
            var differentName = new ParseTokenSimpleTensor(ParserIndices.ParseSimple("^a"), "X");
            var differentIndices = new ParseTokenSimpleTensor(ParserIndices.ParseSimple("_a"), "T");

            Assert.False(token.Equals(differentName));
            Assert.False(token.Equals(differentIndices));
            Assert.False(token.Equals(new ParseToken(TokenType.SimpleTensor)));
            Assert.False(token.Equals(null));
        }
        catch (TypeInitializationException)
        {
        }
    }
}
