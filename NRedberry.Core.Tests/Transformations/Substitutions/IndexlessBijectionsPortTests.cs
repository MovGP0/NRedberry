using NRedberry.Transformations.Substitutions;
using TensorApi = NRedberry.Tensors.Tensors;
using Xunit;

namespace NRedberry.Core.Tests.Transformations.Substitutions;

public sealed class IndexlessBijectionsPortTests
{
    [Fact]
    public void ShouldThrowWhileIndexlessPortIsUnimplemented()
    {
        Assert.Throws<NotImplementedException>(() =>
            new IndexlessBijectionsPort([TensorApi.Parse("a")], [TensorApi.Parse("b")]));
    }
}
