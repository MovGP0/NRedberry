using System;
using System.Linq;
using NRedberry.Core.Utils;
using NRedberry.Transformations.Powerexpand;
using NRedberry.Transformations.Symmetrization;
using TensorApi = NRedberry.Tensors.Tensors;
using Xunit;

namespace NRedberry.Core.Tests.Transformations.Powerexpand;

public sealed class PowerExpandTransformationTests
{
    [Fact]
    public void ShouldExpandAllFactors()
    {
        NRedberry.Tensors.Tensor tensor = Pow("a*b*c", "d");

        NRedberry.Tensors.Tensor actual = PowerExpandTransformation.Instance.Transform(tensor);

        AssertTensorEquals(
            TensorApi.Multiply(Pow("a", "d"), Pow("b", "d"), Pow("c", "d")),
            actual);
    }

    [Fact]
    public void ShouldExpandSelectedVariableOnly()
    {
        NRedberry.Tensors.Tensor tensor = Pow("a*b*c", "d");
        PowerExpandTransformation transformation = new(TensorApi.ParseSimple("a"));

        NRedberry.Tensors.Tensor actual = transformation.Transform(tensor);

        AssertTensorEquals(
            TensorApi.Multiply(Pow("a", "d"), Pow("b*c", "d")),
            actual);
    }

    [Fact]
    public void ShouldExpandIntoChainForCustomIndicator()
    {
        NRedberry.Tensors.Power power = Assert.IsType<NRedberry.Tensors.Power>(Pow("a*b*c", "d"));
        IIndicator<NRedberry.Tensors.Tensor> indicator = new NamedSimpleTensorIndicator(TensorApi.ParseSimple("a"));

        NRedberry.Tensors.Tensor actual = TensorApi.Multiply(
            PowerExpandUtils.PowerExpandIntoChainToArray(power, Array.Empty<int>(), indicator));

        AssertTensorEquals(
            TensorApi.Multiply(Pow("a", "d"), Pow("b*c", "d")),
            actual);
    }

