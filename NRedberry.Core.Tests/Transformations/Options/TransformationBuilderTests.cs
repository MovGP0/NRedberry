using NRedberry.Transformations.Fractions;
using NRedberry.Transformations.Options;
using Xunit;

namespace NRedberry.Core.Tests.Transformations.Options;

public sealed class TransformationBuilderTests
{
    [Fact]
    public void ShouldThrowForMapBasedOptionBinding()
    {
        Assert.Throws<NotImplementedException>(() =>
            TransformationBuilder.BuildOptionsFromMap<object>(new Dictionary<string, object?>()));
        Assert.Throws<NotImplementedException>(() =>
            TransformationBuilder.BuildOptionsFromMap(typeof(object), new Dictionary<string, object?>()));
    }

    [Fact]
    public void ShouldThrowForListBasedOptionBinding()
    {
        Assert.Throws<NotImplementedException>(() =>
            TransformationBuilder.BuildOptionsFromList<object>(new List<object?>()));
        Assert.Throws<NotImplementedException>(() =>
            TransformationBuilder.BuildOptionsFromList(typeof(object), new List<object?>()));
    }

    [Fact]
    public void ShouldThrowForTransformationCreation()
    {
        Assert.Throws<NotImplementedException>(() =>
            TransformationBuilder.CreateTransformation<GetNumeratorTransformation>(
                Array.Empty<object?>(),
                new Dictionary<string, object?>()));
        Assert.Throws<NotImplementedException>(() =>
            TransformationBuilder.CreateTransformation<GetNumeratorTransformation>(Array.Empty<object?>()));
        Assert.Throws<NotImplementedException>(() =>
            TransformationBuilder.CreateTransformation<GetNumeratorTransformation>(
                Array.Empty<object?>(),
                new List<object?>()));
    }
}
