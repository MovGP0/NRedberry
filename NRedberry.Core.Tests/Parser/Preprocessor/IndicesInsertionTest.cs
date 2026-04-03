using NRedberry.Core.Utils;
using NRedberry.Indices;
using NRedberry.Parsers;
using NRedberry.Parsers.Preprocessor;
using NRedberry.Tensors;
using TensorApi = NRedberry.Tensors.Tensors;
using TensorType = NRedberry.Tensors.Tensor;

namespace NRedberry.Core.Tests.Parser.Preprocessor;

public sealed class IndicesInsertionTest
{
    [Fact]
    public void ShouldInsertIndicesForMultipleFactors()
    {
        TensorType tensor = Parse("A*B*C", "^i", "_j", "A", "B", "C");

        ShouldHaveIndicesParity(tensor, "^i_j");
    }

    [Fact]
    public void ShouldInsertIndicesInNestedExpression()
    {
        TensorType tensor = Parse("A*(B*A+C*K)*F", "^i", "_j");

        ShouldHaveIndicesParity(tensor, "^i_j");
        tensor.ToString(OutputFormat.Redberry).ShouldContain("A");
        tensor.ToString(OutputFormat.Redberry).ShouldContain("F");
    }

    [Fact]
    public void ShouldInsertIndicesWithExistingGreekIndices()
    {
        TensorType tensor = Parse("A^{\\alpha n}*B*C", "^ij", "_pq");

        ShouldHaveIndicesParity(tensor, "^{\\alpha n i j}_pq");
    }

    [Fact]
    public void ShouldInsertIndicesWithSelectedNamesOnly()
    {
        TensorType tensor = Parse("a*b*A*c*B*C", "^ij", "_pq", "a", "b", "c", "d");

        ShouldHaveIndicesParity(tensor, "^ij_pq");
    }

    [Fact]
    public void ShouldInsertIndicesIntoSumsWithSelectedNames()
    {
        TensorType tensor = Parse("a*(b+a)*A*(c+d)*B*C", "^ij", "_pq", "a", "b", "c", "d");
        string actual = tensor.ToString(OutputFormat.Redberry);

        actual.ShouldContain("a^{ij}");
        actual.ShouldContain("A");
    }

    [Fact]
    public void ShouldInsertIndicesWhenNamesNotSelected()
    {
        TensorType tensor = Parse("a*(b+a)*A*(c+d)*B*C", "^ij", "_pq", "A", "B", "C", "F");

        ShouldHaveIndicesParity(tensor, "^ij_pq");
    }

    [Fact]
    public void ShouldInsertIndicesInNestedSumAndProduct()
    {
        TensorType tensor = Parse("A*(B+E*(R+K*U))", "^i", "_j");

        ShouldHaveIndicesParity(tensor, "^i_j");
        tensor.ToString(OutputFormat.Redberry).ShouldContain("A^{i}");
    }

    [Fact]
    public void ShouldInsertIndicesRepeatedlyWithResetNames()
    {
        for (int i = 0; i < 5; ++i)
        {
            CC.ResetTensorNames();
            TensorType tensor = Parse("A+B", "^i", "_i", "A");

            ShouldHaveIndicesParity(tensor, "^i_i");
        }
    }

    [Fact]
    public void ShouldInsertIndicesInExpressionWithSumAndProduct()
    {
        TensorType tensor = Parse("a*A*B+((1/2)*a+b)*A*(A+B*(A+X*A))*c", "^a", "_b", "A", "B");

        ShouldHaveIndicesParity(tensor, "^a_b");
    }

    [Fact]
    public void ShouldInsertIndicesInEquation()
    {
        TensorType tensor = Parse("A=2*B+A*B", "^i", "_j");

        ShouldHaveIndicesParity(tensor, "^i_j");
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

        actualText.ShouldContain("DELTA");
        actualText.ShouldContain("HATK");
        actualText.ShouldContain("_{a}");
    }

    [Fact]
    public void ShouldInsertIndicesInSimpleSum()
    {
        TensorType actual = Parse("A+B", "^i", "_i", "A");

        string actualText = actual.ToString(OutputFormat.Redberry);

        actualText.ShouldContain("A^{i}_{i}");
        actualText.ShouldContain("B");
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

    private static void ShouldHaveIndicesParity(NRedberry.Tensors.Tensor tensor, string expectedIndices)
    {
        SimpleIndices expected = ParserIndices.ParseSimple(expectedIndices);

        tensor.Indices.GetFree().EqualsRegardlessOrder(expected.GetFree()).ShouldBeTrue();
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
