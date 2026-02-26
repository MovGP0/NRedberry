using NRedberry.Contexts;
using NRedberry.Parsers;
using NRedberry.Tensors;
using NRedberry.Transformations.Symmetrization;
using Shouldly;
using Xunit;

namespace NRedberry.Core.Contexts.Tests;

public sealed class ParseManagerTests
{
    [Fact(DisplayName = "Should validate parser input")]
    public void ShouldValidateParserInput()
    {
        // Act + Assert
        Should.Throw<ArgumentNullException>(() => _ = new ParseManager(null!));
    }

    [Fact(DisplayName = "Should apply node and tensor preprocessors")]
    public void ShouldApplyNodeAndTensorPreprocessors()
    {
        // Arrange
        var manager = new ParseManager(Parser.Default);
        var nodeTransformer = new CountingParseTokenTransformer();
        var tensorTransformer = new CountingTransformation();

        // Act
        Tensor result = manager.Parse(
            "a",
            new[] { tensorTransformer },
            new[] { nodeTransformer });

        // Assert
        result.ShouldNotBeNull();
        nodeTransformer.Count.ShouldBe(1);
        tensorTransformer.Count.ShouldBe(1);
    }

    [Fact(DisplayName = "Should use and reset default preprocessors")]
    public void ShouldUseAndResetDefaultPreprocessors()
    {
        // Arrange
        var manager = new ParseManager(Parser.Default);
        var nodeTransformer = new CountingParseTokenTransformer();
        var tensorTransformer = new CountingTransformation();
        manager.DefaultParserPreprocessors.Add(nodeTransformer);
        manager.DefaultTensorPreprocessors.Add(tensorTransformer);

        // Act
        manager.Parse("a");

        // Assert
        nodeTransformer.Count.ShouldBe(1);
        tensorTransformer.Count.ShouldBe(1);

        // Act
        manager.Reset();

        // Assert
        manager.DefaultParserPreprocessors.ShouldBeEmpty();
        manager.DefaultTensorPreprocessors.ShouldBeEmpty();
    }

    private sealed class CountingParseTokenTransformer : IParseTokenTransformer
    {
        public int Count { get; private set; }

        public ParseToken Transform(ParseToken node)
        {
            Count++;
            return node;
        }
    }

    private sealed class CountingTransformation : ITransformation
    {
        public int Count { get; private set; }

        public Tensor Transform(Tensor t)
        {
            Count++;
            return t;
        }
    }
}
