using NRedberry.Numbers;
using Shouldly;
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

        tensor?.ToString().ShouldBe(TensorApi.Parse("a+b").ToString());
        number.ShouldBeOfType<Complex>();
        DSLTransformation<TestTransformation>.ToObject(transformation).ShouldBeSameAs(transformation);
    }

    [Fact]
    public void ShouldConvertListsAndDictionariesRecursively()
    {
        List<object?> values = DSLTransformation<TestTransformation>.ToList(["x", 2]);
        IDictionary<string, object?> source = new Dictionary<string, object?> { ["key"] = "y" };
        Dictionary<string, object?> map = DSLTransformation<TestTransformation>.ToDictionary(source);

        values[0]?.ToString().ShouldBe(TensorApi.Parse("x").ToString());
        values[1].ShouldBeOfType<Complex>();
        map["key"]?.ToString().ShouldBe(TensorApi.Parse("y").ToString());
    }
}
