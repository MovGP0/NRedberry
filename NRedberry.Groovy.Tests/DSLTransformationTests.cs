using NRedberry.Groovy;
using NRedberry.Numbers;
using Xunit;
using TensorApi = NRedberry.Tensors.Tensors;

namespace NRedberry.Groovy.Tests;

public sealed class DSLTransformationTests
{
    [Fact]
    public void ShouldConvertKnownGroovyDslValues()
    {
        object? tensor = DSLTransformation<TestTransformation>.ToObject("a+b");
        object? number = DSLTransformation<TestTransformation>.ToObject(7);
        TestTransformation transformation = new();

        Assert.Equal(TensorApi.Parse("a+b").ToString(), tensor?.ToString());
        Assert.IsType<Complex>(number);
        Assert.Same(transformation, DSLTransformation<TestTransformation>.ToObject(transformation));
    }

    [Fact]
    public void ShouldConvertListsAndDictionariesRecursively()
    {
        List<object?> values = DSLTransformation<TestTransformation>.ToList(["x", 2]);
        IDictionary<string, object?> source = new Dictionary<string, object?> { ["key"] = "y" };
        Dictionary<string, object?> map = DSLTransformation<TestTransformation>.ToDictionary(source);

        Assert.Equal(TensorApi.Parse("x").ToString(), values[0]?.ToString());
        Assert.IsType<Complex>(values[1]);
        Assert.Equal(TensorApi.Parse("y").ToString(), map["key"]?.ToString());
    }
}
