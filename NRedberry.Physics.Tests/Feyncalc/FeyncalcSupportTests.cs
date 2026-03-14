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
    public void ShouldThrowExtractTypesUntilTraceUtilsIsPorted()
    {
        Assert.Throws<NotImplementedException>(() => TraceUtils.ExtractTypesFromMatrix(TensorFactory.ParseSimple("T^a'_b'a")));
    }

    [Fact]
    public void ShouldThrowCheckInputUntilTraceUtilsIsPorted()
    {
        Assert.Throws<NotImplementedException>(() => TraceUtils.CheckUnitaryInput(
            TensorFactory.ParseSimple("T^a'_b'a"),
            TensorFactory.ParseSimple("f_abc"),
            TensorFactory.ParseSimple("d_abc"),
            TensorFactory.Parse("N")));
    }
}

public sealed class UnitarySimplifyOptionsTests
{
    [Fact]
    public void ShouldInitializeUnitaryDefaultsWhenConstructed()
    {
        UnitarySimplifyOptions options = new();

        Assert.True(TensorUtils.EqualsExactly(options.UnitaryMatrix, TensorFactory.ParseSimple("T_A")));
        Assert.True(TensorUtils.EqualsExactly(options.StructureConstant, TensorFactory.ParseSimple("f_ABC")));
        Assert.True(TensorUtils.EqualsExactly(options.SymmetricConstant, TensorFactory.ParseSimple("d_ABC")));
        Assert.True(TensorUtils.EqualsExactly(options.Dimension, TensorFactory.Parse("N")));
    }
}

public sealed class SpinorsSimplifyOptionsTests
{
    [Fact]
    public void ShouldThrowConstructorsUntilSpinorsSimplifyOptionsIsPorted()
    {
        Assert.Throws<NotImplementedException>(() => new SpinorsSimplifyOptions());
        Assert.Throws<NotImplementedException>(() => new SpinorsSimplifyOptions("u", "v", "ub", "vb", "p_a", "m"));
    }
}

public sealed class LeviCivitaSimplifyOptionsTests
{
    [Fact]
    public void ShouldInitializeLeviCivitaDefaultsWhenConstructed()
    {
        LeviCivitaSimplifyOptions options = new();
        LeviCivitaSimplifyOptions euclidean = new(false);

        Assert.True(TensorUtils.EqualsExactly(options.LeviCivita, TensorFactory.ParseSimple("e_abcd")));
        Assert.True(options.MinkowskiSpace);
        Assert.Same(Transformation.Identity, options.Simplifications);
        Assert.Same(Transformation.Identity, options.OverallSimplifications);
        Assert.True(TensorUtils.EqualsExactly(options.Dimension, TensorFactory.Parse("N")));
        Assert.False(euclidean.MinkowskiSpace);
    }
}

public sealed class ProductOfGammasTests
{
    [Fact]
    public void ShouldThrowConstructorsUntilProductOfGammasIsPorted()
    {
        Product product = (Product)TensorFactory.Parse("G_a*G_b");
        Assert.Throws<NotImplementedException>(() => new ProductOfGammas(
            0,
            product.Content,
            [],
            [],
            GraphType.Cycle));
        Assert.Throws<NotImplementedException>(() => new ProductOfGammas.It(
            1,
            2,
            product,
            NRedberry.IndexType.Matrix1,
            null));
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
