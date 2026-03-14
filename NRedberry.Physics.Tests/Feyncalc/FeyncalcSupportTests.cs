using NRedberry;
using NRedberry.Core.Utils;
using NRedberry.Graphs;
using NRedberry.Physics.Feyncalc;
using NRedberry.Tensors;
using NRedberry.Transformations;
using NRedberry.Transformations.Symmetrization;
using TensorFactory = NRedberry.Tensors.Tensors;
using Xunit;

namespace NRedberry.Physics.Tests.Feyncalc;

public sealed class TraceUtilsTests
{
    [Fact]
    public void ShouldExtractMetricAndMatrixTypesFromUnitaryMatrix()
    {
        IndexType[] types = TraceUtils.ExtractTypesFromMatrix(TensorFactory.ParseSimple("T^a'_b'a"));

        Assert.Equal(IndexType.LatinLower, types[0]);
        Assert.Equal(IndexType.Matrix1, types[1]);
    }

    [Fact]
    public void ShouldValidateUnitaryInput()
    {
        TraceUtils.CheckUnitaryInput(
            TensorFactory.ParseSimple("T^a'_b'a"),
            TensorFactory.ParseSimple("f_abc"),
            TensorFactory.ParseSimple("d_abc"),
            TensorFactory.Parse("N"));
    }

    [Fact]
    public void ShouldRejectNonMatrixShapesAndInvalidUnitaryOptions()
    {
        Assert.Throws<ArgumentException>(() => TraceUtils.ExtractTypesFromMatrix(TensorFactory.ParseSimple("T^a'_b'")));

        Assert.Throws<ArgumentException>(() => TraceUtils.CheckUnitaryInput(
            TensorFactory.ParseSimple("T^a'_b'a"),
            TensorFactory.ParseSimple("f_abc"),
            TensorFactory.ParseSimple("f_abc"),
            TensorFactory.Parse("N")));

        Assert.Throws<ArgumentException>(() => TraceUtils.CheckUnitaryInput(
            TensorFactory.ParseSimple("T^a'_b'a"),
            TensorFactory.ParseSimple("f_abc"),
            TensorFactory.ParseSimple("d_abc"),
            TensorFactory.Parse("1/2")));

        Assert.Throws<ArgumentException>(() => TraceUtils.CheckUnitaryInput(
            TensorFactory.ParseSimple("T^a'_b'a"),
            TensorFactory.ParseSimple("f_abc"),
            TensorFactory.ParseSimple("d_abc"),
            TensorFactory.Parse("N_a")));
    }
}

public sealed class UnitarySimplifyOptionsTests
{
    [Fact]
    public void ShouldInitializeUnitaryDefaultsWhenConstructed()
    {
        UnitarySimplifyOptions options = new();

        Assert.Equal("T_{A}", options.UnitaryMatrix.ToString(OutputFormat.Redberry));
        Assert.Equal("f_{ABC}", options.StructureConstant.ToString(OutputFormat.Redberry));
        Assert.Equal("d_{ABC}", options.SymmetricConstant.ToString(OutputFormat.Redberry));
        Assert.Equal("N", options.Dimension.ToString(OutputFormat.Redberry));
    }
}

public sealed class DiracOptionsSupportTests
{
    [Fact]
    public void ShouldInitializeDiracDefaultsWhenConstructed()
    {
        DiracOptions options = new();

        Assert.Equal("G_{a}", options.GammaMatrix.ToString(OutputFormat.Redberry));
        Assert.Equal("G5", options.Gamma5?.ToString(OutputFormat.Redberry));
        Assert.Equal("e_{abcd}", options.LeviCivita.ToString(OutputFormat.Redberry));
        Assert.Equal("4", options.Dimension.ToString(OutputFormat.Redberry));
        Assert.Equal("2**(4/2)", options.TraceOfOne.ToString(OutputFormat.Redberry));
        Assert.Same(Transformation.Identity, options.Simplifications);
        Assert.True(options.MinkowskiSpace);
        Assert.True(options.Cache);
        Assert.False(options.Created);
        Assert.Null(options.SimplifyLeviCivita);
        Assert.IsType<ExpandAndEliminateTransformation>(options.ExpandAndEliminate);
    }

    [Fact]
    public void ShouldCloneAndMaterializeDerivedDiracState()
    {
        DiracOptions options = new()
        {
            Dimension = TensorFactory.Parse("6"),
            Simplifications = new IdentityTransformation()
        };

        DiracOptions clone = options.Clone();
        options.TriggerCreate();

        Assert.NotSame(options, clone);
        Assert.Equal(options.Dimension, clone.Dimension);
        Assert.True(options.Created);
        Assert.Equal("2**(6/2)", options.TraceOfOne.ToString(OutputFormat.Redberry));
        Assert.IsType<ExpandAndEliminateTransformation>(options.ExpandAndEliminate);
        Assert.Same(Transformation.Identity, options.SimplifyLeviCivita);
    }
}

public sealed class SpinorsSimplifyOptionsTests
{
    [Fact]
    public void ShouldInitializeSpinorDefaultsWhenConstructed()
    {
        SpinorsSimplifyOptions options = new();

        Assert.Null(options.U);
        Assert.Null(options.V);
        Assert.Null(options.UBar);
        Assert.Null(options.VBar);
        Assert.Null(options.Momentum);
        Assert.Null(options.Mass);
        Assert.False(options.DoDiracSimplify);
        Assert.Equal("G_{a}", options.GammaMatrix.ToString(OutputFormat.Redberry));
    }

