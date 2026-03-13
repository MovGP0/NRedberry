using System.Collections.Generic;
using System.Linq;
using NRedberry.Contexts;
using NRedberry.Indices;
using NRedberry.Parsers;
using NRedberry.Parsers.Preprocessor;
using RedberryParser = NRedberry.Parsers.Parser;
using Xunit;

namespace NRedberry.Core.Tests.Parser.Preprocessor;

public sealed class ChangeIndicesTypesAndTensorNamesTest
{
    [Fact]
    public void ShouldChangeIndexTypes()
    {
        ParseToken token = RedberryParser.Default.Parse("f_mn * (f^ma + k^ma)");
        var transformer = new ChangeIndicesTypesAndTensorNames(
            TypesAndNamesTransformer.Utils.ChangeType(IndexType.LatinLower, IndexType.LatinUpper));

        ParseToken transformed = transformer.Transform(token);
        IReadOnlyList<ParseTokenSimpleTensor> tensors = CollectSimpleTensors(transformed);

        Assert.Equal(3, tensors.Count);
        Assert.Equal(2, tensors.Count(tensor => tensor.Name == "f"));
        Assert.Equal(1, tensors.Count(tensor => tensor.Name == "k"));
        Assert.All(tensors, tensor => Assert.False(ContainsIndicesOfType(tensor.Indices, IndexType.LatinLower)));
        Assert.All(tensors, tensor => Assert.True(ContainsIndicesOfType(tensor.Indices, IndexType.LatinUpper)));
    }

    [Fact]
    public void ShouldChangeTypesAndNames()
    {
        ParseToken token = RedberryParser.Default.Parse("f_mn * (f^ma + k^ma)");
        var transformer = new ChangeIndicesTypesAndTensorNames(
            TypesAndNamesTransformer.Utils.And(
                TypesAndNamesTransformer.Utils.ChangeType(IndexType.LatinLower, IndexType.LatinUpper),
                TypesAndNamesTransformer.Utils.ChangeName(["f"], ["k"])));

        ParseToken transformed = transformer.Transform(token);
        IReadOnlyList<ParseTokenSimpleTensor> tensors = CollectSimpleTensors(transformed);

        Assert.Equal(3, tensors.Count);
        Assert.All(tensors, tensor => Assert.Equal("k", tensor.Name));
        Assert.All(tensors, tensor => Assert.False(ContainsIndicesOfType(tensor.Indices, IndexType.LatinLower)));
    }

    private static IReadOnlyList<ParseTokenSimpleTensor> CollectSimpleTensors(ParseToken node)
    {
        List<ParseTokenSimpleTensor> tensors = [];
        CollectSimpleTensors(node, tensors);
        return tensors;
    }

    private static void CollectSimpleTensors(ParseToken node, ICollection<ParseTokenSimpleTensor> tensors)
    {
        if (node is ParseTokenSimpleTensor simpleTensor)
        {
            tensors.Add(simpleTensor);
        }

        foreach (ParseToken child in node.Content)
        {
            CollectSimpleTensors(child, tensors);
        }
    }

    private static bool ContainsIndicesOfType(SimpleIndices indices, IndexType type)
    {
        for (int i = 0; i < indices.Size(); ++i)
        {
            if (IndicesUtils.GetTypeEnum(indices[i]) == type)
            {
                return true;
            }
        }

        return false;
    }
}
