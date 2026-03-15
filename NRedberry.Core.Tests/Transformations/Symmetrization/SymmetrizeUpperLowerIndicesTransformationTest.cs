using NRedberry.Core.Combinatorics;
using NRedberry.Contexts;
using NRedberry.Indices;
using NRedberry.Tensors;
using NRedberry.Transformations.Symmetrization;
using Shouldly;
using TensorApi = NRedberry.Tensors.Tensors;
using SimpleTensorType = NRedberry.Tensors.SimpleTensor;
using TensorType = NRedberry.Tensors.Tensor;
using Xunit;

namespace NRedberry.Core.Tests.Transformations.Symmetrization;

public sealed class SymmetrizeUpperLowerIndicesTransformationTest
{
    [Fact]
    public void ShouldSymmetrizeLowerIndices()
    {
        TensorType actual = SymmetrizeUpperLowerIndicesITransformation.SymmetrizeUpperLowerIndices(
            TensorApi.Parse("g_mn*g_ab"));

        ShouldHaveCanonicalForm("g_mn*g_ab+g_ma*g_nb+g_mb*g_an", actual);
    }

    [Fact]
    public void ShouldLeaveUpperIndicesUntouched()
    {
        TensorType actual = SymmetrizeUpperLowerIndicesITransformation.SymmetrizeUpperLowerIndices(
            TensorApi.Parse("g_mn*g^ab"));

        ShouldHaveCanonicalForm("g_mn*g^ab", actual);
    }

    [Fact]
    public void ShouldSymmetrizeMixedTensor()
    {
        TensorType actual = SymmetrizeUpperLowerIndicesITransformation.SymmetrizeUpperLowerIndices(
            TensorApi.Parse("g_ab*g^rs*g^pq*g_mn"));

        TensorType expected = TensorApi.Parse(
            "g_{mn}*g_{ab}*g^{pq}*g^{rs}+g_{mn}*g_{ab}*g^{pr}*g^{qs}+g_{mn}*g_{ab}*g^{ps}*g^{qr}+g_{bn}*g_{am}*g^{pq}*g^{rs}+g_{bn}*g_{am}*g^{pr}*g^{qs}+g_{bn}*g_{am}*g^{ps}*g^{qr}+g_{bm}*g_{an}*g^{pq}*g^{rs}+g_{bm}*g_{an}*g^{pr}*g^{qs}+g_{bm}*g_{an}*g^{ps}*g^{qr}");

        ShouldHaveCanonicalForm(expected, actual);
    }

    [Fact]
    public void ShouldMultiplyBySymmetryFactorWhenRequested()
    {
        TensorType actual = SymmetrizeUpperLowerIndicesITransformation.SymmetrizeUpperLowerIndices(
            TensorApi.Parse("d_i^a*d_j^b*d_k^c"),
            true);

        TensorType expected = TensorApi.Parse(
            "(d_i^a*d_j^b*d_k^c+d_i^a*d_j^c*d_k^b+d_i^b*d_j^a*d_k^c+d_i^b*d_j^c*d_k^a+d_i^c*d_j^a*d_k^b+d_i^c*d_j^b*d_k^a)/6");

        ShouldHaveCanonicalForm(expected, actual);
    }

    private static void ShouldHaveCanonicalForm(string expected, TensorType actual)
    {
        ShouldHaveCanonicalForm(TensorApi.Parse(expected), actual);
    }

    private static void ShouldHaveCanonicalForm(TensorType expected, TensorType actual)
    {
        Canonicalize(expected).ShouldBe(Canonicalize(actual));
    }

    private static string Canonicalize(TensorType tensor)
    {
        if (tensor is Sum sum)
        {
            string[] terms = new string[sum.Size];
            for (int i = 0; i < sum.Size; ++i)
            {
                terms[i] = Canonicalize(sum[i]);
            }

            Array.Sort(terms, StringComparer.Ordinal);
            return string.Join(" + ", terms);
        }

        if (tensor is Product product)
        {
            string[] factors = new string[product.Size];
            for (int i = 0; i < product.Size; ++i)
            {
                factors[i] = Canonicalize(product[i]);
            }

            Array.Sort(factors, StringComparer.Ordinal);
            return string.Join("*", factors);
        }

        if (tensor is SimpleTensor simpleTensor)
        {
            return CanonicalizeSimpleTensor(simpleTensor).ToString(OutputFormat.Redberry);
        }

        return tensor.ToString(OutputFormat.Redberry);
    }

    private static SimpleTensorType CanonicalizeSimpleTensor(SimpleTensorType tensor)
    {
        if (tensor.SimpleIndices.Size() != 2 || !HasSymmetricSwap(tensor))
        {
            return tensor;
        }

        int first = tensor.SimpleIndices[0];
        int second = tensor.SimpleIndices[1];
        if (first <= second)
        {
            return tensor;
        }

        return NRedberry.Tensors.Tensor.SimpleTensor(
            tensor.Name,
            IndicesFactory.CreateSimple(null, second, first));
    }

    private static bool HasSymmetricSwap(SimpleTensorType tensor)
    {
        foreach (Symmetry basis in tensor.SimpleIndices.Symmetries.Basis)
        {
            if (!basis.IsAntisymmetry
                && basis.NewIndexOf(0) == 1
                && basis.NewIndexOf(1) == 0)
            {
                return true;
            }
        }

        return false;
    }
}
