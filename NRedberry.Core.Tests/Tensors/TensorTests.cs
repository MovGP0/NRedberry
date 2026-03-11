using NRedberry.Indices;
using NRedberry.Tensors;
using Xunit;

namespace NRedberry.Core.Tests.Tensors;

public sealed class TensorTests
{
    [Fact]
    public void ShouldGetRangesAndArraysFromChildren()
    {
        TestTensor tensor = new("root", 10, new TestTensor("a", 1), new TestTensor("b", 2), new TestTensor("c", 3));

        NRedberry.Tensors.Tensor[] range = tensor.GetRange(1, 3);
        NRedberry.Tensors.Tensor[] array = tensor.ToArray();

        Assert.Equal(["b", "c"], range.Select(t => t.ToString(OutputFormat.Redberry)).ToArray());
        Assert.Equal(["a", "b", "c"], array.Select(t => t.ToString(OutputFormat.Redberry)).ToArray());
    }

    [Fact]
    public void ShouldThrowForInvalidRangeOrSetArguments()
    {
        TestTensor tensor = new("root", 10, new TestTensor("a", 1), new TestTensor("b", 2));

        Assert.Throws<IndexOutOfRangeException>(() => tensor.GetRange(1, 3));
        Assert.Throws<IndexOutOfRangeException>(() => tensor.Set(2, new TestTensor("c", 3)));
        Assert.Throws<ArgumentNullException>(() => tensor.Set(0, null!));
    }

    [Fact]
    public void ShouldUseBaseSetBuilderImplementation()
    {
        TestTensor tensor = new("root", 10, new TestTensor("a", 1), new TestTensor("b", 2));

        NRedberry.Tensors.Tensor replaced = tensor.Set(1, new TestTensor("c", 3));

        Assert.Equal("root(a,c)", replaced.ToString(OutputFormat.Redberry));
    }

    [Fact]
    public void ShouldCompareByHashCode()
    {
        TestTensor left = new("left", 1);
        TestTensor right = new("right", 5);

        Assert.True(left.CompareTo(right) < 0);
        Assert.True(right.CompareTo(left) > 0);
        Assert.Equal(1, right.CompareTo(null));
    }

    [Fact]
    public void StaticHelpersShouldGuardNullArguments()
    {
        Assert.Throws<ArgumentNullException>(() => NRedberry.Tensors.Tensor.Sum(null!));
        Assert.Throws<ArgumentNullException>(() => NRedberry.Tensors.Tensor.Expression(null!, new TestTensor("a", 1)));
        Assert.Throws<ArgumentNullException>(() => NRedberry.Tensors.Tensor.Expression(new TestTensor("a", 1), null!));
    }

    private sealed class TestTensor(string text, int hashCode, params NRedberry.Tensors.Tensor[] children) : NRedberry.Tensors.Tensor
    {
        public override Indices.Indices Indices => IndicesFactory.EmptyIndices;

        public override NRedberry.Tensors.Tensor this[int i] => children[i];

        public override int Size => children.Length;

        public override string ToString(OutputFormat outputFormat)
        {
            if (children.Length == 0)
            {
                return text;
            }

            return $"{text}({string.Join(",", children.Select(child => child.ToString(outputFormat)))})";
        }

        public override int GetHashCode()
        {
            return hashCode;
        }

        public override TensorBuilder GetBuilder()
        {
            return new TestTensorBuilder(text, hashCode);
        }

        public override TensorFactory? GetFactory()
        {
            return null;
        }
    }

    private sealed class TestTensorBuilder(string text, int hashCode) : TensorBuilder
    {
        private readonly List<NRedberry.Tensors.Tensor> _children = [];

        public void Put(NRedberry.Tensors.Tensor tensor)
        {
            _children.Add(tensor);
        }

        public NRedberry.Tensors.Tensor Build()
        {
            return new TestTensor(text, hashCode, _children.ToArray());
        }

        public TensorBuilder Clone()
        {
            TestTensorBuilder clone = new(text, hashCode);
            clone._children.AddRange(_children);
            return clone;
        }
    }
}
