using NRedberry.Indices;
using NRedberry.Numbers;
using NRedberry.Tensors;
using TensorType = NRedberry.Tensors.Tensor;
using Xunit;

namespace NRedberry.Core.Tests.Tensors;

public sealed class FactorNodeTests
{
    [Fact]
    public void ShouldThrowWhenFactorIsNull()
    {
        CaptureBuilder builder = new();

        Assert.Throws<ArgumentNullException>(() => new FactorNode(null!, builder));
    }

    [Fact]
    public void ShouldThrowWhenBuilderIsNull()
    {
        SimpleTensor factor = CreateTensor(1, 1);

        Assert.Throws<ArgumentNullException>(() => new FactorNode(factor, null!));
    }

    [Fact]
    public void ShouldExposeFactorAndForbiddenIndices()
    {
        SimpleTensor factor = CreateTensor(1, 1);
        FactorNode node = new(factor, new CaptureBuilder());

        Assert.Same(factor, node.Factor);
        Assert.Equal([NameWithType(1)], node.FactorForbiddenIndices);
    }

    [Fact]
    public void ShouldRenameDummyWhenPuttingSingleTensor()
    {
        CaptureBuilder builder = new();
        FactorNode node = new(CreateTensor(1, 1), builder);

        node.Put(CreateTensor(2, 1));

        Assert.Single(builder.Items);
        Assert.DoesNotContain(NameWithType(1), TensorUtils.GetAllDummyIndicesT(builder.Items[0]));
    }

    [Fact]
    public void ShouldUseAllowedDummyIndicesWhenPuttingSummandAndFactor()
    {
        CaptureBuilder builder = new();
        FactorNode node = new(CreateTensor(1, 1), builder);
        SimpleTensor summand = CreateTensor(2, 1);
        SimpleTensor factor = CreateTensor(3, 2);

        node.Put(summand, factor);

        Assert.Single(builder.Items);
        Assert.Equal([Lower(2), Upper(2)], builder.Items[0].Indices.AllIndices.ToArray());
    }

    [Fact]
    public void ShouldDelegateBuildToInnerBuilder()
    {
        CaptureBuilder builder = new();
        FactorNode node = new(CreateTensor(1, 1), builder);
        builder.Result = Complex.Two;

        TensorType result = node.Build();

        Assert.Same(Complex.Two, result);
    }

    [Fact]
    public void ShouldCloneWithoutSharingBuilderState()
    {
        CaptureBuilder builder = new();
        FactorNode node = new(CreateTensor(1, 1), builder);

        FactorNode clone = node.Clone();
        clone.Put(CreateTensor(2, 2));

        Assert.Empty(builder.Items);
        Assert.NotSame(node, clone);
        Assert.Same(node.Factor, clone.Factor);
        Assert.Equal(node.FactorForbiddenIndices, clone.FactorForbiddenIndices);
        Assert.IsType<SimpleTensor>(clone.Build());
    }

    private static SimpleTensor CreateTensor(int name, int dummyName)
    {
        return new SimpleTensor(name, IndicesFactory.CreateSimple(null, Lower(dummyName), Upper(dummyName)));
    }

    private static int Lower(int name)
    {
        return IndicesUtils.CreateIndex(name, (byte)0, false);
    }

    private static int Upper(int name)
    {
        return IndicesUtils.CreateIndex(name, (byte)0, true);
    }

    private static int NameWithType(int name)
    {
        return IndicesUtils.GetNameWithType(Lower(name));
    }

    private sealed class CaptureBuilder : TensorBuilder
    {
        public List<TensorType> Items { get; } = [];

        public TensorType Result { get; set; } = Complex.Zero;

        public TensorType Build()
        {
            return Items.Count == 0 ? Result : Items[^1];
        }

        public void Put(TensorType tensor)
        {
            Items.Add(tensor);
        }

        public TensorBuilder Clone()
        {
            CaptureBuilder clone = new()
            {
                Result = Result
            };

            foreach (TensorType item in Items)
            {
                clone.Items.Add(item);
            }

            return clone;
        }
    }
}
