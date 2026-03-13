using NRedberry.Core.Utils;
using NRedberry.Indices;
using NRedberry.Parsers;
using NRedberry.Parsers.Preprocessor;
using NRedberry.Tensors;
using TensorApi = NRedberry.Tensors.Tensors;
using TensorType = NRedberry.Tensors.Tensor;
using Xunit;

namespace NRedberry.Core.Tests.Parser.Preprocessor;

public sealed class GeneralIndicesInsertionTest
{
    [Fact]
    public void ShouldInsertMatrixIndicesWithSingleRule()
    {
        TensorType tensor = Parse("M*N", "^i", "_j", "M", "N");

        AssertFreeIndices(tensor, "^i_j");
    }

    [Fact]
    public void ShouldInsertIndicesForMultipleMatrixTypes()
    {
        TensorType tensor = Parse("A*(B+C)", "^a", "_b", "A", "B", "C");
        string actual = tensor.ToString(OutputFormat.Redberry);

        AssertFreeIndices(tensor, "^a_b");
        Assert.Contains("A", actual);
        Assert.Contains("B", actual);
        Assert.Contains("C", actual);
    }

    [Fact]
    public void ShouldInsertTraceIndicesForSingleType()
    {
        TensorType tensor = Parse("Tr", "^m", "_m", "Tr");

        Assert.True(tensor.Indices.GetFree().EqualsRegardlessOrder(IndicesFactory.EmptySimpleIndices));
    }

    [Fact]
    public void ShouldInsertIndicesForMultiplicationOrder()
    {
        TensorType leftToRight = Parse("A*B*C", "^i", "_j", "A", "B", "C");
        TensorType grouped = Parse("A*(B*C)", "^i", "_j", "A", "B", "C");

        AssertFreeIndices(leftToRight, "^i_j");
        AssertFreeIndices(grouped, "^i_j");
    }

    [Fact]
    public void ShouldRejectExistingConflictingFreeIndices()
    {
        Assert.Throws<ArgumentException>(() => Parse("M^i", "^i", "_j", "M"));
    }

    private static TensorType Parse(string expression, string upper, string lower, params string[] indicator)
    {
        IIndicator<ParseTokenSimpleTensor> indicatorValue = indicator.Length == 0
            ? new TrueIndicator<ParseTokenSimpleTensor>()
            : new NamesIndicator(indicator);

        return TensorApi.Parse(
            expression,
            new IndicesInsertion(
                ParserIndices.ParseSimple(upper),
                ParserIndices.ParseSimple(lower),
                indicatorValue));
    }

    private static void AssertFreeIndices(TensorType tensor, string expected)
    {
        SimpleIndices expectedIndices = ParserIndices.ParseSimple(expected);

        Assert.True(tensor.Indices.GetFree().EqualsRegardlessOrder(expectedIndices.GetFree()));
    }

    private sealed class NamesIndicator(params string[] names) : IIndicator<ParseTokenSimpleTensor>
    {
        private readonly string[] _names = names;

        public bool Is(ParseTokenSimpleTensor @object)
        {
            foreach (string name in _names)
            {
                if (name == @object.Name)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
