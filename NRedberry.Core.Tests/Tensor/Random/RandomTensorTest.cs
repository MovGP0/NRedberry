using NRedberry.Indices;
using NRedberry.Parsers;
using NRedberry.Tensors;
using NRedberry.Transformations.Symmetrization;
using Xunit;
using RandomTensorGenerator = NRedberry.Tensors.Random.RandomTensor;
using RedberryContext = NRedberry.Contexts.Context;
using ContextCC = NRedberry.Contexts.CC;
using IndicesType = NRedberry.Indices.Indices;
using TensorType = NRedberry.Tensors.Tensor;

namespace NRedberry.Core.Tests.Tensor.Random;

public sealed class RandomTensorTest
{
    [Fact]
    public void ShouldGenerateProductWithFreeIndices()
    {
        RandomTensorGenerator random = new(
            4,
            10,
            [4, 0, 0, 0],
            [10, 0, 0, 0],
            false,
            true,
            12345L);

        TensorType tensor = random.NextProduct(4, ParserIndices.ParseSimple("_nm"));

        Assert.True(tensor.Indices.GetFree().EqualsRegardlessOrder(ParserIndices.ParseSimple("_nm")));
        TensorUtils.AssertIndicesConsistency(tensor);
    }

    [Fact]
    public void ShouldGenerateSumWithFreeIndices()
    {
        RandomTensorGenerator random = new(
            4,
            10,
            [4, 0, 0, 0],
            [10, 0, 0, 0],
            false,
            true,
            12345L);

        TensorType tensor = random.NextSum(5, 4, ParserIndices.ParseSimple("_nm"));

        Assert.True(tensor.Indices.EqualsRegardlessOrder(ParserIndices.ParseSimple("_nm")));
        TensorUtils.AssertIndicesConsistency(tensor);
    }

    [Fact]
    public void ShouldGenerateProductsWithStableFreeIndices()
    {
        for (int j = 0; j < 5; j++)
        {
            ContextCC.ResetTensorNames();
            RandomTensorGenerator random = new(
                5,
                20,
                [2, 0, 0, 0],
                [10, 0, 0, 0],
                true,
                true,
                1000L + j);

            for (int i = 0; i < 10; i++)
            {
                TensorType tensor = random.NextProduct(5, ParserIndices.ParseSimple("_mnab^cd"));

                Assert.True(tensor.Indices.GetFree().EqualsRegardlessOrder(ParserIndices.ParseSimple("_mnab^cd")));
                TensorUtils.AssertIndicesConsistency(tensor);
            }
        }
    }

    [Fact]
    public void ShouldHandleNullPointerSeedScenario()
    {
        ContextCC.ResetTensorNames(2312);
        RandomTensorGenerator random = new(
            5,
            6,
            [2, 0, 0, 0],
            [3, 0, 0, 0],
            true,
            true,
            7643543L);

        random.NextSum(5, 2, ParserIndices.ParseSimple("_mn"));
        random.NextSum(5, 2, ParserIndices.ParseSimple("^mn"));
    }

    [Fact]
    public void ShouldGenerateMetricTensors()
    {
        RandomTensorGenerator random = new(0, 0, [2, 0, 0, 0], [3, 0, 0, 0], true, true, 77L);
        random.AddToNamespace(CreateMetric("_mn"));

        Assert.True(RedberryContext.Get().IsKroneckerOrMetric(random.NextSimpleTensor()));

        for (int i = 0; i < 10; i++)
        {
            TensorType tensor = random.NextProduct(i + 2, ParserIndices.ParseSimple("_ab"));
            TensorUtils.AssertIndicesConsistency(tensor);
            tensor = EliminateMetricsTransformation.Eliminate(tensor);
            TensorUtils.AssertIndicesConsistency(tensor);
            Assert.True(tensor.Indices.GetFree().EqualsRegardlessOrder(ParserIndices.ParseSimple("_ab")));
        }
    }

    [Fact]
    public void ShouldGenerateTensorTreesWithConsistentIndices()
    {
        RandomTensorGenerator random = new(0, 0, [1, 0, 0, 0], [3, 0, 0, 0], true, true, 88L);
        random.AddToNamespace(CreateSimpleTensor("f", "_n"), CreateMetric("_mn"));

        for (int i = 0; i < 25; i++)
        {
            TensorType tensor = random.NextTensorTree(
                RandomTensorGenerator.TensorType.Product,
                5,
                2,
                2,
                ParserIndices.ParseSimple("_abc"));

            Assert.True(tensor.Indices.GetFree().EqualsRegardlessOrder(ParserIndices.ParseSimple("_abc")));
            TensorUtils.AssertIndicesConsistency(tensor);
        }
    }

    [Fact]
    public void ShouldPreserveConsistencyThroughTransformations()
    {
        for (int i = 0; i < 25; i++)
        {
            ContextCC.ResetTensorNames();
            RandomTensorGenerator random = new(0, 0, [1, 0, 0, 0], [3, 0, 0, 0], true, true, 500L + i);
            random.AddToNamespace(CreateSimpleTensor("f", "_n"), CreateMetric("_mn"));

            TensorType tensor = random.NextTensorTree(
                RandomTensorGenerator.TensorType.Product,
                5,
                2,
                2,
                ParserIndices.ParseSimple("_abc"));

            TensorUtils.AssertIndicesConsistency(tensor);

            tensor = EliminateMetricsTransformation.Eliminate(tensor);
            TensorUtils.AssertIndicesConsistency(tensor);

            tensor = EliminateMetricsTransformation.Instance.Transform(tensor);
            TensorUtils.AssertIndicesConsistency(tensor);

            tensor = EliminateMetricsTransformation.Eliminate(tensor);
            TensorUtils.AssertIndicesConsistency(tensor);
        }
    }

    [Fact]
    public void ShouldThrowWhenGeneratingWithMissingDescriptors()
    {
        RandomTensorGenerator random = new(false);
        random.ClearNamespace();
        random.AddToNamespace(CreateSimpleTensor("T", "_abcd"), CreateSimpleTensor("T", "_ab"));

        Assert.Throws<ArgumentException>(() => random.NextProduct(2, ParserIndices.ParseSimple("_abc")));
    }

    [Fact]
    public void ShouldCreateIndicesFromStructure()
    {
        RandomTensorGenerator random = new(false);
        SimpleIndices template = ParserIndices.ParseSimple("^a'_b'");

        IndicesType created = IndicesFactory.Create(random.NextIndices(template.StructureOfIndices));

        StructureOfIndices actual = StructureOfIndices.Create(IndicesFactory.CreateSimple(null, created));

        Assert.Equal(template.StructureOfIndices.ToString(), actual.ToString());
    }

    private static global::NRedberry.Tensors.SimpleTensor CreateSimpleTensor(string name, string indices)
    {
        return global::NRedberry.Tensors.Tensor.SimpleTensor(name, ParserIndices.ParseSimple(indices));
    }

    private static global::NRedberry.Tensors.SimpleTensor CreateMetric(string indices)
    {
        return global::NRedberry.Tensors.Tensor.SimpleTensor(RedberryContext.Get().MetricName, ParserIndices.ParseSimple(indices));
    }
}
