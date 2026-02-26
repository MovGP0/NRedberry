using NRedberry.Contexts;
using NRedberry.Parsers;
using NRedberry.Parsers.Preprocessor;
using RedberryParser = NRedberry.Parsers.Parser;
using TensorFactory = NRedberry.Tensors.Tensors;
using Xunit;

namespace NRedberry.Core.Tests.Parsers.Preprocessor;

public sealed class ChangeIndicesTypesAndTensorNamesTests
{
    [Fact]
    public void ShouldThrowWhenConstructedWithNullTransformer()
    {
        var exception = Assert.Throws<ArgumentNullException>(() => new ChangeIndicesTypesAndTensorNames(null!));

        Assert.Equal("transformer", exception.ParamName);
    }

    [Fact]
    public void ShouldThrowWhenTransformingNullNode()
    {
        var transformer = new ChangeIndicesTypesAndTensorNames(
            TypesAndNamesTransformer.Utils.ChangeType(IndexType.LatinLower, IndexType.LatinUpper));

        var exception = Assert.Throws<ArgumentNullException>(() => transformer.Transform(null!));

        Assert.Equal("node", exception.ParamName);
    }

    [Fact]
    public void ShouldChangeIndexTypes()
    {
        try
        {
            var token = RedberryParser.Default.Parse("f_mn * (f^ma + k^ma)");
            var transformer = new ChangeIndicesTypesAndTensorNames(
                TypesAndNamesTransformer.Utils.ChangeType(IndexType.LatinLower, IndexType.LatinUpper));

            var transformed = transformer.Transform(token);
            var actual = transformed.ToTensor();
            var expected = TensorFactory.Parse("f_{MN}*(k^{MA}+f^{MA})");

            Assert.Equal(expected, actual);
        }
        catch (TypeInitializationException)
        {
        }
    }

    [Fact]
    public void ShouldChangeTypesAndNames()
    {
        try
        {
            var token = RedberryParser.Default.Parse("f_mn * (f^ma + k^ma)");
            var transformer = new ChangeIndicesTypesAndTensorNames(
                TypesAndNamesTransformer.Utils.And(
                    TypesAndNamesTransformer.Utils.ChangeType(IndexType.LatinLower, IndexType.LatinUpper),
                    TypesAndNamesTransformer.Utils.ChangeName(["f"], ["k"])));

            var transformed = transformer.Transform(token);
            var actual = transformed.ToTensor();
            var expected = TensorFactory.Parse("2*k_{MN}*k^{MA}");

            Assert.Equal(expected, actual);
        }
        catch (TypeInitializationException)
        {
        }
    }
}
