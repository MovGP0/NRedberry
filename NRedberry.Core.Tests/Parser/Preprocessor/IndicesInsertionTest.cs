using NRedberry.Core.Utils;
using NRedberry.Indices;
using NRedberry.Parsers;
using NRedberry.Parsers.Preprocessor;
using NRedberry.Tensors;
using TensorApi = NRedberry.Tensors.Tensors;
using TensorType = NRedberry.Tensors.Tensor;
using Xunit;

namespace NRedberry.Core.Tests.Parser.Preprocessor;

public sealed class IndicesInsertionTest
{
    [Fact]
    public void ShouldInsertIndicesForMultipleFactors()
    {
        TensorType tensor = Parse("A*B*C", "^i", "_j", "A", "B", "C");

        AssertIndicesParity(tensor, "^i_j");
    }

    [Fact]
    public void ShouldInsertIndicesInNestedExpression()
    {
        TensorType tensor = Parse("A*(B*A+C*K)*F", "^i", "_j");

        AssertIndicesParity(tensor, "^i_j");
        Assert.Contains("A", tensor.ToString(OutputFormat.Redberry));
        Assert.Contains("F", tensor.ToString(OutputFormat.Redberry));
    }

    [Fact]
    public void ShouldInsertIndicesWithExistingGreekIndices()
    {
        TensorType tensor = Parse("A^{\\alpha n}*B*C", "^ij", "_pq");

        AssertIndicesParity(tensor, "^{\\alpha n i j}_pq");
    }

    [Fact]
    public void ShouldInsertIndicesWithSelectedNamesOnly()
    {
        TensorType tensor = Parse("a*b*A*c*B*C", "^ij", "_pq", "a", "b", "c", "d");

        AssertIndicesParity(tensor, "^ij_pq");
    }

    [Fact]
    public void ShouldInsertIndicesIntoSumsWithSelectedNames()
    {
        TensorType tensor = Parse("a*(b+a)*A*(c+d)*B*C", "^ij", "_pq", "a", "b", "c", "d");
        string actual = tensor.ToString(OutputFormat.Redberry);

        Assert.Contains("a^{ij}", actual);
        Assert.Contains("A", actual);
    }

    [Fact]
    public void ShouldInsertIndicesWhenNamesNotSelected()
    {
        TensorType tensor = Parse("a*(b+a)*A*(c+d)*B*C", "^ij", "_pq", "A", "B", "C", "F");

        AssertIndicesParity(tensor, "^ij_pq");
    }

    [Fact]
    public void ShouldInsertIndicesInNestedSumAndProduct()
    {
        TensorType tensor = Parse("A*(B+E*(R+K*U))", "^i", "_j");

        AssertIndicesParity(tensor, "^i_j");
        Assert.Contains("A^{i}", tensor.ToString(OutputFormat.Redberry));
    }

    [Fact]
    public void ShouldInsertIndicesRepeatedlyWithResetNames()
    {
        for (int i = 0; i < 5; ++i)
        {
            CC.ResetTensorNames();
            TensorType tensor = Parse("A+B", "^i", "_i", "A");

            AssertIndicesParity(tensor, "^i_i");
        }
    }

    [Fact]
    public void ShouldInsertIndicesInExpressionWithSumAndProduct()
    {
        TensorType tensor = Parse("a*A*B+((1/2)*a+b)*A*(A+B*(A+X*A))*c", "^a", "_b", "A", "B");

        AssertIndicesParity(tensor, "^a_b");
    }

    [Fact]
    public void ShouldInsertIndicesInEquation()
    {
        TensorType tensor = Parse("A=2*B+A*B", "^i", "_j");

        AssertIndicesParity(tensor, "^i_j");
    }

    [Fact]
    public void ShouldInsertIndicesForMatrixNamesInEquation()
    {
        const string expression = "DELTA^m=-L*HATK^m";
        string[] matrices =
        [
            "KINV", "HATK", "HATW", "HATS", "NABLAS", "HATN", "HATF", "NABLAF", "HATM", "DELTA",
            "Flat", "FF", "WR", "SR", "SSR", "FR", "RR"
        ];

        TensorType actual = TensorApi.Parse(
            expression,
            new IndicesInsertion(ParserIndices.ParseSimple("^{a}"), ParserIndices.ParseSimple("_{a}"), new NamesIndicator(matrices)));

        string actualText = actual.ToString(OutputFormat.Redberry);

        Assert.Contains("DELTA", actualText);
        Assert.Contains("HATK", actualText);
        Assert.Contains("_{a}", actualText);
    }

    [Fact]
    public void ShouldInsertIndicesInSimpleSum()
    {
        TensorType actual = Parse("A+B", "^i", "_i", "A");

        string actualText = actual.ToString(OutputFormat.Redberry);

        Assert.Contains("A^{i}_{i}", actualText);
        Assert.Contains("B", actualText);
    }

    private static NRedberry.Tensors.Tensor Parse(string tensor, string upper, string lower, params string[] indicator)
    {
        IIndicator<ParseTokenSimpleTensor> indicatorValue = indicator.Length == 0
            ? new TrueIndicator<ParseTokenSimpleTensor>()
            : new NamesIndicator(indicator);

        return TensorApi.Parse(
            tensor,
            new IndicesInsertion(
                ParserIndices.ParseSimple(upper),
                ParserIndices.ParseSimple(lower),
                indicatorValue));
    }

    private static void AssertIndicesParity(NRedberry.Tensors.Tensor tensor, string expectedIndices)
    {
        SimpleIndices expected = ParserIndices.ParseSimple(expectedIndices);

        Assert.True(tensor.Indices.GetFree().EqualsRegardlessOrder(expected.GetFree()));
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
