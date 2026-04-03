using BenchmarkDotNet.Attributes;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Arith;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Ufd;
using NRedberry.Transformations.Factor;
using TensorFactory = NRedberry.Tensors.Tensors;
using TensorType = NRedberry.Tensors.Tensor;
using JasBigInteger = NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Arith.BigInteger;

namespace NRedberry.Benchmarks;

[MemoryDiagnoser]
[ShortRunJob]
public class GcdBenchmarks
{
    private const string Test1ResidualFormula =
        "2304*m**8 - 1152*s*m**6 + 288*m**4*s**2 - 1536*m**6*t + 480*m**4*s*t - 48*m**2*s**2*t + 352*m**4*t**2 - 56*m**2*s*t**2 + 2*s**2*t**2 - 32*m**2*t**3 + 2*s*t**3 + t**4";

    private GreatestCommonDivisorAbstract<ModLong> ModularGcd { get; set; } = null!;

    private GenPolynomial<ModLong> ModularPolynomial { get; set; } = null!;

    private GenPolynomial<ModLong> ModularDerivative { get; set; } = null!;

    [GlobalSetup]
    public void Setup()
    {
        TensorType test1Residual = TensorFactory.Parse(Test1ResidualFormula);
        var integerPolynomial = JasFactor.TensorToPoly(test1Residual);
        var kroneckerPolynomial = PolyUfdUtil.SubstituteKronecker(integerPolynomial, integerPolynomial.Degree() + 1);
        var integerRing = (GenPolynomialRing<JasBigInteger>)kroneckerPolynomial.Factory();
        var modularCoefficientRing = new ModLongRing(65_537L, true);
        var modularPolynomialRing = new GenPolynomialRing<ModLong>(
            modularCoefficientRing,
            integerRing.Nvar,
            integerRing.Tord,
            integerRing.GetVars());

        ModularPolynomial = PolyUtil.FromIntegerCoefficients(modularPolynomialRing, kroneckerPolynomial) ?? throw new InvalidOperationException("Failed to convert integer polynomial to modular coefficients.");
        ModularDerivative = PolyUtil.BaseDeriviative(ModularPolynomial);
        ModularGcd = GCDFactory.GetImplementation(modularCoefficientRing);
    }

    [Benchmark]
    public GenPolynomial<ModLong> ModularBaseGcd()
    {
        return ModularGcd.BaseGcd(ModularPolynomial, ModularDerivative);
    }

    [Benchmark]
    public GenPolynomial<ModLong> SparsePseudoRemainder()
    {
        return PolyUtil.BaseSparsePseudoRemainder(ModularPolynomial, ModularDerivative);
    }
}
