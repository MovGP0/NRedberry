using NRedberry.Tensors;
using TensorFactoryApi = NRedberry.Tensors.Tensors;
using Xunit;

namespace NRedberry.Core.Tests.Tensors;

public sealed class TensorFactoryTests
{
    [Fact]
    public void ShouldCreateTensorsThroughFactoryContract()
    {
        TensorFactory factory = new RecordingTensorFactory();
        NRedberry.Tensors.Tensor single = TensorFactoryApi.Parse("a");
        NRedberry.Tensors.Tensor other = TensorFactoryApi.Parse("b");

        Assert.Same(single, factory.Create(single));

        string text = factory.Create(single, other).ToString(OutputFormat.Redberry);
        Assert.Contains("a", text);
        Assert.Contains("b", text);
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
