using BenchmarkDotNet.Attributes;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;

namespace NRedberry.Benchmarks;

[MemoryDiagnoser]
[ShortRunJob]
public class ExpVectorComparisonBenchmarks
{
    private ExpVectorLong[] _leftVectors = null!;
    private ExpVectorLong[] _rightVectors = null!;
    private IComparer<ExpVector> _descendComparer = null!;

    [GlobalSetup]
    public void Setup()
    {
        _descendComparer = new TermOrder().GetDescendComparator();
        _leftVectors = new ExpVectorLong[1024];
        _rightVectors = new ExpVectorLong[1024];

        for (int i = 0; i < _leftVectors.Length; i++)
        {
            long a0 = (i * 17) % 101;
            long a1 = (i * 31) % 97;
            long a2 = (i * 43) % 89;
            long a3 = (i * 59) % 83;

            long b0 = ((i + 3) * 19) % 101;
            long b1 = ((i + 5) * 29) % 97;
            long b2 = ((i + 7) * 41) % 89;
            long b3 = ((i + 11) * 53) % 83;

            _leftVectors[i] = new ExpVectorLong([a0, a1, a2, a3]);
            _rightVectors[i] = new ExpVectorLong([b0, b1, b2, b3]);
        }
    }

    [Benchmark(OperationsPerInvoke = 10_000)]
    public int ExpVectorLongInvGradCompareTo()
    {
        int sum = 0;
        for (int i = 0; i < 10_000; i++)
        {
            int index = i & 1023;
            sum += _leftVectors[index].InvGradCompareTo(_rightVectors[index]);
        }

        return sum;
    }

    [Benchmark(OperationsPerInvoke = 10_000)]
    public int TermOrderDescendCompare()
    {
        int sum = 0;
        for (int i = 0; i < 10_000; i++)
        {
            int index = i & 1023;
            sum += _descendComparer.Compare(_leftVectors[index], _rightVectors[index]);
        }

        return sum;
    }
}
