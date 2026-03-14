using NRedberry.Contexts;
using NRedberry.Indices;
using NRedberry.Parsers;
using NRedberry.Parsers.Preprocessor;
using RedberryParser = NRedberry.Parsers.Parser;
using Xunit;

namespace NRedberry.Core.Tests.Parsers.Preprocessor;

public sealed class ChangeIndicesTypesAndTensorNamesTests
{
    [Fact]
    public void ShouldThrowWhenConstructedWithNullTransformer()
    {
        var exception = Should.Throw<ArgumentNullException>(() => new ChangeIndicesTypesAndTensorNames(null!));

        exception.ParamName.ShouldBe("transformer");
    }

    [Fact]
    public void ShouldThrowWhenTransformingNullNode()
    {
        var transformer = new ChangeIndicesTypesAndTensorNames(
            TypesAndNamesTransformer.Utils.ChangeType(IndexType.LatinLower, IndexType.LatinUpper));

        var exception = Should.Throw<ArgumentNullException>(() => transformer.Transform(null!));

        exception.ParamName.ShouldBe("node");
    }

    [Fact]
    public void ShouldChangeIndexTypes()
    {
        var token = RedberryParser.Default.Parse("f_mn * (f^ma + k^ma)");
        var transformer = new ChangeIndicesTypesAndTensorNames(
            TypesAndNamesTransformer.Utils.ChangeType(IndexType.LatinLower, IndexType.LatinUpper));

        var transformed = transformer.Transform(token);
        IReadOnlyList<ParseTokenSimpleTensor> tensors = CollectSimpleTensors(transformed);

        tensors.Count.ShouldBe(3);
        tensors.Count(tensor => tensor.Name == "f").ShouldBe(2);
        tensors.Count(tensor => tensor.Name == "k").ShouldBe(1);
        foreach (ParseTokenSimpleTensor tensor in tensors)
        {
            ContainsIndicesOfType(tensor.Indices, IndexType.LatinLower).ShouldBeFalse();
            ContainsIndicesOfType(tensor.Indices, IndexType.LatinUpper).ShouldBeTrue();
        }
    }

    [Fact]
    public void ShouldChangeTypesAndNames()
    {
        var token = RedberryParser.Default.Parse("f_mn * (f^ma + k^ma)");
        var transformer = new ChangeIndicesTypesAndTensorNames(
            TypesAndNamesTransformer.Utils.And(
                TypesAndNamesTransformer.Utils.ChangeType(IndexType.LatinLower, IndexType.LatinUpper),
                TypesAndNamesTransformer.Utils.ChangeName(["f"], ["k"])));

        var transformed = transformer.Transform(token);
        IReadOnlyList<ParseTokenSimpleTensor> tensors = CollectSimpleTensors(transformed);

        tensors.Count.ShouldBe(3);
        foreach (ParseTokenSimpleTensor tensor in tensors)
        {
            tensor.Name.ShouldBe("k");
            ContainsIndicesOfType(tensor.Indices, IndexType.LatinLower).ShouldBeFalse();
        }
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
