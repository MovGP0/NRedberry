using System;
using System.IO;
using NRedberry.Physics.Oneloopdiv;
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
            Assert.Throws<NotImplementedException>(() => NonMinimalGravityBenchmark.Main(Array.Empty<string>()));
        }
        finally
        {
            Console.SetOut(originalOutput);
        }
    }
}
