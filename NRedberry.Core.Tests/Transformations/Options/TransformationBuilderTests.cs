using NRedberry.Tensors;
using NRedberry.Transformations.Options;
using NRedberry.Transformations.Symmetrization;
using Xunit;
using TensorType = NRedberry.Tensors.Tensor;
using TensorApi = NRedberry.Tensors.Tensors;

namespace NRedberry.Core.Tests.Transformations.Options;

public sealed class TransformationBuilderTests
{
    [Fact]
    public void ShouldBuildOptionsFromMapAndTriggerCreation()
    {
        BuilderTestOptions options = TransformationBuilder.BuildOptionsFromMap<BuilderTestOptions>(
            new Dictionary<string, object?>
            {
                ["integer"] = 4,
                ["string"] = "mapped"
            });

        Assert.True(options.Created);
        Assert.Equal(4, options.Integer);
        Assert.Equal("mapped", options.StringValue);
        Assert.Equal("default", options.DefaultString);
    }

    [Fact]
    public void ShouldBuildOptionsFromListByIndex()
    {
        BuilderTestOptions options = (BuilderTestOptions)TransformationBuilder.BuildOptionsFromList(
            typeof(BuilderTestOptions),
            [7, "listed"]);

        Assert.True(options.Created);
        Assert.Equal(7, options.Integer);
        Assert.Equal("listed", options.StringValue);
    }

    [Fact]
    public void ShouldCreateOptionsOnlyTransformationFromOptionsList()
    {
        OptionsOnlyTransformation transformation = TransformationBuilder.CreateTransformation<OptionsOnlyTransformation>(
            [3, "value"]);

        Assert.True(transformation.Options.Created);
        Assert.Equal(3, transformation.Options.Integer);
        Assert.Equal("value", transformation.Options.StringValue);
    }

    [Fact]
    public void ShouldCreateTransformationWithArgumentsAndMapOptions()
    {
        SimpleTensor variable = TensorApi.ParseSimple("x");
        ArgumentsAndOptionsTransformation transformation = TransformationBuilder.CreateTransformation<ArgumentsAndOptionsTransformation>(
            [variable],
            new Dictionary<string, object?>
            {
                ["integer"] = 9,
                ["string"] = "configured"
            });

        Assert.Same(variable, transformation.Variable);
        Assert.Equal(9, transformation.Options.Integer);
        Assert.Equal("configured", transformation.Options.StringValue);
    }

    [Fact]
    public void ShouldCreateVarargTransformation()
    {
        SimpleTensor x = TensorApi.ParseSimple("x");
        SimpleTensor y = TensorApi.ParseSimple("y");
        VarargArgumentsTransformation transformation = TransformationBuilder.CreateTransformation<VarargArgumentsTransformation>(
            [x, y],
            [5, "vararg"]);

        Assert.Equal(2, transformation.Variables.Length);
        Assert.Same(x, transformation.Variables[0]);
        Assert.Same(y, transformation.Variables[1]);
        Assert.Equal(5, transformation.Options.Integer);
        Assert.Equal("vararg", transformation.Options.StringValue);
    }

    [Fact]
    public void ShouldThrowWhenCreatorIsMissing()
    {
        InvalidOperationException exception = Assert.Throws<InvalidOperationException>(() =>
            TransformationBuilder.CreateTransformation<NoCreatorTransformation>([]));

        Assert.Contains("No creator constructor", exception.Message, StringComparison.Ordinal);
    }
}

internal sealed class BuilderTestOptions : IOptions
{
    [Option(Name = "integer", Index = 0)]
    public int Integer;

    [Option(Name = "string", Index = 1)]
    public string? StringValue;

    [Option(Name = "defaultString", Index = 2)]
    public string DefaultString = "default";

    public bool Created { get; private set; }

    public void TriggerCreate()
    {
        Created = true;
    }
}

internal sealed class OptionsOnlyTransformation : ITransformation
{
    public OptionsOnlyTransformation()
    {
        throw new InvalidOperationException("Creator constructor should be used.");
    }

    [Creator]
    public OptionsOnlyTransformation([Options] BuilderTestOptions options)
    {
        Options = options;
    }

    public BuilderTestOptions Options { get; }

    public TensorType Transform(TensorType tensor)
    {
        return tensor;
    }
}

internal sealed class ArgumentsAndOptionsTransformation : ITransformation
{
    [Creator(HasArgs = true)]
    public ArgumentsAndOptionsTransformation(SimpleTensor variable, [Options] BuilderTestOptions options)
    {
        Variable = variable;
        Options = options;
    }

    public SimpleTensor Variable { get; }

    public BuilderTestOptions Options { get; }

    public TensorType Transform(TensorType tensor)
    {
        return tensor;
    }
}

internal sealed class VarargArgumentsTransformation : ITransformation
{
    [Creator(HasArgs = true, Vararg = true)]
    public VarargArgumentsTransformation(SimpleTensor[] variables, [Options] BuilderTestOptions options)
    {
        Variables = variables;
        Options = options;
    }

    public SimpleTensor[] Variables { get; }

    public BuilderTestOptions Options { get; }

    public TensorType Transform(TensorType tensor)
    {
        return tensor;
    }
}

internal sealed class NoCreatorTransformation : ITransformation
{
    public TensorType Transform(TensorType tensor)
    {
        return tensor;
    }
}
