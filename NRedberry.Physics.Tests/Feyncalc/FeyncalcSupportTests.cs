using NRedberry.Graphs;
using NRedberry.Physics.Feyncalc;
using NRedberry.Tensors;
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
    public void ShouldThrowConstructorUntilUnitarySimplifyOptionsIsPorted()
    {
        Assert.Throws<NotImplementedException>(() => new UnitarySimplifyOptions());
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
    public void ShouldThrowConstructorsUntilLeviCivitaSimplifyOptionsIsPorted()
    {
        Assert.Throws<NotImplementedException>(() => new LeviCivitaSimplifyOptions());
        Assert.Throws<NotImplementedException>(() => new LeviCivitaSimplifyOptions(true));
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

public sealed class AbstractTransformationWithGammasProbe : AbstractTransformationWithGammas
{
    public AbstractTransformationWithGammasProbe(DiracOptions options)
        : base(options)
    {
    }
}

public sealed class AbstractFeynCalcTransformationProbe : AbstractFeynCalcTransformation
{
    public AbstractFeynCalcTransformationProbe(DiracOptions options, ITransformation? preprocessor)
        : base(options, preprocessor)
    {
    }
}