    [Fact]
    public void ShouldParseProvidedSpinorNamesMomentumAndMass()
    {
        SpinorsSimplifyOptions options = new("u", null, "cu", "cv", "p_a", "0");

        Assert.Equal("u", options.U?.ToString(OutputFormat.Redberry));
        Assert.Null(options.V);
        Assert.Equal("cu", options.UBar?.ToString(OutputFormat.Redberry));
        Assert.Equal("cv", options.VBar?.ToString(OutputFormat.Redberry));
        Assert.Equal("p_{a}", options.Momentum.ToString(OutputFormat.Redberry));
        Assert.Equal("0", options.Mass.ToString(OutputFormat.Redberry));
        Assert.False(options.DoDiracSimplify);
    }
}

public sealed class LeviCivitaSimplifyOptionsTests
{
    [Fact]
    public void ShouldInitializeLeviCivitaDefaultsWhenConstructed()
    {
        LeviCivitaSimplifyOptions options = new();
        LeviCivitaSimplifyOptions euclidean = new(false);

        Assert.Equal("e_{abcd}", options.LeviCivita.ToString(OutputFormat.Redberry));
        Assert.True(options.MinkowskiSpace);
        Assert.Same(Transformation.Identity, options.Simplifications);
        Assert.Same(Transformation.Identity, options.OverallSimplifications);
        Assert.Equal("N", options.Dimension.ToString(OutputFormat.Redberry));
        Assert.False(euclidean.MinkowskiSpace);
    }
}

public sealed class ProductOfGammasTests
{
    [Fact]
    public void ShouldReadGammaLineFromIterator()
    {
        Product product = Assert.IsType<Product>(TensorFactory.Parse("2*x*G_a^a'_b'*G5^b'_c'*G_b^c'_d'"));
        ProductOfGammas.It iterator = new(
            GetGammaName(),
            GetGamma5Name(),
            product,
            IndexType.Matrix1,
            null);

        ProductOfGammas? line = iterator.Take();
        Assert.NotNull(line);
        Tensor[] factors = line!.ToArray();

        Assert.Equal(2, line.Offset);
        Assert.Equal(3, line.Length);
        Assert.Equal(GraphType.Line, line.GraphType);
        Assert.Single(line.G5Positions);
        Assert.Equal(1, line.G5Positions[0]);
        Assert.Equal(GetGamma5Name(), Assert.IsType<SimpleTensor>(factors[line.G5Positions[0]]).Name);
        Assert.Equal(line.Length, line.ToList().Count);
        Assert.True(line.GetIndices().EqualsRegardlessOrder(line.ToProduct().Indices));
        Assert.Null(iterator.Take());
    }

    [Fact]
    public void ShouldMoveSingleGamma5ToEndOfCycle()
    {
        Product product = Assert.IsType<Product>(TensorFactory.Parse("G5^a'_b'*G_a^b'_c'*G_b^c'_a'"));
        ProductOfGammas.It iterator = new(
            GetGammaName(),
            GetGamma5Name(),
            product,
            IndexType.Matrix1,
            null);

        ProductOfGammas? cycle = iterator.Take();
        Assert.NotNull(cycle);
        Tensor[] factors = cycle!.ToArray();

        Assert.Equal(GraphType.Cycle, cycle.GraphType);
        Assert.Single(cycle.G5Positions);
        Assert.Equal(cycle.Length - 1, cycle.G5Positions[0]);
        Assert.Equal(GetGamma5Name(), Assert.IsType<SimpleTensor>(factors[^1]).Name);
        Assert.Null(iterator.Take());
    }

    [Fact]
    public void ShouldRespectGraphFilter()
    {
        Product product = Assert.IsType<Product>(TensorFactory.Parse("G_a^a'_b'*G_b^b'_c'"));
        ProductOfGammas.It iterator = new(
            GetGammaName(),
            GetGamma5Name(),
            product,
            IndexType.Matrix1,
            new GraphTypeEqualsIndicator(GraphType.Cycle));

        Assert.Null(iterator.Take());
    }

    private static int GetGammaName()
    {
        return TensorFactory.ParseSimple("G_a^a'_b'").Name;
    }

    private static int GetGamma5Name()
    {
        return TensorFactory.ParseSimple("G5^a'_b'").Name;
    }
}

public sealed class AbstractTransformationWithGammasTests
{
    [Fact]
    public void ShouldThrowConstructorUntilAbstractTransformationWithGammasIsPorted()
    {
        Assert.Throws<NotImplementedException>(() => new AbstractTransformationWithGammasProbe(null!));
    }
}

public sealed class AbstractFeynCalcTransformationTests
{
    [Fact]
    public void ShouldThrowConstructorUntilAbstractFeynCalcTransformationIsPorted()
    {
        Assert.Throws<NotImplementedException>(() => new AbstractFeynCalcTransformationProbe(null!, null));
    }
}

public sealed class AbstractTransformationWithGammasProbe(DiracOptions options)
    : AbstractTransformationWithGammas(options);

public sealed class AbstractFeynCalcTransformationProbe(DiracOptions options, ITransformation? preprocessor)
    : AbstractFeynCalcTransformation(options, preprocessor);

internal sealed class GraphTypeEqualsIndicator(GraphType expected) : IIndicator<GraphType>
{
    public bool Is(GraphType @object)
    {
        return @object == expected;
    }
}
