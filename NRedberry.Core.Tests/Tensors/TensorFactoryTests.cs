using NRedberry.Tensors;
using TensorFactoryApi = NRedberry.Tensors.Tensors;

namespace NRedberry.Core.Tests.Tensors;

public sealed class TensorFactoryTests
{
    [Fact]
    public void ShouldCreateTensorsThroughFactoryContract()
    {
        TensorFactory factory = new RecordingTensorFactory();
        NRedberry.Tensors.Tensor single = TensorFactoryApi.Parse("a");
        NRedberry.Tensors.Tensor other = TensorFactoryApi.Parse("b");

        factory.Create(single).ShouldBeSameAs(single);

        string text = factory.Create(single, other).ToString(OutputFormat.Redberry);
        text.ShouldContain("a");
        text.ShouldContain("b");
    }

    private sealed class RecordingTensorFactory : TensorFactory
    {
        public NRedberry.Tensors.Tensor Create(params NRedberry.Tensors.Tensor[] tensors)
        {
            return tensors.Length switch
            {
                0 => throw new InvalidOperationException("No tensors supplied."),
                1 => tensors[0],
                _ => TensorExtensions.Sum(tensors)
            };
        }
    }
}
