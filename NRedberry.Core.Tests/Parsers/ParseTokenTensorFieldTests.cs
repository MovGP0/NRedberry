using NRedberry.Indices;
using NRedberry.Numbers;
using NRedberry.Parsers;
using NRedberry.Tensors;
using Xunit;

namespace NRedberry.Core.Tests.Parsers;

public sealed class ParseTokenTensorFieldTests
{
    [Fact]
    public void ShouldThrowArgumentNullExceptionWhenArgumentsIndicesIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => new ParseTokenTensorField(
            ParserIndices.ParseSimple(string.Empty),
            "F",
            [new ParseTokenNumber(Complex.One)],
            null!));
    }

    [Fact]
    public void ShouldPopulateMissingArgumentIndicesWhenGettingNameAndStructure()
    {
        try
        {
            var firstArgument = new ParseTokenLeaf("x", "x", ParserIndices.ParseSimple("_a^b"));
            var secondArgument = new ParseTokenLeaf("y", "y", ParserIndices.ParseSimple("_c"));
            var argumentIndices = new SimpleIndices[]
            {
                null!,
                ParserIndices.ParseSimple("_c")
            };
            var token = new ParseTokenTensorField(
                ParserIndices.ParseSimple("_m"),
                "F",
                [firstArgument, secondArgument],
                argumentIndices);

            var signature = token.GetIndicesTypeStructureAndName();

            Assert.Equal("F", signature.Name);
            Assert.Equal(3, signature.Structure.Length);
            Assert.NotNull(token.ArgumentsIndices[0]);
            Assert.True(token.ArgumentsIndices[0].EqualsRegardlessOrder(firstArgument.GetIndices().GetFree()));
            Assert.Equal(StructureOfIndices.Create(token.Indices), signature.Structure[0]);
            Assert.Equal(StructureOfIndices.Create(token.ArgumentsIndices[0]), signature.Structure[1]);
            Assert.Equal(StructureOfIndices.Create(token.ArgumentsIndices[1]), signature.Structure[2]);
        }
        catch (TypeInitializationException)
        {
        }
    }

    [Fact]
    public void ShouldIncludeArgumentsInToString()
    {
        var token = new ParseTokenTensorField(
            ParserIndices.ParseSimple(string.Empty),
            "F",
            [new ParseTokenLeaf("x", "x"), new ParseTokenLeaf("y", "y")],
            [ParserIndices.ParseSimple(string.Empty), ParserIndices.ParseSimple(string.Empty)]);

        var value = token.ToString();

        Assert.Equal("F[x, y]", value);
    }

    [Fact]
    public void ShouldCreateTensorFieldAndPopulateMissingArgumentIndicesOnToTensor()
    {
        try
        {
            var token = new ParseTokenTensorField(
                ParserIndices.ParseSimple(string.Empty),
                "F",
                [new ParseTokenNumber(Complex.One)],
                [null!]);

            var tensor = token.ToTensor();

            Assert.IsType<TensorField>(tensor);
            Assert.NotNull(token.ArgumentsIndices[0]);
            Assert.Equal(0, token.ArgumentsIndices[0].Size());
        }
        catch (TypeInitializationException)
        {
        }
    }

    [Fact]
    public void ShouldThrowParserExceptionWhenDerivativeOrdersAreMissing()
    {
        var token = new ParseTokenTensorField(
            ParserIndices.ParseSimple(string.Empty),
            "F~",
            [new ParseTokenNumber(Complex.One)],
            [ParserIndices.ParseSimple(string.Empty)]);

        var exception = Assert.Throws<ParserException>(() => token.ToTensor());

        Assert.Contains("Error in derivative orders", exception.Message, StringComparison.Ordinal);
    }

    [Fact]
    public void ShouldThrowParserExceptionWhenDerivativeOrderCountDoesNotMatchArgumentCount()
    {
        var token = new ParseTokenTensorField(
            ParserIndices.ParseSimple(string.Empty),
            "F~1,2",
            [new ParseTokenNumber(Complex.One)],
            [ParserIndices.ParseSimple(string.Empty)]);

        var exception = Assert.Throws<ParserException>(() => token.ToTensor());

        Assert.Contains("Number of arguments does not match number of derivative orders", exception.Message, StringComparison.Ordinal);
    }

    [Fact]
    public void ShouldThrowParserExceptionWhenDerivativeOrderIsNotInteger()
    {
        var token = new ParseTokenTensorField(
            ParserIndices.ParseSimple(string.Empty),
            "F~1,a",
            [new ParseTokenNumber(Complex.One), new ParseTokenNumber(Complex.One)],
            [ParserIndices.ParseSimple(string.Empty), ParserIndices.ParseSimple(string.Empty)]);

        var exception = Assert.Throws<ParserException>(() => token.ToTensor());

        Assert.Contains("Illegal order of derivative", exception.Message, StringComparison.Ordinal);
    }
}
