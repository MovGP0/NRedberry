using NRedberry.Tensors;
using NRedberry.Transformations.Options;
using NRedberry.Transformations.Symmetrization;
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

        options.Created.ShouldBeTrue();
        options.Integer.ShouldBe(4);
        options.StringValue.ShouldBe("mapped");
        options.DefaultString.ShouldBe("default");
    }

    [Fact]
    public void ShouldBuildOptionsFromListByIndex()
    {
        BuilderTestOptions options = (BuilderTestOptions)TransformationBuilder.BuildOptionsFromList(
            typeof(BuilderTestOptions),
            [7, "listed"]);

        options.Created.ShouldBeTrue();
        options.Integer.ShouldBe(7);
        options.StringValue.ShouldBe("listed");
    }

    [Fact]
    public void ShouldCreateOptionsOnlyTransformationFromOptionsList()
    {
        OptionsOnlyTransformation transformation = TransformationBuilder.CreateTransformation<OptionsOnlyTransformation>(
            [3, "value"]);

        transformation.Options.Created.ShouldBeTrue();
        transformation.Options.Integer.ShouldBe(3);
        transformation.Options.StringValue.ShouldBe("value");
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

        transformation.Variable.ShouldBeSameAs(variable);
        transformation.Options.Integer.ShouldBe(9);
        transformation.Options.StringValue.ShouldBe("configured");
    }

    [Fact]
    public void ShouldCreateVarargTransformation()
    {
        SimpleTensor x = TensorApi.ParseSimple("x");
        SimpleTensor y = TensorApi.ParseSimple("y");
        VarargArgumentsTransformation transformation = TransformationBuilder.CreateTransformation<VarargArgumentsTransformation>(
            [x, y],
            [5, "vararg"]);

        transformation.Variables.Length.ShouldBe(2);
        transformation.Variables[0].ShouldBeSameAs(x);
        transformation.Variables[1].ShouldBeSameAs(y);
        transformation.Options.Integer.ShouldBe(5);
        transformation.Options.StringValue.ShouldBe("vararg");
    }

    [Fact]
    public void ShouldThrowWhenCreatorIsMissing()
    {
        InvalidOperationException exception = Should.Throw<InvalidOperationException>(() =>
            TransformationBuilder.CreateTransformation<NoCreatorTransformation>([]));

        exception.Message.ShouldContain("No creator constructor");
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