    [Fact]
    public void ShouldExpandToArray()
    {
        NRedberry.Tensors.Power power = Assert.IsType<NRedberry.Tensors.Power>(Pow("a*b*c", "d"));

        string[] actual = PowerExpandUtils.PowerExpandToArray(power)
            .Select(static tensor => tensor.ToString())
            .OrderBy(static tensor => tensor, StringComparer.Ordinal)
            .ToArray();

        string[] expected =
        [
            TensorApi.Parse("a**d").ToString(),
            TensorApi.Parse("b**d").ToString(),
            TensorApi.Parse("c**d").ToString()
        ];
        Array.Sort(expected, StringComparer.Ordinal);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ShouldExpandToArrayWithVariables()
    {
        NRedberry.Tensors.Power power = Assert.IsType<NRedberry.Tensors.Power>(Pow("a*b*c", "d"));

        NRedberry.Tensors.Tensor actual = TensorApi.Multiply(
            PowerExpandUtils.PowerExpandToArray(power, TensorApi.ParseSimple("a")));

        AssertTensorEquals(
            TensorApi.Multiply(Pow("a", "d"), Pow("b*c", "d")),
            actual);
    }

    [Fact]
    public void ShouldExpandNestedPowerForSingleVariable()
    {
        NRedberry.Tensors.Tensor tensor = Pow(
            TensorApi.Multiply(Pow("a", "e"), Parse("b"), Parse("c")),
            Parse("d"));
        PowerExpandTransformation transformation = new(TensorApi.ParseSimple("a"));

        NRedberry.Tensors.Tensor actual = transformation.Transform(tensor);

        AssertTensorEquals(
            TensorApi.Multiply(Pow("a", "e*d"), Pow("b*c", "d")),
            actual);
    }

    [Fact]
    public void ShouldExpandNestedPowerAcrossProduct()
    {
        NRedberry.Tensors.Tensor tensor = Pow(
            TensorApi.Multiply(
                Pow(TensorApi.Multiply(Pow("a", "r"), Parse("g")), Parse("e")),
                Parse("b"),
                Parse("c")),
            Parse("d"));
        PowerExpandTransformation transformation = new(TensorApi.ParseSimple("a"));

        NRedberry.Tensors.Tensor actual = transformation.Transform(tensor);

        AssertTensorEquals(
            TensorApi.Multiply(
                Pow("a", "r*e*d"),
                Pow(TensorApi.Multiply(Pow("g", "e"), Parse("b"), Parse("c")), Parse("d"))),
            actual);
    }

    [Fact]
    public void ShouldExpandMultipleVariablesAcrossNestedPower()
    {
        NRedberry.Tensors.Tensor tensor = Pow(
            TensorApi.Multiply(
                Pow(TensorApi.Multiply(Pow("a", "r"), Parse("g")), Parse("e")),
                Parse("b"),
                Parse("c")),
            Parse("d"));
        PowerExpandTransformation transformation = new(
            TensorApi.ParseSimple("a"),
            TensorApi.ParseSimple("g"));

        NRedberry.Tensors.Tensor actual = transformation.Transform(tensor);

        AssertTensorEquals(
            TensorApi.Multiply(Pow("a", "r*e*d"), Pow("g", "e*d"), Pow("b*c", "d")),
            actual);
    }

    [Fact]
    public void ShouldExpandInsideLargerExpression()
    {
        NRedberry.Tensors.Tensor tensor = TensorApi.Sum(
            TensorApi.Multiply(
                Pow(
                    TensorApi.Multiply(
                        Pow(TensorApi.Multiply(Pow("a", "r"), Parse("g")), Parse("e")),
                        Parse("b"),
                        Parse("c")),
                    Parse("d")),
                Parse("a+b")),
            Parse("x"));
        PowerExpandTransformation transformation = new(
            TensorApi.ParseSimple("a"),
            TensorApi.ParseSimple("c"));

        NRedberry.Tensors.Tensor actual = transformation.Transform(tensor);

        AssertTensorEquals(
            TensorApi.Sum(
                TensorApi.Multiply(
                    Pow("a", "r*e*d"),
                    Pow("c", "d"),
                    Pow(TensorApi.Multiply(Pow("g", "e"), Parse("b")), Parse("d")),
                    Parse("a+b")),
                Parse("x")),
            actual);
    }

    [Fact]
    public void ShouldUsePowerExpandName()
    {
        Assert.Equal("PowerExpand", PowerExpandTransformation.Instance.ToString(OutputFormat.Redberry));
    }

    [Fact]
    public void ShouldSupportTransformationCollection()
    {
        TransformationCollection collection = new(PowerExpandTransformation.Instance);
        NRedberry.Tensors.Tensor tensor = Pow("a*b", "x");

        NRedberry.Tensors.Tensor actual = collection.Transform(tensor);

        AssertTensorEquals(TensorApi.Multiply(Pow("a", "x"), Pow("b", "x")), actual);
    }

    private static NRedberry.Tensors.Tensor Parse(string expression)
    {
        return TensorApi.Parse(expression);
    }

    private static NRedberry.Tensors.Tensor Pow(string baseExpression, string exponentExpression)
    {
        return Pow(Parse(baseExpression), Parse(exponentExpression));
    }

    private static NRedberry.Tensors.Tensor Pow(NRedberry.Tensors.Tensor argument, NRedberry.Tensors.Tensor exponent)
    {
        return new NRedberry.Tensors.Power(argument, exponent);
    }

    private static void AssertTensorEquals(NRedberry.Tensors.Tensor expected, NRedberry.Tensors.Tensor actual)
    {
        Assert.True(
            NRedberry.Tensors.TensorUtils.EqualsExactly(actual, expected),
            $"Expected: {expected.ToString(OutputFormat.Redberry)}; Actual: {actual.ToString(OutputFormat.Redberry)}");
    }
}

internal sealed class NamedSimpleTensorIndicator : IIndicator<NRedberry.Tensors.Tensor>
{
    private readonly int[] _names;

    public NamedSimpleTensorIndicator(params NRedberry.Tensors.SimpleTensor[] variables)
    {
        ArgumentNullException.ThrowIfNull(variables);

        _names = new int[variables.Length];
        for (int i = 0; i < variables.Length; ++i)
        {
            ArgumentNullException.ThrowIfNull(variables[i]);
            _names[i] = variables[i].Name;
        }
    }

    public bool Is(NRedberry.Tensors.Tensor @object)
    {
        if (@object is not NRedberry.Tensors.SimpleTensor simpleTensor)
        {
            return false;
        }

        for (int i = 0; i < _names.Length; ++i)
        {
            if (_names[i] == simpleTensor.Name)
            {
                return true;
            }
        }

        return false;
    }
}
