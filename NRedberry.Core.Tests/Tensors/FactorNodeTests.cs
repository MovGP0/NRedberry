using NRedberry.Indices;
using NRedberry.Numbers;
using NRedberry.Tensors;
using TensorType = NRedberry.Tensors.Tensor;

namespace NRedberry.Core.Tests.Tensors;

public sealed class FactorNodeTests
{
    [Fact]
    public void ShouldThrowWhenFactorIsNull()
    {
        CaptureBuilder builder = new();

        Should.Throw<ArgumentNullException>(() => new FactorNode(null!, builder));
    }

    [Fact]
    public void ShouldThrowWhenBuilderIsNull()
    {
        SimpleTensor factor = CreateTensor(1, 1);

        Should.Throw<ArgumentNullException>(() => new FactorNode(factor, null!));
    }

    [Fact]
    public void ShouldExposeFactorAndForbiddenIndices()
    {
        SimpleTensor factor = CreateTensor(1, 1);
        FactorNode node = new(factor, new CaptureBuilder());

        node.Factor.ShouldBeSameAs(factor);
        node.FactorForbiddenIndices.ShouldBe([NameWithType(1)]);
    }

    [Fact]
    public void ShouldRenameDummyWhenPuttingSingleTensor()
    {
        CaptureBuilder builder = new();
        FactorNode node = new(CreateTensor(1, 1), builder);

        node.Put(CreateTensor(2, 1));

        builder.Items.ShouldHaveSingleItem();
        TensorUtils.GetAllDummyIndicesT(builder.Items[0]).ShouldNotContain(NameWithType(1));
    }

    [Fact]
    public void ShouldUseAllowedDummyIndicesWhenPuttingSummandAndFactor()
    {
        CaptureBuilder builder = new();
        FactorNode node = new(CreateTensor(1, 1), builder);
        SimpleTensor summand = CreateTensor(2, 1);
        SimpleTensor factor = CreateTensor(3, 2);

        node.Put(summand, factor);

        builder.Items.ShouldHaveSingleItem();
        builder.Items[0].Indices.AllIndices.ToArray().ShouldBe([Lower(2), Upper(2)]);
    }

    [Fact]
    public void ShouldDelegateBuildToInnerBuilder()
    {
        CaptureBuilder builder = new();
        FactorNode node = new(CreateTensor(1, 1), builder);
        builder.Result = Complex.Two;

        TensorType result = node.Build();

        result.ShouldBeSameAs(Complex.Two);
    }

    [Fact]
    public void ShouldCloneWithoutSharingBuilderState()
    {
        CaptureBuilder builder = new();
        FactorNode node = new(CreateTensor(1, 1), builder);

        FactorNode clone = node.Clone();
        clone.Put(CreateTensor(2, 2));

        builder.Items.ShouldBeEmpty();
        clone.ShouldNotBeSameAs(node);
        clone.Factor.ShouldBeSameAs(node.Factor);
        clone.FactorForbiddenIndices.ShouldBe(node.FactorForbiddenIndices);
        clone.Build().ShouldBeOfType<SimpleTensor>();
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
