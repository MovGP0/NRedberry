using NRedberry.Physics.Oneloopdiv;
using Shouldly;
using Xunit;

namespace NRedberry.Physics.Tests.Oneloopdiv;

public sealed class NonMinimalGravityBenchmarkTests
{
    [Fact]
    public void ShouldSurfaceCurrentSpin3GhostBenchmarkGap()
    {
        TextWriter originalOutput = Console.Out;

        try
        {
            Should.Throw<NotImplementedException>(() => NonMinimalGravityBenchmark.Main([]));
        }
        finally
        {
            Console.SetOut(originalOutput);
        }
    }
}
