using BenchmarkDotNet.Attributes;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Ufd;
using NRedberry.Transformations.Factor;
using TensorFactory = NRedberry.Tensors.Tensors;
using TensorType = NRedberry.Tensors.Tensor;
using JasBigInteger = NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Arith.BigInteger;

namespace NRedberry.Benchmarks;

[MemoryDiagnoser]
[ShortRunJob]
public class FactorEngineBenchmarks
{
    private const string ResidualFormula =
        "2304*m**8 - 1152*s*m**6 + 288*m**4*s**2 - 1536*m**6*t + 480*m**4*s*t - 48*m**2*s**2*t + 352*m**4*t**2 - 56*m**2*s*t**2 + 2*s**2*t**2 - 32*m**2*t**3 + 2*s*t**3 + t**4";

    private GenPolynomial<JasBigInteger> IntegerPolynomial { get; set; } = null!;

    private FactorAbstract<JasBigInteger> FactorEngine { get; set; } = null!;

    [GlobalSetup]
    public void Setup()
    {
        TensorType test1Residual = TensorFactory.Parse(ResidualFormula);
        IntegerPolynomial = JasFactor.TensorToPoly(test1Residual);
        FactorEngine = FactorFactory.GetImplementation(JasBigInteger.One);
    }

    [Benchmark]
    public SortedDictionary<GenPolynomial<JasBigInteger>, long> FactorResidualPolynomial()
    {
        return FactorEngine.Factors(IntegerPolynomial);
    }
}
