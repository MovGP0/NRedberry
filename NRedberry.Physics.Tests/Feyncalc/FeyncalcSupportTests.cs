using NRedberry;
using NRedberry.Core.Utils;
using NRedberry.Graphs;
using NRedberry.Physics.Feyncalc;
using Shouldly;
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

        types[0].ShouldBe(IndexType.LatinLower);
        types[1].ShouldBe(IndexType.Matrix1);
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
        Should.Throw<ArgumentException>(() => TraceUtils.ExtractTypesFromMatrix(TensorFactory.ParseSimple("T^a'_b'")));

        Should.Throw<ArgumentException>(() => TraceUtils.CheckUnitaryInput(
            TensorFactory.ParseSimple("T^a'_b'a"),
            TensorFactory.ParseSimple("f_abc"),
            TensorFactory.ParseSimple("f_abc"),
            TensorFactory.Parse("N")));

        Should.Throw<ArgumentException>(() => TraceUtils.CheckUnitaryInput(
            TensorFactory.ParseSimple("T^a'_b'a"),
            TensorFactory.ParseSimple("f_abc"),
            TensorFactory.ParseSimple("d_abc"),
            TensorFactory.Parse("1/2")));

        Should.Throw<ArgumentException>(() => TraceUtils.CheckUnitaryInput(
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

        options.UnitaryMatrix.ToString(OutputFormat.Redberry).ShouldBe("T_{A}");
        options.StructureConstant.ToString(OutputFormat.Redberry).ShouldBe("f_{ABC}");
        options.SymmetricConstant.ToString(OutputFormat.Redberry).ShouldBe("d_{ABC}");
        options.Dimension.ToString(OutputFormat.Redberry).ShouldBe("N");
    }
}

public sealed class DiracOptionsSupportTests
{
    [Fact]
    public void ShouldInitializeDiracDefaultsWhenConstructed()
    {
        DiracOptions options = new();

        options.GammaMatrix.ToString(OutputFormat.Redberry).ShouldBe("G_{a}");
        options.Gamma5?.ToString(OutputFormat.Redberry).ShouldBe("G5");
        options.LeviCivita.ToString(OutputFormat.Redberry).ShouldBe("e_{abcd}");
        options.Dimension.ToString(OutputFormat.Redberry).ShouldBe("4");
        options.TraceOfOne.ToString(OutputFormat.Redberry).ShouldBe("2**(4/2)");
        options.Simplifications.ShouldBeSameAs(Transformation.Identity);
        options.MinkowskiSpace.ShouldBeTrue();
        options.Cache.ShouldBeTrue();
        options.Created.ShouldBeFalse();
        options.SimplifyLeviCivita.ShouldBeNull();
        options.ExpandAndEliminate.ShouldBeOfType<ExpandAndEliminateTransformation>();
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

        clone.ShouldNotBeSameAs(options);
        clone.Dimension.ShouldBe(options.Dimension);
        options.Created.ShouldBeTrue();
        options.TraceOfOne.ToString(OutputFormat.Redberry).ShouldBe("2**(6/2)");
        options.ExpandAndEliminate.ShouldBeOfType<ExpandAndEliminateTransformation>();
        options.SimplifyLeviCivita.ShouldBeSameAs(Transformation.Identity);
    }
}

public sealed class SpinorsSimplifyOptionsTests
{
    [Fact]
    public void ShouldInitializeSpinorDefaultsWhenConstructed()
    {
        SpinorsSimplifyOptions options = new();

        options.U.ShouldBeNull();
        options.V.ShouldBeNull();
        options.UBar.ShouldBeNull();
        options.VBar.ShouldBeNull();
        options.Momentum.ShouldBeNull();
        options.Mass.ShouldBeNull();
        options.DoDiracSimplify.ShouldBeFalse();
        options.GammaMatrix.ToString(OutputFormat.Redberry).ShouldBe("G_{a}");
    }

