using NRedberry.Transformations.Expand;
using TensorApi = NRedberry.Tensors.Tensors;

namespace NRedberry.Core.Tests.Transformations.Expand;

public sealed class ExpandPortTests
{
    [Fact]
    public void ShouldExpandProductUsingPort()
    {
        NRedberry.Tensors.Tensor actual = ExpandPort.ExpandUsingPort(TensorApi.Parse("(a+b)*c"));

        NormalizeSumOfProducts(actual.ToString(OutputFormat.Redberry)).ShouldBe(["a*c", "b*c"]);
    }

    [Fact]
    public void ShouldKeepSymbolicPowerWhenExpansionDisabled()
    {
        NRedberry.Tensors.Tensor tensor = TensorApi.Parse("(a+b)**2");

        NRedberry.Tensors.Tensor actual = ExpandPort.ExpandUsingPort(tensor, false);

        actual.ShouldBeSameAs(tensor);
    }

    [Fact]
    public void ShouldEnumeratePairProducts()
    {
        ExpandPort.ExpandPairPort port = new(
            (NRedberry.Tensors.Sum)TensorApi.Parse("a+b"),
            (NRedberry.Tensors.Sum)TensorApi.Parse("c+d"));

        List<string> terms = [];
        NRedberry.Tensors.Tensor current;
        while ((current = port.Take()) is not null)
        {
            terms.Add(current.ToString(OutputFormat.Redberry));
        }

        string[] normalized = terms
            .Select(NormalizeProduct)
            .OrderBy(term => term, StringComparer.Ordinal)
            .ToArray();
        normalized.ShouldBe(["a*c", "a*d", "b*c", "b*d"]);
    }

    private static string[] NormalizeSumOfProducts(string expression)
    {
        return expression
            .Split('+', StringSplitOptions.RemoveEmptyEntries)
            .Select(NormalizeProduct)
            .OrderBy(term => term, StringComparer.Ordinal)
            .ToArray();
    }

    private static string NormalizeProduct(string expression)
    {
        return string.Join(
            "*",
            expression
                .Split('*', StringSplitOptions.RemoveEmptyEntries)
                .OrderBy(factor => factor, StringComparer.Ordinal));
    }
}
