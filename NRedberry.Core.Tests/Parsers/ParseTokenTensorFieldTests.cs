using NRedberry.Indices;
using NRedberry.Numbers;
using NRedberry.Parsers;
using NRedberry.Tensors;

namespace NRedberry.Core.Tests.Parsers;

public sealed class ParseTokenTensorFieldTests
{
    [Fact]
    public void ShouldThrowArgumentNullExceptionWhenArgumentsIndicesIsNull()
    {
        Should.Throw<ArgumentNullException>(() => new ParseTokenTensorField(
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

            signature.Name.ShouldBe("F");
            signature.Structure.Length.ShouldBe(3);
            token.ArgumentsIndices[0].ShouldNotBeNull();
            token.ArgumentsIndices[0].EqualsRegardlessOrder(firstArgument.GetIndices().GetFree()).ShouldBeTrue();
            signature.Structure[0].ShouldBe(StructureOfIndices.Create(token.Indices));
            signature.Structure[1].ShouldBe(StructureOfIndices.Create(token.ArgumentsIndices[0]));
            signature.Structure[2].ShouldBe(StructureOfIndices.Create(token.ArgumentsIndices[1]));
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

        value.ShouldBe("F[x, y]");
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

            tensor.ShouldBeOfType<TensorField>();
            token.ArgumentsIndices[0].ShouldNotBeNull();
            token.ArgumentsIndices[0].Size().ShouldBe(0);
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

        var exception = Should.Throw<ParserException>(() => token.ToTensor());

        exception.Message.ShouldContain("Error in derivative orders");
    }

    [Fact]
    public void ShouldThrowParserExceptionWhenDerivativeOrderCountDoesNotMatchArgumentCount()
    {
        var token = new ParseTokenTensorField(
            ParserIndices.ParseSimple(string.Empty),
            "F~1,2",
            [new ParseTokenNumber(Complex.One)],
            [ParserIndices.ParseSimple(string.Empty)]);

        var exception = Should.Throw<ParserException>(() => token.ToTensor());

        exception.Message.ShouldContain("Number of arguments does not match number of derivative orders");
    }

    [Fact]
    public void ShouldThrowParserExceptionWhenDerivativeOrderIsNotInteger()
    {
        var token = new ParseTokenTensorField(
            ParserIndices.ParseSimple(string.Empty),
            "F~1,a",
            [new ParseTokenNumber(Complex.One), new ParseTokenNumber(Complex.One)],
            [ParserIndices.ParseSimple(string.Empty), ParserIndices.ParseSimple(string.Empty)]);

        var exception = Should.Throw<ParserException>(() => token.ToTensor());

        exception.Message.ShouldContain("Illegal order of derivative");
    }
}