    [Fact]
    public void ShouldParseProvidedSpinorNamesMomentumAndMass()
    {
        SpinorsSimplifyOptions options = new("u", null, "cu", "cv", "p_a", "0");

        options.U?.ToString(OutputFormat.Redberry).ShouldBe("u");
        options.V.ShouldBeNull();
        options.UBar?.ToString(OutputFormat.Redberry).ShouldBe("cu");
        options.VBar?.ToString(OutputFormat.Redberry).ShouldBe("cv");
        options.Momentum.ToString(OutputFormat.Redberry).ShouldBe("p_{a}");
        options.Mass.ToString(OutputFormat.Redberry).ShouldBe("0");
        options.DoDiracSimplify.ShouldBeFalse();
    }
}

public sealed class LeviCivitaSimplifyOptionsTests
{
    [Fact]
    public void ShouldInitializeLeviCivitaDefaultsWhenConstructed()
    {
        LeviCivitaSimplifyOptions options = new();
        LeviCivitaSimplifyOptions euclidean = new(false);

        options.LeviCivita.ToString(OutputFormat.Redberry).ShouldBe("e_{abcd}");
        options.MinkowskiSpace.ShouldBeTrue();
        options.Simplifications.ShouldBeSameAs(Transformation.Identity);
        options.OverallSimplifications.ShouldBeSameAs(Transformation.Identity);
        options.Dimension.ToString(OutputFormat.Redberry).ShouldBe("N");
        euclidean.MinkowskiSpace.ShouldBeFalse();
    }
}

public sealed class ProductOfGammasTests
{
    [Fact]
    public void ShouldReadGammaLineFromIterator()
    {
        Product product = TensorFactory.Parse("2*x*G_a^a'_b'*G5^b'_c'*G_b^c'_d'").ShouldBeOfType<Product>();
        ProductOfGammas.It iterator = new(
            GetGammaName(),
            GetGamma5Name(),
            product,
            IndexType.Matrix1,
            null);

        ProductOfGammas? line = iterator.Take();
        line.ShouldNotBeNull();
        Tensor[] factors = line!.ToArray();

        line.Offset.ShouldBe(2);
        line.Length.ShouldBe(3);
        line.GraphType.ShouldBe(GraphType.Line);
        line.G5Positions.ShouldHaveSingleItem();
        line.G5Positions[0].ShouldBe(1);
        factors[line.G5Positions[0]].ShouldBeOfType<SimpleTensor>().Name.ShouldBe(GetGamma5Name());
        line.ToList().Count.ShouldBe(line.Length);
        line.GetIndices().EqualsRegardlessOrder(line.ToProduct().Indices).ShouldBeTrue();
        iterator.Take().ShouldBeNull();
    }

    [Fact]
    public void ShouldMoveSingleGamma5ToEndOfCycle()
    {
        Product product = TensorFactory.Parse("G5^a'_b'*G_a^b'_c'*G_b^c'_a'").ShouldBeOfType<Product>();
        ProductOfGammas.It iterator = new(
            GetGammaName(),
            GetGamma5Name(),
            product,
            IndexType.Matrix1,
            null);

        ProductOfGammas? cycle = iterator.Take();
        cycle.ShouldNotBeNull();
        Tensor[] factors = cycle!.ToArray();

        cycle.GraphType.ShouldBe(GraphType.Cycle);
        cycle.G5Positions.ShouldHaveSingleItem();
        cycle.G5Positions[0].ShouldBe(cycle.Length - 1);
        factors[^1].ShouldBeOfType<SimpleTensor>().Name.ShouldBe(GetGamma5Name());
        iterator.Take().ShouldBeNull();
    }

    [Fact]
    public void ShouldRespectGraphFilter()
    {
        Product product = TensorFactory.Parse("G_a^a'_b'*G_b^b'_c'").ShouldBeOfType<Product>();
        ProductOfGammas.It iterator = new(
            GetGammaName(),
            GetGamma5Name(),
            product,
            IndexType.Matrix1,
            new GraphTypeEqualsIndicator(GraphType.Cycle));

        iterator.Take().ShouldBeNull();
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
        Should.Throw<NotImplementedException>(() => new AbstractTransformationWithGammasProbe(null!));
    }
}

public sealed class AbstractFeynCalcTransformationTests
{
    [Fact]
    public void ShouldThrowConstructorUntilAbstractFeynCalcTransformationIsPorted()
    {
        Should.Throw<NotImplementedException>(() => new AbstractFeynCalcTransformationProbe(null!, null));
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
