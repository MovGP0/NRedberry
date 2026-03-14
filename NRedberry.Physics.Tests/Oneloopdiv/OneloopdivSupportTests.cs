using NRedberry.Physics.Oneloopdiv;
using NRedberry.Numbers;
using NRedberry.Tensors;
using Shouldly;
using TensorFactory = NRedberry.Tensors.Tensors;
using Xunit;

namespace NRedberry.Physics.Tests.Oneloopdiv;

public sealed class OneLoopUtilsTests
{
    [Fact]
    public void ShouldThrowWhileAntiDeSitterBackgroundUsesUnportedExpressionSubstitutions()
    {
        Should.Throw<TypeInitializationException>(OneLoopUtils.AntiDeSitterBackground);
    }

    [Fact]
    public void ShouldConfigureRiemannSymmetriesWithoutThrowing()
    {
        OneLoopUtils.SetUpRiemannSymmetries();
        SimpleTensor riemann = TensorFactory.ParseSimple("R_lmab");
        riemann.SimpleIndices.Symmetries.ShouldNotBeNull();
    }
}

public sealed class OneLoopInputTests
{
    [Fact]
    public void ShouldRejectUnsupportedOperatorOrder()
    {
        Should.Throw<ArgumentException>(() => new OneLoopInput(3, null!, null!, null!, null!, null!, null!, null!));
    }
}

public sealed class NaiveSubstitutionTests
{
    [Fact]
    public void ShouldCurrentlyDependOnUnportedIndexMappings()
    {
        NaiveSubstitution substitution = new(
            TensorFactory.Parse("a"),
            TensorFactory.Parse("b"));

        Should.Throw<NotImplementedException>(() => substitution.Transform(Complex.One));
    }
}

public sealed class BenchmarksTests
{
    [Fact]
    public void TimerShouldMeasureNonNegativeElapsedTime()
    {
        Benchmarks.Timer timer = new();
        timer.Start();

        timer.ElapsedTime().ShouldBeGreaterThanOrEqualTo(0L);
        timer.ElapsedTimeInSeconds().ShouldBeGreaterThanOrEqualTo(0L);
    }
}

public sealed class OneLoopCountertermsTests
{
    [Fact]
    public void ShouldSurfaceCurrentCalculationGap()
    {
        Should.Throw<NotImplementedException>(() =>
        {
            Expression iK = TensorFactory.ParseExpression("iK_a^b=d_a^b");
            Expression k = TensorFactory.ParseExpression("K^lm_a^b=d_a^b*g^{lm}");
            Expression s = TensorFactory.ParseExpression("S^lab=0");
            Expression w = TensorFactory.ParseExpression("W_a^b=W_a^b");
            Expression f = TensorFactory.ParseExpression("F_lmab=F_lmab");

            OneLoopInput input = new(2, iK, k, s, w, null!, null!, f);
            _ = OneLoopCounterterms.CalculateOneLoopCounterterms(input);
        });
    }
}
