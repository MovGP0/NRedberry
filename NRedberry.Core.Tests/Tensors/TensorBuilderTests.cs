using NRedberry.Tensors;
using TensorFactory = NRedberry.Tensors.Tensors;
using Xunit;

namespace NRedberry.Core.Tests.Tensors;

public sealed class TensorBuilderTests
{
    [Fact]
    public void ShouldAllowPuttingBuildingAndCloningThroughInterface()
    {
        TensorBuilder builder = new RecordingTensorBuilder();
        builder.Put(TensorFactory.Parse("a"));

        TensorBuilder clone = builder.Clone();
        clone.Put(TensorFactory.Parse("b"));

        builder.Build().ToString(OutputFormat.Redberry).ShouldBe("a");

        string cloneText = clone.Build().ToString(OutputFormat.Redberry);
        cloneText.ShouldContain("a");
        cloneText.ShouldContain("b");
    }

    private sealed class RecordingTensorBuilder : TensorBuilder
    {
        private readonly List<NRedberry.Tensors.Tensor> _items = [];

        public void Put(NRedberry.Tensors.Tensor tensor)
        {
            _items.Add(tensor);
        }

        public NRedberry.Tensors.Tensor Build()
        {
            return _items.Count switch
            {
                0 => throw new InvalidOperationException("No tensors recorded."),
                1 => _items[0],
                _ => TensorExtensions.Sum(_items.ToArray())
            };
        }

        public TensorBuilder Clone()
        {
            RecordingTensorBuilder clone = new();
            clone._items.AddRange(_items);
            return clone;
        }
    }
}
